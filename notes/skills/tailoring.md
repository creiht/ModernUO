# Tailoring

**Tailoring** is the skill used to craft clothing and related items from fabric. It has a well-balanced stat distribution across all three stats.

**Title:** Tailor | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Tailor |
| Primary Stat | Dexterity |
| Secondary Stat | Intelligence |
| Str Scale | 0.0375 |
| Dex Scale | 0.1625 |
| Int Scale | 0.05 |
| Str Gain | 0.38 |
| Dex Gain | 1.63 |
| Int Gain | 0.5 |

---

## Mechanics

- **Stat scaling:** +0.0375 Str, +0.1625 Dex, +0.05 Int per point of those stats
- **Gain rates:** 0.38 Str gain, 1.63 Dex gain, 0.5 Int gain per use
- **StatTotal:** 25 (tied highest among crafting skills)
- **Primary use:** Crafting clothing items from fabric resources

Tailoring is the most Dexterity-reliant crafting skill and provides balanced stat gains across all three stats, making it a versatile choice for multi-stat characters.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `Cobbler` (`Projects/UOContent/Mobiles/Vendors/NPC/Cobbler.cs:14`) — 60.0-83.0
- `TailorGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/TailorGuildmaster.cs:11`) — 90.0-100.0
- `Tailor` (`Projects/UOContent/Mobiles/Vendors/NPC/Tailor.cs:16`) — 64.0-100.0
- `Tanner` (`Projects/UOContent/Mobiles/Vendors/NPC/Tanner.cs:14`) — 36.0-68.0
- `Weaver` (`Projects/UOContent/Mobiles/Vendors/NPC/Weaver.cs:16`) — 65.0-88.0
---

## Spells

None
---

## Crafting

- `DefTailoring` (`Projects/UOContent/Engines/Craft/DefTailoring.cs:25`) — MainSkill, 130 items
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

- `17 files` — total code references in UOContent

---

## Expansion Notes

Tailoring is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Clothing](../items/clothing.md) — Clothing mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
