# Murder System

The murder system tracks player-killing behavior with short-term and long-term murder states. It includes kill decay timers, a bounty board system for reporting and placing bounties, and expansion-variant kill report messages. When a player kills another player, their murder status decays over time unless they commit more murders. Other players can report murderers upon death, and bounties in gold can be placed on repeat offenders.

**Source Files:**
- `Projects/UOContent/Engines/Player Murder System/PlayerMurderSystem.cs` (433 lines) — core engine, persistence, decay timer, report logic
- `Projects/UOContent/Engines/Player Murder System/MurderContext.cs` (130 lines) — per-player tracking, serializable state
- `Projects/UOContent/Engines/Player Murder System/BountyBoard.cs` (90 lines) — bounty bulletin board integration
- `Projects/UOContent/Engines/Player Murder System/BountyMessage.cs` (393 lines) — bounty message formatting and display
- `Projects/UOContent/Engines/Player Murder System/ReportMurdererGump.cs` (176 lines) — murder report gump UI
- `Projects/UOContent/Engines/Player Murder System/BountyReportMurdererGump.cs` (83 lines) — bounty placement gump UI

---

## Config Settings

Configured via `ServerConfiguration.GetOrUpdateSetting()` during server startup:

| Setting | Default | Type | Description |
|---------|---------|------|-------------|
| `murderSystem.shortTermMurderDuration` | `8h` | `TimeSpan` | Interval before short-term murder decays |
| `murderSystem.longTermMurderDuration` | `40h` | `TimeSpan` | Interval before long-term murder (Kills) decays |
| `murderSystem.bountiesEnabled` | `!Core.LBR` | `bool` | Whether bounty boards are available (disabled in LBR+) |
| `murderSystem.recentlyReportedDelay` | `10min` | `TimeSpan` | Cooldown for re-reporting same killer |
| `murderSystem.bountyExpiry` | `14d` | `TimeSpan` | How long bounties remain active (`TimeSpan.Zero` disables expiry) |

---

## Core Engine (`PlayerMurderSystem`)

The murder system is managed by the static `PlayerMurderSystem` class, which extends `GenericPersistence` for save/load support.

### Static Fields

| Field | Type | Purpose |
|-------|------|---------|
| `_murderContexts` | `Dictionary<PlayerMobile, MurderContext>` | All players who have ever had a murder recorded |
| `_contextTerms` | `HashSet<MurderContext>` | Players currently online (uses `MurderContext.EqualityComparer`) |
| `_recentlyReported` | `HashSet<(Mobile, Mobile)>` | Prevents duplicate reports of the same killer by the same reporter |
| `_shortTermMurderDuration` | `TimeSpan` | Short-term murder decay interval |
| `_longTermMurderDuration` | `TimeSpan` | Long-term murder decay interval |
| `_bountyExpiry` | `TimeSpan` | Bounty expiry duration |
| `_recentlyReportedDelay` | `TimeSpan` | Re-report cooldown |

### Public Static Properties

| Property | Type | Description |
|----------|------|-------------|
| `ShortTermMurderDuration` | `TimeSpan` | Configurable short-term murder decay interval |
| `LongTermMurderDuration` | `TimeSpan` | Configurable long-term murder decay interval |
| `PingPongEnabled` | `bool` | `Core.T2A && !Core.LBR` — T2A expansion only, disabled in LBR |
| `BountiesEnabled` | `bool` | Configurable bounty availability |

### Key Static Methods

