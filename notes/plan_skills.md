# Plan: Expand Skill Documentation

## Goal

Expand each of the **58 documented skills** under `notes/skills/` to catalog **every game system, item, NPC, spell, and data file** that references or uses that skill.

## Current State

- 58 skill docs exist under `notes/skills/` (57 implemented + 1 not-implemented reference)
- Each doc currently has: description, skill table (title, stats, scales, gains), brief mechanics, expansion notes, cross-references
- No section exists documenting which game objects **use** the skill

## Proposed New Sections

Add these sections to every skill doc:

### 1. Weapons

All weapon types that use this skill via `DefSkill`. List weapon base classes and any specific weapons that override the default.

### 2. Items

Items that reference the skill, including:
- Tools that check the skill (e.g., LockPick checks Lockpicking)
- Items with `AosSkillBonuses` for this skill
- Items that apply skill mods for this skill
- Harvest tools/resources tied to this skill (mining, lumberjacking, fishing)

### 3. NPCs

NPCs that have this skill assigned:
- Guildmasters and their skill sets
- Notable NPCs (guards, trainers, special creatures)

### 4. Spells

Spells that use this skill as CastSkill, DamageSkill, or resistance skill. Also spells that apply skill mods related to this skill.

### 5. Crafting

Crafting definitions (DefXxx) that use this skill as MainSkill. List craftable item categories and any skill thresholds.

### 6. Weapon Abilities

Weapon abilities that require this skill (e.g., DualWield requires Ninjitsu, ShadowStrike requires Stealth).

### 7. Harvest Systems

Harvest definitions and resource tables that use this skill (mining, lumberjacking, fishing).

### 8. Professions

Professions that list this skill in their requirements or starting skills.

### 9. Quests

Quests that have objectives tied to this skill (skill gain objectives, training quests).

### 10. Other Skills

Skills that depend on or interact with this skill (prerequisites, paired skills, skills that modify it).

### 11. Code Locations

Key source files that implement or reference this skill, with line numbers.

## Template

```markdown
# <Skill Name>

<Skill description paragraph>

**Title:** <Title> | **Primary Stat:** <Stat> | **Secondary Stat:** <Stat>

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | <Title> |
| Primary Stat | <Stat> |
| Secondary Stat | <Stat> |
| Str Scale | <value> |
| Dex Scale | <value> |
| Int Scale | <value> |
| Str Gain | <value> |
| Dex Gain | <value> |
| Int Gain | <value> |

---

## Mechanics

<Bullet points covering skill checks, stat scaling, gain rates, key mechanics>

---

## Weapons

<!-- List all weapon types that use this skill -->
- `BaseWeaponType` — default skill association
- `SpecificWeapon` — overrides default (if applicable)

---

## Items

<!-- Items that check, modify, or grant bonuses for this skill -->
- **Tool/Resource:** <item> — <how it uses the skill>
- **Skill Bonus Items:** <item> — <bonus values>
- **Other:** <item> — <description>

---

## NPCs

<!-- NPCs with this skill assigned -->
- `<NPC name>` — <skill level range>, `file.cs`
- `<NPC name>` — <description>

---

## Spells

<!-- Spells referencing this skill -->
- `<Spell name>` — <role: CastSkill, DamageSkill, resistance, mod>
- `<Spell name>` — <description>

---

## Crafting

<!-- Crafting definitions using this skill -->
- `<CraftSystem name>` — <MainSkill, item categories>
- `<CraftSystem name>` — <description>

---

## Weapon Abilities

<!-- Abilities requiring this skill -->
- `<Ability name>` — <skill requirement>
- `<Ability name>` — <description>

---

## Harvest Systems

<!-- Harvest systems tied to this skill -->
- `<Harvest name>` — <resource types, skill thresholds>

---

## Professions

<!-- Professions that require or start with this skill -->
- `<Profession name>` — <role: starting skill, requirement>

---

## Quests

<!-- Quests with skill objectives -->
- `<Quest name>` — <objective type, skill threshold>

---

## Other Skills

<!-- Skills that interact with this one -->
- `<Skill>` — <relationship: prerequisite, paired, modifies>

---

## Code Locations

<!-- Key source files -->
- `Projects/UOContent/Skills/<SkillName>.cs` — skill handler implementation
- `Projects/UOContent/Items/Weapons/<path>` — weapon definitions
- `Projects/UOContent/Engines/Craft/Def<Skill>.cs` — crafting definition
- `Projects/UOContent/Spells/<path>` — spell references

---

## Expansion Notes

<Expansion availability info>

---

## Cross-References

- [Category Skills](category-skills.md) — Full list of all <category> skills
- [Systems: <System>](../systems/<system>.md) — <system> mechanics
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
```

