# Crafting

Crafting is the system that allows players to create items from raw resources through 11 distinct craft skills. Each craft skill has its own definition file with recipe lists, resource requirements, skill ranges, and expansion-gated items. The system supports success/failure with quality levels, exceptional items, maker's marks, resource mutation (e.g., ore types), tool durability, and multiple supporting features like repair, resmelt, and enhancement.

**Source Files:**
- `Projects/UOContent/Engines/Craft/Core/CraftSystem.cs` (344 lines) — abstract core engine, registration API
- `Projects/UOContent/Engines/Craft/Core/CraftItem.cs` (1422 lines) — recipe definitions, success/exceptional chance, resource consumption
- `Projects/UOContent/Engines/Craft/DefBlacksmithy.cs` (709 lines) — largest craft definition
- `Projects/UOContent/Engines/Craft/DefTailoring.cs` (671 lines)
- `Projects/UOContent/Engines/Craft/DefInscription.cs` (537 lines)
- `Projects/UOContent/Engines/Craft/DefCooking.cs` (437 lines)
- `Projects/UOContent/Engines/Craft/DefBowFletching.cs` (391 lines)
- `Projects/UOContent/Engines/Craft/DefCarpentry.cs` (343 lines)
- `Projects/UOContent/Engines/Craft/DefAlchemy.cs` (301 lines)
- `Projects/UOContent/Engines/Craft/DefTinkering.cs` (291 lines)
- `Projects/UOContent/Engines/Craft/DefGlassblowing.cs` (281 lines)
- `Projects/UOContent/Engines/Craft/DefCartography.cs` (217 lines)
- `Projects/UOContent/Engines/Craft/DefMasonry.cs` (179 lines)
- `Projects/UOContent/Engines/Craft/Core/Repair.cs` (500 lines) — tool repair functionality
- `Projects/UOContent/Engines/Craft/Core/Resmelt.cs` — ore resmelting
- `Projects/UOContent/Engines/Craft/Core/Enhance.cs` (443 lines) — item enhancement
- `Projects/UOContent/Engines/Craft/Core/Recipes.cs` — recipe scroll learning system
- `Projects/UOContent/Engines/Craft/Core/CustomCraft.cs` — complex crafting (runebooks, map making)
- `Projects/UOContent/Engines/Craft/Core/CraftGump.cs` (735 lines) — crafting UI
- `Projects/UOContent/Engines/Craft/Core/QueryMakersMarkGump.cs` — maker's mark confirmation UI
- `Projects/UOContent/Engines/Craft/Core/CraftContext.cs` — per-player craft context (last resource, mark option)
- `Projects/UOContent/Engines/Craft/Core/CraftGroup.cs` — category grouping in UI
- `Projects/UOContent/Engines/Craft/Core/CraftRes.cs` (41 lines) — resource definition
- `Projects/UOContent/Engines/Craft/Core/CraftSkill.cs` (18 lines) — skill requirement definition
- `Projects/UOContent/Engines/Craft/Core/CraftSubRes.cs` (36 lines) — resource mutation types
- `Projects/UOContent/Engines/Craft/Core/CraftSubResCol.cs` — collection of sub-resources
- `Projects/UOContent/Engines/Craft/Core/CraftItemIDAttribute.cs` — item ID lookup attribute

---

## Core Engine (`CraftSystem`)

The `CraftSystem` abstract class is the base for all 11 crafting definitions. Each definition specifies its main skill, ECA type, animation parameters, and calls `InitCraftList()` to register recipes.

### Constructor Parameters

```csharp
new CraftSystem(int minCraftEffect, int maxCraftEffect, double delay)
```

| Parameter | Description | Typical Value |
|-----------|-------------|---------------|
| `minCraftEffect` | Minimum swing animation count | 1 |
| `maxCraftEffect` | Maximum swing animation count | 1 |
| `delay` | Seconds between swings (tick interval) | 1.25 |

### Key Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `MainSkill` | `SkillName` (abstract) | — | Primary skill for this craft |
| `GumpTitle` | `TextDefinition` | Empty | Gump title CLS number |
| `ECA` | `CraftECA` | `ChanceMinusSixty` | Error chance adjustment type |
| `Resmelt` | `bool` | `false` | Enable resmelting option in gump |
| `Repair` | `bool` | `false` | Enable repair option in gump |
| `MarkOption` | `bool` | `false` | Enable maker's mark option |
| `CanEnhance` | `bool` | `false` | Enable enhancement option |
| `CraftItems` | `List<CraftItem>` | — | All registered craftable items |
| `CraftGroups` | `List<CraftGroup>` | — | UI category groups |
| `CraftSubRes` | `CraftSubResCol` | — | Primary resource mutation (e.g., ore types) |
| `CraftSubRes2` | `CraftSubResCol` | — | Secondary resource mutation |

