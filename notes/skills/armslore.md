# ArmsLore

**Arms Lore** provides knowledge about weapons, increasing damage with weapon types and revealing weapon properties.

**Title:** Weapon Master | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Weapon Master |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0.75 |
| Dex Gain | 0.15 |
| Int Gain | 0.1 |

---

## Mechanics

- **Stat scaling:** +0.75 Str gain, +0.15 Dex gain, +0.1 Int gain per use
- **Primary use:** Enhancing weapon effectiveness

---

---

## Weapons

None
---

## Items

- **Tool/Check:** `BaseArmor` (`Projects/UOContent/Items/Armor/BaseArmor.cs:631`)
- **Tool/Check:** `BaseWeapon` (`Projects/UOContent/Items/Weapons/BaseWeapon.cs:711`)
---

## NPCs

- `Merchant` (`Projects/UOContent/Mobiles/Townfolk/Merchant.cs:14`) — 55-78
- `Samurai` (`Projects/UOContent/Mobiles/Townfolk/Samurai.cs:16`) — 64.0-80.0
- `Armorer` (`Projects/UOContent/Mobiles/Vendors/NPC/Armorer.cs:15`) — 64.0-100.0
- `Blacksmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Blacksmith.cs:17`) — 36.0-68.0
- `BlacksmithGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/BlacksmithGuildmaster.cs:12`) — 65.0-88.0
- `MerchantGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/MerchantGuildmaster.cs:12`) — 85.0-100.0
- `WarriorGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/WarriorGuildmaster.cs:11`) — 75.0-98.0
- `IronWorker` (`Projects/UOContent/Mobiles/Vendors/NPC/IronWorker.cs:15`) — 36.0-68.0
- `Weaponsmith` (`Projects/UOContent/Mobiles/Vendors/NPC/Weaponsmith.cs:17`) — 64.0-100.0
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

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1292`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1791`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1835`)
---

## Code Locations

- `Projects/UOContent/Skills/ArmsLore.cs` — skill handler implementation
- `17 files` — total code references in UOContent

---

## Expansion Notes

Arms Lore is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
