# Clothing

Clothing encompasses non-armor wearable items that provide durability-based protection, aesthetic customization, and optional stat bonuses. Unlike armor, clothing does not contribute to Armor Rating (AR), but still provides base resistance values and can carry AosAttributes, AosArmorAttributes, AosElementAttributes, and AosSkillBonuses. All clothing is implemented through the `BaseClothing` class which integrates crafting, dyeability, scissoring, durability, and attribute systems.

**Source Files:**
- `Projects/UOContent/Items/Clothing/BaseClothing.cs` (1056 lines) — core mechanics
- `Projects/UOContent/Items/Clothing/BaseClothing.Migrations.cs` (88 lines) — serialization migrations
- `Projects/UOContent/Items/Clothing/Hats.cs`, `Shirts.cs`, `Pants.cs`, `Shoes.cs`, `Gloves.cs`, `Cloaks.cs`, `Waist.cs`, `MiddleTorso.cs`, `OuterTorso.cs`, `OuterLegs.cs` — clothing subtypes by layer category
- `Projects/Server/Items/Layer.cs` (187 lines) — layer definitions

---

## Overview

Clothing is implemented through the `BaseClothing` class which implements multiple interfaces:

- `IAosItem` — AosAttributes and AosArmorAttributes support
- `ICraftable` — crafting integration via `OnCraft` method
- `IWearableDurability` — hit point-based durability system
- `IDyable` — dye customization via dye tubs
- `IScissorable` — cutting clothing into cloth
- `IFactionItem` — faction item tracking

All clothing pieces are assigned a `Layer` at construction that determines where they are worn on the character. Clothing does not contribute to Armor Rating but provides base resistance values and can carry stat bonuses through AosAttributes and AosSkillBonuses.

---

## Layer System

Layers define the wearable slot for each clothing item. `Layer.cs` defines **30 unique values** (0x00–0x1D) spanning character body slots, internal slots, and system slots (20 character-wearable, 9 internal/non-wearable, plus 4 alias/unused entries).

### Character-Wearable Layers

| Layer | Hex | Category | Example Items |
|-------|-----|----------|---------------|
| `Helm` | 0x06 | Head | Cap, WizardsHat, SkullCap, Bandana, masks |
| `Shirt` | 0x05 | Torso (inner) | Shirt, Tunics |
| `InnerTorso` | 0x0D | Torso (inner) | Alternate inner torso |
| `MiddleTorso` | 0x11 | Torso (middle) | Additional outer layer |
| `OuterTorso` | 0x16 | Torso (outer) | Cloaks, outer robes |
| `Pants` | 0x04 | Legs (inner) | Pants |
| `InnerLegs` | 0x18 | Legs (inner) | Gargish legs |
| `OuterLegs` | 0x17 | Legs (outer) | Gargish outer legs |
| `Gloves` | 0x07 | Hands | Gloves |
| `Shoes` | 0x03 | Feet | Shoes, Boots |
| `Arms` | 0x13 | Arms | Sleeves, arm guards |
| `Cloak` | 0x14 | Back | Cloaks |
| `Waist` | 0x0C | Waist | Belts, sashes, aprons |
| `Neck` | 0x0A | Neck | Gorgets, necklaces |
| `Hair` | 0x0B | Head | Hair styles |
| `FacialHair` | 0x10 | Head | Beards, mustaches |
| `Ring` | 0x08 | Fingers | Rings |
| `Talisman` | 0x09 | Neck | Talismans |
| `Bracelet` | 0x0E | Wrist | Bracelets |
| `Earrings` | 0x12 | Ears | Earrings |

### Internal/Non-Wearable Layers

