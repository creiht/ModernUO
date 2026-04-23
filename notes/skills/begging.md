# Begging

**Begging** allows players to approach NPCs and receive gold or items as alms.

**Title:** Beggar | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Beggar |
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

- **Cooldown:** 30 seconds (targeter) + 10 seconds (skill)
- **Target:** Must target a human NPC within 2 tiles
- **Success:** `CheckTargetSkill(Begging, target, 0.0, 100.0)`
- **Gold amount:** `min(packGold / 10, max(10, fame / 2500 + 10))`
- **Karma penalty:** Negative karma increases chance of rejection

---

## Expansion Notes

Begging is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
