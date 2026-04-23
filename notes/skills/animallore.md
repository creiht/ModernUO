# Animal Lore

**Animal Lore** is the skill used to identify and gather detailed information about creatures. Naturalists use this skill to learn about creature stats, resistances, damage, preferred food, and pack instincts. It is essential for taming, hunting, and understanding the creatures of Britannia.

**Title:** Naturalist | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Projects/UOContent/Skills/AnimalLore.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Naturalist |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 0 |

---

## Mechanics

### Skill Check

```
CheckTargetSkill(AnimalLore, targetCreature, 0.0, 120.0)
```

- **Target range:** 8 tiles
- **Requirements:** Tamer must be alive, target must be a `BaseCreature`
- **Creature requirements:** Must be animal-type, tamed, or tameable
- **Skill thresholds:** 
  - Tamed creatures: identifiable below 100 skill
  - Tameable creatures: identifiable above 110 skill

### Animal Lore Gump

When successful, the Animal Lore gump displays the following pages:

| Page | Content |
|------|---------|
| **Attributes** | Hits, Stamina, Mana, Str/Dex/Int, Barding Difficulty (SE+), Loyalty (AOS+) |
| **Resistances** | Physical, Fire, Cold, Poison, Energy resistances |
| **Damage** | Physical, Fire, Cold, Poison, Energy damage |
| **Combat Ratings** | Wrestling, Tactics, MagicResist, Anatomy, Healing/Poisoning |
| **Lore** | Magery, EvalInt, Meditation |
| **Preferences** | Preferred food type, Pack instincts |

### Simultaneous Skill Gains

Animal Lore can gain skill simultaneously during Animal Taming attempts, allowing efficient dual-skill progression.

---

## Pack Instincts

Animal Lore reveals pack instinct information for creatures that fight in groups. Pack instincts provide bonuses when multiple creatures of the same type are present.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `AnimalTrainer` (`Projects/UOContent/Mobiles/Vendors/NPC/AnimalTrainer.cs:20`) — 64.0-100.0
- `Furtrader` (`Projects/UOContent/Mobiles/Vendors/NPC/Furtrader.cs:16`) — 85.0-100.0
- `RangerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/RangerGuildmaster.cs:11`) — 64.0-100.0
- `Rancher` (`Projects/UOContent/Mobiles/Vendors/NPC/Rancher.cs:14`) — 55.0-78.0
- `Veterinarian` (`Projects/UOContent/Mobiles/Vendors/NPC/Veterinarian.cs:14`) — 85.0-100.0
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

None
---

## Harvest Systems

None
---

## Professions

None
---

## Quests

None
---

## Code Locations

- `Projects/UOContent/Skills/AnimalLore.cs` — skill handler implementation
- `14 files` — total code references in UOContent

---

## Expansion Notes

Animal Lore is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of utility skills
- [Creature Reference](../reference/creature-reference.md) — Complete creature listing
- [Animals](../creatures/animals.md) — Tameable fauna and mounts
- [Systems: Combat](../systems/combat.md) — Combat mechanics
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
