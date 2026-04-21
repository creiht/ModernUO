# Mysticism

Mysticism is the Gargoyle magic school, tied to elemental forces and the afterlife. It requires the **Samurai/Ages of Ascension** expansion (Core.SA) and uses a mana table identical to Magery.

Mysticism uses the **Mysticism** skill for casting, but damage is calculated using **the higher of Imbuing or Focus** skills — a unique dual-skill system not found in any other school.

## Key Mechanics

### Dual Skill System

Mysticism damage uses whichever skill is higher between **Imbuing** and **Focus**:

```
damageSkill = max(Imbuing, Focus)
damageFixed = max(Imbuing.Fixed, Focus.Fixed)
```

This means players can invest in either skill (or both) to maximize mystic spell damage.

### Mana Table

Mysticism uses the same mana table as Magery:

| Circle | Mana |
|--------|------|
| Third  | 9    |
| Fourth | 11   |
| Sixth  | 20   |
| Seventh | 40  |
| Eighth | 50   |

### Cast Time

Cast time scales differently from Magery: `0.5 + Circle x 0.25` seconds (faster than Magery at each circle).

| Circle | Cast Time |
|--------|-----------|
| Third  | 1.25s     |
| Fourth | 1.50s     |
| Sixth  | 2.00s     |
| Seventh | 2.25s    |
| Eighth | 2.50s     |

### Skill Requirements

Each spell has a required Mysticism skill based on its circle:

| Circle | Min Skill | Max Skill |
|--------|-----------|-----------|
| Third  | 20.5     | 45.5      |
| Fourth | 32.5     | 57.5      |
| Sixth  | 45.5     | 70.5      |
| Seventh | 70.5    | 95.5      |
| Eighth | 70.5     | 95.5      |

Cast success range: `RequiredSkill - 12.5` to `RequiredSkill + 37.5`.

### Resist System

Mysticism uses the standard magic resistance system:

```
maxSkill = (1 + Circle) * 10 + (1 + Circle / 6) * 25
resistPercent = min(MagicResist/5, MagicResist - (Mysticism - 20)/5 - (1 + Circle)*5) / 2
```

## Third Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Eagle Strike | `Kal Por Xen` | Bloodmoss, Bone, Spider's Silk, Mandrake Root | Conjures magical eagle projectile. Delayed damage (1.0s): 19 + AOS scaling, energy damage. Reflectable (circle 3). |

### Eagle Strike — Detailed Mechanics

Eagle Strike fires a magical projectile:

- **Projectile**: Visual of a magical eagle
- **Damage**: 19 + AOS scaling
- **Damage type**: Energy
- **Delay**: 1.0 second cast before damage
- **Reflectable**: Yes (circle 3)

## Fourth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Animated Weapon | `In Jux Por Ylem` | Bone, Black Pearl, Mandrake Root, Nightshade | Summons AnimatedWeapon. 4 follower slots. Duration: `10 + level` seconds. Level = `(Mysticism + max(Imbuing, Focus)) / 2`. |
| Stone Form | `In Rel Ylem` | Bloodmoss, Fertile Dirt, Garlic | Self-toggle. Body mod: 0x2C1 (stone appearance). All resistances: `+(Mysticism+max(Imbuing,Focus))/24`. Damage bonus: `+(Mysticism+max(Imbuing,Focus))/12`. Resist cap: `+(Mysticism+max(Imbuing,Focus))/48`. Toggle: cast again to remove. |

### Stone Form — Detailed Mechanics

Stone Form is a self-buff transformation:

- **Body modification**: 0x2C1 (stone-like appearance)
- **Resistance bonuses** (all five types):
  ```
  bonus = (Mysticism + max(Imbuing, Focus)) / 24
  ```
- **Damage bonus**:
  ```
  bonus = (Mysticism + max(Imbuing, Focus)) / 12
  ```
- **Resist cap**:
  ```
  cap = (Mysticism + max(Imbuing, Focus)) / 48
  ```
- **Restrictions**: Cannot use while polymorphed, Animal Form, flying, or with sigil
- **Toggle**: Casting again removes the effect

### Animated Weapon — Detailed Mechanics

Animated Weapon summons a weapon creature:

- **Follower slots**: 4
- **Level calculation**: `(Mysticism + max(Imbuing, Focus)) / 2`
- **Duration**: `10 + level` seconds
- **Summon type**: AnimatedWeapon

## Sixth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Bombard | `Corp Por Ylem` | Bloodmoss, Garlic, Sulfurous Ash, Dragon's Blood | Delayed damage spell. Initial: 40 + AOS scaling. After 1.2s: paralyze up to `(Imbuing/Focus - Resist)/10` seconds. Knockback chance based on skill comparison. Reflectable (circle 6). |
| Cleansing Winds | `In Vas Mani Hur` | Garlic, Ginseng, Mandrake Root, Dragon's Blood | Cures self + up to 3 party members in 2-tile range. Cure chance: `100 + ((Mysticism + SpiritSpeak)/2 * 75)`. Removes all curses. Healing shared and reduced by curses. |

