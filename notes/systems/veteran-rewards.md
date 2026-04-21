# Veteran Rewards

Veteran Rewards is an account age-based progression system that grants long-time players access to exclusive cosmetic and convenience items. ModernUO implements **12 reward levels** spanning **6 categories** of items, with rewards unlocking at configurable intervals (default 30 days). The system also provides skill cap bonuses (up to +200 at level 4, total 7200) and stat cap bonuses (+5) for veteran accounts. Items are granted as deeds or consumables placed directly into the player's backpack, and all reward items are flagged with `IRewardItem.IsRewardItem` to enable account-age validation on use.

**Source Files:**
- `Projects/UOContent/Engines/Veteran Rewards/RewardSystem.cs` (607 lines) — core system, config, access checking, reward tables, login hook
- `Projects/UOContent/Engines/Veteran Rewards/RewardCategory.cs` (25 lines) — category container with cliloc name and entry list
- `Projects/UOContent/Engines/Veteran Rewards/RewardEntry.cs` (88 lines) — individual reward definition with type and args
- `Projects/UOContent/Engines/Veteran Rewards/RewardList.cs` (22 lines) — reward year/tier with age requirement
- `Projects/UOContent/Engines/Veteran Rewards/Gumps/RewardChoiceGump.cs` (220 lines) — main reward selection gump
- `Projects/UOContent/Engines/Veteran Rewards/Gumps/RewardConfirmGump.cs` (91 lines) — confirmation dialog
- `Projects/UOContent/Engines/Veteran Rewards/Gumps/RewardNoticeGump.cs` (40 lines) — login notification popup
- `Projects/UOContent/Engines/Veteran Rewards/Gumps/RewardDemolitionGump.cs` (79 lines) — house addon re-deed gump
- `Projects/UOContent/Engines/Veteran Rewards/Gumps/RewardOptionGump.cs` (101 lines) — generic option selector gump
- `Projects/UOContent/Engines/Veteran Rewards/Character Statue Maker/CharacterStatue.cs` (653 lines) — statue mobile, deed, and targeting
- `Projects/UOContent/Engines/Veteran Rewards/Character Statue Maker/CharacterStatueMaker.cs` (113 lines) — statue placement item (3 variants)
- `Projects/UOContent/Engines/Veteran Rewards/Character Statue Maker/CharacterStatuePackets.cs` (60 lines) — custom animation packet (0xBF 0x19, 17 bytes)
- `Projects/UOContent/Engines/Veteran Rewards/Character Statue Maker/CharacterStatuePlinth.cs` (128 lines) — statue pedestal static
- `Projects/UOContent/Engines/Veteran Rewards/Character Statue Maker/Gumps/CharacterStatueGump.cs` (207 lines) — statue customization gump

---

## Core Engine (`RewardSystem`)

The core system is a static class `RewardSystem` that manages all reward logic. Configuration is read from `vetRewards.*` settings at server startup. Reward tables are lazily initialized via `SetupRewardTables()` on first access to `Categories` or `Lists`.

### Configuration

| Setting | Key | Default | Description |
|---------|-----|---------|-------------|
| `Enabled` | `vetRewards.enable` | `true` | Master toggle for the entire system |
| `SkillCapRewards` | `vetRewards.skillCapRewards` | `true` | Whether skill cap increases are awarded |
| `RewardInterval` | `vetRewards.rewardInterval` | `30 days` | Time required per reward level |

### Core Properties

| Property | Type | Description |
|----------|------|-------------|
| `RewardSystem.Enabled` | `bool` | Whether the system is active |
| `RewardSystem.SkillCapRewards` | `bool` | Whether skill cap bonuses are granted |
| `RewardSystem.RewardInterval` | `TimeSpan` | Time per reward level |
| `RewardSystem.Categories` | `RewardCategory[]` | 6 categories, lazily initialized |
| `RewardSystem.Lists` | `RewardList[]` | 12 reward levels (years 1–12), lazily initialized |

