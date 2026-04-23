# AnimalTaming

**Animal Taming** is the skill used to capture and tame wild creatures as companions. It is one of the most complex skills with multiple validation checks, scaling mechanics, and post-tame modifications. Tamers use this skill to build collections of loyal animal companions.

**Title:** Tamer | **Primary Stat:** Strength | **Secondary Stat:** Intelligence

**Source:** `Projects/UOContent/Skills/AnimalTaming.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Tamer |
| Primary Stat | Strength |
| Secondary Stat | Intelligence |
| Str Scale | 0.14 |
| Dex Scale | 0.02 |
| Int Scale | 0.04 |
| Str Gain | 1.4 |
| Dex Gain | 0.2 |
| Int Gain | 0.4 |

---

## Mechanics

### Pre-Tame Validation

Before the taming attempt begins, the following checks are performed:

| Check | Description |
|-------|-------------|
| `Tamable == true` | Target must be a `BaseCreature` with `Tamable` flag set |
| Not controlled | Creature must not already have a controller |
| Gender restrictions | Some creatures only allow male or female tamers |
| CuSidhe restriction | CuSidhe can only be tamed by Elves |
| Follower capacity | `Followers + ControlSlots <= FollowersMax` must hold |
| Maximum owners | Creature must not have exceeded its maximum owner limit |
| Subdue requirement | Creatures with `SubdueBeforeTame` must have hits below 10% of max |

### Taming Process

The taming process involves multiple ticks with specific requirements:

- **Cooldown:** 30 seconds between attempts
- **Duration:** 3-6 ticks at 3-second intervals (9-18 seconds total)
- **Range:** Tamer must remain within 6-7 tiles (AOS+) of the creature
- **Line of sight:** Must maintain path and visibility
- **No damage:** Cannot deal damage during the taming process
- **Anger chance:** 95% chance to anger creatures with `CanAngerOnTame` flag

### Success Condition

```
AnimalTaming >= creature.MinTameSkill
```

For wolf-type creatures, a `CheckMastery` override may apply instead.

### Post-Tame Scaling

After a successful tame, the creature's stats and skills are modified:

| Tame Type | Skill Modifier | Skill Cap | Additional Notes |
|-----------|---------------|-----------|-----------------|
| Normal tames | 90% of original | 90% | Default behavior |
| Paralyzed tames | 86% of original | 90% | Creature was paralyzed during tame |
| Greater Dragons | 72% of original | 90% | Magery set to cap |
| Stat loss creatures | 50% of raw stats | Variable | Creatures with stat loss on tame |

### Simultaneous Skill Gains

Animal Lore can gain skill simultaneously during taming attempts, allowing efficient dual-skill progression.

---

## Notable Tameable Creatures

| Creature | Min Tame Skill | Expansion |
|----------|---------------|-----------|
| Black Wolf | 25.1 | SE |
| Gray Wolf | 35.1 | SE |
| White Wolf | 55.1 | SE |
| Dire Wolf | 65.1 | SE |
| Greater Dragon | 95.1 | SE |
| CuSidhe | 50.1 | SE |
| Lushroom Fungo | 10.1 | SE |

---

## Expansion Notes

Animal Taming is available from character creation in the base game. The skill has received various improvements in subsequent expansions.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Creatures: Animals](../creatures/animals.md) — Tameable fauna and mounts
- [Systems: Combat](../systems/combat.md) — Combat mechanics
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
