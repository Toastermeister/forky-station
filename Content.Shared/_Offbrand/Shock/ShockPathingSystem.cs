using Content.Shared._Offbrand.Organs;
using Content.Shared.Body;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Damage.Systems;
using Content.Shared.FixedPoint;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared._Offbrand.Shock;

/// <summary>
/// When Shock damage is applied to a body with <see cref="ShockPathingComponent"/>,
/// traces the electrical path through connecting organs and applies secondary damage.
/// </summary>
public sealed partial class ShockPathingSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;

    private static readonly ProtoId<DamageTypePrototype> ShockDamageType = "Shock";

    private static readonly Dictionary<string, string> ShockPathUpstream = new()
    {
        ["Head"] = "Torso",
        ["Torso"] = "Torso",
        ["ArmLeft"] = "Torso",
        ["ArmRight"] = "Torso",
        ["HandLeft"] = "ArmLeft",
        ["HandRight"] = "ArmRight",
        ["LegLeft"] = "Torso",
        ["LegRight"] = "Torso",
        ["FootLeft"] = "LegLeft",
        ["FootRight"] = "LegRight",
    };

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShockPathingComponent, DamageDealtEvent>(OnDamageDealt);
    }

    private void OnDamageDealt(Entity<ShockPathingComponent> ent, ref DamageDealtEvent args)
    {
        if (_timing.ApplyingState)
            return;

        var delta = args.Damage;
        if (!delta.DamageDict.TryGetValue(ShockDamageType, out var shockAmount) || shockAmount <= FixedPoint2.Zero)
            return;

        var entryOrgan = GetEntryOrgan(ent);
        if (entryOrgan == null)
            return;

        var path = TraceShockPath(entryOrgan.Value);
        if (path.Count == 0)
            return;

        var secondaryDamage = new DamageSpecifier();
        secondaryDamage.DamageDict[ShockDamageType] = shockAmount * ent.Comp.PathDamageFraction;

        foreach (var organInPath in path)
        {
            _damageable.TryChangeDamage(organInPath, secondaryDamage, ignoreResistances: false, interruptsDoAfters: false);
        }
    }

    private EntityUid? GetEntryOrgan(Entity<ShockPathingComponent> ent)
    {
        if (!TryComp<BodyComponent>(ent, out var body) || body.Organs == null)
            return null;

        var seed = SharedRandomExtensions.HashCodeCombine((int)_timing.CurTick.Value, GetNetEntity(ent).Id);
        var rand = new System.Random(seed);

        var extremities = new List<EntityUid>();
        var limbs = new List<EntityUid>();
        var torsos = new List<EntityUid>();

        foreach (var organ in body.Organs.ContainedEntities)
        {
            if (!TryComp<OrganComponent>(organ, out var organComp))
                continue;

            var cat = organComp.Category?.ToString();
            if (cat == "HandLeft" || cat == "HandRight" || cat == "FootLeft" || cat == "FootRight" || cat == "Head")
                extremities.Add(organ);
            else if (cat == "ArmLeft" || cat == "ArmRight" || cat == "LegLeft" || cat == "LegRight")
                limbs.Add(organ);
            else if (cat == "Torso")
                torsos.Add(organ);
        }

        var roll = rand.NextDouble();
        if (roll < 0.6 && extremities.Count > 0)
            return extremities[rand.Next(extremities.Count)];
        if (roll < 0.9 && limbs.Count > 0)
            return limbs[rand.Next(limbs.Count)];
        if (torsos.Count > 0)
            return torsos[rand.Next(torsos.Count)];

        return null;
    }

    private List<EntityUid> TraceShockPath(EntityUid entryOrganEnt)
    {
        var path = new List<EntityUid>();

        if (!TryComp<OrganComponent>(entryOrganEnt, out var startOrgComp))
            return path;

        var visited = new HashSet<string>();
        var currentEnt = entryOrganEnt;
        var currentCat = startOrgComp.Category?.ToString();

        while (currentCat != null && visited.Add(currentCat))
        {
            path.Add(currentEnt);

            if (currentCat == "Torso")
                break;

            if (!ShockPathUpstream.TryGetValue(currentCat, out var nextCat) || nextCat == currentCat)
                break;

            // Find the next organ upstream
            if (startOrgComp.Body is not { } body || !TryComp<BodyComponent>(body, out var bodyComp) || bodyComp.Organs == null)
                break;

            EntityUid? nextEnt = null;
            foreach (var organ in bodyComp.Organs.ContainedEntities)
            {
                if (!TryComp<OrganComponent>(organ, out var oc))
                    continue;

                if (oc.Category?.ToString() == nextCat)
                {
                    nextEnt = organ;
                    break;
                }
            }

            if (nextEnt == null)
                break;

            currentEnt = nextEnt.Value;
            currentCat = nextCat;

            if (!TryComp<OrganComponent>(currentEnt, out var nextOrgComp))
                break;

            startOrgComp = nextOrgComp;
        }

        return path;
    }
}
