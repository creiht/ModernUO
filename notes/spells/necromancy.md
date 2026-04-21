# Necromancy

Necromancy is the dark magic school of the undead, encompassing curses, life drain, transformation, and summoning. Necromancers use **SpiritSpeak** as their damage skill (not Evaluate Intelligence), making this school unique among all magical disciplines.

Necromancy spells are not affected by Faster Cast Recovery items (CastDelayFastScalar = 0 in SE). They consume both mana and reagents, and casting them incurs karma loss (increased by the `Increased Karma Loss` attribute in ML+).

## Key Mechanics

### SpiritSpeak as Damage Skill

Unlike all other spell schools, Necromancy uses **SpiritSpeak** (not Magery or Evaluate Intelligence) for damage calculation. This means:

- SpiritSpeak directly affects damage output for all necromancy spells
- SpiritSpeak also affects duration of many spells
- SpiritSpeak interacts with Magic Resist of the target for damage/resistance calculations

### Mana and Skill Requirements

Each spell has its own unique `RequiredMana` and `RequiredSkill`. There is no circle-based mana table — each spell is individually defined:

| Spell | Mana | Skill Req |
|-------|------|-----------|
| Pain Spike | 5 | 20.0 |
| Curse Weapon | 7 | 0.0 |
| Corpse Skin | 11 | 20.0 |
| Evil Omen | 11 | 20.0 |
| Wraith Form | 17 | 20.0 |
| Blood Oath | 13 | 20.0 |
| Mind Rot | 17 | 30.0 |
| Summon Familiar | 17 | 30.0 |
| Animate Dead | 23 | 40.0 |
| Horrific Beast | 11 | 40.0 |
| Poison Strike | 17 | 50.0 |
| Wither | 23 | 60.0 |
| Strangle | 29 | 65.0 |
| Lich Form | 23 | 70.0 |
| Exorcism | 40 | 80.0 |
| Vengeful Spirit | 41 | 80.0 |
| Vampiric Embrace | 23 | 99.0 |

### Karma Loss

Casting necromancy spells causes karma loss:

```
karma = -(40 + 10 * (CastDelayBase.TotalSeconds / 0.25))
```

With `Increased Karma Loss` attribute (ML+):
```
finalKarma = karma + scale(karma, IncreasedKarmaLoss)
```

### Transformation Spells

Necromancy includes 4 transformation spells that change the caster's appearance and grant special abilities:

| Spell | Body | Resist Offsets | Special |
|-------|------|---------------|---------|
| Lich Form | 749 | Fire -10, Cold +10, Poison +10 | -1 HP per 2.5s tick |
| Wraith Form | 747/748 | Physical +15, Fire -5, Energy -5 | Mana leech: 5-20% |
| Horrific Beast | 746 | None | BlockedByHorrificBeast = false (others can coexist) |
| Vampiric Embrace | 745/744 | Fire -25 | Cast range 80-120 at 99+ skill |

Transformation spells can coexist with each other (except Horrific Beast blocks other spell casting). The `BlockedByHorrificBeast` flag determines whether a spell can be cast while in Horrific Beast form.

### Animate Dead — Creature Summoning

Animate Dead is one of the most complex spells, summoning undead from corpses:

**Requirements**:
- Target must be a corpse (ItemID 0x2006, not animated)
- Cannot target player corpses
- Owner's Fame must be >= 100
- Cannot target summoned or bonded creatures

**Creature pools by corpse type**:

| Corpse Category | Summon Options |
|----------------|----------------|
| Insects | Mound of Maggots (0 requirement) |
| Mounts | Hell Steed (10000), Skeletal Mount (0) |
| Elementals | Wailing Banshee (5000), Wraith (0) |
| Dragons | Skeletal Dragon (18000), Flesh Golem (10000), Lich (5000), Skeletal Knight (3000), Mummy (2000), Skeletal Mage (1000), Patchwork Skeleton (0) |
| Default | Same as Dragons + Lich Lord (18000) |
| Silver slayers | Cannot be animated |

