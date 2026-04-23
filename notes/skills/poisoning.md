# Poisoning

**Poisoning** applies poison to weapons, food, darts, and shuriken. It is a key skill for poison-based combat strategies.

**Title:** Assassin | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Assassin |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0.4 |
| Int Gain | 1.6 |

---

## Mechanics

- **Cooldown:** 10 seconds
- **Process:** Target poison potion -> then target item to poison
- **Poisonable items:** Weapons (infectious strike ability), food, fukiya darts, shuriken
- **Weapon charges:** `18 - poisonLevel * 2` (e.g., Lethal poison = 8 charges)
- **Self-poison risk:** 5% chance if skill < 80 and check fails

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `MinotaurCaptain` (`Projects/UOContent/Mobiles/Monsters/ML/Humanoid/Melee/MinotaurCaptain.cs:33`) — 0
- `LadyLissith` (`Projects/UOContent/Mobiles/Monsters/ML/Twisted Weald/LadyLissith.cs:37`) — 96.6-112.9
- `LadySabrix` (`Projects/UOContent/Mobiles/Monsters/ML/Twisted Weald/LadySabrix.cs:37`) — 97.8-116.7

*... and 37 more NPCs with this skill*
---

## Spells

None
---

## Crafting

None
---

## Weapon Abilities

- `SerpentArrow` (`Projects/UOContent/Items/Weapons/Abilities/SerpentArrow.cs:10`) — secondary skill requirement
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:190`) — skill reference
- `WeaponAbility` (`Projects/UOContent/Items/Weapons/Abilities/WeaponAbility.cs:288`) — skill reference
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

- `Projects/UOContent/Skills/Poisoning.cs` — skill handler implementation
- `54 files` — total code references in UOContent

---

## Expansion Notes

Poisoning is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Poisons](../systems/poisons.md) — Poison system
- [Items: Weapons](../items/weapons.md) — Weapon mechanics and systems
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
