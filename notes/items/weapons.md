# Weapons

Melee and ranged weapons are the primary combat instruments in ModernUO. Every weapon defines damage range, swing speed, stat requirements, combat skill association, and can carry magical properties through the AosAttribute system.

**Source Files:**
- `Projects/UOContent/Items/Weapons/BaseWeapon.cs` (4100 lines) — core mechanics
- `Projects/UOContent/Items/Weapons/BaseMeleeWeapon.cs` — melee-specific
- `Projects/UOContent/Items/Weapons/WeaponEnums.cs` — all enums
- `Projects/UOContent/Items/Weapons/SlayerName.cs` — slayer types
- `Projects/UOContent/Items/Weapons/SlayerGroup.cs` — slayer groupings, oppositions, super-slayers
- `Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs` — 31 abilities
- `Projects/UOContent/Items/Weapons/BaseRanged.cs` — bow/crossbow mechanics
- `Projects/UOContent/Misc/AOS.cs` lines 279-305 (AosAttributes), 631-658 (AosWeaponAttribute), 892-898 (AosArmorAttribute), 1268-1277 (AosElementAttribute)

---

## Overview

Weapons are implemented through the `BaseWeapon` class which implements multiple interfaces:

- `IWeapon` — weapon behavior contract
- `IFactionItem` — faction item tracking
- `ICraftable` — crafting integration
- `ISlayer` — two slayer slots (Slayer1/Slayer2)
- `IDurability` — hit point-based durability system
- `IAosItem` — AosAttributes support
- `IIdentifiable` — identification state

All weapons are placed on `Layer.OneHanded` or `Layer.TwoHanded` based on their item data quality field.

---

## Weapon Types

There are **8 weapon types** that determine combat mechanics and special properties:

| Type | Description | Special Mechanics |
|------|-------------|-------------------|
| `Axe` | Axes, Hatches | Concussion blows on hit (pre-AOS), lumberjacking bonus |
| `Slashing` | Katana, Broadsword, Longsword | Poisonable weapons |
| `Staff` | Staves | Non-lethal combat option |
| `Bashing` | War Hammers, Maces, Mauls | Crushing blows (2H), non-poisonable |
| `Piercing` | Spears, Warforks, Daggers | Paralyzing blows (2H), poisonable |
| `Polearm` | Halberd, Bardiche | Two-handed, extended reach |
| `Ranged` | Bow, Crossbows | Ammo system, quiver support, velocity damage |
| `Fists` | Fists | Default weapon, no durability, no requirements |

---

## Weapon Animations

Weapons trigger specific client animations based on type, handedness, and attack style:

| Animation | Client ID | Used By |
|-----------|-----------|---------|
| `Slash1H` | 9 | One-handed slashing weapons |
| `Pierce1H` | 10 | One-handed piercing weapons |
| `Bash1H` | 11 | One-handed bashing weapons |
| `Bash2H` | 12 | Two-handed bashing weapons |
| `Slash2H` | 13 | Two-handed slashing weapons |
| `Pierce2H` | 14 | Two-handed piercing weapons |
| `ShootBow` | 18 | Bows |
| `ShootXBow` | 19 | Crossbows |
| `Wrestle` | 31 | Fists |
| `Throwing` | 32 | Throwing weapons |

---

## Damage System

### AOS Damage Formula (AOS+)

The AOS damage formula is used when `Core.AOS` is true (current expansions):

```
base_damage = RandomMinMax(MinDamage, MaxDamage)

strength_bonus  = GetBonus(Strength, 0.300, 100.0, 5.00)
anatomy_bonus   = GetBonus(Anatomy.Value, 0.500, 100.0, 5.00)
tactics_bonus   = GetBonus(Tactics.Value, 0.625, 100.0, 6.25)
lumber_bonus    = (Axe type) ? GetBonus(Lumberjacking.Value, 0.200, 100.0, 10.00) : 0

damage_bonus_percent = WeaponDamage attribute + other AoS weapon damage bonuses (capped at 100%)

total_bonus = strength_bonus + anatomy_bonus + tactics_bonus + lumber_bonus + (damage_bonus_percent + GetDamageBonus()) / 100.0

final_damage = base_damage + base_damage * total_bonus
```

