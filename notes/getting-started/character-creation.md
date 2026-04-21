# Character Creation

Character creation is the process of establishing your player character's identity, stats, skills, and starting equipment. It determines your race, profession, stat allocation, and starting gear.

## Races

ModernUO offers three playable races, each with unique visual characteristics and expansion requirements.

| Race | Required Expansion | Race Flag | Body ID (Male) | Body ID (Female) |
|---|---|---|---|---|
| Human | None | `0x1` | See game client | See game client |
| Elf | TOL (Trammel of the Lost) | `0x2` | See game client | See game client |
| Gargoyle | TOL (Trammel of the Lost) | `0x4` | See game client | See game client |

Races are defined in the `Race` abstract base class (`Race.cs`). Each race has a unique `RaceFlag` used throughout the codebase to determine race-specific behavior, such as which armor and weapons are available.

### Racial Bonuses

- **Elves** receive a Wild Staff as starting gear when playing generic professions
- **Elves** have +20 Mana on top of the base formula (ML+)
- **Gargoyles** have access to gargish armor variants (different chest/legs models)
- **Gargoyles** cannot use Archery but can use Throwing (Boomerang)
- **Humans** receive +2 to Hits regen points (ML+)

The `Race.ValidateHair()`, `Race.ValidateFacialHair()`, `Race.ClipSkinHue()`, and `Race.ClipHairHue()` methods handle race-specific appearance validation.

## Professions

Professions are loaded from `prof.txt` files in the `Data/Professions/[Expansion]/` directories. The AOS expansion defines five professions, each with preset stat allocations and starting skills.

| Profession | Stats (Str/Dex/Int) | Starting Skills | Starting Gear |
|---|---|---|---|
| **Warrior** | 35 / 35 / 10 | Tactics 50, Healing 45, Swordsmanship 5 | Studded hide, Bascinet |
| **Mage** | 25 / 10 / 45 | Magery 50, Meditation 50 | Spellbook, robes, 3 circles of scrolls |
| **Blacksmith** | 60 / 10 / 10 | Blacksmith 50, Tinkering 45, Mining 5 | Studded hide, Tongs, Pickaxes, Iron Ingots |
| **Paladin** | 30 / 25 / 25 | Chivalry 50, Tactics 50 | Profession-specific armor + Book of Chivalry |
| **Necromancer** | 25 / 20 / 45 | Necromancy 50, Swordsmanship 30, Tactics 20 | Profession-specific armor + Necro spellbook |

Professions are defined in `ProfessionInfo.cs` and loaded from `Data/Professions/AOS/prof.txt`. Each profession entry contains:
- `Name`: Display name
- `Skills`: Array of `(SkillName, byte)` tuples defining starting skill values
- `Stats`: Array of 3 bytes for Str, Dex, Int allocation
- `Gump`: Gump ID used in the character creation UI
- `TopLevel`: Whether this profession appears as a top-level choice

### Profession-Specific Starting Locations

Some professions send you to unique starting locations instead of the default city:

