# Meditation

**Meditation** is used to enter a trance state that significantly increases mana regeneration rate. It is one of the most important skills for spellcasters.

**Title:** Stoic | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Stoic |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
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
- **Primary use:** Faster mana recovery
- **Cooldown:** 10 seconds (5 seconds pre-AOS)

Meditation has specific preconditions that must be met:

**Pre-AOS:**
- Must have >= 10% HP

**AOS+:**
- `RegenRates.GetArmorOffset(m) == 0` (no meditation-blocking armor)
- Hands must be free or holding only spellbook/runebook/spell-channeling items

**Success chance:**
```
chance = (50.0 + (skillVal - (manaMax - mana)) * 2) / 100
```

Higher skill increases the chance of success. Lower mana deficit (closer to full mana) decreases the chance, as the skill is less useful when already near full mana.

**On success:**
- Sets `m.Meditating = true`
- Adds `BuffIcon.ActiveMeditation` buff icon
- Plays meditation sound (0xF9)

---

## Expansion Notes

Meditation is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
