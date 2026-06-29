using Content.Shared.Chemistry.Reagent;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Offbrand.Organs;

/// <summary>
/// Marks an organ as a provider of bloodstream/hematology data for a body.
/// Added to heart-like organs to define blood type, color, and chemistry.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class BloodstreamProviderComponent : Component
{
    /// <summary>
    /// The blood type identifier for this provider (e.g. "Human-O+", "Arachnid-Hα+").
    /// </summary>
    [DataField(required: true)]
    public string BloodType = "Unknown";

    /// <summary>
    /// The prototype ID of the reagent that makes up this species' blood.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<ReagentPrototype> BloodReagent = "Blood";

    /// <summary>
    /// Maximum blood volume.
    /// </summary>
    [DataField]
    public float MaxBloodVolume = 300f;

    /// <summary>
    /// Whether this blood type can receive transfusions from the universal donor type.
    /// </summary>
    [DataField]
    public bool AcceptsUniversalDonor = true;

    /// <summary>
    /// List of incompatible blood types that cause transfusion reactions.
    /// </summary>
    [DataField]
    public List<string> IncompatibleWith = new();
}
