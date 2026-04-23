# ModernUO Player Guide Wiki — First Pass Plan

## Scope

A player-facing wiki in markdown format documenting all systems, stats, skills, items, creatures, and configurations. Includes both player content and developer reference documentation.

## Structure

```
notes/
├── index.md                          # Hub page with full navigation
│
├── getting-started/
│   ├── index.md                      # Getting started overview
│   ├── character-creation.md         # Races, professions, stat allocation, starting gear
│   ├── stats.md                      # Str/Dex/Int, HP/Mana/Stam, regeneration, stat locks
│   └── movement.md                   # Maps, direction, running, spawn points
│
├── skills/
│   ├── index.md                      # Skills overview, skill caps (7000), locks
│   ├── crafting-skills.md            # Blacksmith, Alchemy, Tinkering, Tailoring, etc. (10 skills)
│   ├── combat-skills.md              # Swords, Macing, Fencing, Archery, Throwing (11 skills)
│   ├── magical-skills.md             # Magery, Necromancy, Chivalry, Bushido, Mysticism, etc. (8 skills)
│   └── utility-skills.md             # Detect Hidden, Tracking, Hiding, Cartography, etc. (29 skills)
│
├── spells/
│   ├── index.md                      # Spellcasting overview, mana, reagents, FC/FCR/LMC
│   ├── magery.md                     # Circles 1-8 (64 spells)
│   ├── chivalry.md                   # 11 Paladin spells
│   ├── bushido.md                    # 8 Samurai spells
│   ├── ninjitsu.md                   # 10 Ninja skills/spells
│   ├── necromancy.md                 # 19 Necromancer spells
│   ├── mysticism.md                  # 9 Gargoyle spells
  │   ├── spellweaving.md               # 13 spells + 3 base classes + 2 items + 3 mobile classes
│   └── gargoyle.md                   # Fly spell + Gargoyle traits
│
├── items/
│   ├── index.md                      # Item overview, layers, weight, stacking, LootType
│   ├── weapons.md                    # BaseWeapon: quality, slayers, abilities, durability
│   ├── armor.md                      # BaseArmor: AR, materials, durability, protection levels
│   ├── clothing.md                   # BaseClothing: durability, dyable, stat bonuses
│   ├── jewels.md                     # Gem types, jewelry, crafting
│   ├── food.md                       # Food types, healing, fill factor, poison
│   ├── tools.md                      # Crafting tools, quality, uses remaining
│   ├── books.md                      # Spellbooks, scrolls, lore books
│   └── containers.md                 # Bags, backpacks, secure containers, deeds
│
├── creatures/
│   ├── index.md                      # Creature overview, BaseCreature properties, AI types
│   ├── animals.md                    # All animal types by subcategory
│   ├── monsters.md                   # All monster types by subcategory
│   ├── npcs.md                       # Townfolk, vendors, guards, healers, hireables
│   └── bosses.md                     # Champions, paragons, special bosses, animated dead
│
├── systems/
│   ├── index.md                      # Systems overview
│   ├── combat.md                     # Melee, ranged, damage types, resistances, armor calculation
│   ├── crafting.md                   # 11 craft definitions, ECA, quality, resmelt, repair
│   ├── harvesting.md                 # Mining, Lumberjacking, Fishing mechanics
│   ├── poisons.md                    # 3 poison families (Standard, Darkglow, Parasitic), damage tables
│   ├── ethics.md                     # Hero vs Evil morals, powers, player states
│   ├── virtues.md                    # 8 virtues, 3 levels (Seeker, Follower, Knight)
│   ├── factions.md                   # Faction system, elections, tithe, towns, stability
│   ├── murder-system.md              # Murder tracking, bounty system, report mechanics
│   ├── bulk-orders.md                # Small/Large BODs, materials, rewards
│   ├── veteran-rewards.md            # Account age rewards, categories, skill cap bonus
│   ├── quests.md                     # ML quest system, quest gumps, tracking
│   ├── party.md                      # Party formation, shared loot, coordination
│   ├── khaldun.md                    # Khaldun championship/dungeon system
│   └── ultima-store.md               # Ultima Store integration
│
├── expansions/
│   ├── index.md                      # Expansion overview, cumulative nature
│   └── timeline.md                   # T2A through EJ, features per expansion (latest release focus)
│
└── reference/
    ├── index.md                      # Reference index
    ├── skill-table.md                # All 58 skills: name, category, primary stat, scales, gains
    ├── spell-index.md                # All ~135 spells: name, school, circle, reagents, effects
    ├── resistance-table.md           # Physical/Fire/Cold/Poison/Energy details
    ├── craft-resources.md            # All craft resources: properties, bonuses, tier
    ├── creature-ai-types.md          # AI types: Melee, Archer, Mage, Animal, etc. with descriptions
    ├── creature-reference.md         # Every creature type listed by name and category (comprehensive table)
    └── configuration.md              # All configurable system settings with defaults
```

