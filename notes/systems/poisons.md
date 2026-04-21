# Poisons

Poisons are damage-over-time effects applied to characters through combat, crafting (Alchemy), or food preparation. ModernUO implements **three poison families** — Standard, Darkglow, and Parasitic — with a total of **14 distinct poison levels** ranging from Lesser to Lethal. Each poison applies recurring HP damage based on a percentage of the victim's current health, with special mechanics for range-based bonuses (Darkglow) and attacker healing (Parasitic). Poisons can be cured through auto-cure mechanics, player intervention, or transformation spells.

**Source Files:**
- `Projects/Server/Poison.cs` (102 lines) — abstract base class, poison registration, lookup, parsing
- `Projects/UOContent/Misc/Poison.cs` (162 lines) — `PoisonImpl` implementation, `PoisonTimer` damage engine
- `Projects/UOContent/Misc/PoisonKinds.cs` (109 lines) — all poison definitions, extension methods, registration

---

## Core Engine (`Poison`)

The poison system is built on an abstract `Poison` base class with a concrete `PoisonImpl` implementation. Poisons are registered at startup and stored in global lookup collections.

### `Poison` Abstract Base Class

Defined in `Projects/Server/Poison.cs`:

| Property | Type | Description |
|----------|------|-------------|
| `Index` | `int` | Unique numeric identifier (0-4 Standard, 10-13 Darkglow, 20-24 Parasitic) |
| `Name` | `string` | Display name (e.g., "Lesser", "DeadlyDarkglow") |
| `Level` | `int` | Progression level (0 = Lesser, 4 = Lethal) |
| `Family` | `PoisonFamily` | Parent family enum |

### Static Registries

| Collection | Type | Purpose |
|------------|------|---------|
| `Poison.Poisons` | `List<Poison>` | Ordered list, indexed by `Index` for O(1) lookup |
| `Poison.PoisonsByName` | `Dictionary<string, Poison>` | Case-insensitive name lookup |

### Key Static Methods

| Method | Return | Description |
|--------|--------|-------------|
| `Register(Poison)` | `void` | Validates unique index/name, adds to both registries |
| `GetPoisonByIndex(int)` | `Poison` | O(1) array lookup by index |
| `GetPoison(ReadOnlySpan<char> name)` | `Poison` | Case-insensitive name lookup |
| `IncreaseLevel(Poison)` | `Poison` | Returns poison with index+1 (promotes to next level) |
| `Parse(string)` / `TryParse(string)` | `Poison` | Parses by index number or name string |

### Poison Registration Flow

```
1. Configure() called at CallPriority(10) during server startup
2. For each poison: new PoisonImpl(...) → Poison.Register(...)
3. Register() validates:
   a. No poison with same Index exists
   b. No poison with same Name exists
4. Adds to Poisons list and PoisonsByName dictionary
```

---

## Poison Families

Three poison families are defined by the `PoisonFamily` enum:

| Family | Index Range | Levels | Expansion | Description |
|--------|-----------|--------|-----------|-------------|
| `Standard` | 0–4 | 5 (0–4) | AOS | Base poison family, default on all servers |
| `Darkglow` | 10–13 | 4 (0–3) | ML | Range-based damage bonus (>1 tile) |
| `Parasitic` | 20–24 | 5 (0–4) | ML | Heals attacker for damage dealt (within 1 tile) |

---

## Standard Poison (AOS)

The Standard family provides the core poison progression, available on all server configurations. Each level increases damage percentage, tick count, and interval.

### AOS Poison Table

| Poison | Index | Level | Min | Max | HP% | Ticks | Interval |
|--------|-------|-------|-----|-----|-----|-------|----------|
| Lesser | 0 | 0 | 4 | 16 | 7.5% | 10 | 2.25s |
| Regular | 1 | 1 | 8 | 18 | 10.0% | 10 | 3.25s |
| Greater | 2 | 2 | 12 | 20 | 15.0% | 10 | 4.25s |
| Deadly | 3 | 3 | 16 | 30 | 30.0% | 15 | 5.25s |
| Lethal | 4 | 4 | 20 | 50 | 35.0% | 20 | 5.25s |

