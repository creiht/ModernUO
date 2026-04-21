# Magical Skills

Magical skills are related to spellcasting, resistance, and magical abilities. ModernUO features **8 magical skills** that form the foundation of a spellcaster's capabilities.

Magical skills are defined in `Distribution/Data/skills.json` and grouped in `SkillsInfo.cs` (MagicSkills array, lines 83-99). All 8 skills share a uniform `GainFactor` of 4.

## Magical Skill Table

| Skill | Title | Primary | Secondary | Str Scale | Dex Scale | Int Scale | Str Gain | Dex Gain | Int Gain |
|-------|-------|---------|-----------|-----------|-----------|-----------|----------|----------|----------|
| Chivalry | Paladin | Str | Int | 0 | 0 | 0 | 0 | 0 | 0 |
| EvalInt | Scholar | Int | Str | 0 | 0 | 0 | 0 | 0 | 1 |
| Magery | Mage | Int | Str | 0 | 0 | 0.15 | 0 | 0 | 1.5 |
| Meditation | Stoic | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Mysticism | Mystic | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Necromancy | Necromancer | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| Spellweaving | Arcanist | Int | Str | 0 | 0 | 0 | 0 | 0 | 0 |
| MagicResist | Warder | Str | Dex | 0 | 0 | 0 | 0.25 | 0.25 | 0.5 |

See also: [[reference/skill-table]] for the complete skill data.

---

## Spellcasting Foundations

### Magery

**Title:** Mage | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Magery is the foundational magical skill, required for casting spells from all spell schools. It has the highest Intelligence scale among all magical skills.

### Mechanics

- **Stat scaling:** +0.15 Int per point of Intelligence (highest Int scale of any skill)
- **Gain rates:** 1.5 Int gain per use (highest Int gain of any skill)
- **StatTotal:** 15
- **Primary use:** Casting spells from all spell schools
- **Spell access:** Required for First through Eighth circle spells

Magery is the most Intelligence-reliant skill in the game and the primary stat driver for all spellcasters. Without Magery, a character cannot cast any spells. The `SpellInfo.Magery` requirement for each spell determines the minimum Magery level needed.

See also: [[spells/magery]]

### Evaluating Intelligence

**Title:** Scholar | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Evaluating Intelligence is used to assess the mental capabilities of other creatures. It estimates target Intelligence and Mana percentage.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 1.0 Int gain per use (tied highest Int gain)
- **Primary use:** Assessing target Intelligence and Mana

The skill works by targeting a creature and estimating their Int and Mana values:
```
marginOfError = max(0, 20 - (int)(EvalInt.Value / 5))
estimatedInt = target.Int + random(-marginOfError, +marginOfError)
estimatedMana = (target.Mana * 100 / target.ManaMax) + random(-marginOfError, +marginOfError)
```

At EvalInt >= 76.0, the user can also see the target's Mana percentage.

See source: `Skills/EvalInt.cs`

---

## Magical Resistance

### Resisting Spells (MagicResist)

**Title:** Warder | **Primary Stat:** Strength | **Secondary Stat:** Dexterity

Resisting Spells is used to reduce the damage and effects of incoming magical attacks. It is the defensive counterpart to Magery.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 0.25 Str gain, 0.25 Dex gain, 0.5 Int gain per use
- **Primary use:** Reducing incoming magical damage

MagicResist is the only magical skill with non-zero gain rates across all three stats. It functions as a defensive skill that reduces the effectiveness of enemy spells.

See also: [[systems/combat]]

---

## Mana Recovery

### Meditation

**Title:** Stoic | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Meditation is used to enter a trance state that significantly increases mana regeneration rate. It is one of the most important skills for spellcasters.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Primary use:** Faster mana recovery
- **Cooldown:** 10 seconds (5 seconds pre-AOS)

Meditation has specific preconditions that must be met:

**Pre-AOS:**
- Must have >= 10% HP

**AOS+:**
- `RegenRates.GetArmorOffset(m) == 0` (no meditation-blocking armor)
- Hands must be free or holding only spellbook/runebook/spell-channeling items

**Success chance:**
```
chance = (50.0 + (skillVal - (manaMax - mana)) * 2) / 100
```

