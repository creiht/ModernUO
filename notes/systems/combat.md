# Combat

Combat is the core gameplay mechanic in Ultimate Online, encompassing melee attacks, ranged attacks, damage calculations, armor and resistance systems, weapon abilities, and poison application. This document covers the AOS-era combat system (the default and most widely used version), with notes on pre-AOS behavior where relevant.

**Source Files:**
- `Projects/Server/Mobiles/Mobile.cs` (9214 lines) — core combat mechanics, damage, resistances, HP/Mana/Stam
- `Projects/UOContent/Items/Weapons/BaseWeapon.cs` (4100 lines) — damage calculation, abilities, slayer, durability
- `Projects/UOContent/Items/Armor/BaseArmor.cs` (1673 lines) — AR, resistances, protection levels
- `Projects/Server/Poison.cs` (102 lines) — poison base class, families, registration
- `Projects/UOContent/Misc/Poison.cs` (162 lines) — PoisonImpl, damage timer, Darkglow/Parasitic effects
- `Projects/UOContent/Misc/AOS.cs` — AosAttributes, AosWeaponAttribute, AosArmorAttribute, AosElementAttribute
- `Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs` — 31 weapon abilities
- `Projects/UOContent/Items/Weapons/SlayerName.cs` — slayer types
- `Projects/UOContent/Items/Weapons/WeaponEnums.cs` — weapon quality, type, durability enums
- `Projects/UOContent/Items/Armor/ArmorEnums.cs` — armor quality, durability, protection enums

---

## Damage Formula

### AOS Damage Calculation

The AOS damage formula scales base weapon damage through multiple modifiers:

```
baseDamage = Random(MinDamage, MaxDamage)
strengthBonus = GetBonus(Strength, 0.300, 100.0, 5.00)
anatomyBonus = GetBonus(Anatomy skill, 0.500, 100.0, 5.00)
tacticsBonus = GetBonus(Tactics skill, 0.625, 100.0, 6.25)
lumberBonus = (Type == Axe) ? GetBonus(Lumberjacking skill, 0.200, 100.0, 10.00) : 0

damageBonus = AosAttribute.WeaponDamage + weapon.GetDamageBonus()
              + (HorrificBeastSpell ? +25 : 0)
              + (DivineFury ? +10 : 0)
              - (Discordance malus)
              - (DefenseMastery malus)
              (capped at 100)

totalBonus = strengthBonus + anatomyBonus + tacticsBonus + lumberBonus
             + (damageBonus + GetDamageBonus()) / 100

finalDamage = baseDamage + baseDamage × totalBonus
```

The `GetBonus` helper formula:

```
bonus = value × scalar
if (value >= threshold) bonus += offset
return bonus / 100
```

### Pre-AOS Damage Calculation

Before AOS, damage used a simpler modifier system:

```
tacticsModifier = (Tactics - 50) / 100          // -50% at 0, 0 at 50, +50% at 100
strengthModifier = Strength / 5 / 100
anatomyModifier = Anatomy / 5 / 100
  + (UOTD+ and Anatomy >= 100) ? 0.10 : 0
lumberModifier = (UOR+ and Axe) ? Lumberjacking / 5 / 100 : 0
  + (UOTD+ and Axe and Lumberjacking >= 100) ? 0.10 : 0
qualityModifier = ((int)Quality - 1) × 0.20
virtualDamageModifier = VirtualDamageBonus / 100

modifiers = tacticsModifier + strengthModifier + anatomyModifier
            + lumberModifier + qualityModifier + virtualDamageModifier

finalDamage = (baseDamage + baseDamage × modifiers) scaled by durability
```

### GetBonus Parameters

| Bonus | Value Source | Scalar | Threshold | Offset |
|-------|-------------|--------|-----------|--------|
| Strength | `Str` stat | 0.300 | 100.0 | 5.00 |
| Anatomy | `Skills.Anatomy.Value` | 0.500 | 100.0 | 5.00 |
| Tactics | `Skills.Tactics.Value` | 0.625 | 100.0 | 6.25 |
| Lumberjacking | `Skills.Lumberjacking.Value` | 0.200 | 100.0 | 10.00 |

### Damage Multiplier Cap

All percentage-based damage bonuses (weapon abilities, slayer bonuses, Enemy of One, Damage Bonus attribute, Talisman killer effects) are capped at a combined **+300%** (x3 total multiplier):

