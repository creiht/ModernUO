# Armor

Armor provides defensive protection in ModernUO through Armor Rating (AR), resistance bonuses, and stat modifiers. Every armor piece is implemented through the `BaseArmor` class which integrates crafting, durability, AosAttributes, and identification systems.

**Source Files:**
- `Projects/UOContent/Items/Armor/BaseArmor.cs` (1673 lines) — core mechanics
- `Projects/UOContent/Items/Armor/ArmorEnums.cs` (64 lines) — all enums
- `Projects/UOContent/Items/Armor/` directory — representative armor pieces by material type
- `Projects/UOContent/Items/Shields/BaseShield.cs` — shield-specific mechanics
- `Projects/UOContent/Misc/ResourceInfo.cs` — craft resource definitions
- `notes/reference/craft-resources.md` — pre-generated resource bonus tables

---

## Overview

Armor is implemented through the `BaseArmor` class which implements multiple interfaces:

- `IAosItem` — AosAttributes and AosArmorAttributes support
- `ICraftable` — crafting integration via `OnCraft` method
- `IWearableDurability` — hit point-based durability system
- `IScissorable` — cutting armor into cloth
- `IFactionItem` — faction item tracking
- `IIdentifiable` — identification state for unidentified armor

All armor pieces are placed on a `Layer` determined by `ItemData.Quality` field at construction.

---

## Armor Body Types

There are **7 armor body types** that determine layer assignment, armor scalar, and visual representation:

| Body Type | Layer | Armor Scalar | Example |
|-----------|-------|-------------|---------|
| `Gorget` | `Neck` (0x0E) | 0.07 | Gorget |
| `Gloves` | `Gloves` (0x10) | 0.07 | Gloves |
| `Helmet` | `Helm` (0x01) | 0.14 | Helmet, Jingasa |
| `Arms` | `Arms` (0x13) | 0.15 | Arms |
| `Legs` | `InnerLegs` (0x15), `OuterLegs` (0x16), `Pants` (0x17) | 0.22 | Legs, Haidate, Suneate |
| `Chest` | `InnerTorso` (0x12), `OuterTorso` (0x14), `Shirt` (0x11) | 0.35 | Chest, Do, Kilt |
| `Shield` | `TwoHanded` (0x02) | 0.07 | Shield |

Armor scalars determine what fraction of the armor's base AR is actually applied:

```csharp
ArmorScalars = { 0.07, 0.07, 0.14, 0.15, 0.22, 0.35 }
// Index:         Gorget, Gloves, Helmet, Arms, Legs, Chest
```

Layer-to-body-type mapping (from `BaseArmor.BodyPosition`):

```csharp
Layer.Neck       → ArmorBodyType.Gorget
Layer.TwoHanded  → ArmorBodyType.Shield
Layer.Gloves     → ArmorBodyType.Gloves
Layer.Helm       → ArmorBodyType.Helmet
Layer.Arms       → ArmorBodyType.Arms
Layer.InnerLegs / OuterLegs / Pants → ArmorBodyType.Legs
Layer.InnerTorso / OuterTorso / Shirt → ArmorBodyType.Chest
```

---

## Armor Materials

There are **13 armor materials** that determine default craft resource and visual appearance:

| Material | Default Resource | Default Meditation |
|----------|-----------------|-------------------|
| `Cloth` | N/A (Gargish only) | All |
| `Leather` | `RegularLeather` | All |
| `Studded` | `RegularLeather` | Half |
| `Bone` | `RegularLeather` | None |
| `Spined` | `SpinedLeather` | None |
| `Horned` | `HornedLeather` | None |
| `Barbed` | `BarbedLeather` | None |
| `Ringmail` | `Copper` | Half |
| `Chainmail` | `Iron` | None |
| `Plate` | `Iron` | None |
| `Dragon` | Varies | None |
| `Wood` | N/A | None |
| `Stone` | N/A | None |

Each armor class must override `MaterialType` and optionally `DefaultResource`:

```csharp
public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;
public override CraftResource DefaultResource => CraftResource.Iron;
```

---

## AR Calculation

The Armor Rating (AR) is calculated as follows:

### AOS Formula (current)

```
AR = ScaleArmorByDurability(ArmorBase + resourceBonus + qualityBonus)
```

Where:

1. **ArmorBase** — defined per armor piece (e.g., PlateChest = 40, LeatherChest = 13)
2. **Protection level bonus** (UOR+, if ProtectionLevel != Regular):
   - `+10` fixed for Defense+
   - `+5 × (int)ProtectionLevel`
3. **Resource bonus** (UOR+, from CraftResource):
   - Metals: DullCopper +2, ShadowIron +4, Copper +6, Bronze +8, Gold +10, Agapite +12, Verite +14, Valorite +16
   - Leathers: SpinedLeather +10, HornedLeather +13, BarbedLeather +16
   - Other: 0
4. **Quality bonus**: `8 × (Quality - 1)` → Exceptional = +8
5. **Durability scaling**: `50 + 50 × hitPoints / maxHitPoints`

### Resistance Calculation

Each resistance follows the same pattern:

```
PhysicalResistance = BasePhysicalResistance + GetProtOffset() + ResourceInfo.PhysicalResist + _physicalBonus
FireResistance     = BaseFireResistance     + GetProtOffset() + ResourceInfo.FireResist     + _fireBonus
ColdResistance     = BaseColdResistance     + GetProtOffset() + ResourceInfo.ColdResist     + _coldBonus
PoisonResistance   = BasePoisonResistance   + GetProtOffset() + ResourceInfo.PoisonResist   + _poisonBonus
EnergyResistance   = BaseEnergyResistance   + GetProtOffset() + ResourceInfo.EnergyResist   + _energyBonus
```

Where:
- `Base*Resistance` — defined per armor piece (e.g., PlateChest: Phys=5, Fire=3, Cold=2, Poison=3, Energy=2)
- `GetProtOffset()` — from ProtectionLevel: Guarding=1, Hardening=2, Fortification=3, Invulnerability=4
- `ResourceInfo.*Resist` — from craft resource (see craft-resources.md)
- `_*Bonus` — fixed resist bonuses added via identification or exceptional crafting

### Scaling by Durability

```csharp
ScaleArmorByDurability(armor):
    if (maxHitPoints > 0 && hitPoints < maxHitPoints):
        scale = 50 + 50 * hitPoints / maxHitPoints
    else:
        scale = 100
    return armor * scale / 100
```

| HP Remaining | AR Output |
|-------------|-----------|
| 100% (full) | 100% |
| 50% | 75% |
| 0% (broken) | 50% |

---

## Protection Levels

Protection levels are pre-AOS mechanics that remain active in AOS (UOR+). They provide flat AR and resistance bonuses:

| Level | AR Bonus | Resist Offset | Loc # |
|-------|----------|---------------|-------|
| `Regular` | 0 | 0 | — |
| `Defense` | +15 (fixed 10 + 5×1) | +1 | 1038001 |
| `Guarding` | +20 (fixed 10 + 5×2) | +2 | 1038002 |
| `Hardening` | +25 (fixed 10 + 5×3) | +3 | 1038003 |
| `Fortification` | +30 (fixed 10 + 5×4) | +4 | 1038004 |
| `Invulnerability` | +35 (fixed 10 + 5×5) | +4 | 1038005 |

Protection levels are set during exceptional crafting or via game commands. They affect displayed name, AR, and all resistance values.

---

## Quality System

Armor has **3 quality levels** that affect durability, resist bonuses, and display:

| Quality | Durability Bonus | Resist Bonus (Exceptional Crafted) |
|---------|-----------------|-----------------------------------|
| `Low` | -10 | None |
| `Regular` | 0 | None |
| `Exceptional` | +20 | +14 to +15 resist points distributed randomly, +ArmsLore bonus (ML+) |

### Exceptional Resist Distribution

During crafting, exceptional armor receives random resist bonuses:

