# Party

The Party system enables up to 10 players to group together for cooperative play, shared chat, and coordinated activities. It handles party membership, invitations, messaging (public, private, and staff-observable), stat syncing, and faction-based restrictions. This document covers the party system as implemented in the AOS/SE-era client protocol.

**Source Files:**
- `Projects/UOContent/Engines/Party/Party.cs` (550 lines) — core party container, member management, messaging
- `Projects/UOContent/Engines/Party/PartyMemberInfo.cs` (15 lines) — per-member tracking (CanLoot flag)
- `Projects/UOContent/Engines/Party/PartyPackets.cs` (153 lines) — packet creation for member list, removal, text messages, invitations
- `Projects/UOContent/Engines/Party/DeclineTimer.cs` (37 lines) — 30-second auto-decline timer for invitations
- `Projects/UOContent/Engines/Party/AddPartyTarget.cs` (61 lines) — add member targeting logic
- `Projects/UOContent/Engines/Party/RemovePartyTarget.cs` (34 lines) — remove member targeting logic
- `Projects/UOContent/Engines/Party/RemoveFromParty.cs` (30 lines) — context menu entry for removing members
- `Projects/UOContent/Engines/Party/PartyCommands.cs` (150 lines) — slash command handlers (OnAdd, OnRemove, messaging, canloot, accept/decline)
- `Projects/Server/Party.cs` (14 lines) — abstract `PartyCommands` base class with virtual hooks
- `Projects/Server/Interfaces.cs` (lines 48-53) — `IParty` interface

---

## Core Engine

### Party Class

The `Party` class (`Server.Engines.PartySystem.Party`) serves as the core container, implementing `IParty`. It tracks members, candidates (pending invitations), and staff listeners.

```csharp
public class Party : IParty
{
    public const int Capacity = 10;
    private readonly HashSet<Mobile> m_Listeners;  // staff listening to party chat
    
    public int Count => Members.Count;
    public bool Active => Members.Count > 1;
    public Mobile Leader { get; }
    public List<PartyMemberInfo> Members { get; }
    public List<Mobile> Candidates { get; }
    
    // Indexers: by int position or by Mobile reference
    public PartyMemberInfo this[int index] => Members[index];
    public PartyMemberInfo this[Mobile m] => ...;  // linear search
}
```

### IParty Interface

Defined in `Server/Interfaces.cs`, the minimal interface:

```csharp
public interface IParty
{
    void OnStamChanged(Mobile m);
    void OnManaChanged(Mobile m);
    void OnStatsQuery(Mobile beholder, Mobile beheld);
}
```

### Mobile.Party Property

Each `Mobile` has an `object Party` property (defined in `Mobile.cs:449`). It holds:
- `null` — not in a party
- `Party` — a reference to the `Party` instance
- `Mobile` — while accepting/declining an invitation (temporary holder)

The static helper `Party.Get(Mobile m)` casts `m.Party` to `Party`.

### Configuration

The party system is configured via `Party.Configure()`:
- Registers logout event handler
- Registers `/ListenToParty` GM command

No config flags control the party system — it is always active.

---

## Party Capacity and Structure

### Capacity

Maximum party size is **10 members**, including candidates (pending invites):

```csharp
public const int Capacity = 10;
```

The capacity check is performed in `AddPartyTarget` (line 30-32):

```csharp
if (p.Members.Count + p.Candidates.Count >= Party.Capacity)
{
    from.SendLocalizedMessage(1008095); // You may only have 10 in your party (this includes candidates).
}
```

### Active Status

A party is considered "active" when more than one member is present:

```csharp
public bool Active => Members.Count > 1;
```

---

## Invitation System

### Creating a Party

Parties are created lazily when the leader sends the first invitation. In `Party.Invite(Mobile from, Mobile target)` (line 342):

```csharp
var p = Get(from);
if (p == null)
{
    from.Party = p = new Party(from);
}
```

### Faction Restriction

Before inviting, faction compatibility is checked (line 344-352):

```csharp
var ourFaction = Faction.Find(from);
var theirFaction = Faction.Find(target);

if (ourFaction != null && theirFaction != null && ourFaction != theirFaction)
{
    from.SendLocalizedMessage(1008088);   // You cannot have players from opposing factions in the same party!
    target.SendLocalizedMessage(1008093); // The party cannot have members from opposing factions.
    return;
}
```

This check uses `Faction.Find()` which returns the faction of the given mobile, or `null` if they have no faction. Only opposing factions are blocked — same-faction or non-faction players can always party together.

### Invitation Flow