| Layer | Hex | Purpose |
|-------|-----|---------|
| `OneHanded` | 0x01 | One-handed weapons |
| `TwoHanded` | 0x02 | Two-handed weapons, shields |
| `Backpack` | 0x15 | Player backpack |
| `Mount` | 0x19 | Mount items |
| `ShopBuy` | 0x1A | Vendor buy pack |
| `ShopResale` | 0x1B | Vendor resale pack |
| `ShopSell` | 0x1C | Vendor sell pack |
| `Bank` | 0x1D | Bank box |
| `Invalid` | 0x00 | Invalid/unassigned |

### Layer Compatibility Conflicts

Two layer conflicts are checked in `CheckPropertyConflict()`:

```csharp
Layer.Pants   → conflicts with Layer.InnerLegs
Layer.Shirt   → conflicts with Layer.InnerTorso
```

Attempting to equip a conflicting item will fail. Gargoyles have different body models that use `InnerLegs` and `OuterLegs` instead of `Pants`.

---

## BaseClothing Class

The `BaseClothing` class is an abstract base at 1056 lines that provides the foundation for all clothing items. It is declared with partial class support for serialization migrations:

```csharp
[SerializationGenerator(7, false)]
public abstract partial class BaseClothing
    : Item, IDyable, IScissorable, IFactionItem, ICraftable, IWearableDurability, IAosItem
```

### Constructor

```csharp
public BaseClothing(int itemID, Layer layer, int hue = 0)
```

The constructor initializes:
- `Layer` and `Hue` from parameters
- `_resource` to `DefaultResource`
- `_hitPoints = _maxHitPoints` to `Utility.RandomMinMax(InitMinHits, InitMaxHits)`
- All attribute objects: `AosAttributes`, `AosArmorAttributes`, `AosSkillBonuses`, `AosElementAttributes`

### Virtual Properties

Each clothing subclass overrides these virtual properties to customize behavior:

| Property | Default | Purpose |
|----------|---------|---------|
| `DefaultResource` | `CraftResource.None` | Craft resource determining default hue |
| `BasePhysicalResistance` | 0 | Base physical damage resistance |
| `BaseFireResistance` | 0 | Base fire damage resistance |
| `BaseColdResistance` | 0 | Base cold damage resistance |
| `BasePoisonResistance` | 0 | Base poison damage resistance |
| `BaseEnergyResistance` | 0 | Base energy damage resistance |
| `InitMinHits` | 0 | Minimum initial durability |
| `InitMaxHits` | 0 | Maximum initial durability |
| `ArtifactRarity` | 0 | Artifact rarity value for property display |
| `BaseStrBonus` | 0 | Strength stat bonus when equipped |
| `BaseDexBonus` | 0 | Dexterity stat bonus when equipped |
| `BaseIntBonus` | 0 | Intelligence stat bonus when equipped |
| `AosStrReq` | 10 | Strength requirement (AOS era) |
| `OldStrReq` | 0 | Strength requirement (pre-AOS era) |
| `AllowMaleWearer` | true | Whether males can wear this |
| `AllowFemaleWearer` | true | Whether females can wear this |
| `CanBeBlessed` | true | Whether this can be blessed |
| `RequiredRace` | null | Race restriction (Elf, Gargoyles, etc.) |
| `CanFortify` | true | Whether this can be fortified |

### Resistance Calculation

Base resistances are overridden per piece. Total resistance combines base + AosElementAttributes:

```csharp
public override int PhysicalResistance => BasePhysicalResistance + Resistances.Physical;
public override int FireResistance    => BaseFireResistance    + Resistances.Fire;
public override int ColdResistance    => BaseColdResistance    + Resistances.Cold;
public override int PoisonResistance  => BasePoisonResistance  + Resistances.Poison;
public override int EnergyResistance  => BaseEnergyResistance  + Resistances.Energy;
```

---

## Quality System

Clothing uses the `ClothingQuality` enum with three levels:

```csharp
public enum ClothingQuality
{
    Low,
    Regular,
    Exceptional
}
```

- **Low**: Reduced durability (lower hit points)
- **Regular**: Standard durability (default)
- **Exceptional**: Doubled durability, appears in property list and click naming ("of exceptional quality")

