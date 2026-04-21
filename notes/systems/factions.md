# Factions

The Factions system is a large-scale PvP-based town control and economic game mode. Four factions compete for control of eight towns through PvP combat, town capture mechanics, and an internal economy. The system includes elections for faction leadership, town management with vendors and guards, a kill point ranking system, and a stability code to prevent faction dominance.

**Source Files:**
- `Projects/UOContent/Engines/Factions/Core/FactionSystem.cs` (126 lines) — persistence, toggle config
- `Projects/UOContent/Engines/Factions/Core/Faction.cs` (1541 lines) — core faction logic
- `Projects/UOContent/Engines/Factions/Core/FactionState.cs` (308 lines) — serializable state
- `Projects/UOContent/Engines/Factions/Core/PlayerState.cs` (307 lines) — per-player faction data
- `Projects/UOContent/Engines/Factions/Core/Town.cs` (544 lines) — town ownership and management
- `Projects/UOContent/Engines/Factions/Core/TownState.cs` (139 lines) — per-town serializable state
- `Projects/UOContent/Engines/Factions/Core/Election.cs` (560 lines) — election management
- `Projects/UOContent/Engines/Factions/Definitions/FactionDefinition.cs` (91 lines) — faction config
- `Projects/UOContent/Engines/Factions/Definitions/RankDefinition.cs` (21 lines) — per-rank config
- `Projects/UOContent/Engines/Factions/Definitions/GuardDefinition.cs` (37 lines) — per-guard config
- `Projects/UOContent/Engines/Factions/Definitions/VendorDefinition.cs` (86 lines) — vendor config
- `Projects/UOContent/Engines/Factions/Definitions/TownDefinition.cs` (53 lines) — town config
- `Projects/UOContent/Engines/Factions/Instances/Factions/` — 4 faction implementations
- `Projects/UOContent/Engines/Factions/Instances/Towns/` — 8 town implementations
- `Projects/UOContent/Engines/Factions/Items/` — faction items (Sigil, Silver, traps, etc.)
- `Projects/UOContent/Engines/Factions/Mobiles/Guards/` — 10 guard types
- `Projects/UOContent/Engines/Factions/Mobiles/Vendors/` — 5 vendor types
- `Projects/UOContent/Engines/Factions/Gumps/` — 12 gumps

---

## Factions Overview

### Four Factions

Two Hero-aligned and two Evil-aligned factions compete for control:

| Faction | Alignment | Description |
|---------|-----------|-------------|
| **TrueBritannians** | Hero | Loyalists to Lord Britann |
| **CouncilOfMages** | Hero | The magical council of Magincia |
| **Minax** | Evil | The witch queen |
| **Shadowlords** | Evil | Minax's dark council |

### Map and Scope

Factions operate exclusively on **Felucca** (`Map.Felucca`). All town ownership, faction items, and kill point tracking are facet-specific.

### Configuration

The system is controlled by the `factions.enabled` server setting. When disabled, all faction functionality including persistence, town ownership, and faction items are inactive.

---

## Joining a Faction

### Join Requirements

A player can join a faction if all of the following are true:

1. **Not young** — Young player status prevents joining
2. **No existing character** — No other character on the account is already in factions
3. **Not faction banned** — Account must not have the `FactionBanned` tag
4. **Guild restrictions** — If the player is a guild leader of a regular guild:
   - The guild must be of Regular type
   - The guild must have no active wars
   - The guild must have no non-faction alliances

### Stability System

When any faction has more than **200 members**, a stability code activates to prevent a single faction from becoming too large:

```
if (largestFactionMembers > 200):
    if (newTotal × 100 / 300 > smallestFactionMembers):
        deny join (influx would cause instability)
```

The stability threshold is `StabilityFactor = 300` and activates at `StabilityActivation = 200` members.

### Join Process

Players interact with a **JoinStone** (ItemID `0xEDC`) to join. The `JoinStoneGump` displays faction information and confirmation. Upon acceptance, the player's character is added to the faction's member list via `OnJoinAccepted()`.

---

## Kill Points and Rankings

### Kill Point Transfer

When a faction member kills an opposing faction member, kill points are transferred:

```
award = max(victimKillPoints / 10, 1)
award = min(award, 40)  // capped at 40

killer.KillPoints += award
victim.KillPoints -= award

// If victim has KP <= -6: power transfer is halved
if (victim.KillPoints <= -6):
    powerTransfer = max(1, victimPower / 5) / 2
else:
    powerTransfer = max(1, victimPower / 5)
```

Power is capped at 100 total: `powerTransfer = min(powerTransfer, 100 - killer.Power)`

### Rank Calculation

Player rank is determined by their position in the kill-point-sorted member list:

```
percent = (ZeroRankOffset - RankIndex) × 1000 / ZeroRankOffset
```

`ZeroRankOffset` is the number of members with positive kill points. The percentage is then matched against `RankDefinition.Required` thresholds to determine the player's rank.

### Rank Definitions

Each faction defines its own ranks in `RankDefinition[]`. Ranks determine:
- Display order in member lists
- Maximum wearable faction items (`Rank.MaxWearables`)
- Maximum rank for election candidacy (`CandidateRank` ≥ 5)
- Merchant title eligibility (requires Rank ≥ 5 and 90 skill)

### Active/Inactive Status

A player's `IsActive` flag tracks recent PvP participation. Active members (KP > 0) gain advantages during the atrophy cycle.

---

## Silver Economy

### Silver as Currency

`Silver` (ItemID `0x1063`) serves as the faction's economic currency. Silver flows through three channels:

1. **Creature kills** — Killing faction creatures awards silver based on `FactionSilverWorth`
2. **Town income** — Daily income from town taxation
3. **Sigil returns** — Returning a corrupted sigil awards silver

### Tithe System

Factions collect a tithe percentage (`Tithe`, default 50%) from silver awarded on kills:

```
tithed = silver × Tithe / 100
remaining = silver - tithed
Faction.Silver += tithed
Killer gets remaining as normal silver
```

The tithe can be changed by the faction commander via `FactionStoneGump` (0-100%, step 10).

### Silver Gifts

Players can gift silver to faction mates with a **3-hour cooldown** per recipient, tracked via `SilverGiven` entries in `PlayerState`:

```csharp
public class SilverGivenEntry {
    public DateTime Time;
    public PlayerState From;
}
```

---

## Town Control

### The Eight Towns

| Town | Faction Instance |
|------|-----------------|
| Britain | `Britain.cs` |
| Magincia | `Magincia.cs` |
| Minoc | `Minoc.cs` |
| Moonglow | `Moonglow.cs` |
| SkaraBrae | `SkaraBrae.cs` |
| Trinsic | `Trinsic.cs` |
| Vesper | `Vesper.cs` |
| Yew | `Yew.cs` |

Each town extends the `Town` base class and sets its own `Definition` in the constructor.

### Town Capture — The Sigil System

Town control is mediated through the **Sigil** (a special PvP item). The sigil lifecycle has four states:

| State | Description |
|-------|-------------|
| **Normal** | Sigil is not corrupted, not being corrupted |
| **Corrupting** | Sigil is held at enemy stronghold, corruption timer running |
| **Corrupted** | Sigil fully corrupted, must be returned to town monolith |
| **Purifying** | Sigil recently returned, town is in purification period |

### Sigil Timelines

| Phase | SE | AOS | Description |
|-------|----|----|-------------|
| **CorruptionGrace** | 30s | 15s | Time to switch controlling faction at stronghold monolith |
| **CorruptionPeriod** | 10h | 24h | Time to fully corrupt sigil at stronghold |
| **ReturnPeriod** | 1h | 1h | Time to return corrupted sigil to town monolith |
| **PurificationPeriod** | 3 days | 3 days | Town cannot be recaptured after purification |

### Sigil Mechanics

1. A member of the controlling faction places the sigil on the **enemy stronghold monolith**
2. During **CorruptionGrace**: another faction member can touch the monolith to switch control (resets corruption timer)
3. After **CorruptionGrace**: switching starts a fresh corruption timer for the new holder
4. After **CorruptionPeriod**: the sigil becomes corrupted and must be returned to your faction's town monolith within **ReturnPeriod**
5. On return to a **TownMonolith**: the town is captured for the **PurificationPeriod**
6. During **PurificationPeriod**: the town cannot be recaptured
7. On death or logout: the sigil **returns home**

