using Content.Shared._Offbrand.Organs;
using Content.Shared.Body;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;

namespace Content.Shared._Offbrand.Medical;

public sealed partial class PenLightSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PenLightComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<PenLightComponent, PenLightDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(Entity<PenLightComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target is not { } target)
            return;

        if (!HasComp<BodyComponent>(target))
            return;

        args.Handled = true;
        _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, ent.Comp.DoAfterDuration,
            new PenLightDoAfterEvent(), ent, target: target, used: ent)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
        });
    }

    private void OnDoAfter(Entity<PenLightComponent> ent, ref PenLightDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Target is not { } target)
            return;

        args.Handled = true;

        // Check brain and eye health for pupil response
        var brainDamage = 0f;
        var eyeDamage = 0f;

        if (TryComp<BodyComponent>(target, out var body) && body.Organs != null)
        {
            foreach (var organ in body.Organs.ContainedEntities)
            {
                if (!TryComp<OrganComponent>(organ, out var organComp))
                    continue;

                var categoryStr = organComp.Category?.ToString();

                if (categoryStr == "Brain" && TryComp<DamageableOrganComponent>(organ, out var brainDmg))
                {
                    brainDamage = brainDmg.Damage.Float() / Math.Max(brainDmg.MaxDamage.Float(), 1f);
                }

                if (categoryStr == "Eyes" && TryComp<DamageableOrganComponent>(organ, out var eyeDmg))
                {
                    eyeDamage = eyeDmg.Damage.Float() / Math.Max(eyeDmg.MaxDamage.Float(), 1f);
                }
            }
        }

        var patient = Identity.Entity(target, EntityManager);
        LocId message;

        if (brainDamage >= 1f)
            message = "penlight-pupils-unresponsive";
        else if (brainDamage >= 0.6f)
            message = "penlight-pupils-sluggish";
        else if (eyeDamage >= 0.8f)
            message = "penlight-pupils-sluggish";
        else if (brainDamage > 0f)
            message = "penlight-pupils-sluggish";
        else
            message = "penlight-pupils-normal";

        _popup.PopupEntity(Loc.GetString(message, ("patient", patient)), target, args.User);
    }
}

public sealed partial class PenLightDoAfterEvent : SimpleDoAfterEvent;
