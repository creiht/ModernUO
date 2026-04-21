# Food

Food and beverage items are consumables that restore hunger (stamina), stamina, and can carry poison. ModernUO's food system includes pre-cooked foods, raw ingredients that require cooking, a full beverage/refill system, and integration with farming, cooking, and poison mechanics.

**Source Files:**
- `Projects/UOContent/Items/Food/Food.cs` (535 lines) — core mechanics
- `Projects/UOContent/Items/Food/CookableFood.cs` (335 lines) — raw food → cooked
- `Projects/UOContent/Items/Food/Beverage.cs`, `BeverageEmpty.cs` — drinks
- `Projects/UOContent/Items/Food/Bowls.cs`, `Fruits.cs`, `Vegetables.cs`, `Asian.cs`, `Chocolatiering.cs` — food subtypes
- `Projects/UOContent/Items/Food/EndlessDecanter.cs` — special food item
- `Projects/UOContent/Items/Food/HolidayFoods.cs` — seasonal food items
- `Projects/UOContent/Items/Farming/FarmableCrop.cs` — crop harvesting
- `Projects/UOContent/Items/Cooking.cs` — cooking skill integration

---

## Overview

Food items are implemented through the abstract `Food` class, which handles eating mechanics, hunger restoration, stamina restoration, and poison application. A separate `CookableFood` abstract class represents raw food items that can be cooked at heat sources. Beverages use the `BaseBeverage` class with a quantity-based pour/fill system.

All food items are stackable by default, with stacking restricted to items that share the same poison and poisoner state.

---

## Hunger System

### Fill Factor

Each food item has a `FillFactor` value (default: 1) that determines how much hunger it restores. Hunger ranges from 0 (not hungry) to 20 (full).

```
iHunger = from.Hunger + fillFactor
if iHunger >= 20: from.Hunger = 20, "You manage to eat the food, but you are stuffed!"
else: from.Hunger = iHunger
```

### Hunger Threshold Messages

| Resulting Hunger | Message ID | Message |
|------------------|------------|---------|
| `>= 20` (before eating) | 500867 | "You are simply too full to eat any more!" |
| `20` (capped) | 500872 | "You manage to eat the food, but you are stuffed!" |
| `< 5` | 500868 | "You are still extremely hungry." |
| `< 10` | 500869 | "You begin to feel more satiated." |
| `< 15` | 500870 | "You feel much less hungry." |
| `>= 15` | 500871 | "You feel quite full." |

### Stamina Restoration

Eating food restores stamina using the formula:

```
from.Stam += Random(6, 3) + fillFactor / 5
```

Only applied if `from.Stam < from.StamMax`. Integer division for `fillFactor / 5`.

| Fill Factor | Avg Stamina Restored |
|-------------|---------------------|
| 1 | 7-9 |
| 2 | 8-10 |
| 3 | 9-11 |
| 4 | 10-12 |
| 5 | 11-13 |
| 6 | 12-14 |
| 8 | 14-16 |
| 10 | 16-18 |
| 20 | 20-22 (capped at StamMax) |

---

## Core Food Mechanics (`Food` class)

### Serialization Fields

| Field | Type | Field # | Serialized |
|-------|------|---------|------------|
| `_poisoner` | `Mobile` | 0 | Only for GMs |
| `_poison` | `Poison` | 1 | Only for GMs |
| `_fillFactor` | `int` | 2 | Only for GMs |

### Constructor

```csharp
public Food(int itemID, int amount = 1) : base(itemID)
{
    Stackable = true;
    Amount = amount;
    FillFactor = 1;
}
```

### Stacking Rules

Food items stack with other food items **only if** they share the same `Poison` and `Poisoner`:

```csharp
public override bool CanStackWith(Item dropped) =>
    (dropped is not Food food || Poison == food.Poison && Poisoner == food.Poisoner) &&
    base.CanStackWith(dropped);
```

This prevents poisoned food from mixing with unpoisoned food, and different poison types from stacking together.

