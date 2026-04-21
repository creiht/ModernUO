# Tools

Crafting tools are consumable implements used across ModernUO's 11 crafting and harvesting skills. Each tool has a quality level (Low/Regular/Exceptional), remaining uses, and durability system. Tools open their associated crafting gump on double-click and enforce a "one tool at a time" equipment restriction.

**Source Files:**
- `Projects/UOContent/Items/Skill Items/Tools/BaseTool.cs` (179 lines) — core mechanics
- `Projects/UOContent/Items/Skill Items/Tools/BaseRunicTool.cs` (1193 lines) — runic tool attribute system
- `Projects/UOContent/Items/Skill Items/Tools/SmithHammer.cs`, `Hammer.cs`, `SledgeHammer.cs`, `Saw.cs`, `DovetailSaw.cs`, `FletcherTools.cs`, `RunicFletcherTool.cs`, `SewingKit.cs`, `RunicSewingKit.cs`, `RunicHammer.cs`, `RunicDovetailSaw.cs`, `TinkerTools.cs`, `Tongs.cs`, `MalletAndChisel.cs`, `Pickaxe.cs`, `Blowpipe.cs`, `DrawKnife.cs`, `Froe.cs`, `Inshave.cs`, `JointingPlane.cs`, `MouldingPlane.cs`, `SmoothingPlane.cs`, `Nails.cs`, `RollingPin.cs`, `Skillet.cs`, `FlourSifter.cs`, `MapmakersPen.cs`, `ScribesPen.cs`, `MortarPestle.cs` — 29 concrete tool implementations
- `Projects/UOContent/Engines/Craft/DefBlacksmithy.cs`, `DefTinkering.cs`, `DefTailoring.cs`, `DefCarpentry.cs`, `DefBowFletching.cs`, `DefCooking.cs`, `DefInscription.cs`, `DefCartography.cs`, `DefMasonry.cs` — craft system definitions
- `Projects/UOContent/Items/Skill Items/Harvest Tools/BaseHarvestTool.cs` (228 lines) — parallel harvest tool subsystem
- `Projects/UOContent/Items/Skill Items/Harvest Tools/GargoylesPickaxe.cs`, `ProspectorsTool.cs`, `Shovel.cs`, `SturdyPickaxe.cs`, `SturdyShovel.cs` — harvest tool variants

---

## Overview

Tools in ModernUO serve as the implements through which crafting skills are exercised. Unlike other items, tools are both **craftable** (they implement `ICraftable`) and **used to craft** other items. They form a core bridge between the player and the crafting engine.

The tool system has two parallel hierarchies:
- **`BaseTool`** — crafting tools for blacksmithing, carpentry, tailoring, etc.
- **`BaseHarvestTool`** — harvesting tools for mining, lumberjacking, etc.

Both share the `IUsesRemaining` interface and quality system, but differ in their abstract properties (`CraftSystem` vs `HarvestSystem`) and double-click behavior.

---

## Core Mechanics

### Tool Quality

Tools have three quality levels that affect durability (remaining uses):

| Quality | Enum Value | Uses Scalar | Effect |
|---------|-----------|-------------|--------|
| Low | 0 | 100 | Standard durability |
| Regular | 1 | 100 | Standard durability |
| Exceptional | 2 | 200 | Double durability |

The quality scalar is applied when computing the visible uses:

```
Displayed UsesRemaining = _usesRemaining × GetUsesScalar() / 100
```

Where `GetUsesScalar()` returns 200 for Exceptional, 100 otherwise.

### Uses Remaining

Tools implement `IUsesRemaining`:

```csharp
public interface IUsesRemaining
{
    int UsesRemaining { get; set; }
    bool ShowUsesRemaining { get; set; }
}
```

- **`UsesRemaining`** — the effective/visible uses, computed from the stored base `_usesRemaining` multiplied by the quality scalar.
- **`ShowUsesRemaining`** — controls whether the uses count is displayed in properties (default: `true`).

