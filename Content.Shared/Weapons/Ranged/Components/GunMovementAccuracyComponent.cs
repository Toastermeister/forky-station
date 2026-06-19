using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared.Weapons.Ranged.Components;

/// <summary>
///     Attached to gun entities to track movement-based accuracy modifiers and store per-weapon overrides.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class GunMovementAccuracyComponent : Component
{
    /// <summary>
    ///     Per-weapon coefficient modifier. If set, overrides the global CVar.
    /// </summary>
    [DataField("coefficient"), AutoNetworkedField]
    public float? Coefficient;

    /// <summary>
    ///     Per-weapon maximum penalty multiplier. If set, overrides the global CVar.
    /// </summary>
    [DataField("maxPenalty"), AutoNetworkedField]
    public float? MaxPenalty;

    /// <summary>
    ///     Last tracked speed of the gun's user.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float LastSpeed = 0f;
}