```csharp
DistributeBonuses(amount):
    for i = 0 to amount:
        switch random(5):
            0 → PhysicalBonus++
            1 → FireBonus++
            2 → ColdBonus++
            3 → PoisonBonus++
            4 → EnergyBonus++
```

**Amount of resist points:**
- Runic tool crafted: 6 points
- Core.SE: 15 points
- Non-Core.SE: 14 points
- Exceptional shields: **no resist bonuses** (Core.ML+)

**ArmsLore bonus** (Core.ML+, non-shield exceptional only):
- `bonus = floor(ArmsLore.Value / 20)` additional random resist points
- e.g., 80 ArmsLore = +4 bonus points, 100 ArmsLore = +5 bonus points

---

## Durability System

Armor tracks hit points and degrades with combat use. Durability levels add flat bonuses to max hit points:

| Level | Durability Bonus |
|-------|-----------------|
| `Regular` | 0 |
| `Durable` | 20 |
| `Substantial` | 50 |
| `Massive` | 70 |
| `Fortified` | 100 |
| `Indestructible` | 120 |

Durability scaling formula (UOR+):

```csharp
GetDurabilityBonus():
    if not Core.UOR:
        return (int)Durability * 5 + ((int)Quality - 1) * 10

    bonus = Durability switch
        Durable → 20
        Substantial → 50
        Massive → 70
        Fortified → 100
        Indestructible → 120
        Regular → 0

    bonus += ArmorAttributes.DurabilityBonus + ResourceInfo.ArmorDurability
    if Quality == Exceptional: bonus += 20

    return bonus
```

HP scaling when changing durability or quality:

```csharp
ScaleDurability():
    scale = 100 + GetDurabilityBonus()
    maxHitPoints = maxHitPoints * scale / 100
    hitPoints = hitPoints * scale / 100

UnscaleDurability():
    scale = 100 + GetDurabilityBonus()
    maxHitPoints = maxHitPoints * 100 / scale
    hitPoints = hitPoints * 100 / scale
```

---

## Meditation Allowance

Armor affects mana regeneration while worn. There are **3 meditation allowance levels**:

| Allowance | Mana Regen |
|-----------|-----------|
| `All` | Full mana regeneration (e.g., Leather armor) |
| `Half` | 50% mana regeneration (e.g., Studded armor) |
| `None` | No mana regeneration (e.g., Plate, Chainmail) |

The `MageArmor` attribute (AosArmorAttributes) overrides this — mage armor allows full mana regeneration regardless of the base allowance.

```csharp
MeditationAllowance = Core.AOS ? AosMedAllowance : OldMedAllowance
AosMedAllowance = DefMedAllowance (default)
```

---

## Stat Requirements

Each armor piece can require Strength, Dexterity, and/or Intelligence to wield:

```csharp
ComputeStatReq(type):
    base = type switch
        Str → StrRequirement
        Dex → DexRequirement
        Int → IntRequirement
    return AOS.Scale(base, 100 - GetLowerStatReq())

ComputeStatBonus(type):
    return type switch
        Str → StrBonus + Attributes.BonusStr
        Dex → DexBonus + Attributes.BonusDex
        Int → IntBonus + Attributes.BonusInt
```

Stat check during equip:

```csharp
CanEquip(mobile):
    if mobile.Dex < dexReq || mobile.Dex + dexBonus < 1 → fail
    if mobile.Str < strReq || mobile.Str + strBonus < 1 → fail
    if mobile.Int < intReq || mobile.Int + intBonus < 1 → fail
```

Stat bonuses from armor are applied as temporary `StatMod` entries on the wearer when equipped and removed when unequipped.

### LowerStatReq

Reduces stat requirements as a percentage. Comes from:
- `ArmorAttributes.LowerStatReq` attribute
- Craft resource's `ArmorLowerRequirements` value

```csharp
GetLowerStatReq():
    if not Core.AOS: return 0
    v = ArmorAttributes.LowerStatReq + ResourceInfo.ArmorLowerRequirements
    return min(v, 100)
```

