using Content.Shared.Body;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction.Events;
using Content.Shared.Rejuvenate;
using Robust.Shared.Timing;

namespace Content.Shared._Offbrand.Organs;

public sealed partial class DamageableOrganSystem : EntitySystem
{
    [Dependency] private IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageableOrganComponent, BodyRelayedEvent<SuicideEvent>>(OnSuicide);
        SubscribeLocalEvent<DamageableOrganComponent, BodyRelayedEvent<RejuvenateEvent>>(OnRejuvenate);
    }

    private void OnRejuvenate(Entity<DamageableOrganComponent> ent, ref BodyRelayedEvent<RejuvenateEvent> args)
    {
        ChangeDamage(ent.AsNullable(), -ent.Comp.Damage);
    }

    private void OnSuicide(Entity<DamageableOrganComponent> ent, ref BodyRelayedEvent<SuicideEvent> args)
    {
        ChangeDamage(ent.AsNullable(), ent.Comp.MaxDamage - ent.Comp.Damage);
    }

    /// <summary>
    /// Changes the damage to an organ, syncing both <see cref="DamageableOrganComponent"/>
    /// and the base <see cref="OrganComponent"/> health fields.
    /// </summary>
    /// <param name="organ">The organ to change the damage on.</param>
    /// <param name="amount">The delta to change by.</param>
    /// <seealso cref="OrganDamageChangedEvent" />
    /// <returns>The actual damage delta.</returns>
    public FixedPoint2 ChangeDamage(Entity<DamageableOrganComponent?> organ, FixedPoint2 amount)
    {
        if (!Resolve(organ, ref organ.Comp))
            return FixedPoint2.Zero;

        var oldDamage = organ.Comp.Damage;
        organ.Comp.Damage = FixedPoint2.Clamp(organ.Comp.Damage + amount, FixedPoint2.Zero, organ.Comp.MaxDamage);
        Dirty(organ);

        // Sync base OrganComponent health fields
        if (TryComp<OrganComponent>(organ, out var baseOrgan))
        {
            baseOrgan.Damage = organ.Comp.Damage;
            baseOrgan.MaxDamage = organ.Comp.MaxDamage;
            Dirty(organ, baseOrgan);
        }

        if (oldDamage != organ.Comp.Damage)
        {
            var evt = new OrganDamageChangedEvent((organ, organ.Comp));
            RaiseLocalEvent(organ, ref evt);
        }

        return organ.Comp.Damage - oldDamage;
    }

    /// <summary>
    /// Returns the efficiency (0.0 to 1.0) of an organ based on its current damage.
    /// 1.0 = healthy, 0.0 = fully damaged/failed.
    /// </summary>
    public float GetEfficiency(EntityUid uid)
    {
        if (TryComp<OrganComponent>(uid, out var organ) && organ.MaxDamage > FixedPoint2.Zero)
            return organ.Efficiency;

        if (TryComp<DamageableOrganComponent>(uid, out var dmg) && dmg.MaxDamage > FixedPoint2.Zero)
            return MathF.Max(0f, 1f - dmg.Damage.Float() / dmg.MaxDamage.Float());

        return 1f;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DamageableOrganComponent, PassiveOrganDamageComponent>();
        while (query.MoveNext(out var uid, out var organ, out var passive))
        {
            if (_timing.CurTime < passive.NextUpdate)
                continue;

            passive.NextUpdate = _timing.CurTime + passive.UpdateInterval;
            Dirty(uid, passive);

            if (organ.Damage > passive.DamageCap)
                continue;

            ChangeDamage((uid, organ), passive.Damage);
        }
    }
}
