# Chivalry

Chivalry is the Paladin spell school, focused on divine power, protection, and self-sacrifice. Paladins use tithing points (drawn from their karma pool) rather than mana for most spells, making this school unique among all magical disciplines.

Chivalry relies on the **Chivalry** skill for both casting and damage. Unlike magery spells, Chivalry spells scale their effectiveness through the `ComputePowerValue()` function, which derives power from the caster's Karma and Chivalry skill.

## Key Mechanics

### Tithing Points

Chivalry spells consume **Tithing Points** in addition to mana. Tithing points are drawn from the caster's karma pool:

- Positive karma = more tithing points available
- Negative karma = reduced or no tithing points
- `Lower Reg Cost` attribute can reduce tithing cost to 0

### Power Scaling

All Chivalry spell effects scale through `ComputePowerValue(karma)`:

```
power = sqrt(karma + 20000 + Chivalry * 100) / divisor
```

Higher karma and higher Chivalry skill = stronger spell effects and longer durations.

### Skill Requirements

Each spell has a minimum Chivalry skill requirement. Cast success range: `RequiredSkill` to `RequiredSkill + 50`.

### Mana Costs

Chivalry spells have their own mana costs (separate from tithing), ranging from 10 to 20 mana.

### Faster Casting

Paladins are subject to a Faster Cast Recovery (FCR) cap of 4 by default. If the Paladin has Magery skill of 70+, the cap increases to 2 (same as Magery spellcasters).

## Healing Spells

| Spell | Skill Req | Mana | Tithing | Effect |
|-------|-----------|------|---------|--------|
| Close Wounds | 0.0 | 10 | 10 | Heals 7-39 HP (Karma-scaled). Cannot heal undead, animated dead, poisoned, or Mortal Strike wounded. Melee range target. |
| Noble Sacrifice | 65.0 | 20 | 30 | Sacrifices caster (Hits/Stamina/Mana to 1) to cure and heal target. Removes all curses, poison, paralysis. Heals 8-24 HP. Can resurrect dead targets based on karma. |
| Cleanse By Fire | 5.0 | 10 | 10 | Cures poison from target. Caster takes 13-55 fire damage (Karma-scaled). |

### Noble Sacrifice — Detailed Mechanics

Noble Sacrifice is the most versatile Chivalry spell:

- **Living targets**: Cures poison, heals 8-24 HP, removes all stat curses (Str/Dex/Int reductions), removes paralysis, removes: Evil Omen, Strangle, Corpse Skin, Curse, Mortal Strike, Mind Rot, Blood Oath, Spell Plague, and related buff icons
- **Dead targets**: Chance to resurrect = `0.1 + 0.9 * Karma / 10000` (cannot resurrect in Khaldun)
- **Exclusions**: Cannot target self, undead, animated dead, or golems

## Buff Spells

| Spell | Skill Req | Mana | Tithing | Effect |
|-------|-----------|------|---------|--------|
| Consecrate Weapon | 15.0 | 10 | 10 | Enchants weapon to convert damage to target's worst resistance type. Duration: 3-11s (Karma-scaled). Requires holding a weapon. |
| Divine Fury | 25.0 | 15 | 10 | Restores full stamina. Duration: 7-24s. At 120 Chivalry + 10000 Karma: +15 Attack, +20 Damage, +15 Speed, -10 Defend. |
| Holy Light | 55.0 | 10 | 10 | Area buff (3-tile radius). Also deals 8-24 damage to enemies in range. |
| Sacred Journey | 15.0 | 10 | 15 | Teleports caster and pets to runebook entry location. Same restrictions as Recall. |

### Divine Fury — Detailed Mechanics

Divine Fury provides attack bonuses that scale with Chivalry skill. At maximum Chivalry (120) and high karma:

- **Attack Bonus**: +15
- **Damage Bonus**: +20
- **Weapon Speed**: -15 (faster swings)
- **Defend Malus**: -10 (reduced defense, trade-off for offense)

## Utility Spells

| Spell | Skill Req | Mana | Tithing | Effect |
|-------|-----------|------|---------|--------|
| Remove Curse | 5.0 | 20 | 10 | Removes curses from target. Success chance: 0% at karma < -5000, sqrt(Karma)+25 at karma 0-5624, 100% at karma >= 5625. Removes same curses as Noble Sacrifice. |

## Combat Spells

| Spell | Skill Req | Mana | Tithing | Effect |
|-------|-----------|------|---------|--------|
| Dispel Evil | 35.0 | 10 | 10 | Area effect (8-tile radius). Disperses summoned creatures, causes evil creatures (karma < 0) to flee for 30s, drains mana and stamina from necromancy transformation casters. |
| Enemy of One | 45.0 | 20 | 10 | Select a creature type for bonus damage against. Duration: 1.5-3.5 minutes (Karma-scaled). |

### Dispel Evil — Detailed Mechanics

Dispel Evil has three effects against different targets in its 8-tile radius:

1. **Summoned creatures**: Dispel chance = `(50 + 100 * (Chivalry - DispelDifficulty) / (DispelFocus * 2)) / 100 * dispelSkill / 100`
2. **Evil creatures** (karma < 0, not controlled): Flee chance based on fame and skill for 30 seconds
3. **Necromancy transformation casters**: Drains `5 * dispelSkill` mana and stamina

## Spell List

| Spell | Mantra | Skill Req | Mana | Tithing | Type |
|-------|--------|-----------|------|---------|------|
| Close Wounds | `Obsu Vulni` | 0.0 | 10 | 10 | Healing |
| Cleanse By Fire | `Expor Flamus` | 5.0 | 10 | 10 | Healing/Curse |
| Remove Curse | `Extermo Vomica` | 5.0 | 20 | 10 | Utility |
| Consecrate Weapon | `Consecrus Arma` | 15.0 | 10 | 10 | Buff |
| Sacred Journey | `Sanctum Viatas` | 15.0 | 10 | 15 | Travel |
| Divine Fury | `Divinum Furis` | 25.0 | 15 | 10 | Buff |
| Dispel Evil | `Dispiro Malas` | 35.0 | 10 | 10 | Combat |
| Enemy of One | `Forul Solum` | 45.0 | 20 | 10 | Combat |
| Holy Light | `Augus Luminos` | 55.0 | 10 | 10 | Combat |
| Noble Sacrifice | `Dium Prostra` | 65.0 | 20 | 30 | Healing |

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index
