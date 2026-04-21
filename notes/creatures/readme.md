# Creatures

ModernUO populates its world with **650+ creature types** organized into animals, monsters, NPCs, and bosses. Each creature is defined by its AI type, base stats, equipment, spells, and behavior patterns.

## Creature Organization

### [Animals](animals.md)
Passive and aggressive fauna across all maps. Organized by subcategory (birds, canines, felines, etc.). Generally lower difficulty and useful for resource harvesting.

### [Monsters](monsters.md)
Hostile creatures ranging from low-level slimes to high-level dragons. Organized by subcategory (humanoid, undead, demonic, etc.). Primary sources of loot and experience.

### [NPCs](npcs.md)
Non-hostile characters that serve gameplay functions: vendors, healers, quest givers, guards, and hireable companions. Essential for economy and progression.

### [Bosses](bosses.md)
Elite creatures including champions, paragons, and special named bosses. Often have unique loot tables, special abilities, and spawn in dungeon areas or as world bosses.

## Creature Properties

- **AI Types**: Each creature uses an AI behavior type (MeleeAI, ArcherAI, MageAI, AnimalAI, etc.)
- **BaseStats**: Base Str/Dex/Int values that scale with difficulty
- **HP/Mana/Stam**: Resource pools with regeneration rates
- **Skills**: Skills the creature possesses and their levels
- **Spells**: Spells the creature can cast (for magical creatures)
- **Equipment**: Weapons, armor, and items the creature carries
- **Tamable**: Whether the creature can be tamed by players
- **Summonable**: Whether the creature can be summoned via spells
- **Resistances**: Resistance values against damage types
- **Damage Types**: Weapon damage type (Physical, Fire, Cold, Poison, Energy)

## AI Types

- **MeleeAI**: Engages targets in close combat
- **ArcherAI**: Attacks from range with projectiles
- **MageAI**: Casts spells on targets
- **AnimalAI**: Simplified AI for fauna
- **HealerAI**: Focuses on healing allies
- **BerserkAI**: Aggressive melee with increased damage
- **PredatorAI**: Hunts specific target types
- **ThiefAI**: Steals from players during combat
- **VendorAI**: Manages vendor inventory