```
percentageBonus = min(abilityBonus + moveBonus + forceOfNatureBonus + damageBonusAttribute + slayerBonus + enemyOfOneBonus + talismanBonus, 300)
finalDamage = AOS.Scale(damage, 100 + percentageBonus)
```

### Passive Skill Gains

Swinging a weapon passively checks:
- **Tactics** — every swing
- **Anatomy** — every swing
- **Lumberjacking** — every swing with Axe weapons (UOR+)

---

## Durability Scaling

Damaged weapons and armor deal/reduce reduced damage based on current HP:

```
durabilityScale = 50 + 50 × (hitPoints / maxHitPoints)
// Range: 50% (broken) to 100% (full)

scaledDamage = AOSScale(baseDamage, durabilityScale)
scaledArmor = armor × durabilityScale / 100
```

---

## Weapon Durability Bonuses

Weapons receive flat bonuses to damage based on durability level and quality.

### Post-UOR Formula (AOS+)

```
bonus = (int)DurabilityLevel × 5 + ((int)Quality - 1) × 10

// UOR+ overrides:
switch (DurabilityLevel):
    Durable       → 20
    Substantial   → 50
    Massive       → 70
    Fortified     → 100
    Indestructible → 120

// AOS+ additions:
bonus += WeaponAttributes.DurabilityBonus
bonus += CraftResource.AttributeInfo.WeaponDurability

// Exceptional quality bonus:
if (Quality == Exceptional) bonus += 20
```

### Quality Levels

| Quality | Value | Damage Bonus |
|---------|-------|-------------|
| Low | 0 | 0 |
| Regular | 1 | 0 |
| Exceptional | 2 | +20 (on top of durability bonus) |

---

## HP / Stamina / Mana

### Maximum Values

| Resource | Formula | Source |
|----------|---------|--------|
| **Hits (HP)** | `50 + Str / 2` | `Mobile.cs:2051` |
| **Stamina** | `Dex` | `Mobile.cs:2096` |
| **Mana** | `Int` | `Mobile.cs:2151` |

### Regen

Each stat has a corresponding regen timer that activates when below maximum:
- `HitsTimer` — regenerates HP
- `StamTimer` — regenerates stamina
- `ManaTimer` — regenerates mana

### Stat Locks

Stat locks control whether stats can increase or decrease through combat/experience:

| Lock Type | Behavior |
|-----------|----------|
| `Up` | Stat can increase, not decrease |
| `Down` | Stat can decrease, not increase |
| `Locked` | Stat cannot change |

Defined in `Mobile.cs:70-75`.

---

## Resistance System

### Resistance Types

Five resistance types defined in `ResistanceType` enum (`Mobile.cs:154-161`):

| Type | Index | Property |
|------|-------|----------|
| Physical | 0 | `PhysicalResistance` |
| Fire | 1 | `FireResistance` |
| Cold | 2 | `ColdResistance` |
| Poison | 3 | `PoisonResistance` |
| Energy | 4 | `EnergyResistance` |

### Resistance Calculation

`ComputeResistances()` (`Mobile.cs:3183-3246`) builds the resistance array:

1. Initialize `Resistances[]` with 5 entries set to `int.MinValue` (uncached)
2. Sum `BasePhysicalResistance` through `BaseEnergyResistance`
3. Add all `ResistanceMod` offsets by type
4. Sum resistance from all equipped items (`Items[i].PhysicalResistance` etc.)
5. Clamp each value to `[GetMinResistance(), GetMaxResistance()]`

#### Player vs NPC Limits

```
maxResistance = m_Player ? MaxPlayerResistance : int.MaxValue
```

`MaxPlayerResistance` is a static property defaulting to **70** (`Mobile.cs:431`).

### Weapon Resist Bonuses

Weapons contribute resistances through `AosWeaponAttribute` flags (`AOS.cs:650-654`):

| Attribute | Resistance Type |
|-----------|----------------|
| `ResistPhysicalBonus` | Physical |
| `ResistFireBonus` | Fire |
| `ResistColdBonus` | Cold |
| `ResistPoisonBonus` | Poison |
| `ResistEnergyBonus` | Energy |

These are read in `BaseWeapon.cs:282-286` and added during `ComputeResistances()`.

---

## Armor Rating