### Bombard — Detailed Mechanics

Bombard is a delayed area attack:

- **Initial damage**: 40 + AOS scaling
- **Delay**: 1.2 seconds before secondary effect
- **Paralyze**: Up to `(Imbuing/Focus - MagicResist) / 10` seconds
- **Knockback**: Chance based on skill comparison
- **Reflectable**: Yes (circle 6)

### Cleansing Winds — Detailed Mechanics

Cleansing Winds is a group healing/cure spell:

- **Targets**: Self + up to 3 party members within 2 tiles
- **Cure chance**: `100 + ((Mysticism + SpiritSpeak) / 2 * 75)`
- **Healing**: Shared among all targets
- **Curse reduction**: Each curse reduces healing:
  | Curse | Reduction |
  |-------|-----------|
  | Evil Omen | 1 |
  | Strangle | 2 |
  | Corpse Skin | 3 |
  | Blood Oath | 3 |
  | Mind Rot | 2 |
  | Curse | 4 |
  | Spell Plague | 4 |
  | Mortal Strike | 2 |

## Seventh Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Hail Storm | `Kal Des Ylem` | Dragon's Blood, Bloodmoss, Black Pearl, Mandrake Root | Area damage (2-tile radius). Cold damage: 51 + AOS scaling (PvP vs PvM). Requires LOS. |
| Spell Plague | `Vas Rel Jux Ort` | Daemon Bone, Dragon's Blood, Nightshade, Sulfurous Ash | Initial chaos damage: 33 + AOS scaling. Applies curse: 90% chance of secondary chaos explosion per damage event. Max 3 explosions OR 8 seconds. Secondary: `15+explosions*3 + AOS scaling`. |

### Hail Storm — Detailed Mechanics

Hail Storm is a cold damage area spell:

- **Area**: 2-tile radius
- **Damage**: 51 + AOS scaling
- **Damage type**: Cold
- **Requirements**: Line of sight required
- **Visual**: Custom hailstone particle effects

### Spell Plague — Detailed Mechanics

Spell Plague creates cascading chaos explosions:

- **Initial damage**: 33 + AOS scaling (chaos damage)
- **Curse application**: On each damage event, 90% chance of secondary explosion
- **Explosion chain**: Each successive explosion has 30% less chance (90%, 60%, 30%)
- **Magic Resist reduction**: -3% per 100 MR above 700
- **Maximum**: 3 explosions OR 8 seconds, whichever comes first
- **Stacking**: Multiple plagues can apply sequentially
- **Secondary damage**: `15 + explosions * 3 + AOS scaling`

## Eighth Circle

| Spell | Mantra | Reagents | Effect |
|-------|--------|----------|--------|
| Nether Cyclone | `Grav Hur` | Mandrake Root, Nightshade, Sulfurous Ash, Bloodmoss | Area damage (2-tile radius). Chaos damage: 51 + AOS scaling. Drains stamina AND mana based on skill comparison. Reduction: `(Mysticism+Imbuing/Focus)/1200 - MagicResist/800`. |

### Nether Cyclone — Detailed Mechanics

Nether Cyclone is the most powerful Mysticism spell:

- **Area**: 2-tile radius
- **Damage**: 51 + AOS scaling (chaos damage)
- **Mana drain**: `(Mysticism + max(Imbuing, Focus)) / 1200 - MagicResist / 800`
- **Stamina drain**: Same formula as mana
- **Visual**: Custom cyclone particle effects

## Spell List

| Spell | Mantra | Circle | Mana | Skill Req | Reagents | Type |
|-------|--------|--------|------|-----------|----------|------|
| Eagle Strike | `Kal Por Xen` | 3rd | 11 | 33.0 | Bloodmoss, Bone, Spider's Silk, Mandrake Root | Damage |
| Animated Weapon | `In Jux Por Ylem` | 4th | 11 | 45.0 | Bone, Black Pearl, Mandrake Root, Nightshade | Summon |
| Stone Form | `In Rel Ylem` | 4th | 11 | 45.0 | Bloodmoss, Fertile Dirt, Garlic | Buff |
| Bombard | `Corp Por Ylem` | 6th | 20 | 58.0 | Bloodmoss, Garlic, Sulfurous Ash, Dragon's Blood | Damage |
| Cleansing Winds | `In Vas Mani Hur` | 6th | 20 | 58.0 | Garlic, Ginseng, Mandrake Root, Dragon's Blood | Healing |
| Hail Storm | `Kal Des Ylem` | 7th | 40 | 83.0 | Dragon's Blood, Bloodmoss, Black Pearl, Mandrake Root | Damage |
| Spell Plague | `Vas Rel Jux Ort` | 7th | 40 | 83.0 | Daemon Bone, Dragon's Blood, Nightshade, Sulfurous Ash | Damage |
| Nether Cyclone | `Grav Hur` | 8th | 50 | 83.0 | Mandrake Root, Nightshade, Sulfurous Ash, Bloodmoss | Damage |

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index
- [[spells/magery]] — Comparison with magery school
