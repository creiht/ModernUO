# Khaldun Dungeon

The Khaldun dungeon is a self-contained puzzle dungeon generated via the `/GenKhaldun` command. It features a multi-room tomb exploration with switch-activated stone walls, tile-morphing puzzle pieces, ambient sound effects, approach-sensitive lighting, and a final cylinder-lock puzzle chest. The dungeon is built around a lore narrative told through 3 journal books dropped by 4 cursed NPC guardians. This document covers the dungeon generation, puzzle mechanics, raisable items, puzzle chest, NPCs, and lore books.

**Source Files:**
- `Projects/UOContent/Engines/Khaldun/KhaldunGen.cs` (213 lines) — dungeon generator command
- `Projects/UOContent/Engines/Khaldun/RaisableItem.cs` (165 lines) — raisable stone walls/doors
- `Projects/UOContent/Engines/Khaldun/RaiseSwitch.cs` (185 lines) — switches that raise raisable items
- `Projects/UOContent/Engines/Khaldun/KhaldunPitTeleporter.cs` (64 lines) — teleporters for the dungeon
- `Projects/UOContent/Engines/Khaldun/PuzzleChest.cs` (842 lines) — end reward chest with cylinder puzzle
- `Projects/UOContent/Engines/Khaldun/Books/GrimmochJournal.cs` (578 lines) — Grimmoch's 11 journal entries
- `Projects/UOContent/Engines/Khaldun/Books/LysanderNotebook.cs` (402 lines) — Lysander's 6 notebooks
- `Projects/UOContent/Engines/Khaldun/Books/TavarasJournal.cs` (1025 lines) — Tavara's 14 journal entries
- `Projects/UOContent/Engines/Khaldun/Mobiles/GrimmochDrummel.cs` (106 lines) — archer NPC
- `Projects/UOContent/Engines/Khaldun/Mobiles/LysanderGathenwale.cs` (103 lines) — mage NPC
- `Projects/UOContent/Engines/Khaldun/Mobiles/MorgBergen.cs` (81 lines) — melee NPC
- `Projects/UOContent/Engines/Khaldun/Mobiles/TavaraSewel.cs` (91 lines) — fencer NPC
- `Projects/UOContent/Items/Misc/MorphItem.cs` — tile-changing puzzle pieces (shared, not Khaldun-specific)
- `Projects/UOContent/Items/Misc/EffectController.cs` — sound effect triggers (shared, not Khaldun-specific)

---

## Dungeon Generation

### Command

The dungeon is generated via the `/GenKhaldun` command, registered with `AccessLevel.Developer`:

```csharp
CommandSystem.Register("GenKhaldun", AccessLevel.Developer, GenKhaldun_OnCommand);
```

### Generation Process

`GenKhaldun_OnCommand` (`KhaldunGen.cs:118-211`) creates ~50 dynamic items on `Map.Felucca`:

1. **Morph Items** (~30): Tile-matching puzzle pieces that change appearance when stepped on
2. **Approach Lights** (~8): Light-switching morph items near cave entrances
3. **Sound Effects** (~10): `EffectController` items that play sounds when players are nearby
4. **Big Teleporters** (4): Large pit morph items at the cavern entrance
5. **Central Switches** (2): One `DisappearingRaiseSwitch`, one `RaiseSwitch`
6. **Raisable Items** (2): A stone wall (`0x788`) and a door (`0x1D0`)

The command reports the count of generated items:
```csharp
e.Mobile.SendMessage($"{m_Count} dynamic Khaldun item{(m_Count == 1 ? "" : "s")} generated.");
```

### Helper Methods

| Method | Purpose |
|--------|---------|
| `FindMorphItem(x, y, z, inactiveID, activeID)` | Checks if exact morph item exists at coordinates |
| `FindEffectController(x, y, z)` | Checks if effect controller exists at coordinates |
| `TryCreateItem<T>(x, y, z, srcItem)` | Finds existing item of type T at coords, or creates new one |
| `CreateMorphItem(x, y, z, inactiveID, activeID, range)` | Creates a morph item with given inactive/active IDs and trigger range |
| `CreateApproachLight(x, y, z, off, on, light)` | Creates a morph item that also changes light level |
| `CreateSoundEffect(x, y, z, sound, range)` | Creates an `EffectController` with InRange trigger |
| `CreateBigTeleporterItem(x, y, reverse)` | Creates a large pit morph item (0x17DC ↔ 0x17EE) |

