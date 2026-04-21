# Utility Skills

Utility skills provide support, detection, navigation, and quality-of-life capabilities. ModernUO features **22 utility skills** that enhance gameplay beyond direct combat and crafting.

**Source:** `Projects/UOContent/Skills/SkillsInfo.cs`, `Projects/UOContent/Skills/*.cs`, `Data/skills.json`

---

## Utility Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Anatomy | Biologist | Int | Str | 0 | 0 | 0 | 0.15 | 0.15 | 0.7 |
| AnimalLore | Naturalist | Int | Str | 0 | 0 | 0 | 0 | 0 | 1 |
| ArmsLore | Weapon Master | Int | Str | 0 | 0 | 0 | 0.75 | 0.15 | 0.1 |
| Begging | Beggar | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Camping | Explorer | Dex | Int | 0.2 | 0.15 | 0.15 | 2 | 1.5 | 1.5 |
| DetectHidden | Scout | Int | Dex | 0 | 0 | 0 | 0 | 0.4 | 0.6 |
| Forensics | Detective | Int | Dex | 0 | 0 | 0 | 0 | 0.2 | 0.8 |
| Healing | Healer | Int | Dex | 0.06 | 0.06 | 0.08 | 0.6 | 0.6 | 0.8 |
| Herding | Shepherd | Int | Dex | 0.1625 | 0.0625 | 0.025 | 1.625 | 0.625 | 0.25 |
| Hiding | Shade | Dex | Int | 0 | 0 | 0 | 0 | 0.8 | 0.2 |
| ItemID | Merchant | Int | Dex | 0 | 0 | 0 | 0 | 0 | 1 |
| Lockpicking | Infiltrator | Dex | Int | 0 | 0.25 | 0 | 0 | 2 | 0 |
| Poisoning | Assassin | Int | Dex | 0 | 0 | 0 | 0 | 0.4 | 1.6 |
| RemoveTrap | Trap Specialist | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Snooping | Spy | Dex | Int | 0 | 0.25 | 0 | 0 | 2.5 | 0 |
| SpiritSpeak | Medium | Int | Str | 0 | 0 | 0 | 0 | 0 | 1 |
| Stealing | Pickpocket | Dex | Int | 0 | 0.1 | 0 | 0 | 1 | 0 |
| Stealth | Rogue | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| TasteID | Praegustator | Int | Str | 0 | 0 | 0 | 0.2 | 0 | 0.8 |
| Tracking | Ranger | Int | Dex | 0 | 0.125 | 0.125 | 0 | 1.25 | 1.25 |
| Veterinary | Veterinarian | Int | Dex | 0.08 | 0.04 | 0.08 | 0.8 | 0.4 | 0.8 |
| Fishing | Fisherman | Dex | Str | 0 | 0 | 0 | 0.5 | 0.5 | 0 |
| Lumberjacking | Lumberjack | Str | Dex | 0.2 | 0 | 0 | 2 | 0 | 0 |
| Mining | Miner | Str | Dex | 0.2 | 0 | 0 | 2 | 0 | 0 |
| Discordance | Demoralizer | Int | Dex | 0 | 0.025 | 0.025 | 0 | 0.25 | 0.25 |
| Musicianship | Bard | Dex | Int | 0 | 0 | 0 | 0 | 0.8 | 0.2 |
| Peacemaking | Pacifier | Int | Dex | 0 | 0 | 0 | 0 | 0 | 0 |
| Provocation | Rouser | Int | Dex | 0 | 0.045 | 0.005 | 0 | 0.45 | 0.05 |

See also: [[reference/skill-table]] for the complete skill data.

---

## Detection Skills

### Detecting Hidden (DetectHidden)

**Title:** Scout | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Detect Hidden reveals invisible creatures, objects, and traps in the surrounding area. It works both passively (through movement detection on Felucca) and actively (through area scans).

### Mechanics

- **Skill check:** `CheckSkill(DetectHidden, 0.0, 100.0)`
- **Cooldown:** 30 seconds
- **Detection range:** `range = skill / 10` tiles (halved on failed check)
- **Passive detection:** Felucca PvP only, checks every 3 seconds with debounce

**Active detection** targets a location and reveals all hidden entities within range. The skill check compares DetectHidden vs Hiding with ±10 random variance on both:
```
ss = detectSkill + random(-10, +10)
ts = hidingSkill + random(-10, +10)
if ss >= ts: revealed
```

**Passive detection** triggers when moving near hidden creatures on Felucca only. Excludes party members, guild members/allies, blessed creatures, dead/bonded pets, and region-based PvP rules.

**Additional detection capabilities:**
- Shows `[trapped]` on containers within range when skill check passes
- Reveals hidden faction traps at 80+ DetectHidden
- Reveals dungeon traps at 75+ DetectHidden (HS+)

