using Content.Shared._Offbrand.Organs;
using Content.Shared.Alert;

namespace Content.Shared._Offbrand.Wounds;

public sealed partial class HeartrateAlertsSystem : EntitySystem
{
    [Dependency] private AlertsSystem _alerts = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HeartrateAlertsComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<HeartrateAlertsComponent, ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<HeartrateAlertsComponent, HeartStoppedEvent>(OnHeartStopped);
        SubscribeLocalEvent<HeartrateAlertsComponent, HeartStartedEvent>(OnHeartStarted);
    }

    private void UpdateAlert(Entity<HeartrateAlertsComponent> ent)
    {
        if (ent.Comp.Beating)
        {
            _alerts.ShowAlert(ent.Owner, ent.Comp.StrainAlert, 0);
        }
        else
        {
            _alerts.ShowAlert(ent.Owner, ent.Comp.StoppedAlert);
        }
    }

    private void OnMapInit(Entity<HeartrateAlertsComponent> ent, ref MapInitEvent args)
    {
        UpdateAlert(ent);
    }

    private void OnComponentShutdown(Entity<HeartrateAlertsComponent> ent, ref ComponentShutdown args)
    {
        _alerts.ClearAlertCategory(ent.Owner, ent.Comp.AlertCategory);
    }

    private void OnHeartStopped(Entity<HeartrateAlertsComponent> ent, ref HeartStoppedEvent args)
    {
        ent.Comp.Beating = false;
        Dirty(ent);
        UpdateAlert(ent);
    }

    private void OnHeartStarted(Entity<HeartrateAlertsComponent> ent, ref HeartStartedEvent args)
    {
        ent.Comp.Beating = true;
        Dirty(ent);
        UpdateAlert(ent);
    }
}