### Town Capture Rewards

| Event | Silver Award |
|-------|-------------|
| Unowned → Owned | `SilverCaptureBonus = 10,000` to controlling faction |
| Changed hands | `SilverCaptureBonus = 10,000` to new controller |
| Owned → Unowned | Income timer resets |

### Town Management Roles

| Role | Responsibilities | How to Obtain |
|------|-----------------|---------------|
| **Commander** | Leads faction, starts elections, broadcasts messages | Elected by faction members |
| **Sheriff** | Hires/fires guards, manages guard patrol | Appointed by faction commander at TownStone |
| **Finance** | Hires/fires vendors, changes prices, manages finances | Appointed by faction commander at TownStone |

### Town Income System

Towns generate daily income based on vendor pricing:

```
DailyIncome = 10000 × (100 + Tax) / 100
NetCashFlow = DailyIncome - FinanceUpkeep - SheriffUpkeep

FinanceUpkeep = vendorCount × vendorUpkeepCost
SheriffUpkeep = guardCount × guardUpkeepCost
```

If `Silver + flow < 0`: the town randomly deletes vendors or guards until cash flow is sustainable.

### Tax System

Town tax modifies vendor prices from -30% to +300% in steps of 5. Tax changes have a **12-hour cooldown** (`TaxChangePeriod`).

| Role | Tax Authority |
|------|-------------|
| Finance Minister | Can change town tax |
| Sheriff | Cannot change tax |
| Commander | Cannot directly change tax |

### On Town Capture — Vendors and Guards

When a town changes hands or becomes unowned:
- All existing vendors and guards are **deleted**
- New vendor and guard lists are **constructed** for the new controller
- The new sheriff and finance minister must re-hire staff

---

## Elections

### Election Cycle

Elections follow a fixed cycle managed by `Election.cs`:

```
Pending (5 days) → Campaign (1 day) → Election/Voting (3 days) → Pending...
```

### Candidate Requirements

To run for faction commander:
1. Must be a current faction member
2. Must have `Rank ≥ 5` (defined as `CandidateRank` in `FactionDefinition`)
3. Maximum **10 candidates** per election

### Vote Weight Formula

Each faction member's vote carries a weight based on three factors:

```
factorSkills = 50 + SkillTotal × 100 / 10000
factorKillPts = 100 + KillPoints × 2
factorGameTime = 50 + (GameTime.Ticks × 100 / TicksPerDay)

totalFactor = clamp(
    factorSkills × factorKillPts × max(factorGameTime, 100) / 10000,
    0, 100
)
```

### Vote Cleaning (Mule Vote Removal)

Votes with `factor < 90` are considered illegitimate ("mule votes") and are automatically cleaned. GMs can also remove candidates and individual voters via `ElectionManagementGump`.

### Win Conditions

| Scenario | Result |
|----------|--------|
| Single candidate during Campaign | Auto-wins immediately |
| No candidates | Election goes back to Pending |
| Multiple candidates during Election | Highest vote count wins |

---

## Kill, Death, and Skill Loss

### Death Between Opposing Factions

When a faction member dies to an opposing faction member:

1. **Sigil drop** — If the victim carries a sigil, the killer picks it up if within 64 tiles, has no sigil, and has backpack space
2. **Silver award** — Killer receives silver based on victim's `FactionSilverWorth`, with tithe applied
3. **Kill point transfer** — As described in the Kill Points section above
4. **Skill loss** — Victim loses 1/3 of all base skill values for 20 minutes

### Skill Loss

```
skillLoss = baseSkill × SkillLossFactor  // SkillLossFactor = 1/3
duration = SkillLossPeriod  // 20 minutes
```

Skill loss is applied via `DefaultSkillMod` and can be removed early by consuming a **GemOfEmpowerment**.

### Honor Leadership

Faction commanders can honor faction mates to boost their kill points:

```
cost = 5 KP (deducted from commander)
gain = 4 KP (awarded to recipient)
requirement = Commander must have at least 5 KP
cooldown = tracked via LastHonorTime
```

---

## Atrophy System

