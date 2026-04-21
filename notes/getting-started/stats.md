# Stats

The three core stats — Strength, Dexterity, and Intelligence — govern your character's resource pools, combat effectiveness, and spellcasting capacity.

## Core Stats

| Stat | Determines | Affects |
|---|---|---|
| **Strength (Str)** | Hit Points, melee damage | Combat power, resource pool size |
| **Dexterity (Dex)** | Stamina, hit chance | Endurance, attack frequency |
| **Intelligence (Int)** | Mana capacity | Spellcasting power, magic resistance |

## Resource Formulas

### Hit Points (HP)

| Era | Formula |
|---|---|
| **AOS** | `Str / 2 + 50 + AosAttribute.BonusHits` |
| **Pre-AOS** | `RawStr / 2 + 50` |

`BonusHits` from AosAttributes is capped at +25 for player characters in ML+. The `HitsMax` property is defined in `Mobile.cs` (base) and overridden in `PlayerMobile.cs` (AOS).

### Stamina (Stam)

| Era | Formula |
|---|---|
| **AOS** | `Dex + AosAttribute.BonusStam` |
| **Pre-AOS** | `Dex` |

The `StamMax` property is defined in `Mobile.cs` and overridden in `PlayerMobile.cs`.

### Mana

| Era | Formula |
|---|---|
| **AOS** | `Int + AosAttribute.BonusMana + (ML && Elf ? 20 : 0)` |
| **Pre-AOS** | `Int` |

Elves receive a flat +20 Mana bonus in ML+. The `ManaMax` property is defined in `Mobile.cs` and overridden in `PlayerMobile.cs`.

### Stat Caps

| Era | Cap |
|---|---|
| **ML+** (players) | 150 per stat |
| **Pre-ML** | No cap |

The cap is enforced in `PlayerMobile.Str`, `PlayerMobile.Dex`, and `PlayerMobile.Int` getters.

## Regeneration

Regeneration rates are defined in `RegenRates.cs` and triggered by timer-based updates in `Mobile.cs`.

### Base Regeneration Rates

| Resource | Default Rate |
|---|---|
| **Hits** | 11.0 seconds |
| **Stamina** | 7.0 seconds |
| **Mana** | 7.0 seconds |

These are set as `Mobile.DefaultHitsRate`, `Mobile.DefaultStamRate`, and `Mobile.DefaultManaRate`.

### Hit Regeneration (AOS)

```
Delay = 10.0 / (1 + points) seconds
```

Where `points` includes:
- Base: 4
- `AosAttribute.RegenHits` value
- Human racial bonus (ML+): +2
- Paragon/Leviathan bonus: +40
- Horrorific Beast transformation: +20
- Dog/Cat animal form: `Ninjitsu.Value / 3`
- **ML+ player cap**: 18

Formula: `TimeSpan.FromSeconds(10.0 / (1 + points))`

### Stamina Regeneration (AOS)

```
Delay = 1.0 / (0.1 * (2 + points)) seconds
```

Where `points` includes:
- `Focus.Value * 0.1` (skill-based)
- `AosAttribute.RegenStam` value
- Paragon/Leviathan bonus: +40
- Vampiric Embrace transformation: +15
- Kirin animal form: +20
- **ML+ player cap**: 24

Formula: `TimeSpan.FromSeconds(1.0 / (0.1 * (2 + points)))`

### Mana Regeneration (AOS)

Mana regen differs depending on whether you are meditating:

**Not meditating:**
```
medPoints = (Int + Meditation.Value * 3) * (Meditation < 100 ? 0.025 : 0.0275)
focusPoints = Focus.Value * 0.05
totalPoints = focusPoints + medPoints + RegenMana + bonuses
```

**Meditating:**
- Same base calculation, but meditation bonus is capped at 13.0
- Formula: `totalPoints = focusPoints + medPoints + Math.Min(medPoints, 13.0) + RegenMana + bonuses`

Modifiers include:
- Paragon/Leviathan bonus: +40
- Vampiric Embrace transformation: +3
- Lich Form transformation: +13
- **ML+ player cap** (RegenMana): 18
- **ML+**: totalPoints floored to integer
- **Armor penalty**: Wearing meditation-blocking armor removes meditation bonus entirely (AOS)