### Reward Level Calculation

```
Reward Level = floor(accountAge.TotalDays / RewardInterval.TotalDays)
```

The level is clamped to 0 minimum. A "half-level" check exists for the stat cap bonus at ML+ (account age >= RewardInterval / 2).

### Max Rewards Formula

```
Level 0-5: max = 2 + level
Level 6+:  max = 9 + (level - 6) * 2
```

| Level | Max Rewards | Cumulative |
|-------|------------|------------|
| 0 | 0 | 0 |
| 1 | 3 | 3 |
| 2 | 4 | 7 |
| 3 | 5 | 12 |
| 4 | 6 | 18 |
| 5 | 7 | 25 |
| 6 | 9 | 34 |
| 7 | 11 | 45 |
| 8 | 13 | 58 |
| 9 | 15 | 73 |
| 10 | 17 | 90 |
| 11 | 19 | 109 |
| 12 | 21 | 130 |

### Account Tag Tracking

Used rewards are tracked via the account tag `numRewardsChosen` (stored as string). The tag is incremented via `ConsumeRewardPoint()`. GMs are exempt from point consumption.

---

## Access Checking

Two methods determine whether a player can access rewards:

| Method | Return | Description |
|--------|--------|-------------|
| `HasAccess(Mobile, RewardCategory)` | `bool` | Returns true if any entry in the category is accessible |
| `HasAccess(Mobile, RewardEntry)` | `bool` | Checks expansion + account age for single entry |
| `HasAccess(Mobile, RewardList, out TimeSpan)` | `bool` | Checks account age, returns remaining wait time |
| `HasHalfLevel(Mobile)` | `bool` | Checks if account is at least half a reward interval old |

`CheckIsUsableBy(Mobile, Item, object[])` validates whether a player can use a specific reward item. It iterates all reward lists to find a matching entry. For `DyeTub` and `MonsterStatuette` types, relaxed rules apply: items from level 1 (index 0) can be used regardless of account age. If access is denied, a localized message is sent with months remaining.

### `IRewardItem` Interface

```csharp
public interface IRewardItem
{
    bool IsRewardItem { get; set; }
}
```

All constructed reward items have `IsRewardItem` set to `true` via `RewardEntry.Construct()`. This interface is used by `CheckIsUsableBy()` to identify reward items.

---

## Login Hook

The `[OnEvent(nameof(PlayerMobile.PlayerLoginEvent))]` attribute on `OnLogin(PlayerMobile)` triggers on every player login:

```
1. Skip if system disabled or player not alive
2. Compute reward info (current/Max/Level)
3. Skill cap handling:
   a. If SkillsCap is 7000-7200, clamp level to 0-4
   b. If SkillCapRewards enabled: SkillsCap = 7000 + level * 50
   c. Otherwise: SkillsCap = 7000
4. Stat cap bonus (ML only):
   a. If HasStatReward is false and HasHalfLevel is true:
      - Set HasStatReward = true
      - StatCap += 5
5. If cur < max, show RewardNoticeGump
```

---

## Reward Categories

Six categories are defined by their ItemID (used as gump background icons). Each category has a cliloc name and a list of reward entries.

| Category | Cliloc ID | ItemID | Levels | Entry Count |
|----------|-----------|--------|--------|-------------|
| Monster Statues | 1049750 | 1049750 | 1, 3, 4, 5, 10 | 24 total |
| Cloaks and Robes | 1049752 | 1049752 | 1-5, 10 | 36+ total |
| Ethereal Steeds | 1049751 | 1049751 | 3-6, 9-12 | 16 total |
| Special Dye Tubs | 1049753 | 1049753 | 1-5 | 6 total |
| House AddOns | 1049754 | 1049754 | 1-10 | 20 total |
| Miscellaneous | 1078596 | 1078596 | 1, 8 | 2 total |

---

## Reward Levels — Complete Item Listing

### Level 1 (Bronze) — 1 Month

