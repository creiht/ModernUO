# Cooking

**Cooking** is the skill used to prepare food items from raw ingredients. It has the highest Intelligence gain rate among all crafting skills and the highest total StatTotal.

**Title:** Chef | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Chef |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0.2 |
| Int Scale | 0.3 |
| Str Gain | 0 |
| Dex Gain | 2 |
| Int Gain | 3 |

---

## Mechanics

- **Stat scaling:** +0.2 Dex and +0.3 Int per point of those stats
- **Gain rates:** 2.0 Dex gain, 3.0 Int gain per use (highest Int gain of any skill)
- **StatTotal:** 50 (highest among all skills)
- **Primary use:** Preparing food from raw meat and other ingredients

Cooking is the most Intelligence-reliant crafting skill and provides the highest Int gain rate, making it an excellent skill for intelligence-based characters.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `Baker` (`Projects/UOContent/Mobiles/Vendors/NPC/Baker.cs:14`) — 75.0-98.0
- `Cook` (`Projects/UOContent/Mobiles/Vendors/NPC/Cook.cs:15`) — 90.0-100.0
- `Farmer` (`Projects/UOContent/Mobiles/Vendors/NPC/Farmer.cs:17`) — 36.0-68.0
- `Furtrader` (`Projects/UOContent/Mobiles/Vendors/NPC/Furtrader.cs:17`) — 45.0-68.0
- `Herbalist` (`Projects/UOContent/Mobiles/Vendors/NPC/Herbalist.cs:15`) — 80.0-100.0
---

## Spells

None
---

## Crafting

- `DefCooking` (`Projects/UOContent/Engines/Craft/DefCooking.cs:17`) — MainSkill, 46 items
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

None
---

## Code Locations

- `12 files` — total code references in UOContent

---

## Expansion Notes

Cooking is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Food](../items/food.md) — Food items and effects
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