### Eating (`Eat` method)

```csharp
public virtual bool Eat(Mobile from)
{
    if (CheckHunger(from))
    {
        from.PlaySound(Utility.Random(0x3A, 3));  // Random eat sound (0x3A-0x3C)
        if (from.Body.IsHuman && !from.Mounted)
        {
            from.Animate(34, 5, 1, true, false, 0);  // Eating animation
        }
        if (Poison != null)
        {
            from.ApplyPoison(Poisoner, Poison);
        }
        Consume();  // Destroy the item
        return true;
    }
    return false;
}
```

### Double-Click Behavior

- Only works when `InRange(GetWorldLocation(), 1)` (adjacent)
- Only works if the player is alive
- Adds an `EatEntry` to context menu for alive players
- Non-movable items cannot be double-clicked

---

## Fill Factor Values by Item Category

| Category | Fill Factor | Examples |
|----------|------------|----------|
| Small fruits/vegetables | 1 | Apple, Carrot, Cabbage, Onion, Lettuce, Pumpkin (small), Turnip, Lime, Lemon, Pear, Peach, Dates, Grapes, Banana |
| Cheese slices, small bowls | 1-2 | CheeseSlice, WoodenBowlOfCarrots, PewterBowlOfCarrots |
| Bacon | 1 | Bacon, SlabOfBacon |
| Fried eggs, cookies, muffins, sushi | 4 | FriedEggs, Cookies, Muffins, SushiRolls |
| Bread, fish steak, ham, chicken leg | 3 | BreadLoaf, FishSteak, Ham, ChickenLeg, FrenchBread |
| Cooked bird, pies, melon, cheese wheel | 5 | CookedBird, FruitPie, MeatPie, PumpkinPie, Watermelon, CheeseWheel |
| Pizza, quiche | 6 | CheesePizza, SausagePizza, ApplePie, Quiche, PeachCobbler |
| Pumpkin (large) | 8 | Pumpkin (large) |
| Cake | 10 | Cake |
| Roast pig | 20 | RoastPig (maxes out hunger) |

---

## Poison System

Food items can be poisoned using the poisoning skill. The poison information is stored on the item:

- **`Poison`** — The `Poison` type (e.g., Deadly Poison, Deadly Poison, Deadly Poison, Deadly Poison)
- **`Poisoner`** — The `Mobile` who applied the poison (GM-visible only)

When a poisoned food item is eaten, `from.ApplyPoison(Poisoner, Poison)` is called. Poisoned food stacks separately from unpoisoned food, and different poison types do not stack together.

---

## Cooking System (`CookableFood`)

### Abstract Pattern

```csharp
public abstract partial class CookableFood : Item
{
    private int _cookingLevel;  // Required cooking skill level

    public abstract Food Cook();  // Returns the cooked food item
}
```

Raw food items extend `CookableFood` and override the `Cook()` method to return the cooked `Food` instance. The cooking level requirement is stored as a stat on the raw food item.

### Heat Source Detection

The cooking system checks for valid heat source items by item ID range:

| Item ID Range | Heat Source |
|---------------|-------------|
| `0xDE3`–`0xDE9` | Campfires |
| `0x461`–`0x48E` | Sandstone oven/fireplace |
| `0x92B`–`0x96C` | Stone oven/fireplace |
| `0xFAC` | Firepit |
| `0x184A`–`0x184C`, `0x184E`–`0x1850` | Heating stand (left/right) |
| `0x398C`–`0x399F` | Fire field |

### Cooking Level Requirements

