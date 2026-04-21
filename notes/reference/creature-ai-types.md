# Creature AI Types Reference

## Overview

ModernUO uses a modular AI system where each `BaseCreature` subclass is assigned an `AIType` that determines its behavior through a dedicated AI class. The AI system runs on a timer-driven loop with six possible action states.

**Source files:**
- `Projects/UOContent/Mobiles/AI/BaseAI/AIType.cs` — AI type enum
- `Projects/UOContent/Mobiles/AI/BaseAI/ActionType.cs` — action state enum
- `Projects/UOContent/Mobiles/AI/BaseAI/BaseAI.cs` — abstract base class (~1008 lines)
- `Projects/UOContent/Mobiles/BaseCreature.cs:2246-2271` — `ChangeAIType()` factory switch

---

## ActionType Enum

The AI loop cycles through these six action states, defined in `ActionType.cs`:

| State | Value | Description | AI Classes That Use |
|-------|-------|-------------|---------------------|
| `Wander` | 0 | Roaming, scanning for threats, following waypoints | All 10 AI types |
| `Combat` | 1 | Actively fighting a combatant | All 10 AI types |
| `Guard` | 2 | Standing watch after losing combatant, re-scanning | Melee, Archer, Mage, Berserk, Predator, Thief |
| `Flee` | 3 | Running away from danger when HP is low | Melee, Archer, Animal, Mage, Thief |
| `Backoff` | 4 | Retreating to safety when a threat is detected | Predator, (BaseAI default for all) |
| `Interact` | 5 | Talking/interacting with customers | Vendor only |

---

## AIType Enum

There are **10 AI types** defined in `AIType.cs`:

| Enum Value | AI Class | File |
|------------|----------|------|
| `AI_Use_Default` | Resolves to `m_DefaultAI` | N/A (fallback) |
| `AI_Melee` | `MeleeAI` | `MeleeAI.cs` |
| `AI_Animal` | `AnimalAI` | `AnimalAI.cs` |
| `AI_Archer` | `ArcherAI` | `ArcherAI.cs` |
| `AI_Healer` | `HealerAI` | `HealerAI.cs` |
| `AI_Vendor` | `VendorAI` | `VendorAI.cs` |
| `AI_Mage` | `MageAI` | `MageAI.cs` |
| `AI_Berserk` | `BerserkAI` | `BerserkAI.cs` |
| `AI_Predator` | `MeleeAI` (fallback) | `PredatorAI.cs` (exists but not wired) |
| `AI_Thief` | `ThiefAI` | `ThiefAI.cs` |

> **Note:** `AI_Use_Default` resolves to the creature's `m_DefaultAI` field (set in the `BaseCreature` constructor at line 337). When `AI` is set to `AI_Use_Default`, the getter at `BaseCreature.cs:631-634` substitutes `m_DefaultAI` before calling `ChangeAIType()`.

> **Note:** `AI_Predator` has a `PredatorAI.cs` implementation (89 lines) but the `ChangeAIType()` switch at `BaseCreature.cs:2265-2267` comments it out and instantiates `MeleeAI` instead. The `PredatorAI` code is preserved in the codebase but non-functional.

---

## AI Type Details

### AI_Use_Default

- **Enum value:** `AI_Use_Default` (0)
- **Resolved to:** `m_DefaultAI` (set during creature construction)
- **Description:** Inherits the default AI type from the creature's `m_DefaultAI` field. This allows subclasses to override `m_DefaultAI` to change their base behavior while keeping `AI = AI_Use_Default` for compatibility.
- **Action states used:** Determined by resolved `m_DefaultAI`
- **Flee threshold:** Determined by resolved AI class
- **Key behaviors:**
  - The getter at `BaseCreature.cs:624-638` substitutes `m_CurrentAI = m_DefaultAI` before calling `ChangeAIType()`
  - `m_DefaultAI` is set in the `BaseCreature` constructor (line 337) from the `ai` parameter
  - Serialized/deserialized via `writer.Write((int)m_DefaultAI)` and `reader.ReadInt()`

---

### AI_Melee

