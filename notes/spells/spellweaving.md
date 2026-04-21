# Spellweaving

Spellweaving is the ancient magic school of the Isle of Silk, featuring unique spell mechanics centered around the **Arcane Focus** system. It requires the **Mondain's Legacy** expansion (Core.ML) and completing the epic arcanist quest.

All Spellweaving spells consume **mana** (not tithing points) and use the **Spellweaving** skill for both casting and damage. The Arcane Focus system provides bonus levels that enhance spell effects.

## Key Mechanics

### Arcane Focus System

The **Arcane Focus** is a special item created during **Arcane Circle** that provides a **FocusLevel** bonus to all Spellweaving spells:

- **FocusLevel**: Ranges from 0 to 5 (6 at the sanctuary location)
- **Source**: Stand on valid arcane circle tiles with another Spellweaver
- **Effect**: All spells gain bonuses proportional to FocusLevel

| Spell | FocusLevel Effect |
|-------|-------------------|
| Attune Weapon | +6 absorb per FocusLevel |
| Essence of Wind | +1 damage, +1 FC malus, +1 SSI malus per FL |
| Ethereal Voyage | +2 duration per FL |
| Gift of Life | +2 duration per FL, +1/100 hits scalar per FL |
| Gift of Renewal | +10 duration per FL |
| Immolating Weapon | +1 damage, +1 duration per FL |
| Nature's Fury | +2 duration per FL |
| Reaper Form | +5 resist, +10 swing speed, +10 spell damage per FL |
| Summon Fey/Fiend | +1 follower per FL, +2 duration minutes per FL |
| Thunderstorm | +1 radius, +1 damage, +5 duration per FL |

### Quest Requirement

Unlike all other spell schools, Spellweaving requires completing the **epic arcanist quest** to use:

```csharp
if (caster is PlayerMobile mobile)
{
    var context = MLQuestSystem.GetContext(mobile);
    if (context?.Spellweaving != true)
    {
        // "You must have completed the epic arcanist quest to use this ability."
        return false;
    }
}
```

This restriction applies to PlayerMobiles. NPCs and creatures are exempt.

### Cast Effects

All Spellweaving spells display a unique casting effect:
```
FixedEffect(0x37C4, 10, (int)(GetCastDelay().TotalSeconds * 28), 4, 3)
```

### Spell Types

Spellweaving spells fall into several categories:

| Type | Count | Description |
|------|-------|-------------|
| Buffs | 3 | Arcane Circle, Attune Weapon, Gift of Renewal |
| Transforms | 2 | Ethereal Voyage, Reaper Form |
| Damage | 4 | Essence of Wind, Thunderstorm, Word of Death, Immolating Weapon |
| Summoning | 3 | Summon Fey, Summon Fiend, Nature's Fury |
| Utility | 1 | Gift of Life |

## Buff Spells

| Spell | Mana | Skill Req | Effect |
|-------|------|-----------|--------|
| Arcane Circle | 24 | 0.0 | Creates Arcane Focus items. Requires standing on circle tiles + another Spellweaver within 1 tile. Duration: max(1, Spellweaving/24) hours. |
| Attune Weapon | 24 | 0.0 | Melee damage absorb: `18 + (Spellweaving-10)/10*3 + FocusLevel*6`. Duration: `60 + FocusLevel*12` seconds. 120s cooldown after. |
| Gift of Renewal | 24 | 0.0 | Cures poison if present. Otherwise heals 5+Spellweaving/24+FocusLevel every 2 seconds. Duration: `30 + FocusLevel*10` seconds. 60s cooldown. |

### Arcane Circle — Detailed Mechanics

Arcane Circle is the foundation of all Spellweaving power:

- **Requirements**:
  - Stand on valid arcane circle tile (0xFEA, 0x1216, 0x307F, 0x1D10, 0x1D0F, 0x1D1F, 0x1D12)
  - At least one other Spellweaver within 1 tile
  - Both must have Spellweaving skill within 20 points of each other
- **FocusLevel bonus**: Up to 5 arcanists (6 at sanctuary location 6267,131)
- **Duration**: `max(1, Spellweaving / 24)` hours
- **Focus item**: ArcaneFocus with StrengthBonus = FocusLevel

### Attune Weapon — Detailed Mechanics

Attune Weapon enchants the caster's melee weapon:

- **Melee damage absorb**: `18 + (Spellweaving - 10) / 10 * 3 + FocusLevel * 6`
  - At 100 Spellweaving, 0 FocusLevel: `18 + 27 = 45` absorb
  - At 100 Spellweaving, 5 FocusLevel: `18 + 27 + 30 = 75` absorb
- **Duration**: `60 + FocusLevel * 12` seconds
  - At 5 FocusLevel: 120 seconds
- **Cooldown**: 120 seconds after effect ends
- **Requires**: Melee weapon (not Fists)

### Gift of Renewal — Detailed Mechanics

Gift of Renewal provides continuous healing:

- **If target is poisoned**: Cures poison instead
- **Healing per round**: `5 + Spellweaving / 24 + FocusLevel`
- **Interval**: Every 2 seconds
- **Duration**: `30 + FocusLevel * 10` seconds
  - At 5 FocusLevel: 80 seconds = 40 healing rounds
  - At 100 Spellweaving, 5 FocusLevel: `5 + 4 + 5 = 14` HP per round = 560 total
- **Cooldown**: 60 seconds after effect ends

## Transformation Spells

| Spell | Mana | Skill Req | Effect |
|-------|------|-----------|--------|
| Ethereal Voyage | 32 | 24.0 | Body 0x302, Hue 0x48F. Duration: `12 + Spellweaving/24 + FocusLevel*2` seconds. 5-minute cooldown. Cannot use in combat. |
| Reaper Form | 34 | 24.0 | Body 0x11D. Resist: Physical/Cold/Poison/Energy `5+FL`, Fire `-25`. Swing speed `10+FL`. Spell damage `10+FL`. Walk speed. |

### Ethereal Voyage — Detailed Mechanics

Ethereal Voyage transforms the caster into an ethereal being:

- **Body**: 0x302
- **Hue**: 0x48F
- **Duration**: `12 + Spellweaving / 24 + FocusLevel * 2` seconds
  - At 100 Spellweaving, 0 FL: `12 + 4 = 16` seconds
  - At 100 Spellweaving, 5 FL: `12 + 4 + 10 = 26` seconds
- **Cooldown**: 5 minutes after effect ends
- **Restriction**: Cannot use while in combat
- **Type**: Transformation (removed on aggressive action)

### Reaper Form — Detailed Mechanics

Reaper Form is a powerful combat transformation:

- **Body**: 0x11D (gargoyle reaper appearance)
- **Resistance changes**:
  - Physical: `+5 + FocusLevel`
  - Cold: `+5 + FocusLevel`
  - Poison: `+5 + FocusLevel`
  - Energy: `+5 + FocusLevel`
  - Fire: `-25`
- **Swing speed bonus**: `10 + FocusLevel`
- **Spell damage bonus**: `10 + FocusLevel`
- **Speed control**: Set to Walk (cannot run)
- **Type**: Transformation

## Damage Spells

| Spell | Mana | Skill Req | Effect |
|-------|------|-----------|--------|
| Essence of Wind | 40 | 52.0 | Energy damage: `25 + FocusLevel`. Area: `5+FocusLevel` radius. Applies FC malus `FL+1` and SSI malus `2*(FL+1)`. Duration: `Spellweaving/24+FL` seconds. |
| Thunderstorm | 32 | 10.0 | Energy damage: `10+Spellweaving/24+FocusLevel`. Area: `2+FocusLevel` radius. Cast recovery malus: +6. Duration: `5+FocusLevel` seconds. Only applies to targets who just finished casting. |
| Immolating Weapon | 32 | 10.0 | Fire damage on hit: `5+Spellweaving/24+FocusLevel`. Duration: `10+Spellweaving/24+FocusLevel` seconds. Each hit consumes one charge. Melee weapon only. |
| Word of Death | 50 | 80.0 | Energy damage. Low HP execution: If target HP < 5% * FocusLevel, deals 300 damage. Normal: Spellweaving/5 to Spellweaving/3. |

### Essence of Wind — Detailed Mechanics

Essence of Wind is an area debuff spell:

- **Energy damage**: `25 + FocusLevel` (applied to all targets)
- **Area**: `5 + FocusLevel` radius
  - At 5 FocusLevel: 10-tile radius
- **Debuff on hit targets**:
  - **FC malus**: `FocusLevel + 1` (slower cast recovery)
  - **SSI malus**: `2 * (FocusLevel + 1)` (slower swing speed)
- **Duration**: `Spellweaving / 24 + FocusLevel` seconds
  - At 100 SW, 5 FL: `4 + 5 = 9` seconds

### Thunderstorm — Detailed Mechanics

Thunderstorm is an area damage spell targeting recently-casting enemies:

- **Energy damage**: `10 + Spellweaving / 24 + FocusLevel`
  - At 100 SW, 0 FL: `10 + 4 = 14`
  - At 100 SW, 5 FL: `10 + 4 + 5 = 19`
- **SDI**: Capped at 15% for PvP
- **Area**: `2 + FocusLevel` radius
- **Cast recovery malus**: +6 to affected targets (adds 6 seconds to their next cast recovery)
- **Duration**: `5 + FocusLevel` seconds
- **Trigger**: Only applies to targets who just finished casting a spell
- **Area**: `2 + FocusLevel` radius

### Word of Death — Detailed Mechanics

Word of Death features an execution mechanic:

- **Normal damage**: `Spellweaving / 5` to `Spellweaving / 3` with SDI bonus
  - At 100 Spellweaving: 20 to 33 base
- **Execution threshold**: If target HP < `5% * FocusLevel`, deals **300 damage**
  - At 3 FocusLevel: executes targets below 15 HP
  - At 5 FocusLevel: executes targets below 25 HP
- **SDI bonus**: Capped at 15% for PvP