1. `Party.Invite(from, target)` is called (typically via `/party add <target>`)
2. Target is added to `Candidates` list
3. Target receives a localized message and a `SendPartyInvitation` packet
4. `DeclineTimer` starts with a 30-second timeout
5. Target's `Party` property is set to the leader `Mobile` (temporary)

### Accepting an Invitation

`PartyCommandHandlers.OnAccept(from, sentLeader)` (line 116-131):

1. Retrieves leader from `from.Party` (temporary Mobile holder)
2. Clears `from.Party`
3. Looks up party via `Party.Get(leader)`
4. Validates leader exists and from is in Candidates
5. Checks capacity
6. Calls `p.OnAccept(from)`

`Party.OnAccept(Mobile from, bool force = false)` (line 223-257):

```csharp
var ourFaction = Faction.Find(Leader);
var theirFaction = Faction.Find(from);

if (!force && ourFaction != null && theirFaction != null && ourFaction != theirFaction)
{
    return;  // Hard-faction mismatch at accept time
}
```

Then broadcasts the join message and calls `Add(from)`.

The `force` parameter (default `false`) is used by staff to bypass faction restrictions.

### Declining an Invitation

`PartyCommandHandlers.OnDecline(from, sentLeader)` (line 133-148):

1. Retrieves leader from `from.Party` (temporary Mobile holder)
2. Clears `from.Party`
3. Validates leader exists and from is in Candidates
4. Calls `p.OnDecline(from, leader)`

`Party.OnDecline(Mobile from, Mobile leader)` (line 259-282):

1. Notifies leader with localized message (1008091)
2. Notifies target (1008092)
3. Removes from Candidates
4. Sends `SendPartyRemoveMember` to target
5. If party is empty (no candidates, only 1 member), disbands the party

### Decline Timer

`DeclineTimer` (`Server.Engines.PartySystem.DeclineTimer`) is a 30-second timer that auto-declines invitations:

```csharp
private DeclineTimer(Mobile m, Mobile leader) : base(TimeSpan.FromSeconds(30.0))
```

- Uses a `Dictionary<Mobile, DeclineTimer>` to track active timers per target
- If the target hasn't responded within 30 seconds, calls `PartyCommands.Handler.OnDecline(m_Mobile, m_Leader)`
- Timer is cancelled and replaced if the same target receives a new invitation

---

## Member Management

### Adding a Member

`Party.Add(Mobile m)` (line 189-221):

