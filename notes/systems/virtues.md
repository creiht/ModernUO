# Virtues

The Virtues system tracks eight moral attributes that progress through **three advancement paths** (Seeker → Follower → Knight) based on cumulative virtue points. Each virtue has a maximum value, and four of them (Sacrifice, Justice, Compassion, Valor) decay when inactive through a periodic atrophy timer. The three activatable virtues — Honor, Sacrifice, and Valor — provide distinct gameplay mechanics: Honor modifies combat damage and rewards, Sacrifice enables resurrection and fame-for-virtue exchange, and Valor challenges champion spawns. The system uses `VirtueContext` for per-player state and persists via `GenericPersistence`.

**Source Files:**
- `Projects/UOContent/Engines/Virtues/VirtueSystem.cs` (399 lines) — core engine, level calculation, awarding, atrophy, persistence
- `Projects/UOContent/Engines/Virtues/VirtueContext.cs` (195 lines) — per-player virtue values and tracking fields
- `Projects/UOContent/Engines/Virtues/Honor.cs` (186 lines) — honor embrace mechanics, combat honor context setup
- `Projects/UOContent/Engines/Virtues/HonorContext.cs` (261 lines) — honor combat tracking, damage bonus, honor gain calculation
- `Projects/UOContent/Engines/Virtues/Compassion.cs` (38 lines) — atrophy mechanics only
- `Projects/UOContent/Engines/Virtues/Justice.cs` (344 lines) — player protection system, atrophy
- `Projects/UOContent/Engines/Virtues/Sacrifice.cs` (193 lines) — resurrection, fame-for-virtue exchange, atrophy
- `Projects/UOContent/Engines/Virtues/Valor.cs` (138 lines) — champion challenge system, atrophy
- `Projects/UOContent/Engines/Virtues/VirtueGump.cs` (153 lines) — virtue display gump with dynamic hues
- `Projects/UOContent/Engines/Virtues/VirtueInfoGump.cs`, `VirtueStatusGump.cs` — info/status UI
- `Projects/UOContent/Engines/Virtues/HonorSelfGump.cs` (30 lines) — confirmation dialog for honor embrace
- **12 total files** in Virtues/

---

## Overview

The Virtues system comprises **8 virtues** that players can advance by performing virtue-appropriate actions. Each virtue tracks a numeric value that determines the player's level within that virtue. Four virtues can be actively invoked (Honor, Sacrifice, Valor via the virtue gump; Compassion gains are automatic), while the remaining four (Humility, Spirituality, Justice, Honesty) progress passively through gameplay actions.

**Key Mechanics:**
- **8 Virtues**: Humility, Sacrifice, Compassion, Spirituality, Valor, Honor, Justice, Honesty
- **3 Advancement Paths**: Seeker (≥4000), Follower (4000–max−1), Knight (≥max)
- **Atrophy**: 4 virtues decay after 7 days of inactivity (Sacrifice, Justice, Compassion, Valor)
- **Compassion Daily Limit**: 5 gains per day (resets at midnight)
- **Honor Embrace**: Self-buff costing virtue points, duration varies by level
- **Sacrifice Resurrection**: Dead players can spend points for free resurrection
- **Valor Challenge**: Knights can summon champions; other levels can advance champions

---

## VirtueLevel Enum

| Value | Name | Description |
|-------|------|-------------|
| `0` | `None` | Value < 4000 |
| `1` | `Seeker` | Value ≥ 4000 |
| `2` | `Follower` | Value ≥ 4000 AND below max (formula: `(v + 9999) / 10000 == 1`) |
| `3` | `Knight` | Value ≥ max for that virtue |

---

## Level Thresholds

Level calculation in `VirtueSystem.GetLevel()`:

```csharp
public static VirtueLevel GetLevel(Mobile from, VirtueName virtue)
{
    var v = GetVirtues(from as PlayerMobile)?.GetValue((int)virtue) ?? 0;
    int vl;

    if (v < 4000)
    {
        vl = 0;  // None
    }
    else if (v >= GetMaxAmount(virtue))
    {
        vl = 3;  // Knight
    }
    else
    {
        vl = (v + 9999) / 10000;  // Seeker=1 or Follower=2
    }

    return (VirtueLevel)vl;
}
```

**Intermediate level formula:** `(v + 9999) / 10000` using integer division
- `v = 4000` → `(4000 + 9999) / 10000 = 13999 / 10000 = 1` → Seeker
- `v = 9999` → `19998 / 10000 = 1` → Seeker
- `v = 10000` → `19999 / 10000 = 1` → Seeker
- `v = 19999` → `29998 / 10000 = 2` → Follower
- `v = 20000` → `29999 / 10000 = 2` → Follower (unless max = 20000 for Honor → Knight)

---

## Max Values

Each virtue has a different maximum value. Knight level is reached at or above this threshold.

