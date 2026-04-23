# Tinkering

**Tinkering** is the skill used to modify and repair items, including tools, weapons, and armor. It has moderate stat scaling across all three stats.

**Title:** Tinker | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Tinker |
| Primary Stat | Dexterity |
| Secondary Stat | Intelligence |
| Str Scale | 0.05 |
| Dex Scale | 0.02 |
| Int Scale | 0.03 |
| Str Gain | 0.5 |
| Dex Gain | 0.2 |
| Int Gain | 0.3 |

---

## Mechanics

- **Stat scaling:** +0.05 Str, +0.02 Dex, +0.03 Int per point of those stats
- **Gain rates:** 0.5 Str gain, 0.2 Dex gain, 0.3 Int gain per use
- **Primary use:** Modifying and repairing items, including tool upgrades

Tinkering is closely tied to the tools system and the crafting system. It allows players to enhance tools and create specialized items.

---

---

## Weapons

None
---

## Items

- **Tool/Check:** `LockableContainer` (`Projects/UOContent/Items/Containers/LockableContainer.cs:22`)
---

## NPCs

- `GolemCrafter` (`Projects/UOContent/Mobiles/Vendors/NPC/GolemCrafter.cs:16`) — 64.0-100.0
- `TinkerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/TinkerGuildmaster.cs:16`) — 90.0-100.0
- `Tinker` (`Projects/UOContent/Mobiles/Vendors/NPC/Tinker.cs:16`) — 64.0-100.0
---

## Spells

None
---

## Crafting

- `DefTinkering` (`Projects/UOContent/Engines/Craft/DefTinkering.cs:34`) — MainSkill, 89 items
---

## Weapon Abilities

None
---

## Harvest Systems

None
---

## Professions

- **[AOS]** Tinkering: 45
- **[None]** Tinkering: 45
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:336`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1297`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1795`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1839`)
---

## Code Locations

- `15 files` — total code references in UOContent

---

## Expansion Notes

Tinkering is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Tools](../items/tools.md) — Tool mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
