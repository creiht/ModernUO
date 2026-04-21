# Jewels

Jewels in ModernUO encompass wearable jewelry items and standalone gem resources. Jewelry provides magical bonuses through the AosAttribute system, integrates with the Tinkering crafting engine, and uses gem types for visual identification and recipe association. Standalone gem items serve as crafting components, trade resources, and enchanting materials.

**Source Files:**
- `Projects/UOContent/Items/Jewels/BaseJewel.cs` (445 lines) — core mechanics
- `Projects/UOContent/Items/Jewels/` directory — jewelry type implementations
- `Projects/UOContent/Items/Gems/` directory — standalone gem items
- `Projects/UOContent/Engines/Craft/DefTinkering.cs` — jewelry crafting recipes
- `Projects/UOContent/Misc/Loot.cs` — gem loot tables
- `Projects/UOContent/Items/Misc/CommunicationCrystals.cs` — gem value mappings
- `Projects/UOContent/Mobiles/Vendors/SBInfo/SBVagabond.cs`, `SBJewel.cs` — vendor gem prices

---

## Overview

Jewelry is implemented through the abstract `BaseJewel` class which implements:

- `IAosItem` — AosAttributes, AosElementAttributes, and AosSkillBonuses support
- `ICraftable` — crafting integration via `OnCraft` method with resource-based hue and gem type assignment
- `IWearableDurability` — hit point-based durability system

Jewelry pieces are placed on specific layers determined by their type at construction. All jewelry inherits durability, attribute, and skill bonus systems from `BaseJewel`.

---

## Gem Types

There are **9 gem types** used as crafting components and trade resources. Gems are standalone `Item` instances that are stackable, weigh 0.1, and serve as the second resource slot in jewelry crafting recipes.

| Gem Type | ItemID | Vendor Price | Gem Value | CraftResource Enum |
|----------|--------|-------------|-----------|-------------------|
| `StarSapphire` | 0xF21 | 125 | 1250 | N/A (not in CraftResource enum) |
| `Emerald` | 0xF10 | 100 | 1000 | N/A |
| `Sapphire` | 0xF19 | 100 | 1000 | N/A |
| `Ruby` | 0xF13 | 75 | 1000 | N/A |
| `Citrine` | 0xF15 | 50 | 500 | N/A |
| `Amethyst` | 0xF16 | 100 | 1000 | N/A |
| `Tourmaline` | 0xF2D | 75 | 750 | N/A |
| `Amber` | 0xF25 | 50 | 500 | N/A |
| `Diamond` | 0xF26 | 200 | 2000 | N/A |

The `GemType` enum (9 entries + `None` = 10 total) is stored as an encoded int in `BaseJewel._gemType` and is used for label lookup and crafting recipe identification.

```csharp
public enum GemType
{
    None,          // 0
    StarSapphire,  // 1
    Emerald,       // 2
    Sapphire,      // 3
    Ruby,          // 4
    Citrine,       // 5
    Amethyst,      // 6
    Tourmaline,    // 7
    Amber,         // 8
    Diamond        // 9
}
```

Gem values are defined in `CommunicationCrystals.cs` as a lookup table mapping gem types to their worth for trade and appraisal purposes.

---

## Jewelry Types

There are **5 jewelry types** that inherit from `BaseJewel`, each occupying a specific equipment layer:

| Jewelry Type | Layer | Layer Value | Base Label Offset | Example Items |
|--------------|-------|-------------|-------------------|---------------|
| `Ring` | `Ring` | 0x08 | 1044176 | GoldRing (0x108A), SilverRing (0x1F09) |
| `Bracelet` | `Bracelet` | 0x0E | 1044221 | GoldBracelet (0x1086), SilverBracelet (0x1F06) |
| `Earrings` | `Earrings` | 0x12 | 1044203 | GoldEarrings (0x1087), SilverEarrings (0x1F07) |
| `Necklace` | `Neck` | 0x0A | 1044241 | Necklace (0x1085), GoldNecklace (0x1088), GoldBeadNecklace (0x1089), SilverNecklace (0x1F08), SilverBeadNecklace (0x1F05) |

Each jewelry type has an abstract base class (e.g., `BaseRing`) that sets the `Layer` and `BaseGemTypeNumber` label offset in its constructor. Concrete implementations (e.g., `GoldRing`) provide the `ItemID` and `DefaultWeight`.

All jewelry pieces have `DefaultWeight = 0.1`. The exception is `Beads` (0x108B), which weighs 1.0 and inherits directly from `Item` rather than `BaseJewel` — it is a decorative item without attribute or durability support.

Label lookup uses the formula: `BaseGemTypeNumber + (int)GemType - 1`. For example, a StarSapphire Ring has label number `1044176 + 1 - 1 = 1044176` ("star sapphire ring"), while an Emerald Ring has `1044176 + 2 - 1 = 1044177` ("emerald ring").

---

## Jewelry Attributes

Jewelry supports three attribute systems, all initialized in the `BaseJewel` constructor:

### AosAttributes (20)

