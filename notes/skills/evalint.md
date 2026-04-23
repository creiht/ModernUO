# EvalInt

**Evaluating Intelligence** is used to assess the mental capabilities of other creatures. It estimates target Intelligence and Mana percentage.

**Title:** Scholar | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Scholar |
| Primary Stat | Intelligence |
| Secondary Stat | Strength |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0 |
| Int Gain | 1 |

---

## Mechanics

- **Stat scaling:** None (zero across all stats)
- **Gain rates:** 1.0 Int gain per use (tied highest Int gain)
- **Primary use:** Assessing target Intelligence and Mana

The skill works by targeting a creature and estimating their Int and Mana values:
```
marginOfError = max(0, 20 - (int)(EvalInt.Value / 5))
estimatedInt = target.Int + random(-marginOfError, +marginOfError)
estimatedMana = (target.Mana * 100 / target.ManaMax) + random(-marginOfError, +marginOfError)
```

At EvalInt >= 76.0, the user can also see the target's Mana percentage.

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `BaseHealer` (`Projects/UOContent/Mobiles/Healers/BaseHealer.cs:54`) — 82.0-100.0
- `HireMage` (`Projects/UOContent/Mobiles/Hireables/HireMage.cs:26`) — 100.0-125
- `EvilMage` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Magic/EvilMage.cs:41`) — 75.1-100.0
- `EvilMageLord` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Magic/EvilMageLord.cs:41`) — 80.2-100.0

*... and 36 more NPCs with this skill*
---

## Spells

- `Spell` (`Projects/UOContent/Spells/Base/Spell.cs:52`) — DamageSkill
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

- `MistakenIdentity` (`Projects/UOContent/Engines/ML Quests/Definitions/MistakenIdentity.cs:287`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:312`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:709`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1086`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1124`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1172`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1211`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1249`)
---

## Code Locations

- `Projects/UOContent/Skills/EvalInt.cs` — skill handler implementation
- `148 files` — total code references in UOContent

---

## Expansion Notes

EvalInt is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Magical Skills](magical-skills.md) — Full list of all magical skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