**Registration (AOS branch, `Core.AOS`):**

```csharp
Poison.Register(new PoisonImpl("Lesser", 0, 0, 4, 16, 7.5, 3.0, 2.25, 10, 4));
Poison.Register(new PoisonImpl("Regular", 1, 1, 8, 18, 10.0, 3.0, 3.25, 10, 3));
Poison.Register(new PoisonImpl("Greater", 2, 2, 12, 20, 15.0, 3.0, 4.25, 10, 2));
Poison.Register(new PoisonImpl("Deadly", 3, 3, 16, 30, 30.0, 3.0, 5.25, 15, 2));
Poison.Register(new PoisonImpl("Lethal", 4, 4, 20, 50, 35.0, 3.0, 5.25, 20, 2));
```

### Standard Poison (Pre-AOS)

Before AOS, poisons used a different damage model with lower percentages but higher flat caps:

| Poison | Index | Level | Min | Max | HP% | Ticks | Interval |
|--------|-------|-------|-----|-----|-----|-------|----------|
| Lesser | 0 | 0 | 4 | 26 | 2.5% | 10 | 3.0s |
| Regular | 1 | 1 | 5 | 26 | 3.125% | 10 | 3.0s |
| Greater | 2 | 2 | 6 | 26 | 6.25% | 10 | 3.0s |
| Deadly | 3 | 3 | 7 | 26 | 12.5% | 10 | 4.0s |
| Lethal | 4 | 4 | 9 | 26 | 25.0% | 10 | 5.0s |

**Registration (Pre-AOS branch, `!Core.AOS`):**

```csharp
Poison.Register(new PoisonImpl("Lesser", 0, 0, 4, 26, 2.5, 3.5, 3.0, 10, 2));
Poison.Register(new PoisonImpl("Regular", 1, 1, 5, 26, 3.125, 3.5, 3.0, 10, 2));
Poison.Register(new PoisonImpl("Greater", 2, 2, 6, 26, 6.25, 3.5, 3.0, 10, 2));
Poison.Register(new PoisonImpl("Deadly", 3, 3, 7, 26, 12.5, 3.5, 4.0, 10, 2));
Poison.Register(new PoisonImpl("Lethal", 4, 4, 9, 26, 25.0, 3.5, 5.0, 10, 2));
```

### Key Differences: AOS vs Pre-AOS

| Aspect | AOS | Pre-AOS |
|--------|-----|---------|
| Damage basis | % of current HP (scaling) | % of current HP (lower %) |
| Max damage cap | Level-dependent (16–50) | Flat 26 for all levels |
| Tick count | Scales with level (10–20) | Fixed at 10 |
| Tick interval | Scales with level (2.25–5.25s) | Fixed at 3.0–5.0s |
| Consistent damage | Yes (each tick recalculated) | 50% chance to repeat last tick's damage |

---

## Darkglow Poison (ML+)

Available when `Core.ML` is enabled, Darkglow poisons mirror the first four Standard poison levels but gain a **10% damage boost** when the attacker is more than 1 tile away from the victim.

### Darkglow Poison Table

| Poison | Index | Level | Min | Max | HP% |
|--------|-------|-------|-----|-----|-----|
| LesserDarkglow | 10 | 0 | 4 | 16 | 7.5% |
| RegularDarkglow | 11 | 1 | 8 | 18 | 10.0% |
| GreaterDarkglow | 12 | 2 | 12 | 20 | 15.0% |
| DeadlyDarkglow | 13 | 3 | 16 | 30 | 30.0% |

**Note:** Darkglow has no Lethal (level 4) variant.

### Darkglow Range Bonus

```csharp
// Applied in PoisonTimer.OnTick()
if (_poison.Family == PoisonFamily.Darkglow && From != null && From.Map == _mobile.Map
    && !From.InRange(_mobile, 1))
{
    damage = (int)(damage * 1.1);  // +10% damage
    From.SendLocalizedMessage(1072850);  // "Darkglow poison increases your damage!"
}
```