| Raw Item | Cooking Level | Cooked Result |
|----------|--------------|---------------|
| `RawRibs`, `RawLambLeg`, `RawChickenLeg`, `RawBird`, `RawFishSteak` | 10 | Ribs (5), LambLeg (5), ChickenLeg (4), CookedBird (5), FishSteak (3) |
| `Eggs`, `BrightlyColoredEggs`, `EasterEggs` | 15 | FriedEggs (4) |
| `CookieMix` | 20 | Cookies (4) |
| `UnbakedPie`, `UnbakedQuiche` | 25 | FruitPie (5), MeatPie (5), PumpkinPie (5), ApplePie (5), PeachCobbler (5), Quiche (6) |
| `CakeMix` | 40 | Cake (10) |
| `UncookedCheesePizza`, `UncookedSausagePizza` | 20 | CheesePizza (6), SausagePizza (6) |

### Easter Eggs

`BrightlyColoredEggs` and `EasterEggs` have randomized hue: `3 + Utility.Random(20) * 5`. Both cook into regular `FriedEggs`.

---

## Bowl System

Bowl foods have `FillFactor = 2` and `Stackable = false`. Each overrides `Eat()` to return the appropriate empty container after eating:

| Bowl Food | Item ID | Empty Container |
|-----------|---------|-----------------|
| WoodenBowlOfCarrots | 0x15F9 | EmptyWoodenBowl (0x15F8) |
| WoodenBowlOfCorn | 0x15FA | EmptyWoodenBowl |
| WoodenBowlOfLettuce | 0x15FB | EmptyWoodenBowl |
| WoodenBowlOfPeas | 0x15FC | EmptyWoodenBowl |
| PewterBowlOfCarrots | 0x15FE | EmptyPewterBowl (0x15FD) |
| PewterBowlOfCorn | 0x15FF | EmptyPewterBowl |
| PewterBowlOfLettuce | 0x1600 | EmptyPewterBowl |
| PewterBowlOfPeas | 0x1601 | EmptyPewterBowl |
| PewterBowlOfPotatos | 0x1602 | EmptyPewterBowl |
| WoodenBowlOfStew | 0x1604 | EmptyWoodenTub |
| WoodenBowlOfTomatoSoup | 0x1606 | EmptyWoodenTub |

---

## Beverage System

### `BeverageType` Enum

| Value | Description |
|-------|-------------|
| `Ale` | Ale — BAC +1 |
| `Cider` | Cider — BAC +3 |
| `Liquor` | Liquor — BAC +4 |
| `Milk` | Milk — no alcohol |
| `Wine` | Wine — BAC +2 |
| `Water` | Water — no alcohol |

### `IHasQuantity` Interface

```csharp
public interface IHasQuantity
{
    int Quantity { get; set; }
}
```

### `IWaterSource` Interface

```csharp
public interface IWaterSource : IHasQuantity { }
```

### BaseBeverage Core Properties

```csharp
public abstract partial class BaseBeverage : Item, IHasQuantity
{
    private Poison _poison;
    private Mobile _poisoner;
    private BeverageType _content;
    private int _quantity;

    public virtual bool ShowQuantity => MaxQuantity > 1;
    public virtual bool Fillable => true;
    public virtual bool Pourable => true;
    public bool IsEmpty => _quantity <= 0;
    public bool ContainsAlcohol => !IsEmpty && _content not (Milk or Water);
    public bool IsFull => _quantity >= MaxQuantity;
}
```

### Beverage Container Types

| Container | MaxQuantity | Fillable? | Content Types | Item ID Range |
|-----------|-------------|-----------|---------------|---------------|
| `BeverageBottle` | 5 | No | Ale, Cider, Liquor, Milk, Wine, Water | 0x99F (Ale/Cider), 0x99B (Liquor/Milk/Water) |
| `Jug` | 10 | No | All types | 0x9C8 |
| `CeramicMug` | 1 | Yes | All types | 0x995-0x999, 0x9CA |
| `PewterMug` | 1 | Yes | All types | 0xFFF-0x1002 |
| `Goblet` | 1 | Yes | All types | 0x99A, 0x9B3, 0x9BF, 0x9CB |
| `GlassMug` | 5 | Yes | All types | 0x1F7D-0x1F94 |
| `Pitcher` | 5 | Yes | All types | 0x1F95-0x1F9E, 0xFF6-0xFF9, 0x9AD, 0x9F0 |