| Virtue | Max Value | Notes |
|--------|-----------|-------|
| `Humility` | 21000 | Default |
| `Sacrifice` | 22000 | Highest max; requires more points to reach Knight |
| `Compassion` | 21000 | Default; limited to 5 gains/day |
| `Spirituality` | 21000 | Default |
| `Valor` | 21000 | Default |
| `Honor` | 20000 | Lowest max; honor embrace cost tiers at 4399 and 10599 |
| `Justice` | 21000 | Default; has player protection sub-mechanic |
| `Honesty` | 21000 | Default |

---

## The 8 Virtues

### Humility

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Humility` |
| Index | `0` |
| Max | `21000` |
| Atrophy | No |
| Activatable | No |
| Localized Gain Message | `1052070` — "You have gained in Humility." |
| Localized Path Message | `1155811` — "You have gained a path in Humility!" |
| Localized Max Message | `1155808` — "You cannot gain more Humility." |

**Progression:** Gained passively through gameplay. No active ability.

---

### Sacrifice

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Sacrifice` |
| Index | `1` |
| Max | `22000` |
| Atrophy | Yes — 500 points per tick, 7-day cooldown |
| Activatable | Yes — via gump ID `110` |
| Localized Gain Message | `1054160` — "You have gained in sacrifice." |
| Localized Path Message | `1052008` — "You have gained a path in Sacrifice!" |
| Localized Max Message | Default — "You have achieved the highest path in this virtue." |

**Active Abilities:**

1. **Fame-for-Virtue Exchange** (alive): Sacrifice fame to a creature to gain virtue points. Requires Seeker level.
2. **Resurrection** (dead): Spend available resurrections for free resurrection. Requires Seeker level.

**Fame-for-Virtue Mechanics:**

| Condition | Gain |
|-----------|------|
| Fame < 5000 | +500 |
| Fame 5000–9999 | +1000 |
| Fame ≥ 10000 | +2000 |

**Valid Target Creatures** (must match at least one):
```csharp
m is Lich or Succubus or Daemon or EvilMage or EnslavedGargoyle or GargoyleEnforcer
```
Must be non-controlled and non-summoned.

**Validation Flow for Fame Sacrifice:**
1. Player must be alive
2. Player must not be hidden
3. Target must be a valid creature (see above)
4. Target must be ≥90% HP
5. Player must not be at highest path
6. Player must have ≥2500 fame
7. Must have waited ≥1 day since last sacrifice (`GainDelay = 1 day`)

**Resurrection Mechanics:**

Available resurrections are set by atrophy: `AvailableResurrects = (int)GetLevel(pm, Sacrifice)`

**Resurrection Validation:**
1. Player must be dead
2. Player must not be criminal
3. Player must be Seeker or higher
4. Player must have `AvailableResurrects > 0`

**Gaining Resurrections:**
- Each time a path is gained via fame sacrifice: `AvailableResurrects++` (capped at 3)
- Atrophy sets: `AvailableResurrects = (int)GetLevel(pm, Sacrifice)`

**Timers:**
| Timer | Duration |
|-------|----------|
| `GainDelay` | 1 day (between fame sacrifices) |
| `LossDelay` | 7 days (between atrophy ticks) |
| Atrophy loss amount | 500 points |

---

### Compassion

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Compassion` |
| Index | `2` |
| Max | `21000` |
| Atrophy | Yes — 500 points per tick, 7-day cooldown |
| Activatable | No (shows "This virtue is not activated through the virtue menu.") |
| Localized Gain Message | `1053002` — "You have gained in compassion." |
| Localized Path Message | Default — "You have gained a path in Compassion!" (not in switch, falls through) |
| Localized Max Message | `1053003` — "You have achieved the highest path of compassion and can no longer gain any further." |
| Daily Limit Message | `1053004` — "You must wait about a day before you can gain in compassion again." |

**Compassion Daily Limit:**

| Parameter | Value |
|-----------|-------|
| Max gains per day | 5 |
| Reset condition | `NextCompassionDay` expires (set to `Core.Now + 1 day` after each gain) |
| Atrophy loss | 500 points |
| Atrophy cooldown | 7 days |

**Tracking Fields:**
- `_compassionGains` — count of gains in current day
- `_nextCompassionDay` — when the daily counter resets

**Logic in `AwardVirtue()`:**
```csharp
if (virtue == VirtueName.Compassion)
{
    // Reset daily counter if new day
    if (virtues.CompassionGains > 0 && Core.Now > virtues.NextCompassionDay)
    {
        virtues.NextCompassionDay = DateTime.MinValue;
        virtues.CompassionGains = 0;
    }

    // Check daily limit
    if (virtues.CompassionGains >= 5)
    {
        pm.SendLocalizedMessage(1053004);
        return;
    }

    // After successful award:
    virtues.NextCompassionDay = Core.Now + TimeSpan.FromDays(1.0);
    if (++virtues.CompassionGains >= 5)
    {
        pm.SendLocalizedMessage(1053004);
    }
}
```

---

### Spirituality

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Spirituality` |
| Index | `3` |
| Max | `21000` |
| Atrophy | No |
| Activatable | No |
| Localized Gain Message | `1155832` — "You have gained in Spirituality." |
| Localized Path Message | `1155833` — "You have gained a path in Spirituality!" |
| Localized Max Message | `1155831` — "You cannot gain more Spirituality." |