| Method | Return | Description |
|--------|--------|-------------|
| `Configure()` | `void` | Reads config settings from server configuration |
| `Initialize()` | `void` | Subscribes to `EventSink.Disconnected` |
| `OnPlayerDeleted(Mobile)` | `void` | Cleans up murder context on character deletion |
| `OnLogin(PlayerMobile)` | `void` | Restarts kill decay timers on login |
| `OnDisconnected(Mobile)` | `void` | Decays kills and cleans up context on disconnect |
| `MigrateContext(PlayerMobile, TimeSpan, TimeSpan)` | `void` | Migration helper during world loading from old system |
| `Deserialize(IGenericReader)` | `void` | Loads murder contexts from disk (version 0) |
| `Serialize(IGenericWriter)` | `void` | Persists murder contexts to disk |
| `GetMurderContext(PlayerMobile, out MurderContext)` | `bool` | Lookup helper, returns false if no context exists |
| `GetOrCreateMurderContext(PlayerMobile)` | `MurderContext` | Creates context if none exists |
| `GetBounty(PlayerMobile)` | `int` | Returns current bounty amount |
| `AddBounty(PlayerMobile, int)` | `void` | Adds to a player's bounty |
| `ClearBounty(PlayerMobile)` | `void` | Sets bounty to 0 |
| `GetActiveBountyCount()` | `int` | Counts active (non-expired) bounties |
| `GetActiveBounties()` | `List<(PlayerMobile, int)>` | Sorted by bounty descending |
| `OnPlayerMurder(PlayerMobile)` | `void` | Increments ShortTermMurders, Kills, possibly PingPongs |
| `IsRecentlyReported(Mobile, Mobile)` | `bool` | Checks if reporter already reported this killer |
| `ReportMurder(PlayerMobile, Mobile)` | `bool` | Core murder report logic |
| `ReportKillsToSelf(PlayerMobile)` | `void` | Sends kill status messages (expansion-variant) |
| `ManuallySetPingPong(PlayerMobile, int)` | `void` | GM command to set ping pongs |
| `ManuallySetShortTermMurders(PlayerMobile, int)` | `void` | GM command to set short-term murders |

### Nested Timer: `MurdererTimer`

Runs every **5 minutes**, iterating online murderer contexts and calling `DecayKills()` on each. Expired contexts are removed from the active set.

```csharp
// MurdererTimer.OnTick()
foreach (var context in PooledRefQueue<MurdererTimer>.DequeueAll())
{
    var pm = context._player;
    if (pm == null || pm.Deleted)
        continue;

    context.DecayKills();

    if (context.CanRemove())
    {
        _contextTerms.Remove(context);
        _murderContexts.Remove(pm);
    }
}
```

---

## MurderContext

Each player who has committed at least one murder has a `MurderContext` instance. The class uses `[SerializationGenerator(2)]` for auto-serialization and is persisted to disk.

### Serializable Fields

| ID | Field | Type | Description |
|----|-------|------|-------------|
| 0 | `_shortTermElapse` | `TimeSpan` | Wall clock time until next short-term murder expires |
| 1 | `_longTermElapse` | `TimeSpan` | Wall clock time until next long-term murder expires |
| 2 | `ShortTermMurders` | `int` | Number of short-term murders (setter clamps to `Math.Max(0, value)`) |
| 3 | `_pingPongs` | `int` | Ping pong count (T2A only) |
| 4 | `_bounty` | `int` | Bounty amount in gold |
| 5 | `_lastMurderTime` | `DateTime` | Timestamp of last murder |

### Non-Serializable Fields

| Field | Type | Description |
|-------|------|-------------|
| `_player` | `PlayerMobile` | The player this context belongs to |
| `_nextElapse` | `DateTime` | Internal: wall clock time of next expiration event |

### Key Methods

| Method | Return | Description |
|--------|--------|-------------|
| `ResetKillTime()` | `void` | Sets `_shortTermElapse` and `_longTermElapse` to current game time + respective duration |
| `DecayKills()` | `void` | Decrements ShortTermMurders and Kills based on elapsed wall-clock time |
| `CanRemove()` | `bool` | Returns true if pingPongs <= 0, shortTermMurders <= 0, and Kills <= 0 |
| `CheckStart()` | `bool` | Computes `_nextElapse` (minimum of next short and long term expiry). Returns true if active kills exist. |

### Decay Formula

```
If ShortTermMurders > 0 and _shortTermElapse < gameTime:
    ShortTermElapse += ShortTermMurderDuration
    ShortTermMurders--

If Kills > 0 and _longTermElapse < gameTime:
    LongTermElapse += LongTermMurderDuration
    Kills--
```

