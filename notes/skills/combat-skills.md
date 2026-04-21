# Combat Skills

Combat skills directly affect your character's offensive and defensive capabilities in combat. ModernUO features **11 combat skills**, spanning melee weapons, ranged weapons, and combat techniques.

Combat skills are defined in `Distribution/Data/skills.json` and grouped in `SkillsInfo.cs` (CombatSkills array, lines 22-41). All 11 skills share a uniform `GainFactor` of 4.

## Combat Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Archery | Archer | Dex | Str | 0.025 | 0.075 | 0 | 0.25 | 0.75 | 0 |
| Bushido | Samurai | Str | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Focus | Driven | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Fencing | Fencer | Dex | Str | 0.045 | 0.055 | 0 | 0.45 | 0.55 | 0 |
| Macing | Armsman | Str | Dex | 0.09 | 0.01 | 0 | 0.9 | 0.1 | 0 |
| Ninjitsu | Ninja | Dex | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| Parry | Duelist | Dex | Str | 0.075 | 0.025 | 0 | 0.75 | 0.25 | 0 |
| Swords | Swordsman | Str | Dex | 0.075 | 0.025 | 0 | 0.75 | 0.25 | 0 |
| Tactics | Tactician | Str | Dex | 0 | 0 | 0 | 0 | 0 | 0 |
| Throwing | Bladeweaver | Dex | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Wrestling | Wrestler | Str | Dex | 0.09 | 0.01 | 0 | 0.9 | 0.1 | 0 |

See also: [[reference/skill-table]] for the complete skill data.

---

## Melee Weapons

### Swords

**Title:** Swordsman | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Swords is the skill used with sword-type weapons (broadswords, katanas, scimitars, etc.). It is the profession skill for the Swordsman starting profession.

### Mechanics

- **Stat scaling:** +0.075 Str and +0.025 Dex per point of those stats
- **Gain rates:** 0.75 Str gain, 0.25 Dex gain per use
- **StatTotal:** 10
- **Profession:** Swordsman starting profession
- **Weapon types:** Slashing, Bashing, and Piercing damage based on weapon type

Swords is one of the most balanced melee combat skills, with moderate Strength and Dexterity scaling. It is available from character creation.

### Mace Fighting

**Title:** Armsman | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Mace Fighting is the skill used with mace-type weapons (maces, pickaxes, flails, etc.). It has the highest Strength scale among all combat skills.

### Mechanics

- **Stat scaling:** +0.09 Str and +0.01 Dex per point of those stats (highest Str scale of any combat skill)
- **Gain rates:** 0.9 Str gain, 0.1 Dex gain per use
- **StatTotal:** 10
- **Weapon types:** Slashing, Bashing, and Piercing damage

Mace Fighting is the most Strength-reliant combat skill, making it ideal for pure Strength characters.

### Fencing

**Title:** Fencer | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

Fencing is the skill used with fencing-type weapons (rapiers, long swords, etc.). It has the highest Dexterity scale among all melee weapon skills.

### Mechanics

- **Stat scaling:** +0.045 Str and +0.055 Dex per point of those stats
- **Gain rates:** 0.45 Str gain, 0.55 Dex gain per use
- **StatTotal:** 10
- **Profession:** Fencer starting profession
- **Weapon types:** Piercing and Slashing damage

Fencing is the most Dexterity-reliant melee weapon skill, making it ideal for Dexterity-based characters.

### Wrestling

**Title:** Wrestler | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Wrestling is the skill used for unarmed combat. It shares the highest Strength gain rate with Mace Fighting.

### Mechanics

- **Stat scaling:** +0.09 Str and +0.01 Dex per point of those stats (tied with Macing for highest Str scale)
- **Gain rates:** 0.9 Str gain, 0.1 Dex gain per use (tied with Macing)
- **StatTotal:** 10
- **Primary use:** Unarmed combat

Wrestling is the foundation of unarmed combat and is available from character creation.

---

## Ranged Weapons

### Archery

**Title:** Archer | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

Archery is the skill used with bows and crossbows. It is the profession skill for the Archer starting profession.

### Mechanics

- **Stat scaling:** +0.025 Str and +0.075 Dex per point of those stats
- **Gain rates:** 0.25 Str gain, 0.75 Dex gain per use
- **StatTotal:** 10
- **Profession:** Archer starting profession
- **Weapon types:** Ranged damage

Archery is the most Dexterity-reliant ranged combat skill and is available from character creation.

### Throwing

**Title:** Bladeweaver | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

