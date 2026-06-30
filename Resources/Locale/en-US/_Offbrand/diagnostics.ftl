# Reflex Hammer
reflex-hammer-normal = { CAPITALIZE($patient) }'s limb reacts normally to the reflex test.
reflex-hammer-no-reflex = { CAPITALIZE($patient) } has no reflex response. Nerve damage detected.

# Medical Body Scanner
medical-scanner-result = [bold]Scan Results for { $patient }:[/bold]\n
    Wounds: { $wounds } | Fractures: { $fractures } | Bleeding sites: { $bleeding } | Damaged organs: { $organs }

# Triage Tags
triage-tag-red = <b><color=red>TRIAGE: IMMEDIATE</color></b> — Life-threatening, needs urgent care.
triage-tag-yellow = <b><color=yellow>TRIAGE: DELAYED</color></b> — Serious but stable, can wait.
triage-tag-green = <b><color=lime>TRIAGE: MINOR</color></b> — Walking wounded, minor injuries.
triage-tag-black = <b><color=white>TRIAGE: DECEASED</color></b> — No signs of life.

# Pen-light
penlight-pupils-normal = { CAPITALIZE($patient) }'s pupils constrict normally. Neurological response intact.
penlight-pupils-sluggish = { CAPITALIZE($patient) }'s pupils are sluggish and slow to constrict.
penlight-pupils-unresponsive = { CAPITALIZE($patient) }'s pupils are dilated and unresponsive.