### Hardcoded Coordinates

The dungeon is centered around **x~5459, y~1426** on Felucca. Key areas:

| Area | Coordinates | Components |
|------|-------------|------------|
| Central entrance | (5459, 1426, 10) | DisappearingRaiseSwitch → RaisableItem(0x788, 1.5min) |
| Outer door | (5524, 1367) | RaisableItem(0x1D0, 5min) |
| Outer switch | (5403, 1359) | RaiseSwitch |
| Outer stone | (5403, 1360) | RaisableItem(0x788) |
| Cavern pits | (5387-5388, 1325-1326) | 4 BigTeleporterItem (0x17DC ↔ 0x17EE) |
| Approach lights | (5393-5397, 1417-1421) | 5 lights with Circle150 |
| Cave lights | (5441, 5446, 1393, z=5) | 2 lights with Circle225 |
| Sound effects | (5425, 1489-1491, 5449-5453, 1499) | Multiple sound triggers |

---

## Raisable Items

### RaisableItem Class

`RaisableItem` (`RaisableItem.cs`) represents stone walls and doors that can be raised and lowered via switches.

```csharp
public class RaisableItem : Item
{
    private int _moveSound;
    private int _stopSound;
    private TimeSpan _closeDelay;
    private int _elevation;  // current Z offset from base
    private RaiseTimer _raiseTimer;
    
    public int MaxElevation { get; set; }  // clamped to [0, 60]
    public bool IsRaisable => _raiseTimer == null;
}
```

### Constructors

| Constructor | Parameters |
|-------------|------------|
| `RaisableItem(int itemID)` | Basic construction with 1min default close delay |
| `RaisableItem(int itemID, int maxElevation, TimeSpan closeDelay)` | Custom elevation and auto-close delay |
| `RaisableItem(int itemID, int maxElevation, int moveSound, int stopSound, TimeSpan closeDelay)` | Full construction with custom sounds |

### Raise Animation

The `RaiseTimer` (inner class, lines 92-164) handles the animated raising/lowering:

- **Tick interval**: 0.5 seconds
- **Movement**: Z increments/decrements every 3 ticks (1.5s per tile)
- **Direction**: Goes up first (`_up = true`), then after reaching `MaxElevation`, waits for `CloseDelay`, then lowers
- **Sounds**: Plays `MoveSound` on each movement tick, `StopSound` at top and bottom
- **Completion**: Sets `_raiseTimer = null` when fully lowered, making `IsRaisable` return true again

### Close Delay Behavior

The `CloseDelay` determines how long the item stays raised after reaching maximum elevation:

| Item | MaxElevation | MoveSound | StopSound | CloseDelay |
|------|-------------|-----------|-----------|------------|
| Central stone | 10 | -1 | -1 | 1.5min |
| Outer door | 20 | -1 | -1 | 5.0min |

After `CloseDelay` elapses, the item automatically begins lowering.

### Serialization

```csharp
[SerializableProperty(0)] public int MaxElevation      // serialized
[SerializableField(1)] private int _moveSound           // serialized
[SerializableField(2)] private int _stopSound           // serialized
[SerializableField(3)] private TimeSpan _closeDelay     // serialized
[SerializableField(4, setter: "private")] private int _elevation  // restored on deserialization
```

`AfterDeserialization()` (line 86-90) restores the Z offset:
```csharp
Z -= _elevation;
```

---

## Switches

### RaiseSwitch

`RaiseSwitch` (`RaiseSwitch.cs:7-127`) is a fixed switch that activates a linked `RaisableItem`.

```csharp
public partial class RaiseSwitch : Item
{
    private RaisableItem _raisableItem;  // linked raisable item
    private ResetTimer _resetTimer;      // auto-reset timer
    
    public RaisableItem RaisableItem { get => _raisableItem; set => _raisableItem = value; }
}
```

### Interaction Flow

