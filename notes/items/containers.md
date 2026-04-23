# Containers

Containers are the storage backbone of ModernUO, implementing everything from personal backpacks to secure house storage, treasure chests with respawning loot, trappable containers with deadly traps, and furniture pieces that double as storage.

**Source Files:**
- `Projects/Server/Items/Container.cs` (1666 lines) — core engine Container class
- `Projects/UOContent/Items/Containers/Container.cs` (566 lines) — BaseContainer
- `Projects/UOContent/Items/Containers/BaseTreasureChest.cs` — respawnable treasure chests
- `Projects/UOContent/Items/Containers/TreasureMapChest.cs` — treasure map reward chests
- `Projects/UOContent/Items/Containers/ParagonChest.cs` — paragon reward system
- `Projects/UOContent/Items/Containers/LockableContainer.cs` — lockable/lockpickable containers
- `Projects/UOContent/Items/Containers/MarkContainer.cs` — rune marking system
- `Projects/UOContent/Items/Containers/FurnitureContainer.cs` — furniture storage
- `Projects/UOContent/Items/Containers/SalvageBag.cs` — salvage conversion bags
- `Projects/UOContent/Items/Containers/TrappableContainer.cs` — trap mechanics
- `Projects/UOContent/Items/Containers/Strongbox.cs` — house co-owner secure storage
- `Projects/UOContent/Items/Containers/Bedroll.cs` — sleep/logout system
- `Projects/UOContent/Items/Containers/Campfire.cs` — campfire safety system
- `Projects/UOContent/Engines/FillableContent/` — FillableContainer system
- `Projects/Server/Items/Layer.cs` — layer definitions

---

## Container Engine

### Class Hierarchy

```
Server.Items.Item
    └── Server.Items.Container                        [Core engine - Projects/Server/Items/Container.cs]
            ├── Server.Items.BankBox                  [Bank box]
            │
            └── UOContent.Items.BaseContainer         [Abstract base - Projects/UOContent/Items/Containers/Container.cs]
                    ├── UOContent.Items.Backpack
                    │       └── UOContent.Items.CreatureBackpack
                    │       └── UOContent.Items.StrongBackpack
                    ├── UOContent.Items.StrongBox
                    ├── UOContent.Items.Pouch
                    │       └── UOContent.Items.TrappableContainer
                    ├── UOContent.Items.BaseBagBall
                    │       ├── UOContent.Items.SmallBagBall
                    │       └── UOContent.Items.LargeBagBall
                    ├── UOContent.Items.Bag
                    │       └── UOContent.Items.SalvageBag
                    ├── UOContent.Items.Barrel
                    ├── UOContent.Items.Keg
                    ├── UOContent.Items.PicnicBasket
                    ├── UOContent.Items.Basket
                    ├── UOContent.Items.LockableContainer       [with TrappableContainer mixin]
                    │       ├── UOContent.Items.WoodenBox
                    │       ├── UOContent.Items.Small/Medium/LargeCrate
                    │       ├── UOContent.Items.MetalBox/Chest/GoldenChest
                    │       ├── UOContent.Items.WoodenChest/PlainWoodenChest/OrnateWoodenChest
                    │       ├── UOContent.Items.GildedWoodenChest/WoodenFootLocker
                    │       ├── UOContent.Items.FinishedWoodenChest/RarewoodChest/DecorativeBox
                    │       ├── UOContent.Items.MarkContainer
                    │       ├── UOContent.Items.BaseTreasureChest
                    │       │       ├── UOContent.Items.WoodenTreasureChest
                    │       │       ├── UOContent.Items.MetalGoldenTreasureChest
                    │       │       └── UOContent.Items.MetalTreasureChest
                    │       ├── UOContent.Items.TreasureMapChest
                    │       ├── UOContent.Items.ParagonChest
                    │       ├── UOContent.Items.FillableContainer
                    │       │       └── [content-specific fillable chests]
                    │       └── UOContent.Items.TrappableContainer (base for Pouch)
                    ├── UOContent.Items.FurnitureContainer (multiple classes)
                    │       ├── UOContent.Items.TallCabinet/ShortCabinet
                    │       ├── UOContent.Items.RedArmoire/CherryArmoire/MapleArmoire
                    │       ├── UOContent.Items.ElegantArmoire/FancyElvenArmoire/SimpleElvenArmoire
                    │       ├── UOContent.Items.FullBookcase/EmptyBookcase
                    │       ├── UOContent.Items.Drawer/FancyDrawer
                    │       ├── UOContent.Items.Armoire/FancyArmoire
                    │
                    └── UOContent.Items.BaseBoard (Container + ISecurable)
```

