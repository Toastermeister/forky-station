using Robust.Shared.GameStates;

namespace Content.Shared._Offbrand.Medical;

[RegisterComponent, NetworkedComponent]
[Access(typeof(PenLightSystem))]
public sealed partial class PenLightComponent : Component
{
    [DataField]
    public TimeSpan DoAfterDuration = TimeSpan.FromSeconds(1);
}
