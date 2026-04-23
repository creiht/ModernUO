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

## Expansion Notes

Inscription is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Books](../items/books.md) — Book mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
