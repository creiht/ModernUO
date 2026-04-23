# Anatomy

**Anatomy** reveals the weak points of creatures, increasing damage dealt to them. It is particularly effective against high-resistance targets.

**Title:** Biologist | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Biologist |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0.15 |
| Dex Gain | 0.15 |
| Int Gain | 0.7 |

---

## Mechanics

- **Stat scaling:** +0.15 Dex and +0.7 Int per point of those stats
- **Gain rates:** 0.15 Str gain, 0.15 Dex gain, 0.7 Int gain per use
- **Primary use:** Increasing damage against creatures

---

---

## Weapons

None
---

## Items

- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:2543`)
- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:2616`)
---

## NPCs

- `BaseHealer` (`Projects/UOContent/Mobiles/Healers/BaseHealer.cs:53`) — 75.0-97.5
- `FortuneTeller` (`Projects/UOContent/Mobiles/Healers/FortuneTeller.cs:14`) — 85.0-100.0
- `HirePaladin` (`Projects/UOContent/Mobiles/Hireables/HirePaladin.cs:27`) — 65.0-87.5
- `ChaosDragoonElite` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/ChaosDragoonElite.cs:35`) — 80.1-100.0
- `JukaLord` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/JukaLord.cs:30`) — 90.1-100.0
- `LadyJennifyr` (`Projects/UOContent/Mobiles/Monsters/ML/Bedlam/LadyJennifyr.cs:35`) — 129.0-137.5
- `LadyMarai` (`Projects/UOContent/Mobiles/Monsters/ML/Bedlam/LadyMarai.cs:36`) — 126.2-136.5
- `MinotaurCaptain` (`Projects/UOContent/Mobiles/Monsters/ML/Humanoid/Melee/MinotaurCaptain.cs:34`) — 0-6.3

*... and 32 more NPCs with this skill*
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `ParalyzingBlow` (`Projects/UOContent/Items/Weapons/Abilities/ParalyzingBlow.cs:36`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:360`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:662`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:748`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:809`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:875`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:936`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:980`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1022`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1336`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1376`)
---

## Code Locations

- `Projects/UOContent/Skills/Anatomy.cs` — skill handler implementation
- `106 files` — total code references in UOContent

---

## Expansion Notes

Anatomy is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
