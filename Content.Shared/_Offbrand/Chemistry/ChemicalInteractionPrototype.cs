using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared._Offbrand.Chemistry;

/// <summary>
/// Defines an interaction between two reagents during metabolism.
/// </summary>
[Prototype]
public sealed partial class ChemicalInteractionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The first interacting reagent.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<ReagentPrototype> ReagentA;

    /// <summary>
    /// The second interacting reagent.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<ReagentPrototype> ReagentB;

    /// <summary>
    /// The type of interaction.
    /// </summary>
    [DataField(required: true)]
    public ChemicalInteractionType InteractionType = ChemicalInteractionType.Antagonism;

    /// <summary>
    /// Effect multiplier for the interaction. Applied to effect scale.
    /// Synergy: >1.0 (boost effect). Antagonism: <1.0 (reduce effect).
    /// </summary>
    [DataField]
    public float EffectModifier = 1f;

    /// <summary>
    /// If set, spawns this reagent as a byproduct during metabolism.
    /// </summary>
    [DataField]
    public ProtoId<ReagentPrototype>? Byproduct;

    /// <summary>
    /// Amount of byproduct to spawn per processing tick.
    /// </summary>
    [DataField]
    public float ByproductAmount = 0.1f;
}

public enum ChemicalInteractionType
{
    /// <summary>
    /// Reagents amplify each other's effects.
    /// </summary>
    Synergy,

    /// <summary>
    /// Reagents reduce or cancel each other's effects.
    /// </summary>
    Antagonism,

    /// <summary>
    /// Reagents combine to create a toxic byproduct.
    /// </summary>
    ToxicByproduct,
}