Both decays use real-world TimeSpan durations compared against game-time progression (not pure wall-clock).

### Migration Methods

| Method | Source | Behavior |
|--------|--------|----------|
| `MigrateFrom(V0Content)` | V0 | Migrates from version 0, sets ping pongs based on `Kills >= 5 ? 1 : 0` |
| `MigrateFrom(V1Content)` | V1 | Migrates from version 1, preserves ping pongs, sets `_lastMurderTime = Core.Now` |

### Equality Comparer

Compares by `_player` reference identity. Hash code derived from `_player.GetHashCode()`.

---

## Kill States & Decay

The murder system tracks **two independent murder states** that decay on separate timers.

### Short-Term Murders

- Decays every **8 hours** (configurable via `shortTermMurderDuration`)
- Resets the long-term decay timer when a new murder is committed
- Triggers visual/status messages when thresholds are crossed

### Long-Term Murders (Kills)

- Decays every **40 hours** (configurable via `longTermMurderDuration`)
- Permanent record of player-killing until fully decayed
- Required for bounty eligibility (`Kills >= 4`)

### Ping Pong (T2A Only)

Available when `Core.T2A && !Core.LBR`. Ping Pongs are a separate counter from Kills that allows shorter-term murder decay reset.

| Condition | Effect |
|-----------|--------|
| Player reaches exactly 5 Kills (and `PingPongEnabled`) | `PingPongs++` |
| Player uses Ping Pong | Short-term murder timer resets |

Ping Pongs are migrated from legacy V0 data: `Kills >= 5 ? 1 : 0`.

---

## Report Flow

When a player dies, other players involved in the combat can report the killer. The report flow uses events and gumps to coordinate the process.

### Step 1: Death Event

`PlayerMobile.PlayerDeathEvent` fires when a player dies. `ReportMurdererGump.OnPlayerDeathEvent()` handles the event:

```
1. Reject Thieves Guild members: m.NpcGuild != NpcGuild.ThievesGuild
2. Iterate m.Aggressors:
   a. Attacker is PlayerMobile
   b. ai.CanReportMurder == true
   c. ai.Reported == false
   d. !IsRecentlyReported(reporter, killer)
3. Also iterate m.Aggressed:
   a. Defender is PlayerMobile
   b. Last combat within 30 seconds
4. Karma/Fame awards for defenders (see below)
5. If killers exist and not Thieves Guild: start 4-second GumpTimer
```

### Karma & Fame Awards

| Condition | Karma Award | Fame Award |
|-----------|-------------|------------|
| Defender Notoriety == Innocent | `ourKarma > -2500 ? -850 : -110 - m.Karma / 100` | `m.Fame / 200` |
| Defender Notoriety == Criminal or Murderer | `50` | `m.Fame / 200` |

### Step 2: Report Gump (`ReportMurdererGump`)

After 4 seconds, the reporter sees a gump showing the killer's name:

| Element | Value |
|---------|-------|
| Background | 320x290 at (265, 205), type 5054 |
| Message | Loc 1049066: "Would you like to report..." |
| Button 1 | Yes (gump IDs 0xFA5/0xFA7) → Calls `PlayerMurderSystem.ReportMurder()` |
| Button 2 | No (gump IDs 0xFA5/0xFA7) → Dismisses |

Multiple killers are shown sequentially (advances `_idx` after each response).

### Step 3: Report Murder (`PlayerMurderSystem.ReportMurder`)

```
1. Validate killer is not deleted and is a PlayerMobile
2. Check IsRecentlyReported — return false if already reported
3. Schedule removal from _recentlyReported after _recentlyReportedDelay
4. Record whether killer was already a murderer
5. Call OnPlayerMurder(pk) to increment counters
6. Send "You have been reported for murder!" to PK
7. If killer just became a murderer: "You are now known as a murderer!"
8. If Stealing.SuspendOnMurder && PK.Kills == 1 && PK in Thieves Guild:
    Send Loc 501562 (Thieves Guild suspension message)
```

