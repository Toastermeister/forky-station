using Content.Shared._Offbrand.Organs;
using Content.Shared._Offbrand.Wounds;
using Content.Shared.FixedPoint;
using Content.Shared.StatusEffectNew.Components;
using Content.Shared.StatusEffectNew;

namespace Content.Shared._Offbrand.StatusEffects;

public sealed partial class ModifyOrganOxygenDepletionChanceStatusEffectSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ModifyOrganOxygenDepletionChanceStatusEffectComponent, StatusEffectRelayedEvent<BeforeDepleteOrganOxygen>>(OnBeforeDepleteOrganOxygen);
    }

    private void OnBeforeDepleteOrganOxygen(Entity<ModifyOrganOxygenDepletionChanceStatusEffectComponent> ent, ref StatusEffectRelayedEvent<BeforeDepleteOrganOxygen> args)
    {
        if (Comp<StatusEffectComponent>(ent).AppliedTo is not { } target)
            return;

        var oxygenation = TryComp<HeartDefibrillatableComponent>(target, out var heart) && heart.HeartBeating
            ? FixedPoint2.New(1)
            : FixedPoint2.Zero;

        if (ent.Comp.OxygenationModifierThresholds.LowestMatch(oxygenation) is not { } modifier)
            return;

        args.Args = args.Args with { Chance = args.Args.Chance * modifier };
    }
}
