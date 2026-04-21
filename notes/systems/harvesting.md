# Harvesting

Harvesting is the resource gathering system in Ultimate Online, encompassing three distinct harvesting activities: **Lumberjacking** (chopping trees for wood), **Mining** (extracting ore from rock veins), and **Fishing** (catching fish and aquatic treasures from water tiles). Each harvesting system uses a shared core engine (`HarvestSystem`) with type-specific definitions, resource tables, vein configurations, and regional bank tracking. The system supports skill-based success checks, resource veining with weighted probabilities, racial bonuses (Elves and Humans on Felucca), tool durability, and expansion-gated bonus resources.

**Source Files:**
- `Projects/UOContent/Engines/Harvest/Core/HarvestSystem.cs` (521 lines) — abstract core engine, success/failure flow
- `Projects/UOContent/Engines/Harvest/Core/HarvestDefinition.cs` (215 lines) — per-type configuration (tiles, banks, veining)
- `Projects/UOContent/Engines/Harvest/Core/HarvestResource.cs` (41 lines) — per-resource config (skill ranges, types)
- `Projects/UOContent/Engines/Harvest/Core/HarvestVein.cs` (24 lines) — vein configuration (weights, fallback)
- `Projects/UOContent/Engines/Harvest/Core/HarvestBank.cs` (100 lines) — per-location bank tracking
- `Projects/UOContent/Engines/Harvest/Core/HarvestTimer.cs` — swing animation timer
- `Projects/UOContent/Engines/Harvest/Core/HarvestSoundTimer.cs` — sound effect timer
- `Projects/UOContent/Engines/Harvest/Core/HarvestTarget.cs` (119 lines) — targeting handler
- `Projects/UOContent/Engines/Harvest/Core/BonusHarvestResource.cs` — bonus drop definitions
- `Projects/UOContent/Engines/Harvest/Core/FurnitureAttribute.cs` — furniture destruction attribute
- `Projects/UOContent/Engines/Harvest/Lumberjacking.cs` (195 lines) — wood harvesting
- `Projects/UOContent/Engines/Harvest/Mining.cs` (473 lines) — ore/stone/sand harvesting
- `Projects/UOContent/Engines/Harvest/Fishing.cs` (549 lines) — fishing and aquatic loot

---

## Core Engine (`HarvestSystem`)

The `HarvestSystem` abstract class defines the base harvesting workflow shared by all three harvesting activities. Each specific system (Lumberjacking, Mining, Fishing) extends this class and overrides behavior-specific methods.

### Key Methods (override points)

| Method | Purpose |
|--------|---------|
| `CheckTool(Mobile, Item)` | Validates tool is not worn out |
| `CheckHarvest(Mobile, Item)` | Additional pre-harvest validation (equipped axe, not mounted, etc.) |
| `CheckHarvest(Mobile, Item, HarvestDefinition, object)` | Definition-specific validation |
| `CheckRange(Mobile, Item, HarvestDefinition, Map, Point3D, bool)` | Distance validation |
| `CheckResources(Mobile, Item, HarvestDefinition, Map, Point3D, bool)` | Resource availability check |
| `OnBadHarvestTarget(Mobile, Item, object)` | Error messages for invalid targets |
| `GetLock(Mobile, Item, HarvestDefinition, object)` | Returns lock object for concurrent harvest prevention |
| `OnHarvestStarted(Mobile, Item, HarvestDefinition, object)` | Called when harvesting begins |
| `BeginHarvesting(Mobile, Item)` | Initiates targeting sequence |
| `FinishHarvesting(...)` | Core harvest execution (validation → veining → skill check → item creation) |
| `OnHarvestFinished(...)` | Post-harvest processing |
| `SpecialHarvest(Mobile, Item, HarvestDefinition, Map, Point3D)` | Override for special cases (quest interactions) |
| `Construct(Type, Mobile)` | Creates harvested item instance |
| `Give(Mobile, Item, bool)` | Delivers item to backpack or ground |
| `MutateVein(...)` | Modifies vein selection (e.g., Gargoyles Pickaxe advancement) |
| `MutateResource(...)` | Determines primary vs fallback resource based on skill/race |
| `GetResourceType(...)` | Selects specific item type from resource's type array |
| `DoHarvestingEffect(...)` | Plays swing animation and sound |
| `DoHarvestingSound(...)` | Plays harvest-specific sound |
| `SendSuccessTo(Mobile, Item, HarvestResource)` | Custom success message |
| `SendPackFullTo(Mobile, Item, HarvestDefinition, HarvestResource)` | Pack-full message |
| `StartHarvesting(Mobile, Item, object)` | Entry point — validation → timer start |

