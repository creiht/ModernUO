# Ninjitsu

**Ninjitsu** is a combat technique skill that provides special abilities for ninja characters. It is expansion-gated to **SE (Samurai Empire)** and is the profession skill for the Ninja starting profession.

**Title:** Ninja | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Ninja |
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

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SE expansion
- **Profession:** Ninja starting profession
- **Primary use:** Ninja-specific combat techniques

Ninjitsu provides abilities such as Hide Again, Speed, and Swashbuckler that enhance stealth-based combat.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `EliteNinja` (`Projects/UOContent/Mobiles/Monsters/SE/EliteNinja.cs:44`) — 95.0-120.0
- `Ninja` (`Projects/UOContent/Mobiles/Townfolk/Ninja.cs:17`) — 60.0-80.0
---

## Spells

- `NinjaSpell` (`Projects/UOContent/Spells/Ninjitsu/NinjaSpell.cs:14`) — CastSkill
- `NinjaSpell` (`Projects/UOContent/Spells/Ninjitsu/NinjaSpell.cs:15`) — DamageSkill
---

## Crafting

None
---

## Weapon Abilities

- `DualWield` (`Projects/UOContent/Items/Weapons/Abilities/DualWield.cs:16`) — secondary skill requirement
- `TalonStrike` (`Projects/UOContent/Items/Weapons/Abilities/TalonStrike.cs:17`) — secondary skill requirement
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:153`) — skill reference
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:192`) — skill reference
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:287`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:431`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1460`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1513`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1547`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1592`)
---

## Code Locations

- `18 files` — total code references in UOContent

---

## Expansion Notes

Ninjitsu requires the SE (Samurai Empire) expansion.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Spells: Ninjitsu](../spells/ninjitsu.md) — Ninjitsu spells and abilities
- [Expansions: Timeline](../expansions/timeline.md) — Expansion details
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
