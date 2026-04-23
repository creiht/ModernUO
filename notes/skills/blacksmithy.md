# Blacksmithy

**Blacksmithy** is the skill used to forge metal items such as weapons, armor, and tools. It is a core crafting skill with the highest Strength gain rate among all crafting skills.

**Title:** Blacksmith | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Blacksmith |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
| Str Scale | 0.1 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 1 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

- **Skill check:** `CheckSkill(Blacksmith, 0.0, 100.0)`
- **Stat scaling:** +0.1 Str per point of Strength
- **Gain rates:** 1.0 Str gain per use (highest Str gain of any skill)
- **Primary use:** Crafting weapons and armor from ore resources

Blacksmithy is the most Strength-reliant crafting skill and one of the two skills with the highest Str gain rate (tied with Carpentry and Mining at 2.0, but Blacksmithy has the highest Str scale at 0.1).

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `Armorer` (`Projects/UOContent/Mobiles/Vendors/NPC/Armorer.cs:16`) — 60.0-83.0
- `Blacksmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Blacksmith.cs:18`) — 65.0-88.0
- `BlacksmithGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/BlacksmithGuildmaster.cs:13`) — 90.0-100.0
- `IronWorker` (`Projects/UOContent/Mobiles/Vendors/NPC/IronWorker.cs:16`) — 65.0-88.0
- `Weaponsmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Weaponsmith.cs:18`) — 65.0-88.0
---

## Spells

None
---

## Crafting

- `DefBlacksmithy` (`Projects/UOContent/Engines/Craft/DefBlacksmithy.cs:20`) — MainSkill, 179 items
---

## Weapon Abilities

None
---

## Harvest Systems

None
---

## Professions

- **[AOS]** Blacksmith: 50
- **[None]** Blacksmith: 50
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:639`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1293`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1792`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1836`)
- `NewHavenTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenTraining.cs:467`)
---

## Code Locations

- `17 files` — total code references in UOContent

---

## Expansion Notes

Blacksmithy is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Items: Armor](../items/armor.md) — Armor mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