---

## AosAttributes (24 attributes)

AosAttributes provide statistical bonuses to armor. Each is a bitmask flag:

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
| `IncreasedKarmaLoss` | 0x00800000 | Increased karma loss (ML+) |

---

## AosArmorAttributes (4 attributes)

Armor-specific attributes with property modifiers:

| Attribute | Hex Value | Effect |
|-----------|-----------|--------|
| `LowerStatReq` | 0x00000001 | Lower stat requirements % |
| `SelfRepair` | 0x00000002 | Self repair chance on hit (20% when SelfRepair > random(10)) |
| `MageArmor` | 0x00000004 | Mage armor (-30 + value to Magery skill) |
| `DurabilityBonus` | 0x00000008 | Additional durability bonus |

---

## AosSkillBonuses

Armor pieces can provide skill bonuses via the `AosSkillBonuses` system. Each piece can have up to 4 skill bonus slots, each with:
- A `SkillName` (e.g., Tactics, ArmsLore, Anatomy)
- A `Value` (bonus amount)
- A `Type` (stat association: Str, Dex, or Int)

Skill bonuses are added to the wearer when equipped (`OnAdded`) and removed when unequipped (`OnRemoved`).

---

## Crafting Integration

Armor integrates with the crafting system through the `ICraftable` interface. The `OnCraft` method is called when an armor piece is crafted:

```csharp
OnCraft(quality, makersMark, from, craftSystem, typeRes, tool, craftItem, resHue):
    Quality = (ArmorQuality)quality

    if makersMark:
        Crafter = from.RawName

    Resource = CraftResources.GetFromType(typeRes ?? craftItem.Resources[0].ItemType)
    PlayerConstructed = true
    Identified = true

    if CraftItem.RetainsColor(GetType()) and context.DoNotColor != true:
        Hue = CraftResources.GetHue(Resource)
    else:
        Hue = 0

    if Quality == Exceptional:
        // No resist bonuses for shields (Core.ML+)
        if not (Core.ML and this is BaseShield):
            DistributeBonuses(tool is BaseRunicTool ? 6 : (Core.SE ? 15 : 14))

        // ArmsLore bonus (Core.ML+, non-shield)
        if Core.ML and this is not BaseShield:
            bonus = floor(from.Skills.ArmsLore.Value / 20)
            for i = 0 to bonus:
                DistributeBonuses(1)
            from.CheckSkill(SkillName.ArmsLore, 0, 100)

    if Core.AOS:
        (tool as BaseRunicTool)?.ApplyAttributesTo(this)
```

### Crafting by Material

| Material | Crafting Skill | Resource Group |
|----------|---------------|----------------|
| Cloth | Tailoring | None (Gargish only) |
| Leather, Studded | Tailoring | Leather (Normal, Spined, Horned, Barbed) |
| Bone, Spined, Horned, Barbed | Tailoring | Leather/Scales |
| Ringmail, Chainmail, Plate | Blacksmithing | Metals (Iron through Valorite) |
| Dragon | Various | Dragon hide resources |
| Wood, Stone | Various | Wood/Stone resources |

---

## Gender Restrictions

Each armor piece can restrict wearers by gender:

```csharp
public virtual bool AllowMaleWearer => true
public virtual bool AllowFemaleWearer => true
```

Gender validation occurs in two places:

1. **`ValidateMobile(Mobile m)`** — called periodically, moves non-compliant armor to backpack with localized message
2. **`CanEquip(Mobile from)`** — checked at equip time, prevents equipping with message

Gender-restricted armor pieces include certain chest pieces (e.g., `FemalePlateChest`, `FemaleLeatherChest`), some Gargish variants, and specialty armor.

---

## Scissoring

Armor implements `IScissorable` to allow cutting with scissors. The `Scissor` method:

1. Checks armor is in the scissors user's backpack
2. Prevents scissoring of imbued armor (`Ethic.IsImbued`)
3. Looks up the tailoring craft entry for this armor type
4. If the recipe requires ≥2 units of material, returns half the material (player-crafted) or full material (non-player-crafted)
5. Returns the material item via `ScissorHelper`

