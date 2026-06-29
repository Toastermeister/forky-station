using Content.Shared._Offbrand.Organs;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Body;

/// <summary>
/// Prototype defining organ conditions
/// </summary>
[Prototype]
public sealed partial class OrganConditionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(BodySystem), typeof(DamageableOrganSystem))]
public sealed partial class OrganComponent : Component
{
    /// <summary>
    /// The body entity containing this organ, if any
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Body;

    /// <summary>
    /// What kind of organ is this, if any
    /// </summary>
    [DataField]
    public ProtoId<OrganCategoryPrototype>? Category;

    /// <summary>
    /// Maximum damage this organ can sustain before failing completely.
    /// </summary>
    [DataField]
    public FixedPoint2 MaxDamage;

    /// <summary>
    /// Current damage sustained by this organ.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint2 Damage;

    /// <summary>
    /// Active condition markers on this organ (e.g. Necrotic, Infected, Splinted).
    /// </summary>
    [DataField]
    public List<ProtoId<OrganConditionPrototype>> Conditions = new();

    /// <summary>
    /// Efficiency derived from current vs max damage. 1.0 = healthy, 0.0 = fully damaged.
    /// </summary>
    public float Efficiency => MaxDamage > FixedPoint2.Zero
        ? MathF.Max(0f, 1f - Damage.Float() / MaxDamage.Float())
        : 1f;
}