### AR Formula

```
AR = BaseArmorRating + protectionBonus + resourceBonus + qualityBonus
AR = AR × durabilityScale
```

Where:

```
protectionBonus = (ProtectionLevel != Regular) ? 10 + 5 × (int)ProtectionLevel : 0
// UOR+: +10 flat + 5 × level

resourceBonus = (UOR+) CraftResource bonus:
  DullCopper → +2, ShadowIron → +4, Copper → +6, Bronze → +8
  Gold → +10, Agapite → +12, Verite → +14, Valorite → +16
  SpinedLeather → +10, HornedLeather → +13, BarbedLeather → +16

qualityBonus = 8 × ((int)Quality - 1)
// Low: -8, Regular: 0, Exceptional: +8

durabilityScale = 50 + 50 × (hitPoints / maxHitPoints)
```

### Armor Protection Levels

| Level | Value | AR Bonus |
|-------|-------|----------|
| Regular | 0 | 0 |
| Defense | 1 | 15 |
| Guarding | 2 | 20 |
| Hardening | 3 | 25 |
| Fortification | 4 | 30 |
| Invulnerability | 5 | 35 |

### Armor Durability Bonuses

Same structure as weapons, defined in `ArmorDurabilityLevel` (`ArmorEnums.cs:10-18`):

| Level | UOR+ Bonus |
|-------|-----------|
| Regular | 0 |
| Durable | 20 |
| Substantial | 50 |
| Massive | 70 |
| Fortified | 100 |
| Indestructible | 120 |

Exceptional quality adds +20 to the durability bonus.

### Virtual Armor

Players can set `VirtualArmor` and `VirtualArmorMod` properties on `Mobile` (`Mobile.cs:456,918`) to adjust their effective armor rating independently of equipment.

---

## Damage Types

Damage in AOS can be split across five elemental types. The percentage distribution is defined per weapon:

| Type | Description |
|------|-------------|
| **Physical** | Default; remainder after elemental allocation |
| **Fire** | Fire damage |
| **Cold** | Cold damage |
| **Poison** | Poison damage |
| **Energy** | Energy damage |

Additional types (ML+):
| Type | Description |
|------|-------------|
| **Chaos** | Chaos damage |
| **Direct** | Direct damage (bypasses resistances) |

### Damage Type Distribution

`GetDamageTypes()` (`BaseWeapon.cs:2356-2398`) determines the percentage split:

- For `BaseCreature`: uses creature's `PhysicalDamage`, `FireDamage`, etc. properties
- For players: uses weapon's `AosElementDamages` properties, with Physical as the remainder
- Crafted weapons can override via `CraftResource.AttributeInfo` element damage values

### Consecrated Weapon Special Case

Consecrated weapons (`BaseWeapon.Consecrated = true`) redirect all damage to the defender's weakest resistance type (`BaseWeapon.cs:1919-1974`).

---

## Weapon Abilities

31 weapon abilities defined in `WeaponAbility.Abilities` array (`WeaponAbility.cs:15-50`). Each ability has a `DamageScalar`, `AccuracyBonus`, and `BaseMana` cost.

### Abilities by Expansion