### Abstract Methods (per-definition override)

| Method | Return | Description |
|--------|--------|-------------|
| `GetChanceAtMin(CraftItem item)` | `double` | Base success chance at minimum skill |
| `InitCraftList()` | `void` | Register all recipes via `AddCraft()` etc. |
| `PlayCraftEffect(Mobile from)` | `void` | Play swing animation/sound |
| `PlayEndingEffect(...)` | `int` | Return CLS message number for result |
| `CanCraft(Mobile from, BaseTool tool, Type itemType)` | `int` | CLS error message (0 = can craft) |

### Registration API

The `CraftSystem` base class provides fluent methods for registering recipes:

| Method | Purpose |
|--------|---------|
| `AddCraft(Type, TextDefinition group, TextDefinition name, SkillName, minSkill, maxSkill, Type resType, TextDefinition nameRes, int amount)` | Simple single-resource recipe |
| `AddCraft(Type, TextDefinition group, TextDefinition name, int itemId, ...)` | Recipe with explicit ItemID |
| `AddRes(int index, Type type, TextDefinition name, int amount, TextDefinition message)` | Add secondary resource to recipe at index |
| `SetItemHue(int index, int hue)` | Override default hue for crafted item |
| `SetManaReq(int index, int mana, int cliloc)` | Require mana to craft |
| `SetStamReq(int index, int stam, int cliloc)` | Require stamina to craft |
| `SetHitsReq(int index, int hits, int cliloc)` | Require hit points to craft |
| `SetUseAllRes(int index, bool useAll)` | Consume all available stackable resources |
| `SetNeedHeat(int index, bool needHeat)` | Require nearby heat source |
| `SetNeedOven(int index, bool needOven)` | Require nearby oven |
| `SetNeedMill(int index, bool needMill)` | Require nearby flour mill |
| `SetBeverageType(int index, BeverageType requiredBeverage)` | Require specific beverage type for quantity items |
| `SetNeededExpansion(int index, Expansion expansion)` | Gate recipe by expansion client |
| `ForceNonExceptional(int index)` | Disable exceptional quality for recipe |
| `AddRecipe(int index, int id)` | Link recipe scroll (learnable) |
| `AddRareRecipe(int index, int id)` | Link rare recipe scroll |
| `AddQuestRecipe(int index, int id)` | Link quest-only recipe |
| `AddSubRes(Type, TextDefinition name, double reqSkill, ...)` | Register sub-resource type |
| `SetSubRes(Type, TextDefinition name)` | Set primary sub-resource type |

### CraftContext (Per-Player State)

Tracks player preferences per craft system:
- `LastResourceIndex` — last selected resource mutation type
- `MarkOption` — `CraftMarkOption` enum (`MarkItem`, `DoNotMark`, `PromptForMark`)
- `OnMade(CraftItem item)` — tracks recently crafted items

---

## ECA Types (Error Chance Adjustment)

Three ECA models control how success chance is calculated. All affect both success and exceptional chance formulas.

### `CraftECA` Enum

| Value | Name | Behavior | Used By |
|-------|------|----------|---------|
| 0 | `ChanceMinusSixty` | `chance - 0.60` | Tailoring, Cooking, Inscription, BowFletching, Carpentry, Alchemy, Tinkering, Glassblowing, Cartography, Masonry |
| 1 | `FiftyPercentChanceMinusTenPercent` | `chance × 0.5 - 0.10` | None in current codebase |
| 2 | `ChanceMinusSixtyToFortyFive` | Scales above 95 skill: `chance - clamp(0.60 - (skill - 95) × 0.03, 0.45, 0.60)` | Blacksmithy, Cooking |

### ECA Formula Detail (`ChanceMinusSixtyToFortyFive`)

```
baseAdjustment = 0.60
skillAdjustment = (MainSkill - 95.0) × 0.03
finalAdjustment = clamp(skillAdjustment, 0.45, 0.60)
successChance = rawChance - finalAdjustment
```