### Step 4: Bounty Gump (If Bounties Enabled)

If `BountiesEnabled` is true, `BountyReportMurdererGump` is shown after the report:

| Element | Value |
|---------|-------|
| Background | 393x270 at (265, 205), type 70000, image 1140 overlay |
| Message | Dynamic: `"Would you like to report {_killers[_idx].Name} as a murderer?"` |
| Bounty field | Shown if killer.Kills >= 4 AND victim has bank balance > 0 |
| Max bounty | Victim's current bank balance |
| Button 1 | Yes → Report + optionally place bounty |
| Button 2 | No → Dismisses |

### Bounty Placement

```
1. Parse text entry for bounty amount
2. If valid and > 0: bounty = Math.Min(requested, Banker.GetBalance(from))
3. If Banker.Withdraw(from, bounty) succeeds:
   a. PlayerMurderSystem.AddBounty(pk, bounty)
  b. PK receives: `"{reporter.Name} has placed a bounty of {bounty} {(bounty==1?"gold piece":"gold pieces")} on your head!"`
    c. Reporter receives: `"You place a bounty of {bounty}gp on {pk.Name}'s head."`
```

### Bounty Eligibility

| Condition | Requirement |
|-----------|-------------|
| Killer must have | `Kills >= 4` (for bounty feature; report itself has no kill minimum) |
| Victim must have | Bank balance > 0 |
| Max bounty | Victim's current bank balance |
| Bounty expiry | Configurable (`bountyExpiry`, default 14 days) |

---

## Bounty Board

The bounty board uses a synthetic bulletin board system — no real `BulletinMessage` items are created. All data is synthesized live from `MurderContext` data.

### BountyBoard Item

| Property | Value |
|----------|-------|
| Class | `BountyBoard : BaseBulletinBoard` |
| ItemID | `0x1E5E` |
| Board name | "bounty board" |
| Post allowed | No — always returns "You are not allowed to post to this bulletin board." |
| Remove allowed | No (packets 5 and 6 blocked) |

### Synthetic Serial System

Player serials (max `0x3FFFFFFF`) are offset by `0x80000000` to map into the unused BB packet range (`0x80000001` to `0xBFFFFFFF`). On request, the base offset is subtracted to recover the real player serial.

### Packet Handling

| Packet ID | Action |
|-----------|--------|
| 3 (Content) | Sends bounty info via `BountyMessage.SendBountyContainerContent()` |
| 4 (Header) | Sends bounty info via `BountyMessage.SendBountyBBMessage()` |
| 5 (Post) | Blocked |
| 6 (Remove) | Blocked |

### Bounty Message Display

Each bounty entry shows:

| Element | Content |
|---------|---------|
| Subject | "{bounty} gold" |
| Body | Player name, last murder time, body/hue, equipment list |
| ItemID | `0xEB0` (bulletin message item) |
| Entry size | 20 bytes with grid lines, 19 without |
| Packet | 0x3C (container content), 0x71 (BB message) |
| Writer | `SpanWriter` for zero-allocation packet building |

---

## Bounty Message Content (`BountyMessage`)

The `BountyMessage` static class formats bounty text using randomized phrases and character appearance data.

### Random Title Lines (6 variants)

1. "Bounty for {Name}!"
2. "{Name} must die!"
3. "A price on {Name}!"
4. "{Name} outlawed!"
5. "Execute {Name}!"
6. "WANTED: {Name}!"

### Random Verb Phrases (18 variants)

Examples: "hath murdered one too many!", "is a bloodthirsty monster.", "sheds innocent blood!", "must die for all our sakes."

### Random Intro Phrases (7 variants)

1. "A bounty is hereby offered"
2. "Lord British sets a price"
3. "Claim the reward! 'Tis"
4. "Lord Blackthorn set a price"
5. "The Paladins set a price"
6. "The Merchants set a price"
7. "Lord British's bounty"

