# Books

Books in ModernUO encompass readable lore books, spellbooks that store and cast spells, and runebooks that store recall destinations. Lore books provide flavor content with title/author/page system, spellbooks use a bitmask-based content system to track learned spells, and runebooks provide fast-travel via stored locations with a charge system and gump-based interface.

**Source Files:**
- `Projects/UOContent/Items/Books/BaseBook.cs` (259 lines) — core book mechanics for lore books
- `Projects/UOContent/Items/Books/BookContent.cs`, `BookPackets.cs`, `BookPageInfo.cs` — book data structures and network packets
- `Projects/UOContent/Items/Books/BlueBook.cs`, `BrownBook.cs`, `RedBook.cs`, `TanBook.cs` — lore book variants
- `Projects/UOContent/Items/Books/Defined/` — predefined lore books with `DefaultContent`
- `Projects/UOContent/Items/Skill Items/Magical/Spellbook.cs` (883 lines) — spellbook base class
- `Projects/UOContent/Items/Skill Items/Magical/BookOfBushido.cs`, `BookOfChivalry.cs`, `BookOfNinjitsu.cs` — specialty spellbooks
- `Projects/UOContent/Items/Skill Items/Magical/MysticSpellbook.cs`, `NecromancerSpellbook.cs` — expansion spellbooks
- `Projects/UOContent/Items/Skill Items/Magical/Runebook.cs` — runebook teleportation system
- `Projects/UOContent/Gumps/RunebookGump.cs` — runebook gump interface

---

## Book Types

There are **3 categories** of books in ModernUO:

### Lore Books

Readable books with title, author, and page content. Players can double-click to read, and writable books allow players to edit the title, author, and page content via client packets. There are **4 lore book variants**, each with a distinct item ID:

| Lore Book Type | ItemID | Default Pages | Writable | Description |
|----------------|--------|--------------|----------|-------------|
| `TanBook` | 0xFF0 | 20 | true | Tan/parchment colored book |
| `RedBook` | 0xFF1 | 20 | true | Red-covered book |
| `BlueBook` | 0xFF2 | 40 | true | Blue-covered book, double page count |
| `BrownBook` | 0xFEF | 20 | true | Brown-covered book |

Each variant inherits from `BaseBook` and differs only in item ID and default page count (BlueBook defaults to 40 pages instead of 20).

### Spellbooks

Books that store spells and enable casting. Each spellbook type is associated with a specific spell school and uses a bitmask (`ulong`) to track which spells are learned. Spells are added by dragging spell scrolls onto the book. There are **7 spellbook types**:

| Spellbook Class | SpellbookType | ItemID | BookOffset | BookCount | Default Content | Layer |
|-----------------|--------------|--------|------------|-----------|----------------|-------|
| `Spellbook` | `Regular` | 0x2251 | 0 | 64 | 0 (empty) | OneHanded (ML+), Invalid (pre-ML) |
| `NecromancerSpellbook` | `Necromancer` | 0x2253 | 100 | 16–17¹ | 0 (empty) | OneHanded (ML+), Invalid (pre-ML) |
| `BookOfChivalry` | `Paladin` | 0x2252 | 200 | 10 | 0x3FF (all) | OneHanded (ML+), Invalid (pre-ML) |
| `BookOfBushido` | `Samurai` | 0x238C | 400 | 6 | 0x3F (all) | OneHanded (ML+), Invalid (pre-ML) |
| `BookOfNinjitsu` | `Ninja` | 0x23A0 | 500 | 8 | 0xFF (all) | OneHanded (ML+), Invalid (pre-ML) |
| `MysticSpellbook` | `Mystic` | 0x2D9D | 677 | 16 | 0 (empty) | OneHanded |
| `BookOfArcanist` | `Arcanist` | — | 600 | 17 | — | — |

¹ Necromancer has 17 spells in SE+ expansion, 16 in pre-SE.

Spellbooks use a content bitmask where each bit represents a spell. The `BookOffset` defines the starting spell ID, and `BookCount` defines the number of bits. For example, Bushido spells have offset 400 and count 6, meaning bits 0–5 correspond to spell IDs 400–405.

### Runebooks

Special books that store recall rune locations for fast teleportation. Runebooks do **not** inherit from `Spellbook` — they inherit directly from `Item` and implement `ISecurable` and `ICraftable`. Each runebook stores up to 16 `RunebookEntry` items, each referencing a location (Point3D + Map) and optionally a house.

---

## Core Book Classes

### BaseBook

The base class for lore books, implementing `ISecurable`. Manages title, author, writability, and page content.

**Serialized Fields (version 5):**