Higher skill increases the chance of success. Lower mana deficit (closer to full mana) decreases the chance, as the skill is less useful when already near full mana.

**On success:**
- Sets `m.Meditating = true`
- Adds `BuffIcon.ActiveMeditation` buff icon
- Plays meditation sound (0xF9)

See source: `Skills/Meditation.cs`

See also: [[getting-started/stats]] — Mana regeneration formulas

---

## Magical Combat Skills

### Chivalry

**Title:** Paladin | **Primary Stat:** Strength | **Secondary Stat:** Intelligence

Chivalry is a magical combat skill that provides holy abilities for paladin characters. It is expansion-gated to **AOS (Age of Shadows)** and is the profession skill for the Paladin starting profession.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires AOS expansion
- **Profession:** Paladin starting profession
- **Primary use:** Holy-themed magical combat abilities

Chivalry provides abilities such as Divine Fury, Enemy of One, Honorific Bassinas, and Divine Fury that enhance paladin combat effectiveness.

See also: [[spells/chivalry]]

### Necromancy

**Title:** Necromancer | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Necromancy is a magical combat skill that provides death-themed abilities. It is expansion-gated to **AOS (Age of Shadows)** and is the profession skill for the Necromancer starting profession.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires AOS expansion
- **Profession:** Necromancer starting profession
- **Primary use:** Death-themed magical abilities

Necromancy provides abilities such as Corpse Skin, Blood Oath, and Pain Spike that enhance necromancer combat effectiveness.

See also: [[spells/necromancy]]

---

## Advanced Magical Schools

### Spellweaving

**Title:** Arcanist | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Spellweaving is an advanced magical skill that enhances spellcasting efficiency. It is expansion-gated to **ML (Mondain's Legacy)**.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires ML expansion
- **Primary use:** Enhancing spellcasting efficiency

Spellweaving provides abilities that reduce spellcasting recast delays and enhance spell effectiveness. It works in conjunction with the Magery skill to optimize spellcasting.

See also: [[spells/spellweaving]]

### Mysticism

**Title:** Mystic | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

Mysticism is an advanced magical skill that provides teleportation and dimension-based abilities. It is expansion-gated to **SA (Samurai Adventure)**.

### Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Expansion gate:** Requires SA expansion
- **Primary use:** Teleportation and dimension-based abilities

Mysticism provides abilities such as Arch Cure, Arch Protection, and Magic Trap that enhance magical combat capabilities.

See also: [[spells/mysticism]]

---

## Expansion Notes

| Skill | Expansion | Notes |
|-------|-----------|-------|
| Chivalry | AOS (Age of Shadows) | Paladin profession, zero stat scales |
| Necromancy | AOS (Age of Shadows) | Necromancer profession, zero stat scales |
| Spellweaving | ML (Mondain's Legacy) | Zero stat scales |
| Mysticism | SA (Samurai Adventure) | Zero stat scales |
| All others | Base/AOS | Available from character creation |

See [[expansions/timeline]] for expansion details.

---

## Spell School Integration

Magical skills integrate with the spell system defined in `Spells/`. Key concepts include:

- **Magery requirement:** Each spell has a minimum Magery level defined in `SpellInfo.Magery`
- **Spell schools:** First through Eighth circle, Chivalry, Necromancy, Bushido, Ninjitsu, Mysticism, Spellweaving, Gargoyle
- **Recast delays:** Modified by Spellweaving skill
- **Mana costs:** Determined by `SpellInfo.Mana` for each spell

See [[spells/magery]], [[spells/chivalry]], [[spells/necromancy]], [[spells/bushido]], [[spells/ninjitsu]], [[spells/mysticism]], [[spells/spellweaving]], [[spells/gargoyle]] for spell school documentation.

---

## See Also

- [[reference/skill-table]] — Complete skill data for all 58 skills
- [[systems/combat]] — Combat mechanics and formulas
- [[getting-started/stats]] — Stat-skill relationships
- [[spells/magery]] — First through Eighth circle spells
- [[spells/chivalry]] — Chivalry spells
- [[spells/necromancy]] — Necromancy spells