The base `_usesRemaining` is stored persistently, while the displayed `UsesRemaining` is computed. This allows quality changes to preserve effective durability.

### Default Uses

| Tool Type | Default Uses |
|-----------|-------------|
| Regular tools (new) | Random 25-75 (via `BaseTool(int itemID)` constructor) |
| Regular tools (explicit) | Specified value (via `BaseTool(int uses, int itemID)`) |
| Harvest tools | 50 (via `BaseHarvestTool(int itemID, int usesRemaining = 50)`) |
| Ancient Smithy Hammer | 600 (special reward tool) |

### Uses Scaling

When quality changes, the tool unscales the current effective uses back to base, changes quality, then rescales:

```csharp
// Quality setter flow:
UnscaleUses()     // UsesRemaining = _usesRemaining × 100 / GetUsesScalar()
_quality = value  // Change enum
ScaleUses()       // UsesRemaining = _usesRemaining × GetUsesScalar() / 100
MarkDirty()       // Persist the change
```

### Break on Depletion

The `BreakOnDepletion` property (virtual, default `true`) determines whether the tool is destroyed when uses reach zero. Subclasses can override to `false` for tools that degrade but don't break.

### OnCraft Bonus

When a tool is crafted (the `OnCraft` callback), bonus uses are applied:

| Era | Quality | Bonus |
|-----|---------|-------|
| UOR | Exceptional | Fixed 100 uses |
| Post-UOR | Regular | No bonus |
| Post-UOR | Non-regular | `UsesRemaining += (int)(UsesRemaining × ((int)Quality - 1) × 0.2)` |

For post-UOR eras, the bonus is 20% per quality level above Regular.

---

## BaseTool Class Structure

### Serialization Fields

