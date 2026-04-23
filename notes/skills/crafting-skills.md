# Crafting Skills

Crafting skills are used to create items, modify equipment, and process resources. ModernUO features **10 crafting skills**, each associated with specific crafting disciplines.

Crafting skills are defined in `Distribution/Data/skills.json` and registered in `SkillsInfo.cs`. All 10 skills share a uniform `GainFactor` of 4.

## Crafting Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Alchemy | Alchemist | Int | Dex | 0 | 0.05 | 0.05 | 0 | 0.5 | 0.5 |
| Blacksmithy | Blacksmith | Str | Dex | 0.1 | 0 | 0 | 1 | 0 | 0 |
| Bowcraft/Fletching | Bowyer | Dex | Str | 0.06 | 0.16 | 0 | 0.6 | 1.6 | 0 |
| Carpentry | Carpenter | Str | Dex | 0.2 | 0.05 | 0 | 2 | 0.5 | 0 |
| Cartography | Cartographer | Int | Dex | 0 | 0.075 | 0.075 | 0 | 0.75 | 0.75 |
| Cooking | Chef | Int | Dex | 0 | 0.2 | 0.3 | 0 | 2 | 3 |
| Imbuing | Artificer | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Inscription | Scribe | Int | Dex | 0 | 0.02 | 0.08 | 0 | 0.2 | 0.8 |
| Tailoring | Tailor | Dex | Int | 0.0375 | 0.1625 | 0.05 | 0.38 | 1.63 | 0.5 |
| Tinkering | Tinker | Dex | Int | 0.05 | 0.02 | 0.03 | 0.5 | 0.2 | 0.3 |

## Individual Skill Pages

- [Alchemy](alchemy.md) — Create potions and poisons
- [Blacksmithy](blacksmithy.md) — Forge metal weapons, armor, and tools
- [Bowcraft/Fletching](bowcraft-fletching.md) — Craft bows, arrows, and ranged weapons
- [Carpentry](carpentry.md) — Craft wooden items, furniture, and containers
- [Cartography](cartography.md) — Create maps of explored areas
- [Cooking](cooking.md) — Prepare food items from raw ingredients
- [Imbuing](imbuing.md) — Apply magical properties to items (SA)
- [Inscription](inscription.md) — Copy written pages between books
- [Tailoring](tailoring.md) — Craft clothing from fabric
- [Tinkering](tinkering.md) — Modify and repair items and tools

## Expansion Notes

| Skill | Expansion | Notes |
|-------|-----------|-------|
| Imbuing | SA (Samurai Adventure) | Expansion-gated, zero stat scales |
| All others | Base/AOS | Available from character creation |

See [Expansions: Timeline](../expansions/timeline.md) for expansion details.

## See Also

- [Systems: Crafting](../systems/crafting.md) — Crafting engine and mechanics
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Getting Started: Character Creation](../getting-started/character-creation.md) — Starting skill points
- [Reference: Skill Table](../reference/skill-table.md) — Complete skill data for all 58 skills
