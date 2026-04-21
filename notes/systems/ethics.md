# Ethics

The Ethics system is a moral alignment mechanic that divides players into two opposing factions: **Hero** and **Evil**. Each ethic provides access to **8 unique powers** ranging from detection abilities to area effects, summons, and protective buffs. Ethics require faction membership for eligibility, are activated through speech events near Ankh statues, and can imbue items with ethic-specific properties. The entire system is controlled by the `ethics.enable` configuration setting.

**Source Files:**
- `Projects/UOContent/Engines/Ethics/Core/Ethic.cs` (282 lines) — base ethic class, registration, trade/equip checks, speech event handler
- `Projects/UOContent/Engines/Ethics/Core/EthicsSystem.cs` (59 lines) — persistence layer via `GenericEntityPersistence`
- `Projects/UOContent/Engines/Ethics/Core/EthicsEntity.cs` — base entity with serialization metadata
- `Projects/UOContent/Engines/Ethics/Core/Player.cs` — per-player ethic profile, power/history levels, summoned creatures
- `Projects/UOContent/Engines/Ethics/Core/Power.cs` — abstract power base class, invoke check, power cost deduction
- `Projects/UOContent/Engines/Ethics/Definitions/EthicDefinition.cs` (29 lines) — ethic definition data class
- `Projects/UOContent/Engines/Ethics/Definitions/PowerDefinition.cs` — power definition data class
- `Projects/UOContent/Engines/Ethics/Hero/` — Hero ethic and 8 powers, 2 creature types
- `Projects/UOContent/Engines/Ethics/Evil/` — Evil ethic and 8 powers, 2 creature types
- **28 total files** in Ethics/

---

## Overview

The Ethics system provides a binary moral choice that affects combat interactions, item properties, and available abilities. Players must join a faction first (TrueBritannians/CouncilOfMages for Hero, Minax/Shadowlords for Evil) before they can become eligible for the corresponding ethic.

Upon joining, players receive a `Player` profile that tracks their power level, history level, and references to their summoned familiar and steed. Powers consume power points from this level, and most powers can be invoked repeatedly as long as sufficient power remains.

**Configuration:**
```
ethics.enable = false  # Default: disabled on most servers
```

---

## Ethic Types

Two ethics exist, each with a distinct hue, join phrase, and set of powers.

### Hero Ethic

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HeroEthic` |
| Primary Hue | `0x482` |
| Title | "Hero" |
| Adjunct | "(Hero)" |
| Join Phrase | "I will defend the virtues" |
| Powers | 8 (Holy Sense, Holy Item, Summon Familiar, Holy Blade, Bless, Holy Shield, Holy Steed, Holy Word) |

**Eligibility:**
```csharp
public override bool IsEligible(Mobile mob)
{
    return !mob.Murderer && Faction.Find(mob) is TrueBritannians or CouncilOfMages;
}
```

Conditions:
1. Player must **not** be a murderer (`!mob.Murderer`)
2. Player must be a member of **TrueBritannians** or **CouncilOfMages** faction

### Evil Ethic

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.EvilEthic` |
| Primary Hue | `0x455` |
| Title | "Evil" |
| Adjunct | "(Evil)" |
| Join Phrase | "I am evil incarnate" |
| Powers | 8 (Unholy Sense, Unholy Item, Summon Familiar, Vile Blade, Blight, Unholy Shield, Unholy Steed, Unholy Word) |

**Eligibility:**
```csharp
public override bool IsEligible(Mobile mob)
{
    return Faction.Find(mob) is Minax or Shadowlords;
}
```

Conditions:
1. Player must be a member of **Minax** or **Shadowlords** faction

### Power Cost Comparison

