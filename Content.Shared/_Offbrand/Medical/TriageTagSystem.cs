using Content.Shared.Examine;
using Content.Shared.Interaction;

namespace Content.Shared._Offbrand.Medical;

public sealed partial class TriageTagSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TriageTagComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<TriageTagComponent, ExaminedEvent>(OnExamined);
    }

    private void OnAfterInteract(Entity<TriageTagComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || args.Target is null)
            return;

        // Attach tag to target (patient, bed, gurney)
        // For now, just change the attached entity string for examine purposes
        ent.Comp.AttachedToEntity = args.Target.Value.ToString();
        Dirty(ent);
        args.Handled = true;
    }

    private void OnExamined(Entity<TriageTagComponent> ent, ref ExaminedEvent args)
    {
        var levelName = ent.Comp.Level.ToString().ToLower();
        args.PushMarkup(Loc.GetString($"triage-tag-{levelName}"));
    }
}
