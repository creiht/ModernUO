# Ninjitsu

Ninjitsu is the Ninja combat school, emphasizing stealth, agility, and shadow arts. Like Bushido, Ninjitsu abilities come in two types: **toggle spells** (persistent self-buffs) and **hit-based abilities** (special moves triggered during attacks).

Ninjitsu requires the **Samurai Empire** expansion (Core.SE). These abilities use the **Ninjitsu** skill and consume mana. A key distinction from Bushido: Ninjitsu abilities do NOT reveal the caster on cast (`RevealOnCast = false`).

## Key Mechanics

### Stealth Integration

Ninjitsu is deeply integrated with the Stealth system:

- **No reveal on cast**: Unlike other spell schools, using Ninjitsu abilities does not reveal the caster
- **Backstab and Surprise Attack** require the caster to be Hidden
- **Animal Form** provides stealth bonuses for certain morphs
- **Shadowjump** requires being in stealth mode

### Toggle Spell System

Only one toggle can be active at a time. Casting a new toggle cancels the previous:

- **Shadow Jump** and **Mirror Image** are the toggle spells
- **Animal Form** is also a toggle (transformation-based)
- Casting Animal Form cancels Shadow Jump and Mirror Image

### Special Move System

Hit-based abilities use the `SpecialMove` framework, identical to Bushido:

1. **Activate**: Press the ability key
2. **Swing**: Next weapon attack triggers the special move
3. **Execute**: Effects apply on hit
4. **Cooldown**: 3-second context window

Some abilities return the caster to stealth after use (Backstab, Surprise Attack).

### Skill Requirements

Each ability has a minimum Ninjitsu skill requirement. Cast success range: `RequiredSkill - 12.5` to `RequiredSkill + 37.5`.

### Mana Costs

Toggle spells consume mana on cast. Hit-based abilities consume mana on successful execution. Mana costs range from 0 (Animal Form at 0.0 skill) to 30 (Backstab, Death Strike).

### ML vs Legacy Skill Requirements

Some abilities have different skill requirements depending on expansion:

| Ability | ML Requirement | Legacy Requirement |
|---------|---------------|-------------------|
| Backstab | 40.0 | 20.0 |
| Focus Attack | 30.0 | 60.0 |
| Surprise Attack | 60.0 | 30.0 |
| Mirror Image | 20.0 | 40.0 |
| Ki Attack | ML only | N/A |

## Toggle Spells

Self-buffs with duration.

| Spell | Mana | Skill Req | Duration | Effect |
|-------|------|-----------|----------|--------|
| Shadow Jump | 15 | 50.0 | N/A | Teleport to target location (11-tile range). Requires hidden. Cannot use with sigil, while overloaded, in restricted regions, or houses. |
| Mirror Image | 10 | 20.0 (ML) / 40.0 (legacy) | 30 + Ninjitsu/4 | Creates a clone with identical body, hue, skills, and equipment. Clone follows owner and deletes on taking damage. |
| Animal Form | 0 (legacy) / 10 (ML) | 0.0 | N/A | Transform into selected animal. 15 morph options with varying skill requirements and special abilities. |

### Shadow Jump — Detailed Mechanics

Shadow Jump is a teleportation ability:

- **Range**: 11 tiles
- **Requirements**: Must be in stealth mode (Hidden = true)
- **Restrictions**: Cannot use with sigil active, while overloaded, in restricted regions, or inside houses
- **After jump**: Stealth check is performed at the destination
- **Blocked by**: Animal Form transformation

### Mirror Image — Detailed Mechanics

Mirror Image creates a perfect clone:

- **Clone properties**: Identical body, hue, skills, and equipment (as items)
- **Clone AI**: Uses Basic AI, follows the owner
- **One-hit death**: Clone deletes immediately upon taking any damage
- **Restrictions**: Cannot use while mounted, at follower cap, or in Horrific Beast form
- **Duration**: `30 + Ninjitsu / 4` seconds
  - At 60 Ninjitsu: 45 seconds
  - At 100 Ninjitsu: 55 seconds

### Animal Form — Detailed Mechanics

Animal Form is the most complex Ninjitsu ability, allowing transformation into 15 different creatures:

| Morph | Skill Req | Body ID | Special Ability |
|-------|-----------|---------|----------------|
| Rabbit | 20.0 | 8485 | Stealth bonus +10 |
| Rat | 20.0 | 8483 | Stealth bonus +10 |
| Dog | 40.0 | 8476 | Speed boost |
| Cat | 40.0 | 8475 | Speed boost |
| Ferret | 40.0 | 11672 | Speed boost |
| Llama | 70.0 | 8438 | Speed boost |
| Forest Ostrich | 70.0 | 8503 | Speed boost |
| Giant Serpent | 50.0 | 9663 | Speed boost |
| Bull Frog | 50.0 | 8496 | Speed boost, Stealing bonus +10 |
| Cu Sidhe | 60.0 | 11670 | Heals 20-50 HP every 8s using bandages |
| Grey Wolf | 82.5 | 9681 | Speed boost, Stealth bonus +20 |
| BakeKitsune | 82.5 | 10083 | Speed boost |
| Reptalon | 90.0 | 11669 | Breath attack: 20 damage on combatant |
| Unicorn | 100.0 | 9678 | Speed boost |
| Kirin | 100.0 | 9632 | Speed boost |

**Morph success chance**: Based on Ninjitsu vs. requirement:
- If `Ninjitsu < ReqSkill + 37.5`: success chance = `(Ninjitsu - ReqSkill) / 37.5`
- Otherwise: automatic success