**Special Dye Tubs** (3 items):
| Cliloc | Item Type |
|--------|-----------|
| 1006008 | RewardBlackDyeTub |
| 1006013 | FurnitureDyeTub |
| 1006047 | SpecialDyeTub |

**Cloaks and Robes** (9 items):
| Cliloc | Item Type | Hue | Hue Color | Expansion |
|--------|-----------|-----|-----------|-----------|
| 1006009 | RewardCloak | 0x972 | Bronze | Base |
| 1006010 | RewardRobe | 0x972 | Bronze | Base |
| 1080366 | RewardDress | 0x972 | Bronze | ML |
| 1006011 | RewardCloak | 0x96D | Copper | Base |
| 1006012 | RewardRobe | 0x96D | Copper | Base |
| 1080367 | RewardDress | 0x96D | Copper | ML |

**Monster Statues** (12 items):
| Cliloc | Monster Type |
|--------|-------------|
| 1006024 | Crocodile |
| 1006025 | Daemon |
| 1006026 | Dragon |
| 1006027 | EarthElemental |
| 1006028 | Ettin |
| 1006029 | Gargoyle |
| 1006030 | Gorilla |
| 1006031 | Lich |
| 1006032 | Lizardman |
| 1006033 | Ogre |
| 1006034 | Orc |
| 1006035 | Ratman |
| 1006036 | Skeleton |
| 1006037 | Troll |

**House AddOns** (2 items):
| Cliloc | Item Type | Expansion |
|--------|-----------|-----------|
| 1062692 | ContestMiniHouseDeed | AOS |
| 1072216 | ContestMiniHouseDeed | SE |

**Miscellaneous** (2 items):
| Cliloc | Item Type | Expansion |
|--------|-----------|-----------|
| 1076155 | RedSoulstone | ML |
| 1080523 | CommodityDeedBox | ML |

### Level 2 (Copper) — 2 Months

**Special Dye Tubs** (1 item):
| Cliloc | Item Type |
|--------|-----------|
| 1006052 | LeatherDyeTub |

**Cloaks and Robes** (6 items):
| Cliloc | Item Type | Hue | Hue Color | Expansion |
|--------|-----------|-----|-----------|-----------|
| 1006014 | RewardCloak | 0x979 | Agapite | Base |
| 1006015 | RewardRobe | 0x979 | Agapite | Base |
| 1080369 | RewardDress | 0x979 | Agapite | ML |
| 1006016 | RewardCloak | 0x8A5 | Golden | Base |
| 1006017 | RewardRobe | 0x8A5 | Golden | Base |
| 1080368 | RewardDress | 0x8A5 | Golden | ML |

**House AddOns** (3 items):
| Cliloc | Item Type | Expansion |
|--------|-----------|-----------|
| 1006048 | BannerDeed | Base |
| 1006049 | FlamingHeadDeed | Base |
| 1080409 | MinotaurStatueDeed | ML |

### Level 3 (Agapite) — 3 Months

**Cloaks and Robes** (6 items):
| Cliloc | Item Type | Hue | Hue Color | Expansion |
|--------|-----------|-----|-----------|-----------|
| 1006020 | RewardCloak | 0x89F | Verite | Base |
| 1006021 | RewardRobe | 0x89F | Verite | Base |
| 1080370 | RewardDress | 0x89F | Verite | ML |
| 1006022 | RewardCloak | 0x8AB | Valorite | Base |
| 1006023 | RewardRobe | 0x8AB | Valorite | Base |
| 1080371 | RewardDress | 0x8AB | Valorite | ML |

**Monster Statues** (3 items):
| Cliloc | Monster Type |
|--------|-------------|
| 1006038 | Cow |
| 1006039 | Zombie |
| 1006040 | Llama |

**Ethereal Steeds** (3 items):
| Cliloc | Item Type |
|--------|-----------|
| 1006019 | EtherealHorse |
| 1006050 | EtherealOstard |
| 1006051 | EtherealLlama |