| Power | Hero Cost | Evil Cost | Hero Phrase | Evil Phrase |
|-------|-----------|-----------|-------------|-------------|
| Sense | 0 | 0 | "Drewrok Erstok" | "Drewrok Velgo" |
| Item Imbue | 5 | 5 | "Vidda K'balc" | "Vidda K'balc" |
| Summon Familiar | 5 | 5 | "Trubechs Vingir" | "Trubechs Vingir" |
| Weapon Buff | 10 | 10 | "Erstok Reyam" | "Velgo Reyam" |
| Area Effect | 15 | 15 | "Erstok Ontawl" | "Velgo Ontawl" |
| Shield | 20 | 20 | "Erstok K'blac" | "Velgo K'blac" |
| Summon Steed | 30 | 30 | "Trubechs Yeliab" | "Trubechs Yeliab" |
| Ultimate | 100 | 100 | "Erstok Oostrac" | "Velgo Oostrac" |

---

## Core Engine

### `EthicsEntity` — Base Entity Class

All ethics system entities inherit from `EthicsEntity`, providing serialization and persistence infrastructure.

| Property | Type | Description |
|----------|------|-------------|
| `Created` | `DateTime` | Set to `Core.Now` at construction |
| `Serial` | `Serial` | Assigned from `EthicsSystem.NewProfile` at construction |
| `Deleted` | `bool` | Set to `true` when `Delete()` is called |

### `Ethic` — Abstract Base Class

Defined in `Projects/UOContent/Engines/Ethics/Core/Ethic.cs`:

| Static Member | Type | Description |
|---------------|------|-------------|
| `Hero` | `Ethic` | Singleton instance of HeroEthic |
| `Evil` | `Ethic` | Singleton instance of EvilEthic |
| `Enabled` | `bool` | Set by `Configure()` from config setting |

**Static Methods:**

| Method | Return | Description |
|--------|--------|-------------|
| `RegisterEthic(Ethic)` | `bool` | Registers ethic singleton; `HeroEthic` → `Ethics[0]`, `EvilEthic` → `Ethics[1]`; returns `true` on success |
| `Find(Item)` | `Ethic` | Identifies which ethic an item is imbued with via `SavedFlags` and hue matching |
| `CheckTrade(Mobile, Mobile, Mobile, Item)` | `bool` | Validates trade — denies if buyer's ethic differs from item's ethic |
| `CheckEquip(Mobile, Item)` | `bool` | Validates equip — denies if equipping player's ethic differs from item's ethic |
| `IsImbued(Item)` / `IsImbued(Item, bool)` | `bool` | Checks if item has ethic flag; recursive variant checks child items |
| `Find(Mobile, bool, bool)` | `Ethic` | Resolves ethic for a mobile; supports inheritance via `EthicAllegiance` for creatures |
| `Configure()` | `void` | Reads `ethics.enable` config setting |
| `Initialize()` | `void` | Hooks `EventSink.Speech` if enabled |

**Abstract Members:**
- `abstract bool IsEligible(Mobile mob)` — Implemented by each ethic subclass
- `abstract virtual EthicDefinition Definition { get; }` — Returns the ethic's definition data

### `Player` — Per-Player Profile

Defined in `Projects/UOContent/Engines/Ethics/Core/Player.cs`:

| Field | Type | Default | Description |
|-------|------|---------|-------------|
| `_mobile` | `Mobile` | — | Reference to the player's mobile |
| `_power` | `int` | `5` | Current power level (consumed by powers) |
| `_history` | `int` | `5` | History level (GM/Admin property, no known gameplay effect) |
| `_steed` | `Mobile` | `null` | Reference to summoned steed (HolySteed/UnholySteed) |
| `_familiar` | `Mobile` | `null` | Reference to summoned familiar (HolyFamiliar/UnholyFamiliar) |
| `_shield` | `DateTime` | `MinValue` | Shield activation timestamp |
| `_ethic` | `Ethic` | — | Reference to parent ethic |

| Property | Type | Description |
|----------|------|-------------|
| `IsShielded` | `bool` | `true` if `_shield != MinValue` AND `Core.Now < _shield + 1 hour` |
| `Mobile` | `Mobile` | The player's mobile |
| `Power` | `int` | Current power level (GM/Admin property) |
| `History` | `int` | History level (GM/Admin property) |
| `Steed` | `Mobile` | Summoned steed reference (GM/Admin property) |
| `Familiar` | `Mobile` | Summoned familiar reference (GM/Admin property) |
| `Ethic` | `Ethic` | Parent ethic |

