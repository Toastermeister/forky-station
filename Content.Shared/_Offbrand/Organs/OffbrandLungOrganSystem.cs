using Content.Shared._Offbrand.Medical;
using Content.Shared._Offbrand.Wounds;
using Content.Shared.Body;
using Content.Shared.FixedPoint;

namespace Content.Shared._Offbrand.Organs;

public sealed partial class OffbrandLungOrganSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OffbrandLungOrganComponent, BodyRelayedEvent<BeforeBreathEvent>>(OnBeforeBreath);
        SubscribeLocalEvent<OffbrandLungOrganComponent, BodyRelayedEvent<BaseLungFunctionEvent>>(OnBaseLungFunction);
        SubscribeLocalEvent<OffbrandLungOrganComponent, StethoscopeExamineEvent>(OnStethoscopeExamine);
    }

    private float BreathVolumeModifier(Entity<OffbrandLungOrganComponent> ent)
    {
        var damage = Comp<DamageableOrganComponent>(ent);
        return 1f - MathF.Pow(damage.Damage.Float() / damage.MaxDamage.Float(), 3f);
    }

    private void OnBeforeBreath(Entity<OffbrandLungOrganComponent> ent, ref BodyRelayedEvent<BeforeBreathEvent> args)
    {
        args.Args = args.Args with { BreathVolume = args.Args.BreathVolume * BreathVolumeModifier(ent) };
    }

    private void OnBaseLungFunction(Entity<OffbrandLungOrganComponent> ent, ref BodyRelayedEvent<BaseLungFunctionEvent> args)
    {
        var damageComp = Comp<DamageableOrganComponent>(ent);
        var damage = damageComp.Damage.Float() / damageComp.MaxDamage.Float();
        var health = 1f - damage;

        args.Args = args.Args with { Function = health };
    }

    private void OnStethoscopeExamine(Entity<OffbrandLungOrganComponent> ent, ref StethoscopeExamineEvent args)
    {
        var damage = Comp<DamageableOrganComponent>(ent);

        if (ent.Comp.StethoscopeDepthDescriptions.HighestMatch(1f - BreathVolumeModifier(ent)) is not { } volume)
            return;

        if (ent.Comp.StethoscopeRegularityDescriptions.HighestMatch(damage.Damage) is not { } regularity)
            return;

        var speed = damage.Damage >= ent.Comp.AsphyxiationThreshold
            ? OffbrandLungBreathingSpeed.Fastest
            : OffbrandLungBreathingSpeed.Normal;

        var message = Loc.GetString(ent.Comp.StethoscopeDescription,
            ("volume", volume),
            ("regularity", regularity),
            ("speed", speed));

        args.Messages.Add(message);
    }
}