At 95.0 skill: subtract 0.60
At 100.0 skill: subtract 0.45
At 105.0+ skill: subtract 0.45 (capped)

This means higher-skilled crafters have a better chance and higher exceptional chance.

---

## Success Formula

The success chance calculation happens in `CraftItem.GetSuccessChance()`:

```
minChance = craftSystem.GetChanceAtMin(item)

chance = minChance + (valMainSkill - minMainSkill) / (maxMainSkill - minMainSkill) × (1.0 - minChance)

// Talisman bonus (if applicable)
if (from has Talisman matching MainSkill)
    chance += talisman.SuccessBonus / 100.0

// Check: all required skills met? (any skill below minimum = 0% chance)
```

The linear interpolation scales from `minChance` at `minMainSkill` to 100% at `maxMainSkill`.

### Skill Check (Passive Gains)

When `gainSkills = true` (during actual crafting, not pre-check), the system calls `from.CheckSkill()` for **every skill** listed on the recipe (both main and secondary skills), using the recipe's min/max range.

---

## Exceptional Chance

Computed in `CraftItem.GetExceptionalChance()`:

```
exceptionalChance = chance  // starts as success chance

// Apply ECA modifier
exceptionalChance = ECA.Apply(exceptionalChance)

// Talisman bonus (if applicable)
if (from has Talisman matching MainSkill)
    exceptionalChance += talisman.ExceptionalBonus / 100.0

// ForceNonExceptional overrides to 0
if (item.ForceNonExceptional)
    exceptionalChance = 0
```

Exceptional items are determined by: `exceptionalChance > RandomDouble()`

Quality levels:
- **0** = Below average (barely crafted)
- **1** = Average (normal)
- **2** = Exceptional (best)

Maker's Mark is only available on exceptional items (quality 2) when the crafter has Base skill >= 100.0 on the main skill, and the item is markable.

---

## ConsumeType (Resource Consumption)

Three modes control how resources are consumed during crafting:

| Value | Behavior |
|-------|----------|
| `All` | Consume exact amount per recipe |
| `Half` | Consume half the amount (used when `UseAllRes` is true and checking before final craft) |
| `None` | Check availability without consuming (pre-flight check) |

### UseAllRes

When `UseAllRes = true` on a `CraftItem`, the crafter can craft multiple units at once from a stackable resource. The system calculates `maxAmount = totalStackable / amountPerUnit` and multiplies all resource amounts by `maxAmount`. Unbreakable tools multiply their `UsesRemaining` by `maxAmount` instead of setting item amount.

---

## 11 Craft Definitions

| System | Main Skill | ECA | Core File | Key Features |
|--------|-----------|-----|-----------|-------------|
| **Blacksmithing** | Blacksmith | `ChanceMinusSixtyToFortyFive` | DefBlacksmithy.cs | `Resmelt`, `Repair`, `MarkOption` — requires anvil + forge in range |
| **Tailoring** | Tailoring | `ChanceMinusSixtyToFortyFive` | DefTailoring.cs | `Repair`, `MarkOption` — `GetChanceAtMin = 0.5` |
| **Cooking** | Cooking | `ChanceMinusSixtyToFortyFive` | DefCooking.cs | `NeedHeat`, `NeedOven`, `NeedMill` — no tool required |
| **Inscription** | Magery | `ChanceMinusSixtyToFortyFive` | DefInscription.cs | `MarkOption` — uses Mana as resource, requires blank scroll |
| **BowFletching** | Bowcraft | `ChanceMinusSixtyToFortyFive` | DefBowFletching.cs | `Repair`, `MarkOption` |
| **Carpentry** | Carpentry | `ChanceMinusSixtyToFortyFive` | DefCarpentry.cs | `Repair`, `MarkOption` — requires saw in range |
| **Alchemy** | Alchemy | `ChanceMinusSixtyToFortyFive` | DefAlchemy.cs | `MarkOption` — potions/elixirs |
| **Tinkering** | Tinkering | `ChanceMinusSixtyToFortyFive` | DefTinkering.cs | `Repair`, `MarkOption` — golem repair, tools |
| **Glassblowing** | Glassblowing | `ChanceMinusSixtyToFortyFive` | DefGlassblowing.cs | Resource mutation (sand → various glass types) |
| **Cartography** | Cartography | `ChanceMinusSixtyToFortyFive` | DefCartography.cs | Special: indecipherable maps outside Felucca/Trammel |
| **Masonry** | Masonry | `ChanceMinusSixtyToFortyFive` | DefMasonry.cs | Stone/brick crafting for housing |

