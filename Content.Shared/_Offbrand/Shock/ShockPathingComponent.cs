using Robust.Shared.GameStates;

namespace Content.Shared._Offbrand.Shock;

/// <summary>
/// Marks a body entity as having electrical conductivity.
/// Shock damage will trace a path through the body's organs,
/// applying secondary shock damage to organs along the path.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ShockPathingComponent : Component
{
    /// <summary>
    /// Fraction of the original shock damage applied to each organ along the path.
    /// </summary>
    [DataField]
    public float PathDamageFraction = 0.3f;
}