**House AddOns** (1 item):
| Cliloc | Item Type | Expansion |
|--------|-----------|-----------|
| 1080407 | PottedCactusDeed | ML |

### Level 4 (Verite) — 4 Months

**Special Dye Tubs** (1 item):
| Cliloc | Item Type |
|--------|-----------|
| 1049740 | RunebookDyeTub |

**Cloaks and Robes** (9 items):
| Cliloc | Item Type | Hue | Hue Color | Expansion |
|--------|-----------|-----|-----------|-----------|
| 1049725 | RewardCloak | 0x497 | DarkGray | Base |
| 1049726 | RewardRobe | 0x497 | DarkGray | Base |
| 1080374 | RewardDress | 0x497 | DarkGray | ML |
| 1049727 | RewardCloak | 0x47F | IceGreen | Base |
| 1049728 | RewardRobe | 0x47F | IceGreen | Base |
| 1080372 | RewardDress | 0x47F | IceGreen | ML |
| 1049729 | RewardCloak | 0x482 | IceBlue | Base |
| 1049730 | RewardRobe | 0x482 | IceBlue | Base |
| 1080373 | RewardDress | 0x482 | IceBlue | ML |

**Monster Statues** (3 items):
| Cliloc | Monster Type |
|--------|-------------|
| 1049742 | Ophidian |
| 1049743 | Reaper |
| 1049744 | Mongbat |

**Ethereal Steeds** (3 items):
| Cliloc | Item Type |
|--------|-----------|
| 1049746 | EtherealKirin |
| 1049745 | EtherealUnicorn |
| 1049747 | EtherealRidgeback |

**House AddOns** (2 items):
| Cliloc | Item Type | Expansion |
|--------|-----------|-----------|
| 1049737 | DecorativeShieldDeed | Base |
| 1049738 | HangingSkeletonDeed | Base |

### Level 5 (Valorite) — 5 Months

**Special Dye Tubs** (1 item):
| Cliloc | Item Type |
|--------|-----------|
| 1049741 | StatuetteDyeTub |

**Cloaks and Robes** (9 items):
| Cliloc | Item Type | Hue | Hue Color | Expansion |
|--------|-----------|-----|-----------|-----------|
| 1049731 | RewardCloak | 0x001 | JetBlack | Base |
| 1049732 | RewardRobe | 0x001 | JetBlack | Base |
| 1080377 | RewardDress | 0x001 | JetBlack | ML |
| 1049733 | RewardCloak | 0x47E | IceWhite | Base |
| 1049734 | RewardRobe | 0x47E | IceWhite | Base |
| 1080376 | RewardDress | 0x47E | IceWhite | ML |
| 1049735 | RewardCloak | 0x489 | Fire | Base |
| 1049736 | RewardRobe | 0x489 | Fire | Base |
| 1080375 | RewardDress | 0x489 | Fire | ML |

**Monster Statues** (3 items):
| Cliloc | Monster Type |
|--------|-------------|
| 1049768 | Gazer |
| 1049769 | FireElemental |
| 1049770 | Wolf |

**Ethereal Steeds** (2 items):
| Cliloc | Item Type |
|--------|-----------|
| 1049749 | EtherealSwampDragon |
| 1049748 | EtherealBeetle |

**House AddOns** (2 items):
| Cliloc | Item Type | Expansion |
|--------|-----------|-----------|
| 1049739 | StoneAnkhDeed | Base |
| 1080384 | BloodyPentagramDeed | ML |

### Level 6 — 6 Months

**House AddOns** (4 items, all ML):
| Cliloc | Item Type | StatueType |
|--------|-----------|------------|
| 1076188 | CharacterStatueMaker | Jade |
| 1076189 | CharacterStatueMaker | Marble |
| 1076190 | CharacterStatueMaker | Bronze |
| 1080527 | RewardBrazierDeed | — |