### Core Fields

The core `Container` class maintains these tracked fields:

| Field | Type | Description |
|-------|------|-------------|
| `m_ContainerData` | ContainerData | Visual data from containers.cfg (gump ID, bounds, drop sound) |
| `m_Items` | List\<Item\> | Direct child items |
| `m_TotalGold` | int | Cached total gold (recursive) |
| `m_TotalItems` | int | Cached total item count (recursive) |
| `m_TotalWeight` | int | Cached total weight (recursive) |
| `_version` | int | Version counter, incremented on Add/Remove |
| `_liftOverride` | bool | GM lift override flag |

### Global Defaults

```csharp
Container.GlobalMaxItems = 125;    // Default maximum items per container
Container.GlobalMaxWeight = 400;   // Default maximum weight (stones) per container
```

### Key Properties

| Property | Default | Description |
|----------|---------|-------------|
| `MaxItems` | -1 (uses DefaultMaxItems) | Maximum item slot count; 0 = unlimited |
| `GumpID` | -1 (uses DefaultGumpID) | Gump ID for display |
| `DropSound` | -1 (uses DefaultDropSound) | Sound on drop |
| `MaxWeight` | DefaultMaxWeight | Weight limit; 0 = unlimited |
| `TotalItems` | cached | Recursive total item count |
| `TotalWeight` | cached | Recursive total weight |
| `TotalGold` | cached | Recursive total gold value |
| `Openers` | null | List of Mobiles who have opened this container |
| `DisplaysContent` | true | Whether content is displayed in OPL |
| `IsPublicContainer` | false | If true, any player can view contents |
| `IsDecoContainer` | computed | True if non-movable, unsecured, no parent, no lift override |

### Weight Limits

Weight limits are hierarchical. A container's `MaxWeight` is calculated as:

```csharp
public virtual int MaxWeight => Parent is Container { MaxWeight: 0 } ? 0 : DefaultMaxWeight;
```

If a parent container has `MaxWeight = 0` (unlimited, like BankBox), child containers also have unlimited weight.

`BaseContainer` overrides `DefaultMaxWeight` for secure containers:

```csharp
public override int DefaultMaxWeight => IsSecure ? 0 : base.DefaultMaxWeight;
```

Secure containers inside houses have unlimited weight.

### Totals Tracking

Containers override `GetTotal` and `UpdateTotal` for three total types:

| TotalType | Field | Description |
|-----------|-------|-------------|
| `TotalType.Gold` | `m_TotalGold` | Sum of all gold value (recursive) |
| `TotalType.Items` | `m_TotalItems` | Count of all items (recursive) |
| `TotalType.Weight` | `m_TotalWeight` | Sum of all weight (recursive) |

Virtual items (`IsVirtualItem`) are excluded from totals.

- `UpdateTotals()` — recalculates all totals from scratch
- `UpdateTotal(TotalType, int oldValue, int newValue)` — handles incremental changes

---

## Container Data

Container visual data is loaded from `Data/containers.cfg` at static construction. This maps item IDs to:

| Field | Description | Default |
|-------|-------------|---------|
| `gumpID` | Gump ID used for display | `0x3C` |
| `bounds` | Rectangle2D for display clipping | `(44, 65, 142, 94)` |
| `dropSound` | Sound played on drop | `0x48` |
| `IDs` | Comma-separated list of item IDs | — |

Format is tab-delimited: `gumpID\ttx ty w h\tdropSound\tid1,id2,...`

---

## Weight & Capacity System

### Container Weight Checking

`CheckHold()` validates whether an item can be stored:

```csharp
public virtual bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
```

Checks performed:
1. Parent container hierarchy traversal
2. Deco container restrictions
3. `MaxItems` limit (0 = unlimited)
4. `MaxWeight` limit (0 = unlimited)

Weight formula:
```
TotalWeight + plusWeight + item.TotalWeight + item.PileWeight > MaxWeight
```

`PileWeight` is the base weight of a single item in a stack (not total stack weight).

### Mobile Carry Capacity

The Mobile class tracks weight via the totals system:

```csharp
public virtual int MaxWeight => int.MaxValue;    // Override in PlayerMobile for actual cap
public int TotalWeight => GetTotal(TotalType.Weight);
```

When weight exceeds capacity, the Mobile receives speed reduction penalties.

### Specific Container Weight Overrides

| Container | DefaultMaxWeight | MaxItems | Notes |
|-----------|-----------------|----------|-------|
| BankBox | 0 (unlimited) | 125 | Virtual item |
| Backpack | 400 (256 base + 144 ML) | 125 | Enhanced in ML expansion |
| CreatureBackpack | 3.0 | 125 | For BaseCreature; auto-deletes when empty |
| StrongBackpack | 1600 | 125 | Pack animal storage |
| StrongBox | 0 (unlimited) | 25 | House co-owner storage; 30-min decay |
| SalvageBag | 2.0 base | 125 | Special salvage use |
| Bedroll | 5.0 | 125 | Not a Container (extends Item directly) |

---

## Drop & Stack Mechanics

### TryDropItem

Drops an item into a container with auto-stacking:

```csharp
public virtual bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage, bool playSound)
```

Process:
1. Iterate existing items, attempt `StackWith()` for each
2. If no stack found, call `DropItem()` for new slot
3. Returns true on success, false if capacity exceeded

### OnStackAttempt

Handles stacking when dropping:

```csharp
public virtual bool OnStackAttempt(Mobile from, Item stack, Item dropped) =>
    CheckHold(from, dropped, true, false) && stack.StackWith(from, dropped);
```

### TryDropItems (Bulk Drop)

Uses `PooledRefQueue` for memory efficiency:
1. First attempts stacking with existing items
2. Then allocates new slots
3. Tracks `extraItems` and `extraWeight` for cumulative limit checking
4. Uses `CanStackWith()` for pre-checks

### BaseContainer House Integration

When dropping items into a container inside a locked-down house, `BaseContainer.TryDropItem` automatically calls `house.LockDown()` on the dropped item.

---

## Security System

### ISecurable Interface

```csharp
public interface ISecurable
{
    SecureLevel Level { get; set; }
}
```

### SecureLevel Enum

| Level | Description | Access |
|-------|-------------|--------|
| `Owner` | House owner only | `IsOwner(mobile)` |
| `CoOwners` | Owner + co-owners | `IsCoOwner(mobile)` |
| `Friends` | Owner + friends | `IsFriend(mobile)` |
| `Anyone` | Everyone | Always accessible |
| `Guild` | Guild members | `IsGuildMember(mobile)` |

### SecureAccessResult Enum

```csharp
public enum SecureAccessResult
{
    Insecure,     // No security set
    Accessible,   // Player has access
    Inaccessible  // Player does not have access
}
```

### SecureAccess Logic

```csharp
public bool HasSecureAccess(Mobile m, SecureLevel level)
{
    if (m.AccessLevel >= AccessLevel.GameMaster) return true;
    if (IsCombatRestricted(m)) return false;

    return level switch
    {
        SecureLevel.Owner  => IsOwner(m),
        SecureLevel.CoOwners => IsCoOwner(m),
        SecureLevel.Friends => IsFriend(m),
        SecureLevel.Anyone => true,
        SecureLevel.Guild => IsGuildMember(m),
        _ => false
    };
}
```

### SecureInfo Class

Each container within a house can have its own `SecureInfo` entry:

```csharp
public class SecureInfo : ISecurable
{
    public Container Item { get; }
    public SecureLevel Level { get; set; }
}
```

### CheckAccessible

```csharp
public static bool CheckAccessible(Mobile m, Item item)
{
    if (m.AccessLevel >= AccessLevel.GameMaster) return true;
    var house = FindHouseAt(item);
    if (house == null) return true;

    var res = house.CheckSecureAccess(m, item);
    // Insecure -> continue, Accessible -> return true, Inaccessible -> return false

    if (house.HasLockedDownItem(item))
    {
        return house.IsCoOwner(m) && item is Container;
    }
    return true;
}
```

### CheckHold for House Storage

```csharp
public static bool CheckHold(Mobile m, Container cont, Item item, ...)
{
    var house = FindHouseAt(cont);
    if (house?.IsAosRules != true) return true;

    if (house.HasSecureItem(cont) && !house.CheckAosStorage(1 + item.TotalItems + plusItems))
    {
        // "This action would exceed the secure storage limit of the house."
        return false;
    }
    return true;
}
```