### Harvest Flow

```
1. Player targets a harvestable object
2. HarvestTarget.OnTarget() dispatches:
   a. Special cases (graves for Witch Apprentice quest, IChoppable, furniture destruction)
   b. Default: StartHarvesting()
3. StartHarvesting():
   a. CheckHarvest(tool)
   b. GetHarvestDetails() → tileID, map, location, isLand
   c. GetDefinition(tileID, isLand) → find matching HarvestDefinition
   d. CheckRange() → within MaxRange?
   e. CheckResources() → bank has resources?
   f. CheckHarvest(tool, def, toHarvest)
   g. BeginAction(locked)
   h. HarvestTimer starts → OnHarvesting() ticks → DoHarvestingEffect()
   i. HarvestSoundTimer plays sound → FinishHarvesting()
4. FinishHarvesting():
   a. Validate tileID/isLand against definition
   b. CheckRange() + CheckResources() (timed checks)
   c. MutateVein() → select vein from bank
   d. MutateResource() → primary or fallback based on skill + race
   e. Skill check: skillBase >= resource.ReqSkill && CheckSkill(def.Skill, resource.MinSkill, resource.MaxSkill)
   f. GetResourceType() → select specific item type
   g. MutateType() → region-based type mutation
   h. Construct() → create item instance
   i. Set amount (with Felucca racial bonus)
   j. Consume from bank
   k. Give() → backpack or ground
   l. Bonus resource check (if Core.ML + skill >= bonus.ReqSkill)
   m. Tool durability decrement
   n. OnHarvestFinished()
```

### Concurrent Harvest Lock

Each system returns its own instance as the lock object via `GetLock()`. This prevents a player from harvesting multiple locations simultaneously:

```csharp
// Lumberjacking: lock on the system singleton
public override object GetLock(...) => this;

// Mining: lock on the system singleton
public override object GetLock(...) => this;

// Fishing: lock on the system singleton
public override object GetLock(...) => this;
```

---

## HarvestDefinition

`HarvestDefinition` defines the configuration for a single harvesting activity (e.g., the ore-and-stone mining definition, the sand mining definition, the lumberjacking definition, or the fishing definition). It is referenced by all three harvesting systems.

### Configuration Properties

| Property | Type | Lumberjack | Mining (Ore) | Mining (Sand) | Fishing |
|----------|------|-----------|-------------|---------------|---------|
| `BankWidth` | `int` | 4 | 8 | 8 | 8 |
| `BankHeight` | `int` | 3 | 8 | 8 | 8 |
| `MinTotal` | `int` | 20 | 10 | 6 | 5 |
| `MaxTotal` | `int` | 45 | 34 | 12 | 15 |
| `MinRespawn` | `TimeSpan` | 20 min | 10 min | 10 min | 10 min |
| `MaxRespawn` | `TimeSpan` | 30 min | 20 min | 20 min | 20 min |
| `MaxRange` | `int` | 2 | 2 | 2 | 4 |
| `ConsumedPerHarvest` | `int` | 10 | 1 | 1 | 1 |
| `ConsumedPerFeluccaHarvest` | `int` | 20 | 2 | 1 | 1 |
| `RaceBonus` | `bool` | `Core.ML` | `Core.ML` | `Core.ML` | `Core.ML` |
| `RandomizeVeins` | `bool` | `Core.ML` | `Core.ML` | — | — |
| `RangedTiles` | `bool` | `false` | `true` | `true` | `true` |
| `Skill` | `SkillName` | `Lumberjacking` | `Mining` | `Mining` | `Fishing` |

### CLS Message IDs

| Message | Lumberjack | Mining | Fishing |
|---------|-----------|--------|---------|
| NoResources | `500493` — "There's not enough wood here to harvest." | `503040` — "There is no metal here to mine." / `1044629` — "There is no sand here to mine." | `503172` — "The fish don't seem to be biting here." |
| Fail | `500495` — "You hack at the tree for a while, but fail to produce any useable wood." | `503043` — "You loosen some rocks but fail to find any useable ore." / `1044630` — sand fail | `503171` — "You fish a while, but fail to catch anything." |
| OutOfRange | `500446` — "That is too far away." | `500446` — "That is too far away." | `500976` — "You need to be closer to the water to fish!" |
| TimedOutOfRange | — | `503041` — "You have moved too far away to continue mining." | `500976` — "You need to be closer to the water to fish!" |
| DoubleHarvest | — | `503042` — "Someone has gotten to the metal before you." | — |
| PackFull | `500497` — "You can't place any wood into your backpack!" | `1010481` — "Your backpack is full, so the ore you mined is lost." / `1044632` — sand pack full | `503176` — "You do not have room in your backpack for a fish." |
| ToolBroke | `500499` — "You broke your axe." | `1044038` — "You have worn out your tool!" | `503174` — "You broke your fishing pole." |

