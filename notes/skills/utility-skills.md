# Utility Skills

Utility skills provide support, detection, navigation, and quality-of-life capabilities. ModernUO features **29 utility skills** that enhance gameplay beyond direct combat and crafting.

**Source:** `Projects/UOContent/Skills/SkillsInfo.cs`, `Projects/UOContent/Skills/*.cs`, `Data/skills.json`

## Utility Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Anatomy | Biologist | Int | Str | 0 | 0 | 0 | 0.15 | 0.15 | 0.7 |
| AnimalLore | Naturalist | Int | Str | 0 | 0 | 0 | 0 | 0 | 1 |
| AnimalTaming | Tamer | Str | Int | 0.14 | 0.02 | 0.04 | 1.4 | 0.2 | 0.4 |
| ArmsLore | Weapon Master | Int | Str | 0 | 0 | 0 | 0.75 | 0.15 | 0.1 |
| Begging | Beggar | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Camping | Explorer | Dex | Int | 0.2 | 0.15 | 0.15 | 2 | 1.5 | 1.5 |
| DetectHidden | Scout | Int | Dex | 0 | 0 | 0 | 0 | 0.4 | 0.6 |
| Discordance | Demoralizer | Int | Dex | 0 | 0.025 | 0.025 | 0 | 0.25 | 0.25 |
| Fishing | Fisherman | Dex | Str | 0 | 0 | 0 | 0.5 | 0.5 | 0 |
| Forensics | Detective | Int | Dex | 0 | 0 | 0 | 0 | 0.2 | 0.8 |
| Healing | Healer | Int | Dex | 0.06 | 0.06 | 0.08 | 0.6 | 0.6 | 0.8 |
| Herding | Shepherd | Int | Dex | 0.1625 | 0.0625 | 0.025 | 1.625 | 0.625 | 0.25 |
| Hiding | Shade | Dex | Int | 0 | 0 | 0 | 0 | 0.8 | 0.2 |
| ItemID | Merchant | Int | Dex | 0 | 0 | 0 | 0 | 0 | 1 |
| Lockpicking | Infiltrator | Dex | Int | 0 | 0.25 | 0 | 0 | 2 | 0 |
| Lumberjacking | Lumberjack | Str | Dex | 0.2 | 0 | 0 | 2 | 0 | 0 |
| Mining | Miner | Str | Dex | 0.2 | 0 | 0 | 2 | 0 | 0 |
| Musicianship | Bard | Dex | Int | 0 | 0 | 0 | 0 | 0.8 | 0.2 |
| Peacemaking | Pacifier | Int | Dex | 0 | 0 | 0 | 0 | 0 | 0 |
| Poisoning | Assassin | Int | Dex | 0 | 0 | 0 | 0 | 0.4 | 1.6 |
| Provocation | Rouser | Int | Dex | 0 | 0.045 | 0.005 | 0 | 0.45 | 0.05 |
| RemoveTrap | Trap Specialist | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Snooping | Spy | Dex | Int | 0 | 0.25 | 0 | 0 | 2.5 | 0 |
| SpiritSpeak | Medium | Int | Str | 0 | 0 | 0 | 0 | 0 | 1 |
| Stealing | Pickpocket | Dex | Int | 0 | 0.1 | 0 | 0 | 1 | 0 |
| Stealth | Rogue | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| TasteID | Praegustator | Int | Str | 0 | 0 | 0 | 0.2 | 0 | 0.8 |
| Tracking | Ranger | Int | Dex | 0 | 0.125 | 0.125 | 0 | 1.25 | 1.25 |
| Veterinary | Veterinarian | Int | Dex | 0.08 | 0.04 | 0.08 | 0.8 | 0.4 | 0.8 |

## Individual Skill Pages

### Detection Skills
- [DetectHidden](detecthidden.md) — Reveal invisible creatures, objects, and traps
- [Forensics](forensics.md) — Investigate crime scenes and cause of death
- [ItemID](itemid.md) — Identify properties and value of unidentified items

### Animal Skills
- [AnimalLore](animallore.md) — Identify creatures and gather information
- [AnimalTaming](animaltaming.md) — Capture and tame wild creatures
- [Herding](herding.md) — Control and move groups of tamed animals
- [Veterinary](veterinary.md) — Heal and treat animals and tamed creatures

### Stealth Skills
- [Hiding](hiding.md) — Conceal yourself from view
- [Lockpicking](lockpicking.md) — Open locked containers and doors
- [Snooping](snooping.md) — Search containers and creatures for items
- [Stealing](stealing.md) — Take items from other creatures' backpacks
- [Stealth](stealth.md) — Move quietly while hidden

### Gathering Skills
- [Fishing](fishing.md) — Catch fish and aquatic creatures
- [Lumberjacking](lumberjacking.md) — Chop down trees for lumber
- [Mining](mining.md) — Extract ore from rock veins

### Bard Skills
- [Discordance](discordance.md) — Increase creature aggression
- [Musicianship](musicianship.md) — Enhance bard songs and instruments
- [Peacemaking](peacemaking.md) — Calm hostile creatures
- [Provocation](provocation.md) — Incite creatures to attack

### Combat Support Skills
- [Anatomy](anatomy.md) — Increase damage against creatures
- [ArmsLore](armslore.md) — Enhance weapon effectiveness
- [Poisoning](poisoning.md) — Apply poison to weapons and items
- [RemoveTrap](removetrap.md) — Disarm traps in dungeons and buildings

### Utility Skills
- [Begging](begging.md) — Receive gold or items from NPCs as alms
- [Camping](camping.md) — Enhance wilderness survival
- [SpiritSpeak](spiritspeak.md) — Channel energy from corpses to heal
- [TasteID](tasteid.md) — Identify food and drink properties
- [Tracking](tracking.md) — Follow tracks in the world

### Cross-References
- [Meditation](../skills/magical-skills.md) — Mana regeneration (Magical Skills)

## Expansion Notes

All utility skills are available from character creation in the base game. No utility skills are expansion-gated.

| Category | Skills |
|----------|--------|
| Detection | DetectHidden, Forensics, ItemID |
| Animal | AnimalLore, AnimalTaming, Herding, Veterinary |
| Stealth | Hiding, Lockpicking, Snooping, Stealing, Stealth |
| Gathering | Fishing, Lumberjacking, Mining |
| Bard | Discordance, Musicianship, Peacemaking, Provocation |
| Combat Support | Anatomy, ArmsLore, Poisoning, RemoveTrap |
| Utility | Begging, Camping, SpiritSpeak, TasteID, Tracking |

## See Also

- [Crafting Skills](crafting-skills.md) — Item creation skills
- [Combat Skills](combat-skills.md) — Offensive and defensive skills
- [Magical Skills](magical-skills.md) — Spellcasting and magical abilities
- [Systems: Combat](../systems/combat.md) — Combat mechanics
- [Systems: Crafting](../systems/crafting.md) — Crafting engine
- [Systems: Harvesting](../systems/harvesting.md) — Resource gathering
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
- [Reference: Skill Table](../reference/skill-table.md) — Complete skill data for all 58 skills