### Atrophy Cycle

Every **47 hours**, the `HandleAtrophy()` method runs across all factions:

```
for each player in faction:
    if player.IsActive:
        player.IsActive = false  // Mark active as inactive for next cycle
        continue
    if player.KillPoints > 0:
        atrophy = (KillPoints + 9) / 10  // Ceiling division by 10
        player.KillPoints -= atrophy
        distrib += atrophy
    else:
        player loses all atrophy

// Distribute collected atrophy randomly among active members
```

### Active Status

A player is `IsActive` if they have recently participated in PvP against opposing factions. Inactive members lose all accumulated atrophy without penalty, while active members with KP lose atrophy proportional to their kill points.

---

## Faction Items

### Imbued Faction Items (`FactionItem`)

Faction items are imbued with faction properties and tracked via the `FactionItem` class:

| Property | Value |
|----------|-------|
| **Expiration** | 21 days after imbue |
| **Commander max wearables** | 9 |
| **Other members max wearables** | `Rank.MaxWearables` from RankDefinition |

Imbuing sets the item's hue to the faction's primary color and starts the expiration timer.

### Imbue Categories

| Category | Silver Cost | Vendor |
|----------|------------|--------|
| Metal Armor | 1,000 | Blacksmith |
| Weapon | 1,000 | Blacksmith |
| Ranged Weapon | 1,000 | Bowyer |
| Leather Armor | 750 | Tailor |
| Clothing | 200 | Tailor |
| Scroll | 500 | Mage |

### Power Faction Items

Five power items spawn on PvP kills, weighted by the victim's rank. Spawn weights:

| Item | Spawn Weight |
|------|-------------|
| GemOfEmpowerment | 30% |
| BloodRose | 25% |
| ClarityPotion | 20% |
| UrnOfAscension | 15% |
| StormsEye | 10% |

#### BloodRose

| Property | Value |
|----------|-------|
| Duration | 5–30 minutes (random) |
| Effect | +3 to all stats |

#### ClarityPotion

| Property | Value |
|----------|-------|
| Duration | 5–30 minutes (random) |
| INT bonus | +3 to +9 (random) |
| Effect | Removes Concussion debuff |

#### GemOfEmpowerment

| Effect | Value |
|--------|-------|
| Removes skill loss effect immediately | Yes |

#### StormsEye

| Property | Value |
|----------|-------|
| Type | Targeted AoE weapon |
| Damage | 40% of target's HP |
| Damage cap | 10–75 |
| Tick structure | 3 damage ticks over 1 second |

#### UrnOfAscension

| Property | Value |
|----------|-------|
| Range | 8 tiles |
| Effect | Revives a dead ally of the same faction |
| Requirements | Ally must be within friend housing, must have skill loss |
| Side effect | Clears target's skill loss |

#### Non-Faction User Punishment

Using any power faction item as a non-faction member deals **2–12 damage over 5 seconds** while the user screams.

---

## Faction Traps

### Trap Types

| Trap | ItemID | Placement | Damage | Effect |
|------|--------|-----------|--------|--------|
| **Explosion** | `0x11C1` | Any faction town | 6d10+40 | Fire damage |
| **Gas** | `0x113C` | Stronghold only | N/A | Lethal poison |
| **Saw** | `0x11AC` | Controlled town only | 6d10+40 | Slash damage |
| **Spike** | `0x11A0` | Controlled town only | 6d10+40 | Pierce damage |

### Trap Mechanics

| Property | Value |
|----------|-------|
| **Placement limit** | 15 per faction (`MaximumTraps`) |
| **Conceal period** | 1 minute (becomes invisible) |
| **Decay period** | 1 day (AOS), permanent (pre-AOS) |
| **Silver reward (alive victim)** | 20 |
| **Silver reward (dead victim)** | 40 |
| **Detect hidden chance** | `(DetectHidden.Value - 80) / 20` |

### Trap Removal Kit

| Property | Value |
|----------|-------|
| **Charges** | 25 |
| **Behavior** | Removes placed traps |
| **Deletion** | Deleted when charges reach 0 |

---

## Faction NPCs

### Guard Types

Ten guard types are available per faction, each with different combat capabilities:

