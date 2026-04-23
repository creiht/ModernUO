# Alchemy

**Alchemy** is the skill used to create potions and poisons. It is one of the earliest crafting skills, available from character creation.

**Title:** Alchemist | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Alchemist |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0.05 |
| Int Scale | 0.05 |
| Str Gain | 0 |
| Dex Gain | 0.5 |
| Int Gain | 0.5 |

---

## Mechanics

- **Skill check:** `CheckSkill(Alchemy, 0.0, 100.0)`
- **Stat scaling:** +0.05 Dex and +0.05 Int per point of those stats
- **Gain rates:** 0.5 Dex gain, 0.5 Int gain per use
- **Primary use:** Brewing potions from harvested reagents

Alchemy is closely tied to the crafting system and the poisons system. Alchemical items include healing potions, strength potions, and various magical concoctions.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `Alchemist` (`Projects/UOContent/Mobiles/Vendors/NPC/Alchemist.cs:15`) — 85.0-100.0
- `Furtrader` (`Projects/UOContent/Mobiles/Vendors/NPC/Furtrader.cs:15`) — 
- `Glassblower` (`Projects/UOContent/Mobiles/Vendors/NPC/Glassblower.cs:16`) — 85.0-100.0
- `HairStylist` (`Projects/UOContent/Mobiles/Vendors/NPC/HairStylist.cs:14`) — 80.0-100.0
- `Herbalist` (`Projects/UOContent/Mobiles/Vendors/NPC/Herbalist.cs:14`) — 80.0-100.0
---

## Spells

None
---

## Crafting

- `DefAlchemy` (`Projects/UOContent/Engines/Craft/DefAlchemy.cs:19`) — MainSkill, 25 items
- `DefGlassblowing` (`Projects/UOContent/Engines/Craft/DefGlassblowing.cs:18`) — MainSkill, 14 items
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

- `GainSkillObjective` (`Projects/UOContent/Engines/ML Quests/Objectives/GainSkillObjective.cs:20`) — objective threshold: ?
---

## Code Locations

- `18 files` — total code references in UOContent

---

## Expansion Notes

Alchemy is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Systems: Poisons](../systems/poisons.md) — Poison system
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
