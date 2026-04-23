# Monster Slaying

**Monster Slaying** is a skill that was **not implemented** in ModernUO. This page documents why the skill is referenced in some places but does not exist in the codebase.

---

## Status: Not Implemented

Monster Slaying does not exist as a skill in ModernUO. A search of the codebase reveals no `SkillName.MonsterSlaying` enum value and no corresponding skill implementation file.

### Current Skill List

ModernUO implements **58 skills** total, defined in `Projects/Server/Skills.cs`. The skill list does not include Monster Slaying:

| Category | Skills | Count |
|----------|--------|-------|
| Crafting | Alchemy, Blacksmithy, Bowcraft/Fletching, Carpentry, Cartography, Cooking, Imbuing, Inscription, Tailoring, Tinkering | 10 |
| Combat | Archery, Bushido, Focus, Fencing, Macing, Ninjitsu, Parry, Swords, Tactics, Throwing, Wrestling | 11 |
| Magic | Chivalry, EvalInt, Magery, MagicResist, Meditation, Mysticism, Necromancy, Spellweaving | 8 |
| Utility | AnimalLore, ItemID, ArmsLore, Begging, Camping, DetectHidden, Discordance, Healing, Fishing, Forensics, Herding, Hiding, Provocation, Inscribe, Lockpicking, MagicResist, Musicianship, Poisoning, SpiritSpeak, Stealing, Tailoring, AnimalTaming, TasteID, Tracking, Veterinary, Lumberjacking, Mining, Stealth, RemoveTrap | 29 |

---

## Historical Context

Monster Slaying may have been:
1. **A planned but unreleased skill** — Considered during development but never implemented
2. **A custom server skill** — Added by a third-party server fork but not included in ModernUO
3. **An outdated reference** — The documentation references a skill that existed in an earlier version

---

## Related Skills

For combat against monsters, the following skills serve similar purposes:

| Skill | Purpose |
|-------|---------|
| **Tactics** | Improves combat effectiveness against all creatures |
| **Anatomy** | Reveals weak points, increasing damage (utility skill) |
| **Arms Lore** | Enhances weapon effectiveness against creatures |
| **Monster Lore** | Animal Lore provides creature information |
| **Swords/Macing/Fencing** | Direct damage output against monsters |

---

## Cross-References

- [Combat Skills](combat-skills.md) — Offensive combat skills
- [Utility Skills](utility-skills.md) — Support and detection skills
- [Reference: Skill Table](../reference/skill-table.md) — Complete skill data