**Progression:** Gained passively through gameplay. No active ability.

---

### Valor

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Valor` |
| Index | `4` |
| Max | `21000` |
| Atrophy | Yes — 250 points per tick, 7-day cooldown |
| Activatable | Yes — via gump ID `112` |
| Localized Gain Message | `1054030` — "You have gained in Valor!" |
| Localized Path Message | `1054032` — "You have gained a path in Valor!" |
| Localized Max Message | `1054031` — "You have achieved the highest path in Valor and can no longer gain any further." |
| Atrophy Message | `1054040` — "You have lost some Valor." |

**Active Ability: Champion Challenge**

Target a **Champion Idol of the Champion** to challenge a champion spawn.

**Champion Challenge Costs by Sub-Level:**

| Champion Sub-Level | Valor Points Needed | Valor Points Consumed |
|--------------------|---------------------|-----------------------|
| 0 | 2500 | 2500 |
| 1 | 5000 | 5000 |
| 2 | 10000 | 7500 |
| 3 | 20000 | 10000 |
| Knight (summon) | N/A | 11000 (summons spawn immediately) |

**Validation Flow:**
1. Target must be `IdolOfTheChampion` and not deleted
2. Idol's spawn must not be deleted
3. Player must not be hidden
4. If champion has been advanced: "already been challenged!"
5. If champion is active:
   - Check if `Valor >= needed` for the current sub-level
   - If yes: consume points, advance champion level
   - If no: "ignores your challenge, must further prove valor"
6. If champion inactive and player is Knight:
   - Consume 11000 points, start spawn immediately
7. If champion inactive and not Knight: "must be a Knight of Valor"

**Timers:**
| Timer | Duration |
|-------|----------|
| `LossDelay` | 7 days |
| Atrophy loss amount | 250 points |

---

### Honor

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Honor` |
| Index | `5` |
| Max | `20000` |
| Atrophy | No |
| Activatable | Yes — via gump ID `107` |
| Use Delay | 5 minutes |
| Localized Gain Message | `1063225` — "You have gained in Honor." |
| Localized Path Message | `1063226` — "You have gained a path in Honor!" |
| Localized Max Message | `1063228` — "You cannot gain more Honor." |

**Active Abilities:**

1. **Honor Self (Embrace)**: Self-buff costing virtue points, duration varies by level
2. **Honor Opponent**: Mark a creature for honorable combat, modifying damage and honor gain

**Embrace Mechanics:**

| Level | Duration | Cost |
|-------|----------|------|
| Seeker | 30 seconds | 400 points (if Honor < 4399) |
| Follower | 90 seconds | 600 points (if Honor < 10599) |
| Knight | 300 seconds (5 minutes) | 1000 points (if Honor ≥ 10599) |

**Embrace Validation:**
1. Player must be alive
2. Honor must not already be active (`HonorActive == false`)
3. Must have enough honor for a duration (duration > 0)
4. Must have waited 5 minutes since last embrace use

**Embrace Flow:**
1. Player targets self → opens `HonorSelfGump` (confirmation dialog)
2. Confirms → `ActivateEmbrace()` consumes points, sets `HonorActive = true`
3. After duration expires → `HonorActive = false`, records `LastHonorUse`

**Honor Opponent Mechanics:**

Target a creature to begin an honor context. The creature must:
- Implement `IHonorTarget`
- Not already be honored by someone else (or be the same honoror)
- Be at full HP (≥90% HP check: `target.Hits < target.HitsMax`)
- Not be in a guarded region if humanoid and not always-attackable
- Not be a player (ML expansion check)

**Honor Combat Tracking** (see `HonorContext` below):

---

