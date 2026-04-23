# Expansions

ModernUO supports **12 expansion levels** (0–11) with a cumulative design — each expansion adds features on top of all previous ones. Players with higher expansion flags enabled have access to additional content, maps, skills, spells, and systems.

## How Expansions Work

ModernUO uses a cumulative expansion model. The server's current expansion is stored in `expansion.json` and exposed through two mechanisms:

- **`Core.Expansion`** — An `Expansion` enum value (0–11) representing the server's current expansion level
- **`Core.X` boolean properties** — Convenience flags like `Core.AOS`, `Core.T2A`, etc. that return `true` when the current expansion is at or above that level

```csharp
// Expansion enum (ExpansionInfo.cs:24-38)
public enum Expansion
{
    None,    // 0 — Base UO
    T2A,     // 1 — The Second Age
    UOR,     // 2 — Renaissance
    UOTD,    // 3 — Third Dawn
    LBR,     // 4 — Lord Blackthorn's Revenge
    AOS,     // 5 — Age of Shadows
    SE,      // 6 — Samurai Empire
    ML,      // 7 — Mondain's Legacy
    SA,      // 8 — Shard Age
    HS,      // 9 — Harbinger Series
    TOL,     // 10 — The Last Refuge
    EJ       // 11 — Event Journal
}

// Core flags (Main.cs:157-178)
public static bool T2A => Expansion >= Expansion.T2A;
public static bool AOS => Expansion >= Expansion.AOS;
public static bool ML => Expansion >= Expansion.ML;
// ... all 12 expansions have boolean flags
```

This cumulative design means enabling expansion AOS automatically grants all features from T2A, UOR, UOTD, and LBR. It simplifies server configuration and ensures feature compatibility across all content.

---

## Expansion Levels