### Effect Properties

| Property | Lumberjack | Mining | Fishing |
|----------|-----------|--------|---------|
| `EffectActions` | `[13]` | `[11]` | `[12]` |
| `EffectSounds` | `[0x13E]` | `[0x125, 0x126]` | `[]` (empty) |
| `EffectCounts` | `Core.AOS ? [1] : [1,2,2,2,3]` | `[1]` | `[1]` |
| `EffectDelay` | 1.6s | 1.6s | 0s |
| `EffectSoundDelay` | 0.9s | 0.9s | 8.0s |

### Tile Validation

`Validate(tileID, isLand)` checks whether a tile is harvestable:

- If `RangedTiles = true`: tiles are stored as ranges `[start, end, start, end, ...]` and checked inclusively
- If `RangedTiles = false`: tiles are stored as individual IDs and checked for exact match

### Bank Location Calculation

Banks are keyed by grid position, where each bank covers `BankWidth × BankHeight` tiles:

```
gridX = loc.X / BankWidth
gridY = loc.Y / BankHeight
key = new Point2D(gridX, gridY)
```

### Vein Selection

`GetVeinAt(map, x, y)` selects the vein for a bank location:

1. If only one vein exists: return it directly
2. If `RandomizeVeins = true`: use `GetVeinFrom(Utility.Random(VeinWeights))`
3. Otherwise: deterministic selection using `StableRandom` with seed `(x * 17 + y * 11 + map.MapID * 3)`

Vein weights are accumulated cumulatively. Each vein's `VeinChance` is an upper bound in the cumulative sum.

---

## HarvestResource

`HarvestResource` defines a single resource type within a harvesting definition, specifying skill requirements and the item types that can be produced.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `ReqSkill` | `double` | Minimum skill to access this resource |
| `MinSkill` | `double` | Lower bound for skill check |
| `MaxSkill` | `double` | Upper bound for skill check |
| `Types` | `Type[]` | Array of possible item types (first = primary) |
| `SuccessMessage` | `TextDefinition` | CLS message number or string on success |

### Fallback Mechanic

When a player's skill is below `ReqSkill` or `MinSkill` for the primary resource, the system falls back to a secondary resource defined in the vein (`HarvestVein.FallbackResource`). This ensures lower-skilled players can still gather basic resources.

The fallback is also influenced by the `ChanceToFallback` property on the vein:

```
racialBonus = (def.RaceBonus && from.Race == Race.Elf) ? 0.20 : 0
if (vein.ChanceToFallback > Utility.RandomDouble() + racialBonus)
    return fallback;  // Elf bonus gives +20% chance to stay with primary
```

---

## HarvestVein

`HarvestVein` defines the vein configuration for a bank location, specifying resource priority and fallback behavior.

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `VeinChance` | `uint` | Weight for vein selection (higher = more likely) |
| `ChanceToFallback` | `double` | Probability of falling back to secondary resource (0.0-1.0) |
| `PrimaryResource` | `HarvestResource` | Main resource for this vein |
| `FallbackResource` | `HarvestResource` | Secondary resource when skill is too low or chance triggers |

---

## HarvestBank

`HarvestBank` tracks resource availability per bank location (grid cell). It manages current resource count, respawn timing, and vein assignment.

### Key Behavior

- **Initial state**: Random `m_Maximum` between `MinTotal` and `MaxTotal`, starting at full capacity
- **CheckRespawn()**: Called on every access. If `m_Current < m_Maximum` and `Core.Now >= m_NextRespawn`, resets to full
- **Consume(amount)**: Decrements current count. If at maximum, sets respawn timer:
  - Respawn time = `minRespawn + random × (maxRespawn - minRespawn)`
  - Elves reduce respawn time by 25%: `minutes *= 0.75`

---

## Resource Mutation

`MutateType()` allows regions to override the item type produced by harvesting. The default implementation calls `from.Region.GetResource(type)`, allowing custom regions to define different resource drops.

---

## Felucca Racial Bonus

On Felucca, humans have a 10% chance to harvest double resources when the bank has sufficient reserves:

```
// For Felucca harvestable stacks:
racialAmount = ceil(consumedPerHarvest × 1.1)
feluccaRacialAmount = ceil(consumedPerFeluccaHarvest × 1.1)

eligableForRacialBonus = (def.RaceBonus && from.Race == Race.Human)
inFelucca = (map == Map.Felucca)

// Priority order:
if (eligableForRacialBonus && inFelucca && bank.Current >= feluccaRacialAmount && random < 0.1)
    amount = feluccaRacialAmount
else if (inFelucca && bank.Current >= consumedPerFeluccaHarvest)
    amount = consumedPerFeluccaHarvest
else if (eligableForRacialBonus && bank.Current >= racialAmount && random < 0.1)
    amount = racialAmount
else
    amount = consumedPerHarvest
```

For Lumberjacking: `consumedPerHarvest = 10`, `consumedPerFeluccaHarvest = 20`
- Normal: 10 logs
- Felucca: 20 logs (or 22 with 10% human racial bonus)

For Mining: `consumedPerHarvest = 1`, `consumedPerFeluccaHarvest = 2`
- Normal: 1 ore
- Felucca: 2 ore (or 3 with 10% human racial bonus)

---

## Lumberjacking

Lumberjacking extracts wood from trees using an equipped axe. It is the simplest of the three harvesting systems in terms of resource variety, but supports additional interactions through interfaces.

### Definition

| Property | Value |
|----------|-------|
| `BankWidth × BankHeight` | 4 × 3 |
| `MinTotal / MaxTotal` | 20 / 45 |
| `MinRespawn / MaxRespawn` | 20 min / 30 min |
| `MaxRange` | 2 tiles |
| `ConsumedPerHarvest` | 10 |
| `ConsumedPerFeluccaHarvest` | 20 |
| `EffectActions` | `[13]` |
| `EffectSounds` | `[0x13E]` |

### Tree Tiles (~150 static tile IDs)

Tree tiles are stored in `m_TreeTiles[]` (sorted during `Initialize()`). The array includes:
- Standard tree IDs: `0x0CCA` through `0x0DFF` range
- UO:SE tree IDs: `0x12B5` through `0x12C7` range

Trees are **not** ranged tiles — each tile ID must match exactly.

### Resource Table

| Resource | ReqSkill | MinSkill | MaxSkill | Success Message | Item Type |
|----------|----------|----------|----------|----------------|-----------|
| Log | 0.0 | 0.0 | 100.0 | `1072540` (ML) / `500498` (pre-ML) | `Log` |
| OakLog | 65.0 | 25.0 | 105.0 | `1072541` | `OakLog` |
| AshLog | 80.0 | 40.0 | 120.0 | `1072542` | `AshLog` |
| YewLog | 95.0 | 55.0 | 135.0 | `1072543` | `YewLog` |
| HeartwoodLog | 100.0 | 60.0 | 140.0 | `1072544` | `HeartwoodLog` |
| BloodwoodLog | 100.0 | 60.0 | 140.0 | `1072545` | `BloodwoodLog` |
| FrostwoodLog | 100.0 | 60.0 | 140.0 | `1072546` | `FrostwoodLog` |

### Vein Weights

| Vein | VeinChance | ChanceToFallback | Primary | Fallback |
|------|-----------|-----------------|---------|----------|
| Ordinary Logs | 490 | 0.0 | Log | — |
| Oak | 300 | 0.5 | OakLog | Log |
| Ash | 100 | 0.5 | AshLog | Log |
| Yew | 50 | 0.5 | YewLog | Log |
| Heartwood | 30 | 0.5 | HeartwoodLog | Log |
| Bloodwood | 20 | 0.5 | BloodwoodLog | Log |
| Frostwood | 10 | 0.5 | FrostwoodLog | Log |

### Bonus Resources (ML+)

| Index | Chance | Message | Item Type |
|-------|--------|---------|-----------|
| 0 | 99.4% | — | Nothing |
| 1 | 10.0% | `1072548` | `BarkFragment` |
| 2 | 3.0% | `1072550` | `LuminescentFungi` |
| 3 | 2.0% | `1072547` | `SwitchItem` |
| 4 | 1.0% | `1072549` | `ParasiticPlant` |
| 5 | 0.1% | `1072551` | `BrilliantAmber` |

### Special Behaviors