| Index | Ability | Expansion | Requires |
|-------|---------|-----------|----------|
| 1 | ArmorIgnore | SE | Tactics ≥ 70/90 |
| 2 | BleedAttack | SE | Tactics ≥ 70/90 |
| 3 | ConcussionBlow | SE | Tactics ≥ 70/90, Axe weapon |
| 4 | CrushingBlow | SE | Tactics ≥ 70/90, 2H Bashing |
| 5 | Disarm | SE | Tactics ≥ 70/90 |
| 6 | Dismount | SE | Tactics ≥ 70/90 |
| 7 | DoubleStrike | SE | Tactics ≥ 70/90 |
| 8 | InfectiousStrike | SE | Tactics ≥ 70/90 |
| 9 | MortalStrike | SE | Tactics ≥ 70/90 |
| 10 | MovingShot | SE | Bow/Crossbow |
| 11 | ParalyzingBlow | SE | Tactics ≥ 70/90, 2H Piercing |
| 12 | ShadowStrike | SE | Tactics ≥ 70/90 |
| 13 | WhirlwindAttack | SE | Tactics ≥ 70/90 |
| 14 | RidingSwipe | SA | Tactics ≥ 70/90, Mounted |
| 15 | FrenziedWhirlwind | SA | Tactics ≥ 70/90 |
| 16 | Block | SA | Tactics ≥ 70/90 |
| 17 | DefenseMastery | SA | Tactics ≥ 70/90 |
| 18 | NerveStrike | SA | Ninjitsu ≥ 70/90 |
| 19 | TalonStrike | SA | Ninjitsu ≥ 70/90 |
| 20 | Feint | SA | Ninjitsu ≥ 70/90 |
| 21 | DualWield | SA | Tactics ≥ 70/90 |
| 22 | DoubleShot | SA | Crossbow |
| 23 | ArmorPierce | SA | Tactics ≥ 70/90 |
| 24 | Bladeweave | SA | Mysticism ≥ 70/90 |
| 25 | ForceArrow | EJ | Tactics ≥ 70/90, Bow |
| 26 | LightningArrow | EJ | Tactics ≥ 70/90, Bow |
| 27 | PsychicAttack | EJ | Tactics ≥ 70/90 |
| 28 | SerpentArrow | EJ | Tactics ≥ 70/90, Bow |
| 29 | ForceOfNature | EJ | Tactics ≥ 70/90, Bow |
| 30 | InfusedThrow | EJ | Tactics ≥ 70/90 |
| 31 | MysticArc | EJ | Tactics ≥ 70/90 |

### Ability Activation Flow

1. `OnBeforeSwing()` — can veto the swing entirely
2. `OnBeforeDamage()` — can veto damage application
3. `OnHit()` — applied on successful hit
4. `OnMiss()` — applied on miss

Each ability is set as the `CurrentAbility` on the attacker via `WeaponAbility.SetCurrentAbility()` and cleared after use.

---

## Slayer System

The slayer system applies a **+100% damage bonus** when attacking creatures that the slayer is effective against.

### Slayer Types

Defined in `SlayerName` enum (`SlayerName.cs:3-33`):

| Slayer | Effective Against |
|--------|------------------|
| None | — |
| Silver | Wraiths, Black Widows, Liches |
| OrcSlaying | Orcs, Ogres |
| TrollSlaughter | Trolls |
| OgreTrashing | Ogres |
| Repond | Dragons |
| DragonSlaying | Dragons |
| Terathan | Terathan creatures |
| SnakesBane | Lizardmen, Serpents |
| LizardmanSlaughter | Lizardmen |
| ReptilianDeath | Reptilons |
| DaemonDismissal | Daemons |
| GargoylesFoe | Gargoyles |
| BalronDamnation | Balrons |
| Exorcism | Undead |
| Ophidian | Ophidians |
| SpidersDeath | Spiders |
| ScorpionsBane | Scorpions |
| ArachnidDoom | Arachnid creatures |
| FlameDousing | Fire Elementals |
| WaterDissipation | Water Elementals |
| Vacuum | Air Elementals |
| ElementalHealth | All Elementals |
| EarthShatter | Earth Elementals |
| BloodDrinking | Humans (player-only slayer) |
| SummerWind | Humans (player-only slayer) |
| ElementalBan | All Elementals |
| Fey | Fey creatures |

### Slayer Checking

`CheckSlayers()` (`BaseWeapon.cs:2288-2330`) checks in order:

1. Talisman slayer (if `ButchersWarCleaver` equipped)
2. Weapon's `Slayer` attribute (via `SlayerGroup`)
3. Weapon's `Slayer2` attribute (via `SlayerGroup`)
4. Attacker's equipped talisman
5. Defender's equipped spellbook/weapon slayer (defensive slayer — reduces damage)

Defensive slayers reduce incoming slayer damage by 50%.

### Visual Effect

Slayer hits produce a white particle effect (`0x37B9`).

---

## Damage Flow

The damage application chain when a mobile is hit:

```
Mobile.Damage(amount, from)
  ├─ CanBeDamaged() check (blessed mobiles cannot be damaged)
  ├─ Region.OnDamage() check (region hooks can/cancel/modify)
  ├─ m_Spell?.OnCasterHurt() (notify active spell)
  ├─ RegisterDamage(amount, from) — tracks damage entries
  ├─ DisruptiveAction() — interrupts casting/mounting
  ├─ Paralyzed = false
  ├─ SendVisibleDamage (based on VisibleDamageType)
  ├─ OnDamage(amount, from, willKill)
  ├─ Mount?.OnRiderDamaged()
  └─ if (newHits < 0): Kill()
     else: Hits = newHits
```

