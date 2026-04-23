# Mining

**Mining** is used to extract ore from rock veins. It shares the highest Strength gain rate with Lumberjacking and Carpentry.

**Title:** Miner | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Miner |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
| Str Scale | 0.2 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 2 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.2 Str per point of Strength
- **Gain rates:** 2.0 Str gain per use (highest Str gain, tied with Lumberjacking and Carpentry)
- **Primary use:** Extracting ore from rocks

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `HarborMaster` (`Projects/UOContent/Mobiles/Townfolk/HarborMaster.cs:13`) — 36-68
- `MinerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/MinerGuildmaster.cs:12`) — 90.0-100.0
- `Miner` (`Projects/UOContent/Mobiles/Vendors/NPC/Miner.cs:15`) — 65.0-88.0
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

- `definition` (`Projects/UOContent/Engines/Harvest/Mining.cs:92`)
- `definition` (`Projects/UOContent/Engines/Harvest/Mining.cs:231`)
---

## Professions

- **[AOS]** Mining: 5
- **[None]** Mining: 5
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:615`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1298`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1797`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1841`)
---

## Code Locations

- `11 files` — total code references in UOContent

---

## Expansion Notes

Mining is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Harvesting](../systems/harvesting.md) — Resource gathering
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
