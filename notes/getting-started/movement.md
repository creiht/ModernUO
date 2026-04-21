# Movement

Movement in ModernUO spans six distinct maps, each with unique terrain, creatures, and rules. The direction system uses 8 directional values combined with a running flag.

## Maps

ModernUO supports six explorable maps plus one internal map used for system operations.

| Map | MapID | MapIndex | Rules | Expansion |
|---|---|---|---|---|
| **Felucca** | 0 | 0 | FeluccaRules (no restrictions) | Base |
| **Trammel** | 1 | 1 | TrammelRules (free movement, beneficial/harmful restrictions) | Base |
| **Ilshenar** | 2 | 2 | See expansion | AOS |
| **Malas** | 3 | 3 | See expansion | AOS |
| **Tokuno** | 4 | 4 | See expansion | SE |
| **TerMur** | 5 | 5 | See expansion | SA |
| **Internal** | 0x7F | — | Internal flag | System |

Maps are defined in `Map.cs` as static properties: `Map.Felucca`, `Map.Trammel`, etc. Map availability is controlled by `MapSelectionFlags` in `ExpansionInfo.CoreExpansion.MapSelectionFlags`.

### Map Rules

| Rule | Felucca | Trammel |
|---|---|---|
| **Free Movement** | No (stamina loss for blocking) | Yes |
| **Beneficial Restrictions** | No | Yes (cannot heal criminals/murderers) |
| **Harmful Restrictions** | No | Yes (cannot attack innocents) |

The `MapRules` enum defines these flags:
```csharp
public enum MapRules
{
    None = 0x0000,
    Internal = 0x0001,
    FreeMovement = 0x0002,
    BeneficialRestrictions = 0x0004,
    HarmfulRestrictions = 0x0008,
    TrammelRules = FreeMovement | BeneficialRestrictions | HarmfulRestrictions,
    FeluccaRules = None
}
```

### Map Selection Flags

Map availability is controlled via `MapSelectionFlags` (defined in `MapSelection.cs`):

| Flag | Value | Map |
|---|---|---|
| `Felucca` | `0x00000001` | Felucca |
| `Trammel` | `0x00000002` | Trammel |
| `Ilshenar` | `0x00000004` | Ilshenar |
| `Malas` | `0x00000008` | Malas |
| `Tokuno` | `0x00000010` | Tokuno |
| `TerMur` | `0x00000020` | TerMur |

These flags are set by `ExpansionInfo.StoreMapSelection()` and queried via `ExpansionInfo.CoreExpansion.MapSelectionFlags`.

## Direction System

Movement uses an 8-direction system combined with a running flag, encoded in a single `Direction` byte.

### Direction Values

| Direction | Hex | X Offset | Y Offset |
|---|---|---|---|
| **North** | `0x0` | 0 | -1 |
| **Right** (NE) | `0x1` | +1 | -1 |
| **East** | `0x2` | +1 | 0 |
| **Down** (SE) | `0x3` | +1 | +1 |
| **South** | `0x4` | 0 | +1 |
| **Left** (SW) | `0x5` | -1 | +1 |
| **West** | `0x6` | -1 | 0 |
| **Up** (NW) | `0x7` | -1 | -1 |

The `Direction` enum is defined in `Mobile.cs`:

```csharp
public enum Direction : byte
{
    North = 0x0,
    Right = 0x1,
    East = 0x2,
    Down = 0x3,
    South = 0x4,
    Left = 0x5,
    West = 0x6,
    Up = 0x7,
    Mask = 0x7,
    Running = 0x80,
    ValueMask = 0x87
}
```

### Direction Flags

| Flag | Value | Purpose |
|---|---|---|
| `Mask` | `0x7` | Extracts the base direction (0-7) |
| `Running` | `0x80` | Set for running vs walking |
| `ValueMask` | `0x87` | Combines direction + running flag |

The running flag is combined with direction using bitwise OR. For example, running North-East is `Direction.Right | Direction.Running`.

### Coordinate Offsets

Direction offsets are applied via `Movement.Offset()` in `Movement.cs`:

```csharp
switch (d & Direction.Mask)
{
    case Direction.North:  y -= count; break;
    case Direction.South:  y += count; break;
    case Direction.West:   x -= count; break;
    case Direction.East:   x += count; break;
    case Direction.Right:  x += count; y -= count; break;
    case Direction.Left:   x -= count; y += count; break;
    case Direction.Down:   x += count; y += count; break;
    case Direction.Up:     x -= count; y -= count; break;
}
```

## Movement Delays

Movement speed is configurable via `ServerConfiguration` in `Movement.Configure()`:

| Setting | Default | Description |
|---|---|---|
| `movement.delay.turn` | 0 | Delay between turning |
| `movement.delay.walkFoot` | 400ms | Delay between foot步 steps when walking |
| `movement.delay.runFoot` | 200ms | Delay between foot steps when running |
| `movement.delay.walkMount` | 200ms | Delay between steps when walking on mount |
| `movement.delay.runMount` | 100ms | Delay between steps when running on mount |

