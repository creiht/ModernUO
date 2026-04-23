# Magery

**Magery** is the foundational magical skill, required for casting spells from all spell schools. It has the highest Intelligence scale among all magical skills.

**Title:** Mage | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Mage |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0.15 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 1.5 |

---

## Mechanics

- **Stat scaling:** +0.15 Int per point of Intelligence (highest Int scale of any skill)
- **Gain rates:** 1.5 Int gain per use (highest Int gain of any skill)
- **StatTotal:** 15
- **Primary use:** Casting spells from all spell schools
- **Spell access:** Required for First through Eighth circle spells

Magery is the most Intelligence-reliant skill in the game and the primary stat driver for all spellcasters. Without Magery, a character cannot cast any spells. The `SpellInfo.Magery` requirement for each spell determines the minimum Magery level needed.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `BaseHealer` (`Projects/UOContent/Mobiles/Healers/BaseHealer.cs:56`) — 82.0-100.0
- `HireBard` (`Projects/UOContent/Mobiles/Hireables/HireBard.cs:27`) — 22-22
- `HireBardArcher` (`Projects/UOContent/Mobiles/Hireables/HireBardArcher.cs:27`) — 22-22
- `HireBeggar` (`Projects/UOContent/Mobiles/Hireables/HireBeggar.cs:29`) — 2-2
- `HireFighter` (`Projects/UOContent/Mobiles/Hireables/HireFighter.cs:27`) — 22-22
- `HireMage` (`Projects/UOContent/Mobiles/Hireables/HireMage.cs:27`) — 100-125
- `HireRanger` (`Projects/UOContent/Mobiles/Hireables/HireRanger.cs:29`) — 62-62
- `HireRangerArcher` (`Projects/UOContent/Mobiles/Hireables/HireRangerArcher.cs:29`) — 62-62
- `EvilMage` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Magic/EvilMage.cs:42`) — 75.1-100.0

*... and 31 more NPCs with this skill*
---

## Spells

- `Spell` (`Projects/UOContent/Spells/Base/Spell.cs:51`) — CastSkill
- `Spell` (`Projects/UOContent/Spells/Base/Spell.cs:704`) — CastSkill
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

- **[AOS]** Magery: 50
- **[None]** Magery: 50
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:289`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:711`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1088`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1126`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1174`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1213`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1251`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1294`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1687`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1749`)
---

## Code Locations

- `159 files` — total code references in UOContent

---

## Expansion Notes

Magery is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Spells: Magery](../spells/magery.md) — First through Eighth circle spells
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
