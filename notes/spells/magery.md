# Magery

The foundational magic school, covering all 8 circles of traditional magic. Magery is the first spell school available to players and includes the widest variety of spells: damage, healing, summoning, utility, and area effects.

Magery relies on the **Magery** skill as its casting skill and **Evaluate Intelligence** as its damage skill. The chance to successfully cast a spell is determined by the spell's circle via a required skill range (see *Key Mechanics* below).

## Key Mechanics

### Mana Table

Mana cost increases with circle:

| Circle | Mana |
|--------|------|
| First  | 4    |
| Second | 6    |
| Third  | 9    |
| Fourth | 11   |
| Fifth  | 14   |
| Sixth  | 20   |
| Seventh | 40  |
| Eighth | 50   |

### Cast Time

Cast time scales linearly with circle: `(3 + Circle) x 0.25` seconds.

| Circle | Cast Time |
|--------|-----------|
| First  | 1.00s     |
| Second | 1.25s     |
| Third  | 1.50s     |
| Fourth | 1.75s     |
| Fifth  | 2.00s     |
| Sixth  | 2.25s     |
| Seventh | 2.50s    |
| Eighth | 2.75s     |

### Skill Requirements

The required Magery skill for cast success follows the OSI algorithm:

| Circle | Min Skill | Max Skill |
|--------|-----------|-----------|
| First  | -46.0     | -6.0      |
| Second | -32.0     | 8.0       |
| Third  | -18.0     | 22.0      |
| Fourth | -4.0      | 36.0      |
| Fifth  | 10.0      | 50.0      |
| Sixth  | 24.0      | 64.0      |
| Seventh | 38.0     | 78.0      |
| Eighth | 52.0      | 92.0      |

### Damage Formula

Spell damage is calculated through the following chain:

1. Base dice: `GetNewAosDamage(bonus, dice, sides, target)`
2. Inscribe bonus: `(Inscribe + 1000 * (Inscribe / 1000)) / 200`
3. Intelligence bonus: `Int / 10`
4. Spell Damage Increase (SDI) from equipment (capped at 15% in PvP)
5. Evaluate Intelligence scaling: `30 + 9 * EvalInt / 100`
6. Slayer modifiers
7. Magic Resist resistance check

### Reagent Alternatives

- **Arcane Gem**: Consumes charges instead of reagents for Magery and Necromancy spells. Required charges = `1 + Circle` (AOS+), or `1` (SE).
- **Wands**: Spells cast from wands require no reagents and consume charges instead. Wands bypass reagent consumption entirely.

### Special Casters

Paladins with Magery skill of 70+ are subject to a Faster Cast Recovery (FCR) cap of 2 (same as Magery spellcasters), while lower paladins use a cap of 4.

## Damage Spells

Spells that deal direct damage to a target or area.

### First Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Magic Arrow | `In Por Ylem` | Sulfurous Ash | 10-14 fire damage (AOS). Delayed damage (1.25s). Reflectable via Magic Reflection. |
| Weaken | `Des Mani` | Garlic, Nightshade | Reduces target's Strength by a percentage of RawStr. Reflectable. Duration: `6 * EvalInt / 5` seconds. |
| Clumsy | `Uus Jux` | Bloodmoss, Nightshade | Reduces target's Dexterity by a percentage of RawDex. Reflectable. Duration: `6 * EvalInt / 5` seconds. |
| Feeblemind | `Rel Wis` | Ginseng, Nightshade | Reduces target's Intelligence by a percentage of RawInt. Reflectable. Duration: `6 * EvalInt / 5` seconds. |

### Second Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Harm | `An Mani` | Nightshade, Spider's Silk | 17-21 cold damage (AOS). Immediate damage. Reflectable. No slayer scalar. |
| Curse | `Des Sanct` | Nightshade, Garlic, Sulfurous Ash | Reduces ALL stats (Str, Dex, Int) simultaneously. Reflectable. Duration refresh only if stronger. |