Quality is set during crafting via `OnCraft()` and stored in `_quality` field. Exceptional quality is serialized separately and displayed in property lists with message ID `1060636`.

---

## Durability System

Clothing implements `IWearableDurability` with a hit point-based durability system. Durability is tracked via `_hitPoints` and `_maxHitPoints` fields.

### Durability Lifecycle

**Initialization**: On construction, `_hitPoints = _maxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits)`. If both are zero (e.g., for non-durable clothing), randomization is skipped.

**Durability Bonus Scaling**: The `DurabilityBonus` attribute scales hit points up or down:

```csharp
public void ScaleDurability()
{
    var scale = 100 + ClothingAttributes.DurabilityBonus;
    _hitPoints    = (_hitPoints * scale + 99) / 100;
    _maxHitPoints = (_maxHitPoints * scale + 99) / 100;
}

public void UnscaleDurability()
{
    var scale = 100 + ClothingAttributes.DurabilityBonus;
    _hitPoints    = (_hitPoints * 100 + (scale - 1)) / scale;
    _maxHitPoints = (_maxHitPoints * 100 + (scale - 1)) / scale;
}
```

**Damage on Hit**: When struck by a weapon, `OnHit(BaseWeapon weapon, int damageTaken)` processes damage:

1. Absorbs 1-4 damage randomly
2. 25% chance to reduce durability
3. If SelfRepair > random(10), durability increases by 2 instead of degrading
4. Otherwise, wear amount depends on weapon type:
   - **Bashing**: `absorbed / 2` wear
   - **Other**: random(2) wear (0 or 1)
5. Wear is deducted from current hit points first, then max hit points
6. If max hit points are reduced below current, current is clamped down
7. If max hit points drop below wear amount, they are reduced permanently
8. If hit points reach 0, the item is deleted

**SelfRepair**: If `ClothingAttributes.SelfRepair > Utility.Random(10)`, durability increases by 2 on weapon hit instead of degrading. This only applies when AOS is enabled.

**Display**: Durability is shown in property list as `1060639` with format `"{hitPoints}\t{maxHitPoints}"` (e.g., "durability 14 / 28").

---

## Dyeability

Clothing implements `IDyable` allowing hue changes via dye tubs. The `Dye(Mobile from, DyeTub sender)` method:

1. Checks item is not deleted
2. Verifies the mobile is the wearer (`RootParent`)
3. Sets `Hue = sender.DyedHue`
4. Returns true on success

Some clothing pieces override `Dye()` to prevent dyeing, returning `sender.FailMessage`. Examples: `BearMask`, `DeerMask`, `HornedTribalMask`, `TribalMask`, `SavageMask`, `OrcishKinMask`.

---

## Scissoring

Clothing implements `IScissorable` allowing cutting into cloth using scissors. The `Scissor(Mobile from, Scissors scissors)` method:

1. Verifies item is in the mobile's backpack (message `502437`)
2. Checks item is not Ethic-imbued (message `502440`)
3. Looks up the craft entry from `DefTailoring.CraftSystem.CraftItems`
4. If the craft entry has exactly one resource type with amount >= 2:
   - Creates a resource item from the craft entry's resource
   - Returns `resource.Amount / 2` (player-constructed) or `1` (non-constructed) via `ScissorHelper`
5. Returns false if scissors cannot extract anything (message `502440`)

Scissoring is a key mechanic for recovering materials from unwanted clothing.

---

## Crafting Integration

Clothing implements `ICraftable` with `OnCraft()` handling quality assignment, maker's marks, and resource-based coloring:

```csharp
public virtual int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem,
    Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
```

1. Sets `Quality = (ClothingQuality)quality`
2. If `makersMark`: sets `Crafter = from.RawName`
3. If `DefaultResource != CraftResource.None`: sets `Resource = CraftResources.GetFromType(typeRes)`
4. Otherwise: sets `Hue = resHue`
5. Sets `PlayerConstructed = true`
6. If `context?.DoNotColor == true`: sets `Hue = 0` (black)

