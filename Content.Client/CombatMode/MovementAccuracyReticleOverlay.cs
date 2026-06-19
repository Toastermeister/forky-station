using System;
using System.Numerics;
using Content.Client.Hands.Systems;
using Content.Client.Weapons.Ranged.Systems;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Shared.Enums;
using Robust.Shared.Maths;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.CombatMode;

public sealed class MovementAccuracyReticleOverlay : Overlay
{
    private readonly IInputManager _inputManager;
    private readonly IEntityManager _entMan;
    private readonly IEyeManager _eye;
    private readonly CombatModeSystem _combat;
    private readonly HandsSystem _hands;
    private readonly GunSystem _guns;
    private readonly IGameTiming _timing;
    private readonly IPlayerManager _player;

    private readonly Texture _meleeSight;

    public override OverlaySpace Space => OverlaySpace.ScreenSpace;

    public Color StrokeColor = Color.Black.WithAlpha(0.6f);
    public float Scale = 0.6f;

    public MovementAccuracyReticleOverlay(
        IInputManager input,
        IEntityManager entMan,
        IEyeManager eye,
        CombatModeSystem combatSys,
        HandsSystem hands,
        GunSystem guns,
        IGameTiming timing,
        IPlayerManager player)
    {
        _inputManager = input;
        _entMan = entMan;
        _eye = eye;
        _combat = combatSys;
        _hands = hands;
        _guns = guns;
        _timing = timing;
        _player = player;

        var spriteSys = _entMan.EntitySysManager.GetEntitySystem<SpriteSystem>();
        _meleeSight = spriteSys.Frame0(new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Misc/crosshair_pointers.rsi"),
             "melee_sight"));
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (!_combat.IsInCombatMode())
            return false;

        return base.BeforeDraw(in args);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var mouseScreenPosition = _inputManager.MouseScreenPosition;
        var mousePosMap = _eye.PixelToMap(mouseScreenPosition);
        if (mousePosMap.MapId != args.MapId)
            return;

        var handEntity = _hands.GetActiveHandEntity();

        if (handEntity == null || !_entMan.TryGetComponent<GunComponent>(handEntity.Value, out var gun))
        {
            var mousePos = mouseScreenPosition.Position;
            var uiScale = (args.ViewportControl as Control)?.UIScale ?? 1f;
            var limitedScale = uiScale > 1.25f ? 1.25f : uiScale;
            DrawMeleeSight(_meleeSight, args.ScreenHandle, mousePos, limitedScale * Scale);
            return;
        }

        var player = _player.LocalEntity;
        if (player == null || !_entMan.TryGetComponent<TransformComponent>(player.Value, out var playerXform))
            return;

        var center = mouseScreenPosition.Position;
        var playerScreenPos = _eye.CoordinatesToScreen(playerXform.Coordinates).Position;
        var distancePixels = (center - playerScreenPos).Length();
        var distanceRef = MathF.Max(distancePixels, 100f);

        var timeSinceLastFire = Math.Max(0f, (float)(_timing.CurTime - gun.LastFire).TotalSeconds);
        var currentAngle = new Angle(MathHelper.Clamp(
            gun.CurrentAngle.Theta - gun.AngleDecayModified.Theta * timeSinceLastFire,
            gun.MinAngleModified.Theta,
            gun.MaxAngleModified.Theta
        ));

        var ammoSpread = _guns.GetNextAmmoSpread(handEntity.Value);
        var totalHalfAngle = (currentAngle.Theta / 2f) + (ammoSpread.Theta / 2f);
        var spreadRadius = distanceRef * MathF.Tan((float)totalHalfAngle);
        var baseRadius = 8f;
        var totalRadius = baseRadius + spreadRadius;

        var spreadFactor = 0f;
        if (gun.MaxAngleModified.Theta > gun.MinAngleModified.Theta)
        {
            spreadFactor = (float)((currentAngle.Theta - gun.MinAngleModified.Theta) / (gun.MaxAngleModified.Theta - gun.MinAngleModified.Theta));
            spreadFactor = MathHelper.Clamp(spreadFactor, 0f, 1f);
        }

        var mainColor = Color.InterpolateBetween(Color.Lime, Color.Red, spreadFactor).WithAlpha(0.8f);

        var screen = args.ScreenHandle;

        screen.DrawCircle(center, 2.5f, StrokeColor);
        screen.DrawCircle(center, 1.5f, mainColor);

        var d = totalRadius;
        var tickLength = 5f;

        DrawBracketWithStroke(screen, center + new Vector2(-d, -d), new Vector2(1, 0), new Vector2(0, 1), tickLength, mainColor, StrokeColor);
        DrawBracketWithStroke(screen, center + new Vector2(d, -d), new Vector2(-1, 0), new Vector2(0, 1), tickLength, mainColor, StrokeColor);
        DrawBracketWithStroke(screen, center + new Vector2(-d, d), new Vector2(1, 0), new Vector2(0, -1), tickLength, mainColor, StrokeColor);
        DrawBracketWithStroke(screen, center + new Vector2(d, d), new Vector2(-1, 0), new Vector2(0, -1), tickLength, mainColor, StrokeColor);
    }

    private void DrawBracket(DrawingHandleScreen screen, Vector2 start, Vector2 hDir, Vector2 vDir, float length, Color color)
    {
        screen.DrawLine(start, start + hDir * length, color);
        screen.DrawLine(start, start + vDir * length, color);
    }

    private void DrawBracketWithStroke(DrawingHandleScreen screen, Vector2 start, Vector2 hDir, Vector2 vDir, float length, Color mainColor, Color strokeColor)
    {
        DrawBracket(screen, start + new Vector2(-1, 0), hDir, vDir, length, strokeColor);
        DrawBracket(screen, start + new Vector2(1, 0), hDir, vDir, length, strokeColor);
        DrawBracket(screen, start + new Vector2(0, -1), hDir, vDir, length, strokeColor);
        DrawBracket(screen, start + new Vector2(0, 1), hDir, vDir, length, strokeColor);

        DrawBracket(screen, start, hDir, vDir, length, mainColor);
    }

    private void DrawMeleeSight(Texture sight, DrawingHandleScreen screen, Vector2 centerPos, float scale)
    {
        var sightSize = sight.Size * scale;
        var expandedSize = sightSize + new Vector2(7f, 7f);

        screen.DrawTextureRect(sight,
            UIBox2.FromDimensions(centerPos - sightSize * 0.5f, sightSize), StrokeColor);
        screen.DrawTextureRect(sight,
            UIBox2.FromDimensions(centerPos - expandedSize * 0.5f, expandedSize), Color.White.WithAlpha(0.3f));
    }
}
