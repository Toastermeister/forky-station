// SPDX-FileCopyrightText: 2021 Paul Ritter <ritter.paul1@googlemail.com>
// SPDX-FileCopyrightText: 2021 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 2022-2023 Leon Friedrich <60421075+ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022-2023 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 2022-2023 keronshb <54602815+keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 KIBORG04 <bossmira4@gmail.com>
// SPDX-FileCopyrightText: 2022 wrexbe <81056464+wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 2022 mirrorcult <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2023-2025 metalgearsloth <31366439+metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023-2024 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023-2024 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 2023-2024 themias <89101928+themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 chromiumboy <50505512+chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 2023 Ahion <58528255+Ahion@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024-2025 slarticodefast <161409025+slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024-2025 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 2024 chavonadelal <156101927+chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Kara <lunarautomaton6@gmail.com>
// SPDX-FileCopyrightText: 2024 BombasterDS <115770678+BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 2024 Pspritechologist <81725545+Pspritechologist@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 2024 nikthechampiongr <32041239+nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 2025 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 2025 kosticia <kosticia46@gmail.com>
// SPDX-FileCopyrightText: 2025 Ciarán Walsh <github@ciaranwal.sh>
// SPDX-FileCopyrightText: 2025 Ignaz "Ian" Kraft <ignaz.k@live.de>
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Shared.Access.Systems;
using Content.Shared.ActionBlocker;
using Content.Shared.Clothing;
using Content.Shared.Damage.Components;
using Content.Shared.DeviceNetwork;
using Content.Shared.DoAfter;
using Content.Shared.Emp;
using Content.Shared.Examine;
using Content.Shared.GameTicking;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Station;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Medical.PDASensor;

