# Bushido

**Bushido** is a combat technique skill that provides special abilities for samurai characters. It is expansion-gated to **SE (Samurai Empire)** and is the profession skill for the Samurai starting profession.

**Title:** Samurai | **Primary Stat:** Strength | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Samurai |
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
- **Expansion gate:** Requires SE expansion
- **Profession:** Samurai starting profession
- **Primary use:** Samurai-specific combat techniques

Bushido provides abilities such as Hono (Fire), Kitsui (Speed), and Chi (Spirit) that enhance combat effectiveness.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `Samurai` (`Projects/UOContent/Mobiles/Townfolk/Samurai.cs:17`) — 64.0-85.0
---

## Spells

- `SamuraiSpell` (`Projects/UOContent/Spells/Bushido/SamuraiSpell.cs:16`) — CastSkill
- `SamuraiSpell` (`Projects/UOContent/Spells/Bushido/SamuraiSpell.cs:17`) — DamageSkill
---

## Crafting

None
---

## Weapon Abilities

- `NerveStrike` (`Projects/UOContent/Items/Weapons/Abilities/NerveStrike.cs:13`) — secondary skill requirement
- `RidingSwipe` (`Projects/UOContent/Items/Weapons/Abilities/RidingSwipe.cs:17`) — secondary skill requirement
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:153`) — skill reference
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:191`) — skill reference
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:286`) — skill reference
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:524`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1635`)
---

## Code Locations

- `15 files` — total code references in UOContent

---

## Expansion Notes

Bushido requires the SE (Samurai Empire) expansion.

---

## Cross-References

- [Combat Skills](combat-skills.md) — Full list of all combat skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Spells: Bushido](../spells/bushido.md) — Bushido spells and abilities
- [Expansions: Timeline](../expansions/timeline.md) — Expansion details
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Professions and starting skills
