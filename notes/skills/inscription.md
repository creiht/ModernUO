# Inscription

**Inscription** is the skill used to copy written pages from one book to another. It is the skill used by scribes and spellcasters to transfer spellbooks.

**Title:** Scribe | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Scribe |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0.02 |
| Int Scale | 0.08 |
| Str Gain | 0 |
| Dex Gain | 0.2 |
| Int Gain | 0.8 |

---

## Mechanics

- **Skill check:** `CheckTargetSkill(Inscribe, destinationBook, 0.0, 50.0)`
- **Stat scaling:** +0.02 Dex and +0.08 Int per point of those stats
- **Gain rates:** 0.2 Dex gain, 0.8 Int gain per use
- **Target range:** 8 tiles
- **Primary use:** Copying written pages between books

The Inscription skill check uses a lower difficulty range (0-50) compared to most other skills, reflecting the accessible nature of basic book copying.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `MageGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/MageGuildmaster.cs:13`) — 65.0-88.0
---

## Spells

None
---

## Crafting

- `DefInscription` (`Projects/UOContent/Engines/Craft/DefInscription.cs:36`) — MainSkill, 10 items
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

- `MistakenIdentity` (`Projects/UOContent/Engines/ML Quests/Definitions/MistakenIdentity.cs:288`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:265`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:710`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1087`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1125`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1173`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1212`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1250`)
---

## Code Locations

- `Projects/UOContent/Skills/Inscribe.cs` — skill handler implementation
- `16 files` — total code references in UOContent

---

## Expansion Notes

Inscription is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Books](../items/books.md) — Book mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
