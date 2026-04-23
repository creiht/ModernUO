# Archery

**Archery** is the skill used with bows and crossbows. It is the profession skill for the Archer starting profession.

**Title:** Archer | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Archer |
| Primary Stat | Dexterity |
| Secondary Stat | Strength |
| Str Scale | 0.025 |
| Dex Scale | 0.075 |
| Int Scale | 0 |
| Str Gain | 0.25 |
| Dex Gain | 0.75 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.025 Str and +0.075 Dex per point of those stats
- **Gain rates:** 0.25 Str gain, 0.75 Dex gain per use
- **StatTotal:** 10
- **Profession:** Archer starting profession
- **Weapon types:** Ranged damage

Archery is the most Dexterity-reliant ranged combat skill and is available from character creation.

---

---

## Weapons

- `BaseRanged` (`Projects/UOContent/Items/Weapons/Ranged/BaseRanged.cs:37`)
---

## Items

- **Tool/Check:** `BaseShield` (`Projects/UOContent/Items/Shields/BaseShield.cs:65`)
---

## NPCs

- `HireBard` (`Projects/UOContent/Mobiles/Hireables/HireBard.cs:29`) — 36-67
- `HireBardArcher` (`Projects/UOContent/Mobiles/Hireables/HireBardArcher.cs:29`) — 36-67
- `HireRanger` (`Projects/UOContent/Mobiles/Hireables/HireRanger.cs:28`) — 66-97
- `HireRangerArcher` (`Projects/UOContent/Mobiles/Hireables/HireRangerArcher.cs:28`) — 66-97
- `JukaLord` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/JukaLord.cs:31`) — 95.1-100.0
- `MeerCaptain` (`Projects/UOContent/Mobiles/Monsters/LBR/Meers/MeerCaptain.cs:34`) — 90.1-100.0
- `Bard` (`Projects/UOContent/Mobiles/Vendors/NPC/Bard.cs:18`) — 36.0-68.0
- `BardGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/BardGuildmaster.cs:11`) — 80.0-100.0
- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:16`) — 90.0-100.0
- `Ranger` (`Projects/UOContent/Mobiles/Vendors/NPC/Ranger.cs:18`) — 65.0-88.0
- `Thief` (`Projects/UOContent/Mobiles/Vendors/NPC/Thief.cs:18`) — 65.0-88.0
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:186`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:98`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:814`)
- `NewHavenTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenTraining.cs:280`)
---

## Code Locations

- `34 files` — total code references in UOContent

---

## Expansion Notes

Archery is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
