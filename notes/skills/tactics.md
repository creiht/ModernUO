# Tactics

**Tactics** is a combat technique skill that has no stat scaling but affects combat calculations. It is available from character creation.

**Title:** Tactician | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Tactician |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
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
- **Primary use:** Enhancing combat effectiveness

Tactics is one of the "pure" combat skills that functions independently of stat influence. It affects hit chance and damage calculations in combat.

---

---

## Weapons

None
---

## Items

- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:2540`)
- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:2613`)
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

None
---

## Harvest Systems

None
---

## Professions

- **[AOS]** Tactics: 50
- **[AOS]** Tactics: 20
- **[AOS]** Tactics: 50
- **[None]** Tactics: 50
---

## Quests

- `Heritage` (`Projects/UOContent/Engines/ML Quests/Definitions/Heritage.cs:282`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:146`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:664`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:751`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:813`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:878`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:939`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:983`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1025`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1295`)
---

## Code Locations

- `429 files` — total code references in UOContent

---

## Expansion Notes

Tactics is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