### Blacksmithy-Specific: Anvil & Forge Check

Blacksmithy requires both an anvil AND a forge within range 2 with line of sight:

```
Anvil IDs: 4015, 4016, 11733, 11734, or [AnvilAttribute]
Forge IDs: 4017, 6522-6569, 11736, or [ForgeAttribute]
```

Also checks tiles via `GetStaticAndMultiTiles()` for placed items.

### Tinkering-Specific: Golem Repair

Tinkering repair has special handling for Golems:
- Requires 60.0+ Tinkering
- Can repair up to `skill × 0.3 + 30` hits per attempt
- Consumes IronIngots at rate of 5 per hit point recovered
- 12-second cooldown via `BeginAction<Golem>()`

---

## Resource Mutation (Sub-Resources)

Some craft systems support resource mutation, where the primary resource type can be swapped based on what the player has. This is used for ore-based blacksmithing (Iron, DullCopper, ShadowIron, etc.) and glassblowing (various sand types).

### Registration

```csharp
SetSubRes(typeof(BaseOre), "Ore");
AddSubRes(typeof(IronIngot), 1044036, 0.0, 502925);
AddSubRes(typeof(DullCopperIngot), 1044039, 25.0, 502925);
// ... more sub-resources
```

### Skill Requirements

Each sub-resource has a `RequiredSkill` threshold. If the crafter's main skill is below this threshold, the resource is unavailable and an error message is shown. The resource mutation is checked in `CraftItem.ConsumeRes()`:

```csharp
if (baseType == resCol.ResType && typeRes != null)
{
    var subResource = resCol.SearchFor(baseType);
    if (subResource != null && from.Skills[craftSystem.MainSkill].Base < subResource.RequiredSkill)
    {
        message = subResource.Message;
        return false;
    }
}
```

---

## Hue Preservation

When crafting items from colored resources (e.g., dyed cloth, hued ore), the system can preserve the resource's hue on the crafted item.

### Hue Retention Logic (`CraftItem.RetainsColorFrom()`)

A crafted item retains hue from its resource if:
1. The item type is in `m_ColoredItemTable` (`BaseWeapon`, `BaseArmor`, `BaseClothing`, `BaseJewel`, `DragonBardingDeed`)
2. The resource type is in `m_ColoredResourceTable` (`BaseIngot`, `BaseOre`, `BaseLeather`, `BaseHides`, `UncutCloth`, `Cloth`, `BaseGranite`, `BaseScales`)
3. The item type is NOT in `m_NeverColorTable` (e.g., `OrcHelm`)
4. The craft system's `RetainsColorFrom()` override allows it (e.g., Tailoring has custom logic for specific deed types)

The hue is captured in `OnResourceConsumed()` when `amount >= m_ResAmount` (highest-priority resource consumed).

---

## Maker's Mark

Maker's Marks can be affixed to markable items when:
- Craft results in exceptional quality (quality 2)
- Crafter's main skill Base >= 100.0
- Item is markable (`CraftItem.IsMarkable()`)
- `craftSystem.MarkOption` is enabled

Markable item types:
- `BaseArmor`, `BaseWeapon`, `BaseClothing`, `BaseInstrument`
- `DragonBardingDeed`, `BaseTool`, `BaseHarvestTool`
- `FukiyaDarts`, `Shuriken`, `Spellbook`, `Runebook`, `BaseQuiver`

The `QueryMakersMarkGump` prompts the player to confirm. The `CraftContext.MarkOption` can be set to `MarkItem` (auto-mark), `DoNotMark` (never), or `PromptForMark`.

---

## Tool Requirements and Durability

### Tool Access

