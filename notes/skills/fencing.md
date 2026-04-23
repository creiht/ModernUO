# Fencing

**Fencing** is the skill used with fencing-type weapons (rapiers, long swords, etc.). It has the highest Dexterity scale among all melee weapon skills.

**Title:** Fencer | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Fencer |
| Primary Stat | Dexterity |
| Secondary Stat | Strength |
| Str Scale | 0.045 |
| Dex Scale | 0.055 |
| Int Scale | 0 |
| Str Gain | 0.45 |
| Dex Gain | 0.55 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.045 Str and +0.055 Dex per point of those stats
- **Gain rates:** 0.45 Str gain, 0.55 Dex gain per use
- **StatTotal:** 10
- **Profession:** Fencer starting profession
- **Weapon types:** Piercing and Slashing damage

Fencing is the most Dexterity-reliant melee weapon skill, making it ideal for Dexterity-based characters.

---

---

## Weapons

- `Dagger` (`Projects/UOContent/Items/Weapons/Knives/Dagger.cs:33`)
- `AssassinSpike` (`Projects/UOContent/Items/Weapons/ML Weapons/AssassinSpike.cs:31`)
- `Leafblade` (`Projects/UOContent/Items/Weapons/ML Weapons/Leafblade.cs:31`)
- `WarCleaver` (`Projects/UOContent/Items/Weapons/ML Weapons/WarCleaver.cs:36`)
- `Kama` (`Projects/UOContent/Items/Weapons/SE Weapons/Kama.cs:33`)
- `Lajatang` (`Projects/UOContent/Items/Weapons/SE Weapons/Lajatang.cs:33`)
- `Sai` (`Projects/UOContent/Items/Weapons/SE Weapons/Sai.cs:34`)
- `Tekagi` (`Projects/UOContent/Items/Weapons/SE Weapons/Tekagi.cs:33`)
- `BaseSpear` (`Projects/UOContent/Items/Weapons/SpearsAndForks/BaseSpear.cs:17`)
- `BloodBlade` (`Projects/UOContent/Items/Weapons/Swords/BloodBlade.cs:25`)
- `Kryss` (`Projects/UOContent/Items/Weapons/Swords/Kryss.cs:36`)
- `Lance` (`Projects/UOContent/Items/Weapons/Swords/Lance.cs:36`)
---

## Items

None
---

## NPCs

- `HireRanger` (`Projects/UOContent/Mobiles/Hireables/HireRanger.cs:31`) — 15-37
- `HireRangerArcher` (`Projects/UOContent/Mobiles/Hireables/HireRangerArcher.cs:31`) — 15-37
- `HireSailor` (`Projects/UOContent/Mobiles/Hireables/HireSailor.cs:31`) — 65.0-87.5
- `HireThief` (`Projects/UOContent/Mobiles/Hireables/HireThief.cs:31`) — 65.0-87.5
- `ChaosDragoonElite` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/ChaosDragoonElite.cs:39`) — 85.1-100
- `EliteNinja` (`Projects/UOContent/Mobiles/Monsters/SE/EliteNinja.cs:41`) — 95.0-120.0
- `Ninja` (`Projects/UOContent/Mobiles/Townfolk/Ninja.cs:15`) — 64.0-80.0
- `Blacksmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Blacksmith.cs:19`) — 60.0-83.0
- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:19`) — 36.0-68.0
- `ThiefGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/ThiefGuildmaster.cs:19`) — 75.0-98.0
- `WarriorGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/WarriorGuildmaster.cs:17`) — 60.0-83.0
- `IronWorker` (`Projects/UOContent/Mobiles/Vendors/NPC/IronWorker.cs:17`) — 60.0-83.0
- `KeeperOfChivalry` (`Projects/UOContent/Mobiles/Vendors/NPC/KeeperOfChivalry.cs:15`) — 75.0-85.0
- `Weaponsmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Weaponsmith.cs:19`) — 45.0-68.0

*... and 11 more NPCs with this skill*
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:185`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:122`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:879`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1458`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1511`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1545`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1589`)
---

## Code Locations

- `50 files` — total code references in UOContent

---

## Expansion Notes

Fencing is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