## Phase 1 — Hub & Navigation

1. Create `notes/index.md` as main hub with full table of contents and links to all sections
2. Create each section's `index.md` with brief overview and links to subpages
3. Create each reference page's `index.md`

## Phase 2 — Automated Reference Tables (from source code)

Extract data programmatically from source files:

| File | Source | Content |
|---|---|---|
| `reference/skill-table.md` | `Server/Skills.cs` | All 58 skills, name, category, stat scales, gain rates |
| `reference/spell-index.md` | `Spells/*/` (all spell files) | All ~135 spells, school, circle, reagents, mana cost, effects |
| `reference/craft-resources.md` | `CraftResource` enum + resource definitions | All resources, tier, bonuses, hue |
| `reference/creature-ai-types.md` | `AIType` enum | All AI types with behavior descriptions |
| `reference/creature-reference.md` | `Mobiles/*/` directory structure | Every creature type by name, organized by category |
| `reference/configuration.md` | Config definitions in engine files | All configurable settings with defaults |

## Phase 3 — Descriptive Documentation (read + summarize from source)

### Getting Started
- **Character Creation** — read `CharacterCreation.cs`, `PlayerMobile.cs`, `Race.cs`
  - Races: Human, Elf, Gargoyle (expansion requirements)
  - Professions: Necromancer, Paladin, Samurai, Ninja, Generic (6 subtypes)
  - Stat allocation: 90 total (AOS+), 10-60 each
  - Skill selection: 100 or 120 points
  - Starting gear by profession and race
  - Starting gold: 1000

- **Stats** — read `Mobile.cs`
  - Str/Dex/Int mechanics and formulas
  - HP/Mana/Stam calculation and regeneration rates
  - Stat locks (Up/Down/Locked)
  - Racial skill bonuses

- **Movement** — read `Map.cs`, `Direction.cs`
  - All maps: Felucca, Trammel, Ilshenar, Malas, Tokuno, TerMur
  - Direction system, running mechanics
  - Spawn points per map

### Skills
- Read `UOContent/Skills/` folder for individual skill implementations
- Document each skill with: description, how to use, associated stats, gain mechanics
- Organize into 4 categories: Crafting (19), Combat (8), Magical (10), Utility (12)

### Spells
- Read each spell file across all schools
- Document: name, reagents, mana cost, effects, cast time, recovery time
- Organize by school: Magery (64), Chivalry (11), Bushido (8), Ninjitsu (10), Necromancy (19), Mysticism (9), Spellweaving (21), Gargoyle (1)

### Items
- Read `BaseWeapon.cs`, `BaseArmor.cs`, `BaseClothing.cs`, `BaseJewel.cs`, `BaseTool.cs`
- Document properties per item type with mechanics explanations
- Quality systems: Low/Regular/Exceptional
- Durability: Hit points, repair mechanics
- Attributes: AosAttributes, AosWeaponAttributes, etc.
- Slayer system, crafting system integration

### Creatures
- Read `BaseCreature.cs` for base properties
- Read directory structure of `Mobiles/` to enumerate all creature types
- Document AI types: MeleeAI, ArcherAI, MageAI, AnimalAI, HealerAI, BerserkAI, PredatorAI, ThiefAI, VendorAI
- Organize by category: Animals (by subcategory), Monsters (by subcategory), NPCs, Bosses
- **Every creature type listed by name** in reference tables

### Systems
- **Combat** — read `Mobile.cs`, `BaseWeapon.cs`, `BaseArmor.cs`
  - Melee damage formula, weapon types, swing speed
  - Ranged combat mechanics
  - Damage types and resistance calculation
  - Armor rating formula

- **Crafting** — read `Engines/Craft/`
  - 11 craft definitions and their recipes
  - ECA (Enhancement Chance Adjustment) modes
  - Quality system, resmelt, repair, maker's mark, enhance options

- **Harvesting** — read `Engines/Harvest/`
  - Mining, Lumberjacking, Fishing mechanics
  - Resource veins, skill checks, tool requirements

- **Poisons** — read `Poison.cs`
  - 5 levels: Lesser, Regular, Greater, Deadly, Lethal
  - Darkglow and Parasitic families
  - Damage tables, cure mechanics

- **Ethics** — read `Engines/Ethics/`
  - Hero ethic: 8 powers
  - Evil ethic: 8 powers
  - Player alignment mechanics

- **Virtues** — read `Engines/Virtues/`
  - 8 virtues with descriptions
  - 3 levels: Seeker, Follower, Knight
  - Virtue gumps and mechanics

- **Factions** — read `Engines/Factions/`
  - Faction structure, elections, tithe system
  - Town management, stability codes
  - Skill loss mechanics