### Quantity Description (Visual Indicators)

```csharp
public virtual int GetQuantityDescription()
{
    return (_quantity * 100 / MaxQuantity) switch
    {
        <= 0  => 1042975,  // "Empty"
        <= 33 => 1042974,  // "Low"
        <= 66 => 1042973,  // "Medium"
        _     => 1042972   // "Full"
    };
}
```

### Fill Interaction (`Fill_OnTarget`)

A beverage can be filled from:

| Source | Behavior |
|--------|----------|
| Another `BaseBeverage` | Copies content, poison, poisoner; quantity transfers respecting MaxQuantity |
| `BaseWaterContainer` | Fills with Water only (if current content is Water or empty) |
| `IWaterSource` items | Fills with Water |
| `Cow` | Milk directly from cows (`Content = Milk`, `Quantity = MaxQuantity`) |
| Swamp water tiles | Quest-specific: `WitchApprenticeQuest.FindIngredientObjective` with `SwampWater` |

### Pour Interaction (`Pour_OnTarget`)

A beverage can be poured onto:

| Target | Behavior |
|--------|----------|
| Another `BaseBeverage` | Transfers content, poison, poisoner |
| Self (the drinker) | Reduces thirst by 1, adds BAC, triggers `HeaveTimer`, applies poison if present |
| `BaseWaterContainer` | Only Water can be poured in |
| `PlantItem` | Calls `plant.Pour(from, this)` for watering plants |
| `WaterVatEast/South` | Pours water for SolenMatriarchQuest |
| `WaterElemental` | Triggers EndlessDecanter acquisition mechanic (Pitcher override) |

### Blood Alcohol Content (BAC) System

Per-drink BAC addition:

```csharp
var bac = Content switch
{
    BeverageType.Ale    => 1,
    BeverageType.Wine   => 2,
    BeverageType.Cider  => 3,
    BeverageType.Liquor => 4,
    _                   => 0
};
from.BAC = Math.Min(from.BAC + bac, 60);
```

### HeaveTimer (Drunk Effects)

- **Trigger**: `CheckHeaveTimer` called on login and when BAC increases
- **Interval**: 5 seconds
- **Effects per tick**:
  - 10% chance to sober up (BAC-- if > 0)
  - Stam--1, Mana--1
  - 25% chance to: turn random direction + heave animation (32) + hiccup sound
  - BAC cap: 60
  - When BAC reaches 0: "You feel sober." message

### Static Utility: `ConsumeTotal`

```csharp
public static bool ConsumeTotal(Container pack, BeverageType content, int quantity)
```

Consumes a specified total quantity of a beverage type from all matching beverages in a container. Iterates through all `BaseBeverage` items, depletes them one by one until the required quantity is met.

---

## Endless Decanter (`EndlessDecanter.cs`)

### Special Properties

- Inherits from `Pitcher`, always contains `BeverageType.Water`
- `Hue = 0x399`, `LootType = Blessed`
- `MaxQuantity = 5` (inherited from Pitcher)
- `ComputeItemID()` always returns `0x0FF6` (non-zero so `OnQuantityChanged` fires on empty)

### Link/Refill Mechanic

Three serialized fields:

| Field | Type | Field # | Description |
|-------|------|---------|-------------|
| `_linked` | `bool` | 0 | Whether a trough is linked |
| `_linkLocation` | `Point3D` | 1 | The linked trough's position |
| `_linkMap` | `Map` | 2 | The linked trough's map |

When the decanter becomes empty and the owner is within 10 tiles of the linked trough on the correct map:

```csharp
protected override void OnQuantityChanged()
{
    if (_linked && Content == BeverageType.Water && Quantity == 0 && RootParent is Mobile owner)
    {
        if (owner.Map == _linkMap && owner.InRange(_linkLocation, 10))
        {
            Quantity = MaxQuantity;  // Auto-refill
            owner.SendLocalizedMessage(1115901);  // "The decanter has automatically been filled..."
            owner.PlaySound(0x4E);
        }
    }
}
```

