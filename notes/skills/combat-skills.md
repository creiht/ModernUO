# Combat Skills

Combat skills directly affect your character's offensive and defensive capabilities in combat. ModernUO features **11 combat skills**, spanning melee weapons, ranged weapons, and combat techniques.

Combat skills are defined in `Distribution/Data/skills.json` and grouped in `SkillsInfo.cs` (CombatSkills array, lines 22-41). All 11 skills share a uniform `GainFactor` of 4.

## Combat Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Archery | Archer | Dex | Str | 0.025 | 0.075 | 0 | 0.25 | 0.75 | 0 |
| Bushido | Samurai | Str | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Focus | Driven | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Fencing | Fencer | Dex | Str | 0.045 | 0.055 | 0 | 0.45 | 0.55 | 0 |
| Macing | Armsman | Str | Dex | 0.09 | 0.01 | 0 | 0.9 | 0.1 | 0 |
| Ninjitsu | Ninja | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Parry | Duelist | Dex | Str | 0.075 | 0.025 | 0 | 0.75 | 0.25 | 0 |
| Swords | Swordsman | Str | Dex | 0.075 | 0.025 | 0 | 0.75 | 0.25 | 0 |
| Tactics | Tactician | Str | Dex | 0 | 0 | 0 | 0 | 0 | 0 |
| Throwing | Bladeweaver | Dex | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Wrestling | Wrestler | Str | Dex | 0.09 | 0.01 | 0 | 0.9 | 0.1 | 0 |

## Individual Skill Pages

### Melee Weapons
- [Fencing](fencing.md) — Rapier and long sword combat
- [Macing](macing.md) — Mace, pickaxe, and flail combat
- [Swords](swords.md) — Broadsword, katana, and scimitar combat
- [Wrestling](wrestling.md) — Unarmed combat

### Ranged Weapons
- [Archery](archery.md) — Bow and crossbow combat
- [Throwing](throwing.md) — Thrown weapons (SA, Gargoyle only)

### Combat Techniques
- [Bushido](bushido.md) — Samurai combat techniques (SE)
- [Focus](focus.md) — Reduce stamina cost of weapon abilities (AOS)
- [Ninjitsu](ninjitsu.md) — Ninja combat techniques (SE)
- [Parry](parry.md) — Reduce incoming melee damage
- [Tactics](tactics.md) — Enhance combat effectiveness

## Expansion Notes

| Skill | Expansion | Notes |
|-------|-----------|-------|
| Focus | AOS (Age of Shadows) | Zero stat scales |
| Bushido | SE (Samurai Empire) | Samurai profession, zero stat scales |
| Ninjitsu | SE (Samurai Empire) | Ninja profession, zero stat scales |
| Throwing | SA (Samurai Adventure) | Gargoyle race only, zero stat scales |
| All others | Base/AOS | Available from character creation |

See [Expansions: Timeline](../expansions/timeline.md) for expansion details.

## See Also

- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Starting skill points and professions
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Reference: Skill Table](../reference/skill-table.md) — Complete skill data for all 58 skills
