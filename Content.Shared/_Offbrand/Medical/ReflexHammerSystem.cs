using Content.Shared._Offbrand.Organs;
using Content.Shared._Offbrand.Wounds;
using Content.Shared.Body;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Serialization;

namespace Content.Shared._Offbrand.Medical;

public sealed partial class ReflexHammerSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ReflexHammerComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<ReflexHammerComponent, ReflexHammerDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(Entity<ReflexHammerComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target is not { } target)
            return;

        if (!HasComp<BodyComponent>(target))
            return;

        args.Handled = true;
        _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, ent.Comp.DoAfterDuration,
            new ReflexHammerDoAfterEvent(), ent, target: target, used: ent)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
        });
    }

    private void OnDoAfter(Entity<ReflexHammerComponent> ent, ref ReflexHammerDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Target is not { } target)
            return;

        args.Handled = true;

        // Check limb organs for significant damage affecting reflexes
        var reflexesAffected = false;
        if (TryComp<BodyComponent>(target, out var body) && body.Organs != null)
        {
            foreach (var organ in body.Organs.ContainedEntities)
            {
                if (!HasComp<OrganComponent>(organ) || !HasComp<WoundableOrganComponent>(organ))
                    continue;

                // Check for active wounds on this limb
                if (_statusEffects.TryEffectsWithComp<WoundComponent>(organ, out var wounds) && wounds.Count > 0)
                {
                    reflexesAffected = true;
                    break;
                }
            }
        }

        var patient = Identity.Entity(target, EntityManager);
        var message = reflexesAffected
            ? Loc.GetString("reflex-hammer-no-reflex", ("patient", patient))
            : Loc.GetString("reflex-hammer-normal", ("patient", patient));

        _popup.PopupEntity(message, target, args.User);
    }
}

[Serializable, NetSerializable]
public sealed partial class ReflexHammerDoAfterEvent : SimpleDoAfterEvent;