| Field | Index | Type | Purpose |
|-------|-------|------|---------|
| `_level` | 0 | `SecureLevel` | Security/access level (ignored on dupe) |
| `_title` | 1 | `string` | Book title, interned; defaults to `DefaultContent.Title` |
| `_author` | 2 | `string` | Book author; defaults to `DefaultContent.Author` |
| `_writable` | 3 | `bool` | Whether players can edit the book |
| `_pages` | 4 | `BookPageInfo[]` | Page content array (ignored on dupe, deep-copied in `OnAfterDuped`) |

**Key Properties:**
- `PagesCount` — number of pages (`_pages.Length`)
- `DefaultContent` — virtual override point for predefined book content
- `ContentAsString` — concatenates all page lines with newlines
- `ContentAsStringArray` — flattens all page lines into a pooled `string[]`

**Key Methods:**
- `OnDoubleClick(Mobile)` — auto-names blank writable books ("a book" + player name), then sends cover (`SendBookCover`, packet 0xD4) and content (`SendBookContent`, packet 0x66) packets to the client
- `OnSingleClick(Mobile)` — labels the book with "Title by Author" and "[N pages]"
- `OnAfterDuped(Item)` — deep-copies all page content to the duped book
- `AddNameProperty(IPropertyList)` — displays the title in the property list if non-empty
- `GetContextMenuEntries(Mobile, ref PooledRefList<ContextMenuEntry>)` — adds secure level context menu entry

**Constructors:**

| Constructor | Parameters | Purpose |
|-------------|------------|---------|
| `BaseBook(int itemID, int pageCount, bool writable)` | `itemID`, `pageCount` (default 20), `writable` (default true) | Convenience constructor, chains to 5-param with null title/author |
| `BaseBook(int itemID, string title, string author, int pageCount, bool writable)` | Full parameter set | Full constructor; title/author default from `DefaultContent` if non-null |
| `BaseBook(int itemID, bool writable)` | `itemID`, `writable` | For predefined/defined books only; uses 0 for pageCount (pages come from `DefaultContent`) |

### BookContent

A standalone class (not a subclass) that holds predefined book content. Used by `DefaultContent` override in defined books.

| Property | Type | Description |
|----------|------|-------------|
| `Title` | `string` | Book title |
| `Author` | `string` | Book author |
| `Pages` | `BookPageInfo[]` | Predefined page content |

| Method | Description |
|--------|-------------|
| `Copy()` | Creates a copy of the pages array with new `BookPageInfo` instances (line arrays shared) |
| `IsMatch(BookPageInfo[])` | Deep-compares two page arrays for structural and content equality |

### BookPageInfo

Represents a single page of a book.

| Property | Type | Description |
|----------|------|-------------|
| `Lines` | `string[]` | Array of up to 8 text lines per page |

Each `BookPageInfo` is serialized as: length prefix + N interned strings. The protocol limits pages to **8 lines** with **80 characters per line**.

### BookPackets

Static utility class that handles all book-related network packets. Registers 3 incoming packet handlers in `Configure()`:

| Packet ID | Direction | Purpose | Limits |
|-----------|-----------|---------|--------|
| `0xD4` | In/Out | Book header (title/author) — modern UTF-8 | Title: max 60 chars, Author: max 30 chars |
| `0x93` | In only | Book header — legacy Latin-1 | Title: max 60 chars, Author: max 30 chars |
| `0x66` | In/Out | Book content (page data) | Max 8 lines/page, max 80 chars/line, 1-indexed pages |

**Write restrictions** (all 3 packet handlers):
- Book must be writable (`book.Writable`)
- Player must be adjacent to the book (`InRange(book.GetWorldLocation(), 1)`)
- Book must be accessible (`book.IsAccessibleTo(from)`)

All text passes through `FixHtml()` for sanitization.

---

## Page System

Books contain an array of `BookPageInfo` objects, each with a `Lines` string array.

**Protocol limits:**
- **8 lines** maximum per page
- **80 characters** maximum per line
- **60 characters** maximum for title
- **30 characters** maximum for author
- Page indices are **1-based** in the protocol (converted to 0-based internally)

**Practical capacity:** Approximately 24 full pages at full unicode, or 96 full pages at ASCII-only (limited by packet size).

**Empty pages** have empty `Lines` arrays, initialized by the parameterless `BookPageInfo()` constructor.

**String interning** — all strings read from serialization use `.Intern()` for memory efficiency.

---

## Defined Books (DefaultContent Pattern)

Predefined lore books with static content override `DefaultContent` to return a `BookContent` instance. This pattern sets the title, author, and page content at construction time. When a defined book is created, the constructor chains to `BaseBook(int itemID, bool writable)` which uses 0 as pageCount and initializes pages from `DefaultContent.Copy()`.