- **Enum value:** `AI_Melee` (1)
- **Class:** `MeleeAI` (`MeleeAI.cs`, 157 lines)
- **Description:** Standard close-quarters combat AI. Detects threats while wandering, pursues combatants, and goes on guard when the combatant is lost.
- **Action states used:** Wander, Combat, Guard, Flee
- **Flee threshold:** 20% HP (`FleeHealthThreshold = 0.2`)
- **Flee chance:** 10% (`FleeChance = 0.1`)
- **Key behaviors:**
  - `DoActionWander()`: Acquires focus mob using `Mobile.FightMode`; if none found, wanders randomly
  - `DoActionCombat()`: Moves to within `Mobile.RangeFight` of combatant, attacks when in range
  - `HandleOutOfRangeCombatant()`: If combatant is beyond `RangePerception * 3`, clears combatant and goes to Guard
  - `AttemptMoveToCombatant()`: If move is blocked, acquires a new focus mob; if combatant is beyond `RangePerception + 1`, goes to Guard
  - `DoActionGuard()`: Re-scans for threats; if found, switches to Combat
  - `DoActionFlee()`: If HP recovers above threshold, returns to Combat; otherwise continues fleeing
  - Uses `Mobile.TriggerAbility(MonsterAbilityTrigger.CombatAction, combatant)` each combat tick
  - Does NOT use Backoff or Interact states

---

### AI_Animal

- **Enum value:** `AI_Animal` (2)
- **Class:** `AnimalAI` (`AnimalAI.cs`, 72 lines)
- **Description:** Simple animal behavior with low flee threshold and high backoff chance. Animals are less strategic than humanoid creatures.
- **Action states used:** Wander, Combat, Flee
- **Flee threshold:** 10% HP (`FleeHealthThreshold = 0.1`) — lower than most AIs
- **Flee chance:** 10% (`FleeChance = 0.1`)
- **Backoff chance:** 50% (`BackoffChance = 0.5`)
- **Key behaviors:**
  - `DoActionWander()`: Same pattern as MeleeAI — acquires focus mob or wanders via base
  - `DoActionCombat()`: Simpler than MeleeAI — uses `WalkMobileRange` to close to `RangeFight`; if combatant is lost, goes directly to Wander (not Guard)
  - `DoActionFlee()`: Calls `AcquireFocusMob` with `RangePerception * 2` range and `FightMode` before calling base flee
  - Does NOT implement `DoActionGuard()` — falls through to `BaseAI.DoActionGuard()` which immediately transitions to Wander
  - Does NOT use Guard or Backoff states (though `BackoffChance = 0.5` is set, `ShouldBackoff()` is called from `BaseAI.DoActionWander()` via `ShouldBackoff()`)
  - Uses `Mobile.TriggerAbility(MonsterAbilityTrigger.CombatAction, combatant)` each combat tick

---

### AI_Archer

- **Enum value:** `AI_Archer` (3)
- **Class:** `ArcherAI` (`ArcherAI.cs`, 99 lines)
- **Description:** Ranged combat AI that maintains distance from combatants. Runs out of ammo detection causes fleeing.
- **Action states used:** Wander, Combat, Guard, Flee
- **Flee threshold:** 20% HP (`FleeHealthThreshold = 0.2`)
- **Flee chance:** 10% (`FleeChance = 0.1`)
- **Key behaviors:**
  - `DoActionWander()`: Acquires focus mob or wanders via base
  - `DoActionCombat()`: Maintains range between `RangeFight` and `Weapon.MaxRange` using `WalkMobileRange`
  - **Ammo check:** If `Mobile.Backpack.FindItemByType<Arrow>() == null`, switches to Flee action (line 72-78)
  - `DoActionGuard()`: Re-scans for threats; if found, switches to Combat
  - Does NOT override `DoActionFlee()` — uses base implementation
  - Uses `Mobile.TriggerAbility(MonsterAbilityTrigger.CombatAction, combatant)` each combat tick
  - Does NOT use Backoff or Interact states

---

### AI_Healer

- **Enum value:** `AI_Healer` (4)
- **Class:** `HealerAI` (`HealerAI.cs`, 170 lines)
- **Description:** Support AI that heals allied creatures on the same Team. Only engages in combat when no allies need healing. Uses a strict healing priority system.
- **Action states used:** Wander (no explicit combat/flee transitions)
- **Flee threshold:** 0.0 (inherited from `BaseAI`, no fleeing)
- **Flee chance:** 0.0 (inherited from `BaseAI`, no fleeing)
- **Key behaviors:**
  - Overrides `Think()` instead of individual action methods — entirely custom flow
  - **Healing priority** (checked in order via `Find()` method):
    1. **Cure** — if ally is poisoned (`m.Poisoned`)
    2. **Greater Heal** — if ally HP < max - 40 (`m.Hits < m.HitsMax - 40`)
    3. **Lesser Heal** — if ally HP < max - 10 (`m.Hits < m.HitsMax - 10`)
  - `Find()` searches `Mobile.GetMobilesInRange(RangePerception)` for `BaseCreature` on the same `Team`, prioritizes by distance (closest first)
  - If an ally needs healing, casts the appropriate spell and returns true (skips combat)
  - If no allies need healing: calls `AcquireFocusMob(RangePerception, FightMode.Weakest, false, true, false)` — fights weakest enemy, includes faction friends, excludes faction foes
  - Moves within range 4-7 of target via `WalkMobileRange`
  - Does NOT switch to Guard/Flee/Combat action states — stays in the Think loop
  - Uses `Mobile.TriggerAbility(MonsterAbilityTrigger.CombatAction, Mobile.Combatant)` when fighting
  - `ProcessTarget()` handles mid-cast target switching if a higher-priority ally needs healing