### Third Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Fireball | `Vas Flam` | Black Pearl | 19-23 fire damage (AOS). Delayed damage (1.25s). Reflectable. |

### Fourth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Lightning | `Por Ort Grav` | Mandrake Root, Sulfurous Ash | 12-20 energy damage (AOS). Immediate damage. Reflectable. |
| Mana Drain | `Ort Rel` | Black Pearl, Mandrake Root, Spider's Silk | Drains `clamp(40 + (EvalInt - MagicResist), 0, target.Mana)` mana. Refunded after 5 seconds (AOS). 99% resist percent. |

### Fifth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Mind Blast | `Por Corp Wis` | Black Pearl, Mandrake Root, Nightshade, Sulfurous Ash | `(Magery + Int) / 5` energy damage (AOS), max 60. Delayed damage (1s). Reflectable. No slayer scalar. |
| Energy Bolt | `Corp Por` | Black Pearl, Nightshade | 24-41 energy damage (AOS). Delayed damage (1.25s). Reflectable. |
| Explosion | `Vas Ort Flam` | Bloodmoss, Mandrake Root | 23-44 fire damage (AOS). Delayed damage (3s AOS / 2.5s pre-AOS). Stacks with itself. Reflectable. |
| Mass Curse | `Vas Des Sanct` | Garlic, Nightshade, Mandrake Root, Sulfurous Ash | Applies stat curse to ALL stats on all valid targets in 2-tile radius area. Requires LOS per target. |

### Sixth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Chain Lightning | `Vas Ort Grav` | Black Pearl, Bloodmoss, Mandrake Root, Sulfurous Ash | 27-48 energy damage (AOS: 51 base) to all targets in 2-tile radius. Split damage if >2 targets. Delayed damage. Requires LOS (AOS). |
| Meteor Swarm | `Flam Kal Des Ylem` | Bloodmoss, Mandrake Root, Sulfurous Ash, Spider's Silk | 27-48 fire damage (AOS: 51 base) to all targets in 2-tile radius. Same mechanics as Chain Lightning but fire damage. |

### Seventh Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Fireball | `Vas Flam` | Black Pearl | 27-48 fire damage (AOS). Delayed damage. Reflectable. |
| Mana Vampire | `Ort Sanct` | Black Pearl, Bloodmoss, Mandrake Root, Spider's Silk | Drains `EvalInt - MagicResist` mana from target (half on creatures). Transfers to caster. 98% resist percent. Reflectable. |

### Eighth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Earthquake | `In Vas Por` | Bloodmoss, Ginseng, Mandrake Root, Sulfurous Ash | Physical damage to all targets in `1 + Magery/15` radius. AOS: `Hits / 2 + 0-15` (clamped 15-100 for creatures). Immediate damage (AOS) / Delayed (pre-AOS). No resist check. |
| Energy Vortex | `Vas Corp Por` | Bloodmoss, Black Pearl, Mandrake Root, Nightshade | Summons EnergyVortex (1 follower slot SE / 2 AOS+). Duration: 90s (T2A+) or 40-80s. Requires LOS. |

## Healing Spells

Spells that restore health or cure negative conditions.

| Spell | Circle | Mantra | Reagents | Effect |
|-------|--------|--------|----------|--------|
| Heal | 1st | `In Mani` | Garlic, Ginseng, Spider's Silk | Heals `Magery / 12 + 1-4` HP (AOS). Cannot heal undead, golems, poisoned, or Mortal Strike wounded. |
| Greater Heal | 4th | `In Vas Mani` | Garlic, Ginseng, Mandrake Root, Spider's Silk | Heals `Magery * 0.4 + 1-10` HP. Same restrictions as Heal. |
| Arch Cure | 4th | `Vas An Nox` | Garlic, Ginseng, Mandrake Root | Cures poison in 2-tile radius. Cast time: 0.06s (25% faster). Cannot cure aggressors, victims, murderers. |