See source: `Skills/DetectHidden.cs`

### Forensic Evaluation (Forensics)

**Title:** Detective | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Forensics is used to investigate crime scenes and determine cause of death. It reveals information about how a creature died.

### Mechanics

- **Stat scaling:** +0.02 Dex and +0.08 Int per point of those stats
- **Gain rates:** 0.2 Dex gain, 0.8 Int gain per use
- **Primary use:** Investigating death scenes

### Item Identification (ItemID)

**Title:** Merchant | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Item Identification reveals the properties and value of unidentified items. It is the skill used by merchants and treasure hunters.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 1.0 Int gain per use (tied highest Int gain)
- **Primary use:** Identifying items

---

## Animal Skills

### Animal Lore (AnimalLore)

**Title:** Naturalist | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Animal Lore reveals detailed information about creatures, including their stats, resistances, damage, preferred food, and pack instincts.

### Mechanics

- **Skill check:** `CheckTargetSkill(AnimalLore, targetCreature, 0.0, 120.0)`
- **Target range:** 8 tiles
- **Requirements:** Must be alive, target must be a BaseCreature
- **Creature requirements:** Must be animal-type, tamed, or tameable
- **Skill requirements:** Tamed only below 100 skill; tameable only above 110 skill

The Animal Lore gump shows:
- **Attributes page:** Hits, Stamina, Mana, Str/Dex/Int, Barding Difficulty (SE+), Loyalty (AOS+)
- **Resistances page:** Physical, Fire, Cold, Poison, Energy resistances
- **Damage page:** Physical, Fire, Cold, Poison, Energy damage
- **Combat Ratings page:** Wrestling, Tactics, MagicResist, Anatomy, Healing/Poisoning
- **Lore page:** Magery, EvalInt, Meditation
- **Preferences page:** Preferred food type, Pack instincts

Animal Lore also passively checks during Animal Taming attempts, allowing simultaneous skill gain.

See source: `Skills/AnimalLore.cs`

### Animal Taming (AnimalTaming)

**Title:** Beastmaster | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Animal Taming allows players to tame wild creatures as companions. It is one of the most complex skills with multiple validation checks and scaling mechanics.

### Mechanics

- **Cooldown:** 30 seconds
- **Success condition:** `AnimalTaming >= creature.MinTameSkill` (or CheckMastery for wolf types)
- **Taming process:** 3-6 ticks at 3-second intervals (9-18 seconds total)

**Pre-tame checks:**
- Target must be a `BaseCreature` with `Tamable == true`
- Creature must not already be controlled
- Gender restrictions (some creatures only allow male/female tamers)
- CuSidhe can only be tamed by Elves
- Must have follower capacity (`Followers + ControlSlots <= FollowersMax`)
- Creature must not have exceeded maximum owners
- Creatures marked `SubdueBeforeTame` must have hits below 10% of max

**Taming process:**
- Tamer must remain within 6-7 tiles (AOS+) of the creature
- Must maintain line of sight and path
- Cannot deal damage during taming process
- 95% chance to anger creatures with `CanAngerOnTame` flag

**Post-tame scaling:**
- Normal tames: 90% of original skills, 90% skill cap
- Paralyzed tames: 86% of original skills
- Greater Dragons: 72% of original skills, 90% skill cap, Magery set to cap
- Stat loss creatures: 50% of raw stats

See source: `Skills/AnimalTaming.cs`

### Veterinary

**Title:** Veterinarian | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Veterinary is used to heal and treat animals and tamed creatures. It works similarly to Healing but is specialized for non-human creatures.

### Mechanics

- **Stat scaling:** +0.08 Str, +0.04 Dex, +0.08 Int per point of those stats
- **Gain rates:** 0.8 Str gain, 0.4 Dex gain, 0.8 Int gain per use
- **Primary use:** Healing tamed animals and creatures

### Herding

**Title:** Shepherd | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Herding is used to control and move groups of tamed animals. It allows shepherds to direct their animal companions over distances.

### Mechanics

- **Stat scaling:** +0.1625 Str, +0.0625 Dex, +0.025 Int per point of those stats (high StatTotal)
- **Gain rates:** 1.625 Str gain, 0.625 Dex gain, 0.25 Int gain per use
- **StatTotal:** 25 (highest among utility skills)
- **Primary use:** Moving groups of tamed animals

---

## Stealth Skills

### Hiding

**Title:** Shade | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Hiding allows players to conceal themselves from view. It is the foundation for stealth-based gameplay.

### Mechanics