Serialization uses save flags to detect whether title, author, and content differ from `DefaultContent`. If they match, the default values are used instead of serializing redundant data. The `ShouldSerialize*()` and `*DefaultValue()` methods handle this:

```csharp
// Example from BlackthornWelcomeBook (RedBook variant)
public static readonly BookContent Content = new(
    "A Welcome",
    "Lord Blackthorn",
    new BookPageInfo("  Greetings to you,", "new member of the", ...),
    new BookPageInfo("Britannia's defenders.", ...),
    // ... more pages
);

public override BookContent DefaultContent => Content;
```

Defined books found in `Projects/UOContent/Items/Books/Defined/`:
- `BlackthornWelcomeBook` (RedBook) — 12 pages, opening quest book
- `DrakovsJournal` (BrownBook) — journal
- `FropozJournal` (BrownBook) — journal
- `KaburJournal` (BrownBook) — journal
- `NewAquariumBook` (TanBook) — aquarium description
- `TranslatedGargoyleJournal` (BrownBook) — lore
- `LibraryBooks` — multiple lore books (Blue, Brown, Red, Tan variants)

---

## Security

All books implement `ISecurable` via the `_level` field of type `SecureLevel`. The `_level` field is marked `[SerializedIgnoreDupe]`, meaning it is reset on duplication.

**Context menu integration:** `GetContextMenuEntries` adds `SetSecureLevelEntry` to the context menu, allowing secure level changes (CoOwner, Owner, etc.).

**Access checks for writing:** All packet handlers verify `book.IsAccessibleTo(from)` before allowing modifications.

**Runebook-specific access:** `Runebook.CheckAccess(Mobile)` additionally checks house lockdown permissions — locked down runebooks can only be accessed by the lockdown owner or GMs.

---

## Duplication

Books support duplication via `OnAfterDuped`, which creates deep copies of page content:

**BaseBook duplication:**
- Creates fresh `BookPageInfo` instances for each page
- Deep-copies all line strings to the duped book
- `_level` and `_pages` are not serialized (marked `[SerializedIgnoreDupe]`) and are regenerated

**Runebook duplication:**
- Creates new `RunebookEntry` instances for each entry
- Preserves location, map, description, and house references
- `_level`, `_entries`, and `_curCharges` are not serialized

---

## Spellbook Mechanics

### Content Bitmask

Spellbooks use a `ulong` (`Content` field) as a bitmask where each bit represents whether a spell is learned. The `BookOffset` defines the starting spell ID, and `BookCount` defines the number of valid bits.

**Example — BookOfBushido:**
- `BookOffset = 400`, `BookCount = 6`
- Default content `0x3F` = binary `111111` = all 6 Bushido spells learned
- Spell ID 400 corresponds to bit 0, spell ID 405 to bit 5

**Example — BookOfChivalry:**
- `BookOffset = 200`, `BookCount = 10`
- Default content `0x3FF` = binary `1111111111` = all 10 Chivalry spells learned

### Spell Addition

Players add spells by dragging `SpellScroll` items onto the spellbook. The `OnDragDrop` method validates that the scroll's spell type matches the book's `SpellbookType` before adding. The spell bit is set in the `Content` bitmask.

### Casting

Spellbooks enable casting through `CastSpellRequest()` and `TargetedSpell()` methods. The player opens a spell selection gump, selects a spell, and the system checks `HasSpell(spellID)` (bit check after adjusting for `BookOffset`) before allowing the cast.

### Crafting Integration

`Spellbook.OnCraft()` applies runic attributes based on the crafter's Magery skill:

| Magery Skill | Properties Applied |
|--------------|-------------------|
| Adept (60.1+) | 0–1 properties |
| Master (70.1+) | 0–2 properties |
| Grand Master (80.1+) | 0–3 properties |
| Elder (90.1+) | 0–3 properties |
| Legend (100.0+) | 0–3 properties |

Property distributions are defined in static arrays (`_adeptPropertyCounts`, `_masterPropertyCounts`, etc.) with weighted random selection.

### Find Methods

Static factory methods locate spellbooks on a mobile:
- `Spellbook.Find(Mobile, SpellbookType)` — find a specific type
- `Spellbook.FindRegular(Mobile)`, `FindNecromancer(Mobile)`, `FindPaladin(Mobile)`, `FindSamurai(Mobile)`, `FindNinja(Mobile)`, `FindArcanist(Mobile)`, `FindMystic(Mobile)` — find by type
- `Spellbook.FindRegular(Mobile)` — find any spellbook

---

## Runebook Mechanics

### Structure

Runebooks store up to **16 `RunebookEntry` items** in a `List<RunebookEntry>`. Each entry contains:

| Field | Type | Serialization (version 2) |
|-------|------|--------------------------|
| `_house` | `BaseHouse` | V1: house reference; V0: conditional on house not deleted |
| `_location` | `Point3D` | V0: conditional on house deleted |
| `_map` | `Map` | V0: conditional on house deleted |
| `_description` | `string` | V0: conditional on house deleted |

### Charges

Runebooks have a **charge system** that limits usage:

| Property | Default | Notes |
|----------|---------|-------|
| `_maxCharges` | 6 (pre-SE) / 12 (SE+) | Set at construction |
| `_curCharges` | 0 | Current available charges |

Charges are replenished by dragging `RecallScroll` items onto the runebook. The amount consumed equals `min(recallScroll.Amount, maxCharges - curCharges)`.

### Crafting — Charge Formula

`OnCraft` calculates charges based on quality and Inscribe skill:

```
charges = Min(5 + quality + (int)(Inscribe.Value / 30), 10)
SE+: charges = charges * 2
```

Quality ranges from 0–2 (Low=0, Regular=1, Exceptional=2). Exceptional runebooks effectively get 4× the base charges in SE+.

### Adding Rune Entries

Players add locations by dragging **marked** `RecallRune` items onto the runebook:
- Max 16 entries
- The rune item is consumed (deleted)
- The entry stores the rune's target location, map, description, and house

### Dropping Runes

`DropRune(Mobile, RunebookEntry, int)` removes an entry and returns it as a `RecallRune` item in the player's backpack. Adjusts `_defaultIndex` if the dropped entry was before or at the default position.

### Travel Cooldown

After using a runebook to travel, there is a **7-second cooldown** (`UseDelay`). In SA+ expansion, this cooldown is disabled.

### Gump Interface

`OnDoubleClick` opens `RunebookGump` if the player is in range (1 tile, or 3 in ML+) and has access. The gump shows stored locations and allows teleportation. The `Openers` HashSet tracks which mobiles have the gump open, preventing concurrent access.

### Security

Runebooks implement `ISecurable` with `SecureLevel` and integrate with house lockdown:
- Locked down runebooks can only be accessed by the lockdown owner or GMs
- `CheckAccess()` verifies lockdown and secure access permissions

---

## Enums

### SpellbookType

| Value | Name | Spellbook Class | Offset | Count |
|-------|------|----------------|--------|-------|
| -1 | `Invalid` | — | — | — |
| 0 | `Regular` | `Spellbook` | 0 | 64 |
| 1 | `Necromancer` | `NecromancerSpellbook` | 100 | 16–17 |
| 2 | `Paladin` | `BookOfChivalry` | 200 | 10 |
| 3 | `Ninja` | `BookOfNinjitsu` | 500 | 8 |
| 4 | `Samurai` | `BookOfBushido` | 400 | 6 |
| 5 | `Arcanist` | `BookOfArcanist` | 600 | 17 |
| 6 | `Mystic` | `MysticSpellbook` | 677 | 16 |

### BookQuality

| Value | Name | Description |
|-------|------|-------------|
| 0 | `Regular` | Standard quality |
| 1 | `Exceptional` | Doubled uses/durability (applies to runebooks) |

### Spell ID Ranges

| SpellbookType | Offset | Count | Spell ID Range |
|--------------|--------|-------|----------------|
| Regular | 0 | 64 | 0–63 |
| Necromancer | 100 | 16–17 | 100–116 |
| Paladin (Chivalry) | 200 | 10 | 200–209 |
| Samurai (Bushido) | 400 | 6 | 400–405 |
| Ninja (Ninjitsu) | 500 | 8 | 500–507 |
| Arcanist | 600 | 17 | 600–616 |
| Mystic | 677 | 16 | 677–692 |

---

## String Length Limits

| Field | Max Characters | Notes |
|-------|---------------|-------|
| Title | 60 | Both UTF-8 and Latin-1 handlers |
| Author | 30 | Both UTF-8 and Latin-1 handlers |
| Lines per page | 8 | Hard limit in content packet handler |
| Characters per line | 80 | Hard limit in content packet handler |

---

## Cross-References

- `spells/magery.md` — spell schools and spellcasting system
- `spells/chivalry.md` — Paladin spell list (BookOfChivalry)
- `spells/bushido.md` — Samurai spell list (BookOfBushido)
- `spells/ninjitsu.md` — Ninja spell list (BookOfNinjitsu)
- `spells/necromancy.md` — Necromancy spell list (NecromancerSpellbook)
- `spells/mysticism.md` — Mystic spell list (MysticSpellbook)
- `systems/crafting.md` — Inscription skill for inscribing spells, runic tools
- `reference/skill-table.md` — Inscription skill associations
