using Content.Shared._Offbrand.Organs;
using Content.Shared._Offbrand.Wounds;
using Content.Shared.Body.Events;
using Content.Shared.Damage.Systems;
using Content.Shared.Gibbing;
using Content.Shared.Humanoid;
using Content.Shared.Interaction.Events;
using Content.Shared.Medical;
using Content.Shared.Rejuvenate;

namespace Content.Shared.Body;

public sealed partial class BodySystem
{
    // Refrain from adding an infinite block of relays here - consuming systems can use RelayEvent
    private void InitializeRelay()
    {
        SubscribeLocalEvent<BodyComponent, ApplyMetabolicMultiplierEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, TryVomitEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, BeingGibbedEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, ApplyOrganProfileDataEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, ApplyOrganMarkingsEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, OrganCopyAppearanceEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, HumanoidLayerVisibilityChangedEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, SuicideEvent>(RelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, RejuvenateEvent>(RelayBodyEvent);
        // Begin Offbrand
        SubscribeLocalEvent<BodyComponent, DamageDealtEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, DamageChangedEvent>(RelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, WoundableOrganWeightsEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, WoundGetDamageEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, GetWoundsWithSpaceEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, GetPainEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, HealWoundsEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, GetBleedLevelEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, ClampWoundsEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, BeforeInhaledGasEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, BeforeBreathEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, BaseLungFunctionEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, BaseCardiacOutputEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, CardiacCompensationEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, HeartBeatEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, TargetDefibrillatedEvent>(RefRelayBodyEvent);
        SubscribeLocalEvent<BodyComponent, BaseVascularToneEvent>(RefRelayBodyEvent);
        // End Offbrand
    }

    private void RefRelayBodyEvent<T>(EntityUid uid, BodyComponent component, ref T args) where T : struct
    {
        RelayEvent((uid, component), ref args);
    }

    private void RelayBodyEvent<T>(EntityUid uid, BodyComponent component, T args) where T : class
    {
        RelayEvent((uid, component), args);
    }

    public void RelayEvent<T>(Entity<BodyComponent> ent, ref T args) where T : struct
    {
        var ev = new BodyRelayedEvent<T>(ent, args);
        foreach (var organ in ent.Comp.Organs?.ContainedEntities ?? [])
        {
            RaiseLocalEvent(organ, ref ev);
        }
        args = ev.Args;
    }

    public void RelayEvent<T>(Entity<BodyComponent> ent, T args) where T : class
    {
        var ev = new BodyRelayedEvent<T>(ent, args);
        foreach (var organ in ent.Comp.Organs?.ContainedEntities ?? [])
        {
            RaiseLocalEvent(organ, ref ev);
        }
    }
}

/// <summary>
/// Event wrapper for relayed events.
/// </summary>
[ByRefEvent]
public record struct BodyRelayedEvent<TEvent>(Entity<BodyComponent> Body, TEvent Args);