The `GetBonus` helper:

```
GetBonus(value, scalar, threshold, offset):
    result = value * scalar
    if value >= threshold: result += offset
    return result / 100
```

### Old Damage Formula (Pre-AOS, T2A)

Used when `Core.AOS` is false:

```
tactics_modifier    = (Tactics.Value - 50.0) / 100.0
strength_modifier   = Str / 5.0 / 100.0
anatomy_modifier    = Anatomy.Value / 5.0 / 100.0 + (Anatomy >= 100 ? 0.1 : 0)  [UOTD+]
lumber_modifier     = (Axe type && UOR) ? Lumberjacking.Value / 5.0 / 100.0 + (>=100 ? 0.1 : 0) : 0
quality_modifier    = (Quality - 1) * 0.2  [Exceptional = +0.2, Low = -0.2]

modifiers = tactics_modifier + strength_modifier + anatomy_modifier + lumber_modifier + quality_modifier + VirtualDamageBonus/100
final_damage = base_damage * (1 + modifiers)
final_damage = ScaleDamageByDurability(final_damage)
```

### Damage Modifiers (AOS, applied as percentage on top)

Modifiers stack additively and are capped at **+300% total bonus damage**:

| Modifier | Effect |
|----------|--------|
| Slayer match | +100% |
| EnemyOfOne (NPC attacker vs specific target) | +100% |
| EnemyOfOne (player attacker vs specific target) | +50% |
| Pack Instinct (2+ companions) | +25% to +100% |
| DoubleStrike | -10% |
| Silver slayer on necromorphs (non-HorrificBeast) | +25% |
| Honor virtue active & adjacent | +25% |
| Weapon ability damage scalar | Varies per ability |
| Special move damage scalar | Varies per move |
| ForceOfNature damage scalar | Varies |
| Talisman.Killer.DamageBonus | Variable |

### Durability Damage Scaling

Damaged weapons deal reduced damage based on remaining hit points:

```
if (hitPoints < maxHitPoints):
    scale = 50 + 50 * hitPoints / maxHitPoints
else:
    scale = 100

final_damage = damage * scale / 100
```

| HP Remaining | Damage Output |
|--------------|---------------|
| 100% (full) | 100% |
| 50% | 75% |
| 0% (broken) | 50% |

---

## Speed / Delay System

### Core.ML (AOS+)

```
bonus = WeaponSpeed + DivineFury(10) + HonorableExecution + DualWield + ReaperForm - Discordance - EssenceOfWind (capped at 60)
stamTicks = Stam / 30
ticks = floor(speed * 4)
ticks = floor((ticks - stamTicks) * 100 / (100 + bonus))
minimum 5 ticks (1.25 seconds)
delay = ticks * 0.25 seconds
```

### Pre-ML (AOS formula)

```
v = (Stam + 100) * speed
bonus = WeaponSpeed + DivineFury(10) - Discordance
v = v + scale(v, bonus)
minimum 1
delay = floor(40000 / v) * 0.5 seconds
minimum 1.25 seconds
```

---

## Accuracy System

Pre-AOS accuracy levels provide a bonus to the accuracy skill:

| Level | Accuracy Bonus |
|-------|---------------|
| `Regular` | 0 |
| `Accurate` | 2 |
| `Surpassingly` | 4 |
| `Eminently` | 6 |
| `Exceedingly` | 8 |
| `Supremely` | 10 |

Bonus is applied as `level * 5` to the accuracy skill via a `DefaultSkillMod`.

---

## Quality System

Weapons have **3 quality levels** that affect durability and crafting:

