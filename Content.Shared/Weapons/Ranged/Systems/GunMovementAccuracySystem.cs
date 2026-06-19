using System;
using Content.Shared.CCVar;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Physics.Components;

namespace Content.Shared.Weapons.Ranged.Systems;

public sealed class GunMovementAccuracySystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly SharedGunSystem _gunSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunComponent, GunRefreshModifiersEvent>(OnGunRefreshModifiers);
    }

    private void OnGunRefreshModifiers(Entity<GunComponent> ent, ref GunRefreshModifiersEvent args)
    {
        if (!_cfg.GetCVar(CCVars.GunMovementAccuracyEnabled))
            return;

        var xform = Transform(ent);
        var parent = xform.ParentUid;
        if (!parent.IsValid())
            return;

        if (!TryComp<PhysicsComponent>(parent, out var physics))
            return;

        var speed = physics.LinearVelocity.Length();
        if (speed <= 0.05f)
            return;

        var coeff = _cfg.GetCVar(CCVars.GunMovementAccuracyCoefficient);
        var maxPenalty = _cfg.GetCVar(CCVars.GunMovementAccuracyMaxPenalty);

        if (TryComp<GunMovementAccuracyComponent>(ent, out var accuracy))
        {
            if (accuracy.Coefficient.HasValue)
                coeff = accuracy.Coefficient.Value;
            if (accuracy.MaxPenalty.HasValue)
                maxPenalty = accuracy.MaxPenalty.Value;
        }

        var penalty = 1.0f + (speed * coeff);
        penalty = Math.Min(penalty, maxPenalty);

        args.MinAngle = new Angle(args.MinAngle.Theta * penalty);
        args.MaxAngle = new Angle(args.MaxAngle.Theta * penalty);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_cfg.GetCVar(CCVars.GunMovementAccuracyEnabled))
            return;

        var query = EntityQueryEnumerator<GunComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var gun, out var xform))
        {
            var parent = xform.ParentUid;
            var currentSpeed = 0f;
            var isHeld = false;

            if (parent.IsValid() && TryComp<PhysicsComponent>(parent, out var physics))
            {
                currentSpeed = physics.LinearVelocity.Length();
                isHeld = true;
            }

            if (!isHeld)
            {
                if (TryComp<GunMovementAccuracyComponent>(uid, out var accuracy))
                {
                    if (accuracy.LastSpeed != 0f)
                    {
                        accuracy.LastSpeed = 0f;
                        _gunSystem.RefreshModifiers(uid);
                    }

                    if (accuracy.Coefficient == null && accuracy.MaxPenalty == null)
                    {
                        RemCompDeferred<GunMovementAccuracyComponent>(uid);
                    }
                }
                continue;
            }

            var accuracyComp = EnsureComp<GunMovementAccuracyComponent>(uid);
            if (MathF.Abs(accuracyComp.LastSpeed - currentSpeed) > 0.05f)
            {
                accuracyComp.LastSpeed = currentSpeed;
                _gunSystem.RefreshModifiers(uid);
            }
        }
    }
}