Hue changes from resource assignment are handled by the `Resource` property setter which calls `CraftResources.GetHue(_resource)` and `InvalidateProperties()`.

---

## AosAttributes (24 Entries)

Clothing supports the same 24 AosAttributes as weapons and armor:

| Attribute | Display Message ID | Description |
|-----------|-------------------|-------------|
| `RegenHits` | 1060444 | Hit point regeneration |
| `RegenStam` | 1060443 | Stamina regeneration |
| `RegenMana` | 1060440 | Mana regeneration |
| `DefendChance` | 1060408 | Defense chance increase (%) |
| `AttackChance` | 1060415 | Hit chance increase (%) |
| `BonusStr` | 1060485 | Strength bonus |
| `BonusDex` | 1060409 | Dexterity bonus |
| `BonusInt` | 1060432 | Intelligence bonus |
| `BonusHits` | 1060431 | Hit point increase |
| `BonusStam` | 1060484 | Stamina increase |
| `BonusMana` | 1060439 | Mana increase |
| `WeaponDamage` | 1060401 | Damage increase (%) |
| `WeaponSpeed` | 1060486 | Swing speed increase (%) |
| `SpellDamage` | 1060483 | Spell damage increase (%) |
| `CastRecovery` | 1060412 | Faster cast recovery |
| `CastSpeed` | 1060413 | Faster casting |
| `LowerManaCost` | 1060433 | Lower mana cost (%) |
| `LowerRegCost` | 1060434 | Lower reagent cost (%) |
| `ReflectPhysical` | 1060442 | Reflect physical damage (%) |
| `EnhancePotions` | 1060411 | Enhance potions (%) |
| `Luck` | 1060436 | Luck |
| `SpellChanneling` | 1060482 | Allows casting without reagents |
| `NightSight` | 1060441 | Night vision mode |
| `IncreasedKarmaLoss` | 1075210 | Increased karma loss (%) — ML+ only |

---

## AosArmorAttributes (4 Entries)

Clothing uses the same AosArmorAttributes as armor:

| Attribute | Display Message ID | Description |
|-----------|-------------------|-------------|
| `LowerStatReq` | 1060435 | Lower stat requirements (%) |
| `SelfRepair` | 1060450 | Self repair amount |
| `MageArmor` | 1060437 | Mage armor (allows mana regen while wearing) |
| `DurabilityBonus` | 1060410 | Durability bonus (%) |

`LowerStatReq` reduces strength requirements via: `ComputeStatReq(type) = AOS.Scale(StrRequirement, 100 - GetLowerStatReq())`.

---

## AosElementAttributes (7 Entries)

Resistance attributes applied to clothing:

| Attribute | Resistance Method |
|-----------|------------------|
| `Physical` | `PhysicalResistance` |
| `Fire` | `FireResistance` |
| `Cold` | `ColdResistance` |
| `Poison` | `PoisonResistance` |
| `Energy` | `EnergyResistance` |
| `Chaos` | Not directly mapped |
| `Direct` | Not directly mapped |

Total resistance = `BaseResistance + Resistances.Element`.

---

## AosSkillBonuses

Clothing can provide skill bonuses via the `AosSkillBonuses` object. When equipped, bonuses are applied to the wearer:

```csharp
public virtual void AddStatBonuses(Mobile parent)
{
    var strBonus = ComputeStatBonus(StatType.Str);  // BaseStrBonus + Attributes.BonusStr
    var dexBonus = ComputeStatBonus(StatType.Dex);  // BaseDexBonus + Attributes.BonusDex
    var intBonus = ComputeStatBonus(StatType.Int);  // BaseIntBonus + Attributes.BonusInt

    if (strBonus == 0 && dexBonus == 0 && intBonus == 0) return;

    var serial = Serial;

    if (strBonus != 0)
        parent.AddStatMod(new StatMod(StatType.Str, $"{serial}Str", strBonus, TimeSpan.Zero));

    if (dexBonus != 0)
        parent.AddStatMod(new StatMod(StatType.Dex, $"{serial}Dex", dexBonus, TimeSpan.Zero));

    if (intBonus != 0)
        parent.AddStatMod(new StatMod(StatType.Int, $"{serial}Int", intBonus, TimeSpan.Zero));
}
```