Most craft systems require tools to be:
1. Not deleted and have `UsesRemaining >= 0`
2. Accessible (on the player's person, not in a backpack) via `BaseTool.CheckAccessible()`
3. The correct tool if one is equipped via `BaseTool.CheckTool()`

### Tool Breakage

```csharp
tool.UsesRemaining--;
if (tool.UsesRemaining < 1 && tool.BreakOnDepletion)
{
    toolBroken = true;
    tool.Delete();
}
```

Blacksmithy has additional hammer durability: an `AncientSmithyHammer` on the OneHanded layer also loses a use per craft.

### Repair Weakening

Each repair reduces the item's `MaxHitPoints`:
- **AOS+**: always -1 MaxHitPer repair
- **Pre-AOS**: depends on skill level — >= 90 = -1, >= 70 = -2, else -3

If `MaxHitPoints <= toWeaken`, repair is blocked with "item has been repaired many times" message.

### Repair Difficulty

```
difficulty = (maxHits - curHits) × 1250 / maxHits - 250
skillCheck = CheckSkill(mainSkill, difficulty - 25.0, difficulty + 25.0)
```

### Repair Weaken Chance

```
weakenChance = 40 + (maxHits - curHits) - (skillLevel / 10)
if (weakenChance > Random(100))
    MaxHitPoints -= toWeaken;  // permanent damage
```

Higher damage and lower skill increase weaken chance.

---

## Resmelt

Resmelt converts ore items back to ingots. Enabled via `craftSystem.Resmelt = true` (used by Blacksmithy). The `Resmelt.cs` file provides the targeting and consumption logic.

---

## Enhancement

The `Enhance` system allows improving existing armor and weapons using craft resources. Enabled via `craftSystem.CanEnhance = true`.

### Enhancement Requirements

- Item must be `BaseArmor` or `BaseWeapon`
- Item must be in player's backpack
- Item must NOT be arcane equipment
- Resource must be a non-standard `CraftResource`
- Player must have sufficient skill for success chance > 0

### Enhancement Results (`EnhanceResult` enum)

| Value | Meaning |
|-------|---------|
| `None` | Error message sent |
| `NotInBackpack` | Item not in backpack |
| `BadItem` | Not armor/weapon or arcane |
| `BadResource` | Standard resource or unknown |
| `AlreadyEnhanced` | Item already enhanced |
| `Success` | Enhancement applied |
| `Failure` | Enhancement failed |
| `Broken` | Tool broke |
| `NoResources` | Insufficient resources |
| `NoSkill` | Insufficient skill |

---

## Recipe Scroll System

The `Recipes.cs` file implements the recipe scroll learning system. Players can learn recipes from scrolls, and crafted items can be linked to specific recipe IDs via `AddRecipe()`.

### Recipe Checking

```csharp
if (Recipe != null && (from as PlayerMobile)?.HasRecipe(Recipe) == false)
{
    // "You must learn that recipe from a scroll."
}
```

Recipes are checked before crafting begins in `CraftItem.Craft()`.

---

## Crafting Flow

### 1. Initiation (`CraftItem.Craft()`)

```
1. Verify not already crafting (BeginAction<CraftSystem>)
2. Check expansion requirement
3. Calculate success chance (pre-check, no skill gains)
4. Verify all required skills met
5. Check recipe scroll learned (if applicable)
6. Verify can craft (tool, location, etc.)
7. Check resource availability (ConsumeRes with ConsumeType.None)
8. Check attribute requirements (Mana/Hits/Stam)
9. Start InternalTimer
```

### 2. Crafting Animation (`InternalTimer`)

```
Tick (every Delay seconds):
  1. DisruptiveAction() — interrupt cast/channel
  2. PlayCraftEffect() — sound/animation
  3. If iCount < iCountMax: continue
  4. EndAction<CraftSystem>
  5. Re-verify CanCraft
  6. CheckSkills() — now with gainSkills=false (no passive gains)
  7. Determine quality and maker's mark eligibility
  8. If exceptional + markable + base >= 100: show QueryMakersMarkGump
  9. Call CompleteCraft()
```

### 3. Completion (`CraftItem.CompleteCraft()`)

```
1. Re-verify CanCraft
2. Consume resources (ConsumeType.All)
3. Consume attributes (Mana/Hits/Stam)
4. Decrease tool uses, check breakage
5. Create item instance
6. If ICraftable: call OnCraft() for quality override
7. Apply hue from resource
8. Set amount or usesRemaining for multi-craft
9. Add to backpack
10. Handle faction imbue query (if IFactionItem)
11. Show CraftGump with result message
```

### Failure Path

On failure during `CompleteCraft()`:
1. Resources are consumed (based on `ConsumeOnFailure` setting)
2. Tool uses are decreased
3. Tool may break
4. `PlayEndingEffect(failed: true, lostMaterial: true, ...)` returns message
5. CraftGump shows failure message

---

## CraftItem Properties Reference

| Property | Type | Description |
|----------|------|-------------|
| `ForceNonExceptional` | `bool` | Disable exceptional quality |
| `RequiredExpansion` | `Expansion` | Expansion client requirement |
| `Recipe` | `Recipe` | Linked recipe scroll |
| `RequiredBeverage` | `BeverageType` | Required beverage for quantity items |
| `Mana` | `int` | Mana required to craft |
| `Hits` | `int` | Hit points required to craft |
| `Stam` | `int` | Stamina required to craft |
| `UseSubRes2` | `bool` | Use secondary sub-resource pool |
| `UseAllRes` | `bool` | Craft max from available stackable resources |
| `NeedHeat` | `bool` | Require heat source nearby |
| `NeedOven` | `bool` | Require oven nearby |
| `NeedMill` | `bool` | Require flour mill nearby |
| `ItemType` | `Type` | Type of item to create |
| `ItemHue` | `int` | Override hue |
| `ItemID` | `int` | Resolved ItemID (cached) |
| `Resources` | `List<CraftRes>` | Resource requirements |
| `Skills` | `List<CraftSkill>` | Skill requirements |

### Heat Sources

```
0x461, 0x48E  (Sandstone oven/fireplace)
0x92B, 0x96C  (Stone oven/fireplace)
0xDE3, 0xDE9  (Campfire)
0xFAC         (Firepit)
0x184A-0x1850 (Heating stand)
0x398C, 0x399F (Fire field)
0x2DDB, 0x2DDC (Elven stove)
0x19AA, 0x19A9 (Veterant Reward Brazier)
0x197A, 0x19A9 (Large Forge)
0x0FB1         (Small Forge)
0x2DD8         (Elven Forge)
```

### Ovens

```
0x461, 0x46F  (Sandstone oven)
0x92B, 0x93F  (Stone oven)
0x2DDB, 0x2DDC (Elven stove)
```

### Mills

```
0x1920-0x1924, 0x1295, 0x1926, 0x1928,
0x192C-0x192E, 0x129F, 0x1930-0x1932, 0x1934
```

---

## Type Mapping Tables

### Resource-to-Item Type Pairs (`m_TypesTable`)

Used for quantity-based resource consumption (e.g., Log ↔ Board, Leather ↔ Hides):

| Index | Types |
|-------|-------|
| 0 | Log, Board |
| 1 | HeartwoodLog, HeartwoodBoard |
| 2 | BloodwoodLog, BloodwoodBoard |
| 3 | FrostwoodLog, FrostwoodBoard |
| 4 | OakLog, OakBoard |
| 5 | AshLog, AshBoard |
| 6 | YewLog, YewBoard |
| 7 | Leather, Hides |
| 8 | SpinedLeather, SpinedHides |
| 9 | HornedLeather, HornedHides |
| 10 | BarbedLeather, BarbedHides |
| 11 | BlankMap, BlankScroll |
| 12 | Cloth, UncutCloth |
| 13 | CheeseWheel, CheeseWedge |
| 14 | Pumpkin, SmallPumpkin |
| 15 | WoodenBowlOfPeas, PewterBowlOfPeas |

### Colored Item Types (`m_ColoredItemTable`)

Items that can inherit hue from resources:
- `BaseWeapon`, `BaseArmor`, `BaseClothing`, `BaseJewel`, `DragonBardingDeed`

### Colored Resource Types (`m_ColoredResourceTable`)

Resources whose hue can be preserved:
- `BaseIngot`, `BaseOre`, `BaseLeather`, `BaseHides`, `UncutCloth`, `Cloth`, `BaseGranite`, `BaseScales`

---

## CustomCraft

The `CustomCraft` base class handles complex crafting that can't be expressed through simple recipe definitions. Used for:
- Runebook creation (requires blank recall rune component)
- Map making (ind decipherable outside Felucca/Trammel)
- Other multi-step or conditional crafting

Custom craft items override `CompleteCraft()` to provide custom item creation logic via `CustomCraft.CompleteCraft()`.

---

## Cross-References

- `skills/crafting-skills.md` — skill requirements and training
- `items/tools.md` — tool types, durability, repair
- `reference/craft-resources.md` — ore/leather/wood resource types and bonuses
- `items/weapons.md` — crafted weapons, quality effects
- `items/armor.md` — crafted armor, quality effects
- `systems/combat.md` — item quality affects combat stats
- `systems/factions.md` — faction imbuing on crafted items
- `systems/veteran-rewards.md` — brazier as heat source for cooking
