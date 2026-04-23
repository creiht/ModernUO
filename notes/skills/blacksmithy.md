# Blacksmithy

**Blacksmithy** is the skill used to forge metal items such as weapons, armor, and tools. It is a core crafting skill with the highest Strength gain rate among all crafting skills.

**Title:** Blacksmith | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Blacksmith |
| Primary Stat | Strength |
| Secondary Stat | Dexterity |
| Str Scale | 0.1 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 1 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

- **Skill check:** `CheckSkill(Blacksmith, 0.0, 100.0)`
- **Stat scaling:** +0.1 Str per point of Strength
- **Gain rates:** 1.0 Str gain per use (highest Str gain of any skill)
- **Primary use:** Crafting weapons and armor from ore resources

Blacksmithy is the most Strength-reliant crafting skill and one of the two skills with the highest Str gain rate (tied with Carpentry and Mining at 2.0, but Blacksmithy has the highest Str scale at 0.1).

---

## Expansion Notes

Blacksmithy is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Full list of all crafting skills
- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Items: Armor](../items/armor.md) — Armor mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