---

### AI_Vendor

- **Enum value:** `AI_Vendor` (5)
- **Class:** `VendorAI` (`VendorAI.cs`, 141 lines)
- **Description:** Merchant AI that responds to customer speech keywords (*vendor buy*, *vendor sell*) and cries for guards when attacked.
- **Action states used:** Wander, Interact, Flee
- **Flee threshold:** Not explicitly set (inherits 0.0 from BaseAI — vendors do not flee based on HP)
- **Flee chance:** Not explicitly set (inherits 0.0 from BaseAI)
- **Key behaviors:**
  - `DoActionWander()`: If `Combatant != null`, cries for guards (`GetRandomGuardMessage()` = localization 1005305 or 501603) and switches to Flee; if `FocusMob != null` (customer), switches to Interact
  - `DoActionInteract()`: Stays near customer within `RangeFight`; if customer disappears or leaves, returns to Wander
  - `DoActionGuard()`: Sets `FocusMob = Combatant` then calls base
  - `HandlesOnSpeech()`: Extends speech detection range to 4 tiles (AOS) or 1 tile (pre-AOS)
  - `OnSpeech()`: Listens for keywords:
    - `0x14D` (*vendor sell*) → calls `vendor.VendorSell(from)`
    - `0x3C` (*vendor buy*) → calls `vendor.VendorBuy(from)`
    - `0x177` (*sell*) → calls `vendor.VendorSell(from)`
    - `0x171` (*buy*) → calls `vendor.VendorBuy(from)`
    - Also responds to creature's own name as a keyword
  - Does NOT use Combat or Backoff states
  - Does NOT override `DoActionFlee()` — uses base implementation

---

### AI_Mage

- **Enum value:** `AI_Mage` (6)
- **Class:** `MageAI` (`MageAI.cs`, 1112 lines — largest AI class)
- **Description:** Full spellcasting AI with complex spell selection, healing priority, combo attacks, and dispel logic. Supports "SmartAI" mode for vendors/escortables with different behavior.
- **Action states used:** Wander, Combat, Guard
- **Flee threshold:** 20% HP (`FleeHealthThreshold = 0.2`)
- **Flee chance:** 10% (`FleeChance = 0.1`)
- **Key behaviors:**
  - Overrides `Think()` — combines spell targeting (`ProcessTarget()`) with base AI loop
  - **SmartAI mode** (`SmartAI` property): Enabled for `BaseVendor`, `BaseEscortable`, and `Changeling` — uses simpler, more efficient spell selection
  - **Necromancer detection** (`IsNecromancer`): `Core.AOS && Mobile.Skills.Necromancy.Value > 50` — switches spell pool to necromancy spells
  - **Healing logic** (`CheckCastHealingSpell()`):
    - If poisoned: always casts Cure
    - Summoned creatures never self-heal
    - SmartAI: heals when HP < max - 50 (Greater Heal) or HP < max - 10 (Heal)
    - Non-SmartAI: heals with chance scaled by Magery skill (10% at GM)
    - Post-heal delay: `Int >= 500 ? 7-10s : sqrt(600 - Int)` seconds
  - **Spell selection** (`ChooseSpell()`):
    - Non-SmartAI (16-way random): poison (2/16), bless (1/16), curse (3/16), paralyze (1/16), mana drain (1/16), invis (1/16), damage (7/16)
    - SmartAI (3-way random): poison (1/3), damage (1/3), combo/setup (1/3)
  - **Damage spell pool:**
    - Mage: Magic Arrow (2x), Harm (2x), Fireball (2x), Lightning (2x), Mind Blast (2x), Energy Bolt, Explosion
    - Necromancer: Pain Spike, Poison Strike, Strangle, Wither, Vengeful Spirit
  - **Curse spell pool:**
    - Mage: Curse (if Magery >= 40, 25% chance), Weaken, Clumsy, Feeblemind
    - Necromancer: Blood Oath, Corpse Skin, Evil Omen, Mind Rot
  - **Combo system** (`DoCombo()`): SmartAI can chain Paralyze → Explosion → Poison/Strangle → random finisher (3-step combo)
  - **Dispel logic** (`FindDispelTarget()`): Actively dispels enemy summons, prioritizes by distance. Uses `Mobile.Int >= 95` and `CanDispel()` checks
  - **Teleport logic**: If stuck (move blocked), non-SmartAI teleports with chance scaled by Magery (5% at GM); SmartAI never teleports when stuck
  - **Wander healing** (`DoActionWander()`): SmartAI meditates if mana is low; all Mages have 5% chance to self-heal while wandering
  - **Guard behavior** (`DoActionGuard()`): If last target was hidden, casts Reveal spell to find them
  - `RunTo()` / `RunFrom()`: Positioning logic — SmartAI kites (moves 1 tile away when in melee range), non-SmartAI closes to `RangeFight`
  - `GetDelay()`: Spell cast delay = `6.0 - Magery*0.75` to `6.0 - Magery*1.25` seconds (scaled by skill); SmartAI uses `ActiveSpeed`
  - Does NOT use Flee, Backoff, or Interact states (inherits base flee behavior)

