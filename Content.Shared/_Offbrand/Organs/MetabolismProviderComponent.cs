using Content.Shared.Chemistry.Reagent;
using Content.Shared.Metabolism;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Offbrand.Organs;

/// <summary>
/// Marks an organ as a provider of metabolism data for a body.
/// Added to liver/kidney organs to define drug sensitivity, metabolic rate, etc.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class MetabolismProviderComponent : Component
{
    /// <summary>
    /// The metabolizer type identifier (e.g. "Human", "Arachnid", "Vox", "Plant").
    /// Must match MetabolizerType prototype IDs for reagent effect filtering.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<MetabolizerTypePrototype> MetabolizerType = "Human";

    /// <summary>
    /// Global metabolic rate multiplier for this species.
    /// Higher = faster drug processing.
    /// </summary>
    [DataField]
    public float MetabolicRateMultiplier = 1f;

    /// <summary>
    /// Reagent IDs that are particularly toxic to this species.
    /// </summary>
    [DataField]
    public List<ProtoId<ReagentPrototype>> ToxicReagents = new();

    /// <summary>
    /// Reagent IDs that are particularly potent/healing for this species.
    /// </summary>
    [DataField]
    public List<ProtoId<ReagentPrototype>> BeneficialReagents = new();
}