**Static Methods:**

| Method | Return | Description |
|--------|--------|-------------|
| `Find(Mobile)` / `Find(Mobile, bool)` | `Player` | Looks up player profile; clears profile if no longer eligible |
| `Find(Mobile, bool)` with `inherit` | `Player` | For creatures, resolves via `GetMaster()` |

**Instance Methods:**

| Method | Description |
|--------|-------------|
| `BeginShield()` | Sets `_shield = Core.Now` (activates shield) |
| `FinishShield()` | Sets `_shield = DateTime.MinValue` (expires shield) |
| `CheckAttach()` | If `IsEligible(Mobile)`, calls `Attach()` |
| `Attach()` | Sets `mobile.EthicPlayer = this`, adds to `Ethic.Players` |
| `Detach()` | Sets `mobile.EthicPlayer = null`, removes from `Ethic.Players` |

### `Power` — Abstract Power Base

Defined in `Projects/UOContent/Engines/Ethics/Core/Power.cs`:

| Property | Type | Description |
|----------|------|-------------|
| `Definition` | `PowerDefinition` | Power configuration data |

| Method | Return | Description |
|--------|--------|-------------|
| `CheckInvoke(Player)` | `bool` | Validates: player is alive, has sufficient power level (`from.Power >= Definition.Power`); sends "You lack the power to invoke this ability." (color `0x3B2`) if insufficient |
| `BeginInvoke(Player)` | `void` | Abstract — implemented by each power |
| `FinishInvoke(Player)` | `void` | Default: `from.Power -= Definition.Power` |

### `EthicDefinition` and `PowerDefinition`

**`EthicDefinition` constructor parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `primaryHue` | `int` | Ethic color hue |
| `title` | `TextDefinition` | Display title |
| `adjunct` | `TextDefinition` | Name suffix (e.g., "(Hero)") |
| `joinPhrase` | `TextDefinition` | Speech phrase to join |
| `powers` | `Power[]` | Array of 8 powers |

**`PowerDefinition` constructor parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `power` | `int` | Power level required to invoke |
| `name` | `TextDefinition` | Power display name |
| `phrase` | `TextDefinition` | Speech phrase to invoke |
| `description` | `TextDefinition` | Power description text |

---

## Speech Event System

The `EventSink_Speech` handler in `Ethic.cs` processes all ethic-related speech events. There are two distinct flows: joining an ethic and invoking powers.

### Joining an Ethic

When a player speaks and does **not** yet have a `Player` profile:

```
1. Iterate both Ethics (Hero, Evil)
2. For each ethic:
   a. Check ethic.IsEligible(e.Mobile)
   b. Check if speech matches ethic.Definition.JoinPhrase.String (case-insensitive)
   c. Check for AnkhNorth or AnkhWest tile within range 2 of the player
3. If all conditions met:
   a. Create new Player(ethic, e.Mobile)
   b. Call pl.Attach()
   c. Play effect: FixedEffect(0x373A, 10, 3) and PlaySound(0x209)
   d. If player is in duel context, skip power invocation (no automatic first-power)
```

**Key Requirements:**
- Player must be eligible for the ethic (no murderer flag + correct faction)
- Player must speak the **exact join phrase** (case-insensitive)
- An **AnkhNorth** or **AnkhWest** statue must be within **2 tiles** of the player

### Invoking Powers

When a player speaks and **already has** a `Player` profile:

```
1. Get pl = Player.Find(e.Mobile)
2. Iterate through pl.Ethic.Definition.Powers[]
3. For each power:
   a. Check if speech matches power.Definition.Phrase.String (case-insensitive)
   b. If match: power.CheckInvoke(pl) → power.BeginInvoke(pl)
```