Formula: `TimeSpan.FromSeconds(1.0 / (0.1 * (2 + totalPoints)))`

### Mana Regeneration (Pre-AOS)

```
medPoints = (Int + Meditation.Value) * 0.5
rate = medPoints <= 0 ? 7.0
     : medPoints <= 100 ? 7.0 - 239 * medPoints / 2400 + 19 * medPoints^2 / 48000
     : medPoints < 120 ? 1.0
     : 0.75
rate += armorPenalty
if (meditating) rate *= 0.5
rate = clamp(rate, 0.5, 7.0)
```

### Armor Penalty (Pre-AOS)

Armor meditation penalty is calculated from all armor slots in `GetArmorOffset()`:

```
Penalty = (Neck + Hands + Head + Arms + Legs + Chest) / 4
```

Each armor's contribution depends on `ArmorMeditationAllowance`:
- `None`: Full AR penalty
- `Half`: Half AR penalty
- `All`: No penalty (Mage Armor / Spell Channeling)

### Regen Through Poison

By default, `Mobile.RegenThroughPoison = GlobalRegenThroughPoison = true`. This can be overridden per-mob. Check `CanRegenHits`, `CanRegenStam`, and `CanRegenMana` properties.

## Stat Locks

Skills can be locked to prevent accidental gain loss. The `SkillLock` enum defines four lock states:

| Lock State | Effect |
|---|---|
| `Unlocked` | Skill can gain or lose points |
| `Below` | Skill can gain but not lose below current value |
| `Above` | Skill can lose but not gain above current value |
| `Locked` | Skill cannot change at all |

Stat locks are managed through the `SkillLock` enum and sent to the client via `Mobile.SendStatLockInfo()`. The paperdoll UI displays the current lock state for each skill.

## Skill Stat Scaling

Each skill has associated stat scale values that determine how much your stats affect its effective value.

### Formula

```
Effective Skill Value = Base + (RawStr * StrScale) + (RawDex * DexScale) + (RawInt * IntScale)
```

Scale values are stored as hundredths in `SkillInfo` (e.g., a value of 50 = 0.50).

### Scale Properties

| Property | Description |
|---|---|
| `StrScale` | Strength contribution to skill value |
| `DexScale` | Dexterity contribution to skill value |
| `IntScale` | Intelligence contribution to skill value |
| `StatTotal` | Sum of all three scale values |

The effective value calculation is in `Skills.cs` line 255:

```csharp
var statsOffset = Owner.Owner.RawStr * Info.StrScale +
                  Owner.Owner.RawDex * Info.DexScale +
                  Owner.Owner.RawInt * Info.IntScale;
```

See [[reference/skill-table]] for the complete stat scale values for all 58 skills.

## Stat Gain Mechanics

Stats can be increased through:
1. **Character creation** — Direct allocation (90 or 80 total points)
2. **Profession templates** — Preset allocations from `prof.txt`
3. **Stat increasing items** — Equipment with `AosAttribute.BonusStr`, `BonusDex`, `BonusInt`
4. **Stat increasing spells** — Spells like Strength, Agility, Cunning, Bless
5. **Stat increasing skills** — Wrestling (Str), Tactics (Dex), EvalInt (Int) gain bonuses

## Racial Modifiers

| Race | Bonus | Source |
|---|---|---|
| **Elf** | +20 Mana (ML+) | `PlayerMobile.ManaMax` |
| **Human** | +2 Hits regen points (ML+) | `RegenRates.Mobile_HitsRegenRate` |
| **Gargoyle** | Gargish armor variants | `Race.Gargoyle.RaceFlag` |
| **Gargoyle** | Can use Throwing, cannot use Archery | `CharacterCreation.ValidateSkills` |
| **All** | Body ID changes | `Race.AliveBody()` / `Race.GhostBody()` |

## See Also

- [[character-creation]] — Stat allocation during character creation
- [[systems/combat]] — How stats affect combat
- [[reference/skill-table]] — Skill stat scaling values
- [[systems/poisons]] — Poison effects on regeneration