| Profession | Starting Location | Map | Requirement |
|---|---|---|---|
| **Necromancer** | Umbra (Mardoth's Tower) | Malas | AOS expansion |
| **Paladin** | First available starting city | Default map | None |
| **Samurai** | Haoti's Grounds | Malas | SE + AOS + client flags |
| **Ninja** | Enimo's Residence | Malas | SE + AOS + client flags |

If the required expansion or client flags are not available, the character falls back to the first available starting city and receives a warning message (1062205 for Necromancer, 1063487 for Samurai/Ninja).

## Stat Allocation

During character creation, you allocate points among three core stats: Strength, Dexterity, and Intelligence.

### Rules

- **Total points**: 90 for new character creation, 80 for legacy clients
- **Minimum per stat**: 10
- **Maximum per stat**: 60
- **Sum must equal**: the maxStats value (90 or 80)

Invalid allocations are rejected and reset to `10/10/10`.

The validation logic is in `CharacterCreation.SetStats()`:

```csharp
if (str is < 10 or > 60 || dex is < 10 or > 60 || intel is < 10 or > 60 || str + dex + intel != maxStats)
{
    str = 10; dex = 10; intel = 10;
}
```

### Stat Effects on Resources

Your stats directly determine your character's resource pools:

| Resource | Formula (AOS) | Formula (Pre-AOS) |
|---|---|---|
| **Hit Points** | `Str / 2 + 50 + BonusHits` | `RawStr / 2 + 50` |
| **Stamina** | `Dex + BonusStam` | `Dex` |
| **Mana** | `Int + BonusMana + (Elf ? 20 : 0)` | `Int` |

`BonusHits` is capped at +25 for player characters (ML+). Max stat is capped at 150 for players (ML+).

See [[stats]] for complete details on resource formulas, regeneration, and stat scaling.

## Skill Selection

After choosing your race and profession, you select which skills to specialize in.

### Rules

- **Total skill points**: 100 or 120
- **Maximum per skill**: 50
- **Maximum skills**: 4 (based on profession template)

Profession templates define the starting skill layout, which players can modify. The base game provides 70 available starting skills.

### Allowed Starting Skills

The following 70 skills are available at character creation:

Alchemy, Anatomy, AnimalLore, AnimalTaming, Archery, ArmsLore, Begging, Blacksmith, Fletching, Bushido, Camping, Carpentry, Cartography, Chivalry, Cooking, DetectHidden, Discordance, EvalInt, Fencing, Fishing, Focus, Forensics, Healing, Herding, Hiding, Imbuing, Inscribe, ItemID, Lockpicking, Lumberjacking, Macing, Magery, Meditation, Mining, Musicianship, Mysticism, Necromancy, Ninjitsu, Parry, Peacemaking, Poisoning, Provocation, MagicResist, Snooping, SpiritSpeak, Stealing, Swords, Tactics, Tailoring, TasteID, Throwing, Tinkering, Tracking, Veterinary, Wrestling

### Expansion Restrictions

Certain skills are restricted by expansion or race:

| Skill(s) | Restriction | Replacement if Invalid |
|---|---|---|
| Necromancy, Chivalry, Focus | Requires AOS | Alchemy |
| Ninjitsu, Bushido | Requires SE | Alchemy |
| Throwing, Imbuing | Requires SA | Alchemy |
| Archery | Gargoyles cannot select | Alchemy |
| Throwing | Non-Gargoyles cannot select | Alchemy |

The validation logic is in `CharacterCreation.ValidateSkills()`.

## Starting Equipment

All characters receive the following base items:

| Item | Quantity | Notes |
|---|---|---|
| Backpack | 1 | Non-movable |
| Red Book | 1 | 20 pages, titled with character name |
| Gold | 1000 | Starting currency |
| Dagger | 1 | Basic weapon |
| Candle | 1 | Lighting item |

### Profession-Specific Gear

Profession-specific items are distributed by `GiveProfessionItems()`:

**Warrior/Mage/Blacksmith**: Shirt, pants, and shoes (race-dependent).

**Paladin**:
- Human: Broadsword, Helmet, PlateGorget, RingmailArms, RingmailChest, RingmailLegs, RingmailGloves, ThighBoots, Cloak, BodySash, Book of Chivalry
- Elf: Elven Machete, Winged Helm, Leaf Gorget/Arms/Chest/Legs/Gloves, Elven Boots, Book of Chivalry
- Gargoyle: Glass Sword, Stone armor set (gargish variants), Book of Chivalry

**Necromancer**:
- Human: Bone Harvester, Leather armor set (hue 0x2C3), Skirt, Sandals, Bag of Necro Reagents, Necromancer Spellbook
- Elf: Elven Machete, Leaf armor set (hue 0x2C3), Elven Boots, Bag of Necro Reagents, Necromancer Spellbook
- Gargoyle: Glass Sword, Gargish Leather armor set (hue 0x2C3), Bag of Necro Reagents, Necromancer Spellbook

### Skill-Specific Items

When you select a skill, you may also receive associated tools and items via `AddSkillItems()`:

| Skill | Items Received |
|---|---|
| Alchemy | 4 Bottles, Mortar & Pestle, Robe |
| Anatomy | 3 Bandages, Robe |
| Animal Lore | Shepherd's Crook (human/gargoyle) or Wild Staff (elf), Robe |
| Animal Taming | Shepherd's Crook (human only) |
| Archery | 25 Arrows, race-dependent bow |
| Arms Lore | Random weapon (fencing/macing/swords type), race-dependent |
| Begging | Staff (Wild Staff/Elf, Glass Staff/Gargoyle, Gnarled Staff/Human) |
| Blacksmith | Tongs, 2 Pickaxes, 50 Iron Ingots, Half Apron |
| Bushido | Hakama, Kasa, Book of Bushido |
| Fletching | 14 Boards, 5 Feathers, 5 Shafts |
| Camping | Bedroll, 5 Kindling |
| Carpentry | 10 Boards, Saw, Half Apron (elf/human) |
| Cartography | 4 Blank Maps, Sextant |
| Cooking | 2 Kindling, Raw Lamb/Chicken/Fish, Sack of Flour, Pitcher of Water |
| Chivalry | Book of Chivalry |
| Detect Hidden | Cloak (hue 0x455, elf/human) |
| Discordance | Random musical instrument |
| Fencing | Race-dependent fencing weapon (Kryss/Leafblade/Blood Blade) |
| Fishing | Fishing Pole, hat (Circlet for elf, Floppy Hat for human) |
| Healing | 50 Bandages, Scissors |
| Herding | Shepherd's Crook |
| Hiding | Cloak (hue 0x455, elf/human) |
| Inscribe | 2 Blank Scrolls (2-page), Blue Book |
| Item ID | Staff (Wild Staff/Elf, Serpentstone Staff/Gargoyle, Gnarled Staff/Human) |
| Lockpicking | 20 Lockpicks |
| Lumberjacking | Hatchet (elf/human) or Dual Short Axes (gargoyle) |
| Macing | Race-dependent mace (Club/Diamond Mace/Disc Mace) |
| Magery | 30 Reagents, 3 circles of scrolls, Spellbook, Robe, hat (Circlet for elf, Wizard's Hat for human) |
| Mining | Pickaxe |
| Musicianship | Random musical instrument |
| Necromancy | Bag of Necro Reagents (ML+) |
| Ninjitsu | Hakama (hue 0x2C3), Kasa, Book of Ninjitsu |
| Parry | Wooden Shield (gargish variant for gargoyles) |
| Peacemaking | Random musical instrument |
| Poisoning | 2 Lesser Poison Potions |
| Provocation | Random musical instrument |
| Snooping/Stealing | 20 Lockpicks |
| Spirit Speak | Cloak (hue 0x455) |
| Swords/Tactics | Race-dependent sword (Katana/Rune Blade/Dread Sword) |
| Tailoring | Bolt of Cloth, Sewing Kit |
| Tinkering | Tinker Tools, 3 Tinker Parts (pre-AOS) |
| Tracking | Skinning Knife, boots (Elven Boots for elf, Boots for human) |
| Veterinary | 5 Bandages, Scissors |
| Wrestling | Gloves (Leather/Leaf/Gargish Leather) |
| Throwing | Boomerang (gargoyles only) |

## Young Player Bonus

Young players (marked via `Account.Young`) receive an additional `NewPlayerTicket` in their bank box during character creation.

## GM/Immortal Characters

Characters created with `AccessLevel > AccessLevel.Player` receive:
- All stats set to 100 (Str, Dex, Int)
- All skills set to 1000 (100.0 in fixed-point)
- Human race
- Blessed flag
- Staff Robe (access-level specific)

## See Also

- [[stats]] — Complete stat formulas and resource mechanics
- [[movement]] — Starting cities and maps
- [[skills/crafting-skills]] — Crafting skill details
- [[skills/combat-skills]] — Combat skill details
- [[reference/skill-table]] — Full skill stat scaling reference