| Quality | Durability Bonus | Crafting Bonus |
|---------|-----------------|----------------|
| `Low` | -10 | None |
| `Regular` | 0 | None |
| `Exceptional` | +20 | +35 AosAttributes.WeaponDamage |

Quality also affects pre-AOS damage via `quality_modifier = (Quality - 1) * 0.2`.

---

## Durability System

Weapons track hit points and degrade with use. Durability levels add flat bonuses to max hit points:

| Level | Durability Bonus |
|-------|-----------------|
| `Regular` | 0 |
| `Durable` | 20 |
| `Substantial` | 50 |
| `Massive` | 70 |
| `Fortified` | 100 |
| `Indestructible` | 120 |

Durability scaling formula (UOR+):

```
scale = 100 + durability_bonus + WeaponAttributes.DurabilityBonus + CraftResourceInfo.WeaponDurability
if (Quality == Exceptional): scale += 20

hitPoints = hitPoints * scale / 100
maxHitPoints = maxHitPoints * scale / 100
```

---

## Damage Levels (Pre-AOS, T2A)

Pre-AOS damage levels provide flat damage offsets:

| Level | Integer | Offset (pre-T2A) | Offset (T2A: 2x-1) |
|-------|---------|------------------|-------------------|
| `Regular` | 0 | 0 | 0 |
| `Ruin` | 1 | 1 | 1 |
| `Might` | 2 | 3 | 3 |
| `Force` | 3 | 5 | 5 |
| `Power` | 4 | 7 | 7 |
| `Vanq` | 5 | 9 | 9 |

---

## Stat Requirements

Each weapon can require Strength, Dexterity, and/or Intelligence to wield. The `LowerStatReq` attribute reduces requirements as a percentage.

| Requirement | Effect if not met |
|-------------|-------------------|
| Str | Cannot swing / reduced damage |
| Dex | Cannot swing |
| Int | Cannot swing |

---

## Slayer System

The slayer system grants **+100% damage** against specific creature types. Weapons have two slayer slots (`Slayer1` and `Slayer2`).

### SlayerName Enum (28 entries)

| Name | Target Group |
|------|-------------|
| `None` | — |
| `Silver` | Undead (super slayer) |
| `OrcSlaying` | Orcs (minor) |
| `TrollSlaughter` | Trolls (minor) |
| `OgreTrashing` | Ogres (minor) |
| `Repond` | Humanoid (super slayer) |
| `DragonSlaying` | Dragons (minor) |
| `Terathan` | Terathans (minor) |
| `SnakesBane` | Serpents (minor) |
| `LizardmanSlaughter` | Lizardmen (minor) |
| `ReptilianDeath` | Reptilian (super slayer) |
| `DaemonDismissal` | Daemons (minor) |
| `GargoylesFoe` | Gargoyles (minor) |
| `BalronDamnation` | Balrons (minor) |
| `Exorcism` | Abyss (super slayer) |
| `Ophidian` | Ophidians (minor) |
| `SpidersDeath` | Spiders (minor) |
| `ScorpionsBane` | Scorpions (minor) |
| `ArachnidDoom` | Arachnid (super slayer) |
| `FlameDousing` | Fire elementals (minor) |
| `WaterDissipation` | Water elementals (minor) |
| `Vacuum` | Air elementals (minor) |
| `ElementalHealth` | Poison elementals (minor) |
| `EarthShatter` | Earth elementals (minor) |
| `BloodDrinking` | Blood elementals (minor) |
| `SummerWind` | Ice/Snow elementals (minor) |
| `ElementalBan` | Elemental (super slayer) |
| `Fey` | Fey (super slayer) |

### SlayerGroups (6 groups with oppositions)

| Group | Opposition | Super Slayer |
|-------|-----------|-------------|
| `Humanoid` | `Undead` | `Repond` |
| `Undead` | `Humanoid` | `Silver` |
| `Fey` | `Abyss` | `Fey` |
| `Elemental` | `Abyss` | `ElementalBan` |
| `Abyss` | `Elemental, Fey` | `Exorcism` |
| `Arachnid` | `Reptilian` | `ArachnidDoom` |
| `Reptilian` | `Arachnid` | `ReptilianDeath` |

