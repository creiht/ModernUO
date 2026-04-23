# Housing

The housing system in ModernUO allows players to purchase, design, and maintain homes in Britannia. Houses provide secure storage, vendor placement, and social spaces. The system includes lockdown and secure item management, co-owner permissions, friend/ban lists, and house decay mechanics.

**Source Files:**
- `Projects/UOContent/Multis/Houses/BaseHouse.cs` — Core house class, ownership, permissions, storage limits
- `Projects/UOContent/Multis/Houses/HouseFoundation.cs` — House design and construction
- `Projects/UOContent/Multis/Houses/HousePlacement.cs` — House placement and validation
- `Projects/UOContent/Gumps/Houses/` — House-related gumps

---

## House Types

ModernUO supports multiple house foundation types, each with different visual styles and capabilities:

| Foundation Type | Description |
|----------------|-------------|
| Stone | Classic stone house foundation |
| DarkWood | Dark wood foundation |
| LightWood | Light wood foundation |
| Dungeon | Dungeon-style foundation |
| Brick | Brick foundation |
| ElvenGrey | Grey elven architecture |
| ElvenNatural | Natural elven architecture |
| Crystal | Crystal-themed foundation |
| Shadow | Shadow-themed foundation |

---

## Ownership and Permissions

### Owner

The house owner has full control over the property, including:

- Adding/removing co-owners
- Setting friend and ban lists
- Access control for non-co-owners
- Trading or deeding the house

### Co-Owners

| Limit | Base Game | AOS+ |
|-------|-----------|------|
| Max Co-Owners | 15 | 15 |

Co-owners can:
- Access and move locked-down items
- Access secure containers
- Invite friends and ban players
- Modify house design (with foundation)

### Friends and Bans

| Setting | Base Game | AOS+ |
|---------|-----------|------|
| Max Friends | 50 | 140 |
| Max Bans | 50 | 140 |

Friends can enter the house regardless of access settings. Bans prevent specific players from entering.

---

## Storage Limits

### Lockdown

Lockdown prevents items from being moved by anyone except the owner and co-owners.

```
MaxLockDowns = house_size_based
```

### Secure Storage

Secure containers can only be opened by the owner and co-owners. Items placed in secure containers cannot be stolen by other players.

```
MaxSecures = house_size_based
```

### Internalized Vendors

| Limit | Base Game | AOS+ |
|-------|-----------|------|
| Max Barkeeps | 2 | 2 |

Vendor rental contracts allow NPC barkeepers to operate within the house, generating gold revenue.

---

## House Decay

Houses can decay over time if not maintained. The decay system has multiple stages:

| Decay Stage | Effect |
|------------|--------|
| Stage 1 | Items begin to deteriorate |
| Stage 2 | Further deterioration |
| Stage 3 | Final stage before collapse |

Decay can be enabled or disabled via configuration:

```csharp
public static bool DecayEnabled { get; set; }
```

---

## House Design

### Foundation Design

House foundations support multi-level construction with:

- Room addition/removal
- Stair sectioning (AOS+)
- Wall and floor customization
- Window placement

### Stair Block IDs

The stair system supports proper N-W-S-E sequencing:

```
Stair sequence IDs: 0x3EF, 0x70A, 0x722, 0x739,
                    0x751, 0x76D, 0x789, 0x7A4
```

Block IDs for stair sections:
```
0x3EE, 0x709, 0x71E, 0x721, 0x738, 0x750, 0x76C, 0x788,
0x7A3, 0x7BA, 0x35D2, 0x3609, 0x4317, 0x4318, 0x4B07, 0x7807
```

### House Placement

House placement requires:
- Valid terrain (not on water, mountains, etc.)
- Sufficient space for the chosen size
- No existing structures in the placement area
- Ownership verification

---

## Special Features

### House Teleporters

Houses can be equipped with teleporter runes for quick travel between owned properties.

### House Signs

Each house displays a sign showing:
- House name
- Owner name
- Description (if set)

### House Raffle

The House Raffle system allows players to raffle houses through the `Projects/UOContent/Items/Special/House Raffle/` directory.

### Contest Houses

Contest houses are special properties used for housing design competitions, managed through `ContestHouses.cs`.

### Preview Mode

Preview houses allow players to design houses before purchasing, using the `PreviewHouse.cs` system.

---

## Moving and Deeding

### Moving Crates

Houses can be packed into moving crates for relocation:

```
MovingCrate.cs — Packs house into portable crate
```

### Deeding

Houses can be deeded (transferred) to other players through the deed system.

---

## Expansion Notes

| Feature | Expansion |
|---------|-----------|
| Base housing | SE (Samurai Empire) |
| Co-owners | SE |
| Friends/Bans (140 limit) | AOS |
| Stair sectioning | AOS+ |
| Vendor system improvements | AOS |
| Preview house design | ML+ |

---

## Cross-References

- [Systems: Containers](../items/containers.md) — Lockdown, secure storage mechanics
- [Systems: Crafting](../systems/crafting.md) — Deeds and house construction
- [Items: Containers](../items/containers.md) — Secure storage details