### Lockdown System

```csharp
public bool LockDown(Mobile m, Item item, bool checkIsInside)
```

Requirements:
- Must be co-owner of house
- Item must be inside house bounds
- Item cannot be imbued
- Parent container must be locked down first (if applicable)
- Cannot exceed `MaxLockDowns` limit (SE) or `CheckAosLockdowns` (AOS rules)

---

## Container Types

### Personal Storage

#### Backpack

Standard personal storage for player characters.

```csharp
public class Backpack : BaseContainer
```

| Property | Value |
|----------|-------|
| DefaultMaxWeight | 400 (256 base + 144 ML bonus) |
| MaxItems | 125 |
| Layer | `Layer.Backpack` (0x15) |
| Starting Item | Yes, given at character creation |

#### BankBox

Secure bank storage tied to a player account.

```csharp
public class BankBox : Container
```

| Property | Value |
|----------|-------|
| DefaultMaxWeight | 0 (unlimited) |
| MaxItems | 125 |
| Layer | `Layer.Bank` (0x1D) |
| Virtual | Yes |

Bank access can be blocked by feature flags:

```csharp
if (!ServerFeatureFlags.BankAccess && Owner?.AccessLevel < AccessLevel.Administrator)
{
    Owner.SendMessage(0x22, "Bank access is temporarily disabled.");
    return;
}
```

#### CreatureBackpack

Backpack for BaseCreature (pets).

| Property | Value |
|----------|-------|
| DefaultMaxWeight | 3.0 |
| MaxItems | 125 |
| Hue | 5 |
| Auto-delete | Yes, when empty |

Special restrictions:
- GM-only lift (players cannot drag lift)
- `OnDragDropInto` and `TryDropItem` always return false (no drop-into)
- Name-based labeling

#### StrongBackpack

Backpack for pack animals.

| Property | Value |
|----------|-------|
| DefaultMaxWeight | 1600 |
| MaxItems | 125 |
| Default weight | 13.0 |

Content display is only visible to the creature's control master.

### Bags

#### Bag

Standard portable storage bags (SmallBagBall, LargeBagBall variants use bag ball items).

#### SalvageBag

Converts metal/leather items back to raw materials. Extends `Bag`.

Context menu options:

1. **SalvageIngots** — Converts metal armor/weapons/dragon barding deeds to ingots
   - Requires blacksmithing tool and forge proximity
   - Mining skill affects yield: `((4 + mining) * craftResource.Amount - 4) * 0.0068`
   - Skill difficulty by resource: DullCopper(65), Copper(67), ShadowIron(69), Gold(71), Agapite(73), Verite(75), Heartstone(77), Bloodstone(85), Valorite(99)

2. **SalvageCloth** — Converts leather/cloth items to raw materials via scissors
   - Requires scissors in backpack
   - Types: Leather, Cloth, SpinedLeather, HornedLeather, BarbedLeather, Bandage, Bone

3. **SalvageAll** — Both operations combined

| Property | Value |
|----------|-------|
| DefaultMaxWeight | 2.0 base weight |
| MaxItems | 125 |

### Chests & Boxes

#### LockableContainer

Base class for lockable containers with `ILockpickable` interface.

**Lock Level Constants:**

| Constant | Value | Description |
|----------|-------|-------------|
| `CannotPick` | 0 | Cannot be picked |
| `MagicLock` | -255 | Magic lock (no physical lockpick level) |

**ILockpickable Properties:**

| Property | Description |
|----------|-------------|
| `LockLevel` | Current lock difficulty |
| `Locked` | Whether currently locked |
| `Picker` | Mobile who last picked the lock |
| `MaxLockLevel` | Maximum achievable lock level |
| `RequiredSkill` | Tinkering skill required |

**Trap Override:**

```csharp
public override bool TrapOnOpen => !_trapOnLockpick;
```

When `_trapOnLockpick` is true, trap fires on lockpick attempt instead of open.

#### LockableContainer Variants