| Field | Type | Field # | Description |
|-------|------|---------|-------------|
| `_crafter` | `string` | 0 | Maker's mark name (stored asynchronously from Mobile.RawName) |
| `_quality` | `ToolQuality` | 1 | Quality level |
| `_usesRemaining` | `int` | 2 | Base (unscaled) durability |

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Quality` | `ToolQuality` | Get/Set with scaling on change |
| `BreakOnDepletion` | `bool` | Virtual, default `true`, overridable |
| `CraftSystem` | `CraftSystem` | Abstract — each concrete tool overrides |
| `UsesRemaining` | `int` | Computed via `IUsesRemaining` |
| `ShowUsesRemaining` | `bool` | Display toggle, default `true` |

### Key Methods

| Method | Description |
|--------|-------------|
| `ScaleUses()` | Applies quality scalar to visible uses, invalidates properties |
| `UnscaleUses()` | Reverses scaling (used when quality changes) |
| `GetUsesScalar()` | Returns 200 for Exceptional, 100 otherwise |
| `OnCraft(quality, makersMark, from, craftSystem, typeRes, tool, craftItem, resHue)` | Called when tool is crafted — sets quality, maker's mark, adjusts uses |
| `GetProperties(IPropertyList list)` | Adds "exceptional" label (if exceptional) and "uses remaining: N" |
| `DisplayDurabilityTo(Mobile m)` | Appends "Durability: N" to item name |
| `CheckAccessible(Item tool, Mobile m)` | Static — checks if tool is in backpack or equipped |
| `CheckTool(Item tool, Mobile m)` | Static — validates no conflicting tool equipped |
| `OnDoubleClick(Mobile from)` | Opens CraftGump for this tool's CraftSystem |

### CheckTool() — One Tool at a Time

The `CheckTool()` static method enforces that only one tool can be equipped at a time:

1. **OneHanded layer:** If a `BaseTool` is equipped there that is NOT the target tool AND is NOT an `AncientSmithyHammer`, returns `false`.
2. **TwoHanded layer:** If a `BaseTool` is equipped there that is NOT the target tool AND is NOT an `AncientSmithyHammer`, returns `false`.
3. **Special case:** `AncientSmithyHammer` is always exempt — it can coexist with any regular tool because it provides a blacksmithing bonus as a reward item.

```csharp
public static bool CheckTool(Item tool, Mobile m)
{
    // Check OneHanded layer for conflicting tools
    Item oneHanded = m.Items[m.FindItemOn(Layer.OneHanded)];
    if (oneHanded is BaseTool bt && bt != tool && !(bt is AncientSmithyHammer))
        return false;

    // Check TwoHanded layer for conflicting tools
    Item twoHanded = m.Items[m.FindItemOn(Layer.TwoHanded)];
    if (twoHanded is BaseTool bt2 && bt2 != tool && !(bt2 is AncientSmithyHammer))
        return false;

    return true;
}
```

---

## Tool Categories

Tools are organized by the crafting skill they serve:

### Blacksmithing Tools

| Tool | ItemID | Layer | Weight | CraftSystem |
|------|--------|-------|--------|-------------|
| `SmithHammer` | 0x13E3 | OneHanded | 8.0 | DefBlacksmithy |
| `RunicHammer` | 0x13E3 | OneHanded | 8.0 | DefBlacksmithy |
| `SledgeHammer` | — | — | — | DefBlacksmithy |
| `Tongs` | — | — | — | DefBlacksmithy |

**Environment requirement:** Must be near both an **anvil** (IDs: 4015, 4016, 11733, 11734, or with `AnvilAttribute`) AND a **forge** (IDs: 4017, 6522-6569, 11736, or with `ForgeAttribute`) within range 2.

### Carpentry Tools

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `Hammer` | 0x102A | 2.0 | DefCarpentry |
| `Saw` | 0x1034 | 2.0 | DefCarpentry |
| `DovetailSaw` | 0x1028 | — | DefCarpentry |
| `RunicDovetailSaw` | 0x1028 | 2.0 | DefCarpentry |

**Materials:** Log, OakLog, AshLog, YewLog, HeartwoodLog, BloodwoodLog, FrostwoodLog

### Tailoring Tools

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `SewingKit` | 0xF9D | 2.0 | DefTailoring |
| `RunicSewingKit` | 0xF9D | 2.0 | DefTailoring |

**Materials:** Cloth, UncutCloth, Leather, SpinedLeather, HornedLeather, BarbedLeather

### Tinkering Tools

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `TinkerTools` | 0x1EB8 | 1.0 | DefTinkering |
| `TinkersTools` (alias) | 0x1EBC | 1.0 | DefTinkering |
| `Blowpipe` | — | — | DefTinkering |
| `DrawKnife` | — | — | DefTinkering |
| `Froe` | — | — | DefTinkering |
| `Inshave` | — | — | DefTinkering |
| `JointingPlane` | — | — | DefTinkering |
| `MouldingPlane` | — | — | DefTinkering |
| `SmoothingPlane` | — | — | DefTinkering |
| `Nails` | — | — | DefTinkering |
| `MortarPestle` | — | — | DefTinkering |

**Note:** Many of these tools are themselves crafted items within the Tinkering skill.

### Bowfletching Tools

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `FletcherTools` | 0x1022 | 2.0 | DefBowFletching |
| `RunicFletcherTool` | 0x1022 | 2.0 | DefBowFletching |

**Materials:** Log variants, Shafts, Feathers

### Cooking Tools

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `RollingPin` | — | — | DefCooking |
| `Skillet` | — | — | DefCooking |
| `FlourSifter` | — | — | DefCooking |

### Inscription Tool

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `ScribesPen` | 0x0FBF | 1.0 | DefInscription |

**Special requirement:** Player must already know the spell to inscribe it. LabelNumber: 1044168 ("scribe's pen").

### Cartography Tool

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `MapmakersPen` | — | — | DefCartography |

### Masonry Tool

| Tool | ItemID | Weight | CraftSystem |
|------|--------|--------|-------------|
| `MalletAndChisel` | 0x12B3 | 1.0 | DefMasonry |

**Special requirement:** Requires `PlayerMobile.Masonry` flag AND `Carpentry >= 100.0`. Uses Carpentry skill, not a dedicated Masonry skill.

### Harvest Tools (BaseHarvestTool)

Harvest tools form a parallel subsystem for mining and lumberjacking:

| Tool | CraftSystem | Notes |
|------|-------------|-------|
| `Pickaxe` | DefMining (Harvest) | Standard mining tool |
| `SturdyPickaxe` | DefMining (Harvest) | Enhanced mining |
| `GargoylesPickaxe` | DefMining (Harvest) | Gargoyle-specific variant |
| `Shovel` | DefLumberjacking (Harvest) | Standard lumberjacking |
| `SturdyShovel` | DefLumberjacking (Harvest) | Enhanced lumberjacking |
| `ProspectorsTool` | DefLumberjacking (Harvest) | Prospecting variant |

**Key differences from BaseTool:**
- Abstract property is `HarvestSystem` instead of `CraftSystem`
- `OnDoubleClick` calls `HarvestSystem.BeginHarvesting(from, this)` instead of opening CraftGump
- `GetContextMenuEntries` adds mining stone toggle options
- `OnCraft` does NOT scale uses for exceptional quality

---

## Runic Tools

Runic tools are enhanced craft tools made from specific craft resources that apply random magical properties when used. They extend `BaseTool` and add a rich attribute application system.

### BaseRunicTool Class

```csharp
public abstract class BaseRunicTool : BaseTool
```

**Serialization Fields:**

| Field | Type | Field # | Description |
|-------|------|---------|-------------|
| `_resource` | `CraftResource` | 0 | The craft resource this tool is made from |

### Runic Tool Properties

| Property | Type | Description |
|----------|------|-------------|
| `Resource` | `CraftResource` | The craft resource; setting updates Hue via `CraftResources.GetHue()` |

### Attribute Application System

Runic tools apply up to 32 random properties when used on weapons, armor, hats, jewelry, or spellbooks. The system uses a central `Scale()` method for randomization:

```csharp
Scale(int min, int max, int low, int high)
```

**Randomization behavior:**
- If called from an actual runic tool (`m_IsRunicTool == true`): uses `Utility.RandomMinMax(min, max)` (uniform distribution).
- If called statically (attribute application): uses a sqrt-based bell-curve distribution favoring middle values: `v = sqrt(random(0..10000))`, `v = 100 - v`, with +10 luck bonus chance.

### Weapon Attribute Slots (25 slots)

| Slot | Possible Attributes | Range |
|------|---------------------|-------|
| 0 | HitArea (Physical/Fire/Cold/Poison/Energy) | 2-50 |
| 1 | HitMagic (MagicArrow/Harm/Fireball/Lightning) | 2-50 |
| 2 | UseBestSkill(1,1) or MageWeapon | 1-10 |
| 3 | WeaponDamage | 1-50 |
| 4 | DefendChance | 1-15 |
| 5 | CastSpeed | 1,1 |
| 6 | AttackChance | 1-15 |
| 7 | Luck | 1-100 |
| 8 | WeaponSpeed | 5-30 (scale 5) |
| 9 | SpellChanneling | 1,1 |
| 10 | HitDispel | 2-50 |
| 11 | HitLeechHits | 2-50 |
| 12 | HitLowerAttack | 2-50 |
| 13 | HitLowerDefend | 2-50 |
| 14 | HitLeechMana | 2-50 |
| 15 | HitLeechStam | 2-50 |
| 16 | LowerStatReq | 10-100 (scale 10) |
| 17 | ResistPhysicalBonus | 1-15 |
| 18 | ResistFireBonus | 1-15 |
| 19 | ResistColdBonus | 1-15 |
| 20 | ResistPoisonBonus | 1-15 |
| 21 | ResistEnergyBonus | 1-15 |
| 22 | DurabilityBonus | 10-100 (scale 10) |
| 23 | Slayer | Random (via GetRandomSlayer()) |
| 24 | ElementalDamages (Cold/Energy/Fire/Poison) | Proportional to physical dmg % |

**Ranged weapon exclusions:** Cannot get `UnbrokenWeaponSwing` (slot 2) or `MageWeapon`.

### Armor Attribute Slots (20 slots, offset 4; shields use 7 slots, offset 0)

| Slot | Possible Attributes | Range |
|------|---------------------|-------|
| 0 | SpellChanneling | 1,1 (shields only) |
| 1 | DefendChance | 1-15 (shields only) |
| 2 | ReflectPhysical/AttackChance | 1-15 (shields only) |
| 3 | CastSpeed | 1,1 (shields only) |
| 4 | LowerStatReq | 10-100 |
| 5 | SelfRepair | 1-5 |
| 6 | DurabilityBonus | 10-100 |
| 7 | MageArmor | 1,1 |
| 8 | RegenHits | 1-2 |
| 9 | RegenStam | 1-3 |
| 10 | RegenMana | 1-2 |
| 11 | NightSight | 1,1 |
| 12 | BonusHits | 1-5 |
| 13 | BonusStam | 1-8 |
| 14 | BonusMana | 1-8 |
| 15 | LowerManaCost | 1-8 |
| 16 | LowerRegCost | 1-20 |
| 17 | Luck | 1-100 |
| 18 | ReflectPhysical | 1-15 |
| 19-23 | Resist bonuses (Physical/Fire/Cold/Poison/Energy) | 1-15 each |

**Exclusions:**
- Self-repair armor: cannot get LowerStatReq (slot 0)
- Leather armor: cannot get LowerStatReq or DurabilityBonus (slots 0, 2)
- Elf-only armor: cannot get NightSight (slot 7)
- Meditation-allowing armor: cannot get MageArmor (slot 3)

### Hat Attribute Slots (19 slots)

Includes resistances as element attributes.

### Jewel Attribute Slots (24 slots)

Includes 5 skill bonus slots (indices 0-4) with random skill assignment from 24 possible skills.

### Spellbook Attribute Slots (16 slots)

Slots 0-3 all grant BonusInt (1-8) and mark slots 0-3 as used.

### Static Utility Methods

| Method | Description |
|--------|-------------|
| `GetUniqueRandom(int count)` | Returns random unused index from [0, count), marks used. Returns -1 if all used. |
| `GetRandomSlayer()` | 10% chance of super slayer (excluding fey), otherwise minor slayer (excluding fey/undead) |
| `GetElementalDamages(BaseWeapon weapon, bool randomizeOrder)` | Distributes elemental damage (Cold/Energy/Fire/Poison) across a weapon based on its physical damage percentage |

### Static Fields

| Field | Type | Description |
|-------|------|-------------|
| `MaxProperties` | `int` | 32 — maximum random properties a runic tool can apply |
| `m_IsRunicTool` | `static bool` | Flag indicating if current attribute application is from a runic tool |
| `m_LuckChance` | `static int` | Luck bonus for attribute intensity |
| `m_PossibleBonusSkills` | `SkillName[]` | 24 skills that can receive bonus from jewelry/hats/spellbooks |
| `m_PossibleSpellbookSkills` | `SkillName[]` | 4 skills for spellbooks: Magery, Meditation, EvalInt, MagicResist |
| `m_Props` | `BitArray<32>` | Tracks which property slots have been used |
| `m_Possible` | `int[32]` | Working array for available property indices |

---

## Crafting Integration

### ICraftable Interface

Tools implement `ICraftable`, which defines:

```csharp
public interface ICraftable
{
    int OnCraft(
        int quality, bool makersMark, Mobile from, CraftSystem craftSystem,
        Type typeRes, BaseTool tool, CraftItem craftItem, int resHue
    );
}
```

### CraftSystem Mapping

Each concrete tool overrides the abstract `CraftSystem` property to return its associated craft system:

```csharp
public override CraftSystem CraftSystem => DefBlacksmithy.CraftSystem;
```

### Double-Click Behavior

When a tool is double-clicked:
1. Checks if tool is in the mobile's backpack or equipped.
2. Gets the `CraftSystem` from the override property.
3. Calls `system.CanCraft(from, this, null)` to check if crafting conditions are met.
4. If there is an error message (num > 0) AND it is NOT the anvil/forge proximity message (1044267) OR the era is not SE, displays the localized error.
5. Otherwise, sends a `CraftGump` for the associated craft system.

**Note:** Blacksmithing shows the gump regardless of anvil/forge proximity after SE expansion.

### Maker's Mark

When crafted with `makersMark = true`, the crafter's raw name is stored in `_crafter`. The code comment notes "Makers mark not displayed on OSI" — it is stored for completeness but intentionally hidden from display.

---

## Properties Display

When a tool's properties are read (`GetProperties`), the following are added:

| Condition | Label Key | Display |
|-----------|-----------|---------|
| Quality == Exceptional | 1060636 | "exceptional" |
| Always | 1060584 | "uses remaining: X" |

The `DisplayDurabilityTo()` method appends "Durability: {_usesRemaining}" to the item's name using label key 1017323.

---

## Tool-to-CraftSystem Reference Table

| Tool Class | CraftSystem | MainSkill | Tool Environment |
|-----------|-------------|-----------|-----------------|
| `SmithHammer` | DefBlacksmithy | Blacksmith | Anvil + Forge |
| `RunicHammer` | DefBlacksmithy | Blacksmith | Anvil + Forge |
| `SledgeHammer` | DefBlacksmithy | Blacksmith | Anvil + Forge |
| `Tongs` | DefBlacksmithy | Blacksmith | Anvil + Forge |
| `Hammer` | DefCarpentry | Carpentry | None |
| `Saw` | DefCarpentry | Carpentry | None |
| `DovetailSaw` | DefCarpentry | Carpentry | None |
| `RunicDovetailSaw` | DefCarpentry | Carpentry | None |
| `FletcherTools` | DefBowFletching | Fletching | None |
| `RunicFletcherTool` | DefBowFletching | Fletching | None |
| `SewingKit` | DefTailoring | Tailoring | None |
| `RunicSewingKit` | DefTailoring | Tailoring | None |
| `TinkerTools` / `TinkersTools` | DefTinkering | Tinkering | None |
| `MalletAndChisel` | DefMasonry | Carpentry (w/ Masonry flag) | None |
| `ScribesPen` | DefInscription | Inscribe | None |
| `MapmakersPen` | DefCartography | Cartography | None |
| `RollingPin` | DefCooking | Cooking | None |
| `Skillet` | DefCooking | Cooking | None |
| `FlourSifter` | DefCooking | Cooking | None |
| `TinkerTools` (crafted items) | DefTinkering | Tinkering | None |
| `Pickaxe` | DefMining (Harvest) | Mining | — |
| `SturdyPickaxe` | DefMining (Harvest) | Mining | — |
| `GargoylesPickaxe` | DefMining (Harvest) | Mining | — |
| `Shovel` | DefLumberjacking (Harvest) | Lumberjacking | — |
| `SturdyShovel` | DefLumberjacking (Harvest) | Lumberjacking | — |
| `ProspectorsTool` | DefLumberjacking (Harvest) | Lumberjacking | — |

---

## Cross-References

- `systems/crafting.md` — 11 craft definitions and tool requirements
- `systems/harvesting.md` — mining/lumberjacking tools
- `reference/skill-table.md` — skill-to-tool associations
- `reference/craft-resources.md` — craft resource definitions (used by runic tools)
