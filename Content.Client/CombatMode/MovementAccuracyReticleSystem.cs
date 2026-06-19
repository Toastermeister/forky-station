using Content.Client.Hands.Systems;
using Content.Client.Weapons.Ranged.Systems;
using Content.Shared.CCVar;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Timing;

namespace Content.Client.CombatMode;

public sealed class MovementAccuracyReticleSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlayManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IEyeManager _eye = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        Subs.CVar(_cfg, CCVars.CombatModeIndicatorsPointShow, _ => UpdateOverlays(), true);
        Subs.CVar(_cfg, CCVars.GunMovementAccuracyEnabled, _ => UpdateOverlays(), true);
    }

    public override void Shutdown()
    {
        if (_overlayManager.HasOverlay<MovementAccuracyReticleOverlay>())
            _overlayManager.RemoveOverlay<MovementAccuracyReticleOverlay>();

        // Restore default overlay on shutdown if configured to show it
        if (_cfg.GetCVar(CCVars.CombatModeIndicatorsPointShow) && !_overlayManager.HasOverlay<CombatModeIndicatorsOverlay>())
        {
            _overlayManager.AddOverlay(new CombatModeIndicatorsOverlay(
                _inputManager,
                EntityManager,
                _eye,
                EntityManager.System<CombatModeSystem>(),
                EntityManager.System<HandsSystem>()
            ));
        }

        base.Shutdown();
    }

    private void UpdateOverlays()
    {
        var showIndicators = _cfg.GetCVar(CCVars.CombatModeIndicatorsPointShow);
        var movementEnabled = _cfg.GetCVar(CCVars.GunMovementAccuracyEnabled);

        if (showIndicators && movementEnabled)
        {
            if (_overlayManager.HasOverlay<CombatModeIndicatorsOverlay>())
                _overlayManager.RemoveOverlay<CombatModeIndicatorsOverlay>();

            if (!_overlayManager.HasOverlay<MovementAccuracyReticleOverlay>())
            {
                _overlayManager.AddOverlay(new MovementAccuracyReticleOverlay(
                    _inputManager,
                    EntityManager,
                    _eye,
                    EntityManager.System<CombatModeSystem>(),
                    EntityManager.System<HandsSystem>(),
                    EntityManager.System<GunSystem>(),
                    _timing,
                    _playerManager
                ));
            }
        }
        else
        {
            if (_overlayManager.HasOverlay<MovementAccuracyReticleOverlay>())
                _overlayManager.RemoveOverlay<MovementAccuracyReticleOverlay>();

            if (showIndicators)
            {
                if (!_overlayManager.HasOverlay<CombatModeIndicatorsOverlay>())
                {
                    _overlayManager.AddOverlay(new CombatModeIndicatorsOverlay(
                        _inputManager,
                        EntityManager,
                        _eye,
                        EntityManager.System<CombatModeSystem>(),
                        EntityManager.System<HandsSystem>()
                    ));
                }
            }
            else
            {
                if (_overlayManager.HasOverlay<CombatModeIndicatorsOverlay>())
                    _overlayManager.RemoveOverlay<CombatModeIndicatorsOverlay>();
            }
        }
    }
}
