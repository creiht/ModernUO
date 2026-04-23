# Tracking

**Tracking** is used to follow tracks left by creatures or players in the world. It allows rangers to locate and pursue their quarry.

**Title:** Ranger | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Ranger |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0.125 |
| Int Scale | 0.125 |
| Str Gain | 0 |
| Dex Gain | 1.25 |
| Int Gain | 1.25 |

---

## Mechanics

- **Stat scaling:** +0.125 Dex and +0.125 Int per point of those stats
- **Gain rates:** 1.25 Dex gain, 1.25 Int gain per use
- **Primary use:** Following tracks in the world

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `Furtrader` (`Projects/UOContent/Mobiles/Vendors/NPC/Furtrader.cs:18`) — 36.0-68.0
- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:17`) — 90.0-100.0
- `Ranger` (`Projects/UOContent/Mobiles/Vendors/NPC/Ranger.cs:19`) — 65.0-88.0
- `Thief` (`Projects/UOContent/Mobiles/Vendors/NPC/Thief.cs:19`) — 65.0-88.0
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

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:501`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1457`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1510`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1544`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1588`)
---

## Code Locations

- `Projects/UOContent/Skills/Tracking/Tracking.cs` — skill handler implementation
- `8 files` — total code references in UOContent

---

## Expansion Notes

Tracking is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