**Caster ability calculation**:
```
ability = (Necromancy * 30 + SpiritSpeak * 70) / 10 * 18
clamped to corpse owner's Fame
```

**Limitations**:
- Maximum 3 animated creatures per caster
- Animated creatures take 1 HP damage every 1.65 seconds
- Skeletal Dragons have 50% HP/Strength reduction

## Damage Spells

Spells that deal direct damage to targets.

| Spell | Mana | Skill Req | Reagents | Effect |
|-------|------|-----------|----------|--------|
| Pain Spike | 5 | 20.0 | Grave Dust, Pig Iron | Initial: max((SpiritSpeak-Resist)/10 + (player?18:30), 1). Refresh: 3-7 damage + extends duration 2s. Duration: 10s. Restores initial damage to target if alive after duration. |
| Wither | 23 | 60.0 | Nox Crystal, Grave Dust, Pig Iron | Cold damage to 4-5 tile radius. Damage: `random(30,35) * (300 + Karma/100 + SpiritSpeak*10) / 1000`. SDI capped at 15% PvP. |
| Evil Omen | 11 | 20.0 | Bat Wing, Nox Crystal | Sets target's Magic Resist to 50%. Next harmful event: +25% damage, +1 poison level. Single-use (cleared on harmful event). |
| Poison Strike | 17 | 50.0 | Nox Crystal | Area damage (2-tile radius). Main target: `random(32/36,40) * (300+SpiritSpeak*9)/1000`. 1 tile: 50%. 2 tiles: 33%. SDI capped at 15% PvP. |
| Exorcism | 40 | 80.0 | Nox Crystal, Grave Dust | Only in champion spawn regions. Teleports dead player corpses to nearest shrine. Range: 48 (ML) / 18. Excludes party/allied guild/faction. |

### Pain Spike — Detailed Mechanics

Pain Spike is a damage-over-time spell with a twist:

- **Initial damage**: `max((SpiritSpeak - ResistMagic) / 10 + (PlayerMobile ? 18 : 30), 1)`
- **Duration**: 10 seconds
- **Refresh**: Recasting deals 3-7 damage and extends duration by 2 seconds
- **After duration**: If target is still alive, they restore the initial damage amount (the "debt" is forgiven)
- **Damage type**: Direct damage with life leech to caster
- **Buff icon**: Shows damage per tick

### Wither — Detailed Mechanics

Wither is an area-of-effect cold damage spell:

- **Range**: 4-tile radius (ML) / 5-tile radius (legacy)
- **Damage formula**: `random(30,35) * (300 + Karma/100 + SpiritSpeak*10) / 1000`
- **SDI bonus**: Capped at 15% for PvP (SE+)
- **Targeting**: Similar to Poison Strike (main target + radius)
- **Cast time**: 1.0s (legacy) / 1.5s (ML) / 1.25s (SA)

### Poison Strike — Detailed Mechanics

Poison Strike is a multi-target poison damage spell:

- **Radius**: 2 tiles around target
- **Damage by distance**:
  - Main target: full damage
  - 1 tile away: 50% damage
  - 2 tiles away: 33% damage
- **Damage formula**: `random(32/36, 40) * (300 + SpiritSpeak * 9) / 1000`
- **SDI**: Up to 15% for PvP
- **Special**: Animated dead cannot target familiars, players, or pets

## Buff/Transformation Spells

Spells that transform the caster or grant temporary abilities.

