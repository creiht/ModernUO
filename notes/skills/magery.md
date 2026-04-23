# Magery

**Magery** is the foundational magical skill, required for casting spells from all spell schools. It has the highest Intelligence scale among all magical skills.

**Title:** Mage | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Mage |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0.15 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 1.5 |

---

## Mechanics

- **Stat scaling:** +0.15 Int per point of Intelligence (highest Int scale of any skill)
- **Gain rates:** 1.5 Int gain per use (highest Int gain of any skill)
- **StatTotal:** 15
- **Primary use:** Casting spells from all spell schools
- **Spell access:** Required for First through Eighth circle spells

Magery is the most Intelligence-reliant skill in the game and the primary stat driver for all spellcasters. Without Magery, a character cannot cast any spells. The `SpellInfo.Magery` requirement for each spell determines the minimum Magery level needed.

---

## Expansion Notes

Magery is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Spells: Magery](../spells/magery.md) — First through Eighth circle spells
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
