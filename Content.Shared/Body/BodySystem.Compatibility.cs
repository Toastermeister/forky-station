using Robust.Shared.Prototypes;

namespace Content.Shared.Body;

public sealed partial class BodySystem
{
    [Obsolete("Use an event-relay based approach instead")]
    public bool TryGetOrgansWithComponent<TComp>(Entity<BodyComponent?> ent, out List<Entity<TComp>> organs) where TComp : Component
    {
        organs = new();
        if (!_bodyQuery.Resolve(ent, ref ent.Comp))
            return false;

        foreach (var organ in ent.Comp.Organs?.ContainedEntities ?? [])
        {
            if (TryComp<TComp>(organ, out var comp))
                organs.Add((organ, comp));
        }

        return organs.Count != 0;
    }

    public bool TryGetOrgansWithCategoryAndComponent<TComp>(Entity<BodyComponent?> ent, out List<Entity<OrganComponent, TComp>> organs, ProtoId<OrganCategoryPrototype> category) where TComp : Component
    {
        organs = new();
        if (!_bodyQuery.Resolve(ent, ref ent.Comp))
            return false;

        foreach (var uid in ent.Comp.Organs?.ContainedEntities ?? [])
        {
            if (TryComp(uid, out OrganComponent? organ) && TryComp<TComp>(uid, out TComp? comp))
            {
                if (organ.Category == category)
                    organs.Add((uid, organ, comp));
            }
        }

        return organs.Count != 0;
    }
}