| Variant | ItemID | Default Lock |
|---------|--------|-------------|
| WoodenBox | 0xECA | 50 |
| SmallCrate | 0x982 | 50 |
| MediumCrate | 0x983 | 50 |
| LargeCrate | 0x984 | 50 |
| MetalBox | 0xE9B | 50 |
| MetalChest | 0xE9C | 50 |
| MetalGoldenChest | 0xE9D | 50 |
| WoodenChest | 0xE7C | 50 |
| PlainWoodenChest | 0xE79 | 50 |
| OrnateWoodenChest | 0xE78 | 50 |
| GildedWoodenChest | 0xE7B | 50 |
| WoodenFootLocker | 0xE7A | 50 |
| FinishedWoodenChest | 0xE77 | 50 |
| RarewoodChest | 0xE76 | 50 |
| DecorativeBox | 0x998 | 50 |

### Treasure Containers

#### BaseTreasureChest

Respawnable treasure chest with auto-reset.

**TreasureLevel Enum:**

| Level | Lock | Gold Range | Respawn Time |
|-------|------|-----------|-------------|
| Level1 | 5 | 100-300 | 10-60 min |
| Level2 | 20 | 300-600 | 10-60 min |
| Level3 | 50 | 600-1000 | 10-60 min |
| Level4 | 70 | 1000-2000 | 10-60 min |
| Level5 | 90 | 2000-5000 | 10-60 min |
| Level6 | 100 | 5000-9000 | 10-60 min |

**Concrete Variants:**

| Variant | ItemID |
|---------|--------|
| WoodenTreasureChest | 0x9AB |
| MetalGoldenTreasureChest | 0xE40 |
| MetalTreasureChest | 0xE41 |

#### TreasureMapChest

Treasure map reward chest with guardians and expiry.

| Property | Value |
|----------|-------|
| Expiry Timer | 3 hours (`_expireTimer`) |
| Guardians | List of Mobiles that must be killed before level-0 chests open |
| Criminality | Taking from non-owner chests in PVP maps triggers criminality |
| Monster Spawn | 10% chance per item lifted to spawn additional monsters |
| Lifted Tracking | `_lifted` HashSet prevents duplicate spawns |

**Level 6 Artifacts:** May contain one of 20 named artifacts.

Cannot be refilled — `CheckHold` always rejects (message 1048122). Owner can use context menu to permanently remove the chest.

#### ParagonChest

Paragon reward chest, randomly selected from 4 chest types. Extends `LockableContainer`.

**Randomized Properties:**
- ItemID from: {0x9AB, 0xE40, 0xE41, 0xE7C}
- Hue from 11 random hues

**Loot Tables by Level:**

| Level | Lock | Gold | Scrolls | Gear | Reagents | Gems | Explosion Power |
|-------|------|------|---------|------|----------|------|----------------|
| 1 | 36 | 200 | 1 | 2 | 40-60 | 1 | 25 |
| 2 | 76 | 500 | 2 | 4 | 40-60 | 2 | 50 |
| 3 | 84 | 750 | 3 | 6 | 40-60 | 3 | 75 |
| 4 | 92 | 1000 | 4 | 8 | 40-60 | 4 | 100 |
| 5 | 100 | 1250 | 5 | 10 | 40-60 | 5 | 125 |

**AOS Attributes:** Random 1-6 attributes with tiered ranges:

| Tier | Attribute Range |
|------|----------------|
| 1 | 10-20 |
| 2 | 12-24 |
| 3 | 15-30 |
| 4 | 18-36 |
| 5 | 20-70 |

#### FillableContainer

Content-based respawning containers (loot crates, etc.).

