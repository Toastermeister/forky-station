using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Offbrand.Medical;

[RegisterComponent, NetworkedComponent]
public sealed partial class MedicalBodyScannerComponent : Component
{
    [DataField]
    public TimeSpan ScanDuration = TimeSpan.FromSeconds(3);
}

[Serializable, NetSerializable]
public sealed partial class MedicalBodyScanDoAfterEvent : SimpleDoAfterEvent;