---

### AI_Berserk

- **Enum value:** `AI_Berserk` (7)
- **Class:** `BerserkAI` (`BerserkAI.cs`, 82 lines)
- **Description:** Aggressive AI that attacks everything in range, including friends and faction allies. No fleeing behavior.
- **Action states used:** Wander, Combat, Guard
- **Flee threshold:** Not explicitly set (inherits 0.0 from BaseAI — berserkers do not flee)
- **Flee chance:** Not explicitly set (inherits 0.0 from BaseAI)
- **Key behaviors:**
  - `DoActionWander()`: Calls `AcquireFocusMob(RangePerception, FightMode.Closest, false, true, true)` — the `true, true` flags mean it attacks both faction friends AND faction foes
  - `DoActionCombat()`: Standard melee pursuit — moves to `RangeFight`, attacks when in range
  - `DoActionGuard()`: Re-scans for threats using same `true, true` flags (attacks everyone)
  - Does NOT override `DoActionFlee()` — inherits base which has 0% chance to flee
  - Uses `Mobile.TriggerAbility(MonsterAbilityTrigger.CombatAction, combatant)` each combat tick
  - Does NOT use Flee, Backoff, or Interact states

---

### AI_Predator

- **Enum value:** `AI_Predator` (8)
- **Class:** `MeleeAI` (fallback — `PredatorAI` is commented out in `ChangeAIType()`)
- **Description:** **Currently non-functional.** The `PredatorAI` class exists in the codebase but is not wired into the `ChangeAIType()` factory. Creatures with `AI_Predator` get `MeleeAI` instead. The intended design was a stalking predator that backoffs when safe and re-engages when cornered.
- **Action states used (in PredatorAI.cs):** Wander, Combat, Guard, Backoff
- **Flee threshold:** Not set in PredatorAI (inherits 0.0)
- **Flee chance:** Not set in PredatorAI (inherits 0.0)
- **PredatorAI.cs intended behaviors (non-functional):**
  - `DoActionWander()`: If currently in combat (hurt/being attacked), goes to Combat; if detects a threat while not in combat, goes to Backoff (stalking behavior); otherwise wanders
  - `DoActionCombat()`: Standard melee pursuit — moves to `RangeFight`, attacks when in range
  - `DoActionBackoff()`: If hurt or has combatant, switches to Combat; otherwise retreats to safe distance (`RangePerception` to `RangePerception * 2`), then returns to Wander
  - Does NOT use Flee or Interact states
- **Actual behavior (wired in ChangeAIType):** Falls through to `MeleeAI` — standard close-quarters combat with 20% flee threshold

---

### AI_Thief