Controlled by the `AosAttributes` component. Applied to the wearer when equipped via `OnAdded()` and removed via `OnRemoved()`. Displayed in the property list via `GetProperties()`.

| Attribute | Property ID | Display Format |
|-----------|-------------|----------------|
| `WeaponDamage` | 1060401 | "damage increase ~val~%" |
| `DefendChance` | 1060408 | "defense chance increase ~val~%" |
| `BonusDex` | 1060409 | "dexterity bonus ~val~" |
| `EnhancePotions` | 1060411 | "enhance potions ~val~%" |
| `CastRecovery` | 1060412 | "faster cast recovery ~val~" |
| `CastSpeed` | 1060413 | "faster casting ~val~" |
| `AttackChance` | 1060415 | "hit chance increase ~val~%" |
| `BonusHits` | 1060431 | "hit point increase ~val~" |
| `BonusInt` | 1060432 | "intelligence bonus ~val~" |
| `LowerManaCost` | 1060433 | "lower mana cost ~val~%" |
| `LowerRegCost` | 1060434 | "lower reagent cost ~val~%" |
| `Luck` | 1060436 | "luck ~val~" |
| `BonusMana` | 1060439 | "mana increase ~val~" |
| `RegenMana` | 1060440 | "mana regeneration ~val~" |
| `NightSight` | 1060441 | "night sight" (boolean) |
| `ReflectPhysical` | 1060442 | "reflect physical damage ~val~%" |
| `RegenStam` | 1060443 | "stamina regeneration ~val~" |
| `RegenHits` | 1060444 | "hit point regeneration ~val~" |
| `SpellChanneling` | 1060482 | "spell channeling" (boolean) |
| `SpellDamage` | 1060483 | "spell damage increase ~val~%" |
| `BonusStam` | 1060484 | "stamina increase ~val~" |
| `BonusStr` | 1060485 | "strength bonus ~val~" |
| `WeaponSpeed` | 1060486 | "swing speed increase ~val~%" |
| `IncreasedKarmaLoss` | 1075210 | "increased karma loss ~val~%" (ML+ only) |

### AosElementAttributes (7)

Resistances displayed via `AddResistanceProperties()` after all other properties. Covers Physical, Fire, Cold, Poison, Energy, Chaos, and Direct elements. Jewelry overrides the armor resistance properties via `PhysicalResistance`, `FireResistance`, `ColdResistance`, `PoisonResistance`, and `EnergyResistance` virtual properties that delegate to `Resistances`.

### AosSkillBonuses

Skill bonus entries for character skills. Added to the wearer via `SkillBonuses.AddTo(from)` in `OnAdded()`, removed via `SkillBonuses.Remove()` in `OnRemoved()`. Displayed via `SkillBonuses.GetProperties(list)` in `GetProperties()`.

---

## Stat Modifiers on Equip

When jewelry with `BonusStr`, `BonusDex`, or `BonusInt` is equipped, `OnAdded()` applies temporary `StatMod` objects to the mobile using the jewelry's Serial number as a unique key:

```csharp
// On equip
from.AddStatMod(new StatMod(StatType.Str, $"{serial}Str", strBonus, TimeSpan.Zero));
from.AddStatMod(new StatMod(StatType.Dex, $"{serial}Dex", dexBonus, TimeSpan.Zero));
from.AddStatMod(new StatMod(StatType.Int, $"{serial}Int", intBonus, TimeSpan.Zero));
from.CheckStatTimers();

// On remove
from.RemoveStatMod($"{serial}Str");
from.RemoveStatMod($"{serial}Dex");
from.RemoveStatMod($"{serial}Int");
from.CheckStatTimers();
```

The `TimeSpan.Zero` duration means the modifier persists indefinitely while equipped. The Serial-keyed name ensures multiple pieces of jewelry with the same stat bonus are tracked independently.

---

## Durability

Jewelry implements hit point-based durability through `IWearableDurability`. Each jewelry type overrides `InitMinHits` and `InitMaxHits` to define its durability range.

Durability is displayed via property ID 1060639 in the format "durability ~current~ / ~max~". When `HitPoints` reaches 0, the item is deleted.

```csharp
// Constructor initializes durability
_hitPoints = _maxHitPoints = Utility.RandomMinMax(InitMinHits, InitMaxHits);

// HitPoints property enforces bounds and triggers invalidation
public int HitPoints
{
    get => _hitPoints;
    set
    {
        if (value != _hitPoints && _maxHitPoints > 0)
        {
            _hitPoints = value;
            if (_hitPoints < 0) { Delete(); }
            else if (_hitPoints > _maxHitPoints) { _hitPoints = _maxHitPoints; }
            InvalidateProperties();
            MarkDirty();
        }
    }
}
```

Durability of 0 (from `InitMinHits` and `InitMaxHits` defaulting to 0) means the jewelry has no durability tracking and cannot degrade.

---

## Crafting Integration

Jewelry is crafted through the Tinkering skill via `DefTinkering.AddJewelrySet()`. Each gem type generates 6 crafting recipes (Ring, Bracelet, Earrings, Necklace ×2, GoldBracelet).