**Special form abilities**:
- **Cu Sidhe**: Auto-heals 20-50 HP every 8 seconds (consumes bandages from backpack)
- **Reptalon**: Breath attack deals 20 fire damage to combatant within perception range
- **Grey Wolf / Rabbit / Rat / Bull Frog**: Grant Stealth +20 bonus

## Hit-Based Abilities

Special moves triggered during weapon attacks.

| Ability | Mana | Skill Req | Effect |
|---------|------|-----------|--------|
| Backstab | 30 | 40.0 (ML) / 20.0 (legacy) | Requires hidden. Damage: `1.0 + Ninjitsu/360 + StalkingBonus/100`. Returns to stealth for 5s after use. |
| Death Strike | 30 | 85.0 | Complex multi-hit damage system with escalating damage. |
| Focus Attack | 10 (ML) / 20 (legacy) | 30.0 (ML) / 60.0 (legacy) | Melee only (no shield/ranged). Damage: `1.0 + Ninjitsu^2/43636`. Property bonus: `1.0 + (Ninjitsu^2/43636 * 3 + 0.01)`. |
| Ki Attack | 25 | 80.0 | Distance-based damage. Requires moving toward target. Melee only (ML). |
| Surprise Attack | 20 | 60.0 (ML) / 30.0 (legacy) | Requires hidden. Defense malus on target: `Ninjitsu/60 + StalkingBonus` for 8s. Returns to stealth for 5s. |

### Backstab — Detailed Mechanics

Backstab is the signature Ninjitsu ability:

- **Requirements**: Must be Hidden AND have allowed Stealth steps > 0
- **Reveals** the caster on use
- **Damage scalar**: `1.0 + Ninjitsu / 360 + StalkingBonus / 100`
  - At 60 Ninjitsu: `1.0 + 0.17 + StalkingBonus/100`
  - At 100 Ninjitsu: `1.0 + 0.28 + StalkingBonus/100`
- **After use**: Returns to stealth mode for 5 seconds
- **Cooldown**: Cannot be used again for 5 seconds (BeginAction<Stealth> lock)

### Death Strike — Detailed Mechanics

Death Strike uses a complex escalating damage system:

1. **Initial hit**: 50% base damage
2. **Counter applied**: Target receives a "Death Strike counter" (5 steps to trigger)
3. **Each subsequent hit** from the same attacker adds one step
4. **After 5 steps**: Full damage triggers

**Damage formulas**:
- **5+ steps (full trigger)**: `min(60, Ninjitsu/3 * (0.3 + 0.7 * scalar)) + stalking`
- **Less than 5 steps**: `min(30, Ninjitsu/9 * (0.3 + 0.7 * scalar)) + stalking`
- **Scalar**: `(Hiding + Stealth) / 220`
- **Ranged attacks**: Damage halved

**Hit chance**:
- **< 100 Ninjitsu**: `30 + (Ninjitsu - 85) * 2.2`
- **>= 100 Ninjitsu**: `63 + (Ninjitsu - 100) * 1.1`

### Focus Attack — Detailed Mechanics

Focus Attack emphasizes concentrated melee power:

- **Requirements**: Melee weapon only (no shield, no ranged)
- **Damage scalar**: `1.0 + Ninjitsu^2 / 43636`
  - At 60 Ninjitsu: `1.0 + 3600/43636 = 1.08`
  - At 100 Ninjitsu: `1.0 + 10000/43636 = 1.23`
  - At 120 Ninjitsu: `1.0 + 14400/43636 = 1.33`
- **Property bonus**: `1.0 + (Ninjitsu^2/43636 * 3 + 0.01)`
  - At 100 Ninjitsu: `1.0 + 0.69 = 1.69`
- **Mana check**: Performed before damage

### Ki Attack — Detailed Mechanics

Ki Attack rewards forward momentum:

- **Requirements**: Melee weapon only (ML version)
- **Cannot use while Hidden** (ML)
- **Movement requirement**: Must travel toward target within 2 seconds of activation
- **Distance-based damage**: `min(distanceTraveled, 20) / 10` (PvP) or `/40` (PvM)
- **Hidden attackers**: No distance bonus
- **Maximum bonus**: +2.0 (PvP) or +0.5 (PvM) at full distance

### Surprise Attack — Detailed Mechanics

Surprise Attack weakens the target:

- **Requirements**: Must be Hidden AND have allowed Stealth steps > 0
- **Reveals** the caster on use
- **Defense malus**: `Ninjitsu/60 + StalkingBonus` for 8 seconds
  - At 60 Ninjitsu: `1.0 + StalkingBonus`
  - At 100 Ninjitsu: `1.67 + StalkingBonus`
- **After use**: Returns to stealth mode for 5 seconds
- **Cooldown**: Cannot be used again for 5 seconds

## Ability Progression

Ninjitsu abilities unlock progressively as Ninjitsu skill increases:

```
0.0    Animal Form (toggle - transformation)
20.0   Backstab (ML: 40.0) / Mirror Image (legacy: 40.0)
30.0   Focus Attack (ML) / Surprise Attack (legacy)
40.0   Backstab (ML)
50.0   Shadow Jump
60.0   Focus Attack (legacy) / Surprise Attack (ML)
80.0   Ki Attack
85.0   Death Strike
```

## Cross-References

- [[spells/index]] — Spell school overview
- [[skills/magical-skills]] — Magical skills reference
- [[reference/spell-index]] — Complete spell index
- [[spells/bushido]] — Samurai equivalent abilities