### CheckSlayerResult

| Result | Effect | Visual |
|--------|--------|--------|
| `None` | No bonus | — |
| `Slayer` | +100% damage | 0x37B9 effect |
| `Opposition` | +100% damage | Opposing group's super slayer vs defender's slayer |

The slayer check logic:
1. Check if attacker's slayer matches defender's creature type → `Slayer` result
2. Check if defender has an ISlayer and attacker's slayer is the opposing group's super slayer → `Opposition` result

---

## AosAttributes (24 attributes)

AosAttributes provide statistical bonuses to weapons. Each is a bitmask flag:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `RegenHits` | 0x00000001 | Hit point regeneration |
| `RegenStam` | 0x00000002 | Stamina regeneration |
| `RegenMana` | 0x00000004 | Mana regeneration |
| `DefendChance` | 0x00000008 | Defense chance increase |
| `AttackChance` | 0x00000010 | Hit chance increase |
| `BonusStr` | 0x00000020 | Strength bonus |
| `BonusDex` | 0x00000040 | Dexterity bonus |
| `BonusInt` | 0x00000080 | Intelligence bonus |
| `BonusHits` | 0x00000100 | Hit point increase |
| `BonusStam` | 0x00000200 | Stamina increase |
| `BonusMana` | 0x00000400 | Mana increase |
| `WeaponDamage` | 0x00000800 | Damage increase % |
| `WeaponSpeed` | 0x00001000 | Swing speed increase % |
| `SpellDamage` | 0x00002000 | Spell damage increase % |
| `CastRecovery` | 0x00004000 | Faster cast recovery |
| `CastSpeed` | 0x00008000 | Faster casting |
| `LowerManaCost` | 0x00010000 | Lower mana cost % |
| `LowerRegCost` | 0x00020000 | Lower reagent cost % |
| `ReflectPhysical` | 0x00040000 | Reflect physical damage % |
| `EnhancePotions` | 0x00080000 | Enhance potions % |
| `Luck` | 0x00100000 | Luck |
| `SpellChanneling` | 0x00200000 | Spell channeling |
| `NightSight` | 0x00400000 | Night sight |
| `IncreasedKarmaLoss` | 0x00800000 | Increased karma loss |

---

## AosWeaponAttributes (22 attributes)

Weapon-specific attributes with on-hit effects and property modifiers:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `LowerStatReq` | 0x00000001 | Lower stat requirements % |
| `SelfRepair` | 0x00000002 | Self repair chance |
| `HitLeechHits` | 0x00000004 | Hit life leech % (30% of damage) |
| `HitLeechStam` | 0x00000008 | Hit stamina leech % (100% of damage) |
| `HitLeechMana` | 0x00000010 | Hit mana leech % (40% of damage) |
| `HitLowerAttack` | 0x00000020 | Hit lower attack % |
| `HitLowerDefend` | 0x00000040 | Hit lower defense % |
| `HitMagicArrow` | 0x00000080 | Hit magic arrow % (1d4+10, 1s channel) |
| `HitHarm` | 0x00000100 | Hit harm % (1d5+17, range-based falloff) |
| `HitFireball` | 0x00000200 | Hit fireball % (1d5+19, 1s channel) |
| `HitLightning` | 0x00000400 | Hit lightning % (1d4+23, direct) |
| `HitDispel` | 0x00000800 | Hit dispel % (dispel summoned creatures) |
| `HitColdArea` | 0x00001000 | Hit cold area % (AoE, 10 tile ML / 5 tile pre-ML) |
| `HitFireArea` | 0x00002000 | Hit fire area % |
| `HitPoisonArea` | 0x00004000 | Hit poison area % |
| `HitEnergyArea` | 0x00008000 | Hit energy area % |
| `HitPhysicalArea` | 0x00010000 | Hit physical area % |
| `ResistPhysicalBonus` | 0x00020000 | Physical resistance bonus |
| `ResistFireBonus` | 0x00040000 | Fire resistance bonus |
| `ResistColdBonus` | 0x00080000 | Cold resistance bonus |
| `ResistPoisonBonus` | 0x00100000 | Poison resistance bonus |
| `ResistEnergyBonus` | 0x00200000 | Energy resistance bonus |
| `UseBestSkill` | 0x00400000 | Use best of Swords/Fencing/Macing |
| `MageWeapon` | 0x00800000 | Mage weapon (-30 + value to Magery skill) |
| `DurabilityBonus` | 0x01000000 | Additional durability bonus |

