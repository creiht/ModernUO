# Wrestling

**Wrestling** is the skill used for unarmed combat. It shares the highest Strength gain rate with Mace Fighting.

**Title:** Wrestler | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Wrestler |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
| Str Scale | 0.09 |
| Dex Scale | 0.01 |
| Int Scale | 0 |
| Str Gain | 0.9 |
| Dex Gain | 0.1 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.09 Str and +0.01 Dex per point of those stats (tied with Macing for highest Str scale)
- **Gain rates:** 0.9 Str gain, 0.1 Dex gain per use (tied with Macing)
- **StatTotal:** 10
- **Primary use:** Unarmed combat

Wrestling is the foundation of unarmed combat and is available from character creation.

---

---

## Weapons

- `Fists` (`Projects/UOContent/Items/Weapons/Fists.cs:36`)
---

## Items

None
---

## NPCs

None
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `Disarm` (`Projects/UOContent/Items/Weapons/Abilities/Disarm.cs:18`) — tactics exemption
- `ParalyzingBlow` (`Projects/UOContent/Items/Weapons/Abilities/ParalyzingBlow.cs:22`) — skill reference
---

## Harvest Systems

None
---

## Professions

- **[AOS]** Wrestling: 0
- **[AOS]** Wrestling: 0
- **[AOS]** Wrestling: 0
- **[None]** Wrestling: 0
---

## Quests

- `Heritage` (`Projects/UOContent/Engines/ML Quests/Definitions/Heritage.cs:283`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:51`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:713`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1090`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1128`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1176`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1215`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1253`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1590`)
---

## Code Locations

- `400 files` — total code references in UOContent

---

## Expansion Notes

Wrestling is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