Conditions:
- Attacker (`From`) must not be null
- Attacker and victim must share the same map
- Attacker must be **more than 1 tile away** from victim

---

## Parasitic Poison (ML+)

Available when `Core.ML` is enabled, Parasitic poisons mirror all five Standard poison levels but gain a **healing effect** for the attacker when within 1 tile of the victim.

### Parasitic Poison Table

| Poison | Index | Level | Min | Max | HP% |
|--------|-------|-------|-----|-----|-----|
| LesserParasitic | 20 | 0 | 4 | 16 | 7.5% |
| RegularParasitic | 21 | 1 | 8 | 18 | 10.0% |
| GreaterParasitic | 22 | 2 | 12 | 20 | 15.0% |
| DeadlyParasitic | 23 | 3 | 16 | 30 | 30.0% |
| LethalParasitic | 24 | 4 | 20 | 50 | 35.0% |

### Parasitic Heal Effect

```csharp
// Applied in PoisonTimer.OnTick()
if (_poison.Family == PoisonFamily.Parasitic && From != null && From.Map == _mobile.Map
    && From.InRange(_mobile, 1))
{
    From.Heal(damage);
    From.SendLocalizedMessage(1060203, damage.ToString());  // "~1_HEALED_AMOUNT~ hit points healed"
}
```

Conditions:
- Attacker (`From`) must not be null
- Attacker and victim must share the same map
- Attacker must be **within 1 tile** of victim (adjacent)

---

## Damage Formula

### AOS Damage Calculation

Each tick, the poison calculates damage based on the victim's **current** HP (not max HP):

```
scalar = percent × 0.01
rawDamage = 1 + (int)(Mobile.Hits × scalar)
damage = Math.Clamp(rawDamage, minimum, maximum)
```

Where:
- `scalar` is derived from the poison's HP% value (e.g., 7.5% → 0.075)
- `Mobile.Hits` is the victim's **current** HP at tick time (reduces as poison deals damage)
- `minimum` and `maximum` are poison-level-specific clamps

### Pre-AOS Damage Calculation

Pre-AOS adds a consistency mechanic:

```
if (!Core.AOS && _lastDamage != 0 && Utility.RandomBool())
{
    damage = _lastDamage;  // 50% chance to repeat previous tick's damage
}
else
{
    damage = 1 + (int)(Mobile.Hits × scalar);
    damage = Math.Clamp(damage, minimum, maximum);
    _lastDamage = damage;
}
```

---

## Poison Timer (`PoisonTimer`)

The `PoisonTimer` nested class within `PoisonImpl` manages the damage-over-time execution. It inherits from `Timer` and runs on a server timer loop.

### Timer Construction Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `name` | `string` | Poison name |
| `index` | `int` | Poison index |
| `level` | `int` | Poison level (0–4) |
| `min` | `int` | Minimum damage per tick |
| `max` | `int` | Maximum damage per tick |
| `percent` | `double` | HP percentage scalar (e.g., 7.5 for 7.5%) |
| `delay` | `double` | Seconds before first tick |
| `interval` | `double` | Seconds between subsequent ticks |
| `count` | `int` | Total number of damage ticks |
| `messageInterval` | `int` | Tick interval for `OnPoisoned()` callback |
| `family` | `PoisonFamily` | Poison family (default: Standard) |

### Timer Tick Flow

```
1. OnTick() fires every `interval` seconds (after initial `delay`)
2. Auto-cure check (Core.AOS):
   a. Level < 4 + VampiricEmbraceSpell → cure
   b. Level < 3 + OrangePetals → cure
   c. AnimalForm (Unicorn) → cure all levels
3. Tick limit check: if _index >= _count → poison expires
4. Calculate damage (see Damage Formula above)
5. Apply family effects:
   a. Darkglow: +10% damage if >1 tile from attacker
   b. Parasitic: heal attacker for damage if within 1 tile
6. DoHarmful() registration (PvP tracking)
7. AOS.Damage() — apply damage to victim
8. 40% chance of RevealingAction() (reveals stealth)
9. If _index % messageInterval == 0 → OnPoisoned() callback
```

