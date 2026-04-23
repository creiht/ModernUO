# Stealth

**Stealth** allows players to move quietly while hidden, reducing the chance of detection by opponents.

**Title:** Rogue | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Rogue |
| Primary Stat | Dexterity |
| Secondary Stat | Intelligence |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

- **Prerequisite:** Must be hidden first (Hiding skill)
- **Hiding requirement:** 30+ skill (ML+), 50+ (SE), 80+ (base)
- **Armor penalty:** Armor Rating affects stealth check range
  - Check range: `(-20 + armorRating * 2)` to `(60 + armorRating * 2)` (AOS+)
  - Maximum allowed armor: 42 (AOS+) or 26 (pre-AOS)
- **Success:** Sets `AllowedStealthSteps = skill / 5` (AOS+) or `skill / 10` (pre-AOS)
- **Cooldown:** 10 seconds

---

---

## Weapons

None
---

## Items

- **Skill Bonus:** `EmbroideredOakLeafCloak` (`Projects/UOContent/Items/Champion Artifacts/Shared/EmbroideredOakLeafCloak.cs:14`)
---

## NPCs

- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:18`) — 60.0-83.0
- `ThiefGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/ThiefGuildmaster.cs:20`) — 85.0-100.0
---

## Spells

- `AnimalForm` (`Projects/UOContent/Spells/Ninjitsu/AnimalForm.cs:229`) — skill mod
---

## Crafting

None
---

## Weapon Abilities

- `ShadowStrike` (`Projects/UOContent/Items/Weapons/Abilities/ShadowStrike.cs:17`) — secondary skill requirement
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:189`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:477`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1459`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1512`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1546`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1591`)
---

## Code Locations

- `Projects/UOContent/Skills/Stealth.cs` — skill handler implementation
- `14 files` — total code references in UOContent

---

## Expansion Notes

Stealth is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
