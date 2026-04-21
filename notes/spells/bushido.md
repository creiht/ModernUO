# Bushido

Bushido is the Samurai martial arts school, blending physical combat technique with spiritual power. Bushido abilities come in two distinct types: **toggle spells** (self-buffs that persist for a duration) and **hit-based abilities** (special moves triggered during weapon attacks).

Bushido requires the **Samurai Empire** expansion (Core.SE). These abilities use the **Bushido** skill and consume mana. Unlike Magery spells, Bushido abilities are not affected by Faster Cast Recovery items (CastDelayFastScalar = 0).

## Key Mechanics

### Two Ability Types

Bushido abilities fall into two categories:

1. **Toggle Spells** — Self-buffs with duration. Only one toggle can be active at a time (casting a new one cancels the previous).
2. **Hit-Based Abilities (Special Moves)** — Triggered during weapon swings. Each ability is activated before a specific attack, consuming mana at that moment.

### Toggle Spell System

When casting a toggle spell, it automatically cancels the other active toggles:
- Casting **Confidence** cancels Evasion and Counter Attack
- Casting **Evasion** cancels Confidence and Counter Attack
- Casting **Counter Attack** cancels Confidence and Evasion

Toggles have a 30-second duration (except Evasion, which scales with Bushido skill).

### Special Move System

Hit-based abilities use the `SpecialMove` framework:

1. **Activate**: Press the ability key to enter the "ready" state
2. **Swing**: Next weapon attack triggers the special move
3. **Execute**: Ability effects apply on hit or miss
4. **Cooldown**: Ability becomes unavailable for a short period

Some abilities have a 3-second context window after activation. If not used within that time, the ability is lost.

### Skill Requirements

Each ability has a minimum Bushido skill requirement. Cast success range: `RequiredSkill - 12.5` to `RequiredSkill + 37.5`.

### Mana Costs

Hit-based abilities consume mana on successful execution. Toggle spells consume mana on cast. Mana costs range from 0 (Honorable Execution on hit) to 10 (toggles).

### Parry System (Evasion)

Evasion uses the parry mechanic:

```
Parry modifier = 1.0 + 0.16 + (Bushido - 60) * 0.004
```

With GM Tactics + GM Anatomy + 100+ Bushido: bonus increases by additional 0.10, capping at 50%.

## Toggle Spells

Self-buffs that persist for a duration.

| Spell | Mana | Skill Req | Duration | Effect |
|-------|------|-----------|----------|--------|
| Confidence | 10 | 25.0 | 30s | +Health regeneration: `(15 + Bushido^2 / 576) / 4` HP per tick (4 ticks/sec). HS version: +Attack/Damage bonuses. |
| Evasion | 10 | 60.0 | 3-7s | Increases parry chance. Base: `16 + (Bushido-60)*0.4%`. With GM Tactics+Anatomy+100 Bushido: up to 50%. Requires weapon or shield. ML version requires 50+ weapon skill. |
| Counter Attack | 5 | 40.0 | 30s | Next blocked blow triggers a counter-attack. Requires weapon or shield equipped. |

### Confidence — Detailed Mechanics

Confidence provides continuous health regeneration:

- **HP per tick**: `(15 + Bushido^2 / 576) / 4` (4 ticks per second = 1 tick every 0.25s)
- **Total HP/sec**: `(15 + Bushido^2 / 576)`
- At GM Bushido (120): `(15 + 14400/576) = 15 + 25 = 40` HP per second
- Duration: 30 seconds, total heal potential: 1200 HP at GM

### Evasion — Detailed Mechanics

Evasion modifies the parry system:

- **Base parry modifier**: `1.0 + 0.16 + (Bushido - 60) * 0.004`
- **With GM Tactics + GM Anatomy + 100+ Bushido**: Additional `+0.10` bonus
- **Maximum parry**: 50%
- **Cooldown**: ~20 seconds between uses (BeginAction lock)
- **Requirements**: Weapon or shield equipped; ML version requires 50+ weapon skill

## Hit-Based Abilities

Special moves triggered during weapon attacks.

| Ability | Mana | Skill Req | Type | Effect |
|---------|------|-----------|------|--------|
| Honorable Execution | 0 | 25.0 | Hit-based | On kill: heal 20 + Bushido^2/480 HP, +swing bonus for 20s. On miss: -40 all resistances, -MagicResist for 7s. |
| Lightning Strike | 5 | 50.0 | Hit-based | +50 accuracy bonus. Armor ignore chance: `Bushido^2 / 72000`. Deals lightning damage on hit. |
| Momentum Strike | 10 | 70.0 | Hit-based | After hitting primary target, strikes second combatant within weapon range. Damage bonus: Bushido/100. 1.5x if primary target is dead. |

### Honorable Execution — Detailed Mechanics

Honorable Execution is a high-risk, high-reward ability:

- **On Kill**:
  - Heal: `20 + Bushido^2 / 480` HP
  - Swing speed bonus: `Bushido / 720` for 20 seconds
  - At GM (120): `20 + 14400/480 = 50` HP healed, `120/720 = 0.17` swing bonus
- **On Miss**:
  - -40 to ALL resistances for 7 seconds
  - -MagicResist equal to current value for 7 seconds
  - This can be devastating if the ability is used and the attack misses

### Lightning Strike — Detailed Mechanics

Lightning Strike focuses on accuracy and armor penetration:

- **Accuracy Bonus**: +50 to hit
- **Armor Ignore Chance**: `Bushido^2 / 72000`
  - At 50 Bushido: `2500/72000 = 3.5%`
  - At 100 Bushido: `10000/72000 = 13.9%`
  - At 120 Bushido: `14400/72000 = 20%`
- **Damage**: Lightning damage on successful hit
- **Delayed Context**: True (mana consumed only on hit)

### Momentum Strike — Detailed Mechanics

Momentum Strike is a multi-target ability:

1. Hit primary target as normal
2. If there is a second combatant within weapon range, strike them as well
3. **Damage bonus**: `Bushido / 100`
   - At 70 Bushido: 0.7x bonus
   - At 120 Bushido: 1.2x bonus
4. **Dead target bonus**: 1.5x if primary target is dead

## Ability Progression

Bushido abilities unlock progressively as Bushido skill increases:

```
25.0  Honorable Execution (hit-based)
      Confidence (toggle)
40.0  Counter Attack (toggle)
50.0  Lightning Strike (hit-based)
60.0  Evasion (toggle)
70.0  Momentum Strike (hit-based)
```

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index
- [[spells/ninjitsu]] — Ninja equivalent abilities
