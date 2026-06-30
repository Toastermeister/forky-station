using Robust.Shared.GameStates;

namespace Content.Shared._Offbrand.Medical;

/// <summary>
/// A triage tag that can be attached to a patient, bed, or gurney.
/// Color indicates severity: Red (immediate), Yellow (delayed), Green (minor), Black (deceased/unrevivable).
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TriageTagComponent : Component
{
    [DataField, AutoNetworkedField]
    public TriageLevel Level = TriageLevel.Green;

    [DataField]
    public string? AttachedToEntity;
}

public enum TriageLevel : byte
{
    /// <summary>Immediate - life-threatening, needs immediate attention</summary>
    Red,

    /// <summary>Delayed - serious but stable, can wait</summary>
    Yellow,

    /// <summary>Minor - walking wounded, minor injuries</summary>
    Green,

    /// <summary>Deceased/Unrevivable - no chance of recovery</summary>
    Black,
}