| Guard Type | AI Flags | Skills | Armor | VA | Weapon |
|------------|----------|--------|-------|----|--------|
| **Henchman** | Melee | Fencing 80–90 | Studded | 8 | Spear |
| **Mercenary** | Melee, Smart | Fencing 90–100 | Chainmail | 16 | Short Spear |
| **Berserker** | Melee, Curse, Bless | Swords 100–110 | Body Sash/Kilt | 24 | Double Axe |
| **Death Knight** | Melee, Curse, Bless | Swords 100–110 | Shroud | 24 | Executioner's Axe |
| **Knight** | Melee, Magic, Smart, Curse, Bless | Swords 100–110 | Full Chain | 24 | Bardiche |
| **Dragoon** | Melee, Magic, Smart, Bless, Curse | Macing 110–120 | Full Plate | 32 | War Hammer |
| **Paladin** | Melee, Magic, Smart, Curse, Bless | Swords 110–120 | Full Plate | 32 | Halberd |
| **Sorceress** | Magic, Bless, Curse | Macing 100–110 | Leather | 24 | Quarter Staff |
| **Wizard** | Magic, Smart, Bless, Curse | Macing 110–120 | Robe | 32 | Gnarled Staff |
| **Necromancer** | Magic, Smart, Bless, Curse | Macing 110–120 | Shroud | 32 | Gnarled Staff |

### Guard AI

Guards use the `GuardAI` system with configurable spell combos:

| AI Flag | Meaning |
|---------|---------|
| `Bless` | Healing/curing/buffing spells |
| `Curse` | Poisoning/debuffing spells |
| `Melee` | Uses weapons in combat |
| `Magic` | Uses damage spells |
| `Smart` | Smart spell combos |

**Simple Combo**: Paralyze (20%) → Explosion (100%, 2.8s hold) → Poison (30%) → Energy Bolt

**Strong Combo**: Paralyze (20%) → Explosion (50%, 2.8s) → Poison (30%) → Explosion (100%, 2.8s) → Energy Bolt → Poison (30%) → Energy Bolt

**Combat Behavior**:
- Dispels summoned creatures
- Uses healing when damaged or poisoned
- Recalls if far from home and HP < 10%
- Uses stat buffs/debuffs (2% chance each)
- Uses potions when mana is low
- 20% chance for combo, 2% for random spell, 2% for buff/debuff per tick

### Guard Speech Commands

| Speech | Effect |
|--------|--------|
| `*orders*` / `*name* orders*` | Opens order mode |
| `*attack* [faction]*` | Set reaction to that faction as Attack |
| `*warn* [faction]*` | Set reaction to that faction as Warn |
| `*ignore* [faction]*` | Set reaction to that faction as Ignore |
| `*patrol*` | Patrol current location |
| `*follow*` | Follow speaker |

Reaction types: **Ignore** (no interaction), **Warn** (verbal warning), **Attack** (hostile).

### Guard Potions

| Potions | Packed Amount | Types |
|---------|--------------|-------|
| **Strong** | 6–12 | GreaterHeal, GreaterCure, GreaterStrength, GreaterAgility, TotalRefresh, GreaterExplosion |
| **Weak** | 3–8 (1–4 for henchmen) | Heal, Cure, Strength, Agility, Refresh, Explosion |

### Guard Definitions

Each guard type is configured via `GuardDefinition`:

```csharp
public class GuardDefinition {
    public Type Type;           // Guard mob type
    public int Price;           // Purchase cost
    public int Upkeep;          // Daily cost
    public int Maximum;         // Max deployable
    public int ItemID;          // Display icon
    public TextDefinition Header, Label;
}
```

### Vendor Types

Five predefined vendor types are available per town:

| Vendor | Type | ItemID | Price | Upkeep | Max |
|--------|------|--------|-------|--------|----|
| Potion Bottle | `FactionBottleVendor` | `0xF0E` | 5,000 | 1,000 | 10 |
| Wooden Board | `FactionBoardVendor` | `0x1BD7` | 3,000 | 500 | 10 |
| Iron Ore | `FactionOreVendor` | `0x19B8` | 3,000 | 500 | 10 |
| Reagent | `FactionReagentVendor` | `0xF86` | 5,000 | 1,000 | 10 |
| Horse Breeder | `FactionHorseVendor` | `0x20DD` | 5,000 | 1,000 | 1 |

