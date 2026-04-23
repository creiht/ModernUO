# Bowcraft/Fletching

**Bowcraft/Fletching** is the skill used to craft bows, arrows, and related ranged weapons. It has the highest Dexterity gain rate among all crafting skills.

**Title:** Bowyer | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Bowyer |
| Primary Stat | Dexterity |
| Secondary Stat | Strength |
| Str Scale | 0.06 |
| Dex Scale | 0.16 |
| Int Scale | 0 |
| Str Gain | 0.6 |
| Dex Gain | 1.6 |
| Int Gain | 0 |

---

## Mechanics

- **Skill check:** `CheckSkill(Fletching, 0.0, 100.0)`
- **Stat scaling:** +0.06 Str and +0.16 Dex per point of those stats
- **Gain rates:** 0.6 Str gain, 1.6 Dex gain per use (highest Dex gain of any skill)
- **Primary use:** Crafting bows, arrows, and thrown weapons

The skill's dual stat scaling (Str + Dex) reflects the physical demands of bow crafting. Fletching items are used in conjunction with the crafting system.

---

## Expansion Notes

Bowcraft/Fletching is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
