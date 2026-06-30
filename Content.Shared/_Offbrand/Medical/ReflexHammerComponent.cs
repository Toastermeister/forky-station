using Robust.Shared.GameStates;

namespace Content.Shared._Offbrand.Medical;

[RegisterComponent, NetworkedComponent]
[Access(typeof(ReflexHammerSystem))]
public sealed partial class ReflexHammerComponent : Component
{
    [DataField]
    public TimeSpan DoAfterDuration = TimeSpan.FromSeconds(1);
}
