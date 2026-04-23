# SpiritSpeak

**Spirit Speak (SpiritSpeak)** allows players to contact the netherworld and channel energy from corpses to heal wounds.

**Title:** Medium | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Medium |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 1 |

---

## Mechanics

**Pre-AOS:**
- `CheckSkill(SpiritSpeak, 0, 100)` determines if contact succeeds
- Duration scales with skill: `max(15, (skill / 50) * 90)` seconds
- Sets `CanHearGhosts = true` for the duration

**AOS+ (Spell system):**
- Functions as a spell with mantra "Anh Mi Sah Ko"
- Targets nearby corpses to channel energy for healing
- Healing range: `min` to `min + 4` hits, where `min = 1 + (skill * 0.25)`
- Costs 0 mana when channeling from corpse, 10 mana otherwise
- Requires 100+ skill for reliable healing: `skill / 100.0` success chance

---

## Expansion Notes

SpiritSpeak is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
