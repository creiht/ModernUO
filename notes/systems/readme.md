# Systems

ModernUO includes a comprehensive set of game systems that define the player experience. These systems cover combat, crafting, social features, progression, and world mechanics.

## Core Mechanics

### [Combat](combat.md)
Melee and ranged combat with damage types (Physical, Fire, Cold, Poison, Energy), armor rating calculation, resistance system, and weapon abilities.

### [Crafting](crafting.md)
11 craft definitions (Blacksmithing, Tailoring, Alchemy, etc.) with quality systems, ECA (Enhancement Chance Adjustment), maker's marks, and repair mechanics.

### [Harvesting](harvesting.md)
Resource gathering through Mining, Lumberjacking, and Fishing. Includes the shared `HarvestSystem` engine, `HarvestDefinition` configuration, vein/weight systems, bank tracking, racial bonuses (Elves/Humans on Felucca), tool durability, and expansion-gated bonus resources.

### [Poisons](poisons.md)
Three poison families (Standard, Darkglow, Parasitic) with 14 total poison levels (Lesser through Lethal). Includes damage formulas, Darkglow range bonus, Parasitic healing, auto-cure mechanics, and application via Poisoning skill.

## Social & Moral Systems

### [Ethics](ethics.md)
Hero and Evil moral alignments, each with 8 unique powers. Players choose an ethic that affects available abilities and interactions.

### [Virtues](virtues.md)
Eight UO virtues (Honesty, Honor, Humility, Sacrifice, etc.) with three progression levels above None: Seeker, Follower, and Knight.

### [Factions](factions.md)
Four-player faction PvP system (TrueBritannians, CouncilOfMages, Minax, Shadowlords) with town capture via sigils, kill point rankings, silver economy with tithe system, elections with weighted voting, guard and vendor management, faction imbued items, power items (BloodRose, StormsEye, etc.), faction traps, and a stability code to prevent faction dominance.

### [Murder System](murder-system.md)
Tracks player-killing with short-term (8h decay) and long-term (40h decay) murder states. Includes kill decay timers, bounty boards with synthetic bulletin board integration, report gumps, bounty placement mechanics (Kills >= 4), expansion-variant kill report messages, and ping pong counters (T2A only).

## Progression & Rewards

### [Bulk Orders](bulk-orders.md)
Small and Large Bulk Order Deeds for Blacksmithing and Tailoring. Includes BOD generation (skill-based quantity, material, and exceptional chance), reward calculation (Smith: 9 tiers up to Runic Hammer+8 / Ancient Hammer+60; Tailor: 14 tiers up to Runic Kit+3), Bulk Order Book collection system with dual-scope filtering (book + personal), entry persistence via BOBEntries registry, and the small/large BOD combine flow.

### [Veteran Rewards](veteran-rewards.md)
Account age-based rewards with progressive tiers, special items, and skill cap bonuses at level 4.

### [Quests](quests.md)
Two quest systems: the ML Quest System (Mondain's Legacy, template-based with 21 quest types, chain triggers, timed objectives, escort missions, skill training) and the Modern Quest System (player-owned with 11 profession-restricted quest types, dialogue trees, regional objectives, and dynamic quest items). Includes quest gumps, objective tracking, reward distribution, and GM tools.

### [Party](party.md)
Party formation, shared loot distribution, and coordination tools.

## Special Systems

### [Khaldun](khaldun.md)
A self-contained puzzle dungeon with switch-activated stone walls, tile-morphing puzzle pieces, ambient sound effects, and a final cylinder-lock puzzle chest. Features 4 cursed NPC guardians and 3 lore journals telling the story of Khal Ankur's tomb.

### [Housing](housing.md)
House system: foundation types, ownership, permissions, co-owners, friend/ban lists, lockdown, secure storage, vendor contracts, decay mechanics, and design tools.

### [Ultima Store](ultima-store.md)
Microtransaction integration for cosmetic and convenience items.

## Expansions

### [Expansions](../expansions/readme.md)
12 expansion levels (T2A through EJ) with cumulative feature unlocks including new maps, races, skills, and spells.