**Note:** Each power has a unique invocation phrase. The phrase matching is case-insensitive.

---

## Hero Powers

### 1. Holy Sense

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HolySense` |
| Power Cost | `0` |
| Invocation Phrase | "Drewrok Erstok" |

Detects nearby Evil ethic players with directional indication.

**Range Formula:**
```
maxRange = 18 + from.Power
opponentRange = Math.Max(18, maxRange - pl.Power)
```

**Logic:**
1. Iterates through `Ethic.Evil.Players`
2. Filters: must be on the same map, must be alive
3. Tracks `enemyCount` and `primary` (enemy with highest power level)
4. Sends overhead message in color `0x59`:
   - "You sense X enemy" (singular) or "You sense X enemies" (plural)
   - If single enemy found: adds directional indicator

**Direction Mapping:**

| Direction Value | Output Word |
|-----------------|-------------|
| East | "to the east" |
| North | "to the north" |
| South | "to the south" |
| Up | "to the north-west" |
| Down | "to the south-east" |
| Left | "to the south-west" |
| Right | "to the north-east" |
| Other/Unknown | "to the west" |

---

### 2. Holy Item

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HolyItem` |
| Power Cost | `5` |
| Invocation Phrase | "Vidda K'balc" |

Imbues an unowned item with Hero ethic properties, changing its hue and setting an internal flag.

**Targeting:** Range 12, no movement cancel.

**Validation:**
1. Target must be an `Item`
2. Item must be parented to the player (`item.Parent != from.Mobile`)
3. Item must NOT have `SavedFlags & 0x100` (Hero) or `SavedFlags & 0x200` (Evil) set
4. Item type must be `Spellbook`, `BaseClothing`, `BaseArmor`, or `BaseWeapon`
5. Item must have **no custom name** (`item.Name == null`)

**On Success:**
- Sets `item.Hue = 0x482` (Hero primary hue)
- Sets `item.SavedFlags |= 0x100`
- Plays `FixedEffect(0x375A, 10, 20)` and `PlaySound(0x209)`

**Item Flags:**
| Flag | Value | Ethic |
|------|-------|-------|
| Hero Imbue | `0x100` | Hero |
| Evil Imbue | `0x200` | Evil |
| Either Imbued | `0x300` | Combined check |

---

### 3. Summon Familiar (Hero)

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.SummonFamiliar` |
| Power Cost | `5` |
| Invocation Phrase | "Trubechs Vingir" |

Summons a `HolyFamiliar` (silver wolf) mount.

**Logic:**
1. If existing `from.Familiar?.Deleted == true`, clears the reference
2. If `from.Familiar != null`: sends "You already have a holy familiar."
3. Checks `from.Mobile.Followers + 1 > from.Mobile.FollowersMax`
4. Spawns via `BaseCreature.Summon(familiar, from.Mobile, from.Mobile.Location, 0x217, TimeSpan.FromHours(1.0))`
5. On success: `from.Familiar = familiar`

**Summon Duration:** 1 hour

---

### 4. Holy Blade

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HolyBlade` |
| Power Cost | `10` |
| Invocation Phrase | "Erstok Reyam" |

**Status: NOT IMPLEMENTED** — `BeginInvoke()` is an empty stub.

---

### 5. Bless

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.Bless` |
| Power Cost | `15` |
| Invocation Phrase | "Erstok Ontawl" |

Consecrates an area, granting +10 to ALL stats for affected allies for 30 minutes.

**Targeting:** Range 12, movement allowed.

**Logic:**
1. Gets surface top via `SpellHelper.GetSurfaceTop(ref p)`
2. Iterates `from.Mobile.GetMobilesInRange(6)`
3. For each target mobile:
   - Skips self, indirect targets, and those already under "Holy Bless" stat mod
   - Checks `CanBeBeneficial(mob, false)`
   - Calls `DoBeneficial(mob)`
   - Adds `StatMod(StatType.All, "Holy Bless", +10, 30 minutes)`
   - Particles: `0x373A, 10, 15, 5018, EffectLayer.Waist`
   - Sound: `0x1EA`
4. If any targets affected: `SpellHelper.Turn`, sound `0x299`, message "You consecrate the area."
5. If no targets: `FixedEffect(0x3735, 6, 30)`, sound `0x5C`

**Stat Mod Details:**
| Property | Value |
|----------|-------|
| Stat | `StatType.All` (Str, Dex, Int combined) |
| Modifier | `+10` |
| Duration | 30 minutes |
| Name | "Holy Bless" |

---

### 6. Holy Shield

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HolyShield` |
| Power Cost | `20` |
| Invocation Phrase | "Erstok K'blac" |