Vendors are frozen and stationary, registered with their town on construction.

### Faction War Horse

| Property | Value |
|----------|-------|
| **Cost** | 500 silver + 3,000 gold |
| **BodyID** | `WarHorseBody` from FactionDefinition |
| **ItemID** | `WarHorseItem` from FactionDefinition |
| **Str** | 400 |
| **Dex** | 125 |
| **Int** | 51–55 |
| **HP** | 240 |
| **Damage** | 5–8 (physical) |
| **Resistances** | 30–40 to all |
| **Skills** | MagicResist 25–30, Tactics 29–44, Wrestling 29–44 |
| **ControlSlots** | 1 |
| **FavoriteFood** | FruitsAndVeggies \| GrainsAndHay |

**Riding Requirements**: Must be faction member, same faction as horse, Rank ≥ 2.

---

## Faction Speech Keywords

Special speech keywords allow players to interact with town guards and the system:

| Keyword ID | Speech | Effect |
|-----------|--------|--------|
| `0x00E4` | `*i wish to access the city treasury*` | Opens FinanceGump |
| `0x00ED` | `*i am sheriff*` | Opens SheriffGump |
| `0x00EF` | `*you are fired*` | Begin firing order (finance/sheriff) |
| `0x00E5` | `*i wish to resign as finance minister*` | Resign as finance minister |
| `0x00EE` | `*i wish to resign as sheriff*` | Resign as sheriff |
| `0x00E9` | `*what is my faction term status*` | Show leave timer |
| `0x00EA` | `*message faction*` | Commander broadcast prompt |
| `0x00EC` | `*showscore*` | Show kill points overhead |
| `0x0178` | `*i honor your leadership*` | Honor leadership targeting |

---

## Gumps

### FactionStoneGump (364 lines)

Main faction hub opened via FactionStone. Pages:

| Page | Content |
|------|---------|
| 1 | Leader, Tithe, Traps, Vote, City Status, Statistics, Merchant, Commander Options, Leave |
| 2 | City Status — all 8 towns with ownership indicators |
| 3 | Statistics |
| 4 | Merchant title selection (6 titles, 90 skill requirement) |
| 5 | Commander options — tithe change (0–100%, step 10), silver transfer (10,000 per town) |

### TownStoneGump (211 lines)

Hire/fire Sheriff and Finance Minister. Only the faction commander can use it. The commander cannot be elected to town positions.

### SheriffGump (169 lines)

| Page | Content |
|------|---------|
| 1 | Hire Guards, View Finances |
| 2 | Finance — silver, upkeep costs, daily income, net cash flow |
| 3 | Guard hire overview |
| 4+ | Individual guard type pages with count, max, cost, upkeep, hire button |

### FinanceGump (295 lines)

| Page | Content |
|------|---------|
| 1 | Change Prices, Buy Shopkeepers, View Finances |
| 2 | Price change — 13 radio buttons (-30% to +300%, step 5, plus normal at 0) |
| 3 | Vendor hire overview |
| 4 | Finance statement |
| 5+ | Individual vendor pages with count, max, cost, upkeep, buy button |

### ElectionGump (132 lines)

Displays election state (Pending/Campaign/Election) with countdown. Campaign page shows "CAMPAIGN FOR LEADERSHIP" button for rank ≥ 5 members. Election page shows "VOTE FOR LEADERSHIP".

### VoteGump (75 lines)

Lists candidates with vote counts. Each member can vote once per election cycle.

### LeaveFactionGump (115 lines)

Initiates a 3-day leave period (`LeavePeriod`). Guild leaders can resign their entire guild at once.

### JoinStoneGump (52 lines)

Simple join confirmation with faction info display.

### FactionImbueGump (113 lines)

Crafting integration for imbuing items. Shows item quality, silver cost, available silver, and primary/secondary color selection. Consumes silver from the player's backpack.

### HorseBreederGump (96 lines)

Purchase war horse for 500 silver + 3,000 gold. Checks follower capacity.