**Axe must be equipped**: `CheckHarvest()` verifies `tool.Parent == from` (axe must be on the player's person, not in a backpack):

```csharp
if (tool.Parent != from)
{
    from.SendLocalizedMessage(500487); // The axe must be equipped for any serious wood chopping.
    return false;
}
```

**Target interactions**: `HarvestTarget.OnTarget()` supports multiple target types:
- `IChoppable` → calls `OnChop(from)`
- `IAxe` (item in backpack) → calls `Axe(from, axe)`
- `ICarvable` → calls `Carve(from, tool)`
- `FurnitureAttribute` → destroys furniture (with trap execution)
- `Mobile` → "You can only skin dead creatures."
- `Item` (not axe/furniture) → "Use this on corpses to carve away meat and hide"

**ML revealing action**: When `Core.ML` is enabled, `OnHarvestStarted()` calls `from.RevealingAction()`.

---

## Mining

Mining extracts ore from rock veins and sand from desert tiles. It supports two definitions (`OreAndStone` and `Sand`) within a single `Mining` system.

### Definition: Ore and Stone

| Property | Value |
|----------|-------|
| `BankWidth × BankHeight` | 8 × 8 |
| `MinTotal / MaxTotal` | 10 / 34 |
| `MinRespawn / MaxRespawn` | 10 min / 20 min |
| `MaxRange` | 2 tiles |
| `ConsumedPerHarvest` | 1 |
| `ConsumedPerFeluccaHarvest` | 2 |
| `RangedTiles` | `true` (tile ranges) |
| `EffectActions` | `[11]` |
| `EffectSounds` | `[0x125, 0x126]` |

### Definition: Sand

| Property | Value |
|----------|-------|
| `BankWidth × BankHeight` | 8 × 8 |
| `MinTotal / MaxTotal` | 6 / 12 |
| `MinRespawn / MaxRespawn` | 10 min / 20 min |
| `MaxRange` | 2 tiles |
| `ConsumedPerHarvest` | 1 |
| `ConsumedPerFeluccaHarvest` | 1 |
| `RangedTiles` | `true` (tile ranges) |
| `EffectActions` | `[11]` |
| `EffectCounts` | `[6]` (6 swings per harvest) |
| `Skill` | 100.0+ Mining required |

### Ore and Stone Resource Table

| Ore | ReqSkill | MinSkill | MaxSkill | Success Message | Item Types |
|-----|----------|----------|----------|----------------|------------|
| IronOre | 0.0 | 0.0 | 100.0 | `1007072` | `IronOre`, `Granite` |
| DullCopperOre | 65.0 | 25.0 | 105.0 | `1007073` | `DullCopperOre`, `DullCopperGranite`, `DullCopperElemental` |
| ShadowIronOre | 70.0 | 30.0 | 110.0 | `1007074` | `ShadowIronOre`, `ShadowIronGranite`, `ShadowIronElemental` |
| CopperOre | 75.0 | 35.0 | 115.0 | `1007075` | `CopperOre`, `CopperGranite`, `CopperElemental` |
| BronzeOre | 80.0 | 40.0 | 120.0 | `1007076` | `BronzeOre`, `BronzeGranite`, `BronzeElemental` |
| GoldOre | 85.0 | 45.0 | 125.0 | `1007077` | `GoldOre`, `GoldGranite`, `GoldenElemental` |
| AgapiteOre | 90.0 | 50.0 | 130.0 | `1007078` | `AgapiteOre`, `AgapiteGranite`, `AgapiteElemental` |
| VeriteOre | 95.0 | 55.0 | 135.0 | `1007079` | `VeriteOre`, `VeriteGranite`, `VeriteElemental` |
| ValoriteOre | 99.0 | 59.0 | 139.0 | `1007080` | `ValoriteOre`, `ValoriteGranite`, `ValoriteElemental` |

Each ore resource has up to 3 types: primary ore (index 0), granite (index 1), and elemental creature (index 2).

### Sand Resource Table

| Resource | ReqSkill | MinSkill | MaxSkill | Success Message | Item Type |
|----------|----------|----------|----------|----------------|-----------|
| Sand | 100.0 | 70.0 | 400.0 | `1044631` | `Sand` |

### Vein Weights

| Vein | VeinChance | ChanceToFallback | Primary | Fallback |
|------|-----------|-----------------|---------|----------|
| Iron | 496 | 0.0 | IronOre | — |
| Dull Copper | 112 | 0.5 | DullCopperOre | IronOre |
| Shadow Iron | 98 | 0.5 | ShadowIronOre | IronOre |
| Copper | 84 | 0.5 | CopperOre | IronOre |
| Bronze | 70 | 0.5 | BronzeOre | IronOre |
| Gold | 56 | 0.5 | GoldOre | IronOre |
| Agapite | 42 | 0.5 | AgapiteOre | IronOre |
| Verite | 28 | 0.5 | VeriteOre | IronOre |
| Valorite | 14 | 0.5 | ValoriteOre | IronOre |

### Mountain Cave Tiles

**Land tiles (ranges)**: `_mountainCaveTiles[]` — ~40 individual tile IDs in the 220-2105 range plus 0x3F39-0x3FCF range.

```
220, 231, 236, 247, 252, 268, 286, 297, 321, 324, 467, 474, 476, 487, 492, 495, 543, 579,
581, 621, 1741, 1757, 1771, 1790, 1801, 1824, 1831, 1854, 1861, 1884, 1981, 2004, 2028, 2033,
2100, 2105, 0x3F39, 0x3F74, 0x3F82, 0x3F8F, 0x3F91, 0x3FCF
```

**Static tiles**: `_mountainCaveStaticTiles[]` — `0x053B, 0x054F`

### Sand Tiles (ranges)

`_sandTiles[]` — ~30 tile IDs in the 22-1650 range.

### Special Behaviors

**Not mounted**: `CheckHarvest()` blocks mining while mounted:
```csharp
if (from.Mounted)
{
    from.SendLocalizedMessage(501864); // You can't mine while riding.
    return false;
}
```

**Not polymorphed**: Blocks mining while in non-human body mod form:
```csharp
if (from.IsBodyMod && !from.Body.IsHuman)
{
    from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
    return false;
}
```

**Sand mining requires 100+ skill + toggle**: `CheckHarvest(def=Sand)` requires:
```csharp
(from is PlayerMobile mobile && mobile.Skills.Mining.Base >= 100.0 && mobile.SandMining)
```

**Stone mining toggle**: `GetResourceType()` gives 10% chance to get granite instead of ore:
```csharp
if (from.Skills.Mining.Base >= 100.0 && from is PlayerMobile pm && pm.StoneMining && pm.ToggleMiningStone
    && Utility.RandomDouble() < 0.1)
{
    return resource.Types[1];  // Granite
}
return resource.Types[0];  // Ore
```

**Gargoyles Pickaxe vein advancement**: `MutateVein()` advances to the next vein tier when using a Gargoyles Pickaxe:
```csharp
if (tool is GargoylesPickaxe && def == OreAndStone)
{
    var veinIndex = Array.IndexOf(def.Veins, vein);
    if (veinIndex >= 0 && veinIndex < def.Veins.Length - 1)
        return def.Veins[veinIndex + 1];  // Next higher tier
}
```

**Gargoyles Pickaxe elemental spawn**: `OnHarvestFinished()` gives 10% chance to spawn an elemental creature when mining with Gargoyles Pickaxe:
```csharp
if (tool is GargoylesPickaxe && def == OreAndStone && Utility.RandomDouble() < 0.1)
{
    // Spawn res.Types[2] (elemental) near miner
}
```

**Success message for granite**: `SendSuccessTo()` sends custom message for granite:
```csharp
if (item is BaseGranite)
    from.SendLocalizedMessage(1044606); // You carefully extract some workable stone from the ore vein!
```

**ML revealing action**: `OnHarvestStarted()` and `OnHarvestFinished()` call `from.RevealingAction()` when `Core.ML`.

### Bonus Resources (ML+)

| Index | Chance | Message | Item Type |
|-------|--------|---------|-----------|
| 0 | 99.4% | — | Nothing |
| 1 | 0.1% | `1072562` | `BlueDiamond` |
| 2 | 0.1% | `1072567` | `DarkSapphire` |
| 3 | 0.1% | `1072570` | `EcruCitrine` |
| 4 | 0.1% | `1072564` | `FireRuby` |
| 5 | 0.1% | `1072566` | `PerfectEmerald` |
| 6 | 0.1% | `1072568` | `Turquoise` |

---

## Fishing

Fishing catches fish and treasures from water tiles. It has the most complex special behavior, including quest integrations, underwater serpents, and a mutate table for skill-dependent special catches.

### Definition

| Property | Value |
|----------|-------|
| `BankWidth × BankHeight` | 8 × 8 |
| `MinTotal / MaxTotal` | 5 / 15 |
| `MinRespawn / MaxRespawn` | 10 min / 20 min |
| `MaxRange` | 4 tiles |
| `ConsumedPerHarvest` | 1 |
| `ConsumedPerFeluccaHarvest` | 1 |
| `RangedTiles` | `true` (tile ranges) |
| `EffectActions` | `[12]` |
| `EffectSounds` | `[]` (empty) |
| `EffectDelay` | 0s |
| `EffectSoundDelay` | 8.0s |

### Water Tiles

**Land tiles (ranges)**: `_waterLandTiles[]` — `0x00A8-0x00AB`, `0x0136-0x0137`

**Static tiles**: `waterStaticTiles[]` — `0x1797-0x179C`, `0x346E-0x3485`, `0x3490-0x34AB`, `0x34B5-0x35D5`

### Resource Table

| Resource | ReqSkill | MinSkill | MaxSkill | Success Message | Item Type |
|----------|----------|----------|----------|----------------|-----------|
| Fish | 0.0 | 0.0 | 100.0 | `1043297` | `Fish` |

### Bonus Resources (ML+)

| Index | Chance | Message | Item Type |
|-------|--------|---------|-----------|
| 0 | 99.4% | — | Nothing |
| 1 | 0.6% | `1072597` | `WhitePearl` |

### Mutate Table (Special Catches)

The `_mutateTable[]` defines special catches based on Fishing skill. Each entry has a required skill base, a skill value range for chance calculation, and a deep water flag.

| Catch | ReqSkill | MinSkill | MaxSkill | DeepWater | Item Types |
|-------|----------|----------|----------|-----------|------------|
| SpecialFishingNet | 80.0 | 80.0 | 4080.0 | Yes | `SpecialFishingNet` |
| BigFish | 80.0 | 80.0 | 4080.0 | Yes | `BigFish` |
| TreasureMap | 90.0 | 80.0 | 4080.0 | Yes | `TreasureMap` |
| MessageInABottle | 100.0 | 80.0 | 4080.0 | Yes | `MessageInABottle` |
| Magic Fish | 0.0 | 125.0 | -2375.0 | No | `PrizedFish`, `WondrousFish`, `TrulyRareFish`, `PeculiarFish` |
| Shoes | 0.0 | 105.0 | -420.0 | No | `Boots`, `Shoes`, `Sandals`, `ThighBoots` |
| Nothing | 0.0 | 200.0 | -200.0 | No | — (fallback) |

The chance calculation:
```
if (skillBase >= entry.m_ReqSkill)
{
    chance = (skillValue - entry.m_MinSkill) / (entry.m_MaxSkill - entry.m_MinSkill)
    if (chance > Utility.RandomDouble())
        return entry.m_Types.RandomElement()
}
```

Note: Magic Fish and Shoes have negative MaxSkill values, making the denominator negative and the chance calculation inverted (higher skill = higher chance).

### Deep Water Check

`SpecialFishingNet.FullValidation(map, loc.X, loc.Y)` determines if a location is deep water. Only deep-water entries in the mutate table are available in deep water; non-deep-water entries are only available in shallow water.

### Special Behaviors

**Not mounted**: `CheckHarvest()` blocks fishing while mounted:
```csharp
if (from.Mounted)
{
    from.SendLocalizedMessage(500971); // You can't fish while riding!
    return false;
}
```

**SOS (Sailor's Sign of the Sea) integration**: `CheckResources()` checks for SOS items in the player's backpack that are within range:
```csharp
foreach (sos in pack.FindItemsByType<SOS>())
    if ((from.Map == Map.Felucca || from.Map == Map.Trammel) && from.InRange(sos.TargetLocation, 60))
        return true;  // SOS provides resources even if local bank is empty
```

**Concurrent harvest message**: `OnConcurrentHarvest()` sends "You are already fishing." (500972).

**Quest integration**: `SpecialHarvest()` checks for `CollectorQuest` with `FishPearlsObjective`:
```csharp
if (qs is CollectorQuest)
{
    var obj = qs.FindObjective<FishPearlsObjective>();
    if (obj?.Completed == false)
    {
        if (Utility.RandomBool())
            // "You pull a shellfish out of the water, and find a rainbow pearl inside of it."
            obj.CurProgress++;
        else
            // "You pull a shellfish out of the water, but it doesn't have a rainbow pearl."
        return true;  // Prevent normal harvesting
    }
}
```

**Special item construction**: `Construct()` handles special item creation:
- `TreasureMap`: Level 0 if young + Trammel + Haven Island, else Level 1
- `MessageInABottle`: Creates with appropriate map
- SOS targeting: Returns shipwrecked items, chests with treasure maps, or fishing nets

**Shipwrecked loot table** (from SOS):
| Case | Type | Item IDs |
|------|------|----------|
| 0 | Body parts | `0x1CDD, 0x1CE5` (arm), `0x1CE0, 0x1CE8` (torso), `0x1CE1, 0x1CE9` (head), `0x1CE2, 0x1CEC` (leg) |
| 1 | Bone parts | Skulls `0x1AE0-0x1AE4`, bone piles `0x1B09-0x1B10`, pelvis `0x1B15, 0x1B16` |
| 2 | Paintings/portraits | `0x0E9F-0x0EA8` |
| 3 | Pillows | `0x13A4-0x13AE` |
| 4 | Shells | `0x0FC4-0x0FCC` |
| 5 | Hats | `SkullCap` or `TricorneHat` |
| 6 | Misc | Random from `miscItems[]` or `Candelabra` |

**Sea Serpent delivery**: `Give()` handles special items by spawning a sea serpent:
```csharp
if (item is TreasureMap or MessageInABottle or SpecialFishingNet)
{
    serp = (random < 0.25) ? new DeepSeaSerpent() : new SeaSerpent();
    // Find nearby water tile within 10 tiles
    // Move serpent to water, place item on serpent
    from.SendLocalizedMessage(503170); // Uh oh! That doesn't look like a fish!
    return true;
}
```

**Chest delivery**: `Give()` also places `BigFish`, `WoodenChest`, and `MetalGoldenChest` at the player's feet:
```csharp
return base.Give(m, item, placeAtFeet || item is BigFish or WoodenChest or MetalGoldenChest);
```

**Success messages**: `SendSuccessTo()` has extensive custom messaging for different catch types:
- `BigFish` → "Your fishing pole bends as you pull a big fish from the depths!" (1042635)
- `WoodenChest/MetalGoldenChest` → "You pull up a heavy chest from the depths of the ocean!" (503175)
- `BaseMagicFish` → "a mess of small fish" (1008124)
- `TreasureMap` → "a sodden piece of parchment" (1008125)
- `MessageInABottle` → "a bottle, with a message in it" (1008125)
- `SpecialFishingNet` → "a special fishing net" (1008125)

**Cast effect**: `OnHarvestStarted()` plays a cast effect after 1.5 seconds:
```csharp
Timer.StartTimer(TimeSpan.FromSeconds(1.5), () =>
{
    if (Core.ML) from.RevealingAction();
    Effects.SendLocationEffect(loc, map, 0x352D, 16, 4);
    Effects.PlaySound(loc, map, 0x364);
});
```

**ML revealing action**: `OnHarvestStarted()` and `OnHarvestFinished()` call `from.RevealingAction()` when `Core.ML`.

---

## Tool Requirements

Each harvesting system has specific tool requirements validated in `CheckHarvest()`:

| System | Tool | Requirement | Breaks on Depletion |
|--------|------|-------------|---------------------|
| Lumberjacking | `BaseAxe` (any) | Must be **equipped** (`tool.Parent == from`) | Yes |
| Mining | `Pickaxe` (any) | Must be **usable** (not equipped requirement) | Yes |
| Fishing | `FishingPole` (any) | Must be **usable** (not equipped requirement) | Yes |

Tool durability is decremented in `FinishHarvesting()`:
```csharp
if (tool is IUsesRemaining toolWithUses)
{
    toolWithUses.ShowUsesRemaining = true;
    if (toolWithUses.UsesRemaining > 0)
        --toolWithUses.UsesRemaining;
    if (toolWithUses.UsesRemaining < 1)
    {
        tool.Delete();
        def.SendMessageTo(from, def.ToolBrokeMessage);
    }
}
```

---

## Expansion Dependencies

Several harvesting features depend on expansion flags:

| Feature | Expansion | Condition |
|---------|-----------|-----------|
| Multiple log types (Oak, Ash, Yew, etc.) | ML | `Core.ML` |
| Multiple ore types + granites + elementals | ML | `Core.ML` |
| Bonus resources (ML gems, bark fragments, etc.) | ML | `Core.ML` |
| Race bonus (Elves/Humans) | ML | `Core.ML` (`def.RaceBonus = Core.ML`) |
| Randomize veins | ML | `Core.ML` (`def.RandomizeVeins = Core.ML`) |
| Revealing action | ML | `Core.ML` |
| Lumberjack effect counts (pre-AOS) | Pre-AOS | `!Core.AOS` → 5 swings |
| Lumberjack effect counts (AOS+) | AOS+ | `Core.AOS` → 1 swing |

---

## Cross-References

- [`skills/utility-skills.md`](skills/utility-skills.md) — Fishing, Lumberjacking, Mining skill definitions
- [`items/tools.md`](items/tools.md) — Harvesting tools (pickaxes, axes, fishing poles)
- [`systems/crafting.md`](systems/crafting.md) — Crafted resources (logs → boards, ore → ingots)
- [`creatures/monsters.md`](creatures/monsters.md) — Elemental creatures from mining with Gargoyles Pickaxe
- [`creatures/npcs.md`](creatures/npcs.md) — Quest NPCs (Witch Apprentice, Collector Quest)
- [`expansions/timeline.md`](expansions/timeline.md) — Expansion flags (Core.ML, Core.AOS)