---

## AosElementAttributes (7 elements)

Elemental damage percentages applied to weapon hits. Remaining damage % goes to Physical. Total always sums to 100%:

| Element | Hex Value |
|---------|-----------|
| `Physical` | 0x00000001 |
| `Fire` | 0x00000002 |
| `Cold` | 0x00000004 |
| `Poison` | 0x00000008 |
| `Energy` | 0x00000010 |
| `Chaos` | 0x00000020 |
| `Direct` | 0x00000040 |

---

## AosArmorAttributes (4 attributes)

Shared with armor — applicable to clothing and accessories:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `LowerStatReq` | 0x00000001 | Lower stat requirements % |
| `SelfRepair` | 0x00000002 | Self repair chance |
| `MageArmor` | 0x00000004 | Mage armor (-30 + value to Magery) |
| `DurabilityBonus` | 0x00000008 | Additional durability bonus |

---

## Weapon Abilities (31)

Weapon abilities are activated abilities that cost mana and have a 3-second cooldown. Consecutive abilities within 3s cost double mana.

**Mana formula:**

```
mana = BaseMana
skill_total = sum of 9 combat skills
if skill_total >= 300: mana -= 10
if skill_total >= 200: mana -= 5
lmc = min(LowerManaCost, 40)
mana = mana * (1 - lmc/100)
if on cooldown: mana *= 2
```

All abilities require 70/90 Tactics and have a base cooldown of 3 seconds.

| # | Name | Requires SE | Default Mana | Damage Scalar | Description |
|---|------|-------------|-------------|---------------|-------------|
| 1 | *(null)* | — | — | — | — |
| 2 | `ArmorIgnore` | No | — | — | Ignores part of target's AR |
| 3 | `BleedAttack` | No | — | — | Causes bleeding damage over time |
| 4 | `ConcussionBlow` | No | — | — | Reduces target's Int temporarily |
| 5 | `CrushingBlow` | No | — | 1.75 | Heavy blow with damage multiplier |
| 6 | `Disarm` | No | — | — | Disarms target, removes weapon |
| 7 | `Dismount` | No | — | — | Dismounts rider from mount |
| 8 | `DoubleStrike` | No | — | 0.90 | Two strikes with -10% damage each |
| 9 | `InfectiousStrike` | No | — | — | Applies poison on hit |
| 10 | `MortalStrike` | No | — | — | Heals attacker for damage dealt |
| 11 | `MovingShot` | No | — | — | Allows shooting while moving (ranged) |
| 12 | `ParalyzingBlow` | No | — | — | Paralyzes target temporarily |
| 13 | `ShadowStrike` | No | — | — | Shadow-themed attack |
| 14 | `WhirlwindAttack` | No | — | 1.30 | Area attack hitting multiple targets |
| 15 | `RidingSwipe` | No | — | — | Swipe attack while mounted |
| 16 | `FrenziedWhirlwind` | No | — | — | Extended whirlwind attack |
| 17 | `Block` | No | — | — | Defensive ability, increases defense |
| 18 | `DefenseMastery` | No | — | — | Mastery of defensive techniques |
| 19 | `NerveStrike` | Yes | — | — | Strikes nerve points (Ninjitsu/Bushido 50+) |
| 20 | `TalonStrike` | Yes | — | — | Talon-themed strike (Ninjitsu/Bushido 50+) |
| 21 | `Feint` | Yes | — | — | Feint attack with accuracy bonus (Poisoning 50+) |
| 22 | `DualWield` | Yes | — | — | Dual wielding ability |
| 23 | `DoubleShot` | Yes | — | — | Double shot with ranged weapons |
| 24 | `ArmorPierce` | Yes | — | — | Pierces armor effectively |
| 25 | `Bladeweave` | No | — | — | Bladeweaving technique |
| 26 | `ForceArrow` | No | — | — | Force arrow attack (ranged) |
| 27 | `LightningArrow` | No | — | — | Lightning arrow attack (bypasses ammo) |
| 28 | `PsychicAttack` | No | — | — | Psychic damage attack |
| 29 | `SerpentArrow` | No | — | — | Serpent-themed arrow attack |
| 30 | `ForceOfNature` | No | — | — | Nature-themed attack |
| 31 | `InfusedThrow` | No | — | — | Infused throwing attack |
| 32 | `MysticArc` | No | — | — | Mystic arc attack |

