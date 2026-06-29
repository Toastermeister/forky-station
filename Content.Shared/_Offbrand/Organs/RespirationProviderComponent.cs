using Content.Shared.Atmos;
using Robust.Shared.GameStates;

namespace Content.Shared._Offbrand.Organs;

/// <summary>
/// Marks an organ as a provider of respiration/gas exchange data for a body.
/// Added to lung organs to define breathable gas, asphyxiation behavior, etc.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RespirationProviderComponent : Component
{
    /// <summary>
    /// The gas that this species breathes (e.g. Oxygen, Nitrogen).
    /// </summary>
    [DataField(required: true)]
    public Gas BreathableGas = Gas.Oxygen;

    /// <summary>
    /// The gas that this species exhales.
    /// </summary>
    [DataField(required: true)]
    public Gas ExhaledGas = Gas.CarbonDioxide;

    /// <summary>
    /// How many seconds of oxygen deprivation before asphyxiation damage starts.
    /// </summary>
    [DataField]
    public float AsphyxiationDelay = 5f;

    /// <summary>
    /// Rate of asphyxiation damage when deprived of breathable gas.
    /// </summary>
    [DataField]
    public float AsphyxiationDamageRate = 5f;
}
