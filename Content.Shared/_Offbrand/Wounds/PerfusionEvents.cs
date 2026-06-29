using Content.Shared.Body;

namespace Content.Shared._Offbrand.Wounds;

/// <summary>
/// Legacy event definitions retained for dependent status effect systems.
/// These events are no longer actively raised by the perfusion system,
/// which has been removed. They exist solely so that modifier status effect
/// systems (CardiacOutputModifier, VascularToneModifier, etc.) can still compile.
/// </summary>

[ByRefEvent]
public record struct HeartBeatEvent;

[ByRefEvent]
public record struct BaseVascularToneEvent(float Tone);

[ByRefEvent]
public record struct ModifiedVascularToneEvent(float Tone);

[ByRefEvent]
public record struct BaseLungFunctionEvent(float Function);

[ByRefEvent]
public record struct ModifiedLungFunctionEvent(float Function);

[ByRefEvent]
public record struct BaseCardiacOutputEvent(float? Output);

[ByRefEvent]
public record struct ModifiedCardiacOutputEvent(float Output);

[ByRefEvent]
public record struct CardiacCompensationEvent(float Compensation, float Strain, float Supply, float Demand);

[ByRefEvent]
public record struct BaseMetabolicRateEvent(float Rate);

[ByRefEvent]
public record struct ModifiedMetabolicRateEvent(float Rate);

[ByRefEvent]
public record struct ModifiedRespiratoryRateEvent(float Rate);

[ByRefEvent]
public record struct ApplyRespiratoryRateModifiersEvent(float BreathRate);
