# Meditation

**Meditation** is used to enter a trance state that significantly increases mana regeneration rate. It is one of the most important skills for spellcasters.

**Title:** Stoic | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Stoic |
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

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** None (zero across all stats)
- **Primary use:** Faster mana recovery
- **Cooldown:** 10 seconds (5 seconds pre-AOS)

Meditation has specific preconditions that must be met:

**Pre-AOS:**
- Must have >= 10% HP

**AOS+:**
- `RegenRates.GetArmorOffset(m) == 0` (no meditation-blocking armor)
- Hands must be free or holding only spellbook/runebook/spell-channeling items

**Success chance:**
```
chance = (50.0 + (skillVal - (manaMax - mana)) * 2) / 100
```

Higher skill increases the chance of success. Lower mana deficit (closer to full mana) decreases the chance, as the skill is less useful when already near full mana.

**On success:**
- Sets `m.Meditating = true`
- Adds `BuffIcon.ActiveMeditation` buff icon
- Plays meditation sound (0xF9)

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `HireMage` (`Projects/UOContent/Mobiles/Hireables/HireMage.cs:28`) — 100-125
- `EvilMageLord` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Magic/EvilMageLord.cs:43`) — 27.5-50.0
- `MasterTheophilus` (`Projects/UOContent/Mobiles/Monsters/ML/Bedlam/MasterTheophilus.cs:40`) — 128.8-132.9
- `MinotaurCaptain` (`Projects/UOContent/Mobiles/Monsters/ML/Humanoid/Melee/MinotaurCaptain.cs:30`) — 0

*... and 36 more NPCs with this skill*
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

- **[AOS]** Meditation: 50
- **[None]** Meditation: 50
---

## Quests

- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2022`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2066`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2111`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2154`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2199`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2247`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2288`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2337`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2378`)
- `Heartwood` (`Projects/UOContent/Engines/ML Quests/Definitions/Heartwood.cs:2476`)
---

## Code Locations

- `Projects/UOContent/Skills/Meditation.cs` — skill handler implementation
- `108 files` — total code references in UOContent

---

## Expansion Notes

Meditation is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
