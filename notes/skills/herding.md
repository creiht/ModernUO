# Herding

**Herding** is used to control and move groups of tamed animals. It allows shepherds to direct their animal companions over distances.

**Title:** Shepherd | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Shepherd |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0.1625 |
| Dex Scale | 0.0625 |
| Int Scale | 0.025 |
| Str Gain | 1.625 |
| Dex Gain | 0.625 |
| Int Gain | 0.25 |

---

## Mechanics

- **Stat scaling:** +0.1625 Str, +0.0625 Dex, +0.025 Int per point of those stats (high StatTotal)
- **Gain rates:** 1.625 Str gain, 0.625 Dex gain, 0.25 Int gain per use
- **StatTotal:** 25 (highest among utility skills)
- **Primary use:** Moving groups of tamed animals

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:20`) — 36.0-68.0
- `Rancher` (`Projects/UOContent/Mobiles/Vendors/NPC/Rancher.cs:16`) — 64.0-100.0
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

- `5 files` — total code references in UOContent

---

## Expansion Notes

Herding is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Creatures: Animals](../creatures/animals.md) — Tameable fauna and mounts
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