- **Enum value:** `AI_Thief` (9)
- **Class:** `ThiefAI` (`ThiefAI.cs`, 160 lines)
- **Description:** Stealthy combat AI that attempts to steal from combatants. Uses the Stealing skill and targets specific items on enemies. Has dynamic flee chance based on HP differential.
- **Action states used:** Wander, Combat, Guard, Flee
- **Flee threshold:** 20% HP (`Mobile.Hits < Mobile.HitsMax * 20 / 100` at line 93)
- **Flee chance:** Dynamic — `(10 + max(0, combatant.Hits - thief.Hits))` out of 100. If the thief is significantly weaker, flee chance approaches 100%.
- **Key behaviors:**
  - `DoActionWander()`: Standard focus mob acquisition, then wanders if none found
  - `DoActionCombat()`: Multi-phase behavior:
    1. **Disarm phase:** If `_toDisarm` is null, looks for `Layer.OneHanded` or `Layer.TwoHanded` item on combatant
    2. **Disarm attack:** Pre-AOS, if `Wrestling >= 80` and `ArmsLore >= 80` and disarm is ready, calls `Fists.DisarmRequest(Mobile)`
    3. **Steal from backpack:** If disarm target is in thief's backpack, uses Stealing skill on it
    4. **Try stealing consumables:** If no disarm target, attempts to steal Bandage, Nightshade, BlackPearl, or MandrakeRoot from combatant's backpack
    5. **Fallback to flee:** If all stealing attempts fail, switches to Flee action
    6. **Dynamic flee:** At < 20% HP, calculates flee chance as `10 + (combatant.Hits - thief.Hits)` capped at 0-100
  - `DoActionGuard()`: Re-scans for threats; if found, switches to Combat
  - `DoActionFlee()`: If HP recovers above 50% of max, returns to Combat; otherwise continues fleeing
  - `TryStealFrom<T>()`: Generic method to find and steal items of type T from combatant's backpack
  - Does NOT use Backoff or Interact states

---

## Summary Table

| AI Type | Class | Lines | Flee % | Flee Chance | States Used | SmartAI | Special |
|---------|-------|-------|--------|-------------|-------------|---------|---------|
| AI_Use_Default | Resolves to m_DefaultAI | — | Varies | Varies | Varies | No | Falls back to default AI |
| AI_Melee | MeleeAI | 157 | 20% | 10% | Wander, Combat, Guard, Flee | No | Standard melee combat |
| AI_Animal | AnimalAI | 72 | 10% | 10% | Wander, Combat, Flee | No | Low flee threshold, 50% backoff |
| AI_Archer | ArcherAI | 99 | 20% | 10% | Wander, Combat, Guard, Flee | No | Flees if out of arrows |
| AI_Healer | HealerAI | 170 | 0% | 0% | Wander (custom Think) | No | Heals allies by priority |
| AI_Vendor | VendorAI | 141 | 0% | 0% | Wander, Interact, Flee | No | Responds to buy/sell keywords |
| AI_Mage | MageAI | 1112 | 20% | 10% | Wander, Combat, Guard | Yes | Spellcasting, combos, dispel |
| AI_Berserk | BerserkAI | 82 | 0% | 0% | Wander, Combat, Guard | No | Attacks everything (friends + foes) |
| AI_Predator | MeleeAI (fallback) | 89* | 0%** | 0%** | — | No | **Non-functional** — commented out |
| AI_Thief | ThiefAI | 160 | 20% | Dynamic | Wander, Combat, Guard, Flee | No | Steals from combatants |

*PredatorAI.cs is 89 lines but not wired. **Inherits from MeleeAI when actually used.

---

## Base AI Infrastructure

### BaseAI Core Methods

| Method | Purpose | Override Points |
|--------|---------|-----------------|
| `Think()` | Main AI loop — checks flee, dispatches to action handler | MageAI, HealerAI (custom flow) |
| `DoActionWander()` | Wander behavior | All AIs |
| `DoActionCombat()` | Combat behavior | All AIs |
| `DoActionGuard()` | Guard behavior | Melee, Archer, Mage, Berserk, Predator, Thief |
| `DoActionFlee()` | Flee behavior | Melee, Archer, Animal, Thief |
| `DoActionBackoff()` | Backoff behavior | BaseAI only (used by Predator) |
| `DoActionInteract()` | Interact behavior | BaseAI only (used by Vendor) |
| `CheckFlee()` | HP-based flee check | BaseAI (shared) |
| `AcquireFocusMob()` | Target acquisition | BaseAI (shared, with AI-specific params) |

### Shared Constants

| Constant | Value | Source |
|----------|-------|--------|
| `DefaultRangePerception` | 10 | `BaseCreature` |
| `HealChance` | 0.10 | `MageAI` (10% at GM magery) |
| `TeleportChance` | 0.05 | `MageAI` (5% at GM magery) |
| `DispelChance` | 0.75 | `MageAI` (75% at GM magery) |
| `InvisChance` | 0.50 | `MageAI` (50% at GM magery) |

### AI Factory (ChangeAIType)

Defined at `BaseCreature.cs:2246-2271`:

```
AI_Melee   => MeleeAI
AI_Animal  => AnimalAI
AI_Berserk => BerserkAI
AI_Archer  => ArcherAI
AI_Healer  => HealerAI
AI_Vendor  => VendorAI
AI_Mage    => MageAI
AI_Predator => MeleeAI (PredatorAI commented out)
AI_Thief   => ThiefAI
_Use_Default => null (resolved in AI property getter)
```
