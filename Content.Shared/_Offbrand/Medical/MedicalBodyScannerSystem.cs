using Content.Shared._Offbrand.Organs;
using Content.Shared._Offbrand.Wounds;
using Content.Shared.Body;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.StatusEffectNew;

namespace Content.Shared._Offbrand.Medical;

public sealed partial class MedicalBodyScannerSystem : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;
    [Dependency] private readonly BodySystem _body = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MedicalBodyScannerComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<MedicalBodyScannerComponent, MedicalBodyScanDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(Entity<MedicalBodyScannerComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target is not { } target)
            return;

        if (!HasComp<BodyComponent>(target))
            return;

        args.Handled = true;
        _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, ent.Comp.ScanDuration,
            new MedicalBodyScanDoAfterEvent(), ent, target: target, used: ent)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
        });
    }

    private void OnDoAfter(Entity<MedicalBodyScannerComponent> ent, ref MedicalBodyScanDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Target is not { } target)
            return;

        args.Handled = true;
        var patient = Identity.Entity(target, EntityManager);

        // Count wounds per body part
        var woundCount = 0;
        var fractureCount = 0;
        var bleedCount = 0;
        var organDamageCount = 0;

        if (TryComp<BodyComponent>(target, out var body) && body.Organs != null)
        {
            foreach (var organ in body.Organs.ContainedEntities)
            {
                if (!HasComp<OrganComponent>(organ))
                    continue;

                var organWounds = 0;
                if (_statusEffects.TryEffectsWithComp<WoundComponent>(organ, out var wounds))
                {
                    organWounds = wounds.Count;
                    woundCount += organWounds;

                    foreach (var wound in wounds)
                    {
                        if (HasComp<BleedingWoundComponent>(wound))
                            bleedCount++;
                    }
                }

                if (organWounds > 0 || (TryComp<DamageableOrganComponent>(organ, out var dmg) && dmg.Damage > 0))
                    organDamageCount++;
            }
        }

        // Check for fractures via body-level effects
        if (_statusEffects.TryEffectsWithComp<WoundComponent>(target, out var bodyWounds))
        {
            foreach (var wound in bodyWounds)
            {
                if (HasComp<WoundComponent>(wound) && Comp<WoundComponent>(wound).MaximumDamage >= 5)
                    fractureCount++;
            }
        }

        var result = Loc.GetString("medical-scanner-result",
            ("patient", patient),
            ("wounds", woundCount),
            ("fractures", fractureCount),
            ("bleeding", bleedCount),
            ("organs", organDamageCount));

        _popup.PopupEntity(result, target, args.User, PopupType.Large);
    }
}
