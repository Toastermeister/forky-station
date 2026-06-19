using System.Numerics;
using System.Threading.Tasks;
using Content.IntegrationTests.Tests.Interaction;
using Content.Shared.CCVar;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Configuration;
using Robust.Shared.Maths;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using NUnit.Framework;

namespace Content.IntegrationTests.Tests.Weapons;

[TestFixture]
public sealed class GunMovementAccuracyTest : InteractionTest
{
    protected override string PlayerPrototype => "MobHuman";
    private static readonly string Mosin = "WeaponSniperMosin";

    [Test]
    public async Task MovementAccuracyPenaltyTest()
    {
        var cfg = Server.ResolveDependency<IConfigurationManager>();
        var physicsSystem = SEntMan.System<SharedPhysicsSystem>();

        // Enable CVar
        cfg.SetCVar(CCVars.GunMovementAccuracyEnabled, true);
        cfg.SetCVar(CCVars.GunMovementAccuracyCoefficient, 2.0f);
        cfg.SetCVar(CCVars.GunMovementAccuracyMaxPenalty, 4.0f);

        // Spawn player and gun
        var player = Player;
        var playerEnt = ToServer(player);
        var physics = Comp<PhysicsComponent>(player);

        // Put Mosin in hand
        var mosinNet = await PlaceInHands(Mosin);

        // Verify component dynamically attached
        Assert.That(HasComp<GunMovementAccuracyComponent>(mosinNet), Is.True);

        // Let the system settle and check base values
        await RunTicks(5);
        var gun = Comp<GunComponent>(mosinNet);
        var baseMinAngle = gun.MinAngle;
        Assert.That(gun.MinAngleModified.Theta, Is.EqualTo(baseMinAngle.Theta).Within(0.001));

        // Start moving
        physicsSystem.SetLinearVelocity(playerEnt, new Vector2(2f, 0f), body: physics);
        await RunTicks(5);

        // Penalty = 1.0 + (2.0 speed * 2.0 coeff) = 5.0, clamped to 4.0 maxPenalty
        var expectedPenalty = 4.0;
        Assert.That(gun.MinAngleModified.Theta, Is.EqualTo(baseMinAngle.Theta * expectedPenalty).Within(0.01));

        // Stop moving
        physicsSystem.SetLinearVelocity(playerEnt, Vector2.Zero, body: physics);
        await RunTicks(5);
        Assert.That(gun.MinAngleModified.Theta, Is.EqualTo(baseMinAngle.Theta).Within(0.01));

        // Disable system via CVar
        cfg.SetCVar(CCVars.GunMovementAccuracyEnabled, false);
        physicsSystem.SetLinearVelocity(playerEnt, new Vector2(2f, 0f), body: physics);
        await RunTicks(5);

        // Verify no penalty is applied when disabled
        Assert.That(gun.MinAngleModified.Theta, Is.EqualTo(baseMinAngle.Theta).Within(0.01));
    }
}