## Execution Strategy

### Phase 1: Discovery Script

Write a bash/grep-based script to auto-discover all references for each skill across the codebase. This will produce raw data for each section above.

**What to search for each skill:**
1. `SkillName.<Skill>` — all direct code references (grep -rn)
2. Weapon files: `DefSkill` overrides in `Items/Weapons/`
3. Item files: `AosSkillBonuses.SetValues` and skill mod references in `Items/`
4. NPC files: `SetSkill(SkillName.<Skill>` in `Mobiles/`
5. Spell files: CastSkill/DamageSkill/DefaultSkillMod in `Spells/`
6. Craft files: `MainSkill = SkillName.<Skill>` in `Engines/Craft/`
7. Harvest files: `Skill = SkillName.<Skill>` in `Engines/Harvest/`
8. Profession files: skill references in `Distribution/Data/Professions/`
9. Quest files: `GainSkillObjective(SkillName.<Skill>` in `Engines/ML Quests/`
10. Ability files: skill requirement checks in `Items/Weapons/Abilities/`

### Phase 2: Expand All Skills

Process skills by category, in order:

#### Crafting Skills (10)
1. alchemy.md
2. blacksmithy.md
3. bowcraft-fletching.md
4. carpentry.md
5. cartography.md
6. cooking.md
7. inscription.md
8. imbuing.md
9. tailoring.md
10. tinkering.md

#### Combat Skills (11)
11. archery.md
12. bushido.md
13. focus.md
14. fencing.md
15. macing.md
16. ninjitsu.md
17. parry.md
18. swords.md
19. tactics.md
20. throwing.md
21. wrestling.md

#### Magical Skills (8)
22. chivalry.md
23. evalint.md
24. magery.md
25. meditation.md
26. mysticism.md
27. necromancy.md
28. spellweaving.md
29. magicresist.md

#### Utility Skills (29)
30. anatomy.md
31. animallore.md
32. animaltaming.md
33. armslore.md
34. begging.md
35. camping.md
36. detecthidden.md
37. discordance.md
38. fishing.md
39. forensics.md
40. healing.md
41. herding.md
42. hiding.md
43. itemid.md
44. lockpicking.md
45. lumberjacking.md
46. mining.md
47. musicianship.md
48. peacemaking.md
49. poisoning.md
50. provocation.md
51. removetrap.md
52. snooping.md
53. spiritspeak.md
54. stealing.md
55. stealth.md
56. tasteid.md
57. tracking.md
58. veterinary.md

#### Not Implemented (1)
59. monsterslaying.md (reference doc only, no code — skip expansion)

### Phase 3: Verification

- Re-read the readme to ensure cross-references in the overview are correct
- Spot-check 5-10 expanded docs for accuracy and consistency
- Verify all file paths in Code Locations section actually exist

## Key Statistics from Discovery

Based on initial codebase scan:

| Category | Count |
|----------|-------|
| Total SkillName references | 3,138+ |
| Skill handler files | 25+ |
| NPC skill assignments (SetSkill) | 1,901+ |
| Weapon skill definitions (DefSkill) | 224+ |
| Crafting definition files | 11 |
| Harvest systems | 3 (Mining, Lumberjacking, Fishing) |
| Items with AosSkillBonuses | 129+ |
| Profession files | Multiple in `Distribution/Data/Professions/` |
| Quest skill objectives | 18+ training quests |

## Notes

references to expand.
- Some skills (e.g., Magery, Stealth, Cooking) will have very extensive sections while others (e.g., EvalInt, TasteID) will be more concise.
- Where a section has no entries, include the section header with "None" rather than omitting it, for consistency.
