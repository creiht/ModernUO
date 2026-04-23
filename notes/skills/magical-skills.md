# Magical Skills

Magical skills are related to spellcasting, resistance, and magical abilities. ModernUO features **8 magical skills** that form the foundation of a spellcaster's capabilities.

Magical skills are defined in `Distribution/Data/skills.json` and grouped in `SkillsInfo.cs` (MagicSkills array, lines 83-99). All 8 skills share a uniform `GainFactor` of 4.

## Magical Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Chivalry | Paladin | Str | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| EvalInt | Scholar | Int | Str | 0 | 0 | 0 | 0 | 0 | 1 |
| Magery | Mage | Int | Str | 0 | 0 | 0.15 | 0 | 0 | 1.5 |
| Meditation | Stoic | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Mysticism | Mystic | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Necromancy | Necromancer | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Spellweaving | Arcanist | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| MagicResist | Warder | Str | Dex | 0 | 0 | 0 | 0.25 | 0.25 | 0.5 |

## Individual Skill Pages

### Spellcasting Foundations
- [Magery](magery.md) — Cast spells from all spell schools
- [EvalInt](evalint.md) — Assess target Intelligence and Mana

### Magical Resistance
- [MagicResist](magicresist.md) — Reduce incoming magical damage

### Mana Recovery
- [Meditation](meditation.md) — Increase mana regeneration rate

### Magical Combat Skills
- [Chivalry](chivalry.md) — Holy abilities for paladins (AOS)
- [Necromancy](necromancy.md) — Death-themed abilities (AOS)

### Advanced Magical Schools
- [Spellweaving](spellweaving.md) — Enhance spellcasting efficiency (ML)
- [Mysticism](mysticism.md) — Teleportation and dimension abilities (SA)

## Expansion Notes

| Skill | Expansion | Notes |
|-------|-----------|-------|
| Chivalry | AOS (Age of Shadows) | Paladin profession, zero stat scales |
| Necromancy | AOS (Age of Shadows) | Necromancer profession, zero stat scales |
| Spellweaving | ML (Mondain's Legacy) | Zero stat scales |
| Mysticism | SA (Samurai Adventure) | Zero stat scales |
| All others | Base/AOS | Available from character creation |

See [Expansions: Timeline](../expansions/timeline.md) for expansion details.

## See Also

- [Spells: Magery](../spells/magery.md) — First through Eighth circle spells
- [Spells: Chivalry](../spells/chivalry.md) — Chivalry spells
- [Spells: Necromancy](../spells/necromancy.md) — Necromancy spells
- [Spells: Bushido](../spells/bushido.md) — Bushido spells
- [Spells: Ninjitsu](../spells/ninjitsu.md) — Ninjitsu spells
- [Spells: Mysticism](../spells/mysticism.md) — Mysticism spells
- [Spells: Spellweaving](../spells/spellweaving.md) — Spellweaving spells
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Reference: Skill Table](../reference/skill-table.md) — Complete skill data for all 58 skills