Bonuses are keyed by the item's serial to allow proper removal. They are added in `OnAdded()` and removed in `OnRemoved()`:

```csharp
public override void OnAdded(IEntity parent)
{
    if (parent is Mobile mob)
    {
        if (Core.AOS) SkillBonuses.AddTo(mob);
        AddStatBonuses(mob);
        mob.CheckStatTimers();
    }
    base.OnAdded(parent);
}

public override void OnRemoved(IEntity parent)
{
    if (parent is Mobile mob)
    {
        if (Core.AOS) SkillBonuses.Remove();
        var serial = Serial;
        mob.RemoveStatMod($"{serial}Str");
        mob.RemoveStatMod($"{serial}Dex");
        mob.RemoveStatMod($"{serial}Int");
        mob.CheckStatTimers();
    }
    base.OnRemoved(parent);
}
```

---

## Stat Requirements

Clothing enforces strength requirements via `StrRequirement`, which resolves differently based on AOS era:

```csharp
public virtual int StrRequirement
{
    get => _strReq == -1 ? Core.AOS ? AosStrReq : OldStrReq : _strReq;
}
```

- `_strReq == -1` means "use default"
- AOS era: uses `AosStrReq` (default 10)
- Pre-AOS: uses `OldStrReq` (default 0)
- `_strReq >= 0`: uses the explicit value

Requirements are scaled by `LowerStatReq`:

```csharp
public int ComputeStatReq(StatType type) => AOS.Scale(StrRequirement, 100 - GetLowerStatReq());
```

The `CanEquip()` check:

```csharp
var strReq = ComputeStatReq(StatType.Str);
if (from.Str < strReq || from.Str + strBonus < 1)
{
    from.SendLocalizedMessage(500213); // You are not strong enough to equip that.
    return false;
}
```

---

## Gender Restrictions

Each clothing piece can restrict wearers by gender via `AllowMaleWearer` and `AllowFemaleWearer`. These are enforced in two places:

**`CanEquip()`** — When attempting to equip:

```csharp
if (!AllowMaleWearer && !from.Female)
{
    from.SendLocalizedMessage(1010388); // Only females can wear this.
    return false;
}
if (!AllowFemaleWearer && from.Female)
{
    from.SendLocalizedMessage(1063343); // Only males can wear this.
    return false;
}
```

**`ValidateMobile()`** — When validating a mobile's equipment (e.g., on login):

Items that violate gender or race restrictions are automatically moved to the mobile's backpack with an appropriate message.

---

## Race Restrictions

Clothing can restrict wearers by race via the `RequiredRace` property. Currently supported races include `Elf` and `Gargoyles`:

```csharp
public virtual Race RequiredRace => null;
```

Race checks in `CanEquip()`:
- **Elf**: localized message `1072203` ("Only Elves may use this.")
- **Other races**: generic message `($"Only {RequiredRace.PluralName} may use this.")`

Race checks in `ValidateMobile()`:
- Violating items are moved to backpack with the same messages.

---

## Arcane Equipment

The `IArcaneEquip` interface defines arcane equipment for mage characters:

```csharp
public interface IArcaneEquip
{
    bool IsArcane { get; }
    int CurArcaneCharges { get; set; }
    int MaxArcaneCharges { get; set; }
}
```

Arcane charges are consumed when casting spells, providing a resource pool that mages can enhance with clothing.

---

## Faction Integration

Clothing implements `IFactionItem` tracking faction allegiance:

```csharp
public FactionItem FactionItemState
{
    get => _factionState;
    set
    {
        _factionState = value;
        if (_factionState == null) Hue = 0;
        LootType = _factionState == null ? LootType.Regular : LootType.Blessed;
    }
}
```

Faction items display as "faction item" in property list (message `1041350`) and are automatically blessed.

---

## Property Display

`GetProperties()` displays clothing properties in a specific order:

1. Crafter name (`1050043`: "crafted by ~1_NAME~")
2. Faction item (`1041350`)
3. Exceptional quality (`1060636`)
4. Race restriction (`1075086` Elves Only, `1111709` Gargoyles Only)
5. Skill bonuses (via `SkillBonuses.GetProperties()`)
6. Artifact rarity (`1061078`)
7. Weapon damage (`1060401`)
8. Defense chance (`1060408`)
9. Dex bonus (`1060409`)
10. Enhance potions (`1060411`)
11. Cast recovery (`1060412`)
12. Cast speed (`1060413`)
13. Attack chance (`1060415`)
14. Bonus hits (`1060431`)
15. Bonus int (`1060432`)
16. Lower mana cost (`1060433`)
17. Lower reagent cost (`1060434`)
18. Lower stat req (`1060435`)
19. Luck (`1060436`)
20. Mage armor (`1060437`)
21. Bonus mana (`1060439`)
22. Mana regen (`1060440`)
23. Night sight (`1060441`)
24. Reflect physical (`1060442`)
25. Stamina regen (`1060443`)
26. Hit point regen (`1060444`)
27. Self repair (`1060450`)
28. Spell channeling (`1060482`)
29. Spell damage (`1060483`)
30. Bonus stam (`1060484`)
31. Bonus str (`1060485`)
32. Weapon speed (`1060486`)
33. Increased karma loss (`1075210`) — ML+ only
34. Resistance properties (via `AddResistanceProperties()`)
35. Durability bonus (`1060410`)
36. Strength requirement (`1061170`)
37. Durability hit points (`1060639`)

### Name Display

`AddNameProperty()` handles ore-type naming for resource-based clothing:

```csharp
var oreType = _resource switch
{
    CraftResource.DullCopper    => 1053108,
    CraftResource.ShadowIron    => 1053107,
    CraftResource.Copper        => 1053106,
    CraftResource.Bronze        => 1053105,
    CraftResource.Gold          => 1053104,
    CraftResource.Agapite       => 1053103,
    CraftResource.Verite        => 1053102,
    CraftResource.Valorite      => 1053101,
    CraftResource.SpinedLeather => 1061118,
    CraftResource.HornedLeather => 1061117,
    CraftResource.BarbedLeather => 1061116,
    CraftResource.RedScales     => 1060814,
    CraftResource.YellowScales  => 1060818,
    CraftResource.BlackScales   => 1060820,
    CraftResource.GreenScales   => 1060819,
    CraftResource.WhiteScales   => 1060821,
    CraftResource.BlueScales    => 1060815,
    _                           => 0
};
```

When an ore type matches, name format is `1053099`: "~1_oretype~ ~2_armortype~".

---

## Single Click Naming

`OnSingleClick()` displays clothing names differently based on expansion:

**Pre-UOTD** (`OnSingleClickPreUotd`):
- Unnamed items: `"a/an {lowercase label}"`
- With quality: `"a name of exceptional quality"`
- With crafter: `"a name crafted by Crafter"`
- With both: `"a name crafted with exceptional quality by Crafter"`

**UOTD+**: Uses `DisplayEquipmentInfo` with `EquipInfoAttribute` list for structured display.

---

## Duplication

`OnAfterDuped()` handles item duplication (e.g., from magic containers):