| Spell | Mana | Skill Req | Reagents | Effect |
|-------|------|-----------|----------|--------|
| Corpse Skin | 11 | 20.0 | Bat Wing, Grave Dust | Resistance changes: Fire -15, Poison -15, Cold +10, Physical +10. Duration: `(SpiritSpeak - ResistMagic)/2.5 + 40` seconds. |
| Wraith Form | 17 | 20.0 | Nox Crystal, Pig Iron | Body 747/748, Hue 0x4001. Physical +15, Fire -5, Energy -5. Mana leech: `5 + SpiritSpeak*15/100`%. Ignores other mobiles. |
| Lich Form | 23 | 70.0 | Grave Dust, Daemon Blood, Nox Crystal | Body 749. Fire -10, Cold +10, Poison +10. -1 HP per 2.5s tick. Lifetime drain. |
| Horrific Beast | 11 | 40.0 | Bat Wing, Daemon Blood | Body 746. Can coexist with other transformations. Blocks other spell casting. |
| Vampiric Embrace | 23 | 99.0 | Bat Wing, Nox Crystal, Pig Iron | Body 745/744, Hue 0x847E. Fire -25. Cast range 80-120 at 99+ skill. |
| Curse Weapon | 7 | 0.0 | Pig Iron | Weapon's Cursed flag = true. Duration: `SpiritSpeak/3.4 + 1` seconds. Half weapon damage drained to caster. |
| Blood Oath | 13 | 20.0 | Daemon Blood | Dark pact: damage to caster reflected to target. Duration: `(SpiritSpeak - ResistMagic)/80 + 8` seconds. One oath per caster/target. |

### Corpse Skin — Detailed Mechanics

Corpse Skin provides resistance modifications:

```
Fire:         -15
Poison:       -15
Cold:         +10
Physical:     +10
```

- **Duration**: `(SpiritSpeak - ResistMagic) / 2.5 + 40` seconds
- **Self-targeting**: Skips resistance check (always succeeds on self)
- **Refreshable**: Recasting reapplies duration
- **Can be refreshed** even while active

### Wraith Form — Detailed Mechanics

Wraith Form grants mana leech and ghost-like properties:

- **Body**: 747 (female) / 748 (male)
- **Hue**: 0x4001 (male only, female has no hue mod)
- **Resistance changes**: Physical +15, Fire -5, Energy -5
- **Mana leech**: `5 + SpiritSpeak * 15 / 100`%
  - At 100 SpiritSpeak: 20% mana leech
- **IgnoreMobiles**: True (ignores other mobiles in pathfinding)

## Debuffing Spells

Spells that weaken or hinder opponents.

| Spell | Mana | Skill Req | Reagents | Effect |
|-------|------|-----------|----------|--------|
| Mind Rot | 17 | 30.0 | Bat Wing, Pig Iron, Daemon Blood | Increases mana cost of spells cast by target. Players: 1.25x mana scalar. NPCs: 2.0x. Duration: `(SpiritSpeak - ResistMagic)/5.0 + 20`. |
| Strangle | 29 | 65.0 | Daemon Blood, Nox Crystal | Damage over time. Power: `max(4, SpiritSpeak/10)`. Ticks: power ticks over `3*power+5` seconds. Base damage: `(power-2 to power+1) * (3 - stam/StamMax * 2)`. NPC damage: 1.75x. 40% reveal chance per tick. |

### Mind Rot — Detailed Mechanics

Mind Rot affects spellcasting by increasing mana costs:

- **Player targets**: 1.25x mana cost multiplier
- **NPC targets**: 2.00x mana cost multiplier
- **Duration**: `(SpiritSpeak - ResistMagic) / 5.0 + 20` seconds
- **Buff icon**: Mindrot
- **Stacking**: Multiple Mind Rot spells do not stack (only the strongest applies)

### Strangle — Detailed Mechanics

Strangle is a stamina-draining damage spell:

- **Power**: `max(4, SpiritSpeak / 10)`
- **Number of ticks**: Equal to power
- **Timing**: First tick after 5 seconds, then decreasing interval down to 1 second
- **Damage formula**: `(power-2 to power+1) * (3 - stam/StamMax * 2)`
  - At full stam: `(power-2 to power+1) * 1.0`
  - At half stam: `(power-2 to power+1) * 2.0`
  - At 0 stam: `(power-2 to power+1) * 3.0`