- **Murder System** — read `Engines/Player Murder System/`
  - Short-term and long-term murder tracking
  - Bounty board mechanics

- **Bulk Orders** — read `Engines/Bulk Orders/`
  - Small BOD (1-14 items), Large BOD (15-50 items)
  - Materials, rewards

- **Veteran Rewards** — read `Engines/Veteran Rewards/`
  - Account age progression, reward categories
  - Skill cap bonus at level 4

- **Quests** — read `Engines/ML Quests/`, `Engines/Quests/`
  - Quest system mechanics

- **Party** — read `Engines/Party/`
  - Party formation, shared mechanics

- **Khaldun** — read `Engines/Khaldun/`
  - Championship dungeon system

- **Ultima Store** — read `Engines/UltimaStore/`
  - Store integration details

### Expansions
- Read `ExpansionInfo.cs`
- Document all 12 expansion levels (0-11)
- Focus on features available in latest release (EJ)
- Map availability per expansion

## Phase 4 — Configuration Reference

Extract all configurable settings from engine files and configuration definitions:

| System | Settings to Document |
|---|---|
| Murder System | Short-term duration, long-term duration, bounty expiry, recently-reported cooldown |
| Factions | Enabled toggle, election config, skill loss factor, stability threshold |
| Veteran Rewards | Reward intervals by level, skill cap unlock level |
| Harvesting | Skill ranges per resource, tool durability, range checks |
| Spawn System | Spawn timers, region configs, maximum counts |
| Resurrection | Resurrection delays, costs, locations |
| Party | Shared loot config, exp sharing |
| ConPVP | Constitutional PvP rules |
| Buff Icons | Icon display settings |
| Chat | Channel configuration |
| Help System | GM page configuration |

## Source Files Reference

| Topic | Primary Files to Read |
|---|---|
| Skills | `Server/Skills.cs` |
| Spells | `UOContent/Spells/Base/BaseSpell.cs` + all spell files |
| Items | `Server/Items/Item.cs`, `UOContent/Items/Weapons/BaseWeapon.cs`, `UOContent/Items/Armor/BaseArmor.cs`, `UOContent/Items/Clothing/BaseClothing.cs`, `UOContent/Items/Jewels/BaseJewel.cs`, `UOContent/Items/Skill Items/Tools/BaseTool.cs` |
| Creatures | `Server/Mobiles/Mobile.cs`, `UOContent/Mobiles/BaseCreature.cs`, `UOContent/Mobiles/PlayerMobile.cs` |
| Stats/Mechanics | `Server/Mobiles/Mobile.cs`, `Server/Poison.cs`, `Server/Race.cs` |
| Expansions | `Server/ExpansionInfo.cs` |
| Character Creation | `UOContent/Engines/Character Creation/CharacterCreation.cs` |
| Crafting | `UOContent/Engines/Craft/` |
| Harvesting | `UOContent/Engines/Harvest/` |
| Factions | `UOContent/Engines/Factions/` |
| Murder System | `UOContent/Engines/Player Murder System/` |
| Ethics | `UOContent/Engines/Ethics/` |
| Virtues | `UOContent/Engines/Virtues/` |
| Veteran Rewards | `UOContent/Engines/Veteran Rewards/` |
| Quests | `UOContent/Engines/ML Quests/`, `UOContent/Engines/Quests/` |
| Party | `UOContent/Engines/Party/` |
| Khaldun | `UOContent/Engines/Khaldun/` |
| Bulk Orders | `UOContent/Engines/Bulk Orders/` |
| AI | `UOContent/Mobiles/AI/` |

## Content Estimates

| Category | Count |
|---|---|
| Skills | 58 (across 4 category pages) |
| Spells | ~135 (across 8 school pages) |
| Item Types | ~20 categories (documented by type, not individually) |
| Creature Types | ~590+ (documented by category with full reference table) |
| Engine Systems | ~28 (documented in systems/ and expansions/) |
| Craft Resources | ~30+ resources |
| Resistance Types | 5 |
| Races | 3 |
| Professions | 5 main + 6 generic subtypes |
| Markdown Files | ~40-50 |

## Execution Order

1. Create directory structure
2. Create `notes/index.md` hub page
3. Phase 2: Extract automated reference tables (skill-table, spell-index, craft-resources, creature-ai-types, creature-reference, configuration)
4. Phase 3: Write descriptive docs (getting-started, skills, spells, items, creatures, systems, expansions)
5. Final pass: Ensure cross-references, consistent formatting, and complete navigation

## Formatting Conventions

- Use markdown tables for reference data (skills, creatures, spells, resources)
- Use code blocks for enum values and configuration keys
- Use headers for all section subdivisions
- Include cross-links between related pages
- Include configuration defaults in a dedicated reference section
- Every creature type listed by name in `reference/creature-reference.md`
- Focus on latest release features in expansion documentation
