# Chivalry

**Chivalry** is a magical combat skill that provides holy abilities for paladin characters. It is expansion-gated to **AOS (Age of Shadows)** and is the profession skill for the Paladin starting profession.

**Title:** Paladin | **Primary Stat:** Strength | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Paladin |
| Primary Stat | Strength |
| Secondary Stat | Intelligence |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires AOS expansion
- **Profession:** Paladin starting profession
- **Primary use:** Holy-themed magical combat abilities

Chivalry provides abilities such as Divine Fury, Enemy of One, Honorific Bassinas, and Divine Fury that enhance paladin combat effectiveness.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `HirePaladin` (`Projects/UOContent/Mobiles/Hireables/HirePaladin.cs:33`) — 85-100
- `KeeperOfChivalry` (`Projects/UOContent/Mobiles/Vendors/NPC/KeeperOfChivalry.cs:18`) — 100.0
---

## Spells

- `Spell` (`Projects/UOContent/Spells/Base/Spell.cs:705`) — CastSkill
- `PaladinSpell` (`Projects/UOContent/Spells/Chivalry/PaladinSpell.cs:16`) — CastSkill
- `PaladinSpell` (`Projects/UOContent/Spells/Chivalry/PaladinSpell.cs:17`) — DamageSkill
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

- **[AOS]** Chivalry: 50
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:27`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:668`)
---

## Code Locations

- `15 files` — total code references in UOContent

---

## Expansion Notes

Chivalry requires the AOS (Age of Shadows) expansion.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Spells: Chivalry](../spells/chivalry.md) — Chivalry spells
- [Expansions: Timeline](../expansions/timeline.md) — Expansion details
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