```csharp
Scissor(mobile, scissors):
    if not in backpack → fail
    if imbued → fail

    entry = DefTailoring.CraftItems.SearchFor(GetType())
    if entry.Resources[0].Amount >= 2:
        material = ResourceInfo.ResourceTypes[0].CreateInstance()
        returnAmount = PlayerConstructed ? entry.Resources[0].Amount / 2 : entry.Resources[0].Amount
        ScissorHelper(mobile, material, returnAmount)
    else:
        → fail
```

---

## Display Names

Armor display names vary by identification state, quality, material, and crafter:

### Unidentified (pre-UOTD)
```
"an unidentified [armor name]"
```

### Identified (UOTD+ / game commands)
```
[durabilityText] [armor name] of [protectionText]
```

### Standard naming (pre-UOTD)
```
// Identified magic item
"[durability] [armor name] of [protection]"
"[durability] [armor name]" (no protection if Regular)

// Normal item
"an [armor name]" or "a [armor name]"
"an [armor name] of exceptional quality"
"[armor name] crafted by [crafter]"
"[armor name] crafted with exceptional quality by [crafter]"
```

### Resource naming
When crafted from a craft resource, the name includes the resource type:
```
"Dull Copper [Armor Name]"
"Barbed Leather [Armor Name]"
"Exceptional Red Scales [Armor Name]"
```

---

## Damage Absorption

When an armored wearer is hit, armor absorbs a portion of the incoming damage:

```csharp
OnHit(weapon, damageTaken):
    halfAR = ArmorRating / 2.0
    absorbed = floor(halfAR + halfAR * random(0,1))
    damageTaken = max(0, damageTaken - absorbed)

    // 25% chance to lower durability
    if random(4) == 0:
        if Core.AOS and ArmorAttributes.SelfRepair > random(10):
            HitPoints += 2  // self-repair
        else:
            wear = weapon.Type == Bashing ? max(1, absorbed / 2) : random(2)
            if wear > 0 and maxHitPoints > 0:
                applyWear(wear)  // reduce hitPoints, possibly maxHitPoints, or delete
```

Key behaviors:
- **Absorption**: Half the AR, plus half the AR randomly (range: `AR/2` to `AR`)
- **Bashing weapons**: Deal double durability wear (`absorbed / 2` minimum 1)
- **Self-repair**: 10% chance when `SelfRepair` attribute is set (rolls `SelfRepair > random(10)`)
- **Severe damage**: If wear exceeds current HP, max HP is reduced; if max HP is exhausted, armor is deleted
- **Shields**: Override `OnHit` — use Parry skill instead of AR for damage reduction (pre-AOS), or use AR with `TryLowerDurability` (AOS+)

---

## Duplication

When armor is duplicated (e.g., via a pack animal or trade), `OnAfterDuped` is called to properly copy all attribute data:

```csharp
OnAfterDuped(newItem):
    armor = newItem as BaseArmor
    armor.Attributes = new AosAttributes(newItem, Attributes)
    armor.ArmorAttributes = new AosArmorAttributes(newItem, ArmorAttributes)
    armor.SkillBonuses = new AosSkillBonuses(newItem, SkillBonuses)
    armor.Hue = Hue  // reapply resource hue
    armor.HitPoints = HitPoints  // reapply durability scaling
    armor.MaxHitPoints = MaxHitPoints
```

---

## Post-Deserialization

After loading from save, armor performs several initialization steps in `AfterDeserialization`:

1. Adds skill bonuses to wearer if `Core.AOS`
2. Resets invalid `CraftResource.None` to `DefaultResource`
3. Applies stat bonuses as `StatMod` entries on the wearer
4. Calls `CheckStatTimers()` on the wearer

---

## Armor Inheritance Hierarchy