### Ability Activation Flow

1. `OnBeforeSwing(attacker, defender)` — pre-swing validation, returns false to clear ability
2. `Validate(attacker)` — checks SE expansion, transformation, skill requirements
3. `CheckMana(attacker, consume)` — checks and deducts mana
4. Stored in `WeaponAbility.Table[attacker]`
5. `GetCurrentAbility(attacker)` called during hit — re-validates with `CheckMana(m, false)`
6. `OnBeforeDamage(attacker, defender)` — final check before damage application
7. `OnHit(attacker, defender, damage, worldLocation)` — post-hit effect execution

### Ability Base Properties

| Property | Default | Description |
|----------|---------|-------------|
| `BaseMana` | 0 | Base mana cost (varies per ability) |
| `AccuracyBonus` | 0 | Accuracy bonus (e.g., Feint adds accuracy) |
| `DamageScalar` | 1.0 | Damage multiplier |
| `RequiresTactics` | true | Requires Tactics skill |
| `RequiresSecondarySkill` | false | Requires secondary skill (Ninjitsu/Bushido/Poisoning) |

---

## Accuracy Formula (AOS)

Hit chance calculation in AOS:

```
// Attacker side:
atkValue = attacker.Skills[usedSkill].Value
bonus = AttackChance + DivineFury(10) + Wolf/BakeKitsune(20) - HitLowerAttack(-25) + abilityAccuracy + moveAccuracy - 20 (capped at 45)
ourValue = (atkValue + 20) * (100 + bonus)

// Defender side:
defValue = defender.Skills[usedSkill].Value
bonus = DefendChance - ForceArrowDefenseMalus - DivineFury(20) - HitLowerDefense(-25) + BlockBonus - SurpriseAttackMalus - DiscordanceEffect (capped at 45)
theirValue = (defValue + 20) * (100 + bonus)

// Final:
chance = ourValue / (theirValue * 2.0)
minimum 2% (AOS+)
return attacker.CheckSkill(atkSkill.SkillName, chance)
```

`usedSkill` is determined by `GetUsedSkill()`:
- `UseBestSkill` attribute: picks highest of Swords/Fencing/Macing
- `MageWeapon` attribute: picks highest of Magery vs weapon skill
- Otherwise: weapon's default skill (or Wrestling for non-humans with higher Wrestling)

---

## Ranged Weapons

Ranged weapons (`BaseRanged`) extend `BaseMeleeWeapon` and add bow/crossbow mechanics.

### Required Abstract Members

Each ranged weapon must define:

| Member | Type | Description |
|--------|------|-------------|
| `EffectID` | int | Projectile particle effect ID |
| `AmmoType` | Type | Arrow, Bolt, etc. |
| `Ammo` | Item | Default ammo instance |