1. **Double-click check**: Player must be within 2 tiles (`InRange(this, 2)`)
2. **Flip**: Switch toggles between `0x1093` (flipped/down) and `0x1095` (unflipped/up)
3. **Sound**: Plays 0x3E8 on flip
4. **Activate**: If `RaisableItem.IsRaisable` is true, calls `RaisableItem.Raise()`
5. **Message**: Player hears "You hear a grinding noise echoing in the distance."

If the raisable item is already raised (`!IsRaisable`), the player hears "You flip the switch again, but nothing happens."

### Auto-Reset Timer

After flipping up, a `ResetTimer` is started with the raisable item's `CloseDelay` (default 2.0min if null):

```csharp
private class ResetTimer : Timer
{
    private readonly RaiseSwitch _raiseSwitch;
    // OnTick: calls _raiseSwitch.Reset() which flips it back down
}
```

### DisappearingRaiseSwitch

`DisappearingRaiseSwitch` (`RaiseSwitch.cs:129-185`) is a variant that appears/disappears based on player proximity:

```csharp
public partial class DisappearingRaiseSwitch : RaiseSwitch
{
    public DisappearingRaiseSwitch() : base(0x108F) { }
    
    public int CurrentRange => Visible ? 3 : 2;
    public override bool HandlesOnMovement => true;
}
```