### DamageEntry

Tracks damage sources for murder reporting and kill credit (`Mobile.cs:44-59`):

```csharp
public class DamageEntry {
    public Mobile Damager;
    public int DamageGiven;
    public DateTime LastDamage;
    public bool HasExpired => Core.Now > LastDamage + ExpireDelay;
    public List<DamageEntry> Responsible;  // For damage masters (tamed creatures)
    public static TimeSpan ExpireDelay = TimeSpan.FromMinutes(2.0);
}
```

### VisibleDamageType

Controls who sees damage numbers (`Mobile.cs:146-152`):

| Type | Visibility |
|------|-----------|
| `None` | No one sees damage numbers |
| `Related` | Only attacker and defender see damage |
| `Everyone` | Everyone in range sees damage |
| `Selective` | Selective visibility based on NetState |

---

## Direction and Distance

### Direction Enum

Defined in `Mobile.cs:79-93`:

```
Mask = 0x7      (8 directions: North, Right, East, Down, South, Left, West, Up)
Running = 0x80  (flag: 0 = walking, 1 = running)
ValueMask = 0x87
```

The 8 cardinal/intercardinal directions: North (0x0), Right (0x1), East (0x2), Down (0x3), South (0x4), Left (0x5), West (0x6), Up (0x7).

### Distance in Combat

Distance calculations affect:
- **Melee range** — typically 1 tile
- **Ranged attacks** — weapon's `DefMaxRange` or `AosMaxRange`
- **Darkglow poison bonus** — +10% damage when attacker is >1 tile from victim (`Poison.cs:127-133`)
- **Parasitic poison heal** — attacker heals for damage dealt only when within 1 tile (`Poison.cs:142-148`)

---

## Poison in Combat

### Poison Application

Poisons are applied via weapon hits. The `ApplyPoisonResult` enum (`Mobile.cs:163-169`):

| Result | Meaning |
|--------|---------|
| `Poisoned` | Successfully applied |
| `Immune` | Target is immune to this poison level |
| `HigherPoisonActive` | A stronger poison is already active |
| `Cured` | Existing poison was cured |

### Poison Families

Three families defined in `PoisonFamily` enum (`Poison.cs:7`):

| Family | Levels | Special Effect |
|--------|--------|---------------|
| **Standard** | 5 (Lesser → Lethal) | Base poison damage |
| **Darkglow** | 4 (Lesser → Deadly) | +10% damage when attacker >1 tile away |
| **Parasitic** | 5 (Lesser → Lethal) | Heals attacker for damage dealt when within 1 tile |

### Poison Damage Formula

```
damage = 1 + (int)(Mobile.Hits × _scalar)
damage = clamp(damage, _minimum, _maximum)

_scalar = percent × 0.01  (percent is HP% per tick)
```

### Auto-Cure Interactions

| Effect | Cures | Condition |
|--------|-------|-----------|
| Vampiric Embrace Spell | Poison levels < 4 | AOS+ |
| Orange Petals | Poison levels < 3 | — |
| Unicorn transformation (Animal Form) | All levels | — |

### Poison Timer

Each poison has:
- **Initial delay** — time until first tick
- **Interval** — time between subsequent ticks
- **Count** — number of damage ticks

After `count` ticks, the poison wears off automatically.

---

## AOS Attributes Reference

### AosAttribute (Equipment Attributes)