Activates a protective shield lasting 1 hour.

**Logic:**
1. If `from.IsShielded`: sends "You are already under the protection of a holy shield."
2. Calls `from.BeginShield()` — sets `_shield = Core.Now`
3. Sends "You are now under the protection of a holy shield."

**Shield Duration:** Exactly 1 hour from activation.

**Note:** The `IsShielded` property checks `_shield != DateTime.MinValue AND Core.Now < _shield + TimeSpan.FromHours(1.0)`. If the check occurs after the hour expires, it calls `FinishShield()` and returns `false`.

---

### 7. Holy Steed

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HolySteed` |
| Power Cost | `30` |
| Invocation Phrase | "Trubechs Yeliab" |

Summons a `HolySteed` (silver steed) mountable horse.

**Logic:**
1. If existing `from.Steed?.Deleted == true`, clears the reference
2. If `from.Steed != null`: sends "You already have a holy steed."
3. Checks `from.Mobile.Followers + 1 > from.Mobile.FollowersMax`
4. Spawns via `BaseCreature.Summon(steed, from.Mobile, from.Mobile.Location, 0x217, TimeSpan.FromHours(1.0))`
5. On success: `from.Steed = steed`

**Summon Duration:** 1 hour

---

### 8. Holy Word

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Hero.HolyWord` |
| Power Cost | `100` |
| Invocation Phrase | "Erstok Oostrac" |

**Status: NOT IMPLEMENTED** — `BeginInvoke()` is an empty stub.

---

## Evil Powers

### 1. Unholy Sense

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.UnholySense` |
| Power Cost | `0` |
| Invocation Phrase | "Drewrok Velgo" |

Detects nearby Hero ethic players with directional indication.

**Mirror of Holy Sense** — iterates `Ethic.Hero.Players` instead of `Ethic.Evil.Players`. All other mechanics (range formula, direction mapping, message format) are identical.

---

### 2. Unholy Item

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.UnholyItem` |
| Power Cost | `5` |
| Invocation Phrase | "Vidda K'balc" |

Imbues an unowned item with Evil ethic properties.

**Mirror of Holy Item** — same targeting, validation, and item type rules. On success:
- Sets `item.Hue = 0x455` (Evil primary hue)
- Sets `item.SavedFlags |= 0x200`
- Same effects: `FixedEffect(0x375A, 10, 20)`, `PlaySound(0x209)`

---

### 3. Summon Familiar (Evil)

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.SummonFamiliar` |
| Power Cost | `5` |
| Invocation Phrase | "Trubechs Vingir" |

Summons an `UnholyFamiliar` (dark wolf).

**Mirror of Hero Summon Familiar** — spawns `new UnholyFamiliar()`. Messages use "unholy" instead of "holy".

---

### 4. Vile Blade

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.VileBlade` |
| Power Cost | `10` |
| Invocation Phrase | "Velgo Reyam" |

**Status: NOT IMPLEMENTED** — `BeginInvoke()` is an empty stub.

---