### Poison Expiration

When tick count is exhausted:
```csharp
_mobile.SendLocalizedMessage(502136);  // "The poison seems to have worn off."
_mobile.Poison = null;
```

---

## Auto-Cure Interactions

Certain abilities and transformations automatically cure poisons at each tick. Cure checks occur **before** damage is dealt.

| Cure Source | Condition | Cures Up To |
|-------------|-----------|-------------|
| `VampiricEmbraceSpell` | Victim under transformation | Level < 4 ( Lesser through Deadly ) |
| `OrangePetals` | Active effect on victim | Level < 3 ( Lesser through Greater ) |
| `AnimalForm` (Unicorn) | Victim transformed into Unicorn | All levels (0–4) |

When auto-cure triggers:
- SA servers: "* You feel yourself resisting the effects of the poison *" (local emote)
- SA servers: "* ~1_NAME~ seems resistant to the poison *" (non-local emote)
- Pre-SA servers: equivalent text messages
- Timer stops immediately

---

## Extension Methods (`PoisonKinds`)

The `PoisonKinds` class provides C# 13 extension methods on `Poison` for convenient access to registered poisons.

### Direct Access Extensions

| Extension | Returns |
|-----------|---------|
| `poison.Lesser` | Standard Lesser poison (index 0) |
| `poison.Regular` | Standard Regular poison (index 1) |
| `poison.Greater` | Standard Greater poison (index 2) |
| `poison.Deadly` | Standard Deadly poison (index 3) |
| `poison.Lethal` | Standard Lethal poison (index 4) |
| `poison.LesserDarkglow` | Darkglow Lesser poison (index 10) |
| `poison.RegularDarkglow` | Darkglow Regular poison (index 11) |
| `poison.GreaterDarkglow` | Darkglow Greater poison (index 12) |
| `poison.DeadlyDarkglow` | Darkglow Deadly poison (index 13) |
| `poison.LesserParasitic` | Parasitic Lesser poison (index 20) |
| `poison.RegularParasitic` | Parasitic Regular poison (index 21) |
| `poison.GreaterParasitic` | Parasitic Greater poison (index 22) |
| `poison.DeadlyParasitic` | Parasitic Deadly poison (index 23) |
| `poison.LethalParasitic` | Parasitic Lethal poison (index 24) |

### Boolean Check Extensions

| Extension | Returns |
|-----------|---------|
| `poison.IsDarkglow` | `true` if `poison.Family == PoisonFamily.Darkglow` |
| `poison.IsParasitic` | `true` if `poison.Family == PoisonFamily.Parasitic` |

### Lookup Extensions

| Method | Return | Description |
|--------|--------|-------------|
| `Poison.GetPoison(int level)` | `Poison` | Finds Standard poison by level (0–4) |
| `Poison.GetPoisonByFamilyAndLevel(PoisonFamily, int)` | `Poison` | Finds any poison by family and level |

---

## ApplyPoisonResult

The result of poison application attempts (used by the Poisoning skill when applying poison to weapons, food, or other items):

| Value | Description |
|-------|-------------|
| `Poisoned` | Target successfully received the poison |
| `Immune` | Target is immune to the poison level |
| `HigherPoisonActive` | A higher-level poison is already active on the target |
| `Cured` | Poison was applied but immediately cured (e.g., by auto-cure) |

---

## Cross-References

- [`systems/combat.md`](systems/combat.md) — Poison damage application in combat, `Damage()` method
- [`skills/combat-skills.md`](skills/combat-skills.md) — Poisoning skill mechanics
- [`skills/utility-skills.md`](skills/utility-skills.md) — Poisoning skill definition
- [`items/weapons.md`](items/weapons.md) — Weapon poison application and charges
- [`items/food.md`](items/food.md) — Poisoning food items
