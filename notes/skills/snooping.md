# Snooping

**Snooping** allows players to search containers and creatures for hidden items.

**Title:** Spy | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Spy |
| Primary Stat | Dexterity |
| Secondary Stat | Intelligence |
| Str Scale | 0 |
| Dex Scale | 0.25 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 2.5 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.25 Dex per point of Dexterity
- **Gain rates:** 2.5 Dex gain per use (highest Dex gain of any skill)
- **Primary use:** Searching containers for items

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `HireSailor` (`Projects/UOContent/Mobiles/Hireables/HireSailor.cs:35`) — 65-87
- `HireThief` (`Projects/UOContent/Mobiles/Hireables/HireThief.cs:35`) — 65-87
- `ThiefGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/ThiefGuildmaster.cs:16`) — 90.0-100.0
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

None
---

## Code Locations

- `Projects/UOContent/Skills/Snooping.cs` — skill handler implementation
- `8 files` — total code references in UOContent

---

## Expansion Notes

Snooping is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Items: Containers](../items/containers.md) — Container mechanics
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