## Buff Spells

Spells that enhance the caster or allies.

| Spell | Circle | Mantra | Reagents | Effect |
|-------|--------|--------|----------|--------|
| Agility | 2nd | `Ex Uus` | Bloodmoss, Mandrake Root | Adds Dex bonus = percentage of RawDex. |
| Strength | 2nd | `Uus Mani` | Mandrake Root, Nightshade | Adds Str bonus = percentage of RawStr. |
| Cunning | 2nd | `Uus Wis` | Mandrake Root, Nightshade | Adds Int bonus = percentage of RawInt. |
| Bless | 3rd | `Rel Sanct` | Garlic, Mandrake Root | Adds stat bonus to ALL three stats simultaneously. |
| Protection | 2nd | `Uus Sanct` | Garlic, Ginseng, Sulfurous Ash | Toggle: `-15 + Inscribe/20` Physical, `-35 + Inscribe/20` Magic Resist, `+2` slower cast speed. Persists through logout. |
| Arch Protection | 4th | `Vas Uus Sanct` | Garlic, Ginseng, Mandrake Root, Sulfurous Ash | Applies Protection to self and party members in 2-tile radius (AOS). |
| Reactive Armor | 1st | `Flam Sanct` | Garlic, Spider's Silk, Sulfurous Ash | Toggle (AOS): `+15 + Inscribe/20` Physical, `-5` all elemental resistances. Persists through logout. Pre-AOS: 10-35% melee damage reflection. |
| Magic Reflection | 5th | `In Jux Sanct` | Garlic, Mandrake Root, Spider's Silk | Toggle (AOS): `-25 + Inscribe/20` Physical, `+10` all elemental resistances. Reflects incoming magic spells. Persists through logout. |
| Divine Fury | Chiv | `Divinum Furis` | Tithing: 10, Mana: 15 | Restores full stamina. Duration: 7-24s (Karma-scaled). Attack/Damage/Speed bonuses at 120 Chivalry. |
| Consecrate Weapon | Chiv | `Consecrus Arma` | Tithing: 10, Mana: 10 | Weapon damage converts to target's worst resistance. Duration: 3-11s (Karma-scaled). |
| Noble Sacrifice | Chiv | `Dium Prostra` | Tithing: 30, Mana: 20 | Sacrifices self to cure/heal target. Cures all curses, poison, paralysis. Heals 8-24 HP. |

## Utility Spells

Spells for travel, concealment, and other non-combat purposes.