### Main Paragraph Format

```
The foul scum known as {Name} {verb}. For {pronoun} is responsible for {Kills} murders. {intro} of {bounty} gold pieces for {possessive} head!
```

Wrapped at 28 characters per line.

### Physical Description

The bounty includes a description of the target's appearance based on their current character model.

#### Hair Style Mapping

| ItemID | Style |
|--------|-------|
| 0x203B | hair worn short |
| 0x203C | hair worn long |
| 0x203D | hair tied back |
| 0x2044 | a mohawk hairstyle |
| 0x2045 | pageboy hair |
| 0x2046 | hair tied in buns |
| 0x2047 | curly hair |
| 0x2048 | receding hairline |
| 0x2049 | hair in two pigtails |
| 0x204A | shaved head and topknot |
| other | bald |

#### Hair Color Mapping

| Hue(s) | Color |
|--------|-------|
| 0 | indeterminate color |
| 0x44E–0x450 | white |
| 0x451–0x453 | graying |
| 0x454–0x455 | black |
| 0x456–0x458 | copper |
| 0x459–0x45C | brown |
| 0x45D | reddish brown |
| 0x45E–0x460 | blonde |
| 0x461–0x463 | light brown |
| 0x464–0x465 | golden brown |
| 0x466–0x468 | golden |
| 0x469–0x46B | bronze |
| 0x46C–0x46D | dark brown |
| 0x46E–0x46F | sandy |
| 0x470–0x472 | honey-colored |
| 0x473–0x475 | red |
| 0x476–0x478 | nut brown |
| 0x479–0x47B | rich brown |
| 0x47C–0x47D | very dark brown |
| other | outlandishly colored |

#### Skin Tone Mapping

| Hue(s) | Tone |
|--------|------|
| 0x3EA, 0x3EB, 0x3F9–0x3FB | fair |
| 0x3F1, 0x3F2, 0x3F8, 0x3FF–0x400, 0x406–0x408, 0x40D–0x40E, 0x415–0x416 | pale |
| 0x3F3, 0x3F4, 0x3FC, 0x401–0x402, 0x409, 0x40F–0x411, 0x418 | tanned |
| 0x3EC–0x3EE, 0x3F5, 0x403, 0x412–0x413, 0x419, 0x421 | copper |
| 0x3EF–0x3FE, 0x404–0x40B, 0x40C, 0x414, 0x41A–0x41B, 0x420 | dark |
| 0x41C–0x41E | yellow |
| other | deathly |

### Closing Instructions

```
If you kill {objective}, remove the head, and give it to a guard to claim your reward.
```

---

## Kill Report Messages (`ReportKillsToSelf`)

When a player checks their murder status, the message format varies by expansion:

| Expansion | Behavior |
|-----------|----------|
| SA | Single message with short-term and long-term kills tab-separated |
| SE | One `SendMessage` call (short-term and long-term on same line) |
| AOS | Two `SendMessage` calls + Ping Pong count if enabled |
| T2A with PingPong >= 5 | Loc 502123 (hue 0x22 red if short-term >= 5, 0x59 yellow otherwise) |
| T2A with short-term >= 5 | Loc 502126 (hue 0x22 red) |
| T2A with short-term > 0 | Loc 502125 (hue 0x59 yellow) |
| T2A with kills > 0 | Loc 502124 (hue 0x59 yellow) |
| T2A with no kills | Loc 502122 (hue 0x59 yellow) |
| Pre-T2A | No message at all |

---

## Cross-References

- [`../systems/factions.md`](../systems/factions.md) — Faction kill point mechanics
- [`../systems/virtues.md`](../systems/virtues.md) — Honor virtue interaction
- [`../skills/combat-skills.md`](../skills/combat-skills.md) — Murderer status affects combat
- [`../items/weapons.md`](../items/weapons.md) — Poison application by murderers
- [`../skills/utility-skills.md`](../skills/utility-skills.md) — Thieves Guild suspension on murder
