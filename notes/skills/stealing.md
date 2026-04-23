# Stealing

**Stealing** allows players to take items from other creatures' backpacks.

**Title:** Pickpocket | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Pickpocket |
| Primary Stat | Dexterity |
| Secondary Stat | Intelligence |
| Str Scale | 0 |
| Dex Scale | 0.1 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 1 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.1 Dex per point of Dexterity
- **Gain rates:** 1.0 Dex gain per use
- **Primary use:** Taking items from targets

---

---

## Weapons

None
---

## Items

- **Tool/Check:** `PickpocketDips` (`Projects/UOContent/Items/Addons/PickpocketDips.cs:61`)
---

## NPCs

- `HireSailor` (`Projects/UOContent/Mobiles/Hireables/HireSailor.cs:26`) — 66.0-97.5
- `HireThief` (`Projects/UOContent/Mobiles/Hireables/HireThief.cs:26`) — 66.0-97.5
- `ThiefGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/ThiefGuildmaster.cs:18`) — 90.0-100.0
---

## Spells

- `AnimalForm` (`Projects/UOContent/Spells/Ninjitsu/AnimalForm.cs:237`) — skill mod
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

None
---

## Code Locations

- `Projects/UOContent/Skills/Stealing.cs` — skill handler implementation
- `15 files` — total code references in UOContent

---

## Expansion Notes

Stealing is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