- **Cooldown:** 10 seconds
- **Range calculation:** `range = min((100 - hidingValue) / 2 + 8, 18)`
- **Success condition:** `CheckSkill(Hiding, -bonus, 100 - bonus)`
- **House bonus:** +100 within own house (AOS+), +50 in any house (pre-AOS)

**Combat restriction:** Cannot hide while in combat (`Combatant != null`) or within range of creatures that have the player as Combatant. Range decreases with higher Hiding skill.

**On success:** Sets `m.Hidden = true` and disables Warmode. Cancels any active Invisibility spell timer.

See source: `Skills/Hiding.cs`

### Stealth

**Title:** Rogue | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Stealth allows players to move quietly while hidden, reducing the chance of detection by opponents.

### Mechanics

- **Prerequisite:** Must be hidden first (Hiding skill)
- **Hiding requirement:** 30+ skill (ML+), 50+ (SE), 80+ (base)
- **Armor penalty:** Armor Rating affects stealth check range
  - Check range: `(-20 + armorRating * 2)` to `(60 + armorRating * 2)` (AOS+)
  - Maximum allowed armor: 42 (AOS+) or 26 (pre-AOS)
- **Success:** Sets `AllowedStealthSteps = skill / 5` (AOS+) or `skill / 10` (pre-AOS)
- **Cooldown:** 10 seconds

See source: `Skills/Stealth.cs`

### Stealing

**Title:** Pickpocket | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Stealing allows players to take items from other creatures' backpacks.

### Mechanics

- **Stat scaling:** +0.1 Dex per point of Dexterity
- **Gain rates:** 1.0 Dex gain per use
- **Primary use:** Taking items from targets

### Snooping

**Title:** Spy | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Snooping allows players to search containers and creatures for hidden items.

### Mechanics

- **Stat scaling:** +0.25 Dex per point of Dexterity
- **Gain rates:** 2.5 Dex gain per use (highest Dex gain of any skill)
- **Primary use:** Searching containers for items

---

## Gathering Skills

### Mining

**Title:** Miner | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Mining is used to extract ore from rock veins. It shares the highest Strength gain rate with Lumberjacking and Carpentry.

### Mechanics

- **Stat scaling:** +0.2 Str per point of Strength
- **Gain rates:** 2.0 Str gain per use (highest Str gain, tied with Lumberjacking and Carpentry)
- **Primary use:** Extracting ore from rocks

### Lumberjacking

**Title:** Lumberjack | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Lumberjacking is used to chop down trees for lumber. It shares the highest Strength gain rate with Mining and Carpentry.

### Mechanics

- **Stat scaling:** +0.2 Str per point of Strength
- **Gain rates:** 2.0 Str gain per use (highest Str gain, tied with Mining and Carpentry)
- **Primary use:** Chopping trees for wood

### Fishing

**Title:** Fisherman | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

Fishing is used to catch fish and other aquatic creatures. It uses fishing poles and bait.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 0.5 Str gain, 0.5 Dex gain per use
- **Primary use:** Catching fish from water tiles

---

## Bard Skills

### Peacemaking

**Title:** Pacifier | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Peacemaking is used to calm hostile creatures and reduce aggression. Bards use this skill to pacify enemies.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Primary use:** Calming hostile creatures

### Discordance

**Title:** Demoralizer | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Discordance increases the aggression and hostility of targeted creatures, making them attack each other or their owner.

### Mechanics

- **Stat scaling:** +0.025 Dex and +0.025 Int per point of those stats
- **Gain rates:** 0.25 Dex gain, 0.25 Int gain per use
- **Primary use:** Increasing creature aggression

### Provocation

**Title:** Rouser | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Provocation is used to incite creatures to attack, particularly effective against tamed animals.

### Mechanics

- **Stat scaling:** +0.045 Dex and +0.005 Int per point of those stats
- **Gain rates:** 0.45 Dex gain, 0.05 Int gain per use
- **Primary use:** Inciting creatures to attack

### Musicianship

**Title:** Bard | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Musicianship enhances the effectiveness of bard songs and instruments. It provides buffs to nearby allies and debuffs to enemies.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 0.8 Dex gain, 0.2 Int gain per use
- **Primary use:** Enhancing bard songs and instrument effects

---

## Combat Support Skills

### Anatomy

**Title:** Biologist | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Anatomy reveals the weak points of creatures, increasing damage dealt to them. It is particularly effective against high-resistance targets.

### Mechanics

- **Stat scaling:** +0.15 Dex and +0.7 Int per point of those stats
- **Gain rates:** 0.15 Str gain, 0.15 Dex gain, 0.7 Int gain per use
- **Primary use:** Increasing damage against creatures

### Arms Lore (ArmsLore)

**Title:** Weapon Master | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Arms Lore provides knowledge about weapons, increasing damage with weapon types and revealing weapon properties.

