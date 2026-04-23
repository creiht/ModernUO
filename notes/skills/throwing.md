# Throwing

**Throwing** is the skill used with thrown weapons (daggers, knives, shuriken, fukiya darts, etc.). It is expansion-gated to **SA (Samurai Adventure)** and is restricted to Gargoyle characters.

**Title:** Bladeweaver | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Bladeweaver |
| Primary Stat | Dexterity |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SA expansion
- **Race restriction:** Gargoyles only (per `CharacterCreation.ValidateSkills`)
- **Weapon types:** Thrown ranged weapons

Thrown weapons have dynamic range based on the attacker's Strength:
```
DefMaxRange = baseRange - 3 + (Str - AosStrengthReq) / ((140 - AosStrengthReq) / 3)
```

After hitting or missing, thrown weapons return to the thrower after 0.3 seconds (unless the `MysticArc` weapon ability is active).

---

## Expansion Notes

Throwing requires the SA (Samurai Adventure) expansion and is restricted to Gargoyle characters.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Expansions: Timeline](../expansions/timeline.md) — Expansion details
