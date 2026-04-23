# Swords

**Swords** is the skill used with sword-type weapons (broadswords, katanas, scimitars, etc.). It is the profession skill for the Swordsman starting profession.

**Title:** Swordsman | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Swordsman |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
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
- **Profession:** Swordsman starting profession
- **Weapon types:** Slashing, Bashing, and Piercing damage based on weapon type

Swords is one of the most balanced melee combat skills, with moderate Strength and Dexterity scaling. It is available from character creation.

---

---

## Weapons

- `BaseAxe` (`Projects/UOContent/Items/Weapons/Axes/BaseAxe.cs:33`)
- `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:251`)
- `BaseKnife` (`Projects/UOContent/Items/Weapons/Knives/BaseKnife.cs:16`)
- `BasePoleArm` (`Projects/UOContent/Items/Weapons/PoleArms/BasePoleArm.cs:28`)
- `BladedStaff` (`Projects/UOContent/Items/Weapons/SpearsAndForks/BladedStaff.cs:33`)
- `BaseSword` (`Projects/UOContent/Items/Weapons/Swords/BaseSword.cs:13`)
---

## Items

- **Skill Bonus:** `CaptainJohnsHat` (`Projects/UOContent/Items/Champion Artifacts/Shared/CaptainJohnsHat.cs:17`)
---

## NPCs

- `EvilHealer` (`Projects/UOContent/Mobiles/Healers/EvilHealer.cs:17`) — 80.0-100.0
- `Healer` (`Projects/UOContent/Mobiles/Healers/Healer.cs:20`) — 80.0-100.0
- `HireBard` (`Projects/UOContent/Mobiles/Hireables/HireBard.cs:28`) — 45-67
- `HireBardArcher` (`Projects/UOContent/Mobiles/Hireables/HireBardArcher.cs:28`) — 45-67
- `HireFighter` (`Projects/UOContent/Mobiles/Hireables/HireFighter.cs:28`) — 64-100
- `HirePaladin` (`Projects/UOContent/Mobiles/Hireables/HirePaladin.cs:26`) — 66.0-97.5
- `HirePeasant` (`Projects/UOContent/Mobiles/Hireables/HirePeasant.cs:28`) — 5-27
- `HireRanger` (`Projects/UOContent/Mobiles/Hireables/HireRanger.cs:30`) — 35-57
- `HireRangerArcher` (`Projects/UOContent/Mobiles/Hireables/HireRangerArcher.cs:30`) — 35-57
- `OrcCaptain` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Melee/OrcCaptain.cs:34`) — 70.1-95.0
- `OrcishLord` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Melee/OrcishLord.cs:33`) — 60.1-85.0
- `ChaosDragoonElite` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/ChaosDragoonElite.cs:38`) — 72.5-95.0
- `JukaLord` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/JukaLord.cs:34`) — 90.1-100.0
- `MeerCaptain` (`Projects/UOContent/Mobiles/Monsters/LBR/Meers/MeerCaptain.cs:36`) — 90.1-100.0
- `EliteNinja` (`Projects/UOContent/Mobiles/Monsters/SE/EliteNinja.cs:43`) — 95.0-120.0
- `Ninja` (`Projects/UOContent/Mobiles/Townfolk/Ninja.cs:20`) — 64.0-85.0
- `Noble` (`Projects/UOContent/Mobiles/Townfolk/Noble.cs:15`) — 80.0-100.0
- `Samurai` (`Projects/UOContent/Mobiles/Townfolk/Samurai.cs:19`) — 64.0-85.0
- `Bard` (`Projects/UOContent/Mobiles/Vendors/NPC/Bard.cs:19`) — 36.0-68.0
- `Blacksmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Blacksmith.cs:21`) — 60.0-83.0
- `BardGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/BardGuildmaster.cs:16`) — 80.0-100.0
- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:21`) — 45.0-68.0

*... and 18 more NPCs with this skill*
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:183`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:170`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:665`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:940`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:984`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1026`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1296`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1340`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1380`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1420`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1634`)
---

## Code Locations

- `72 files` — total code references in UOContent

---

## Expansion Notes

Swords is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