### Justice

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Justice` |
| Index | `6` |
| Max | `21000` |
| Atrophy | Yes — 950 points per tick, 7-day cooldown |
| Activatable | Yes — via gump ID `109` |
| Localized Gain Message | `1049363` — "You have gained in Justice." |
| Localized Path Message | `1049367` — "You have gained a path in Justice!" |
| Localized Max Message | `1049534` — "You cannot gain more Justice." |

**Active Ability: Player Protection**

One player can offer protection to another player. Both must be Seeker or higher in Justice.

**Protection Flow:**
1. Protector targets a player (range 14)
2. Validation checks on both players:
   - Both must be on Felucca
   - Protector must be Seeker level
   - Protector must not have a cooldown (`CanBeginAction<JusticeVirtue>()`)
   - Protector must not already be protecting someone
   - Target must not be criminal, murderer, or already protected
   - Target must not already have a pending protection offer gump
3. Target receives `AcceptProtectorGump` with Yes/No options
4. On acceptance: both players' `JusticeStatus` and `JusticeProtection` are set bidirectionally
5. On rejection: protector gets 15-minute cooldown

**Protection States:**

```csharp
public enum JusticeProtectorStatus : byte
{
    None,
    Protector,
    Protected
}
```

| Field | Protector | Protected |
|-------|-----------|-----------|
| `JusticeStatus` | `Protector` | `Protected` |
| `JusticeProtection` | Reference to protected player | Reference to protector |

**Protection Removal:**
- `CancelProtection(pm)` — removes protection for both players
- Triggered on player deletion via `[OnEvent(PlayerDeletedEvent)]`

**Map Regions** (for region-based checks):
```csharp
// Map ID < 2 (not Felucca/Britannia/etc.) → region 0
// X < 5120 → region 0
// Y < 2304 → region 1
// Y >= 2304 → region 2
```

**Timers:**
| Timer | Duration |
|-------|----------|
| `LossDelay` | 7 days |
| Atrophy loss amount | 950 points |
| Rejection cooldown | 15 minutes |

---

### Honesty

| Property | Value |
|----------|-------|
| `VirtueName` | `VirtueName.Honesty` |
| Index | `7` |
| Max | `21000` |
| Atrophy | No |
| Activatable | No |
| Localized Gain Message | Default — "You have gained in {virtueName}." (not in switch) |
| Localized Path Message | Default — "You have gained a path in {virtueName}!" (not in switch) |
| Localized Max Message | `1153771` — "You have achieved the highest path in Honesty and can no longer gain any further." |

**Progression:** Gained passively through gameplay. No active ability.

---

## Honor Context — Combat Honor System

The `HonorContext` class tracks all honor-related combat mechanics between a player and their honored target.

### Honor Context Fields

| Field | Type | Description |
|-------|------|-------------|
| `Source` | `PlayerMobile` | The player who initiated the honor |
| `Target` | `Mobile` | The honored creature |
| `_initialLocation` | `Point3D` | Player's location when honor was initiated |
| `_initialMap` | `Map` | Player's map when honor was initiated |
| `_firstHit` | `FirstHit` | Enum: `NotDelivered`, `Delivered`, `Granted` |
| `_honorDamage` | `double` | Accumulated damage that counts for honor |
| `_totalDamage` | `int` | Total damage dealt to target |
| `_poisoned` | `bool` | Flag: was target poisoned since last damage tick? |
| `PerfectionDamageBonus` | `int` | 0–100, increases with precise hits, decreases on misses/heals |
| `PerfectionLuckBonus` | `int` | `PerfectionDamageBonus² / 10` |
| `_timer` | `InternalTimer` | 1-second tick, cancels if out of range |

### First Hit System

`FirstHit` enum tracks whether the player struck first in the honor combat:

| State | Value | Effect |
|-------|-------|--------|
| `NotDelivered` | 0 | Default; no one has struck yet |
| `Delivered` | 1 | Player struck the target (unhonorable) |
| `Granted` | 2 | Target struck first, then player responded (honorable) |

**Logic:**
- `OnSourceDamaged()`: If target damaged source and first hit was `NotDelivered` → `Granted`
- `OnTargetDamaged()`: If first hit was `NotDelivered` → `Delivered`

### Damage Tracking

| Method | Condition | Honor Damage Multiplier |
|--------|-----------|------------------------|
| `OnTargetDamaged()` | Source attacks, target can see source AND (in range 1 OR same location as initial) | `×1.0` (full) |
| `OnTargetDamaged()` | Source attacks, other conditions | `×0.8` |
| `OnTargetDamaged()` | Source's creature attacks (GetMaster == Source) | `×0.8` |
| `OnTargetDamaged()` | Target was poisoned (flag set by `OnTargetPoisoned()`) | `×0.8` (poison damage), flag cleared |
| `OnSourceBeneficialAction(target)` | Healing/buffing the honored target | Resets `PerfectionDamageBonus` to 0 |

### Perfection System

Perfection tracks how precisely the player fights their honored opponent. Ranges from 0 to 100.

**Increasing Perfection:**
```csharp
// On OnTargetHit() when source hits target
var bushido = (int)from.Skills.Bushido.Value;
if (bushido < 50) return;
PerfectionDamageBonus += bushido / 10;
// Capped at 100
```

| Bushido Skill | Perfection Gain per Hit |
|---------------|------------------------|
| 50 | +5 |
| 60 | +6 |
| 70 | +7 |
| 80 | +8 |
| 90 | +9 |
| 100 | +10 |

**Decreasing Perfection:**
| Trigger | Amount |
|---------|--------|
| `OnTargetMissed()` | −25 (resets to 0 if below) |
| `OnSourceBeneficialAction(target)` | Resets to 0 |

**Perfection Rewards on Target Kill:**
```
restore = min(PerfectionDamageBonus × (targetFame + 5000) / 25000, 10)
Source.Hits += restore
Source.Stam += restore
Source.Mana += restore
```

### Honor Gain Formula

When the honored target dies:

```
targetFame = Target.Fame

