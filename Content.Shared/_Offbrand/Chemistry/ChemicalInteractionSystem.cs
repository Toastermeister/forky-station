using System.Linq;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared._Offbrand.Chemistry;

/// <summary>
/// Handles chemical interactions during metabolism (synergy, antagonism, toxic byproducts).
/// Called from <see cref="MetabolizerSystem"/> during reagent processing.
/// </summary>
public sealed partial class ChemicalInteractionSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    private List<ChemicalInteractionPrototype> _interactions = new();

    public override void Initialize()
    {
        base.Initialize();
        _interactions = _prototype.EnumeratePrototypes<ChemicalInteractionPrototype>().ToList();
    }

    /// <summary>
    /// Checks if the given reagent has active interactions in the solution.
    /// Returns an effect multiplier (default 1.0 = no interaction).
    /// </summary>
    /// <param name="reagentId">The reagent being metabolized.</param>
    /// <param name="solution">The solution containing all reagents.</param>
    /// <param name="bodyEntity">The body entity for byproduct spawning.</param>
    /// <param name="bodySolution">The body's blood solution for byproduct spawning.</param>
    /// <returns>Effect multiplier from interactions.</returns>
    public float GetInteractionModifier(
        ProtoId<ReagentPrototype> reagentId,
        Solution solution,
        EntityUid bodyEntity,
        Entity<SolutionComponent>? bodySolution)
    {
        var modifier = 1f;

        foreach (var interaction in _interactions)
        {
            if (interaction.ReagentA != reagentId && interaction.ReagentB != reagentId)
                continue;

            var partnerReagent = interaction.ReagentA == reagentId
                ? interaction.ReagentB
                : interaction.ReagentA;

            // Check if the partner reagent is present in the solution
            if (solution.GetTotalPrototypeQuantity(partnerReagent) <= FixedPoint2.Zero)
                continue;

            switch (interaction.InteractionType)
            {
                case ChemicalInteractionType.Synergy:
                    modifier *= interaction.EffectModifier;
                    break;

                case ChemicalInteractionType.Antagonism:
                    modifier *= interaction.EffectModifier;
                    break;

                case ChemicalInteractionType.ToxicByproduct:
                    if (interaction.Byproduct != null && bodySolution != null)
                    {
                        _solutionContainer.TryAddSolution(
                            bodySolution.Value,
                            new Solution(interaction.Byproduct.Value, FixedPoint2.New(interaction.ByproductAmount)));
                    }
                    break;
            }
        }

        return modifier;
    }

    /// <summary>
    /// Checks if Charcoal is present in the solution,
    /// which reduces the effective metabolism rate of all other reagents (competitive binding).
    /// </summary>
    /// <param name="solution">The solution being metabolized.</param>
    /// <returns>The rate multiplier (1.0 = no charcoal, 0.6 = charcoal present).</returns>
    public float GetCompetitiveBindingModifier(Solution solution)
    {
        if (solution.GetTotalPrototypeQuantity("Charcoal") > FixedPoint2.Zero)
            return 0.6f;

        return 1f;
    }
}