public abstract class SharedPDASensorSystem : EntitySystem
{
    [Dependency] private readonly SharedStationSystem _stationSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly MobThresholdSystem _mobThresholdSystem = default!;
    [Dependency] private readonly SharedInteractionSystem _interactionSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedIdCardSystem _idCardSystem = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private EntityQuery<PDASensorComponent> _sensorQuery;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PDASensorComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawn);
        SubscribeLocalEvent<PDASensorComponent, ClothingGotEquippedEvent>(OnEquipped);
        SubscribeLocalEvent<PDASensorComponent, ClothingGotUnequippedEvent>(OnUnequipped);
        SubscribeLocalEvent<PDASensorComponent, EmpPulseEvent>(OnEmpPulse);
        SubscribeLocalEvent<PDASensorComponent, EmpDisabledRemovedEvent>(OnEmpFinished);
        SubscribeLocalEvent<PDASensorComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<PDASensorComponent, GetVerbsEvent<Verb>>(OnVerb);
        SubscribeLocalEvent<PDASensorComponent, EntGotInsertedIntoContainerMessage>(OnInsert);
        SubscribeLocalEvent<PDASensorComponent, EntGotRemovedFromContainerMessage>(OnRemove);
        SubscribeLocalEvent<PDASensorComponent, PDASensorChangeDoAfterEvent>(OnPDASensorDoAfter);

        _sensorQuery = GetEntityQuery<PDASensorComponent>();
    }

    /// <summary>
    /// Checks whether the sensor is assigned to a station or not
    /// and tries to assign an unassigned sensor to a station if it's currently on a grid.
    /// </summary>
    /// <returns>True if the sensor is assigned to a station or assigning it was successful. False otherwise.</returns>
    public bool CheckSensorAssignedStation(Entity<PDASensorComponent> sensor)
    {
        if (!sensor.Comp.StationId.HasValue && Transform(sensor.Owner).GridUid == null)
            return false;

        sensor.Comp.StationId = _stationSystem.GetOwningStation(sensor.Owner);
        Dirty(sensor);
        return sensor.Comp.StationId.HasValue;
    }

    private void OnMapInit(Entity<PDASensorComponent> ent, ref MapInitEvent args)
    {
        // Fallback
        ent.Comp.StationId ??= _stationSystem.GetOwningStation(ent.Owner);

        // generate random mode
        if (ent.Comp.RandomMode)
        {
            //make the sensor mode favor higher levels, except coords.
            var modesDist = new[]
            {
                PDASensorMode.SensorOff,
                PDASensorMode.SensorBinary, PDASensorMode.SensorBinary,
                PDASensorMode.SensorVitals, PDASensorMode.SensorVitals, PDASensorMode.SensorVitals,
                PDASensorMode.SensorCords, PDASensorMode.SensorCords
            };
            ent.Comp.Mode = _random.Pick(modesDist);
        }

        ent.Comp.NextUpdate = _timing.CurTime;
        Dirty(ent);
    }

    private void OnPlayerSpawn(PlayerSpawnCompleteEvent ev)
    {
        // If the player spawns in arrivals then the grid underneath them may not be appropriate.
        // in which case we'll just use the station spawn code told us they are attached to and set all of their
        // sensors.
        RecursiveSensor(ev.Mob, ev.Station);
    }

    private void RecursiveSensor(EntityUid uid, EntityUid stationUid)
    {
        var xform = Transform(uid);
        var enumerator = xform.ChildEnumerator;

        while (enumerator.MoveNext(out var child))
        {
            if (_sensorQuery.TryComp(child, out var sensor))
            {
                sensor.StationId = stationUid;
                Dirty(child, sensor);
            }

            RecursiveSensor(child, stationUid);
        }
    }

    private void OnEquipped(Entity<PDASensorComponent> ent, ref ClothingGotEquippedEvent args)
    {
        ent.Comp.User = args.Wearer;
        Dirty(ent);
    }

    private void OnUnequipped(Entity<PDASensorComponent> ent, ref ClothingGotUnequippedEvent args)
    {
        ent.Comp.User = null;
        Dirty(ent);
    }

    private void OnEmpPulse(Entity<PDASensorComponent> ent, ref EmpPulseEvent args)
    {
        args.Affected = true;
        args.Disabled = true;

        ent.Comp.PreviousMode = ent.Comp.Mode;
        SetSensor(ent.AsNullable(), PDASensorMode.SensorOff, null);

        ent.Comp.PreviousControlsLocked = ent.Comp.ControlsLocked;
        ent.Comp.ControlsLocked = true;
        // SetSensor already calls Dirty
    }

    private void OnEmpFinished(Entity<PDASensorComponent> ent, ref EmpDisabledRemovedEvent args)
    {
        SetSensor(ent.AsNullable(), ent.Comp.PreviousMode, null);
        ent.Comp.ControlsLocked = ent.Comp.PreviousControlsLocked;
    }

    private void OnExamine(Entity<PDASensorComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        string msg;
        switch (ent.Comp.Mode)
        {
            case PDASensorMode.SensorOff:
                msg = "PDA-sensor-examine-off";
                break;
            case PDASensorMode.SensorBinary:
                msg = "PDA-sensor-examine-binary";
                break;
            case PDASensorMode.SensorVitals:
                msg = "PDA-sensor-examine-vitals";
                break;
            case PDASensorMode.SensorCords:
                msg = "PDA-sensor-examine-cords";
                break;
            default:
                return;
        }

        args.PushMarkup(Loc.GetString(msg));
    }

    private void OnVerb(Entity<PDASensorComponent> ent, ref GetVerbsEvent<Verb> args)
    {
        // check if user can change sensor
        if (ent.Comp.ControlsLocked)
            return;

        // standard interaction checks
        if (!args.CanInteract || args.Hands == null)
            return;

        if (!_interactionSystem.InRangeUnobstructed(args.User, args.Target))
            return;

        // check if target is incapacitated (cuffed, dead, etc)
        if (ent.Comp.User != null && args.User != ent.Comp.User && _actionBlocker.CanInteract(ent.Comp.User.Value, null))
            return;

        args.Verbs.UnionWith(new[]
        {
            CreateVerb(ent, args.User, PDASensorMode.SensorOff),
            CreateVerb(ent, args.User, PDASensorMode.SensorBinary),
            CreateVerb(ent, args.User, PDASensorMode.SensorVitals),
            CreateVerb(ent, args.User, PDASensorMode.SensorCords)
        });
    }

    private void OnInsert(Entity<PDASensorComponent> ent, ref EntGotInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.ActivationContainer)
            return;

        ent.Comp.User = args.Container.Owner;
        Dirty(ent);
    }

    private void OnRemove(Entity<PDASensorComponent> ent, ref EntGotRemovedFromContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.ActivationContainer)
            return;

        ent.Comp.User = null;
        Dirty(ent);
    }

    private Verb CreateVerb(Entity<PDASensorComponent> ent, EntityUid userUid, PDASensorMode mode)
    {
        return new Verb()
        {
            Text = GetModeName(mode),
            Disabled = ent.Comp.Mode == mode,
            Priority = -(int)mode, // sort them in descending order
            Category = VerbCategory.SetSensor,
            Act = () => TrySetSensor(ent.AsNullable(), mode, userUid)
        };
    }

    public string GetModeName(PDASensorMode mode)
    {
        string name;
        switch (mode)
        {
            case PDASensorMode.SensorOff:
                name = "PDA-sensor-mode-off";
                break;
            case PDASensorMode.SensorBinary:
                name = "PDA-sensor-mode-binary";
                break;
            case PDASensorMode.SensorVitals:
                name = "PDA-sensor-mode-vitals";
                break;
            case PDASensorMode.SensorCords:
                name = "PDA-sensor-mode-cords";
                break;
            default:
                return "";
        }

        return Loc.GetString(name);
    }

    /// <summary>
    /// Attempts to set <see cref="PDASensorComponent"/> mode of the entity to the selected in params.
    /// Works instantly if the user is the player wearing the sensors and will start a DoAfter otherwise.
    /// </summary>
    /// <param name="sensors">Entity and its component that should be changed.</param>
    /// <param name="mode">Selected mode</param>
    /// <param name="userUid">userUid, when not equal to the <see cref="PDASensorComponent.User"/>, creates doafter</param>
    public bool TrySetSensor(Entity<PDASensorComponent?> sensors, PDASensorMode mode, EntityUid userUid)
    {
        if (!Resolve(sensors, ref sensors.Comp, false))
            return false;

        if (sensors.Comp.User == null || userUid == sensors.Comp.User)
            SetSensor(sensors, mode, userUid);
        else
        {
            var doAfterEvent = new PDASensorChangeDoAfterEvent(mode);
            var doAfterArgs = new DoAfterArgs(EntityManager, userUid, sensors.Comp.SensorsTime, doAfterEvent, sensors)
            {
                BreakOnMove = true,
                BreakOnDamage = true
            };

            _doAfterSystem.TryStartDoAfter(doAfterArgs);
        }
        return true;
    }

    private void OnPDASensorDoAfter(Entity<PDASensorComponent> sensors, ref PDASensorChangeDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        SetSensor(sensors.AsNullable(), args.Mode, args.User);
    }

    /// <summary>
    /// Sets mode of the <see cref="PDASensorComponent"/> of the chosen entity.
    /// Makes popup when <param name="userUid"> not null
    /// </summary>
    /// <param name="sensors">Entity and it's component that should be changed</param>
    /// <param name="mode">Selected mode</param>
    /// <param name="userUid">uid, required for the popup</param>
    public void SetSensor(Entity<PDASensorComponent?> sensors, PDASensorMode mode, EntityUid? userUid = null)
    {
        if (!Resolve(sensors, ref sensors.Comp, false))
            return;

        sensors.Comp.Mode = mode;
        Dirty(sensors);

        if (userUid != null)
        {
            var msg = Loc.GetString("PDA-sensor-mode-state", ("mode", GetModeName(mode)));
            _popupSystem.PopupClient(msg, sensors, userUid.Value);
        }
    }

    /// <summary>
    /// Set all PDA sensors on the equipment someone is wearing to the specified mode.
    /// </summary>
    public void SetAllSensors(EntityUid target, PDASensorMode mode, SlotFlags slots = SlotFlags.All)
    {
        // iterate over all inventory slots
        var slotEnumerator = _inventory.GetSlotEnumerator(target, slots);
        while (slotEnumerator.NextItem(out var item, out _))
        {
            if (TryComp<PDASensorComponent>(item, out var sensorComp))
                SetSensor((item, sensorComp), mode);
        }
    }

    /// <summary>
    /// Attempts to get full <see cref="PDASensorStatus"/> from the <see cref="PDASensorComponent"/>
    /// </summary>
    /// <param name="uid">Entity to get status</param>
    /// <returns>Full <see cref="PDASensorStatus"/> of the chosen uid</returns>
    public PDASensorStatus? GetSensorState(Entity<PDASensorComponent?, TransformComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp1, ref ent.Comp2, false))
            return null;

        var sensor = ent.Comp1;
        var transform = ent.Comp2;

        // check if sensor is enabled and worn by user
        if (sensor.Mode == PDASensorMode.SensorOff || sensor.User == null || !HasComp<MobStateComponent>(sensor.User) || transform.GridUid == null)
            return null;

        // try to get mobs id from ID slot
        var userName = Loc.GetString("PDA-sensor-component-unknown-name");
        var userJob = Loc.GetString("PDA-sensor-component-unknown-job");
        var userJobIcon = "JobIconNoId";
        var userJobDepartments = new List<string>();

        if (_idCardSystem.TryFindIdCard(sensor.User.Value, out var card))
        {
            if (card.Comp.FullName != null)
                userName = card.Comp.FullName;
            if (card.Comp.LocalizedJobTitle != null)
                userJob = card.Comp.LocalizedJobTitle;
            userJobIcon = card.Comp.JobIcon;

            foreach (var department in card.Comp.JobDepartments)
                userJobDepartments.Add(Loc.GetString(_proto.Index(department).Name));
        }

        // get health mob state
        var isAlive = false;
        if (TryComp(sensor.User.Value, out MobStateComponent? mobState))
            isAlive = !_mobStateSystem.IsDead(sensor.User.Value, mobState);

        // get mob total damage
        var totalDamage = 0;
        if (TryComp<DamageableComponent>(sensor.User.Value, out var damageable))
            totalDamage = damageable.TotalDamage.Int();

        // Get mob total damage crit threshold
        int? totalDamageThreshold = null;
        if (_mobThresholdSystem.TryGetThresholdForState(sensor.User.Value, MobState.Critical, out var critThreshold))
            totalDamageThreshold = critThreshold.Value.Int();

        // finally, form PDA sensor status
        var status = new PDASensorStatus(GetNetEntity(sensor.User.Value), GetNetEntity(ent.Owner), userName, userJob, userJobIcon, userJobDepartments);
        switch (sensor.Mode)
        {
            case PDASensorMode.SensorBinary:
                status.IsAlive = isAlive;
                break;
            case PDASensorMode.SensorVitals:
                status.IsAlive = isAlive;
                status.TotalDamage = totalDamage;
                status.TotalDamageThreshold = totalDamageThreshold;
                break;
            case PDASensorMode.SensorCords:
                status.IsAlive = isAlive;
                status.TotalDamage = totalDamage;
                status.TotalDamageThreshold = totalDamageThreshold;
                EntityCoordinates coordinates;
                var xformQuery = GetEntityQuery<TransformComponent>();

                if (transform.GridUid != null)
                {
                    coordinates = new EntityCoordinates(transform.GridUid.Value,
                        Vector2.Transform(_transform.GetWorldPosition(transform, xformQuery),
                            _transform.GetInvWorldMatrix(xformQuery.GetComponent(transform.GridUid.Value), xformQuery)));
                }
                else if (transform.MapUid != null)
                {
                    coordinates = new EntityCoordinates(transform.MapUid.Value,
                        _transform.GetWorldPosition(transform, xformQuery));
                }
                else
                {
                    coordinates = EntityCoordinates.Invalid;
                }

                status.Coordinates = GetNetCoordinates(coordinates);
                break;
        }

        return status;
    }

    /// <summary>
    /// Create a device network package from the PDA sensors status.
    /// </summary>
    public NetworkPayload PDASensorToPacket(PDASensorStatus status)
    {
        var payload = new NetworkPayload()
        {
            [DeviceNetworkConstants.Command] = DeviceNetworkConstants.CmdUpdatedState,
            [PDASensorConstants.NET_NAME] = status.Name,
            [PDASensorConstants.NET_JOB] = status.Job,
            [PDASensorConstants.NET_JOB_ICON] = status.JobIcon,
            [PDASensorConstants.NET_JOB_DEPARTMENTS] = status.JobDepartments,
            [PDASensorConstants.NET_IS_ALIVE] = status.IsAlive,
            [PDASensorConstants.NET_PDA_SENSOR_UID] = status.PDASensorUid,
            [PDASensorConstants.NET_OWNER_UID] = status.OwnerUid,
        };

        if (status.TotalDamage != null)
            payload.Add(PDASensorConstants.NET_TOTAL_DAMAGE, status.TotalDamage);
        if (status.TotalDamageThreshold != null)
            payload.Add(PDASensorConstants.NET_TOTAL_DAMAGE_THRESHOLD, status.TotalDamageThreshold);
        if (status.Coordinates != null)
            payload.Add(PDASensorConstants.NET_COORDINATES, status.Coordinates);

        return payload;
    }

    /// <summary>
    /// Try to create the PDA sensors status from the device network message.
    /// </summary>
    public PDASensorStatus? PacketToPDASensor(NetworkPayload payload)
    {
        // check command
        if (!payload.TryGetValue(DeviceNetworkConstants.Command, out string? command))
            return null;
        if (command != DeviceNetworkConstants.CmdUpdatedState)
            return null;

        // check name, job and alive
        if (!payload.TryGetValue(PDASensorConstants.NET_NAME, out string? name)) return null;
        if (!payload.TryGetValue(PDASensorConstants.NET_JOB, out string? job)) return null;
        if (!payload.TryGetValue(PDASensorConstants.NET_JOB_ICON, out string? jobIcon)) return null;
        if (!payload.TryGetValue(PDASensorConstants.NET_JOB_DEPARTMENTS, out List<string>? jobDepartments)) return null;
        if (!payload.TryGetValue(PDASensorConstants.NET_IS_ALIVE, out bool? isAlive)) return null;
        if (!payload.TryGetValue(PDASensorConstants.NET_PDA_SENSOR_UID, out NetEntity PDASensorUid)) return null;
        if (!payload.TryGetValue(PDASensorConstants.NET_OWNER_UID, out NetEntity ownerUid)) return null;

        // try get total damage and cords (optionals)
        payload.TryGetValue(PDASensorConstants.NET_TOTAL_DAMAGE, out int? totalDamage);
        payload.TryGetValue(PDASensorConstants.NET_TOTAL_DAMAGE_THRESHOLD, out int? totalDamageThreshold);
        payload.TryGetValue(PDASensorConstants.NET_COORDINATES, out NetCoordinates? coords);

        var status = new PDASensorStatus(ownerUid, PDASensorUid, name, job, jobIcon, jobDepartments)
        {
            IsAlive = isAlive.Value,
            TotalDamage = totalDamage,
            TotalDamageThreshold = totalDamageThreshold,
            Coordinates = coords,
        };
        return status;
    }
}