```
Item
 └── BaseArmor (1673 lines)
      └── BaseShield (113 lines) — parry-based AR, override OnHit
           ├── MetalShield, WoodenShield, BronzeShield, etc.
      └── [Material-specific armor]
           ├── Plate/ → PlateChest, PlateArms, PlateLegs, etc.
           ├── Chain/ → ChainChest, ChainArms, ChainLegs, etc.
           ├── Studded/ → StuddedChest, StuddedArms, etc.
           ├── Leather/ → LeatherChest, LeatherArms, etc.
           ├── Bone/ → BoneChest, BoneArms, etc.
           ├── Dragon/ → Dragon armor pieces
           ├── DaemonBone/ → Daemon bone armor
           ├── Studded/ → Studded armor pieces
           ├── Ranger/ → Ranger-specific armor
           └── [Gargish variants in Cloth/, Leather/, etc.]
```

### Example: PlateChest

```csharp
[Flippable(0x1415, 0x1416)]
public partial class PlateChest : BaseArmor
{
    [Constructible]
    public PlateChest() : base(0x1415) { }

    public override double DefaultWeight => 10.0;

    public override int BasePhysicalResistance => 5;
    public override int BaseFireResistance => 3;
    public override int BaseColdResistance => 2;
    public override int BasePoisonResistance => 3;
    public override int BaseEnergyResistance => 2;

    public override int InitMinHits => 50;
    public override int InitMaxHits => 65;

    public override int AosStrReq => 95;
    public override int OldStrReq => 60;

    public override int OldDexBonus => -8;

    public override int ArmorBase => 40;

    public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;
}
```

### Example: LeatherChest

```csharp
[Flippable(0x13cc, 0x13d3)]
public partial class LeatherChest : BaseArmor
{
    [Constructible]
    public LeatherChest() : base(0x13CC) { }

    public override double DefaultWeight => 6.0;

    public override int BasePhysicalResistance => 2;
    public override int BaseFireResistance => 4;
    public override int BaseColdResistance => 3;
    public override int BasePoisonResistance => 3;
    public override int BaseEnergyResistance => 3;

    public override int InitMinHits => 30;
    public override int InitMaxHits => 40;

    public override int AosStrReq => 25;
    public override int OldStrReq => 15;

    public override int ArmorBase => 13;

    public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
    public override CraftResource DefaultResource => CraftResource.RegularLeather;
    public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.All;
}
```

### Example: ChainChest

```csharp
[Flippable(0x13bf, 0x13c4)]
public partial class ChainChest : BaseArmor
{
    [Constructible]
    public ChainChest() : base(0x13BF) { }

    public override double DefaultWeight => 7.0;

    public override int BasePhysicalResistance => 4;
    public override int BaseFireResistance => 4;
    public override int BaseColdResistance => 4;
    public override int BasePoisonResistance => 1;
    public override int BaseEnergyResistance => 2;

    public override int InitMinHits => 45;
    public override int InitMaxHits => 60;

    public override int AosStrReq => 60;
    public override int OldStrReq => 20;
    public override int OldDexBonus => -5;

    public override int ArmorBase => 28;

    public override ArmorMaterialType MaterialType => ArmorMaterialType.Chainmail;
}
```

### Example: StuddedChest

```csharp
[Flippable(0x13db, 0x13e2)]
public partial class StuddedChest : BaseArmor
{
    [Constructible]
    public StuddedChest() : base(0x13DB) { }

    public override double DefaultWeight => 8.0;

    public override int BasePhysicalResistance => 2;
    public override int BaseFireResistance => 4;
    public override int BaseColdResistance => 3;
    public override int BasePoisonResistance => 3;
    public override int BaseEnergyResistance => 4;

    public override int InitMinHits => 35;
    public override int InitMaxHits => 45;

    public override int AosStrReq => 35;
    public override int OldStrReq => 35;
    public override int OldDexBonus => -8;

    public override int ArmorBase => 16;

    public override ArmorMaterialType MaterialType => ArmorMaterialType.Studded;
    public override CraftResource DefaultResource => CraftResource.RegularLeather;
    public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.Half;
}
```

### Example: BoneChest