### Acquisition Mechanic (Throwing at WaterElemental)

```csharp
public static void HandleThrow(Pitcher pitcher, WaterElemental elemental, Mobile thrower)
{
    // Requirements: full pitcher, within 5 tiles, elemental still has decanter
    if (!pitcher.IsFull || !thrower.InRange(elemental.Location, 5) || !elemental.HasDecanter)
        return;

    elemental.Damage(1, thrower);

    // 10% chance to get the decanter
    if (0.1 > Utility.RandomDouble())
    {
        elemental.HasDecanter = false;
        pitcher.Delete();
        thrower.AddToBackpack(new EndlessDecanter());
    }
    else
    {
        pitcher.Delete();  // Pitcher shatters
    }
}
```

### Context Menu Entries

| Entry | Target | Behavior |
|-------|--------|----------|
| Link | Water trough (0xB41-0xB44) | Sets the refill link |
| Unlink | — | Removes the link |

---

## Chocolate & Candy System

### Ingredients (Non-Food Items)

| Item | Item ID | Hue | Stackable |
|------|---------|-----|-----------|
| `CocoaLiquor` | 0x103F | 0x46A | No |
| `SackOfSugar` | 0x1039 | 0x461 | Yes |
| `CocoaButter` | 0x1044 | 0x457 | No |
| `Vanilla` | 0xE2A | 0x462 | Yes |
| `CocoaPulp` | 0xF7C | 0x219 | Yes |

### Chocolate Types

All extend `CandyCane` (toothache system, blessed loot type, non-stackable):

| Chocolate | Item ID | Hue | Label |
|-----------|---------|-----|-------|
| `DarkChocolate` | 0xF10 | 0x465 | "Dark chocolate" |
| `MilkChocolate` | 0xF18 | 0x461 | "Milk chocolate" |
| `WhiteChocolate` | 0xF11 | 0x47E | "White chocolate" |

---

## Holiday Foods

### CandyCane

- **Random item ID**: `0x2BDD + Random(4)` (4 variants)
- **Non-stackable**, **Blessed** loot type
- **Overrides `CheckHunger`**: Always returns true, adds 32 to a `_toothAches` counter
- **Toothache Timer**: 30-second intervals, counts down from eaten value
  - At >60 eaten: random complaint dialogue + possible heave animation
  - At 60: pain subsides message
  - Timer stops when counter reaches 0 or eater is deleted

### GingerBreadCookie

- **Random item ID**: `0x2BE1` or `0x2BE2` (2 variants)
- **Blessed**, non-stackable
- **Overrides `Eat`**: 1 in 7 chance to eat normally; otherwise plays one of 7 running-away dialogue messages and returns false (does not consume)

---

## Farming & Crop Integration

### FarmableCrop Base Class

```csharp
public abstract partial class FarmableCrop : Item
{
    private bool _picked;
    public abstract Item GetCropObject();  // Returns the harvested food item
    public abstract int GetPickedID();     // Item ID after picking
}
```

When double-clicked on an unpicked crop in range with LOS (not locked down):

```csharp
public override void OnDoubleClick(Mobile from)
{
    if (!_picked)
    {
        OnPicked(from, loc, map);
    }
}

public virtual void OnPicked(Mobile from, Point3D loc, Map map)
{
    ItemID = GetPickedID();
    var spawn = GetCropObject();
    spawn?.MoveToWorld(loc, map);
    _picked = true;
    Unlink();
    Timer.StartTimer(TimeSpan.FromMinutes(5.0), Delete);  // Auto-delete after 5 min
}
```

### Farmable Crop to Food Mapping

