# Gargoyle

The Gargoyle spell school consists of a single racial ability available exclusively to Gargoyle characters. Unlike other spell schools, this ability does not require a skill, mana, or reagents.

## Gargoyle Flight

Gargoyle Flight is the ability to take to the skies, granting the Gargoyle character the **Flying** state.

### Mechanics

| Property | Value |
|----------|-------|
| Mana Cost | 0 |
| Reagents | None |
| Skill Required | None |
| Cast Time | 0.25s |
| Cast Recovery | 0 |
| Fast Cast Scalar | 0 |
| Clear Hands | No |
| Reveal on Cast | No |

### Effect

When cast, Gargoyle Flight:

1. Sets `Caster.Flying = true`
2. Plays animation 60 (body animate, speed 10, 1 repeat)
3. Displays message "You are flying."
4. Adds `BuffIcon.Fly` buff icon to the client display

### Restrictions

- **Racial only**: Only Gargoyle characters can use this ability
- **Disruption**: Can be disrupted by being hurt or equipping items (but NOT by using objects)
- **Toggle**: Casting again stops the flight
- **No cooldown**: No recovery time between uses
- **No mana cost**: Does not consume any mana

### Implementation Details

```csharp
public override bool CheckDisturb(DisturbType type, bool checkFirst, bool resistable) =>
    type != DisturbType.EquipRequest && type != DisturbType.UseRequest;
```

The flight spell is remarkably resilient — it only stops when the Gargoyle takes damage or manually cancels it by casting again. Using items does NOT disrupt the flight.

## Cross-References

- [[spells/index]] — Spell school overview
- [[getting-started/character-creation]] — Races and character creation
