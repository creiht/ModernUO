# Reference

Quick-reference tables and data extracted from source code. These pages provide comprehensive listings organized for fast lookup.

## Pages

### [Skill Table](skill-table.md)
Complete table of all **58 skills** with name, category, primary stat, scaling stats, and gain rates.

### [Spell Index](spell-index.md)
Complete listing of all **~117 spells** organized by school, with circle, reagents, mana cost, and effects.

### [Resistance Table](resistance-table.md)
Details on the **5 resistance types**: Physical, Fire, Cold, Poison, and Energy. Includes damage type interactions.

### [Craft Resources](craft-resources.md)
All **30+ craft resources** with tier, bonuses, hue, and associated craft definitions.

### [Creature AI Types](creature-ai-types.md)
All AI types used by creatures with behavior descriptions: MeleeAI, ArcherAI, MageAI, AnimalAI, HealerAI, BerserkAI, PredatorAI, ThiefAI, VendorAI.

### [Creature Reference](creature-reference.md)
**Every creature type listed by name**, organized by category (Animals, Monsters, NPCs, Bosses) and subcategory.

### [Configuration](configuration.md)
All configurable system settings with their defaults. Covers 135+ settings across 51 systems:
- Murder System (short-term/long-term duration, bounty expiry)
- Factions (enabled toggle)
- Veteran Rewards (intervals, skill cap unlock)
- Harvesting (skill ranges, durability)
- Spawn System (timers, regions, max counts)
- Movement, Stats, Stamina, Client Verification
- Network, CrashGuard, AutoSave, AutoArchive
- Plus: Buff Icons, Chat, Ethics, Quests, Pathfinding, and more
- Includes code-defined settings and expansion-gated settings

### [Configuration (JsonConfig)](configuration-json.md)
Complex JSON configuration files:
- Email configuration (SMTP settings, addresses)
- Assistant configuration (UOSteam/Razer negotiation settings)

### [Feature Flags](feature-flags.md)
Runtime toggleable features with admin control:
- Server feature flags (trading, PvP, bank access, speedhack detection)
- Content feature flags (vendors, houses, boats, bulk orders)
- Block entries for gumps, items, skills, spells

## Data Sources

Reference tables are generated from source code:

| Table | Source |
|-------|--------|
| Skill Table | `Server/Skills.cs` |
| Spell Index | `Spells/*/` (all spell files) |
| Craft Resources | `CraftResource` enum + resource definitions |
| Creature AI Types | `AIType` enum |
| Creature Reference | `Mobiles/*/` directory structure |
| Configuration | `ServerConfiguration.Get*()` calls across engine files |
| JsonConfig | `EmailConfiguration.cs`, `AssistantConfiguration.cs` |
| Feature Flags | `ServerFeatureFlags.cs`, `ContentFeatureFlags.cs` |