Throwing is the skill used with thrown weapons (daggers, knives, shuriken, fukiya darts, etc.). It is expansion-gated to **SA (Samurai Adventure)** and is restricted to Gargoyle characters.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SA expansion
- **Race restriction:** Gargoyles only (per `CharacterCreation.ValidateSkills`)
- **Weapon types:** Thrown ranged weapons

Thrown weapons have dynamic range based on the attacker's Strength:
```
DefMaxRange = baseRange - 3 + (Str - AosStrengthReq) / ((140 - AosStrengthReq) / 3)
```

After hitting or missing, thrown weapons return to the thrower after 0.3 seconds (unless the `MysticArc` weapon ability is active).

See also: [[items/weapons]]

---

## Combat Techniques

### Tactics

**Title:** Tactician | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Tactics is a combat technique skill that has no stat scaling but affects combat calculations. It is available from character creation.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Primary use:** Enhancing combat effectiveness

Tactics is one of the "pure" combat skills that functions independently of stat influence. It affects hit chance and damage calculations in combat.

### Parrying

**Title:** Duelist | **Primary Stat:** Dexterity | **Secondary Stat:** Strength

Parrying is the skill used to reduce incoming melee damage. It is the profession skill for the Duelist starting profession.

### Mechanics

- **Stat scaling:** +0.075 Str and +0.025 Dex per point of those stats
- **Gain rates:** 0.75 Str gain, 0.25 Dex gain per use
- **StatTotal:** 10
- **Profession:** Duelist starting profession (also known as Paladin in some contexts)
- **Primary use:** Reducing incoming melee damage

Parrying is the only combat skill with a non-zero Str scale among the defensive techniques.

### Focus

**Title:** Driven | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Focus is a combat technique skill that reduces stamina cost of weapon abilities. It is expansion-gated to **AOS (Age of Shadows)**.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires AOS expansion
- **Primary use:** Reducing stamina cost of weapon abilities

Focus is one of the "pure" AOS+ skills with zero stat scales. It functions as a force multiplier for weapon ability usage.

See also: [[systems/combat]]

### Bushido

**Title:** Samurai | **Primary Stat:** Strength | **Secondary Stat:** Intelligence

Bushido is a combat technique skill that provides special abilities for samurai characters. It is expansion-gated to **SE ( Samurai Edition)** and is the profession skill for the Samurai starting profession.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SE expansion
- **Profession:** Samurai starting profession
- **Primary use:** Samurai-specific combat techniques

Bushido provides abilities such as Hono (Fire), Kitsui (Speed), and Chi (Spirit) that enhance combat effectiveness.

See also: [[spells/bushido]]

### Ninjitsu

**Title:** Ninja | **Primary Stat:** Dexterity | **Secondary Stat:** Intelligence

Ninjitsu is a combat technique skill that provides special abilities for ninja characters. It is expansion-gated to **SE (Samurai Edition)** and is the profession skill for the Ninja starting profession.

### mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SE expansion
- **Profession:** Ninja starting profession
- **Primary use:** Ninja-specific combat techniques

Ninjitsu provides abilities such as Hide Again, Speed, and Swashbuckler that enhance stealth-based combat.

See also: [[spells/ninjitsu]]

---

## Expansion Notes

| Skill | Expansion | Notes |
|-------|-----------|-------|
| Focus | AOS (Age of Shadows) | Zero stat scales |
| Bushido | SE (Samurai Edition) | Samurai profession, zero stat scales |
| Ninjitsu | SE (Samurai Edition) | Ninja profession, zero stat scales |
| Throwing | SA (Samurai Adventure) | Gargoyle race only, zero stat scales |
| All others | Base/AOS | Available from character creation |

See [[expansions/timeline]] for expansion details.

---

## Combat System Integration

Combat skills integrate with the core combat engine defined in `Mobile.cs` and `BaseWeapon.cs`. Key concepts include:

- **Weapon types:** Each weapon has a damage type (Slashing, Bashing, Piercing) that interacts with resistance calculations
- **Swing speed:** Determined by weapon speed and character stats
- **Hit chance:** Affected by Tactics, Parry, and other combat skills
- **Weapon abilities:** Certain skills (Focus) reduce the stamina cost of weapon abilities
- **Stat requirements:** Weapons have Strength requirements that affect swing speed

See [[systems/combat]] for the complete combat system documentation.

---

## See Also

- [[reference/skill-table]] — Complete skill data for all 58 skills
- [[systems/combat]] — Combat mechanics and formulas
- [[getting-started/stats]] — Stat-skill relationships
- [[getting-started/character-creation]] — Starting skill points and professions
- [[items/weapons]] — Weapon mechanics and systems