| Spell | Circle | Mantra | Reagents | Effect |
|-------|--------|--------|----------|--------|
| Night Sight | 1st | `In Lor` | Sulfurous Ash, Spider's Silk | Sets LightLevel for dungeon detection. Cannot stack (excludes from LightCycle). |
| Create Food | 1st | `In Mani Ylem` | Garlic, Ginseng, Mandrake Root | Creates random food item into caster's backpack. 10 varieties. |
| Teleport | 3rd | `Rel Por` | Bloodmoss, Mandrake Root | Teleports to target location. Travel restrictions apply (dungeons, safe zones, etc.). |
| Recall | 4th | `Kal Ort Por` | Black Pearl, Bloodmoss, Mandrake Root | Teleports to home or runebook mark. Consumes runebook charge. |
| Mark | 6th | `Kal Por Ylem` | Black Pearl, Bloodmoss, Mandrake Root | Sets a RecallRune's mark to caster's location. |
| Gate Travel | 7th | `Vas Rel Por` | Black Pearl, Mandrake Root, Sulfurous Ash | Creates linked moongate pair. Lasts 30s (T2A+) / 10s. Cross-facet travel allowed. |
| Incognito | 5th | `Kal In Ex` | Bloodmoss, Garlic, Nightshade | Transforms appearance to random human. Duration: `min(Magery * 6/5, 144)` seconds. |
| Invisibility | 6th | `An Lor Xen` | Bloodmoss, Nightshade | Sets Hidden = true. Duration: `1.2 * Magery` seconds. Reveals on attacking. |
| Reveal | 6th | `Wis Quas` | Bloodmoss, Sulfurous Ash | Reveals hidden mobiles in `1 + Magery/20` radius. Always reveals Invisibility targets. |
| Holy Light | Chiv | `Augus Luminos` | Tithing: 10, Mana: 10 | Deals 8-24 damage to enemies in 3-tile radius area. |
| Cleanse By Fire | Chiv | `Expor Flamus` | Tithing: 10, Mana: 10 | Cures poison. Caster takes 13-55 fire damage (Karma-scaled). |
| Remove Curse | Chiv | `Extermo Vomica` | Tithing: 10, Mana: 20 | Removes curses. Success chance based on Karma. |
| Sacred Journey | Chiv | `Sanctum Viatas` | Tithing: 15, Mana: 10 | Teleports caster and pets to runebook location. |
| Enemy of One | Chiv | `Forul Solum` | Tithing: 10, Mana: 20 | Select target type for bonus damage. Duration: 1.5-3.5 minutes. |
| Dispel Evil | Chiv | `Dispiro Malas` | Tithing: 10, Mana: 10 | Dispel summons, flee evil creatures, drain mana from necro transform casters in 8-tile radius. |

## Summoning Spells

Spells that call creatures to fight for you.

| Spell | Circle | Mantra | Reagents | Followers | Duration |
|-------|--------|--------|----------|-----------|----------|
| Summon Creature | 5th | `Kal Xen` | Bloodmoss, Mandrake Root, Spider's Silk | 2 | `Magery * 4` seconds (expansions) |
| Blade Spirits | 5th | `In Jux Hur Ylem` | Black Pearl, Mandrake Root, Nightshade | 1 | 120s (AOS) |
| Air Elemental | 8th | `Kal Vas Xen Hur` | Bloodmoss, Mandrake Root, Spider's Silk | 2 | `4 * max(5, Magery)` seconds |
| Earth Elemental | 8th | `Kal Vas Xen Ylem` | Bloodmoss, Mandrake Root, Spider's Silk | 2 | Same as Air Elemental |
| Fire Elemental | 8th | `Kal Vas Xen Flam` | Bloodmoss, Mandrake Root, Spider's Silk, Sulfurous Ash | 4 | Same as Air Elemental |
| Water Elemental | 8th | `Kal Vas Xen An Flam` | Bloodmoss, Mandrake Root, Spider's Silk | 3 | Same as Air Elemental |
| Summon Daemon | 8th | `Kal Vas Xen Corp` | Bloodmoss, Mandrake Root, Spider's Silk, Sulfurous Ash | 4 SE / 5 AOS+ | Same as Air Elemental |
| Resurrection | 8th | `An Corp` | Bloodmoss, Garlic, Ginseng | N/A | Revives dead player within 1 tile |
| Summon Fey | SW | `Alalithra` | Tithing: 10, Mana: 10 | 1 + FocusLevel | `Spellweaving/24 + FocusLevel * 2` minutes |
| Summon Fiend | SW | `Nylisstra` | Tithing: 10, Mana: 10 | 1 + FocusLevel | Same as Summon Fey |
| Nature's Fury | SW | `Rauvvrae` | Tithing: 0, Mana: 24 | 1 | `Spellweaving/24 + 25 + FocusLevel * 2` seconds |

## Field Spells

Spells that create persistent area effects on the ground.