### Mechanics

- **Stat scaling:** +0.75 Str gain, +0.15 Dex gain, +0.1 Int gain per use
- **Primary use:** Enhancing weapon effectiveness

### Poisoning

**Title:** Assassin | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

Poisoning applies poison to weapons, food, darts, and shuriken. It is a key skill for poison-based combat strategies.

### Mechanics

- **Cooldown:** 10 seconds
- **Process:** Target poison potion → then target item to poison
- **Poisonable items:** Weapons (infectious strike ability), food, fukiya darts, shuriken
- **Weapon charges:** `18 - poisonLevel * 2` (e.g., Lethal poison = 8 charges)
- **Self-poison risk:** 5% chance if skill < 80 and check fails

See source: `Skills/Poisoning.cs`

### Remove Trap

**Title:** Trap Specialist | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Remove Trap allows players to safely disarm traps found in dungeons and buildings.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Primary use:** Disarming traps

---

## Utility Skills

### Meditation

**Title:** Stoic | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Meditation increases mana regeneration rate when activated. See [[skills/magical-skills]] for detailed mechanics.

### Spirit Speak (SpiritSpeak)

**Title:** Medium | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Spirit Speak allows players to contact the netherworld and channel energy from corpses to heal wounds.

### Mechanics

**Pre-AOS:**
- `CheckSkill(SpiritSpeak, 0, 100)` determines if contact succeeds
- Duration scales with skill: `max(15, (skill / 50) * 90)` seconds
- Sets `CanHearGhosts = true` for the duration

**AOS+ (Spell system):**
- Functions as a spell with mantra "Anh Mi Sah Ko"
- Targets nearby corpses to channel energy for healing
- Healing range: `min` to `min + 4` hits, where `min = 1 + (skill * 0.25)`
- Costs 0 mana when channeling from corpse, 10 mana otherwise
- Requires 100+ skill for reliable healing: `skill / 100.0` success chance

See source: `Skills/SpiritSpeak.cs`

### Taste Identification (TasteID)

**Title:** Praegustator | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Taste Identification determines the properties of food and drink items by tasting them.

### Mechanics

- **Stat scaling:** +0.2 Str gain, +0.8 Int gain per use
- **Primary use:** Identifying food properties

### Camping

**Title:** Explorer | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Camping enhances survival in the wilderness, improving outdoor capabilities and resource gathering.

### Mechanics

- **Stat scaling:** +0.2 Str, +0.15 Dex, +0.15 Int per point of those stats
- **Gain rates:** 2.0 Str gain, 1.5 Dex gain, 1.5 Int gain per use
- **Primary use:** Wilderness survival

### Begging

**Title:** Beggar | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Begging allows players to approach NPCs and receive gold or items as alms.

### Mechanics

- **Cooldown:** 30 seconds (targeter) + 10 seconds (skill)
- **Target:** Must target a human NPC within 2 tiles
- **Success:** `CheckTargetSkill(Begging, target, 0.0, 100.0)`
- **Gold amount:** `min(packGold / 10, max(10, fame / 2500 + 10))`
- **Karma penalty:** Negative karma increases chance of rejection

See source: `Skills/Begging.cs`

### Lockpicking

**Title:** Infiltrator | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Lockpicking allows players to open locked containers and doors. It has the highest Dexterity gain rate among utility skills.

### Mechanics

- **Stat scaling:** +0.25 Dex per point of Dexterity
- **Gain rates:** 2.0 Dex gain per use (tied highest Dex gain)
- **Primary use:** Opening locked containers and doors

---

## Expansion Notes

All utility skills are available from character creation in the base game. No utility skills are expansion-gated.

| Category | Skills |
|----------|--------|
| Detection | DetectHidden, Forensics, ItemID |
| Animal | AnimalLore, AnimalTaming, Veterinary, Herding |
| Stealth | Hiding, Stealth, Stealing, Snooping |
| Gathering | Fishing, Lumberjacking, Mining |
| Bard | Peacemaking, Discordance, Provocation, Musicianship |
| Combat Support | Anatomy, ArmsLore, Poisoning, RemoveTrap |
| Utility | Meditation, SpiritSpeak, TasteID, Camping, Begging, Lockpicking |

---

## Cross-References

- [Crafting Skills](crafting-skills.md) — Item creation skills
- [Combat Skills](combat-skills.md) — Offensive and defensive skills
- [Magical Skills](magical-skills.md) — Spellcasting and magical abilities
- [Systems: Combat](systems/combat.md) — Combat mechanics
- [Systems: Crafting](systems/crafting.md) — Crafting engine
- [Getting Started: Stats](getting-started/stats.md) — Stat-skill relationships