// Base gain: 1/100th of target's fame, weighted by damage ratio
dGain = targetFame / 100.0 × (_honorDamage / _totalDamage)

// Honorable combat bonus
if (Math.Abs(_honorDamage - _totalDamage) < 0.01 && _firstHit == FirstHit.Granted)
    dGain *= 1.5;  // Full honor + target struck first
else
    dGain *= 0.9;   // Unhonorable actions

// Clamp: minimum 1, maximum 200
gain = clamp((int)dGain, 1, 200)
```

**Honor check after gain:** If `Honor > targetFame`, no honor is awarded (you're already "honorable" enough).

### Distance Check

The honor context timer checks every second:
```csharp
bool CheckDistance() => Utility.InRange(Source.Location, Target.Location, 18);
```

If source and target are more than 18 tiles apart, the context cancels.

### Honor Timer

Two timers manage the honor context lifecycle:
1. **InternalTimer**: 1-second interval, cancels context if out of range
2. **DelayedCall**: 40-minute timeout, cancels context if `_honorTime` expires

The `Source._honorTime` is set to `Core.Now + 40 minutes` when the context is created.

---

## Core Engine

### `VirtueSystem` — Main Class

Defined in `Projects/UOContent/Engines/Virtues/VirtueSystem.cs`:

**Persistence:**
- Extends `GenericPersistence` with filename `"Virtues"`, version `10`
- Stores `Dictionary<PlayerMobile, VirtueContext>`
- On version 0 deserialization: reads player count, then serializes each player+context pair

**Static Methods:**

| Method | Return | Description |
|--------|--------|-------------|
| `Configure()` | `void` | Creates `VirtueSystem` singleton via `new VirtueSystem()` |
| `Initialize()` | `void` | Processes `Mobile.VirtueMigrations` — applies saved virtue values to players being migrated |
| `GetVirtues(PlayerMobile)` | `VirtueContext` | Returns player's virtue context, or `null` |
| `GetOrCreateVirtues(PlayerMobile)` | `VirtueContext` | Returns existing or creates new virtue context |
| `GetLevel(Mobile, VirtueName)` | `VirtueLevel` | Calculates virtue level using formula: `<4000=None, ≥max=Knight, else (v+9999)/10000` |
| `IsHighestPath(PlayerMobile, VirtueName)` | `bool` | `true` if virtue value ≥ max for that virtue |
| `GetName(VirtueName)` | `string` | PascalCase name (e.g., "Honor", "Sacrifice") |
| `GetLowerCaseName(VirtueName)` | `string` | Lowercase name (e.g., "honor", "sacrifice") |
| `GetMaxAmount(VirtueName)` | `int` | Max value: Honor=20000, Sacrifice=22000, others=21000 |
| `GetGainedLocalizedMessage(VirtueName)` | `int` | Locale-specific "You have gained in X." message ID |
| `GetGainedAPathLocalizedMessage(VirtueName)` | `int` | Locale-specific "You have gained a path in X!" message ID |
| `GetHightestPathLocalizedMessage(VirtueName)` | `int` | Locale-specific "You cannot gain more X." message ID |
| `Award(PlayerMobile, VirtueName, int, ref bool gainedPath)` | `bool` | Core award logic. Returns `true` on success, sets `gainedPath` if level changed |
| `Atrophy(PlayerMobile, VirtueName, int)` | `bool` | Removes points; returns `true` if value was > 0 before |
| `IsSeeker(PlayerMobile, VirtueName)` | `bool` | `GetLevel() >= VirtueLevel.Seeker` |
| `IsFollower(PlayerMobile, VirtueName)` | `bool` | `GetLevel() >= VirtueLevel.Follower` |
| `IsKnight(PlayerMobile, VirtueName)` | `bool` | `GetLevel() >= VirtueLevel.Knight` |
| `AwardVirtue(PlayerMobile, VirtueName, int)` | `void` | Public entry point: handles Compassion daily limit, calls `Award()`, sends messages |
| `CheckAtrophies(PlayerMobile)` | `void` | Calls atrophy checks for all 4 atrophying virtues |
| `OnPlayerDeleted(PlayerMobile)` | `void` | Removes player from `_playerVirtues` dictionary |

**Award Logic:**
```csharp
public static bool Award(PlayerMobile from, VirtueName virtue, int amount, ref bool gainedPath)
{
    var virtues = from.Virtues;
    var current = virtues.GetValue((int)virtue);
    var maxAmount = GetMaxAmount(virtue);

    if (current >= maxAmount)
        return false;

    // Clamp to max
    if (current + amount >= maxAmount)
        amount = maxAmount - current;

    var oldLevel = GetLevel(from, virtue);
    virtues.SetValue((int)virtue, current + amount);
    gainedPath = GetLevel(from, virtue) != oldLevel;

    return true;
}
```

### `VirtueTimer` — Periodic Atrophy Timer

| Property | Value |
|----------|-------|
| Interval | 5 minutes |
| Delay | 5 minutes |
| Checked virtues | Sacrifice, Justice, Compassion, Valor |
| Cleanup | Removes `VirtueContext` entries that `IsUsed()` returns `false` for |

**Tick Logic:**
1. For each player in `_playerVirtues`:
   - Call `CheckAtrophies(player)` — each virtue checks its own cooldown and loss amount
   - If `!virtues.IsUsed()` (no active data), queue player for removal
2. Dequeue and remove all queued players

**`IsUsed()` Check:** Returns `true` if ANY of these fields have non-default values:
- `_lastSacrificeGain`, `_lastSacrificeLoss`, `_availableResurrects`
- `_lastJusticeLoss`, `_justiceStatus`, `_justiceProtection`
- `_nextCompassionDay`, `_compassionGains`, `_lastCompassionLoss`
- `_lastValorLoss`, `_lastHonorUse`, `_honorActive`
- `_values` (the virtue value array)

---

## VirtueGump — Virtue Display

The `VirtueGump` displays all 8 virtues as gump images with dynamic hues based on the observed player's virtue values.

**Registration:**
| Virtue | Gump ID | Position (x, y) | Body ID |
|--------|---------|-----------------|---------|
| Humility | `108` | (61, 71) | `108` |
| Sacrifice | `110` | (35, 135) | `110` |
| Compassion | `105` | (211, 133) | `105` |
| Spirituality | `111` | (61, 195) | `111` |
| Valor | `112` | (123, 46) | `112` |
| Honor | `107` | (187, 70) | `107` |
| Justice | `109` | (186, 195) | `109` |
| Honesty | `106` | (121, 221) | `106` |

**Hue Calculation (`GetHueFor`):**

```csharp
private int GetHueFor(int index)
{
    var value = VirtueSystem.GetVirtues(_beheld)?.GetValue(index) ?? 0;

    if (value < 4000)
        return 2402;  // None level — neutral hue

    if (value >= 30000)
        value = 20000;  // Cap for hue calculation

    var vl = value switch
    {
        < 10000 => 0,  // Seeker
        >= 20000 when index == 5 => 2,  // Honor Follower (max=20000, so this is unreachable for Knight)
        >= 22000 when index == 1 => 2,  // Sacrifice Follower (max=22000, same)
        >= 21000 when index != 1 => 2,  // Follower (most virtues max=21000)
        _ => 1  // Seeker (for virtues with value >= 10000 but below their max)
    };

    return _table[index * 3 + vl];
}
```

**Hue Table** (`_table` array, 24 entries = 8 virtues × 3 levels):

| Index | None (vl=0) | Seeker (vl=1) | Follower (vl=2) | Virtue |
|-------|-------------|---------------|-----------------|--------|
| 0–2 | `0x0481` | `0x0963` | `0x0965` | Humility |
| 3–5 | `0x060A` | `0x060F` | `0x002A` | Sacrifice |
| 6–8 | `0x08A4` | `0x08A7` | `0x0034` | Compassion |
| 9–11 | `0x0965` | `0x08FD` | `0x0480` | Spirituality |
| 12–14 | `0x00EA` | `0x0845` | `0x0020` | Valor |
| 15–17 | `0x0011` | `0x0269` | `0x013D` | Honor |
| 18–20 | `0x08A1` | `0x08A3` | `0x0042` | Justice |
| 21–23 | `0x0543` | `0x0547` | `0x0061` | Honesty |

**Access Controls:**
- Murderers cannot invoke virtues: `"Murderers cannot invoke this virtue."` (`1049609`)
- Unregistered virtues show: `"That virtue is not active yet."` (`1052066`)
- Request requires: same map, within 12 tiles

**Virtue Invocation Paths:**
1. **Via gump**: `RequestVirtueGump(beholder, beheld)` → shows gump with all 8 virtues → click triggers callback
2. **Via item**: `RequestVirtueItem(beholder, beheld, gumpID)` → looks up gumpID callback
3. **Via macro**: `RequestVirtueMacro(beholder, virtue)` — maps: `0=Honor(107)`, `1=Sacrifice(110)`, `2=Valor(112)`

---

## VirtueContext — Per-Player State

Defined in `Projects/UOContent/Engines/Virtues/VirtueContext.cs`:

**Serialized Fields** (serialization version 0):

| Field Index | Field Name | Type | Default | Serialized When |
|-------------|-----------|------|---------|-----------------|
| 0 | `_lastSacrificeGain` | `DateTime` | `MinValue` | `!SacrificeVirtue.CanGain(this)` |
| 1 | `_lastSacrificeLoss` | `DateTime` | `MinValue` | `!SacrificeVirtue.CanAtrophy(this)` |
| 2 | `_availableResurrects` | `int` | `0` | `> 0` |
| 3 | `_lastJusticeLoss` | `DateTime` | `MinValue` | `!JusticeVirtue.CanAtrophy(this)` |
| 4 | `_lastCompassionLoss` | `DateTime` | `MinValue` | `!CompassionVirtue.CanAtrophy(this)` |
| 5 | `_nextCompassionDay` | `DateTime` | `MinValue` | `> Core.Now` |
| 6 | `_compassionGains` | `int` | `0` | `> 0` |
| 7 | `_lastValorLoss` | `DateTime` | `MinValue` | `!ValorVirtue.CanAtrophy(this)` |
| 8 | `_lastHonorUse` | `DateTime` | `MinValue` | `!HonorVirtue.CanUse(this)` |
| 9 | `_honorActive` | `bool` | `false` | `true` |
| 10 | `_justiceProtection` | `PlayerMobile` | `null` | `!= null && status != None` |
| 11 | `_justiceStatus` | `JusticeProtectorStatus` | `None` | `protection != null && status != None` |
| 12 | `_values` | `int[]` (size 8) | `null` | Any value `> 0` |

**Property Accessors:**

| Property | Index | Type | Description |
|----------|-------|------|-------------|
| `Humility` | 0 | `int` | Virtue value |
| `Sacrifice` | 1 | `int` | Virtue value |
| `Compassion` | 2 | `int` | Virtue value |
| `Spirituality` | 3 | `int` | Virtue value |
| `Valor` | 4 | `int` | Virtue value |
| `Honor` | 5 | `int` | Virtue value |
| `Justice` | 6 | `int` | Virtue value |
| `Honesty` | 7 | `int` | Virtue value |
| `LastSacrificeGain` | — | `DateTime` | Last fame sacrifice timestamp |
| `LastSacrificeLoss` | — | `DateTime` | Last sacrifice atrophy timestamp |
| `AvailableResurrects` | — | `int` | Number of free resurrects available |
| `LastJusticeLoss` | — | `DateTime` | Last justice atrophy timestamp |
| `LastCompassionLoss` | — | `DateTime` | Last compassion atrophy timestamp |
| `NextCompassionDay` | — | `DateTime` | When compassion daily counter resets |
| `CompassionGains` | — | `int` | Gains in current day |
| `LastValorLoss` | — | `DateTime` | Last valor atrophy timestamp |
| `LastHonorUse` | — | `DateTime` | Last honor embrace use timestamp |
| `HonorActive` | — | `bool` | Whether honor embrace is active |
| `JusticeProtection` | — | `PlayerMobile` | The other player in a protection pair |
| `JusticeStatus` | — | `JusticeProtectorStatus` | Protector or Protected |
| `_values` | — | `int[]` | Raw virtue values array (size 8) |

**Value Methods:**
```csharp
public int GetValue(int index) => _values?[index] ?? 0;
public void SetValue(int index, int value)
{
    _values ??= new int[8];
    _values[index] = value;
}
```

---

## Message IDs Reference

### Gain Messages ("You have gained in X.")

| Virtue | Message ID | Text |
|--------|-----------|------|
| Sacrifice | `1054160` | "You have gained in sacrifice." |
| Compassion | `1053002` | "You have gained in compassion." |
| Spirituality | `1155832` | "You have gained in Spirituality." |
| Valor | `1054030` | "You have gained in Valor!" |
| Honor | `1063225` | "You have gained in Honor." |
| Justice | `1049363` | "You have gained in Justice." |
| Humility | `1052070` | "You have gained in Humility." |
| Honesty | — | "You have gained in {virtueName}." (fallback) |

### Path Messages ("You have gained a path in X!")

| Virtue | Message ID | Text |
|--------|-----------|------|
| Sacrifice | `1052008` | "You have gained a path in Sacrifice!" |
| Spirituality | `1155833` | "You have gained a path in Spirituality!" |
| Valor | `1054032` | "You have gained a path in Valor!" |
| Honor | `1063226` | "You have gained a path in Honor!" |
| Justice | `1049367` | "You have gained a path in Justice!" |
| Humility | `1155811` | "You have gained a path in Humility!" |
| Compassion | — | Fallback message |
| Honesty | — | Fallback message |

### Max Messages ("You cannot gain more X.")

| Virtue | Message ID | Text |
|--------|-----------|------|
| Compassion | `1053003` | "You have achieved the highest path of compassion and can no longer gain any further." |
| Spirituality | `1155831` | "You cannot gain more Spirituality." |
| Valor | `1054031` | "You have achieved the highest path in Valor and can no longer gain any further." |
| Honor | `1063228` | "You cannot gain more Honor." |
| Justice | `1049534` | "You cannot gain more Justice." |
| Humility | `1155808` | "You cannot gain more Humility." |
| Honesty | `1153771` | "You have achieved the highest path in Honesty and can no longer gain any further." |
| Default | `1052050` | "You have achieved the highest path in this virtue." |

### Honor Messages

| ID | Text |
|----|------|
| `1063160` | "Target what you wish to honor." |
| `1063166` | "You cannot honor this monster because it is too damaged." |
| `1063230` | "You must wait awhile before you can embrace honor again." |
| `1063232` | "You are too far away to honor your opponent" |
| `1063233` | "Somebody else is honoring this opponent" |
| `1063234` | "You do not have enough honor to do that" |
| `1063235` | "You embrace your honor" |
| `1063236` | "You no longer embrace your honor" |
| `1063240` | "You must wait ~1_HONOR_WAIT~ minutes before embracing honor again" (param: minutes) |
| `1071218` | "Are you sure you want to use honor points on yourself?" |
| `1063254` | "You have Achieved Perfection in inflicting damage to this opponent!" |
| `1063255` | "You gain in Perfection as you precisely strike your opponent." |
| `1063256` | "You have lost all Perfection in fighting this opponent." |
| `1063257` | "You have lost some Perfection in fighting this opponent." |
| `1075614` | "You cannot honor other players." (ML expansion) |

### Justice Messages

| ID | Text |
|----|------|
| `1049366` | "Choose the player you wish to protect." |
| `1049369` | "You cannot protect that player right now." |
| `1049370` | "You must wait a while before offering your protection again." |
| `1049372` | "You cannot use this ability here." |
| `1049373` | "You have lost some Justice." |
| `1049436` | "That player cannot be protected." |
| `1049444` | "Yes, I would like their protection." |
| `1049445` | "No thanks, I can take care of myself." |
| `1049451` | "You are now being protected by ~1_NAME~." |
| `1049452` | "You are now protecting ~2_NAME~." |
| `1049453` | "You have declined protection from ~1_NAME~." |
| `1049454` | "~2_NAME~ has declined your protection." |
| `1049542` | "You cannot protect someone while being protected." |
| `1049609` | "Murderers cannot invoke this virtue." |
| `1049610` | "You must reach the first path in this virtue to invoke it." |
| `1049678` | "Only players can be protected." |

### Sacrifice Messages

| ID | Text |
|----|------|
| `1052004` | "You cannot use this ability." |
| `1052005` | "You do not have any resurrections left." |
| `1052007` | "You cannot use this ability while flagged as a criminal." |
| `1052008` | "You have gained a path in Sacrifice!" |
| `1052009` | "I have seen the error of my ways!" (creature overhead message) |
| `1052010` | "You have set the creature free." |
| `1052013` | "You cannot sacrifice for this monster because it is too damaged." |
| `1052014` | "You cannot sacrifice your fame for that creature." |
| `1052015` | "You cannot do that while hidden." |
| `1052016` | "You must wait approximately one day before sacrificing again." |
| `1052017` | "You do not have enough fame to sacrifice." |
| `1052041` | "You have lost some Sacrifice." |
| `1052068` | "You have already attained the highest path in this virtue." |

### Valor Messages

| ID | Text |
|----|------|
| `1054034` | "Target the Champion Idol of the Champion you wish to challenge!" |
| `1054035` | "You must target a Champion Idol to challenge the Champion's spawn!" |
| `1054036` | "You must be a Knight of Valor to summon the champion's spawn in this manner!" |
| `1054037` | "Your challenge is heard by the Champion of this region! Beware its wrath!" |
| `1054038` | "The Champion of this region has already been challenged!" |
| `1054039` | "The Champion of this region ignores your challenge. You must further prove your valor." |
| `1054040` | "You have lost some Valor." |
| `1112470` | "You may not use Valor on this Champion Idol. The Champion has already spawned." |

### Compassion Messages

| ID | Text |
|----|------|
| `1053001` | "This virtue is not activated through the virtue menu." |
| `1053004` | "You must wait about a day before you can gain in compassion again." |

---

## Cross-References

- [`systems/ethics.md`](systems/ethics.md) — Both moral progression systems (ethics + virtues)
- [`systems/factions.md`](systems/factions.md) — Virtue Honor affects faction interactions
- [`systems/combat.md`](systems/combat.md) — Honor combat mechanics, damage tracking
- [`creatures/npcs.md`](creatures/npcs.md) — Virtue-related NPCs and champion idols
- [`systems/party.md`](systems/party.md) — Party system (no direct virtue integration)
- [`systems/murder-system.md`](systems/murder-system.md) — Murderer flag blocks virtue invocation
