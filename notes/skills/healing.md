# Healing

**Healing** is used to restore hit points to creatures. It is the primary healing skill for players.

**Title:** Healer | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Healer |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0.06 |
| Dex Scale | 0.06 |
| Int Scale | 0.08 |
| Str Gain | 0.6 |
| Dex Gain | 0.6 |
| Int Gain | 0.8 |

---

## Mechanics

- **Stat scaling:** +0.06 Str, +0.06 Dex, +0.08 Int per point of those stats
- **Gain rates:** 0.6 Str gain, 0.6 Dex gain, 0.8 Int gain per use
- **Primary use:** Healing players and creatures

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `BaseHealer` (`Projects/UOContent/Mobiles/Healers/BaseHealer.cs:55`) — 75.0-97.5
- `FortuneTeller` (`Projects/UOContent/Mobiles/Healers/FortuneTeller.cs:15`) — 90.0-100.0
- `HirePaladin` (`Projects/UOContent/Mobiles/Hireables/HirePaladin.cs:29`) — 65.0-87.5
- `HireSailor` (`Projects/UOContent/Mobiles/Hireables/HireSailor.cs:29`) — 65.0-87.5
- `HireThief` (`Projects/UOContent/Mobiles/Hireables/HireThief.cs:29`) — 65.0-87.5
- `JukaLord` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/JukaLord.cs:32`) — 80.1-100.0
- `HealerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/HealerGuildmaster.cs:13`) — 90.0-100.0
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

- **[AOS]** Healing: 45
- **[None]** Healing: 45
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:384`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:750`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:812`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:877`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:938`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:982`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1024`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1338`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1378`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1418`)
---

## Code Locations

- `38 files` — total code references in UOContent

---

## Expansion Notes

Healing is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