1. Checks if mobile is already a member (via index lookup)
2. Creates `PartyMemberInfo` and adds to `Members` list
3. Sets `m.Party = this`
4. Allocates packets using stackalloc:
   - `PartyMemberList` packet (all members' serials)
   - `MobileAttributes` packet for the new member
5. Sends member list to all members
6. Sends compact status and attributes to/from both new and existing members

### Removing a Member

`Party.Remove(Mobile m)` (line 284-324):

1. If removing the leader, calls `Disband()` immediately
2. Otherwise, removes from `Members` list
3. Sends `SendPartyRemoveMember` to the removed player
4. Sets `m.Party = null`
5. Sends localized message to removed player (1005451: "You have been removed from the party.")
6. Broadcasts removal to all remaining members
7. If only 1 member remains, broadcasts (1005450: "The last person has left the party...") and calls `Disband()`

### Disbanding

`Party.Disband()` (line 328-340):

1. Broadcasts (1005449: "Your party has disbanded.")
2. Sends `SendPartyRemoveMember` to every member
3. Clears `m.Party = null` for all members
4. Clears `Members` list

### Contains Check

`Party.Contains(Mobile m)` uses the `this[Mobile m]` indexer, which performs a linear search through the `Members` list.

---

## Stamina/Mana Sync

### OnStamChanged

`Party.OnStamChanged(Mobile m)` (line 53-68):

1. Creates a `MobileAttributePacket` with the target's current stamina
2. Iterates all members
3. For each member not equal to the target, checks:
   - Has active network connection (`NetState != null`)
   - Same map (`m.Map == c.Map`)
   - In update range (`Utility.InUpdateRange(c.Location, m.Location)`)
   - Can see the target (`c.CanSee(m)`)
4. Sends the stamina packet to qualifying members

### OnManaChanged

`Party.OnManaChanged(Mobile m)` (line 70-85):

Same logic as `OnStamChanged`, but sends a `MobileMana` packet instead.

---

## Stats Query

`Party.OnStatsQuery(Mobile beholder, Mobile beheld)` (line 87-99):

When one party member views another's stats:

1. Validates: different players, beholder is in the party, same map, in update range
2. If beholder cannot see beheld: sends `SendMobileStatusCompact`
3. Always sends `SendMobileAttributes`

This ensures party members see full stats for each other regardless of line-of-sight.

---

## Messaging

### Public Messages

`Party.SendPublicMessage(Mobile from, string text)` (line 402-411):

1. Creates a `PartyTextMessage` packet with `toAll = true`
2. Sends to all party members via `SendToAll`
3. Sends formatted message to listeners: `[{from.Name}]: {text}`
4. Sends staff-visible message: `[Party]: {text}`

### Private Messages

`Party.SendPrivateMessage(Mobile from, Mobile to, string text)` (line 413-418):

1. Sends to target only: `SendPartyTextMessage(from.Serial, text, false)`
2. Sends formatted message to listeners: `[{from.Name}]->[{to.Name}]: {text}`
3. Sends staff-visible message: `[Party]->[{to.Name}]: {text}`

Message length is capped at 128 characters in `PartyCommandHandlers.OnPrivateMessage`.

### SendToAll / SendToAllListeners

- `SendToAll(Span<byte> span)` — sends raw packet buffer to all party members
- `SendToAllListeners(Span<byte> span)` — sends to all staff listeners (excluding party members)
- `SendToAllListeners(string text)` — creates a formatted message packet and sends to listeners

### Staff Listening

GMs can listen to party chat via the `/ListenToParty` command:

```
/ListenToParty [target player]
```

- Toggles listening on/off
- Listeners are stored in a `HashSet<Mobile> m_Listeners`
- Staff must not already be in the party to listen
- Staff within 8 tiles who are GM and have higher access than the speaker also receive messages via `SendToStaffMessage` (line 420-452), even without explicit `/ListenToParty`

---

## Death Messages

`Party.OnPlayerDeathEvent(Mobile from)` (line 139-161) is triggered by the `PlayerMobile.PlayerDeathEvent` event:

| Scenario | Message |
|----------|---------|
| Suicide (`LastKiller == from`) | "I killed myself !!" |
| Unknown killer (`LastKiller == null`) | "I was killed !!" |
| Killed by another | "I was killed by {killer.Name} !!" |

---

## Login/Logout Handling

### Login

`Party.OnLogin(PlayerMobile from)` (line 163-176):

- If the player was in a party before logout, starts a `RejoinTimer` (1-second delay)
- If not in a party, clears `from.Party = null`

### RejoinTimer

Inner class `Party.RejoinTimer` (line 489-548):

1. Waits 1 second after login
2. Checks if player still has a party reference
3. If yes:
   - Sends "You have rejoined the party." message
   - Sends party member list
   - Broadcasts rejoin message to all members
   - Exchanges compact status and attributes between rejoining player and all other members

### Logout

`Party.EventSink_Logout(Mobile from)` (line 178-185):

1. Calls `p?.Remove(from)` — which handles leader disbanding if needed
2. Sets `from.Party = null`

---

## PartyMemberInfo

`Server.Engines.PartySystem.PartyMemberInfo` (15 lines):

```csharp
public class PartyMemberInfo
{
    public Mobile Mobile { get; }
    public bool CanLoot { get; set; }
    
    public PartyMemberInfo(Mobile m)
    {
        Mobile = m;
        CanLoot = !Core.ML;  // True unless ML expansion is active
    }
}
```

The `CanLoot` flag controls whether party members can loot the player's corpse. It defaults to `true` in Core/AOS/SE/SA, and `false` in ML expansion.

---

## Slash Commands

Implemented in `PartyCommandHandlers` (`Server.Engines.PartySystem.PartyCommandHandlers`), which extends the abstract `PartyCommands` base class.

### Command Handlers

| Method | Description |
|--------|-------------|
| `OnAdd(Mobile from)` | Opens `AddPartyTarget` for adding members (leader only, capacity check) |
| `OnRemove(Mobile from, Mobile target)` | If leader with no target: opens `RemovePartyTarget`. If leader with target or non-leader targeting self: removes directly |
| `OnPrivateMessage(Mobile from, Mobile target, string text)` | Sends private message (128 char limit, party member only) |
| `OnPublicMessage(Mobile from, string text)` | Sends public message (128 char limit) |
| `OnSetCanLoot(Mobile from, bool canLoot)` | Toggles corpse loot permission |
| `OnAccept(Mobile from, Mobile sentLeader)` | Processes party accept |
| `OnDecline(Mobile from, Mobile sentLeader)` | Processes party decline |

### Initialize

`PartyCommandHandlers.Initialize()` sets the static `PartyCommands.Handler` singleton.

---

## Context Menu

`RemoveFromPartyEntry` (`Server.ContextMenus.RemoveFromPartyEntry`):

- Context menu option (entry ID 0198)
- Only visible/active when the player is the party leader
- Leader cannot remove themselves (must use `/party remove` or `/party leave`)
- Calls `p.Remove(mobile)` on click

---

## Add/Remove Targeting

### AddPartyTarget

`Server.Engines.PartySystem.AddPartyTarget`:

- 8 tile range, non-compass targeting
- Validates: not self, leader check, capacity, target not already in another party, target is a player
- Non-players (NPCs) respond with flavor text or rejection
- Calls `Party.Invite(from, m)` on success

### RemovePartyTarget

`Server.Engines.PartySystem.RemovePartyTarget`:

- 8 tile range, non-compass targeting
- Leader-only validation
- Leader cannot target themselves
- Calls `p.Remove(m)` on success

---

## Packet Format

All party packets use the **0xBF** main packet ID with **sub-packet 0x06**.

### Party Member List (Command 0x01)

```
0xBF (1 byte)
Length (2 bytes)
0x06 (2 bytes) — sub-packet
0x01 (1 byte)  — command: member list
Party Count (1 byte)
Serial × Count (4 bytes each)
```

Length formula: `7 + partyCount × 4`

### Party Remove Member (Command 0x02)

```
0xBF (1 byte)
Length (2 bytes)
0x06 (2 bytes) — sub-packet
0x02 (1 byte)  — command: remove member
Party Count (1 byte)
Removed Serial (4 bytes)
Serial × Remaining Count (4 bytes each)
```

Length formula: `11 + partyCount × 4`

### Party Text Message (Commands 0x03/0x04)

```
0xBF (1 byte)
Length (2 bytes)
0x06 (2 bytes) — sub-packet
0x03 (1 byte)  — command: private message
0x04 (1 byte)  — command: public message
Sender Serial (4 bytes)
Text (variable, null-terminated wide string)
```

Length formula: `12 + text.Length × 2`

### Party Invitation (Command 0x07)

```
0xBF (1 byte)
Length (2 bytes) — always 10
0x06 (2 bytes) — sub-packet
0x07 (1 byte)  — command: invitation
Leader Serial (4 bytes)
```

Total: 10 bytes (fixed)

---

## Message Localization IDs

| ID | Message | Context |
|----|---------|---------|
| 1005437 | "You have rejoined the party." | Login rejoin |
| 1005439 | "You cannot add yourself to a party." | Add self |
| 1005440 | "This person is already in your party!" | Already member |
| 1005441 | "This person is already in a party!" | Already in another party |
| 1005442 | "You may only add living things to your party!" | Invalid target |
| 1005443 | "Nay, I would rather stay here and watch a nail rust." | NPC rejects invite |
| 1005444 | "The creature ignores your offer." | Non-human creature |
| 1005445 | "You have been added to the party." | Join confirmation |
| 1005446 | "You may only remove yourself from a party if you are not the leader." | Leader self-remove |
| 1005447 | "You have chosen to allow your party to loot your corpse." | CanLoot true |
| 1005448 | "You have chosen to prevent your party from looting your corpse." | CanLoot false |
| 1005449 | "Your party has disbanded." | Disband broadcast |
| 1005450 | "The last person has left the party..." | Last member leaves |
| 1005451 | "You have been removed from the party." | Removal notification |
| 1005452 | "A player has been removed from your party." | Removal broadcast |
| 1005453 | "You may only add members to the party if you are the leader." | Non-leader add |
| 1005454 | "Who would you like to add to your party?" | Add prompt |
| 1005455 | "Who would you like to remove from your party?" | Remove prompt |
| 1008087 | "{name} has joined the party." | Join broadcast (affix) |
| 1008088 | "You cannot have players from opposing factions in the same party!" | Faction conflict |
| 1008089 | "You are invited to join the party..." | Invite received |
| 1008090 | "You have invited them to join the party." | Invite sent |
| 1008091 | "{name} does not wish to join the party." | Decline to leader |
| 1008092 | "You notify them that you do not wish to join the party." | Decline confirmation |
| 1008093 | "The party cannot have members from opposing factions." | Faction to inviter |
| 1008094 | "{name} joined the party." | Accept broadcast (affix) |
| 1008095 | "You may only have 10 in your party (this includes candidates)." | Capacity full |
| 3000211 | "You are not in a party." | General not-in-party |
| 3000222 | "No one has invited you to be in a party." | No pending invite |

---

## Cross-References

- [`systems/factions.md`](systems/factions.md) — faction restrictions on party formation
- [`systems/murder-system.md`](systems/murder-system.md) — murder system interaction with party deaths
- [`skills/combat-skills.md`](skills/combat-skills.md) — party combat coordination
