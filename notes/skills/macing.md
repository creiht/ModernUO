# Macing

**Mace Fighting** is the skill used with mace-type weapons (maces, pickaxes, flails, etc.). It has the highest Strength scale among all combat skills.

**Title:** Armsman | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Armsman |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
| Str Scale | 0.09 |
| Dex Scale | 0.01 |
| Int Scale | 0 |
| Str Gain | 0.9 |
| Dex Gain | 0.1 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** +0.09 Str and +0.01 Dex per point of those stats (highest Str scale of any combat skill)
- **Gain rates:** 0.9 Str gain, 0.1 Dex gain per use
- **StatTotal:** 10
- **Weapon types:** Slashing, Bashing, and Piercing damage

Mace Fighting is the most Strength-reliant combat skill, making it ideal for pure Strength characters.

---

---

## Weapons

- `WarAxe` (`Projects/UOContent/Items/Weapons/Axes/WarAxe.cs:37`)
- `BaseBashing` (`Projects/UOContent/Items/Weapons/Maces/BaseBashing.cs:16`)
- `BaseStaff` (`Projects/UOContent/Items/Weapons/Staves/BaseStaff.cs:15`)
---

## Items

None
---

## NPCs

- `HireFighter` (`Projects/UOContent/Mobiles/Hireables/HireFighter.cs:30`) — 36-67
- `HireMage` (`Projects/UOContent/Mobiles/Hireables/HireMage.cs:31`) — 100-125
- `ChaosDragoonElite` (`Projects/UOContent/Mobiles/Monsters/LBR/Jukas/ChaosDragoonElite.cs:40`) — 85.1-100
- `EliteNinja` (`Projects/UOContent/Mobiles/Monsters/SE/EliteNinja.cs:42`) — 95.0-120.0
- `Ninja` (`Projects/UOContent/Mobiles/Townfolk/Ninja.cs:16`) — 64.0-80.0
- `Blacksmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Blacksmith.cs:20`) — 61.0-93.0
- `BlacksmithGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/BlacksmithGuildmaster.cs:14`) — 36.0-68.0
- `MageGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/MageGuildmaster.cs:18`) — 36.0-68.0
- `WarriorGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/WarriorGuildmaster.cs:16`) — 60.0-83.0
- `IronWorker` (`Projects/UOContent/Mobiles/Vendors/NPC/IronWorker.cs:18`) — 61.0-93.0
- `KeeperOfChivalry` (`Projects/UOContent/Mobiles/Vendors/NPC/KeeperOfChivalry.cs:16`) — 75.0-85.0
- `Weaponsmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Weaponsmith.cs:20`) — 45.0-68.0

*... and 14 more NPCs with this skill*
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:184`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:75`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:752`)
---

## Code Locations

- `46 files` — total code references in UOContent

---

## Expansion Notes

Mace Fighting is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