Defined in `AOS.cs:279-305`:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `RegenHits` | 0x00000001 | Hit point regeneration |
| `RegenStam` | 0x00000002 | Stamina regeneration |
| `RegenMana` | 0x00000004 | Mana regeneration |
| `DefendChance` | 0x00000008 | Chance to defend against attacks |
| `AttackChance` | 0x00000010 | Chance to gain extra attack |
| `BonusStr` | 0x00000020 | Strength bonus |
| `BonusDex` | 0x00000040 | Dexterity bonus |
| `BonusInt` | 0x00000080 | Intelligence bonus |
| `BonusHits` | 0x00000100 | Hit point bonus |
| `BonusStam` | 0x00000200 | Stamina bonus |
| `BonusMana` | 0x00000400 | Mana bonus |
| `WeaponDamage` | 0x00000800 | Weapon damage bonus (%) |
| `WeaponSpeed` | 0x00001000 | Weapon speed bonus |
| `SpellDamage` | 0x00002000 | Spell damage bonus |
| `CastRecovery` | 0x00004000 | Cast recovery time |
| `CastSpeed` | 0x00008000 | Cast speed |
| `LowerManaCost` | 0x00010000 | Mana cost reduction |
| `LowerRegCost` | 0x00020000 | Regeneration cost reduction |
| `ReflectPhysical` | 0x00040000 | Physical damage reflection |
| `EnhancePotions` | 0x00080000 | Potion enhancement |
| `Luck` | 0x00100000 | Luck |
| `SpellChanneling` | 0x00200000 | Spell channeling (no break on damage) |
| `NightSight` | 0x00400000 | Night sight |
| `IncreasedKarmaLoss` | 0x00800000 | Increased karma loss on death |

### AosWeaponAttribute (Weapon-Specific)

Defined in `AOS.cs:631-658`:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `LowerStatReq` | 0x00000001 | Reduces stat requirements |
| `SelfRepair` | 0x00000002 | Self-repair over time |
| `HitLeechHits` | 0x00000004 | Hit point leech on strike |
| `HitLeechStam` | 0x00000008 | Stamina leech on strike |
| `HitLeechMana` | 0x00000010 | Mana leech on strike |
| `HitLowerAttack` | 0x00000020 | Lowers attacker's skill |
| `HitLowerDefend` | 0x00000040 | Lowers defender's skill |
| `HitMagicArrow` | 0x00000080 | Fires magic arrow on hit |
| `HitHarm` | 0x00000100 | Casts Harm on hit |
| `HitFireball` | 0x00000200 | Fires fireball on hit |
| `HitLightning` | 0x00000400 | Strikes with lightning on hit |
| `HitDispel` | 0x00000800 | Dispels magic on hit |
| `HitColdArea` | 0x00001000 | Cold area effect on hit |
| `HitFireArea` | 0x00002000 | Fire area effect on hit |
| `HitPoisonArea` | 0x00004000 | Poison area effect on hit |
| `HitEnergyArea` | 0x00008000 | Energy area effect on hit |
| `HitPhysicalArea` | 0x00010000 | Physical area effect on hit |
| `ResistPhysicalBonus` | 0x00020000 | Physical resistance bonus |
| `ResistFireBonus` | 0x00040000 | Fire resistance bonus |
| `ResistColdBonus` | 0x00080000 | Cold resistance bonus |
| `ResistPoisonBonus` | 0x00100000 | Poison resistance bonus |
| `ResistEnergyBonus` | 0x00200000 | Energy resistance bonus |
| `UseBestSkill` | 0x00400000 | Use best combat skill |
| `MageWeapon` | 0x00800000 | Usable by mages |
| `DurabilityBonus` | 0x01000000 | Additional durability bonus |

### AosArmorAttribute (Armor-Specific)

Defined in `AOS.cs:892-898`:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `LowerStatReq` | 0x00000001 | Reduces stat requirements |
| `SelfRepair` | 0x00000002 | Self-repair over time |
| `MageArmor` | 0x00000004 | Wearable by mages (no stat penalty) |
| `DurabilityBonus` | 0x00000008 | Additional durability bonus |

### AosElementAttribute (Damage Type Allocation)

Defined in `AOS.cs:1268-1277`:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `Physical` | 0x00000001 | Physical damage % |
| `Fire` | 0x00000002 | Fire damage % |
| `Cold` | 0x00000004 | Cold damage % |
| `Poison` | 0x00000008 | Poison damage % |
| `Energy` | 0x00000010 | Energy damage % |
| `Chaos` | 0x00000020 | Chaos damage % |
| `Direct` | 0x00000040 | Direct damage % |

---

## Cross-References

- [`../items/weapons.md`](../items/weapons.md) — weapon damage, slayer system, abilities
- [`../items/armor.md`](../items/armor.md) — AR calculation, resist bonuses
- [`../systems/poisons.md`](../systems/poisons.md) — poison damage mechanics
- [`../getting-started/stats.md`](../getting-started/stats.md) — stat formulas, stat locks
- [`../reference/configuration.md`](../reference/configuration.md) — combat config settings
