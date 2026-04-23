# Resistance Table

Complete reference of damage resistances, resistance attributes, and resistance-related mechanics in ModernUO. This table covers base resistances, AosAttributes, AosElementAttributes, and AosArmorAttributes.

**Source Files:**
- `Projects/Server/Mobiles/Mobile.cs` — Resistance properties, calculation, limits
- `Projects/UOContent/Misc/AOS.cs` — AosAttribute, AosArmorAttribute, AosElementAttribute enums
- `Projects/Server/Items/Item.cs` — Item resistance properties

---

## Base Resistances

All mobile and item entities have five base resistance values:

| Resistance Type | Property | Default | Max (Player) |
|----------------|----------|---------|--------------|
| Physical | `BasePhysicalResistance` | 0 | 70 |
| Fire | `BaseFireResistance` | 0 | 70 |
| Cold | `BaseColdResistance` | 0 | 70 |
| Poison | `BasePoisonResistance` | 0 | 70 |
| Energy | `BaseEnergyResistance` | 0 | 70 |

The maximum player resistance is configurable:

```csharp
public static int MaxPlayerResistance { get; set; } = 70;
```

### Resistance Calculation

Total resistance for a mobile is calculated via `GetResistance(ResistanceType)`:

```
TotalResistance = BaseResistance + ItemBonuses + StatBonuses + SkillBonuses
```

The `Resistances` array on `Mobile` stores the current computed values:

```csharp
public int[] Resistances { get; private set; }
```

---

## AosAttributes (Equipment Bonuses)

AosAttributes are bonus properties that can be applied to equipment items. These modify character statistics and combat capabilities.

| AosAttribute | Value | Description |
|-------------|-------|-------------|
| `RegenHits` | 0x00000001 | Hit point regeneration rate |
| `RegenStam` | 0x00000002 | Stamina regeneration rate |
| `RegenMana` | 0x00000004 | Mana regeneration rate |
| `DefendChance` | 0x00000008 | Chance to defend against attacks |
| `AttackChance` | 0x00000010 | Chance to gain extra attack swings |
| `BonusStr` | 0x00000020 | Bonus Strength |
| `BonusDex` | 0x00000040 | Bonus Dexterity |
| `BonusInt` | 0x00000080 | Bonus Intelligence |
| `BonusHits` | 0x00000100 | Bonus hit points |
| `BonusStam` | 0x00000200 | Bonus stamina |
| `BonusMana` | 0x00000400 | Bonus mana |
| `WeaponDamage` | 0x00000800 | Weapon damage bonus |
| `WeaponSpeed` | 0x00001000 | Weapon speed modifier |
| `SpellDamage` | 0x00002000 | Spell damage bonus |
| `CastRecovery` | 0x00004000 | Cast recovery time reduction |
| `CastSpeed` | 0x00008000 | Casting speed modifier |
| `LowerManaCost` | 0x00010000 | Reduced mana cost for spells |
| `LowerRegCost` | 0x00020000 | Reduced reagent cost |
| `ReflectPhysical` | 0x00040000 | Physical damage reflection chance |
| `EnhancePotions` | 0x00080000 | Enhanced potion effectiveness |
| `Luck` | 0x00100000 | Luck bonus |
| `SpellChanneling` | 0x00200000 | Spell channeling (no movement restriction) |
| `NightSight` | 0x00400000 | Night sight activation |
| `IncreasedKarmaLoss` | 0x00800000 | Increased karma loss on death |

---

## AosArmorAttributes (Armor-Specific)

AosArmorAttributes are special properties that only apply to armor and clothing items.

| AosArmorAttribute | Value | Description |
|------------------|-------|-------------|
| `LowerStatReq` | 0x00000001 | Reduces stat requirements for wearing |
| `SelfRepair` | 0x00000002 | Auto-repairs durability over time |
| `MageArmor` | 0x00000004 | Functions as mage armor (reduces dex penalty for casters) |
| `DurabilityBonus` | 0x00000008 | Increased durability |

---

## AosElementAttributes (Elemental Damage)

AosElementAttributes modify the elemental damage dealt by weapons and attacks.

| AosElementAttribute | Value | Description |
|-------------------|-------|-------------|
| `Physical` | 0x00000001 | Physical damage bonus |
| `Fire` | 0x00000002 | Fire damage bonus |
| `Cold` | 0x00000004 | Cold damage bonus |
| `Poison` | 0x00000008 | Poison damage bonus |
| `Energy` | 0x00000010 | Energy damage bonus |
| `Chaos` | 0x00000020 | Chaos damage bonus |
| `Direct` | 0x00000040 | Direct damage application |

---

## Resistance Bonuses from Equipment

Equipment can provide resistance bonuses through AosAttributes. The total resistance is the sum of:

1. **Base resistance** — Set by race, class, or direct assignment
2. **Item resistance bonuses** — From AosAttributes on equipped items
3. **Skill bonuses** — From MagicResist skill (for players)
4. **Stat bonuses** — Indirect effects through other stats

### Maximum Resistance

| Entity Type | Max Resistance |
|------------|---------------|
| Players | 70 (configurable) |
| NPCs | Varies by creature |
| Items | No inherent limit |

### Resistance Distribution

The sum of all five resistances should generally not exceed 140 for balanced character builds, though there is no hard cap on the total.

---

## Resistance Reduction and Penetration

Some spells and abilities can reduce target resistances:

| Effect | Type | Description |
|--------|------|-------------|
| Curse Weapon | Spell | Reduces target resistances |
| Dispel | Spell | Removes beneficial resistance effects |
| Armor-ignoring attacks | Ability | Bypasses a percentage of AR |

---

## Element-Specific Resistances

AosElementAttributes on weapons affect the elemental damage distribution of attacks:

```
ElementalDamage = WeaponBaseDamage × ElementBonus / 100
```

When a weapon has AosElementAttributes, a portion of its damage is converted to the specified element. This is particularly useful against creatures with low resistance to a specific element.

---

## Cross-References

- [Systems: Combat](../systems/combat.md) — Damage and resistance mechanics
- [Items: Armor](../items/armor.md) — Armor AR and resistance calculations
- [Items: Weapons](../items/weapons.md) — Weapon damage and elemental bonuses