### 5. Blight

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.Blight` |
| Power Cost | `15` |
| Invocation Phrase | "Velgo Ontawl" |

Curses an area, applying -10 to ALL stats for affected targets for 30 minutes.

**Mirror of Bless** — same targeting flow (range 12, movement allowed). Key differences:

**Logic:**
1. Iterates `from.Mobile.GetMobilesInRange(6)`
2. For each target:
   - Skips self, indirect targets, and those already under "Holy Curse" stat mod
   - Checks `CanBeHarmful(mob, false)`
   - Calls `DoHarmful(mob, true)`
   - Adds `StatMod(StatType.All, "Holy Curse", -10, 30 minutes)`
   - Particles: `0x374A, 10, 15, 5028, EffectLayer.Waist`
   - Sound: `0x1FB`
3. If any targets: `SpellHelper.Turn`, sound `0x1FB`, message "You curse the area."
4. If none: `FixedEffect(0x3735, 6, 30)`, sound `0x5C`

**Stat Mod Details:**
| Property | Value |
|----------|-------|
| Stat | `StatType.All` (Str, Dex, Int combined) |
| Modifier | `-10` |
| Duration | 30 minutes |
| Name | "Holy Curse" |

---

### 6. Unholy Shield

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.UnholyShield` |
| Power Cost | `20` |
| Invocation Phrase | "Velgo K'blac" |

Activates an unholy shield lasting 1 hour.

**Mirror of Holy Shield** — same mechanics, 1-hour duration. Messages use "unholy" instead of "holy".

---

### 7. Unholy Steed

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.UnholySteed` |
| Power Cost | `30` |
| Invocation Phrase | "Trubechs Yeliab" |

Summons an `UnholySteed` (dark steed) mountable horse.

**Mirror of Hero Summon Steed** — spawns `new UnholySteed()`. Messages use "unholy" instead of "holy".

---

### 8. Unholy Word

| Property | Value |
|----------|-------|
| Class | `Server.Ethics.Evil.UnholyWord` |
| Power Cost | `100` |
| Invocation Phrase | "Velgo Oostrac" |

**Status: NOT IMPLEMENTED** — `BeginInvoke()` is an empty stub.

---

## Summoned Creatures

### Holy Familiar / Unholy Familiar

| Property | Holy Familiar | Unholy Familiar |
|----------|--------------|-----------------|
| Class | `Server.Mobiles.HolyFamiliar` | `Server.Mobiles.UnholyFamiliar` |
| Body ID | `100` | `99` |
| Sound ID | `0xE5` | `0xE5` |
| Default Name | "a silver wolf" | "a dark wolf" |
| Corpse Name | "a holy corpse" | "an evil corpse" |
| AI Type | `AI_Melee` | `AI_Melee` |
| Tamable | `false` | `false` |
| Control Slots | `1` | `1` |
| Dispellable | `false` | `false` |
| Bondable | `false` | `false` |
| Meat | `1` | `1` |
| Hides | `7` | `7` |
| Favorite Food | `Meat` | `Meat` |
| Pack Instinct | `Canine` | `Canine` |

**Shared Stats:**

| Stat | Range |
|------|-------|
| Strength | 96–120 |
| Dexterity | 81–105 |
| Intelligence | 36–60 |
| Hits | 58–72 |
| Mana | 0 |
| Damage | 11–17 (100% Physical) |
| Virtual Armor | 22 |
| Fame | 2500 |
| Karma | 2500 |

**Shared Resistances:**

| Type | Range |
|------|-------|
| Physical | 20–25 |
| Fire | 10–20 |
| Cold | 5–10 |
| Poison | 5–10 |
| Energy | 10–15 |

**Shared Skills:**

| Skill | Range |
|-------|-------|
| Magic Resist | 57.6–75.0 |
| Tactics | 50.1–70.0 |
| Wrestling | 60.1–80.0 |

**Name Suffix:** Both append `"(Hero)"` or `"(Evil)"` via `ApplyNameSuffix(suffix)`.

---

### Holy Steed / Unholy Steed

| Property | Holy Steed | Unholy Steed |
|----------|-----------|--------------|
| Class | `Server.Mobiles.HolySteed` | `Server.Mobiles.UnholySteed` |
| Base | `BaseMount` | `BaseMount` |
| Body ID | `0x74` | `0x74` |
| Sound ID | `0x3EA7` | `0x3EA7` |
| Default Name | "a silver steed" | "a dark steed" |
| Corpse Name | "a holy corpse" | "an unholy corpse" |
| AI Type | `AI_Melee` | `AI_Melee` |
| Tamable | `false` | `false` |
| Control Slots | `1` | `1` |
| Dispellable | `false` | `false` |
| Bondable | `false` | `false` |
| Steps Max | `6400` | `6400` |
| Favorite Food | `FruitsAndVeggies \| GrainsAndHay` | `FruitsAndVeggies \| GrainsAndHay` |
| Karma | `2500` | `-14000` |
| Ride Restriction | `Ethic.Hero` players only | `Ethic.Evil` players only |

**Shared Stats:**

| Stat | Range |
|------|-------|
| Strength | 496–525 |
| Dexterity | 86–105 |
| Intelligence | 86–125 |
| Hits | 298–315 |
| Damage | 16–22 (40% Physical, 40% Fire, 20% Energy) |
| Virtual Armor | 60 |
| Fame | 14000 |

**Shared Resistances:**

| Type | Range |
|------|-------|
| Physical | 55–65 |
| Fire | 30–40 |
| Cold | 30–40 |
| Poison | 30–40 |
| Energy | 20–30 |

**Shared Skills:**

| Skill | Range |
|-------|-------|
| Magic Resist | 25.1–30.0 |
| Tactics | 97.6–100.0 |
| Wrestling | 80.5–92.5 |

**Monster Abilities:** `[MonsterAbilities.FireBreath]`

**Ride Restriction:**
```csharp
// Overridden in both steed classes
public override void OnDoubleClick(Mobile from)
{
    if (Ethic.Find(from) == typeof(HolySteed) ? Ethic.Hero : Ethic.Evil)
        base.OnDoubleClick(from);
}
```

---

## Item Trade and Equip Restrictions

Ethic-imbued items have strict ownership rules enforced by static methods on `Ethic`.

### `CheckTrade` Flow

```csharp
Ethic itemEthic = Find(item);
Ethic newOwnerEthic = Find(newOwner);

