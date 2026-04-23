# SpiritSpeak

**Spirit Speak (SpiritSpeak)** allows players to contact the netherworld and channel energy from corpses to heal wounds.

**Title:** Medium | **Primary Stat:** Intelligence | **Secondary Stat:** Strength

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Medium |
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

**Pre-AOS:**
- `CheckSkill(SpiritSpeak, 0, 100)` determines if contact succeeds
- Duration scales with skill: `max(15, (skill / 50) * 90)` seconds
- Sets `CanHearGhosts = true` for the duration

**AOS+ (Spell system):**
- Functions as a spell with mantra "Anh Mi Sah Ko"
- Targets nearby corpses to channel energy for healing
- Healing range: `min` to `min + 4` hits, where `min = 1 + (skill * 0.25)`
- Costs 0 mana when channeling from corpse, 10 mana otherwise
- Requires 100+ skill for reliable healing: `skill / 100.0` success chance

---

---

## Weapons

None
---

## Items

None
---

## NPCs

- `EvilHealer` (`Projects/UOContent/Mobiles/Healers/EvilHealer.cs:16`) — 80.0-100.0
- `EvilWanderingHealer` (`Projects/UOContent/Mobiles/Healers/EvilWanderingHealer.cs:19`) — 80.0-100.0
- `FortuneTeller` (`Projects/UOContent/Mobiles/Healers/FortuneTeller.cs:17`) — 65.0-88.0
- `Healer` (`Projects/UOContent/Mobiles/Healers/Healer.cs:19`) — 80.0-100.0
- `WanderingHealer` (`Projects/UOContent/Mobiles/Healers/WanderingHealer.cs:18`) — 80.0-100.0
- `LichLord` (`Projects/UOContent/Mobiles/Monsters/Humanoid/Magic/LichLord.cs:34`) — 90.0-110.0
- `MasterJonath` (`Projects/UOContent/Mobiles/Monsters/ML/Bedlam/MasterJonath.cs:37`) — 99.6-106.9
- `MasterMikael` (`Projects/UOContent/Mobiles/Monsters/ML/Bedlam/MasterMikael.cs:37`) — 96.1-105.3
- `MasterTheophilus` (`Projects/UOContent/Mobiles/Monsters/ML/Bedlam/MasterTheophilus.cs:39`) — 125.6-133.8
- `LadyOfTheSnow` (`Projects/UOContent/Mobiles/Monsters/SE/LadyOfTheSnow.cs:42`) — 90.0-110.0
- `HealerGuildmaster` (`Projects/UOContent/Mobiles/Vendors/NPC/Guildmasters/HealerGuildmaster.cs:16`) — 65.0-88.0

*... and 7 more NPCs with this skill*
---

## Spells

- `NecromancerSpell` (`Projects/UOContent/Spells/Necromancy/NecromancerSpell.cs:15`) — DamageSkill
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

- `MistakenIdentity` (`Projects/UOContent/Engines/ML Quests/Definitions/MistakenIdentity.cs:290`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:591`) — objective threshold: 500
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1689`)
- `NewHavenSkillTraining` (`Projects/UOContent/Engines/ML Quests/Definitions/NewHavenSkillTraining.cs:1751`)
---

## Code Locations

- `Projects/UOContent/Skills/SpiritSpeak.cs` — skill handler implementation
- `30 files` — total code references in UOContent

---

## Expansion Notes

SpiritSpeak is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
