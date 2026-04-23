# Parry

**Parrying** is the skill used to reduce incoming melee damage. It is the profession skill for the Duelist starting profession.

**Title:** Duelist | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Duelist |
| Primary Stat | Dexterity |
| Secondary Stat | Strength |
| Str Scale | 0.075 |
| Dex Scale | 0.025 |
| Int Scale | 0 |
| Str Gain | 0.75 |
| Dex Gain | 0.25 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.075 Str and +0.025 Dex per point of those stats
- **Gain rates:** 0.75 Str gain, 0.25 Dex gain per use
- **StatTotal:** 10
- **Profession:** Duelist starting profession (also known as Paladin in some contexts)
- **Primary use:** Reducing incoming melee damage

Parrying is the only combat skill with a non-zero Str scale among the defensive techniques.

---

---

## Weapons

None
---

## Items

- **Tool/Check:** `BaseShield` (`Projects/UOContent/Items/Shields/BaseShield.cs:63`)
- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:1540`)
- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:1581`)
---

## NPCs

- `HireBard` (`Projects/UOContent/Mobiles/Hireables/HireBard.cs:30`) — 45-60
- `HireBardArcher` (`Projects/UOContent/Mobiles/Hireables/HireBardArcher.cs:30`) — 45-60
- `HireFighter` (`Projects/UOContent/Mobiles/Hireables/HireFighter.cs:29`) — 60-82
- `HirePaladin` (`Projects/UOContent/Mobiles/Hireables/HirePaladin.cs:32`) — 45.0-60.5
- `HireRanger` (`Projects/UOContent/Mobiles/Hireables/HireRanger.cs:27`) — 45-60
- `HireRangerArcher` (`Projects/UOContent/Mobiles/Hireables/HireRangerArcher.cs:27`) — 45-60
- `HireSailor` (`Projects/UOContent/Mobiles/Hireables/HireSailor.cs:32`) — 45.0-60.5
- `HireThief` (`Projects/UOContent/Mobiles/Hireables/HireThief.cs:32`) — 45.0-60.5
- `Ninja` (`Projects/UOContent/Mobiles/Townfolk/Ninja.cs:18`) — 64.0-80.0
- `Noble` (`Projects/UOContent/Mobiles/Townfolk/Noble.cs:14`) — 80.0-100.0
- `Samurai` (`Projects/UOContent/Mobiles/Townfolk/Samurai.cs:18`) — 64.0-80.0
- `Blacksmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Blacksmith.cs:23`) — 61.0-93.0
- `BlacksmithGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/BlacksmithGuildmaster.cs:15`) — 36.0-68.0
- `WarriorGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/WarriorGuildmaster.cs:12`) — 85.0-100.0
- `IronWorker` (`Projects/UOContent/Mobiles/Vendors/NPC/IronWorker.cs:21`) — 61.0-93.0
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:187`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:194`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:749`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:810`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:876`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:937`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:981`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1023`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1337`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1377`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1417`)
---

## Code Locations

- `27 files` — total code references in UOContent

---

## Expansion Notes

Parrying is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