if (itemEthic == null || newOwnerEthic == itemEthic)
    return true;  // Allow: unimbued or same ethic

if (itemEthic == Hero)
    from.SendLocalizedMessage(1062582);  // "Only heroes may receive this item."
else
    from.SendLocalizedMessage(1062583);  // "Only the evil may receive this item."

return false;  // Deny
```

### `CheckEquip` Flow

```csharp
Ethic itemEthic = Find(item);
Ethic fromEthic = Find(from);

if (fromEthic == null || fromEthic == itemEthic)
    return true;  // Allow: no ethic or matching ethic

if (itemEthic == Hero)
    from.SendLocalizedMessage(1062580);  // "Only heroes may wear this item."
else
    from.SendLocalizedMessage(1062581);  // "Only the evil may wear this item."

return false;  // Deny
```

### `Find(Item)` Logic

```csharp
// Hero check
if ((item.SavedFlags & 0x100) != 0 && item.Hue == Hero.Definition.PrimaryHue)
    return Hero;

// Evil check
if ((item.SavedFlags & 0x200) != 0 && item.Hue == Evil.Definition.PrimaryHue)
    return Evil;

// Invalid flag cleanup
if ((item.SavedFlags & 0x100) != 0 && item.Hue != Hero.Definition.PrimaryHue)
    item.SavedFlags &= ~0x100;
if ((item.SavedFlags & 0x200) != 0 && item.Hue != Evil.Definition.PrimaryHue)
    item.SavedFlags &= ~0x200;

