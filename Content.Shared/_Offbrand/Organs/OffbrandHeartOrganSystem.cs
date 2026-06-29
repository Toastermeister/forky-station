using Content.Shared._Offbrand.Medical;
using Content.Shared._Offbrand.Wounds;
using Content.Shared.Body;
using Content.Shared.Medical;
using Content.Shared.Rejuvenate;
using Content.Shared.StatusEffectNew;

namespace Content.Shared._Offbrand.Organs;

/// <summary>
/// Simplified heart system. Heart state is binary (beating/stopped).
/// The heart beats as long as organ damage is below max. It stops at max damage.
/// </summary>
public sealed partial class OffbrandHeartOrganSystem : EntitySystem
{
    [Dependency] private DamageableOrganSystem _damageable = default!;
    [Dependency] private StatusEffectsSystem _statusEffects = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OffbrandHeartOrganComponent, OrganGotInsertedEvent>(OnOrganGotInserted);
        SubscribeLocalEvent<OffbrandHeartOrganComponent, OrganGotRemovedEvent>(OnOrganGotRemoved);
        SubscribeLocalEvent<OffbrandHeartOrganComponent, OrganDamageChangedEvent>(OnOrganDamageChanged);
        SubscribeLocalEvent<OffbrandHeartOrganComponent, BodyRelayedEvent<RejuvenateEvent>>(OnRejuvenate);
        SubscribeLocalEvent<OffbrandHeartOrganComponent, StethoscopeExamineEvent>(OnStethoscopeExamine);
        SubscribeLocalEvent<HeartDefibrillatableComponent, BodyRelayedEvent<TargetDefibrillatedEvent>>(OnTargetDefibrillated);
    }

    private void OnStethoscopeExamine(Entity<OffbrandHeartOrganComponent> ent, ref StethoscopeExamineEvent args)
    {
        if (!ent.Comp.Beating)
            return;

        var damage = Comp<DamageableOrganComponent>(ent);

        var message = damage.Damage >= ent.Comp.StethoscopeDamagedAbove
            ? "heart-stethoscope-damaged"
            : "heart-stethoscope-healthy";

        args.Messages.Add(Loc.GetString(message));
    }

    private void OnOrganGotInserted(Entity<OffbrandHeartOrganComponent> ent, ref OrganGotInsertedEvent args)
    {
        if (ent.Comp.Beating)
        {
            var evt = new HeartStartedEvent();
            RaiseLocalEvent(args.Target, ref evt);
        }
        else
        {
            var stoppedEvt = new HeartStoppedEvent();
            RaiseLocalEvent(args.Target, ref stoppedEvt);
        }
    }

    private void OnOrganGotRemoved(Entity<OffbrandHeartOrganComponent> ent, ref OrganGotRemovedEvent args)
    {
        var stoppedEvt = new HeartStoppedEvent();
        RaiseLocalEvent(args.Target, ref stoppedEvt);
    }

    private void OnOrganDamageChanged(Entity<OffbrandHeartOrganComponent> ent, ref OrganDamageChangedEvent args)
    {
        if (!ent.Comp.Beating)
            return;

        if (args.Organ.Comp.Damage >= args.Organ.Comp.MaxDamage)
            StopHeart(ent);
    }

    private void OnRejuvenate(Entity<OffbrandHeartOrganComponent> ent, ref BodyRelayedEvent<RejuvenateEvent> args)
    {
        if (ent.Comp.Beating)
            return;

        StartHeart(ent);
    }

    private void StopHeart(Entity<OffbrandHeartOrganComponent> ent)
    {
        ent.Comp.Beating = false;
        Dirty(ent);

        if (Comp<OrganComponent>(ent).Body is not { } body)
            return;

        if (TryComp<HeartDefibrillatableComponent>(body, out var defib))
        {
            defib.HeartBeating = false;
            Dirty(body, defib);
        }

        var stoppedEvt = new HeartStoppedEvent();
        RaiseLocalEvent(body, ref stoppedEvt);
    }

    private void TryRestartHeart(Entity<OffbrandHeartOrganComponent?, DamageableOrganComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp1, ref ent.Comp2, false))
            return;

        if (ent.Comp2.MaxDamage <= ent.Comp2.Damage || ent.Comp1.Beating)
            return;

        StartHeart((ent, ent.Comp1));
    }

    private void StartHeart(Entity<OffbrandHeartOrganComponent> ent)
    {
        ent.Comp.Beating = true;
        Dirty(ent);

        if (Comp<OrganComponent>(ent).Body is not { } body)
            return;

        if (TryComp<HeartDefibrillatableComponent>(body, out var defib))
        {
            defib.HeartBeating = true;
            Dirty(body, defib);
        }

        var evt = new HeartStartedEvent();
        RaiseLocalEvent(body, ref evt);
    }

    private void OnTargetDefibrillated(Entity<HeartDefibrillatableComponent> ent, ref BodyRelayedEvent<TargetDefibrillatedEvent> args)
    {
        TryRestartHeart(ent.Owner);
    }
}