```csharp
[Flippable(0x144f, 0x1454)]
public partial class BoneChest : BaseArmor
{
    [Constructible]
    public BoneChest() : base(0x144F) { }

    public override double DefaultWeight => 6.0;

    public override int BasePhysicalResistance => 3;
    public override int BaseFireResistance => 3;
    public override int BaseColdResistance => 4;
    public override int BasePoisonResistance => 2;
    public override int BaseEnergyResistance => 4;

    public override int InitMinHits => 25;
    public override int InitMaxHits => 30;

    public override int AosStrReq => 60;
    public override int OldStrReq => 40;
    public override int OldDexBonus => -6;

    public override int ArmorBase => 30;
    public override int RevertArmorBase => 11;

    public override ArmorMaterialType MaterialType => ArmorMaterialType.Bone;
    public override CraftResource DefaultResource => CraftResource.RegularLeather;
}
```

### Example: MetalShield

```csharp
public partial class MetalShield : BaseShield
{
    [Constructible]
    public MetalShield() : base(0x1B7B) { }

    public override double DefaultWeight => 6.0;

    public override int BasePhysicalResistance => 0;
    public override int BaseFireResistance => 1;
    public override int BaseColdResistance => 0;
    public override int BasePoisonResistance => 0;
    public override int BaseEnergyResistance => 0;

    public override int InitMinHits => 50;
    public override int InitMaxHits => 65;

    public override int AosStrReq => 45;

    public override int ArmorBase => 11;
}
```

---

## Representative Armor Pieces

### Chest Armor Comparison

| Piece | Material | AR Base | Phys | Fire | Cold | Poison | Energy | Str Req (AOS) | Str Req (Old) | Weight |
|-------|----------|---------|------|------|------|--------|--------|---------------|---------------|--------|
| LeatherChest | Leather | 13 | 2 | 4 | 3 | 3 | 3 | 25 | 15 | 6.0 |
| StuddedChest | Studded | 16 | 2 | 4 | 3 | 3 | 4 | 35 | 35 | 8.0 |
| ChainChest | Chainmail | 28 | 4 | 4 | 4 | 1 | 2 | 60 | 20 | 7.0 |
| PlateChest | Plate | 40 | 5 | 3 | 2 | 3 | 2 | 95 | 60 | 10.0 |
| BoneChest | Bone | 30 | 3 | 3 | 4 | 2 | 4 | 60 | 40 | 6.0 |

### Shield

| Piece | AR Base | Phys | Fire | Cold | Poison | Energy | Str Req | Weight |
|-------|---------|------|------|------|--------|--------|---------|--------|
| MetalShield | 11 | 0 | 1 | 0 | 0 | 0 | 45 | 6.0 |

Shields use a separate AR formula based on Parry skill:

```csharp
// AOS+ shield AR
shieldAR = owner.Skills.Parry.Value * armorRating / 200.0 + 1.0
```

---

## Gargish Variants

Gargoyles have different body models and armor layer assignments. Gargish armor uses `OuterLegs`/`OuterTorso` layers instead of `InnerLegs`/`InnerTorso`:

| Normal Layer | Gargish Layer |
|-------------|---------------|
| `InnerTorso` (Shirt) | `OuterTorso` |
| `InnerLegs` (Pants) | `OuterLegs` |
| `InnerTorso` (Pants) | `OuterLegs` (Kilt) |

Gargish armor files are organized in subdirectories: `Cloth/`, `Leather/`, with naming convention `Gargish[Material][BodyType]Type[N].cs`.

---

## Cross-References

- [`../reference/craft-resources.md`](../reference/craft-resources.md) — full resource bonus tables for all metals, leathers, scales
- [`../systems/crafting.md`](../systems/crafting.md) — armor crafting (Blacksmithing, Tailoring)
- [`../systems/combat.md`](../systems/combat.md) — armor rating and resistance mechanics in combat
- [`../reference/skill-table.md`](../reference/skill-table.md) — skill-to-armor associations (ArmsLore, Parry)