### Crafting Recipe Structure

For each of the 9 gem types, `AddJewelrySet()` creates:

| Recipe | Target Item | Skill Range | Metal Required | Gem Required |
|--------|------------|-------------|---------------|-------------|
| 1 | GoldRing | 40.0–90.0 | 2× IronIngot | 1× gem |
| 2 | SilverBeadNecklace | 40.0–90.0 | 2× IronIngot | 1× gem |
| 3 | GoldNecklace | 40.0–90.0 | 2× IronIngot | 1× gem |
| 4 | GoldEarrings | 40.0–90.0 | 2× IronIngot | 1× gem |
| 5 | GoldBeadNecklace | 40.0–90.0 | 2× IronIngot | 1× gem |
| 6 | GoldBracelet | 40.0–90.0 | 2× IronIngot | 1× gem |

Total: 9 gem types × 6 recipes = **54 jewelry crafting recipes**.

### OnCraft Implementation

The `OnCraft` method in `BaseJewel` handles resource assignment:

1. **Resource slot 1 (metal):** Sets `Resource` property which calls `CraftResources.GetHue(_resource)` to assign the correct hue for the metal type. If `DoNotColor` context flag is set, `Hue = 0`.

2. **Resource slot 2 (gem):** Uses an if/else chain to check `typeof(T)` against each gem type class and sets `_gemType` accordingly. The gem type determines the item's label number and visual appearance.

3. Returns quality value of `1`.

```csharp
public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem,
    Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
{
    var resourceType = typeRes ?? craftItem.Resources[0].ItemType;
    Resource = CraftResources.GetFromType(resourceType);

    var context = craftSystem.GetContext(from);
    if (context?.DoNotColor == true) { Hue = 0; }

    if (craftItem.Resources.Count > 1)
    {
        resourceType = craftItem.Resources[1].ItemType;
        if (resourceType == typeof(StarSapphire)) { GemType = GemType.StarSapphire; }
        else if (resourceType == typeof(Emerald)) { GemType = GemType.Emerald; }
        // ... 7 more gem types
    }
    return 1;
}
```

### OnAfterDuped

When a jewel is duplicated, `OnAfterDuped` deep-copies all three attribute systems so the copy retains its magical properties:

```csharp
public override void OnAfterDuped(Item newItem)
{
    if (newItem is not BaseJewel jewel) { return; }
    jewel.Attributes = new AosAttributes(newItem, Attributes);
    jewel.Resistances = new AosElementAttributes(newItem, Resistances);
    jewel.SkillBonuses = new AosSkillBonuses(newItem, SkillBonuses);
    jewel.Hue = Hue;
}
```

---

## Serialization

`BaseJewel` uses the ModernUO serialization system with version 4. The serialized fields are:

| Version | Field | Type | Description |
|---------|-------|------|-------------|
| 0 | `_maxHitPoints` | `EncodedInt` | Maximum durability |
| 1 | `HitPoints` | `EncodedInt` | Current durability |
| 2 | `_resource` | `EncodedInt` | Craft resource (metal type) |
| 3 | `_gemType` | `EncodedInt` | Gem type enum value |
| 4 | `_attributes` | `AosAttributes` | AosAttributes component |
| 5 | `_resistances` | `AosElementAttributes` | AosElementAttributes component |
| 6 | `_skillBonuses` | `AosSkillBonuses` | AosSkillBonuses component |

The `AfterDeserialization` callback restores stat modifiers and skill bonuses if the item was equipped at time of serialization.

---

## Standalone Gems

Nine gem item classes exist as standalone resources in `Projects/UOContent/Items/Gems/`. These are stackable items used in crafting, vendor trade, and loot generation.

| Gem Class | ItemID | Stackable | Weight |
|-----------|--------|-----------|--------|
| `StarSapphire` | 0xF21 | Yes | 0.1 |
| `Emerald` | 0xF10 | Yes | 0.1 |
| `Sapphire` | 0xF19 | Yes | 0.1 |
| `Ruby` | 0xF13 | Yes | 0.1 |
| `Citrine` | 0xF15 | Yes | 0.1 |
| `Amethyst` | 0xF16 | Yes | 0.1 |
| `Tourmaline` | 0xF2D | Yes | 0.1 |
| `Amber` | 0xF25 | Yes | 0.1 |
| `Diamond` | 0xF26 | Yes | 0.1 |

Gems appear in:
- **Vendor inventories** — SBVagabond and SBJewel sell all 9 gem types
- **Loot tables** — Defined in `LootPack.GemItems` and `Loot.GemItems`
- **Communication crystals** — Used as currency/value references in `CommunicationCrystals.cs`

---

## Cross-References

- [`reference/craft-resources.md`](../reference/craft-resources.md) — full craft resource bonus tables
- [`systems/crafting.md`](../systems/crafting.md) — Tinkering craft definition and crafting system
- [`items/clothing.md`](clothing.md) — layer system and equipment slots
- [`items/armor.md`](armor.md) — AosAttributes and AosElementAttributes reference