return null;
```

---

## Creature Integration

### `BaseCreature.EthicAllegiance`

Creatures can declare ethic allegiance by overriding the `EthicAllegiance` property. This affects combat targeting and power gain on kills.

| Creature | EthicAllegiance |
|----------|----------------|
| `DarkWisp` | `Ethic.Evil` |
| `Wisp` | `Ethic.Hero` |
| `Daemon` | `Ethic.Evil` |
| `OgreLord` | `Ethic.Evil` |
| `SilverSerpent` | `Ethic.Hero` |

### `GetEthicAllegiance(Mobile)`

```csharp
public Allegiance GetEthicAllegiance(Mobile mob)
{
    if (mob == null || Map != Faction.Facet || EthicAllegiance == null)
        return Allegiance.None;

    Ethic ethic = Ethic.Find(mob, true);
    return ethic == EthicAllegiance ? Allegiance.Ally : Allegiance.Enemy;
}
```

### Power Gain on Creature Kill

When a player kills a creature with `EthicAllegiance`:

```csharp
if (bc.EthicAllegiance != null)
{
    if (bc.GetEthicAllegiance(killer) == Allegiance.Enemy)
    {
        // 1 in (100 - killerEPL.Power) chance to gain +1 Power and +1 History
        if (Utility.Random(100 - killerEPL.Power) == 0 && killerEPL != null)
        {
            killerEPL.Power++;
            killerEPL.History++;
        }
    }
}
```

Conditions:
- `killerEPL` must be a registered `Player` profile (not just any player)
- Chance is `1 / (100 - power)`, so higher power = lower chance
- At power 5: 1 in 95 chance; at power 99: 1 in 1 chance

### Shield Protection in Combat

Creatures with `EthicAllegiance` are blocked from attacking shielded players of the same ethic:
- If a player is shielded and their ethic matches the creature's `EthicAllegiance`, the attack is invalid
- This prevents same-ethic creatures from harming shielded players

### AI Targeting

Creatures with `EthicAllegiance != null` are excluded from passive aggression mode. Ethic allegiance is checked in `IsInvalidFightModeTarget()` for `FightMode.Aggressor` and `FightMode.Evil` modes.

---

## Player Name Suffix

Players with an ethic profile receive a name suffix via `PlayerMobile.ApplyNameSuffix()`:

```csharp
// In PlayerMobile.ApplyNameSuffix()
if (EthicPlayer != null)
{
    suffix = EthicPlayer.Ethic.Definition.Adjunct.String;  // "(Hero)" or "(Evil)"
}
```

---

## Serialization

### `EthicsSystem` Persistence

The ethics system uses `GenericEntityPersistence<EthicsEntity>` for save/load:

| Parameter | Value |
|-----------|-------|
| Filename | `"Ethics"` |
| Min Serial | `0x1` |
| Max Serial | `0x7FFFFFFF` |

### Deserialization Order

**`Ethic` deserialization:**
1. Reads encoded player count
2. Creates `new Player(this, null)` for each (mobile resolved later)
3. After full deserialization, schedules `pl.CheckAttach` via `Timer.StartTimer`

**`Player` deserialization** (custom, does NOT call base):
1. Reads `_mobile` (encoded)
2. Reads `_power` (encoded)
3. Reads `_history` (encoded)
4. Reads `_steed` (encoded)
5. Reads `_familiar` (encoded)
6. Reads `_shield` (delta time)

---

## Unimplemented Powers

The following four powers have empty `BeginInvoke()` stubs and are not functional:

| Power | Ethic | Cost |
|-------|-------|------|
| Holy Blade | Hero | 10 |
| Holy Word | Hero | 100 |
| Vile Blade | Evil | 10 |
| Unholy Word | Evil | 100 |

These powers can be invoked (the speech phrase matches) but have no effect when called.

---

## Cross-References

- [`systems/factions.md`](systems/factions.md) — Ethics tied to faction alignment (Hero/Evil factions)
- [`systems/virtues.md`](systems/virtues.md) — Both moral progression systems (ethics + virtues)
- [`creatures/npcs.md`](creatures/npcs.md) — Ethic-affiliated NPCs and creatures
- [`systems/combat.md`](systems/combat.md) — Combat interactions with ethic allegiance