### Firing Mechanics (`OnFired`)

1. Check for quiver on `Layer.Cloak`
2. If `LowerAmmoCost` roll passes (or no quiver): consume ammo from quiver or backpack via `ConsumeTotal(AmmoType)`
3. If `LowerAmmoCost` but no ammo at all → fail
4. Send projectile particles: `MovingParticles(defender, EffectID, 18, 1, false, false, 0, 0, 0, 0, 0, EffectLayer.RightHand, 0)`

### Swing Requirements (`OnSwing`)

- Must have been standing still for 0.25s (SE) / 0.5s (AOS) / 1s (pre-AOS) before firing
- Exception: `MovingShot` ability ignores this
- Cannot swing if paralyzed, frozen, or casting a movement-blocking spell (AOS)
- `LightningArrow` bypasses ammo requirement

### OnHit Mechanics

- 40% chance to drop ammo on animal/monster kills (if ammo couldn't be added to backpack)
- **Velocity** (ML+): if `_velocity > 0` and distance-based random roll passes, deals `distance * 3` physical damage
- Quiver can alter bow damage via `AlterBowDamage()`

### OnMiss Mechanics

- 40% chance to recover ammo
- SE+: Added to `RecoverableAmmo` dictionary, recovered after 10s timer if not in warmode
- Pre-SE: Ammo item placed at defender's location

### Wood Bonuses (ML+ Crafting)

| Wood | Bonus |
|------|-------|
| `OakWood` | Luck +40, WeaponDamage +5 |
| `AshWood` | WeaponSpeed +10, LowerStatReq +20 |
| `YewWood` | AttackChance +5, WeaponDamage +10 |
| `Bloodwood` | RegenHits +2, HitLeechHits +16 |
| `Heartwood` | Random of: Luck+40 / DurabilityBonus+50 / LowerStatReq+20 / WeaponSpeed+10 / AttackChance+5 / HitLeechHits+10 |
| `Frostwood` | Physical 60%, Cold 40%, WeaponDamage +12 |

### Range

| Weapon | Default Range |
|--------|--------------|
| Bow | 10 tiles |
| Crossbow | 12 tiles |

---

## Fists

Fists are the default weapon, always equipped, and have no physical representation:

```csharp
public partial class Fists : BaseMeleeWeapon
{
    public Fists() : base(0)
    {
        Visible = false;
        Movable = false;
        Quality = WeaponQuality.Regular;
    }

    public static void Initialize()
    {
        Mobile.DefaultWeapon = new Fists();
    }
}
```

### Properties

| Property | Value |
|----------|-------|
| `AosStrengthReq` | 0 |
| `AosMinDamage` | 1 |
| `AosMaxDamage` | 4 |
| `AosSpeed` | 50 |
| `MlSpeed` | 2.50f |
| `OldStrengthReq` | 0 |
| `OldMinDamage` | 1 |
| `OldMaxDamage` | 8 |
| `OldSpeed` | 30 |
| `DefSkill` | Wrestling |
| `DefType` | Fists |
| `DefAnimation` | Wrestle |
| `DefHitSound` | -1 (none) |
| `DefMissSound` | -1 (none) |
| `PrimaryAbility` | Disarm |
| `SecondaryAbility` | ParalyzingBlow |

### Monster Fists Damage

```csharp
if (this is Fists && !c.Body.IsHuman):
    min = max = c.Str / 28
```

### Defend Skill (LBR+)

```
incrValue = min((Anatomy + EvalInt + 20) * 0.5, 120)
return Wrestling > incrValue ? Wrestling : incrValue
```

### Pre-AOS UOR Wrestling Moves

**Stun** (requires Anatomy >= 80, Wrestling >= 80, 15 stamina):
- Chance: `(Wrestling + Anatomy) / 400` (40% at 80/80, 50% at 100/100, 60% at 120/120)
- Freezes defender for 4 seconds

**Disarm** (requires ArmsLore >= 80, Wrestling >= 80, 15 stamina):
- Cannot disarm non-humans/non-players
- Moves defender's weapon to their backpack
- 10-second cooldown

### Parry

Fists cannot parry (`CheckParry` returns false for Fists).

---

## Weapon Inheritance Hierarchy

```
Item
 └── BaseWeapon (4100 lines)
      └── BaseMeleeWeapon (44 lines)
           ├── BaseAxe → Axe, BattleAxe, etc.
           ├── BaseKnife → Dagger, AssassinSpike, etc.
           ├── BaseBashing → Mace, WarHammer, Maul, etc.
           ├── BaseSword → Broadsword, Longsword, Katana, etc.
           ├── BasePolearm → Halberd, Bardiche, etc.
           ├── BaseStaff → Staff, etc.
           ├── BaseSpearsAndForks → Spear, Warfork, etc.
           ├── BaseRanged → Bow, CrossBow
           ├── Fists (special — no physical item)
           └── [Throwing, SE Weapons, ML Weapons, Artifacts]
```

### BaseMeleeWeapon Adds

- `_showUsesRemaining`, `_usesRemaining` fields (default 150 for axes/knives)
- `GetUsesScalar()`: Exceptional = 200, Regular = 100
- Harvesting support (Lumberjacking) via `OnDoubleClick`
- `OnHit`: Pre-AOS UOR concussion blow (Int reduction) when Anatomy >= 80
- Overrides `ScaleDurability()` / `UnscaleDurability()` to also scale UsesRemaining

### Example: Axe (Axes/Axe.cs)

```csharp
[Flippable(0xF49, 0xF4a)]
public partial class Axe : BaseAxe
{
    [Constructible]
    public Axe() : base(0xF49) { }

    public override double DefaultWeight => 4.0;

    public override WeaponAbility PrimaryAbility   => WeaponAbility.CrushingBlow;
    public override WeaponAbility SecondaryAbility => WeaponAbility.Dismount;

    // AOS values
    public override int AosStrengthReq => 35;
    public override int AosMinDamage   => 14;
    public override int AosMaxDamage   => 16;
    public override int AosSpeed       => 37;
    public override float MlSpeed      => 3.00f;

    // Old values (pre-AOS)
    public override int OldStrengthReq => 35;
    public override int OldMinDamage   => 6;
    public override int OldMaxDamage   => 33;
    public override int OldSpeed       => 37;

    public override int InitMinHits => 31;
    public override int InitMaxHits => 110;
}
```

---

## Crafting Integration

Weapons integrate with the crafting system through:

- `ICraftable` interface — `OnCraft` method
- `CraftResource` field — determines hue and bonuses
- `Crafter` field — stores crafter name for maker's mark
- `PlayerConstructed` flag — tracks player-crafted vs generated
- `EngravedText` — engraved text on weapons
- Resource bonuses applied from `CraftResourceInfo` (see craft-resources.md)

### Pre-AOS Runic Tool Bonuses

| Resource | Durability | Damage | Accuracy |
|----------|-----------|--------|----------|
| DullCopper | Durable | — | Accurate |
| ShadowIron | Durable | Ruin | — |
| Copper | Fortified | Ruin | Surpassingly |
| Bronze | Fortified | Might | Surpassingly |
| Gold | Indestructible | Force | Eminently |
| Agapite | Indestructible | Power | Eminently |
| Verite | Indestructible | Power | Exceedingly |
| Valorite | Indestructible | Vanq | Supremely |

---

## Cross-References

- [`systems/combat.md`](systems/combat.md) — melee/ranged combat mechanics, hit chance, damage resolution
- [`reference/craft-resources.md`](reference/craft-resources.md) — full resource bonus tables
- [`reference/skill-table.md`](reference/skill-table.md) — weapon skill associations
- [`systems/crafting.md`](systems/crafting.md) — weapon crafting, quality systems