### Immolating Weapon — Detailed Mechanics

Immolating Weapon enchants a melee weapon with fire damage:

- **Fire damage per hit**: `5 + Spellweaving / 24 + FocusLevel`
  - At 100 SW, 0 FL: `5 + 4 = 9`
  - At 100 SW, 5 FL: `5 + 4 + 5 = 14`
- **Duration**: `10 + Spellweaving / 24 + FocusLevel` seconds
- **Charges**: Each hit consumes one charge
- **Requirements**: Melee weapon only (not Fists or ranged)

## Summoning Spells

| Spell | Mana | Skill Req | Effect |
|-------|------|-----------|--------|
| Summon Fey | 10 | 38.0 | Summons ArcaneFey. Followers: `1 + FocusLevel`. Duration: `Spellweaving/24 + FocusLevel*2` minutes. Requires SummonFey quest context. |
| Summon Fiend | 10 | 38.0 | Summons ArcaneFiend. Followers: `1 + FocusLevel`. Duration: Same as Summon Fey. Requires SummonFiend quest context. |
| Nature's Fury | 24 | 0.0 | Summons NatureFury. 1 follower slot. Duration: `Spellweaving/24 + 25 + FocusLevel*2` seconds. NatureFury damage increases by 1 every 5 seconds (max 20). House protection. |

### Summon Fey / Summon Fiend — Detailed Mechanics

Both spells follow the same pattern with different creature types:

- **Follower slots**: `1 + FocusLevel`
  - At 3 FocusLevel: 4 followers
  - At 5 FocusLevel: 6 followers
- **Duration**: `Spellweaving / 24 + FocusLevel * 2` minutes
  - At 100 SW, 5 FL: `4 + 10 = 14` minutes
- **Quest requirements**:
  - **Summon Fey**: Must have completed SummonFey quest context
  - **Summon Fiend**: Must have completed SummonFiend quest context

### Nature's Fury — Detailed Mechanics

Nature's Fury summons a persistent creature:

- **Follower slots**: 1
- **Duration**: `Spellweaving / 24 + 25 + FocusLevel * 2` seconds
  - At 100 SW, 0 FL: `4 + 25 = 29` seconds
  - At 100 SW, 5 FL: `4 + 25 + 10 = 39` seconds
- **Scaling damage**: Increases by 1 every 5 seconds (maximum 20)
- **Protection**: Protected in house regions

## Utility Spells

| Spell | Mana | Skill Req | Effect |
|-------|------|-----------|--------|
| Gift of Life | 70 | 38.0 | Prevents death: on death, triggers resurrection gump. Hits scalar: `(Spellweaving/2.4 + FocusLevel)/100`. Duration: `(Spellweaving/24)*2 + FocusLevel` minutes. Retains through death. |

### Gift of Life — Detailed Mechanics

Gift of Life is a powerful death-prevention spell:

- **Effect**: When target would die, triggers a resurrection gump instead
- **Hits scalar**: `(Spellweaving / 2.4 + FocusLevel) / 100`
  - At 100 SW, 0 FL: `41.7 / 100 = 0.417`
  - At 100 SW, 5 FL: `41.7 + 5 = 46.7 / 100 = 0.467`
- **Duration**: `(Spellweaving / 24) * 2 + FocusLevel * 2` minutes
  - At 100 SW, 0 FL: `8.33` minutes
  - At 100 SW, 5 FL: `8.33 + 10 = 18.33` minutes
- **Target restriction**: Self or bonded pet only
- **Pet death**: Shows PetResurrectGump to master and friends

## Spell List

| Spell | Mantra | Mana | Skill Req | Type |
|-------|--------|------|-----------|------|
| Arcane Circle | `Myrshalee` | 24 | 0.0 | Buff |
| Attune Weapon | `Haeldril` | 24 | 0.0 | Buff |
| Essence of Wind | `Anathrae` | 40 | 52.0 | Damage |
| Ethereal Voyage | `Orlavdra` | 32 | 24.0 | Transform |
| Gift of Life | `Illorae` | 70 | 38.0 | Utility |
| Gift of Renewal | `Olorisstra` | 24 | 0.0 | Buff |
| Immolating Weapon | `Thalshara` | 32 | 10.0 | Damage |
| Nature's Fury | `Rauvvrae` | 24 | 0.0 | Summon |
| Reaper Form | `Tarisstree` | 34 | 24.0 | Transform |
| Summon Fey | `Alalithra` | 10 | 38.0 | Summon |
| Summon Fiend | `Nylisstra` | 10 | 38.0 | Summon |
| Thunderstorm | `Erelonia` | 32 | 10.0 | Damage |
| Word of Death | `Nyraxle` | 50 | 80.0 | Damage |

## Arcane Focus Item

The **ArcaneFocus** item is created during Arcane Circle and provides the FocusLevel bonus:

- **StrengthBonus**: Equal to FocusLevel (0-5, or 6 at sanctuary)
- **Location**: Held or in backpack
- **Detection**: `FindArcaneFocus(from)` checks Holding and Backpack

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index