| Crop | Harvested Food Item | Fill Factor |
|------|---------------------|-------------|
| `FarmableCarrot` | Carrot (ID 3191-3192) | 1 |
| `FarmableCabbage` | Cabbage | 1 |
| `FarmableLettuce` | Lettuce | 1 |
| `FarmableOnion` | Onion | 1 |
| `FarmablePumpkin` | Pumpkin | 8 |
| `FarmableTurnip` | Turnip | 1 |
| `FarmableWheat` | WheatSheaf (used in flour mill) | — |
| `FarmableCotton` | Cotton (non-food) | — |
| `FarmableFlax` | Flax (non-food) | — |

### WheatSheaf / Flour Mill Integration

```csharp
// WheatSheaf.OnTarget
if (obj is IFlourMill mill)
{
    var needs = mill.MaxFlour - mill.CurFlour;
    if (needs > Amount) needs = Amount;
    mill.CurFlour += needs;
    Consume(needs);
}
```

The `FlourMill` (`IFlourMill` interface) has `MaxFlour = 2` and three stages (Empty, Filled, Working). When a player activates a full mill, it takes 5 seconds to produce a `SackFlour`.

### SackFlour (IHasQuantity)

- Starts with `Quantity = 20`
- ItemID changes as quantity decreases: `0x1039` → `0x1045` → ... → `0` (deleted at 0)
- Double-clicking on a movable sack advances its ItemID (visual depletion)

---

## Plant Watering Integration

`BaseBeverage.Pour_OnTarget` supports pouring onto `PlantItem`:

```csharp
else if (targ is PlantItem item)
{
    item.Pour(from, this);
}
```

`PlantItem.Pour` checks:
- Plant is not dead (status < DeadTwigs)
- Plant is not decorative
- Player has usability rights
- Beverage is not empty, is pourable, and contains Water

If all checks pass, the plant receives the water and the beverage quantity is decremented.

---

## Vendor Sales (SE Expansion)

The `SBSEFood` vendor sells:

| Item | Buy Price | Max Amount |
|------|-----------|------------|
| Wasabi | 2g each | 20 |
| BentoBox | 6g each | 20 |
| GreenTeaBasket | 2g each | 20 |

Sell prices: Wasabi 1g, BentoBox 3g, GreenTeaBasket 1g.

---

## Special Non-Food Items in Food Directory

| Item | Item ID | Weight | Notes |
|------|---------|--------|-------|
| `SheafOfHay` | — | 10.0 | Extends `Item` (not `Food`), decoration/hay bales |
| `Glass` | — | 0.1 | Empty drinkware (BeverageEmpty.cs) |
| `GlassBottle` | — | 0.3 | Empty drinkware (BeverageEmpty.cs) |
| `WoodenBowl` | — | 1.0 | Ingredient container (Cooking.cs) |
| `BowlFlour` | — | 1.0 | Ingredient container (Cooking.cs) |

---

## Expansion Flags

Several items check `Core.ML` (Mongrel Lord expansion flag) for stackability:

| Item | Stackable Condition |
|------|---------------------|
| `Cookies` | `Stackable = Core.ML` |
| `Quiche` | `Stackable = Core.ML` |
| `Dough` | `Stackable = Core.ML` |
| `SweetDough` | `Stackable = Core.ML` |
| `SushiPlatter` | `Stackable = Core.ML` |

---

## Key Enums

| Enum | Values | Location |
|------|--------|----------|
| `BeverageType` | Ale, Cider, Liquor, Milk, Wine, Water | Beverage.cs |
| `FlourMillStage` | Empty, Filled, Working | FlourMillEastAddon.cs |
| `PlantStatus` | BowlOfDirt, Seed, Sapling, Plant, FullGrownPlant, DecorativePlant, DeadTwigs, Stage1-9 | PlantItem.cs |

---

## Cross-References

- `systems/crafting.md` — cooking skill, flour mill
- `systems/poisons.md` — poisoning food mechanics
- `systems/farming.md` — crop planting and harvesting
- `reference/skill-table.md` — skill-to-item associations