- **NPC multiplier**: 1.75x damage
- **Reveal chance**: 40% per tick
- **Total duration**: `3*power+5` or `3*power+3` (depending on `power % 5`)

## Summoning Spells

Spells that call creatures to fight.

| Spell | Mana | Skill Req | Reagents | Effect |
|-------|------|-----------|----------|--------|
| Animate Dead | 23 | 40.0 | Grave Dust, Daemon Blood | Summons undead from corpses. Max 3 at once. Creatures take 1 HP damage every 1.65s. |
| Summon Familiar | 17 | 30.0 | Bat Wing, Grave Dust, Daemon Blood | Summons familiar from Necromancy + SpiritSpeak skill table. One at a time. Duration: 1 day. |
| Vengeful Spirit | 41 | 80.0 | Bat Wing, Grave Dust, Pig Iron | Summons Revenant (3 follower slots). Duration: `SpiritSpeak*80/120 + 10` seconds. Tracks target anywhere. |

### Summon Familiar — Detailed Mechanics

Summon Familiar requires both Necromancy and SpiritSpeak skills:

| Familiar | Necro Req | SpiritSpeak Req |
|----------|-----------|-----------------|
| Horde Minion | 30.0 | 30.0 |
| Shadow Wisp | 50.0 | 50.0 |
| Dark Wolf | 60.0 | 60.0 |
| Death Adder | 80.0 | 80.0 |
| Vampire Bat | 100.0 | 100.0 |

- **Magic Resist**: Equals caster's Magic Resist skill
- **Limitation**: Only one familiar at a time
- **Duration**: 1 day

## Spell List

| Spell | Mantra | Mana | Skill Req | Reagents | Type |
|-------|--------|------|-----------|----------|------|
| Curse Weapon | `An Sanct Gra Char` | 7 | 0.0 | Pig Iron | Buff |
| Pain Spike | `In Sar` | 5 | 20.0 | Grave Dust, Pig Iron | Damage/DoT |
| Corpse Skin | `In Agle Corp Ylem` | 11 | 20.0 | Bat Wing, Grave Dust | Buff |
| Evil Omen | `Pas Tym An Sanct` | 11 | 20.0 | Bat Wing, Nox Crystal | Debuff |
| Wraith Form | `Rel Xen Um` | 17 | 20.0 | Nox Crystal, Pig Iron | Transform |
| Blood Oath | `In Jux Mani Xen` | 13 | 20.0 | Daemon Blood | Buff |
| Mind Rot | `Wis An Ben` | 17 | 30.0 | Bat Wing, Pig Iron, Daemon Blood | Debuff |
| Summon Familiar | `Kal Xen Bal` | 17 | 30.0 | Bat Wing, Grave Dust, Daemon Blood | Summon |
| Animate Dead | `Uus Corp` | 23 | 40.0 | Grave Dust, Daemon Blood | Summon |
| Horrific Beast | `Rel Xen Vas Bal` | 11 | 40.0 | Bat Wing, Daemon Blood | Transform |
| Poison Strike | `In Vas Nox` | 17 | 50.0 | Nox Crystal | Damage |
| Wither | `Kal Vas An Flam` | 23 | 60.0 | Nox Crystal, Grave Dust, Pig Iron | Damage |
| Strangle | `In Bal Nox` | 29 | 65.0 | Daemon Blood, Nox Crystal | Damage/DoT |
| Lich Form | `Rel Xen Corp Ort` | 23 | 70.0 | Grave Dust, Daemon Blood, Nox Crystal | Transform |
| Exorcism | `Ort Corp Grav` | 40 | 80.0 | Nox Crystal, Grave Dust | Utility |
| Vengeful Spirit | `Kal Xen Bal Beh` | 41 | 80.0 | Bat Wing, Grave Dust, Pig Iron | Summon |
| Vampiric Embrace | `Rel Xen An Sanct` | 23 | 99.0 | Bat Wing, Nox Crystal, Pig Iron | Transform |

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index
- [[spells/magery]] — Comparison with magery school