Movement is implemented through the `IMovementImpl` interface in `Movement.cs`. The `Movement.CheckMovement()` method validates whether a move is legal (checks for obstacles, terrain, etc.) and returns the new Z coordinate.

## Starting Cities

Characters start at one of several inns, depending on their chosen map and profession. Starting cities are defined in `CharacterCreation.cs`.

### Felucca Starting Cities (9 cities)

| City | Inn | X | Y | Z | Heading |
|---|---|---|---|---|---|
| Yew | The Empath Abbey | 1075072 | 633 | 858 | 0 |
| Minoc | The Barnacle | 1075073 | 2476 | 413 | 15 |
| Britain | Sweet Dreams Inn | 1075074 | 1496 | 1628 | 10 |
| Moonglow | The Scholars Inn | 1075075 | 4408 | 1168 | 0 |
| Trinsic | The Traveler's Inn | 1075076 | 1845 | 2745 | 0 |
| Magincia | The Great Horns Tavern | 1075077 | 3734 | 2222 | 20 |
| Jhelom | The Mercenary Inn | 1075078 | 1374 | 3826 | 0 |
| Skara Brae | The Falconer's Inn | 1075079 | 618 | 2234 | 0 |
| Vesper | The Ironwood Inn | 1075080 | 2771 | 976 | 0 |

### Trammel Starting Cities (7 cities)

| City | Inn | X | Y | Z | Heading |
|---|---|---|---|---|---|
| Yew | The Empath Abbey | 1075072 | 633 | 858 | 0 |
| Minoc | The Barnacle | 1075073 | 2476 | 413 | 15 |
| Moonglow | The Scholars Inn | 1075075 | 4408 | 1168 | 0 |
| Trinsic | The Traveler's Inn | 1075076 | 1845 | 2745 | 0 |
| Jhelom | The Mercenary Inn | 1075078 | 1374 | 3826 | 0 |
| Skara Brae | The Falconer's Inn | 1075079 | 618 | 2234 | 0 |
| Vesper | The Ironwood Inn | 1075080 | 2771 | 976 | 0 |

### New Haven Starting Cities (2 cities)

| City | Inn | X | Y | Z | Heading |
|---|---|---|---|---|---|
| New Haven | The Bountiful Harvest Inn | 1150168 | 3503 | 2574 | 14 |
| Britain | The Wayfarer's Inn | 1075074 | 1602 | 1591 | 20 |

### TerMur Starting Cities (1 city)

| City | Inn | X | Y | Z | Heading |
|---|---|---|---|---|---|
| Royal City | Royal City Inn | 1150169 | 738 | 3486 | -19 |

### City Resolution Logic

Cities are assembled by `ConstructAvailableStartingCities()` in `CharacterCreation.cs`:

```csharp
if (trammelAvailable)
{
    if (pre6000ClientSupport)
        return OldHavenStartingCities + TrammelStartingCities;
    if (terMerAvailable)
        return NewHavenStartingCities + TrammelStartingCities + StartingCitiesSA;
    return NewHavenStartingCities + TrammelStartingCities;
}
if (availableMaps.Includes(MapSelectionFlags.Felucca))
    return FeluccaStartingCities;
```

## Profession-Specific Spawn Locations

Some professions send characters to unique starting locations:

| Profession | Location | X | Y | Z | Map | Condition |
|---|---|---|---|---|---|---|
| **Necromancer** | Mardoth's Tower | 2114 | 1301 | -50 | Malas | AOS enabled |
| **Paladin** | First available city | — | — | — | Default | Always |
| **Samurai** | Haoti's Grounds | 368 | 780 | -1 | Malas | SE + AOS + client flags |
| **Ninja** | Enimo's Residence | 414 | 823 | -1 | Malas | SE + AOS + client flags |

If the required conditions are not met, the character falls back to the first available city and receives a localized warning message.

## Map Properties

Each `Map` object has the following properties defined in `Map.cs`:

| Property | Type | Description |
|---|---|---|
| `MapID` | `int` | Unique map identifier (0-5 for playable maps) |
| `MapIndex` | `int` | Index in the Maps array |
| `Width` | `int` | Map width in tiles |
| `Height` | `int` | Map height in tiles |
| `Season` | `int` | Current season |
| `Rules` | `MapRules` | Movement and interaction rules |
| `Tiles` | `TileMatrix` | Terrain data |
| `Regions` | `Dictionary<string, Region>` | Named regions on the map |
| `DefaultRegion` | `Region` | Default region for unregioned locations |

## See Also

- [[character-creation]] — Starting cities and profession-specific spawns
- [[systems/combat]] — Combat movement and range
- [[expansions/timeline]] — When each map was introduced