| Spell | Circle | Mantra | Reagents | Effect |
|-------|--------|--------|----------|--------|
| Fire Field | 4th | `In Flam Grav` | Black Pearl, Spider's Silk, Sulfurous Ash | 5 fire tiles, 2 damage/tick. Duration scales with expansion. |
| Poison Field | 5th | `In Nox Grav` | Black Pearl, Nightshade, Spider's Silk | 5 poison tiles. Poison level based on Magery+Poisoning skill. |
| Paralyze Field | 6th | `In Ex Grav` | Black Pearl, Ginseng, Spider's Silk | 5 paralyze tiles. Duration: `2 + (EvalInt/10 - MagicResist/10)` (AOS). |
| Energy Field | 7th | `In Sanct Grav` | Black Pearl, Mandrake Root, Spider's Silk, Sulfurous Ash | 5 invisible blocking tiles. Blocks non-allied players. |
| Wall of Stone | 3rd | `In Sanct Ylem` | Bloodmoss, Garlic | 3 invisible wall tiles. Lasts 10 seconds. Dispellable. |
| Dispel Field | 5th | `An Grav` | Black Pearl, Spider's Silk, Sulfurous Ash, Garlic | Destroys any dispellable field item. |

## Debuffing Spells

Spells that weaken or hinder opponents.

| Spell | Circle | Mantra | Reagents | Effect |
|-------|--------|--------|----------|--------|
| Clumsy | 1st | `Uus Jux` | Bloodmoss, Nightshade | Reduces Dex by percentage of RawDex. |
| Weaken | 1st | `Des Mani` | Garlic, Nightshade | Reduces Str by percentage of RawStr. |
| Feeblemind | 1st | `Rel Wis` | Ginseng, Nightshade | Reduces Int by percentage of RawInt. |
| Curse | 4th | `Des Sanct` | Nightshade, Garlic, Sulfurous Ash | Reduces ALL stats simultaneously. |
| Mass Curse | 6th | `Vas Des Sanct` | Garlic, Nightshade, Mandrake Root, Sulfurous Ash | Area curse affecting all stats and all targets. |
| Poison | 3rd | `In Nox` | Nightshade | Applies poison level based on Magery+Poisoning. Level 0-4. |
| Paralyze | 5th | `An Ex Por` | Garlic, Mandrake Root, Spider's Silk | Paralyzes target. Duration: `clamp(EvalInt/10 - MagicResist/10, 0)` (AOS), 3x on creatures. |
| Dispel | 6th | `An Ort` | Garlic, Mandrake Root, Sulfurous Ash | Attempts to dispel IsDispellable creatures. |
| Mass Dispel | 7th | `Vas An Ort` | Garlic, Mandrake Root, Black Pearl, Sulfurous Ash | Area dispel in 8-tile radius. |
| Evil Omen | Necro | `Pas Tym An Sanct` | Bat Wing, Nox Crystal | Sets Magic Resist to 50%. Next harmful event magnified. |
| Strangle | Necro | `In Bal Nox` | Daemon Blood, Nox Crystal | Damage over time based on SpiritSpeak. 40% reveal chance per tick. |

## Mantra Vocabulary

| Prefix | Meaning | Examples |
|--------|---------|----------|
| `Vas` | Greater/More | Vas Flam (Fireball), Vas An Nox (Arch Cure) |
| `An` | Anti/Against | An Nox (Cure), An Ort (Dispel) |
| `Ex` | Extinguish/Remove | Ex Uus (Agility), Ex Por (Unlock) |
| `In` | Apply/On | In Mani (Heal), In Lor (Night Sight) |
| `Rel` | Restore | Rel Sanct (Bless), Rel Wis (Feeblemind) |
| `Des` | Decrease/Weaken | Des Sanct (Curse), Des Mani (Weaken) |
| `Kal` | Summon/Call | Kal Xen (Summon Creature), Kal Por (Mark) |
| `Ort` | Location/Target | Ort Por (Telekinesis), Ort Grav (Lightning) |
| `Uus` | New/Improved | Uus Sanct (Protection), Uus Mani (Strength) |

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index with all 127 spells
- [[systems/combat]] — Combat mechanics and damage types
