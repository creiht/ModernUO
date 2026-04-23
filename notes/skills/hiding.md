# Hiding

**Hiding** allows players to conceal themselves from view. It is the foundation for stealth-based gameplay.

**Title:** Shade | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Shade |
| Primary Stat | Dexterity |
| Secondary Stat | Intelligence |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0.8 |
| Int Gain | 0.2 |

---

## Mechanics

- **Cooldown:** 10 seconds
- **Range calculation:** `range = min((100 - hidingValue) / 2 + 8, 18)`
- **Success condition:** `CheckSkill(Hiding, -bonus, 100 - bonus)`
- **House bonus:** +100 within own house (AOS+), +50 in any house (pre-AOS)

**Combat restriction:** Cannot hide while in combat (`Combatant != null`) or within range of creatures that have the player as Combatant. Range decreases with higher Hiding skill.

**On success:** Sets `m.Hidden = true` and disables Warmode. Cancels any active Invisibility spell timer.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `HireSailor` (`Projects/UOContent/Mobiles/Hireables/HireSailor.cs:34`) — 65-87
- `HireThief` (`Projects/UOContent/Mobiles/Hireables/HireThief.cs:34`) — 65-87
- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:13`) — 75.0-98.0
- `ThiefGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/ThiefGuildmaster.cs:14`) — 65.0-88.0
- `Ranger` (`Projects/UOContent/Mobiles/Vendors/NPC/Ranger.cs:17`) — 45.0-68.0
- `Thief` (`Projects/UOContent/Mobiles/Vendors/NPC/Thief.cs:17`) — 45.0-68.0
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

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:454`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1455`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1508`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1542`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1586`)
---

## Code Locations

- `Projects/UOContent/Skills/Hiding.cs` — skill handler implementation
- `12 files` — total code references in UOContent

---

## Expansion Notes

Hiding is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
