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

See also: [[reference/skill-table]] for the complete skill data.

---

## Alchemy

**Title:** Alchemist | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Alchemy is used to create potions and poisons. It is one of the earliest crafting skills, available from character creation.

### Mechanics

- **Skill check:** `CheckSkill(Alchemy, 0.0, 100.0)`
- **Stat scaling:** +0.05 Dex and +0.05 Int per point of those stats
- **Gain rates:** 0.5 Dex gain, 0.5 Int gain per use
- **Primary use:** Brewing potions from harvested reagents

Alchemy is closely tied to the [[systems/crafting]] system and the [[systems/poisons]] system. Alchemical items include healing potions, strength potions, and various magical concoctions.

---

## Blacksmithy

**Title:** Blacksmith | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Blacksmithy is used to forge metal items such as weapons, armor, and tools. It is a core crafting skill with the highest Strength gain rate among all crafting skills.

### Mechanics

- **Skill check:** `CheckSkill(Blacksmith, 0.0, 100.0)`
- **Stat scaling:** +0.1 Str per point of Strength
- **Gain rates:** 1.0 Str gain per use (highest Str gain of any skill)
- **Primary use:** Crafting weapons and armor from ore resources

Blacksmithy is the most Strength-reliant crafting skill and one of the two skills with the highest Str gain rate (tied with Carpentry and Mining at 2.0, but Blacksmithy has the highest Str scale at 0.1).

See also: [[items/weapons]], [[items/armor]]

---

## Bowcraft/Fletching

**Title:** Bowyer | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

Bowcraft/Fletching is used to craft bows, arrows, and related ranged weapons. It has the highest Dexterity gain rate among all crafting skills.

### Mechanics

- **Skill check:** `CheckSkill(Fletching, 0.0, 100.0)`
- **Stat scaling:** +0.06 Str and +0.16 Dex per point of those stats
- **Gain rates:** 0.6 Str gain, 1.6 Dex gain per use (highest Dex gain of any skill)
- **Primary use:** Crafting bows, arrows, and thrown weapons

The skill's dual stat scaling (Str + Dex) reflects the physical demands of bow crafting. Fletching items are used in conjunction with the [[systems/crafting]] system.

---

## Carpentry

**Title:** Carpenter | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Carpentry is used to craft wooden items including furniture, barrels, doors, and various containers. It shares the highest Strength gain rate with Mining and Lumberjacking.

### Mechanics

- **Stat scaling:** +0.2 Str and +0.05 Dex per point of those stats
- **Gain rates:** 2.0 Str gain, 0.5 Dex gain per use (highest Str gain, tied with Mining and Lumberjacking)
- **StatTotal:** 25 (highest among crafting skills)
- **Primary use:** Crafting wooden items from lumber resources

Carpentry is one of three skills with a StatTotal of 50 or higher, making it one of the most stat-influenced skills in the game.

---

## Cartography

**Title:** Cartographer | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Cartography is used to create maps of explored areas. It is a support crafting skill that helps players navigate the world.

### Mechanics

- **Stat scaling:** +0.075 Dex and +0.075 Int per point of those stats
- **Gain rates:** 0.75 Dex gain, 0.75 Int gain per use
- **Primary use:** Creating maps of explored regions

Cartography requires balanced Dexterity and Intelligence investment and is one of the fewer stat-dependent crafting skills.

---

## Cooking

**Title:** Chef | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Cooking is used to prepare food items from raw ingredients. It has the highest Intelligence gain rate among all crafting skills and the highest total StatTotal.

### Mechanics

- **Stat scaling:** +0.2 Dex and +0.3 Int per point of those stats
- **Gain rates:** 2.0 Dex gain, 3.0 Int gain per use (highest Int gain of any skill)
- **StatTotal:** 50 (highest among all skills)
- **Primary use:** Preparing food from raw meat and other ingredients

Cooking is the most Intelligence-reliant crafting skill and provides the highest Int gain rate, making it an excellent skill for intelligence-based characters.

---

## Imbuing

**Title:** Artificer | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Imbuing is used to apply magical properties to items through the artifact imbuing system. It is expansion-gated to **SA (Samurai Adventure)**.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SA expansion
- **Primary use:** Applying imbuing charges to weapons and armor

Imbuing is one of the "pure" expansion skills with zero stat scales and zero gain modifiers. It functions independently of stat influence and is used exclusively with the artifact imbuing system.

See also: [[systems/crafting]]

---

## Inscription

**Title:** Scribe | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Inscription is used to copy written pages from one book to another. It is the skill used by scribes and spellcasters to transfer spellbooks.

### Mechanics

- **Skill check:** `CheckTargetSkill(Inscribe, destinationBook, 0.0, 50.0)`
- **Stat scaling:** +0.02 Dex and +0.08 Int per point of those stats
- **Gain rates:** 0.2 Dex gain, 0.8 Int gain per use
- **Target range:** 8 tiles
- **Primary use:** Copying written pages between books

The Inscription skill check uses a lower difficulty range (0-50) compared to most other skills, reflecting the accessible nature of basic book copying. Inscription is closely tied to the [[items/books]] system.

See also: [[items/books]]

---

## Tailoring

**Title:** Tailor | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Tailoring is used to craft clothing and related items from fabric. It has a well-balanced stat distribution across all three stats.

### Mechanics

- **Stat scaling:** +0.0375 Str, +0.1625 Dex, +0.05 Int per point of those stats
- **Gain rates:** 0.38 Str gain, 1.63 Dex gain, 0.5 Int gain per use
- **StatTotal:** 25
- **Primary use:** Crafting clothing items from fabric resources

Tailoring is the most Dexterity-reliant crafting skill and provides balanced stat gains across all three stats, making it a versatile choice for multi-stat characters.

---

## Tinkering

**Title:** Tinker | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Tinkering is used to modify and repair items, including tools, weapons, and armor. It has moderate stat scaling across all three stats.

### Mechanics

- **Stat scaling:** +0.05 Str, +0.02 Dex, +0.03 Int per point of those stats
- **Gain rates:** 0.5 Str gain, 0.2 Dex gain, 0.3 Int gain per use
- **Primary use:** Modifying and repairing items, including tool upgrades

Tinkering is closely tied to the [[items/tools]] system and the [[systems/crafting]] system. It allows players to enhance tools and create specialized items.

---

## Crafting System Integration

All crafting skills integrate with the core crafting engine defined in `Engines/Craft/`. Key concepts include:

- **Craft definitions:** Each crafting discipline has a `Def` class (e.g., `DefBlacksmithy`, `DefTailoring`) that defines recipes, resource requirements, and quality options
- **Quality system:** Items can be crafted as Regular or Exceptional quality
- **Maker's mark:** Crafted items can be marked with the crafter's name
- **Enhance options:** Certain crafting disciplines allow enhancing existing items
- **Tool requirements:** Each crafting skill requires specific tools (e.g., anvil for Blacksmithy, sewing kit for Tailoring)

See [[systems/crafting]] for the complete crafting system documentation.

---

## Expansion Notes

| Skill | Expansion | Notes |
|-------|-----------|-------|
| Imbuing | SA (Samurai Adventure) | Expansion-gated, zero stat scales |
| All others | Base/AOS | Available from character creation |

See [[expansions/timeline]] for expansion details.

---

## See Also

- [[reference/skill-table]] — Complete skill data for all 58 skills
- [[systems/crafting]] — Crafting engine and mechanics
- [[getting-started/stats]] — Stat-skill relationships
- [[getting-started/character-creation]] — Starting skill points