| Property | Value |
|----------|-------|
| Respawn Time | 60-90 minutes (configurable) |
| Spawn Threshold | 2 items (won't respawn if more items present) |
| Trap (level 1) | 80% chance (4/5 roll) |
| Trap (level >1) | Always traps |
| Trap Types | Poison or explosion (random) |

Locking: `difficulty = (level - 1) * 30`

Dynamic content via `FillableEntry` types registered with `FillableContent.Acquire(location, map)`.

### Mark Containers

MarkContainer allows players to mark secret locations for recall runes. Extends `LockableContainer`.

**Bone variant:** ItemID 0xECA, hue 1102
**Non-bone variant:** ItemID 0xE79 (Pouch)

**Key Properties:**

| Property | Description |
|----------|-------------|
| `AutoLock` | When true, container re-locks after being opened |
| `_relockTimer` | InternalTimer for auto-relock (5 minutes) |
| `_targetMap` | Target map for recall rune |
| `_target` | Target location Point3D |
| `_description` | Rune description string |

**Rune Marking:**

```csharp
public void Mark(RecallRune rune)
{
    if (_targetMap != null)
    {
        rune.Marked = true;
        rune.TargetMap = _targetMap;
        rune.Target = _target;
        rune.Description = _description;
        rune.House = null;
    }
}
```

Triggered by dragging a `RecallRune` onto the MarkContainer. Both `OnDragDrop` and `OnDragDropInto` check for RecallRune type.

**Auto-Lock Behavior:**

`InternalTimer` (lines 205-225): Recalls after 5 minutes, sets `Locked = true` and `LockLevel = MagicLock`. Timer is deserialized and restarted if auto-lock is enabled and container is currently locked.

**Secret Location Generation:**

`SecretLocGen_OnCommand` generates mark containers at 10 specific Malas coordinates via `CreateMalasPassage`.

### Furniture Containers

FurnitureContainer variants that provide storage while serving as house decorations.

| Variant | Type |
|---------|------|
| TallCabinet, ShortCabinet | Cabinet storage |
| RedArmoire, CherryArmoire, MapleArmoire | Armoire storage |
| ElegantArmoire, FancyElvenArmoire, SimpleElvenArmoire | Elven armoire storage |
| FullBookcase, EmptyBookcase | Bookcase storage |
| Drawer, FancyDrawer | Drawer storage |
| Armoire, FancyArmoire | Standard armoire storage |

All implement `Container` with furniture-specific display.

### Special Containers

#### Strongbox

House co-owner's personal secure container.

| Property | Value |
|----------|-------|
| Item Limit | 25 |
| Weight Limit | 0 (unlimited) |
| Decay Time | 30 minutes |
| Ownership | Tied to `Mobile _owner` and `BaseHouse _house` |
| IChoppable | Yes — can be destroyed with an axe by owner/co-owner/GM |

Decay: `Decays = _house == null || _owner?.Deleted || !_house.IsCoOwner(_owner)`

Access: Owner + GM when owner is co-owner of house.

Conversion: `ConvertToStandardContainer()` transfers contents to a `MetalBox`.

#### Bedroll

NOT a Container — extends `Item` directly.

| Property | Value |
|----------|-------|
| Rolled ItemID | 0xA57 |
| Unrolled ItemIDs | 0xA55/0xA56 (direction-dependent) |
| Flippable | 0xA57, 0xA58, 0xA59 |
| Weight | 5.0 |
| MaxItems | 125 |

Functionality:
- Double-click to roll/unroll
- When unrolled near a safe campfire, allows safe logout via `LogoutGump`
- `Campfire.GetEntry(from)?.Safe == true` check for safe logout eligibility

#### Campfire

NOT a Container — extends `Item` directly.

| Property | Value |
|----------|-------|
| Burning ItemID | 0xDE3 |
| Extinguishing ItemID | 0xDE9 |
| Off ItemID | 0xDEA |
| Burning Light | Circle300 |
| Extinguishing Light | Circle150 |
| Auto-destruction | 100 seconds |
| Secure Range | 7 tiles |

**Status Progression:**
- 0-60s: Burning
- 60-90s: Extinguishing
- 90-100s: Off

**CampfireEntry System:**
- Tracks players near campfire
- Marks as "safe" after 30 seconds
- Automatic registration when player enters range of burning campfire

---

## Trappable Containers

### TrapType Enum

| Type | Description |
|------|-------------|
| `None` | No trap |
| `MagicTrap` | Magical damage trap |
| `ExplosionTrap` | Explosive damage trap |
| `DartTrap` | Projectile dart trap |
| `PoisonTrap` | Poison cloud trap |

### TrappableContainer Fields

| Field | Type | Description |
|-------|------|-------------|
| `_trapLevel` | int | Trap difficulty level |
| `_trapPower` | int | Trap raw power (overrides level-based calc) |
| `_trapType` | TrapType | Active trap type |

### Trap Behavior

- `TrapOnOpen` (default true): Triggers when opened via double-click
- `OnTelekinesis`: Also triggers trap when moved by telekinesis
- Trap fires once, then resets: `TrapType = None, TrapPower = 0, TrapLevel = 0`

### Trap Damage Calculations

| Trap Type | Damage Formula | Range | Effect |
|-----------|---------------|-------|--------|
| Explosion | `RandomMinMax(10,30) * _trapLevel` or `_trapPower` | 3 tiles | Direct damage, "skin blisters" message |
| Magic | `_trapPower` | 1 tile | Direct damage, 5 particle effects |
| Dart | `RandomMinMax(5,15) * _trapLevel` or `_trapPower` | 3 tiles | Physical damage, "dart embeds" message |
| Poison | `Poison.GetPoison(min(4, _trapLevel-1))` or Greater + damage | 3 tiles | Poison application, "noxious green cloud" |

### LockableContainer Trap Override

```csharp
public override bool TrapOnOpen => !_trapOnLockpick;
```

When `_trapOnLockpick` is true, trap fires on lockpick attempt instead of open. The `LockPick()` method (line 94-102) unlocks and optionally fires the trap.

---

## Layer System (Container-Related)

| Layer | Hex | Description |
|-------|-----|-------------|
| `Backpack` | 0x15 | Player backpacks, CreatureBackpack, StrongBackpack |
| `Bank` | 0x1D | Bank boxes |
| `Mount` | 0x19 | Mount items |
| `ShopBuy` | 0x1A | Vendor buy pack |
| `ShopResale` | 0x1B | Vendor resale pack |
| `ShopSell` | 0x1C | Vendor sell pack |

---

## Feature Flags

### Container Feature Flag Check

```csharp
public override void DisplayTo(Mobile to)
{
    if (to.AccessLevel < FeatureFlagSettings.RequiredAccessLevel
        && FeatureFlagManager.IsItemUseBlocked(GetType(), out var reason))
    {
        to.SendMessage(0x22, reason);
        return;
    }
    base.DisplayTo(to);
}
```

### FeatureFlagBlockEntry Types

```csharp
public sealed class ItemBlockEntry : FeatureFlagBlockEntry
{
    public bool BlockUse { get; set; }
    public bool BlockEquip { get; set; }
    public bool BlockContainerAccess { get; set; }    // Key for containers
}
```

### Default Messages

```csharp
const string DefaultContainerBlockedMessage = "This container cannot be opened at this time.";
```

---

## Key Methods Summary

| Method | Class | Purpose |
|--------|-------|---------|
| `CheckHold()` | Container | Validates if item can be stored (capacity/weight/deco checks) |
| `TryDropItem()` | Container | Drop item with auto-stacking |
| `TryDropItems()` | Container | Bulk drop with cumulative checking |
| `OnDragDropInto()` | Container | Handle drag-drop placement |
| `OnDoubleClick()` | Container | Open display to viewer |
| `DisplayTo()` | Container | Send container gump + contents to client |
| `ProcessOpeners()` | Container | Manage who can see contents (privacy) |
| `SendContentTo()` | Container | Send item list to client |
| `GetProperties()` | Container | Add content count/weight to OPL |
| `UpdateTotals()` | Container | Recalculate Gold/Items/Weight from scratch |
| `ConsumeTotal()` | Container | Consume items by type (multiple overloads) |
| `ConsumeTotalGrouped()` | Container | Consume grouped/stacked items |
| `GetAmount()` | Container | Count items of type (recursive) |
| `FindItemByType<T>()` | Container | Find first item of type (recursive BFS) |
| `Destroy()` | Container | Eject all items, then delete self |
| `DropItem()` | Container | Add item with random positioning within bounds |
| `OnStackAttempt()` | Container | Handle stacking when dropping |
| `ExecuteTrap()` | TrappableContainer | Fire trap effects and reset |
| `LockPick()` | LockableContainer | Unlock the container |
| `Mark()` | MarkContainer | Set recall rune target |
| `Fill()` | ParagonChest/TreasureMapChest | Generate loot contents |
| `Respawn()` | FillableContainer | Regenerate contents on timer |

---

## Cross-References

- [`../systems/combat.md`](../systems/combat.md) — trappable container damage mechanics
- [`../systems/crafting.md`](../systems/crafting.md) — salvage mechanics via SalvageBag
- [`../systems/housing.md`](../systems/housing.md) — lockdown, secure storage, secure items
- [`../systems/bulk-orders.md`](../systems/bulk-orders.md) — bulk order containers
- [`../getting-started/character-creation.md`](../getting-started/character-creation.md) — starting backpack
- [`../reference/skill-table.md`](../reference/skill-table.md) — Tinkering for lockpicking, Blacksmithing for salvage