```csharp
public override void OnAfterDuped(Item newItem)
{
    newItem.Attributes    = new AosAttributes(newItem, Attributes);
    newItem.Resistances   = new AosElementAttributes(newItem, Resistances);
    newItem.SkillBonuses  = new AosSkillBonuses(newItem, SkillBonuses);
    newItem.ClothingAttributes = new AosArmorAttributes(newItem, ClothingAttributes);

    newItem.Hue = Hue;          // Reset hue for resource
    newItem.HitPoints = HitPoints;      // Reset for durability
    newItem.MaxHitPoints = MaxHitPoints;
}
```

All attribute objects are deep-copied to the new item.

---

## Representative Clothing Files

### Hats.cs

Contains hat-type clothing on `Layer.Helm`. Examples include:

| Item | ItemID | Base Resistances | InitMinHits | InitMaxHits | Notes |
|------|--------|-----------------|-------------|-------------|-------|
| `Kasa` | 0x2798/0x27E3 (flippable) | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `ClothNinjaHood` | 0x278F | P:3 F:3 C:6 Pn:9 E:9 | 20 | 30 | High poison/energy resist |
| `FlowerGarland` | 0x2306 | P:3 F:3 C:6 Pn:9 E:9 | 20 | 30 | Same resist as ninja hood |
| `FloppyHat` | 0x1713 | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `Cap` | 0x1715 | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `SkullCap` | 0x1544 | P:0 F:3 C:5 Pn:8 E:8 | ML?14:7 | ML?28:12 | ML era has higher durability |
| `Bandana` | 0x1540 | P:0 F:3 C:5 Pn:8 E:8 | 20 | 30 | |
| `BearMask` | 0x1545 | P:5 F:3 C:8 Pn:4 E:4 | 20 | 30 | Undyeable |
| `DeerMask` | 0x1547 | P:2 F:6 C:8 Pn:1 E:7 | 20 | 30 | Undyeable |
| `HornedTribalMask` | 0x1549 | P:6 F:9 C:0 Pn:4 E:5 | 20 | 30 | Undyeable |
| `TribalMask` | 0x154B | P:3 F:0 C:6 Pn:10 E:5 | 20 | 30 | Undyeable |
| `OrcishKinMask` | 0x141B | P:1 F:1 C:7 Pn:7 E:8 | 20 | 30 | -20 karma, undyeable, blocked by savage kin paint |
| `SavageMask` | 0x154B | P:3 F:0 C:6 Pn:10 E:5 | 20 | 30 | Random bird hue, undyeable |
| `WizardsHat` | 0x1718 | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `MagicWizardsHat` | 0x1718 | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | BaseStrBonus:-5, BaseDexBonus:-5, BaseIntBonus:+5 |
| `Bonnet` | 0x1719 | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `FeatheredHat` | 0x171A | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `TricorneHat` | 0x171B | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |
| `JesterHat` | 0x171C | P:0 F:5 C:9 Pn:5 E:5 | 20 | 30 | |

Hats also implement `IShipwreckedItem` for shipwreck-recovered items. Exceptional hats distribute 14-15 (15 in SE, 14 otherwise) bonus resist points via `DistributeBonuses()`.

### Other Clothing Files

- `Shirts.cs` — Inner torso clothing
- `Pants.cs` — Inner leg clothing
- `Shoes.cs` — Foot clothing
- `Gloves.cs` — Hand clothing
- `Cloaks.cs` — Outer/back clothing
- `Waist.cs` — Belt/sash clothing
- `MiddleTorso.cs` — Middle torso layer
- `OuterTorso.cs` — Outer torso layer
- `OuterLegs.cs` — Outer leg layer (Gargish)

Each follows the same pattern: inherit from `BaseClothing`, specify layer, override `DefaultResource`, `Base*Resistance`, `InitMinHits`, `InitMaxHits`, and optionally `AosStrReq`, `AllowMaleWearer`, `AllowFemaleWearer`, and stat bonuses.

---

## Cross-References

- `getting-started/character-creation.md` — starting clothing
- `systems/crafting.md` — clothing crafting (Tailoring)
- `reference/craft-resources.md` — resource hue tables
- `items/armor.md` — armor system (comparable quality/durability)
