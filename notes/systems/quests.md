# Quests

Quests provide structured player objectives with narrative progression, item collection, creature slaying, escort missions, and reward distribution. ModernUO implements **two distinct quest systems**: the **ML Quest System** (Mondain's Legacy, template-based with per-player context tracking) and the **Modern Quest System** (player-owned with inline objectives and conversations). The ML system supports 20 quest definitions plus 1 base class (BaseEscort) across multiple areas with chain triggers, timed objectives, and skill training quests. The modern system implements 11 quests with profession-restricted access, dialogue trees, and regional objectives.

**Source Files:**
- `Projects/UOContent/Engines/ML Quests/MLQuestSystem.cs` (865 lines) — ML quest management, speech events, quest resolution, GM commands
- `Projects/UOContent/Engines/ML Quests/MLQuest.cs` (308 lines) — ML quest template, objectives/rewards definitions, instance creation
- `Projects/UOContent/Engines/ML Quests/MLQuestContext.cs` (307 lines) — per-player ML quest tracking, completion history, chain offers
- `Projects/UOContent/Engines/ML Quests/MLQuestEntry.cs` (567 lines) — ML quest instance data, objective instances, timer, reward flow
- `Projects/UOContent/Engines/ML Quests/MLQuestPersistence.cs` (63 lines) — save/load singleton
- `Projects/UOContent/Engines/ML Quests/MLQuestPackets.cs` (32 lines) — ML quest packet handling
- `Projects/UOContent/Engines/ML Quests/QuestArea.cs` (61 lines) — quest area wrapper for location checks
- `Projects/UOContent/Engines/ML Quests/QuesterNameAttribute.cs` (33 lines) — quester name attribute
- `Projects/UOContent/Engines/ML Quests/IQuestGiver.cs` (15 lines) — quest giver interface
- `Projects/UOContent/Engines/Quests/Core/QuestSystem.cs` (700 lines) — modern quest system, player-owned quest engine
- `Projects/UOContent/Engines/Quests/Core/QuestObjective.cs` (250 lines) — modern quest objective base class
- `Projects/UOContent/Engines/Quests/Core/QuestConversation.cs` (138 lines) — modern quest dialogue events
- `Projects/UOContent/Engines/Quests/Core/BaseQuester.cs` (91 lines) — quest-giving NPC base vendor
- `Projects/UOContent/Engines/Quests/Core/QuestCallbackEntry.cs` (21 lines) — context menu callback entry
- `Projects/UOContent/Engines/Quests/Core/QuestItemInfo.cs` (57 lines) — quest item info display
- `Projects/UOContent/Engines/Quests/Core/QuestRestartInfo.cs` (35 lines) — restart tracking data
- `Projects/UOContent/Engines/Quests/Core/QuestSerializer.cs` (194 lines) — quest serialization
- **109 ML Quest files** (Definitions, Gumps, Items, Mobiles, Objectives, Rewards)
- **136 Modern Quest files** (Core, Quest definitions, regions, items, NPCs)

---

## ML Quest System (Mondain's Legacy)

The ML Quest System is a template-based quest engine where quest templates define objectives and rewards, and per-player `MLQuestContext` instances track active quests and completion history. The system manages up to **10 concurrent quests** per player and supports quest chains, timed objectives, skill training, and escort missions.

### Configuration

```
questSystem.enableMLQuests = true  # Default: Core.ML
```

**Config File:** `Data/MLQuests.cfg` (tab-separated: `QuestType [QuesterType1 QuesterType2...]`) — maps quest types to their NPC quest givers. **Present by default** with 265 quest entries; quests are also auto-registered via code.

### Core Engine (`MLQuestSystem`)

The `MLQuestSystem` class is a static singleton managing all ML quest state.

#### Static Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `MaxConcurrentQuests` | `int` | 10 | Maximum active quests per player |
| `SpeechColor` | `int` | 0x3B2 | Speech color for quest NPCs |
| `AutoGenerateNew` | `bool` | true | Auto-generate quest templates on startup |
| `Debug` | `bool` | false | Debug logging flag |
| `Enabled` | `bool` | `Core.ML` | Master toggle from config |
| `Quests` | `Dictionary<Type, MLQuest>` | — | Quest type → template mapping |
| `QuestGivers` | `Dictionary<Type, List<MLQuest>>` | — | NPC type → available quests |
| `Contexts` | `Dictionary<PlayerMobile, MLQuestContext>` | — | Player → quest context |

#### Quest Resolution Priority (`FindQuest`)

When a player double-clicks a quest giver (`OnDoubleClick`), quests are resolved in this priority order:

```
1. Active quest with this NPC → send progress gump
2. Delivery objectives for this NPC → process delivery
3. Chain offers (unlocked previous quests) → send offer
4. Random starter quest → send offer
```

#### Key Static Methods

| Method | Return | Description |
|--------|--------|-------------|
| `GetContext(PlayerMobile)` | `MLQuestContext` | Retrieve or create player context |
| `FindQuest(IQuestGiver, PlayerMobile, MLQuestContext, out MLQuest, out MLQuestInstance)` | `bool` | Priority-based quest resolution |
| `MarkQuestItem(PlayerMobile, Item)` | `void` | Marks item as quest item, checks objective completion |
| `HandleKill(PlayerMobile, Mobile)` | `void` | Processes kill events against active quest objectives |
| `HandleDelivery(PlayerMobile, IQuestGiver, Type)` | `void` | Processes delivery objectives |
| `HandleSkillGain(PlayerMobile, SkillName)` | `void` | Processes skill gain events |
| `RandomStarterQuest(IQuestGiver, PlayerMobile, MLQuestContext)` | `MLQuest` | Picks random eligible starter quest |
| `OnPlayerDeath(PlayerMobile)` | `void` | Event handler for player death |
| `OnPlayerDeleted(PlayerMobile)` | `void` | Event handler for character deletion |

#### GM Commands

| Command | Description |
|---------|-------------|
| `/mlquestsinfo` | Display ML quest system status |
| `/savequest` | Save current player's quest context |
| `/saveallquests` | Save all player quest contexts |
| `/invalidquestitems` | List invalid quest items |
| `/viewquests` | View all registered quest templates |
| `/viewmlcontext` | View a player's ML quest context |

---

### Quest Template (`MLQuest`)

The `MLQuest` class defines a quest template with objectives, rewards, and text. Templates are registered at startup and instantiated per-player as `MLQuestInstance` objects.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Activated` | `bool` | Whether the quest is available to be offered |
| `SaveEnabled` | `bool` | Whether instances should be serialized |
| `Objectives` | `List<BaseObjective>` | Objective templates |
| `ObjectiveType` | `ObjectiveType` | `All` (all complete) or `Any` (any one complete) |
| `Rewards` | `List<BaseReward>` | Reward templates |
| `Instances` | `List<MLQuestInstance>` | All active instances |
| `OneTimeOnly` | `bool` | Cannot be repeated |
| `HasRestartDelay` | `bool` | Requires delay before repeating |
| `IsEscort` | `bool` | Quest is an escort type |
| `IsSkillTrainer` | `bool` | Quest provides skill training |
| `RequiresCollection` | `bool` | Requires item collection |
| `IsChainTriggered` | `bool` | Unlocks next quest on completion |
| `NextQuest` | `Type` | Type of the chained quest |
| `Title` | `TextDefinition` | Quest title |
| `Description` | `TextDefinition` | Quest description |
| `RefusalMessage` | `TextDefinition` | Message on refusal |
| `InProgressMessage` | `TextDefinition` | Message during progress |
| `CompletionMessage` | `TextDefinition` | Message on completion |
| `CompletionNotice` | `TextDefinition` | Notice shown when rewards available |
| `Version` | `int` | Serialization version |

#### Lifecycle Methods

| Method | Trigger | Description |
|--------|---------|-------------|
| `Generate()` | Startup / `AutoGenerateNew` | Override to set up objectives/rewards |
| `CanOffer(IQuestGiver, PlayerMobile, MLQuestContext, bool)` | Quest resolution | Checks eligibility |
| `SendOffer(IQuestGiver, PlayerMobile)` | Accept offer | Sends `QuestOfferGump` |
| `OnAccept(IQuestGiver, PlayerMobile)` | Player accepts | Creates instance, plays sound, hooks objectives |
| `OnRefuse(IQuestGiver, PlayerMobile)` | Player refuses | Sends `QuestConversationGump` with refusal message |
| `GetRewards(MLQuestInstance)` | Rewards claimed | Override for custom rewards |
| `OnRewardClaimed(MLQuestInstance)` | Rewards claimed | Post-reward hook |
| `OnCancel(MLQuestInstance)` | Quest cancelled | Cleanup hook |
| `GetRestartDelay()` | Repeat check | Default: random 30-150 seconds |

---

### Per-Player Context (`MLQuestContext`)

The `MLQuestContext` class tracks a player's quest state, including active instances, completed quests, and special flags.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Owner` | `PlayerMobile` | The player |
| `QuestInstances` | `List<MLQuestInstance>` | Active quest instances |
| `ChainOffers` | `List<MLQuest>` | Unlocked chained quests |
| `IsFull` | `bool` | `QuestInstances.Count >= MaxConcurrentQuests` |
| `Spellweaving` | `bool` | Bit flag: Spellweaving quest progress |
| `SummonFey` | `bool` | Bit flag: Summon Fey quest progress |
| `SummonFiend` | `bool` | Bit flag: Summon Fiend quest progress |
| `BedlamAccess` | `bool` | Bit flag: Bedlam dungeon access |

#### Completion Tracking

| Method | Description |
|--------|-------------|
| `HasDoneQuest(Type)` | Check if quest was ever completed |
| `HasDoneQuest(MLQuest, out DateTime)` | Check completion + next available time for repeatable |
| `SetDoneQuest(MLQuest, DateTime)` | Log completion with optional restart timestamp |
| `RemoveDoneQuest(MLQuest)` | Remove done quest record |
| `FindInstance(Type)` | Find active instance by quest type |
| `IsDoingQuest(Type)` | Check if player is actively doing a quest |

#### Bit Flag System

Uses `MLQuestFlag` enum with bitwise operations for compact storage of quest progress flags:

```csharp
GetFlag(MLQuestFlag) → bool
SetFlag(MLQuestFlag, bool) → void
```

---

### Quest Instance (`MLQuestInstance`)

The `MLQuestInstance` class represents an active quest for a specific player, created from a template. It manages objective instances, timers, and the reward flow.

#### Properties

| Property | Type | Description |
|-----------|------|-------------|
| `Quest` | `MLQuest` | The parent template |
| `Quester` | `IQuestGiver` | The NPC who gave the quest |
| `Player` | `PlayerMobile` | The player |
| `Accepted` | `DateTime` | When the quest was accepted |
| `ClaimReward` | `bool` | Quest complete, rewards pending |
| `Removed` | `bool` | Instance unregistered |
| `Failed` | `bool` | Timed objectives expired |
| `Objectives` | `BaseObjectiveInstance[]` | Per-instance objective states |
| `SkipReportBack` | `bool` | True if `CompletionMessage` is null/empty |

#### Instance Flags (`MLQuestInstanceFlags`)

| Flag | Description |
|------|-------------|
| `ClaimReward` | Player can claim rewards |
| `Removed` | Instance removed from context |
| `Failed` | Quest failed (timed objectives expired) |

#### Timer Flow

```
1. Constructor: Creates objective instances from template, registers with quest/context
2. Starts timer if any objectives are timed
3. Slice(): Checks timed objectives, fails quest if any expired (ObjectiveType.All)
4. OnTick(): Checks timed objectives, calls Fail() if expired
```

#### Reward Claim Flow (`ContinueReportBack`)

```
1. Evaluate all objectives against ObjectiveType:
   a. ObjectiveType.All: All objectives must be complete
   b. ObjectiveType.Any: First objective complete triggers claim
2. Set ClaimReward flag
3. Log done quest with SetDoneQuest()
4. Add chain offers to ChainOffers list
5. Remove instance from QuestInstances
6. Send RewardGump → ClaimRewards()
```

#### UI Methods

| Method | Description |
|--------|-------------|
| `SendProgressGump()` | Show quest progress |
| `SendRewardOffer()` | Offer rewards after completion |
| `SendRewardGump()` | Display reward selection |
| `SendReportBackGump()` | Report quest completion |

---

### ML Quest Objectives

Five objective types cover the common quest actions: slaying creatures, collecting items, delivering to NPCs, escorting, and skill training.

#### Objective Types

| Objective | Template Class | Instance Class | Tracking |
|-----------|---------------|----------------|----------|
| **Kill** | `KillObjective` | `KillObjectiveInstance` | `Slain` count (creature type(s)) |
| **Timed Kill** | `TimedKillObjective` | `TimedKillObjectiveInstance` | Kill count with expiry timer |
| **Collect** | `CollectObjective` | `CollectObjectiveInstance` | Item count via quest item marking |
| **Deliver** | `DeliverObjective` | `DeliverObjectiveInstance` | Delivery to specific quest giver |
| **Escort** | `EscortObjective` | `EscortObjectiveInstance` | NPC arrival at `QuestArea` (timed) |
| **Skill Gain** | `GainSkillObjective` | `GainSkillObjectiveInstance` | Skill amount gained (listens to `HandleSkillGain`) |

#### Quest Area (`QuestArea`)

Wraps a region name and optional map for objective location checks. Used by `KillObjective`, `EscortObjective`, and similar area-dependent objectives.

---

### ML Quest Rewards

Two reward types distribute items and effects to players upon quest completion.

| Reward | Class | Description |
|--------|-------|-------------|
| **Item Reward** | `ItemReward` | Gives items by type/quantity. Uses static pre-defined reward definitions. |
| **Dummy Reward** | `DummyReward` | No-op reward (displays text only, e.g., "A step closer to entering Blighted Grove.") |

**Pre-defined rewards** include `LargeBagOfTreasure` and various item reward categories registered in `ItemReward`.

---

### ML Quest Definitions (21 Quests)

Quest definitions are in `Projects/UOContent/Engines/ML Quests/Definitions/`:

| Quest | Type | Area/Theme | Key Features |
|-------|------|-----------|---------------|
| `AGhostOfCovetous` | Dungeon | Covetous | Ghost-themed dungeon quest |
| `Bedlam` | Dungeon | Bedlam | Bedlam dungeon access quest |
| `BlightedGrove` | Chain | Blighted Grove | Multi-step chain (6 quests), custom NPCs (Jamal, Iosep) |
| `Britannia` | Multiple | Britannia | Britannia-themed quests |
| `Heartwood` | Multiple | Heartwood | Heartwood-area quests |
| `Heritage` | Multiple | Heritage | Heritage-themed quests |
| `HonestBeggar` | Escort | — | Escort quest, inherits `BaseEscort` |
| `Ilshenar` | Multiple | Ilshenar | Ilshenar-area quests |
| `LostItems` | Collection | — | Lost item recovery |
| `Malas` | Multiple | Malas | Malas-area quests |
| `MistakenIdentity` | Multiple | — | Identity-themed quest |
| `NewHaven` | Escort | New Haven | 12 escort types: Alchemist, Bard, Warrior, Tailor, Carpenter, Mapmaker, Mage, Inn, Farm, Docks, Bowyer, Bank |
| `NewHavenSkillTraining` | Training | New Haven | Skill training quests |
| `NewHavenTraining` | Training | New Haven | General training quests |
| `Sanctuary` | Multiple | Sanctuary | Sanctuary-themed quest |
| `Spellweaving` | Multiple | Spellweaving | Spellweaving progression quest |
| `TheAncientWorld` | Multiple | Ancient World | Ancient world exploration |
| `TownEscorts` | Escort | Towns | Town escort quests |
| `UnfadingMemories` | Multiple | — | Memory-themed quest |
| `WarriorsOfTheGemKeeper` | Multiple | Gem Keeper | Warriors of the Gem quest chain |

**Base class for escorts:** `BaseEscort` — adds `HumanInNeed.AwardTo(instance.Player)` reward with `AwardHumanInNeed` flag.

---

### ML Quest Gumps (10)

All in `Projects/UOContent/Engines/ML Quests/Gumps/`:

| Gump | Class | Description |
|------|-------|-------------|
| **Base** | `BaseQuestGump` | OSI-accurate layout (IDs 800-810). Methods: `AddDescription`, `AddObjectives`, `AddObjectivesProgress`, `AddRewards`, `AddConversation`, `BuildPage` |
| **Offer** | `QuestOfferGump` | Quest offer/accept dialog |
| **Conversation** | `QuestConversationGump` | Refusal and in-progress conversation |
| **Quest Log** | `QuestLogGump` | Quest overview log |
| **Quest Log (Detailed)** | `QuestLogDetailedGump` | Detailed quest log view |
| **Reward** | `QuestRewardGump` | Reward selection/claim dialog |
| **Report Back** | `QuestReportBackGump` | Report quest completion dialog |
| **Cancel Confirm** | `QuestCancelConfirmGump` | Cancel quest confirmation |
| **Info NPC** | `InfoNPCGump` | Quest giver info dialog |
| **Race Changer** | `RaceChangeGump` | Race changer gump |

---

## Modern Quest System

The Modern Quest System is a player-owned quest engine where each quest is a subclass of `QuestSystem` stored on the `PlayerMobile`. Objectives and conversations are first-class objects with inline serialization via `TypeReferenceTable`. Supports profession-restricted quests, dialogue trees, regional objectives, and dynamic quest items.

### Core Engine (`QuestSystem`)

Each modern quest is a `QuestSystem` subclass with 11 total quest definitions.

#### Instance Properties

| Property | Type | Description |
|----------|------|-------------|
| `From` | `PlayerMobile` | The player who owns this quest |
| `Objectives` | `List<QuestObjective>` | Active objectives |
| `Conversations` | `List<QuestConversation>` | Active conversation events |
| `Name` | `object` | Quest name (abstract, overridden per quest; typically returns CLLOC int) |
| `OfferMessage` | `object` | Initial offer message (abstract; typically returns CLLOC int) |
| `Picture` | `int` | Gump icon (abstract) |
| `IsTutorial` | `bool` | Whether this is a tutorial quest (abstract) |
| `RestartDelay` | `TimeSpan` | Delay before repeat (abstract; `Zero` = no restart, `MaxValue` = never) |
| `TypeReferenceTable` | `Type[]` | Type lookup table for serialization (abstract) |

#### Timer Model

```
1. StartTimer() — starts 0.5s slice interval timer
2. Slice() — checks GetTimerEvent() on each objective
3. StopTimer() — stops timer on quest end
```

#### Key Methods

| Method | Return | Description |
|--------|--------|-------------|
| `Accept()` | `void` | Sets `From.Quest = this`, plays accept message, starts timer |
| `Decline()` | `void` | Sends decline message |
| `Cancel()` | `void` | Ends quest, handles restart delay via `From.DoneQuests` |
| `Complete()` | `void` | Completes quest, clears state |
| `ClearQuest(bool completed)` | `void` | Stops timer, clears `From.Quest`, records restart info |
| `SendOffer()` | `void` | Sends `QuestOfferGump` |
| `ShowQuestLog()` | `void` | Shows quest log gump |
| `ShowQuestConversation()` | `void` | Shows conversation gump |
| `ShowQuestLogUpdated()` | `void` | Shows updated notification |
| `OnKill(BaseCreature, Container)` | `void` | Processes kill events against objectives |
| `IgnoreYoungProtection(Mobile)` | `bool` | Checks if objectives ignore young protection |
| `FindObjective<T>()` | `T` | Find objective by type |
| `IsObjectiveInProgress(Type)` | `bool` | Check if objective type is active |
| `AddConversation(QuestConversation)` | `void` | Add conversation at runtime |
| `AddObjective(QuestObjective)` | `void` | Add objective at runtime |

#### Static Methods

| Method | Return | Description |
|--------|--------|-------------|
| `CanOfferQuest(Mobile, Type, out bool)` | `bool` | Checks profession restrictions, restart delays, existing gumps |

#### Context Menu Entries

Active quests add three context menu entries: "View Quest Log", "Quest Conversation", "Cancel Quest" via `GetContextMenuEntries()`.

---

### Quest Objective (`QuestObjective`)

Objectives track progress toward quest goals with inline serialization.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `System` | `QuestSystem` | Parent quest system |
| `CurProgress` | `int` | Current progress (triggers `CheckCompletionStatus` on set) |
| `MaxProgress` | `int` | Default 1, overridable for multi-stage |
| `Completed` | `bool` | `CurProgress >= MaxProgress` |
| `HasCompleted` | `bool` | First-time completion flag |
| `HasBeenRead` | `bool` | Message read tracking |
| `Info` | `QuestItemInfo[]` | Items shown in info gump |
| `Message` | `int` | Abstract CLLOC string for objective description |

#### Key Methods

| Method | Description |
|--------|-------------|
| `Complete()` | Sets progress to max |
| `CheckCompletionStatus()` | Sets `HasCompleted`, calls `OnComplete()` |
| `GetTimerEvent()` | Timer callback for progress checking |
| `CheckProgress()` | Progress update callback |
| `GetKillEvent(BaseCreature, Container)` | Kill event hook |
| `OnKill()` | Kill handler |
| `RenderMessage(Gump)` / `RenderProgress(Gump)` | UI rendering |

#### Gumps

| Gump | Defined In | Description |
|------|-----------|-------------|
| `QuestLogUpdatedGump` | `QuestObjective.cs` | "Quest Log Updated" notification |
| `QuestObjectivesGump` | `QuestObjective.cs` | Multi-page objectives display |

---

### Quest Conversation (`QuestConversation`)

First-class conversation events with optional item info and read tracking.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `System` | `QuestSystem` | Parent quest system |
| `Message` | `int` | Abstract CLLOC int for dialogue text |
| `Info` | `QuestItemInfo[]` | Optional item info |
| `Logged` | `bool` | Whether to add to `Conversations` list |
| `HasBeenRead` | `bool` | Read tracking |

#### Key Methods

| Method | Description |
|--------|-------------|
| `OnRead()` | Called when player first reads the conversation |
| `RenderMessage(Gump)` | UI rendering |

#### Gump

| Gump | Description |
|------|-------------|
| `QuestConversationsGump` | Multi-page conversation display |

---

### Quest Serialization (`QuestSerializer`)

Uses the `TypeReferenceTable` from each quest to serialize/deserialize objective and conversation objects by their type index. Stored directly on the `PlayerMobile.Quest` instance.

### Restart Tracking (`QuestRestartInfo`)

Stores quest type + restart delay time in `PlayerMobile.DoneQuests` list for repeatable quest cooldowns.

### Item Info Display (`QuestItemInfo`)

Simple data class with `Name` (object for int/string message types) and `ItemID`. Displayed via `QuestItemInfoGump` as a small floating info panel.

---

### Quest NPC Base (`BaseQuester`)

Quest-giving NPCs inherit from `BaseVendor` with specific modifications.

| Property | Value | Description |
|----------|-------|-------------|
| `TalkNumber` | 6146 | Default CLLOC for "Talk" |
| `IsActiveVendor` | false | Not a standard vendor |
| `IsInvulnerable` | true | Cannot be harmed |
| `DisallowAllMoves` | true | Items cannot be moved from vendor |

#### Key Methods

| Method | Description |
|--------|-------------|
| `OnTalk(PlayerMobile, bool)` | Abstract, overridden per quest NPC |
| `CanTalkTo(PlayerMobile)` | Profession/level access checks |
| `GetAutoTalkRange(PlayerMobile)` | Auto-talk range (-1 = disabled) |
| `OnMovement()` | Auto-talk trigger when player enters range |
| `AddCustomContextEntries()` | Adds "Talk" context menu entry |

---

### Modern Quest Definitions (11 Quests)

| Quest | Directory | Type | Profession | Key Features |
|-------|-----------|------|-----------|--------------|
| **The Summoning** | `The Summoning/` | Doom/Endgame | Any | Necromancy quest — collect daemon bones, vanquish the daemon. 6 quest items (BellOfTheDead, ChylothShroud, ChylothStaff, GoldenSkull, GrandGrimoire, SummoningAltar) |
| **Dark Tides** | `Dark Tides/` | Sea | Necromancer | Necromancer-only quest. Items: DarkTidesHorn, KronusScroll, MaabusCoffin, ScrollOfAbraxus. NPCs: Horus, Maabus, Mardoth, SummonedPaladin |
| **Uzeraan Turmoil** | `Uzeraan Turmoil/` | Magic/War | Warrior/Magician/Paladin | Profession-restricted quest |
| **Collector** | `Collector/` | Art | Any | Extensive multi-branch quest with 30+ conversations/objectives. Items: Obsidian, PaintedImage, EnchantedPaints. NPCs: AlbertaGiacco, ElwoodMcCarrin, GabrielPiete, Impresario, TomasONeerlan |
| **Witch Apprentice** | `Witch Apprentice/` | Magic | Any | Witch apprentice quest |
| **Study of the Solen Hive** | `Study of the Solen Hive/` | Nature | Any | Solen hive exploration. Items/NPCs in hive area |
| **Solen Matriarch** | `Solen Matriarch/` | Boss | Any | Solen Matriarch boss fight quest |
| **Ambitious Solen Queen** | `Ambitious Solen Queen/` | Boss | Any | Solen Queen boss quest |
| **Emino's Undertaking** | `Emino's Undertaking/` | Ninjitsu | Ninja | Ninja-only quest. Items: EminosKatana, teleporters, GuardianBarrier. NPCs: Emino, Zoel, JedahEntille, EnshroudedFigure, HiddenFigure, Henchman |
| **Haochi's Trials** | `Haochi's Trials/` | Samurai | Samurai | Samurai-only quest. Items: HaochisKatana, HonorCandle, TreasureChest. NPCs: Haochi, Relnia, CursedSoul, DeadlyImp, DiseasedCat, FierceDragon, InjuredWolf, YoungNinja, YoungRonin, Guardsman |
| **Terrible Hatchlings** | `Terrible Hatchlings/` | Family | Any | Family-themed quest. NPC: AnsellaGryen |

---

### Modern Quest Regions

Four region types provide spatial quest controls:

| Region | Class | Description |
|--------|-------|-------------|
| **Cancel Quest** | `CancelQuestRegion` | Region where quest can be cancelled |
| **Complete Objective** | `QuestCompleteObjectiveRegion` | Region to complete objectives |
| **No Entry** | `QuestNoEntryRegion` | Restricted quest area |
| **Offer** | `QuestOfferRegion` | Region where quest can be offered |

### Modern Quest Items

Core quest-specific items in `Projects/UOContent/Engines/Quests/Core/Items/`:

| Item | Description |
|------|-------------|
| `DynamicTeleporter` | Teleporter for quest areas |
| `EnchantedSextant` | Sextant for quest navigation |
| `HornOfRetreat` | Quest retreat item |
| `QuestItem` | Base class for quest-specific items |

---

## Architectural Comparison

| Aspect | ML Quest System | Modern Quest System |
|--------|----------------|---------------------|
| **Ownership** | Server-managed (`MLQuestSystem.Contexts`) | Player-owned (`PlayerMobile.Quest`) |
| **Model** | Template + Instance classes | Single `QuestSystem` subclass per quest |
| **Objectives** | `BaseObjective` template → `BaseObjectiveInstance` instance | `QuestObjective` directly on player |
| **Conversations** | Simple text messages via gumps | First-class `QuestConversation` objects |
| **Serialization** | `MLQuestPersistence` singleton item | `QuestSerializer` on player's quest |
| **Quest Chains** | `IsChainTriggered` + `NextQuest` + `ChainOffers` list | No explicit chain support |
| **Restart Delay** | `SetDoneQuest` with timestamp | `QuestRestartInfo` in `PlayerMobile.DoneQuests` |
| **Timer Model** | Per-instance timer (if timed objectives) | Per-quest timer (checks all objectives) |
| **Kill Tracking** | Centralized `MLQuestSystem.HandleKill()` | `QuestSystem.OnKill()` delegates to objectives |
| **Profession Checks** | Per-quest in `CanOffer()` | `CanOfferQuest()` static method |
| **GM Tools** | `/mlquestsinfo`, `/savequest`, `/viewquests` | Context menu on quest gump |
| **Config File** | `Data/MLQuests.cfg` (tab-separated) | None required |
| **Max Concurrent** | 10 quests | 1 quest at a time |

---

## Cross-References

- [`../getting-started/character-creation.md`](../getting-started/character-creation.md) — Profession restrictions for Uzeraan, Dark Tides, Emino's, Haochi's quests
- [`../systems/crafting.md`](../systems/crafting.md) — Crafting-related quest objectives (item collection)
- [`../skills/combat-skills.md`](../skills/combat-skills.md) — Combat skill training via ML quests
- [`../creatures/npcs.md`](../creatures/npcs.md) — Quest-giving NPCs across both systems
- [`../items/containers.md`](../items/containers.md) — Quest item storage and backpack management
- [`../expansions/timeline.md`](../expansions/timeline.md) — ML expansion context (Mondain's Legacy)
