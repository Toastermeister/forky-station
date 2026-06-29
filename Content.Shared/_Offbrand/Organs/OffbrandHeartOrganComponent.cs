using Content.Shared.Body;
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared._Offbrand.Organs;

/// <summary>
/// Simplified heart organ. Binary beating/stopped state driven by organ health.
/// The heart beats as long as organ damage is below MaxDamage.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(OffbrandHeartOrganSystem))]
public sealed partial class OffbrandHeartOrganComponent : Component
{
    /// <summary>
    /// Whether the heart is currently beating.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Beating = true;

    /// <summary>
    /// The stethoscope sound is considered to be damaged above this damage threshold.
    /// </summary>
    [DataField(required: true)]
    public FixedPoint2 StethoscopeDamagedAbove;
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(OffbrandHeartOrganSystem))]
public sealed partial class HeartDefibrillatableComponent : Component
{
    [DataField]
    public LocId TargetIsDead = "heart-defibrillatable-target-is-dead";

    /// <summary>
    /// Whether the entity's heart is currently beating.
    /// Updated by OffbrandHeartOrganSystem when heart starts/stops.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool HeartBeating = true;
}

/// <summary>
/// Raised on an entity if the heart has stopped beating
/// </summary>
[ByRefEvent]
public record struct HeartStoppedEvent;

/// <summary>
/// Raised on an entity if the heart has started beating
/// </summary>
[ByRefEvent]
public record struct HeartStartedEvent;

/// <summary>
/// Raised on an entity to see what the defibrillator will say before defibrillation
/// </summary>
[ByRefEvent]
public record struct BeforeTargetDefibrillatedEvent(List<LocId> Messages);