**Behavior differences from `RaiseSwitch`**:
- **ItemID**: 0x108F (vs 0x1093 base)
- **Flip()**: No-op (doesn't visually toggle)
- **Reset()**: No-op (no auto-reset)
- **OnMovement**: Calls `Refresh()` when player enters/exits range
- **Refresh()**: Scans `GetMobilesInRange(CurrentRange)` — visible if any non-hidden or non-player mobile is present
- **Range**: 3 tiles when visible, 2 tiles when invisible

### Switch Pairing

In `GenKhaldun_OnCommand` (lines 198-208), switches are paired with raisable items after creation:

```csharp
var sw = TryCreateItem(5459, 1426, 10, new DisappearingRaiseSwitch());
var stone = TryCreateItem(5403, 1360, 0, new RaisableItem(0x788, 10, 0x477, 0x475, TimeSpan.FromMinutes(1.5)));
sw.RaisableItem = stone;

var lv = TryCreateItem(5403, 1359, 0, new RaiseSwitch());
var door = TryCreateItem(5524, 1367, 0, new RaisableItem(0x1D0, 20, 0x477, 0x475, TimeSpan.FromMinutes(5.0)));
lv.RaisableItem = door;
```

---

## Puzzle Chest

The `PuzzleChest` is the dungeon's end reward, featuring a 5-cylinder lock puzzle inspired by Mastermind.

### PuzzleChestCylinder Enum

```csharp
public enum PuzzleChestCylinder
{
    None = 0xE73,
    LightBlue = 0x186F,
    Blue = 0x186A,
    Green = 0x186B,
    Orange = 0x186C,
    Purple = 0x186D,
    Red = 0x186E,
    DarkBlue = 0x1869,
    Yellow = 0x1870
}
```

8 cylinder colors plus `None` (empty slot).

### PuzzleChestSolution

A solution consists of exactly 5 cylinders, each independently random:

```csharp
public class PuzzleChestSolution
{
    public const int Length = 5;
    private PuzzleChestCylinder[] _cylinders;
    
    public PuzzleChestCylinder First { get; set; }   // index 0
    public PuzzleChestCylinder Second { get; set; }   // index 1
    public PuzzleChestCylinder Third { get; set; }    // index 2
    public PuzzleChestCylinder Fourth { get; set; }   // index 3
    public PuzzleChestCylinder Fifth { get; set; }    // index 4
}
```

Random generation picks from 8 colors with equal probability (1/8 each).

### Puzzle Matching Algorithm

The `Matches()` method implements a Mastermind-like comparison:

```csharp
public bool Matches(PuzzleChestSolution solution, out int cylinders, out int colors)
```

- **cylinders**: Number of cylinders in the correct position (exact match)
- **colors**: Number of cylinders that exist in the solution but are in the wrong position

Algorithm:
1. First pass: count exact position matches, mark matched positions
2. Second pass: count color matches among unmatched positions (each source cylinder can only match one destination)

Returns `true` when all 5 cylinders match exactly.

### PuzzleChest Base Class

```csharp
public abstract class PuzzleChest : BaseTreasureChest
{
    public const int HintsCount = 3;
    public static readonly TimeSpan CleanupTime = TimeSpan.FromHours(1.0);
    
    private PuzzleChestCylinder[] _hints;
    private Dictionary<Mobile, PuzzleChestSolutionAndTime> _guesses;
    private PuzzleChestSolution _solution;
}
```

### Hints System

3 hint cylinders are generated from the solution on creation:

```csharp
private void InitHints()
{
    // Copy cylinders 1-4, shuffle, take first 3
    Span<PuzzleChestCylinder> cylinders = stackalloc PuzzleChestCylinder[Solution.Cylinders.Length - 1];
    Solution.Cylinders.AsSpan(1).CopyTo(cylinders);
    cylinders.Shuffle();
    _hints = cylinders[..HintsCount].ToArray();
}
```

Hints are shuffled from positions 1-4 (excluding position 0), providing 3 cylinder colors from the solution without revealing their positions.

### Guess Tracking and Cleanup

Player guesses are tracked in `_guesses` dictionary:

```csharp
public void SubmitSolution(Mobile m, PuzzleChestSolution solution)
{
    if (solution.Matches(Solution, out var correctCylinders, out var correctColors))
    {
        LockPick(m);  // Opens the chest
        DisplayTo(m);
    }
    else
    {
        (_guesses ??= []).Add(m, new PuzzleChestSolutionAndTime(Core.Now, solution));
        StartCleanupTimer();
        
        m.SendGump(new StatusGump(correctCylinders, correctColors));
        DoDamage(m);  // Punishment for wrong guess
    }
}
```

**Cleanup Timer**: Runs every 1 hour, removes guesses older than 1 hour. Stops when no guesses remain.

### Wrong Guess Punishment

`DoDamage(Mobile to)` randomly applies one of 4 damage types (25% each):

| Case | Effect | Damage Type | Amount | Message |
|------|--------|-------------|--------|---------|
| 0 | Toxics vapor effect + poison | Poison | — | "A toxic vapor envelops thee." |
| 1 | Fire effect + fire damage | Fire | 10-40 HP | "Searing heat scorches thy skin." |
| 2 | Pain effect + physical damage | Physical | 10-40 HP | "Pain lances through thee from a sharp metal blade." |
| 3 | Bolt effect + energy damage | Energy | 10-40 HP | "Lightning arcs through thy body." |

### Treasure Table

`GenerateTreasure()` (`PuzzleChest.cs:431-528`) awards:

| Item | Quantity | Details |
|------|----------|---------|
| Gold | 600-900 | Random amount |
| Gems | Up to 9 | `Loot.RandomGem()` with deduplication (same gem types stack) |
| BagOfReagents | 20% chance | — |
| Weapon/Armor/Hat/Jewelry | 2 items | Random AOS-infused gear |

**AOS Infusion** (via `GetRandomAOSStats`):

| RNG Roll | Attribute Count | Min Value | Max Value | Probability |
|----------|----------------|-----------|-----------|-------------|
| 0 | 2-6 | 20 | 70 | 6.67% |
| 1-2 | 2-4 | 20 | 50 | 13.33% |
| 3-5 | 2-3 | 20 | 40 | 20% |
| 6-9 | 1-2 | 10 | 30 | 26.67% |
| 10-14 | 1 | 10 | 20 | 33.33% |

### PuzzleGump

The puzzle solving UI (`PuzzleChest.PuzzleGump`, lines 575-784):

**Layout**:
- 500×410 pixel gump with 0x53 background
- 10 cylinder selection buttons (5 left: LightBlue/Blue/Green/Orange/Purple, 5 right: Red/DarkBlue/Yellow/None/cycle)
- 5 pedestal switches (0x867/0x86A radio buttons) for placing cylinders
- Submit button (ID 1)
- Previous guess display (if available)

**Lockpicking Hints** (based on player's Lockpicking skill):

| Skill Range | Hints Shown |
|-------------|-------------|
| 60+ | "Lockpicking hint:" label + FirstHint |
| 70+ | + SecondHint |
| 80+ | First cylinder hint + FirstHint |
| 90+ | + SecondHint |
| 100+ | + ThirdHint |

**Note**: The first cylinder hint shows `Solution.First` (the actual first cylinder from positions 1-4), not `FirstHint`. This gives a direct hint about position 0.

### PuzzleChest Subtypes

| Class | ItemID | Flippable |
|-------|--------|-----------|
| `MetalGoldenPuzzleChest` | 0xE41 | 0xE40 ↔ 0xE41 |
| `StrongBoxPuzzle` | 0xE80 | 0x9A8 ↔ 0xE80 |

### Chest Lock Level

```csharp
protected override void SetLockLevel()
{
    LockLevel = ILockpickable.CannotPick;  // Cannot be picked by lockpicks
}
```

The chest can only be opened by solving the cylinder puzzle, not by lockpicking.

---

## KhaldunPitTeleporter

`KhaldunPitTeleporter` (`KhaldunPitTeleporter.cs`) is a hidden teleporter for traversing the cavern pits.

```csharp
public partial class KhaldunPitTeleporter : Item
{
    private bool _active;
    private Point3D _pointDest;
    private Map _mapDest;
    
    public bool Active => _active;
    public Point3D PointDest => _pointDest;
    public Map MapDest => _mapDest;
}
```

### Properties

| Property | Default | Description |
|----------|---------|-------------|
| ItemID | 0x053B | Hidden pit tile |
| Hue | 1 | Nearly invisible |
| Movable | false | Fixed in place |
| LabelNumber | 1016511 | "the floor of the cavern seems to have collapsed here - a faint light is visible at the bottom of the pit" |

### Teleport Behavior

1. **Active check**: Teleport only works when `_active == true`
2. **Range check**: Player must be within 3 tiles
3. **Destination validation**: Map must not be `Internal`
4. **Teleport**: Moves player and pets to destination
5. **Inability**: If range check fails, sends "I can't reach that."

---

## NPCs

All 4 Khaldun NPCs are `BaseCreature` with `AlwaysMurderer = true`, `DeleteCorpseOnDeath = true`, and `ShowFameTitle = false`. They all share the title "the Cursed" and a distinctive hue (0x8596 for warriors, 0x8838 for mage/sorceress).

### GrimmochDrummel — "the Cursed"

**Type**: Archer AI | **Hue**: 0x8596 | **Body**: 0x190 (male)

**Stats**:
| Stat | Range |
|------|-------|
| Str | 111-120 |
| Dex | 151-160 |
| Int | 41-50 |
| Hits | 180-207 |
| Mana | 0 |

**Skills**:
| Skill | Range |
|-------|-------|
| Archery | 90.1-110.0 |
| Tactics | 90.1-100.0 |
| Anatomy | 90.1-100.0 |
| Swords | 60.1-70.0 |
| MagicResist | 60.1-70.0 |

**Resistances**:
| Type | Range |
|------|-------|
| Physical | 35-45 |
| Fire | 25-30 |
| Cold | 45-55 |
| Poison | 30-40 |
| Energy | 20-25 |

**Equipment**: Bow (0x8A4), Boots (0x8A4), BodySash (0x8A4), LeatherGloves (0x96F), LeatherChest (0x96F), Backpack

**Loot**: 40 Arrows, 3% FireHorn, ~33% GrimmochJournal, 190-230 Gold on death, Backpack on death

**Karma**: -1000 | **Fame**: 5000

### LysanderGathenwale — "the Cursed"

**Type**: Mage AI | **Hue**: 0x8838 | **Body**: 0x190 (male)

**Stats**:
| Stat | Range |
|------|-------|
| Str | 111-120 |
| Dex | 71-80 |
| Int | 121-130 |
| Hits | 180-207 |
| Mana | 227-265 |

**Skills**:
| Skill | Range |
|-------|-------|
| EvalInt | 95.1-100.0 |
| Magery | 90.1-100.0 |
| Meditation | 90.1-100.0 |
| Tactics | 90.1-100.0 |
| MagicResist | 80.1-90.0 |
| Wrestling | 80.1-90.0 |

**Resistances**:
| Type | Range |
|------|-------|
| Physical | 35-45 |
| Fire | 25-30 |
| Cold | 50-60 |
| Poison | 25-35 |
| Energy | 25-35 |

**Equipment**: Spellbook (0x599), RingmailGloves (0x599), StuddedChest (0x96F), PlateArms (0x599), Boots (0x599), Cloak (0x96F)

**Loot**: 2× MedScrolls (via `LootPack.MedScrolls`), 30 random reagents, ~33% LysanderNotebook on death

**Karma**: -10000 | **Fame**: 5000

### MorgBergen — "the Cursed"

**Type**: Melee AI | **Hue**: 0x8596 | **Body**: 0x190 (male)

**Stats**:
| Stat | Range |
|------|-------|
| Str | 111-120 |
| Dex | 111-120 |
| Int | 51-60 |
| Hits | 180-207 |
| Mana | 0 |

**Skills**:
| Skill | Range |
|-------|-------|
| Swords | 90.1-100.0 |
| Tactics | 90.1-100.0 |
| Anatomy | 90.1-100.0 |
| MagicResist | 80.1-90.0 |

**Resistances**:
| Type | Range |
|------|-------|
| Physical | 35-45 |
| Fire | 25-30 |
| Cold | 50-60 |
| Poison | 25-35 |
| Energy | 25-35 |

**Damage Type**: Physical 40%, Cold 60%

**Equipment**: Bardiche (0x96F), LeatherGloves (0x96F), LeatherArms (0x96F), ShortPants (0x59C)

**Loot**: 190-230 Gold on death

**Karma**: -1000 | **Fame**: 5000

### TavaraSewel — "the Cursed"

**Type**: Melee AI | **Hue**: 0x8838 | **Body**: 0x191 (female)

**Stats**:
| Stat | Range |
|------|-------|
| Str | 111-120 |
| Dex | 111-120 |
| Int | 111-120 |
| Hits | 180-207 |
| Stam | 126-150 |
| Mana | 0 |

**Skills**:
| Skill | Range |
|-------|-------|
| Fencing | 90.1-100.0 |
| Tactics | 90.1-100.0 |
| Anatomy | 90.1-100.0 |
| MagicResist | 80.1-90.0 |

**Resistances**:
| Type | Range |
|------|-------|
| Physical | 25-30 |
| Fire | 25-30 |
| Cold | 50-60 |
| Poison | 25-35 |
| Energy | 25-35 |

**Equipment**: Kryss (0x96F), Buckler (0x96F), RingmailGloves (0x599), FemalePlateChest (0x96F), Kilt (0x59C), Sandals (0x599)

**Loot**: 190-230 Gold on death, ~33% TavarasJournal on death

**Karma**: -1000 | **Fame**: 5000

---

## Lore Books

The dungeon's story is told through 3 journal books, each representing a different perspective on the same events. The books are dropped by NPCs on death (Grimmoch and Tavara ~33% chance, Lysander ~33% chance).

### GrimmochJournal (11 entries)

Grimmoch Drummel's perspective — a huntsman who grows increasingly disturbed by the tomb.

| Entry | Days | Summary |
|-------|------|---------|
| GrimmochJournal1 | Day One | Excavation begins, workers uneasy about stone doors |
| GrimmochJournal2 | Day Two | Lysander's incantation opens the doors |
| GrimmochJournal3 | Day Three-Five | Blocked hallway, Lysander dismisses concerns |
| GrimmochJournal6 | Day Six | Beast attack, camp moved inside tomb |
| GrimmochJournal7 | Day Seven-Ten | Grimmoch hears scratching/laughter, 3 workers missing |
| GrimmochJournal11 | Day Eleven-Thirteen | Lysander gone with 2 workers, Grimmoch hears them too |
| GrimmochJournal14 | Day Fourteen-Sixteen | Dead piled up, barricade built, Lysander taken by undead |
| GrimmochJournal17 | Day Seventeen-Twenty-Two | Continuous fighting, Thomas killed and reanimated |
| GrimmochJournal23 | Day Twenty-Three | "We no longer bury the dead." |

### LysanderNotebook (6 entries)

Lysander Gathenwale's perspective — a cultist of Khal Ankur seeking dark power.

| Entry | Days | Summary |
|-------|------|---------|
| LysanderNotebook1 | Day One | Invocation to Khal Ankur, seeking his secrets |
| LysanderNotebook2 | Day Two | Despises Tavara Sewel, impatient with slow progress |
| LysanderNotebook3 | Day Three-Six | Beasts attacking camp, convinced Tavara to move inside |
| LysanderNotebook7 | Day Seven | Vows to kill Tavara and Grimmoch, asks Khal Ankur for the gift |
| LysanderNotebook8 | Day Eight-Ten | Workers missing, suspects they fled, vows to kill them |
| LysanderNotebook11 | Day Eleven-Thirteen | Found Khal Ankur's path, killed 2 workers, joined the Khaldun |

### TavarasJournal (14 entries)

Tavara Sewel's perspective — a scholar leading the archaeological expedition.

| Entry | Days | Summary |
|-------|------|---------|
| TavarasJournal1 | Day One | Excavation begins, admiring the stone doors |
| TavarasJournal2 | Day Two | Tomb of Khal Ankur opened, passage blocked by rubble |
| TavarasJournal3 | Day Three-Five | Puzzling stone pile, seems deliberately placed |
| TavarasJournal6 | Day Six | Beast attack, camp moved inside tomb |
| TavarasJournal7 | Day Seven | History of Khal Ankur's cult, Keepers of the Seventh Death |
| TavarasJournal8 | Day Eight | New antechamber discovered, library/museum |
| TavarasJournal9 | Day Nine-Ten | 3 workers missing, Lysander becomes unhinged |
| TavarasJournal11 | Day Eleven-Thirteen | 2 more workers missing, Lysander disappeared |
| TavarasJournal14 | Day Fourteen-Fifteen | Lysander returns bloodied and armed with a dagger |
| TavarasJournal16 | Day Sixteen | Bergen killed by undead, entrance sealed by earthquake |
| TavarasJournal16b | Day Sixteen (Later) | Ran back to antechamber, barricade erected |
| TavarasJournal17 | Day Seventeen-Eighteen | Barricade won't hold, undead like ocean waves |
| TavarasJournal19 | Day Nineteen-Twenty-One | "I must end this." |

---

## MorphItem (Shared)

`MorphItem` (in `Items/Misc/MorphItem.cs`, not Khaldun-specific) is a shared item that changes its tile appearance when a player steps on it. It's used for Khaldun puzzle pieces and approach lights.

- **Inactive state**: Shown when no player is nearby
- **Active state**: Shown when a player enters the trigger range
- **Range**: Configurable per instance (1-3 tiles typical for Khaldun)

### Usage in Khaldun

Morph items are used for:
- **Puzzle pieces**: Tile ID changes (e.g., 0x1D0 → 0x1, 0x1 → 0x53B, etc.)
- **Approach lights**: 0x1857 → 0x1858 (with light level change Circle150/Circle225)
- **Big pit teleporters**: 0x17DC ↔ 0x17EE

---

## EffectController (Shared)

`EffectController` (in `Items/Misc/EffectController.cs`, not Khaldun-specific) triggers sound effects when players are nearby.

- **TriggerType**: `InRange` — triggers when player is within `TriggerRange` tiles
- **SoundId**: The sound to play
- **TriggerRange**: Distance in tiles (1-3 typical for Khaldun)

### Sounds Used in Khaldun

| Sound ID | Location | Range |
|----------|----------|-------|
| 0x102 | (5425, 1489-1491, z=5), (5524, 1367) | 1 |
| 0xF5 | (5449-5453, 1499, z=10) | 1 |
| 0x220 | (5450, 1370-1372, z=0) | 2 |
| 0x244 | (5460, 1416, z=0) | 2 |
| 0x14 | (5483, 1439, z=5) | 3 |

---

## Cross-References

- [`creatures/npcs.md`](creatures/npcs.md) — Khaldun NPCs (Grimmoch, Lysander, Morg, Tavara)
- [`items/weapons.md`](items/weapons.md) — weapons used by Khaldun NPCs (Bow, Bardiche, Kryss)
- [`items/armor.md`](items/armor.md) — armor types used by Khaldun NPCs (Leather, Ringmail, Plate)
- [`systems/combat.md`](systems/combat.md) — combat mechanics used by Khaldun NPCs
