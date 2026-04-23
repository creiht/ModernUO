# MagicResist

**Resisting Spells (MagicResist)** is used to reduce the damage and effects of incoming magical attacks. It is the defensive counterpart to Magery.

**Title:** Warder | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Warder |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0.25 |
| Dex Gain | 0.25 |
| Int Gain | 0.5 |

---

## Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 0.25 Str gain, 0.25 Dex gain, 0.5 Int gain per use
- **Primary use:** Reducing incoming magical damage

MagicResist is the only magical skill with non-zero gain rates across all three stats. It functions as a defensive skill that reduces the effectiveness of enemy spells.

---

---

## Weapons

None
---

## Items

- **Skill Bonus:** `ShroudOfDeceit` (`Projects/UOContent/Items/Champion Artifacts/Unique/ShroudOfDeceit.cs:17`)
- **Tool/Check:** `FireHorn` (`Projects/UOContent/Items/Skill Items/Misc/FireHorn.cs:175`)
---

## NPCs

None
---

## Spells

- `HonorableExecution` (`Projects/UOContent/Spells/Bushido/HonorableExecution.cs:69`) — skill mod
- `EvilOmen` (`Projects/UOContent/Spells/Necromancy/EvilOmen.cs:57`) — skill mod
- `Protection` (`Projects/UOContent/Spells/Second/Protection.cs:141`) — skill mod
---

## Crafting

None
---

## Weapon Abilities

- `ForceOfNature` (`Projects/UOContent/Items/Weapons/Abilities/ForceOfNature.cs:76`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `Heritage` (`Projects/UOContent/Engines/ML Quests/Definitions/Heritage.cs:281`)
- `MistakenIdentity` (`Projects/UOContent/Engines/ML Quests/Definitions/MistakenIdentity.cs:289`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:218`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:663`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:712`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1089`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1127`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1175`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1214`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1252`)
---

## Code Locations

- `427 files` — total code references in UOContent

---

## Expansion Notes

MagicResist is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
