# DetectHidden

**Detecting Hidden (DetectHidden)** reveals invisible creatures, objects, and traps in the surrounding area. It works both passively (through movement detection on Felucca) and actively (through area scans).

**Title:** Scout | **Primary Stat:** Intelligence | **Secondary Stat:** Dexterity

**Source:** `Distribution/Data/skills.json`, `SkillsInfo.cs`

---

## Skill Table

| Property | Value |
|----------|-------|
| Title | Scout |
| Primary Stat | Intelligence |
| Secondary Stat | Dexterity |
| Str Scale | 0 |
| Dex Scale | 0 |
| Int Scale | 0 |
| Str Gain | 0 |
| Dex Gain | 0.4 |
| Int Gain | 0.6 |

---

## Mechanics

- **Skill check:** `CheckSkill(DetectHidden, 0.0, 100.0)`
- **Cooldown:** 30 seconds
- **Detection range:** `range = skill / 10` tiles (halved on failed check)
- **Passive detection:** Felucca PvP only, checks every 3 seconds with debounce

**Active detection** targets a location and reveals all hidden entities within range. The skill check compares DetectHidden vs Hiding with +/-10 random variance on both:
```
ss = detectSkill + random(-10, +10)
ts = hidingSkill + random(-10, +10)
if ss >= ts: revealed
```

**Passive detection** triggers when moving near hidden creatures on Felucca only. Excludes party members, guild members/allies, blessed creatures, dead/bonded pets, and region-based PvP rules.

**Additional detection capabilities:**
- Shows `[trapped]` on containers within range when skill check passes
- Reveals hidden faction traps at 80+ DetectHidden
- Reveals dungeon traps at 75+ DetectHidden (HS+)

---

## Expansion Notes

DetectHidden is available from character creation in the base game. No expansion is required.

---

## Cross-References

- [Utility Skills](utility-skills.md) — Full list of all utility skills
- [Systems: Combat](../systems/combat.md) — Combat mechanics and formulas
- [Getting Started: Stats](../getting-started/stats.md) — Stat-skill relationships
