using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    /// Whether gun accuracy modifiers based on movement are enabled.
    /// </summary>
    public static readonly CVarDef<bool> GunMovementAccuracyEnabled =
        CVarDef.Create("gun.movement_accuracy_enabled", true, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    /// <summary>
    /// How much movement velocity increases the minimum and maximum spread angles.
    /// </summary>
    public static readonly CVarDef<float> GunMovementAccuracyCoefficient =
        CVarDef.Create("gun.movement_accuracy_coefficient", 2.0f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    /// <summary>
    /// The maximum multiplier for the spread penalty due to movement.
    /// </summary>
    public static readonly CVarDef<float> GunMovementAccuracyMaxPenalty =
        CVarDef.Create("gun.movement_accuracy_max_penalty", 4.0f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);
}