| # | Level | Expansion | Core Flag | Unlocks |
|---|-------|-----------|-----------|---------|
| 0 | 0 | **None** | — | Base UO content |
| 1 | 1 | **T2A** (The Second Age) | `Core.T2A` | Bushido, Gargoyles intro, Tokuno, Statues, Elves |
| 2 | 2 | **UOR** (Renaissance) | `Core.UOR` | Spellweaving, new creatures |
| 3 | 3 | **UOTD** (Third Dawn) | `Core.UOTD` | Sea-themed content |
| 4 | 4 | **LBR** (Lord Blackthorn's Revenge) | `Core.LBR` | New maps, creatures |
| 5 | 5 | **AOS** (Age of Shadows) | `Core.AOS` | Mysticism, Ninjitsu, Ethics, Virtues, AOS stats, Ilshenar/Malas/TerMur |
| 6 | 6 | **SE** (Samurai Empire) | `Core.SE` | Bushido refinement, new spells |
| 7 | 7 | **ML** (Mondain's Legacy) | `Core.ML` | Quest system, bonus harvest, Parasitic/Darkglow poisons, skill cap bonus |
| 8 | 8 | **SA** (Stygian Abyss) | `Core.SA` | Gothic/Rustic housing themes |
| 9 | 9 | **HS** (High Seas) | `Core.HS` | Additional content |
| 10 | 10 | **TOL** (Time of Legends) | `Core.TOL` | Jungle/Shadowguard themes, full Gargoyle content |
| 11 | 11 | **EJ** (Endless Journey) | `Core.EJ` | Latest features |

---

## Feature Flags

Each expansion has a `FeatureFlags` value — a bitmask of features that are enabled. These flags are cumulative, built using bitwise OR operations:

### Individual Feature Flags

There are **24 individual feature flags** defined in the `FeatureFlags` enum.

| Flag | Value | Unlocked At | Description |
|------|-------|-------------|-------------|
| T2A | `0x00000001` | T2A | Third Age features |
| UOR | `0x00000002` | UOR | Age of Revelation (negates T2A body replacement in later clients) |
| UOTD | `0x00000004` | UOTD | The Dark Tide |
| LBR | `0x00000008` | LBR | Legends Britannia |
| AOS | `0x00000010` | AOS | Age of Shadows (AOS stats, resistances, item attributes) |
| SixthCharacterSlot | `0x00000020` | AOS | 6th character slot |
| SE | `0x00000040` | SE | Samurai Empire |
| ML | `0x00000080` | ML | Mondain's Legacy |
| EighthAge | `0x00000100` | — | Eighth Age content |
| NinthAge | `0x00000200` | ML | Crystal/Shadow custom house tiles |
| TenthAge | `0x00000400` | — | Tenth Age content |
| IncreasedStorage | `0x00000800` | — | Increased housing/bank storage |
| SeventhCharacterSlot | `0x00001000` | — | 7th character slot |
| RoleplayFaces | `0x00002000` | — | RP face options |
| TrialAccount | `0x00004000` | — | Trial account support |
| LiveAccount | `0x00008000` | — | Live account support |
| SA | `0x00010000` | SA | SA features |
| HS | `0x00020000` | HS | HS features |
| Gothic | `0x00040000` | SA | Gothic housing tiles |
| Rustic | `0x00080000` | SA | Rustic housing tiles |
| Jungle | `0x00100000` | TOL | Jungle housing tiles |
| Shadowguard | `0x00200000` | TOL | Shadowguard content |
| TOL | `0x00400000` | TOL | TOL features |
| EJ | `0x00800000` | EJ | EJ features |

### Cumulative Feature Flag Values

```
ExpansionT2A    = T2A
ExpansionUOR    = T2A | UOR
ExpansionUOTD   = UOR | UOTD
ExpansionLBR    = UOTD | LBR
ExpansionAOS    = LBR | AOS | LiveAccount
ExpansionSE     = AOS | SE
ExpansionML     = SE | ML | NinthAge
ExpansionSA     = ML | SA | Gothic | Rustic
ExpansionHS     = SA | HS
ExpansionTOL    = HS | TOL | Jungle | Shadowguard
ExpansionEJ     = TOL | EJ
```

---

## Client Flags

Client flags control which maps and features are advertised to the game client:

| Flag | Value | Unlocked At | Description |
|------|-------|-------------|-------------|
| Felucca | `0x00000001` | Base | Felucca map |
| Trammel | `0x00000002` | Base | Trammel map |
| Ilshenar | `0x00000004` | AOS | Ilshenar map |
| Malas | `0x00000008` | AOS | Malas map |
| Tokuno | `0x00000010` | T2A | Tokuno map |
| TerMur | `0x00000020` | AOS | TerMur map |
| KR | `0x00000040` | — | Korean client flag |
| Unk2 | `0x00000080` | — | Unknown/unused flag |
| UOTD | `0x00000100` | UOTD | UOTD client flag |

---

## Character List Flags

Character list flags control what appears in the client's login screen:

| Flag | Value | Unlocked At | Description |
|------|-------|-------------|-------------|
| ContextMenus | `0x00000008` | Base | Context menus available |
| AOS | `0x00000020` | AOS | AOS character list |
| SixthCharacterSlot | `0x00000040` | AOS | 6th character slot |
| SE | `0x00000080` | SE | SE character list |
| ML | `0x00000100` | ML | ML character list |
| KR | `0x00000200` | — | Korean client |
| UO3DClientType | `0x00000400` | — | UO 3D client |
| SeventhCharacterSlot | `0x00001000` | — | 7th character slot |
| NewMovementSystem | `0x00004000` | — | New movement system |
| NewFeluccaAreas | `0x00008000` | — | New Felucca areas |

The `CharacterListFlags` enum also contains undocumented flags: `Unk1`, `OverwriteConfigButton`, `OneCharacterSlot`, `SlotLimit`, `Unk3`, and `Unk4`. The table above documents only the known/used flags.

### Cumulative Character List Flag Values

```
ExpansionNone  = ContextMenus
ExpansionT2A   = ContextMenus
ExpansionUOR   = ContextMenus
ExpansionUOTD  = ContextMenus
ExpansionLBR   = ContextMenus
ExpansionAOS   = ContextMenus | AOS
ExpansionSE    = ExpansionAOS | SE
ExpansionML    = ExpansionSE | ML
ExpansionSA    = ExpansionML
ExpansionHS    = ExpansionSA
ExpansionTOL   = ExpansionHS
ExpansionEJ    = ExpansionTOL
```

---

## Housing Flags

Housing flags control which custom house tile sets are available:

| Flag | Value | Unlocked At | Description |
|------|-------|-------------|-------------|
| AOS | `0x10` | AOS | AOS housing |
| SE | `0x40` | SE | SE housing |
| ML | `0x80` | ML | ML housing |
| Crystal | `0x200` | ML | Crystal custom house tiles |
| SA | `0x10000` | SA | SA housing |
| HS | `0x20000` | HS | HS housing |
| Gothic | `0x40000` | SA | Gothic housing tiles |
| Rustic | `0x80000` | SA | Rustic housing tiles |
| Jungle | `0x100000` | TOL | Jungle housing tiles |
| Shadowguard | `0x200000` | TOL | Shadowguard housing tiles |
| TOL | `0x400000` | TOL | TOL housing |
| EJ | `0x800000` | EJ | EJ housing |

### Cumulative Housing Flag Values

```
HousingAOS  = AOS
HousingSE   = AOS | SE
HousingML   = SE | ML | Crystal
HousingSA   = ML | SA | Gothic | Rustic
HousingHS   = SA | HS
HousingTOL  = HS | TOL | Jungle | Shadowguard
HousingEJ   = TOL | EJ
```

---

## Map Availability

| Map | Available At | Expansion |
|-----|-------------|-----------|
| Felucca | Base (0) | Base |
| Trammel | Base (0) | Base |
| Tokuno | Expansion 1 | T2A |
| Ilshenar | Expansion 5 | AOS |
| Malas | Expansion 5 | AOS |
| TerMur | Expansion 5 | AOS |

Map availability is controlled through `MapSelectionFlags` on each `ExpansionInfo` entry. The server uses these flags to determine which maps appear in the client's map selection screen.

---

## Race Unlocks

| Race | Available At | Expansion |
|------|-------------|-----------|
| Human | Base (0) | Base |
| Elf | Expansion 1 | T2A |
| Gargoyle | Expansion 10 | TOL |

Gargoyles were introduced as content in earlier expansions but are fully available as a playable race at the TOL expansion level.

---

## Expansion-Dependent Config Settings

Several configuration settings change their default values based on the current expansion:

| Setting | Default | Expansion-Gated | Description |
|---------|---------|----------------|-------------|
| `expansion.forceOldAnimations` | `false` | — | Force Pre-AOS body replacement graphics |
| `insurance.enable` | `Core.AOS` | AOS | Object Property List insurance system |
| `opl.enable` | `Core.AOS` | AOS | Object Property List |
| `visibleDamage` | `Core.AOS` | AOS | Visible damage indicators |
| `actionDelay` | `Core.AOS ? 1000 : 500` | AOS | Action delay (1s AOS+, 0.5s pre-AOS) |
| `guildClickMessage` | `!Core.AOS` | AOS | Guild click message format |
| `asciiClickMessage` | `!Core.AOS` | AOS | ASCII click message format |

These are configured in `ExpansionConfiguration.Configure()` and loaded from `Configuration/expansion.json`.

---

## Configuration System

### Runtime Configuration

The server's active expansion is stored in `Configuration/expansion.json` and loaded at startup:

```csharp
// Loading
ExpansionInfo.LoadConfiguration(out Expansion expansion);

// Saving
ExpansionInfo.SaveConfiguration();
```

### Static Expansion Data

Individual expansion metadata (name, flags, required client version, etc.) is loaded from `Data/expansions.json` at static constructor time:

```csharp
Table = JsonConfig.Deserialize<ExpansionInfo[]>(path);
```

### Accessing Expansion Info

```csharp
// Get info for a specific expansion level
ExpansionInfo info = ExpansionInfo.GetInfo(Expansion.AOS);

// Get info for the current server expansion
ExpansionInfo coreInfo = ExpansionInfo.CoreExpansion;

// Get era-specific data folder
string eraFolder = ExpansionInfo.GetEraFolder(parentDirectory);
```

### ExpansionInfo Properties

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `int` | Expansion index (0–11) |
| `Name` | `string` | Human-readable name |
| `ClientFlags` | `ClientFlags` | Map and client feature flags |
| `SupportedFeatures` | `FeatureFlags` | Cumulative feature bitmask |
| `CharacterListFlags` | `CharacterListFlags` | Login screen flags |
| `RequiredClient` | `ClientVersion` | Minimum client version |
| `HousingFlags` | `HousingFlags` | Custom housing tile sets |
| `MobileStatusVersion` | `int` | Mobile status packet version |
| `MapSelectionFlags` | `MapSelectionFlags` | Map selection screen flags |

---

## Timeline

```
None → T2A → UOR → UOTD → LBR → AOS → SE → ML → SA → HS → TOL → EJ
 0      1      2      3      4     5     6     7     8     9    10    11

 Key milestones:
 ├─ The Second Age (1):  Bushido, Tokuno, Elves, Statues
  ├─ Renaissance (2):  Spellweaving
  ├─ Third Dawn (3): Sea-themed content
  ├─ Lord Blackthorn's Revenge (4):  New maps, creatures
 ├─ AOS (5):  Mysticism, Ninjitsu, Ethics, Virtues, AOS stats system, Ilshenar/Malas/TerMur
 ├─ SE (6):   Samurai Empire refinement
 ├─ ML (7):   Quest system, bonus harvest, Parasitic/Darkglow poisons, skill cap bonus
 ├─ Stygian Abyss (8):   Gothic/Rustic housing themes
  ├─ High Seas (9):   Additional content
  ├─ Time of Legends (10): Jungle/Shadowguard themes, full Gargoyle content
  └─ Endless Journey (11):  Latest features
```

---

## Cross-References

- [Character Creation](../getting-started/character-creation.md) — race unlocks, profession restrictions by expansion
- [Movement](../getting-started/movement.md) — map availability per expansion
- [Crafting Skills](../skills/crafting-skills.md) — skill additions per expansion
- [Magical Skills](../skills/magical-skills.md) — magical skill additions (Mysticism, Ninjitsu)
- [Combat Skills](../skills/combat-skills.md) — combat skill additions (Bushido)
- [Magery Spells](../spells/magery.md) — spell school additions
- [Bushido Spells](../spells/bushido.md) — Bushido spells (T2A+)
- [Ninjitsu Spells](../spells/ninjitsu.md) — Ninjitsu spells (AOS+)
- [Mysticism Spells](../spells/mysticism.md) — Mysticism spells (AOS+)
- [Ethics](../systems/ethics.md) — ethics system (AOS+)
- [Virtues](../systems/virtues.md) — virtues system (AOS+)
- [Poisons](../systems/poisons.md) — Darkglow/Parasitic poisons (ML+)
- [Quests](../systems/quests.md) — ML quest system (ML+)
- [Veteran Rewards](../systems/veteran-rewards.md) — skill cap bonus at level 4 (ML+)
- [Harvesting](../systems/harvesting.md) — bonus harvest resources (ML+)
- [Skill Table](../reference/skill-table.md) — skill expansion associations
- [Configuration](../reference/configuration.md) — expansion-dependent config settings