### ElectionManagementGump (203 lines)

GM tool to manage elections. Shows candidates, voters with legitimacy factors (skills, KP, game time), and allows removal of candidates and individual voters. Pagination: 10 voters per page.

---

## Leave and Resignation

### Leave Period

When a player initiates leaving their faction, a **3-day grace period** (`LeavePeriod = TimeSpan.FromDays(3.0)`) begins. During this period:

- The player can cancel the leave
- If the player is a guild leader, the entire guild can resign
- After 3 days, the player is fully removed from the faction

### Guild Resignation

Guild leaders can resign their entire guild from the faction through the leave gump. All guild members are removed simultaneously.

---

## Admin Commands

| Command | Description |
|---------|-------------|
| `/FactionElection` | Open election management for targeted faction stone |
| `/FactionCommander` | Set targeted player as faction commander |
| `/FactionReset` | Reset all faction data (members, items, traps) |
| `/FactionTownReset` | Reset towns, sigils, monoliths |
| `/FactionItemReset` | Reset faction item associations |
| `/FactionKick` | Kick a player from their faction |
| `/FactionBan` | Ban a player's account from factions |
| `/FactionUnban` | Remove faction ban from a player's account |

---

## Broadcast System

Faction commanders can broadcast messages to all faction members:

- **Rate limit**: Maximum 2 broadcasts per 1-hour period
- **Tracking**: `m_LastBroadcasts` array of 2 `DateTime` entries in `FactionState`
- **Slot availability**: A broadcast slot is free when `Core.Now >= lastBroadcast + 1 hour`

---

## Key Formulas Summary

| Mechanic | Formula |
|----------|---------|
| Daily Income | `10000 × (100 + Tax) / 100` |
| Kill Points Awarded | `max(min(victimKP / 10, 40), 1)` |
| Power Transfer | `max(1, victimPower / 5)`, capped at 100 |
| Silver Tithe | `silver × Tithe / 100` to faction treasury |
| Atrophy Amount | `(KP + 9) / 10` (ceiling division by 10) |
| Atrophy Cycle | Every 47 hours |
| Skill Loss | `base × 1/3` for 20 minutes |
| Vote Weight | `clamp((50 + skills×100/10000) × (100 + KP×2) × max(50 + gameTime×100/days, 100) / 10000, 0, 100)` |
| Stability Check | `(members + influx) × 100 / 300 <= smallestMembers` |
| Silver Gift Cooldown | 3 hours |
| Leave Period | 3 days |
| Item Expiration | 21 days |
| Trap Conceal | 1 minute |
| Trap Decay | 1 day (AOS) |
| Tax Change Cooldown | 12 hours |
| Town Income | Every 24 hours |
| StormsEye Damage | `40% of target HP`, capped at 10–75, split into 3 ticks over 1 second |

---

## Constants Reference

| Constant | Value | Location |
|----------|-------|----------|
| `StabilityFactor` | 300 | Faction.cs |
| `StabilityActivation` | 200 | Faction.cs |
| `SkillLossFactor` | 1/3 | Faction.cs |
| `SkillLossPeriod` | 20 minutes | Faction.cs |
| `LeavePeriod` | 3 days | Faction.cs |
| `Facet` | Map.Felucca | Faction.cs |
| `MaximumTraps` | 15 | Faction.cs |
| `SilverCaptureBonus` | 10,000 | Town.cs |
| `TaxChangePeriod` | 12 hours | Town.cs |
| `IncomePeriod` | 24 hours | Town.cs |

---

## Cross-References

- [`systems/ethics.md`](systems/ethics.md) — faction alignment tied to ethics (Hero/Evil)
- [`systems/virtues.md`](systems/virtues.md) — virtue Honor affects faction interactions
- [`systems/murder-system.md`](systems/murder-system.md) — PvP overlap with murder tracking
- [`systems/party.md`](systems/party.md) — faction restriction on party formation
- [`creatures/npcs.md`](creatures/npcs.md) — faction NPCs (guards, vendors, war horses)
- [`items/tools.md`](items/tools.md) — trap placement and faction tools
- [`expansions/timeline.md`](expansions/timeline.md) — expansion-specific sigil timelines (SE vs AOS)