### Level 7 — 7 Months

**House AddOns** (2 items, all ML):
| Cliloc | Item Type |
|--------|-----------|
| 1076157 | CannonDeed |
| 1080550 | TreeStumpDeed |

### Level 8 — 8 Months

**Miscellaneous** (1 item, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1076158 | WeaponEngravingTool |

### Level 9 — 9 Months

**Ethereal Steeds** (1 item, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1076159 | RideablePolarBear |

**House AddOns** (1 item, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1080549 | WallBannerDeed |

### Level 10 — 10 Months

**Monster Statues** (2 items, ML):
| Cliloc | Monster Type |
|--------|-------------|
| 1080520 | Harrower |
| 1080521 | Efreet |

**Cloaks and Robes** (6 items, ML):
| Cliloc | Item Type | Hue | Hue Color |
|--------|-----------|-----|-----------|
| 1080382 | RewardCloak | 0x490 | Pink |
| 1080380 | RewardRobe | 0x490 | Pink |
| 1080378 | RewardDress | 0x490 | Pink |
| 1080383 | RewardCloak | 0x485 | Crimson |
| 1080381 | RewardRobe | 0x485 | Crimson |
| 1080379 | RewardDress | 0x485 | Crimson |

**Ethereal Steeds** (1 item, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1080386 | EtherealCuSidhe |

**House AddOns** (2 items, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1080548 | MiningCartDeed |
| 1080397 | AnkhOfSacrificeDeed |

### Level 11 — 11 Months

**Ethereal Steeds** (1 item, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1113908 | EtherealReptalon |

### Level 12 — 12 Months

**Ethereal Steeds** (1 item, ML):
| Cliloc | Item Type |
|--------|-----------|
| 1113813 | EtherealHiryu |

---

## Character Statue Maker

A level 6 sub-system that lets players create a detailed statue of their character. The system consists of a placement item, a frozen mobile statue, a pedestal, and a customization gump.

### Statue Types

| Type | Maker Item Hue | Description |
|------|---------------|-------------|
| Marble | Default | Standard marble statue |
| Jade | 0x65E | Green jade statue |
| Bronze | 0x972 | Bronze statue |

### Statue Properties

| Property | Type | Values | Effect |
|----------|------|--------|--------|
| `StatueType` | `StatueType` | Marble, Jade, Bronze | Affects hue |
| `Pose` | `StatuePose` | Ready, Casting, Salute, AllPraiseMe, Fighting, HandsOnHips | Affects animation |
| `Material` | `StatueMaterial` | Antique, Dark, Medium, Light | Affects hue |
| `Direction` | `Direction` | 8 directions | Facing orientation |
| `SculptedBy` | `string` | Player name | Shown in paperdoll properties |
| `SculptedOn` | `DateTime` | Creation timestamp | Shown in paperdoll properties |

### Statue Creation Flow

```
1. Player double-clicks CharacterStatueMaker (3 variants)
2. Enters targeting mode
3. Targets location inside a owned house, near a door
4. Validates: house location, door proximity, body mod state
5. Creates CharacterStatue (frozen, blessed, non-damageable Mobile)
6. Creates CharacterStatuePlinth (pedestal static/addon)
7. CloneBody(from) copies player's body/mount
8. CloneClothes(from) copies player's equipment
9. CharacterStatueGump opens for customization
```

### Customization Gump (`CharacterStatueGump`)

The gump provides the following controls:

| Control | Type | Options | Description |
|---------|------|---------|-------------|
| Pose | Prev/Next | 6 poses | Cycles through animation poses |
| Direction | Prev/Next | 8 directions | Changes facing orientation |
| Material | Prev/Next | 4 materials | Changes statue hue |
| Sculpt | Button | — | Finalizes, deletes maker item |
| Restore | Button | — | Copies appearance from backup deed |
| Cancel | Button | — | Demolishes statue back to deed |

### Statue Metadata

| Property | Serialized Field | Command Property | Description |
|----------|-----------------|-----------------|-------------|
| `_sculptedBy` | Field 3 | AccessLevel.GameMaster | Name of sculptor |
| `_sculptedOn` | Field 4 | AccessLevel.GameMaster | Creation datetime |
| `_plinth` | Field 5 | AccessLevel.GameMaster | Reference to pedestal |
| `_isRewardItem` | Field 6 | AccessLevel.GameMaster | IRewardItem flag |

### Demolish and Restore

- **Demolish:** Available via context menu for house co-owners or GMs. Converts the statue back into a `CharacterStatueDeed`.
- **Restore:** The deed form can be used to recreate the statue with the same appearance as the original.
- **InvalidatePose():** Sends animation packets to nearby clients using custom packet `0xBF 0x19` (17 bytes).

---

## Gumps

### Reward Notice Gump (`RewardNoticeGump`)

Simple login notification popup. Shown when `cur < max` on login.

| Button | Action |
|--------|--------|
| OK | Opens `RewardChoiceGump` |
| Cancel | Closes |

### Reward Choice Gump (`RewardChoiceGump`)

Main reward selection gump with a 600x450 background (ID 2600). Displays:

- Welcome text with reward interval description
- Current rewards remaining (`max - cur`) and already chosen (`cur`)
- Category buttons that navigate to sub-pages (24 items per page)
- Main Menu button (closes and reopens notice)

Category buttons use a custom encoding: `buttonID - 1 = index * 20 + type`, where `type` is the category index (0-5) and `index` is the entry index within that category.

### Reward Confirm Gump (`RewardConfirmGump`)

Confirmation dialog shown after selecting a specific reward item.

| Element | Description |
|---------|-------------|
| Item name | Localized cliloc label |
| Target character | Character receiving the reward |
| Warning | Non-transferability notice |
| Confirm | Constructs item, adds to backpack (if reward point available), decrements remaining |

### Reward Demolition Gump (`RewardDemolitionGump`)

Used for re-deeding house addons. Asks "Do you wish to re-deed this decoration?" Only the house owner can confirm. Returns the `Deed` from `IAddon.Deed`.

### Reward Option Gump (`RewardOptionGump`)

Generic option-selection gump using the `IRewardOption` interface. Displays a list of cliloc-labeled buttons. Supports `RewardOption` and `RewardOptionList` helper classes.

---

## Reward Year Labels

Items display their reward year using cliloc lookups via `GetRewardYearLabel(Item, object[])`:

| Level | Cliloc ID Calculation | Cliloc ID |
|-------|----------------------|-----------|
| 1-9 | `1076216 + level` | 1076217 - 1076225 |
| 10-12 | `1076216 + level - 9 + 4240` | 1080447 - 1080449 |

---

## Configuration

All settings are managed via `ServerConfiguration.GetOrUpdateSetting()` during `RewardSystem.Configure()`:

```csharp
Enabled = ServerConfiguration.GetOrUpdateSetting("vetRewards.enable", true);
SkillCapRewards = ServerConfiguration.GetOrUpdateSetting("vetRewards.skillCapRewards", true);
RewardInterval = ServerConfiguration.GetOrUpdateSetting("vetRewards.rewardInterval", TimeSpan.FromDays(30.0));
```

The `RewardInterval` setting affects all calculations: reward level computation, `RewardList.Age` values, gump welcome text, and half-level stat reward eligibility. Supported interval values produce human-readable text in the gump: 30 days = "month", 60 days = "two months", 90 days = "three months", 365 days = "year", or custom day count.

---

## Cross-References

- `expansions/timeline.md` — expansion requirements for reward items (ML, AOS, SE)
- `getting-started/stats.md` — stat cap bonuses, skill cap mechanics
- `items/containers.md` — reward items placed in backpack
- `systems/crafting.md` — WeaponEngravingTool (level 8) integration with weapon crafting
- `reference/configuration.md` — vetRewards config settings
