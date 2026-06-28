using Content.Shared.Body.Events;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.Systems;
using Content.Shared.Mobs.Events;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Rejuvenate;
using Content.Shared.Speech;
using Content.Shared.StatusEffectNew.Components;
using Content.Shared.Stunnable;
using Robust.Shared.Player;

namespace Content.Shared.StatusEffectNew;

public sealed partial class StatusEffectsSystem
{
    private void InitializeRelay()
    {
        SubscribeLocalEvent<StatusEffectContainerComponent, LocalPlayerAttachedEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, LocalPlayerDetachedEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, RejuvenateEvent>(RelayStatusEffectEvent);

        SubscribeLocalEvent<StatusEffectContainerComponent, RefreshMovementSpeedModifiersEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, UpdateCanMoveEvent>(RelayStatusEffectEvent);

        SubscribeLocalEvent<StatusEffectContainerComponent, RefreshFrictionModifiersEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, TileFrictionEvent>(RefRelayStatusEffectEvent);

        SubscribeLocalEvent<StatusEffectContainerComponent, StandUpAttemptEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, StunEndAttemptEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, RefreshStaminaCritThresholdEvent>(RefRelayStatusEffectEvent);

        SubscribeLocalEvent<StatusEffectContainerComponent, BeforeForceSayEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, BeforeAlertSeverityCheckEvent>(RelayStatusEffectEvent);

        SubscribeLocalEvent<StatusEffectContainerComponent, AccentGetEvent>(RelayStatusEffectEvent);

        SubscribeLocalEvent<StatusEffectContainerComponent, BleedModifierEvent>(RefRelayStatusEffectEvent);

        // Begin Offbrand
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.WoundGetDamageEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.GetWoundsWithSpaceEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.GetPainEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.HealWoundsEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.GetBleedLevelEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.PainSuppressionEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Organs.BeforeDealOrganOxygenDamage>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Organs.BeforeDepleteOrganOxygen>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Organs.BeforeHealOrganOxygenDamage>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.ModifiedVascularToneEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.ModifiedLungFunctionEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.ModifiedMetabolicRateEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.ModifiedCardiacOutputEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Wounds.ModifiedRespiratoryRateEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Weapons.RelayedGetMeleeDamageEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Weapons.RelayedGetMeleeAttackRateEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared._Offbrand.Weapons.RelayedGunRefreshModifiersEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Damage.Systems.ModifySlowOnDamageSpeedEvent>(RefRelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Eye.Blinding.Systems.GetBlurEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.IdentityManagement.Components.SeeIdentityAttemptEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Movement.Pulling.Events.PullStartedMessage>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Movement.Pulling.Events.PullStoppedMessage>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Weapons.Ranged.Events.SelfBeforeGunShotEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Chemistry.Events.SelfBeforeInjectEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, DamageChangedEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Examine.ExaminedEvent>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Verbs.GetVerbsEvent<Content.Shared.Verbs.AlternativeVerb>>(RelayStatusEffectEvent);
        SubscribeLocalEvent<StatusEffectContainerComponent, Content.Shared.Hands.BeforeEquippingHandEvent>(RefRelayStatusEffectEvent);
        // End Offbrand
    }

    private void RefRelayStatusEffectEvent<T>(EntityUid uid, StatusEffectContainerComponent component, ref T args) where T : struct
    {
        RelayEvent((uid, component), ref args);
    }

    private void RelayStatusEffectEvent<T>(EntityUid uid, StatusEffectContainerComponent component, T args) where T : class
    {
        RelayEvent((uid, component), args);
    }

    public void RelayEvent<T>(Entity<StatusEffectContainerComponent> statusEffect, ref T args) where T : struct
    {
        // this copies the by-ref event if it is a struct
        var ev = new StatusEffectRelayedEvent<T>(args);
        foreach (var activeEffect in statusEffect.Comp.ActiveStatusEffects?.ContainedEntities ?? [])
        {
            RaiseLocalEvent(activeEffect, ref ev);
        }
        // and now we copy it back
        args = ev.Args;
    }

    public void RelayEvent<T>(Entity<StatusEffectContainerComponent> statusEffect, T args) where T : class
    {
        // this copies the by-ref event if it is a struct
        var ev = new StatusEffectRelayedEvent<T>(args);
        foreach (var activeEffect in statusEffect.Comp.ActiveStatusEffects?.ContainedEntities ?? [])
        {
            RaiseLocalEvent(activeEffect, ref ev);
        }
    }
}

/// <summary>
/// Event wrapper for relayed events.
/// </summary>
[ByRefEvent]
public record struct StatusEffectRelayedEvent<TEvent>(TEvent Args);
