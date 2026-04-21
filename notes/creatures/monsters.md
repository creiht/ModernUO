# Monsters

Monsters are hostile creatures ranging from low-level slimes to world-ending dragons. They are the primary source of experience, loot, and challenge in ModernUO. There are **207 monster types** organized into 14 subcategories across all expansion packs.

Monsters are defined in `Projects/UOContent/Mobiles/Monsters/` and inherit from `BaseCreature`. Their behavior is controlled by various AI types: `AI_Melee` (close combat), `AI_Mage` (spellcasting), `AI_Archer` (ranged combat), and others.

---

## Key Concepts

- **AI Types** — Monsters use different AI behaviors: `AI_Melee` (close combat), `AI_Mage` (spellcasting), `AI_Archer` (ranged), `AI_Animal` (simple behavior), `AI_Berserk` (aggressive melee), `AI_Healer` (support), `AI_Predator` (hunting), `AI_Thief` (stealing).
- **Fight Modes** — How monsters choose targets: `Closest` (nearest enemy), `Strongest` (highest threat), `Weakest` (lowest defense), `Aggressor` (who attacked first), `Evil` (evil-aligned targets), `None` (passive).
- **Tamability** — Some monsters are tamable but require very high taming skill (often 70-104). Tamable monsters listed with their minimum taming skill in parentheses.
- **Damage Types** — Monsters deal Physical, Fire, Cold, Poison, or Energy damage. Some have multi-type damage pools.
- **Resistances** — Five resistance types: Physical (P), Fire (F), Cold (C), Poison (Po), Energy (E). Typical ranges 20-90, with elite monsters reaching 80-100.
- **Fame/Karma** — Fame indicates difficulty and reward; negative karma indicates evil alignment. Boss monsters can have fame above 20,000.
- **Virtual Armor (VA)** — Additional defense layer that reduces incoming damage.
- **Special Flags** — `BleedImmune` (immune to bleeding), `PoisonImmune(Poison.X)` (immune to specific poison levels), `CanFly` (flying creature), `Unprovokable` (cannot be attacked first).
- **TML Tags** — "The Metal Lich" tags (TML1-TML5) indicate expansion tier difficulty.
- **BaseCreature properties** — SetStr/SetDex/SetInt, SetHits, SetDamage, SetDamageType, SetResistance, Fame, Karma, VirtualArmor define base stats.

---

## AOS

**21 creatures** — Age of Shadows expansion monsters. Generally high fame (1,500-30,000), most have `PoisonImmune(Poison.Lethal)`. Many carry TML tags indicating high difficulty.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AbysmalHorror | an abysmal horror | Mage | — | — | — | — | 6000 | 13-17 | P:30-35, C:50-55, Po:60-65, E:77-80 | 26000 | -26000 | 5 | PoisonImmune(Poison.Lethal), TML1 |
| BoneDemon | a bone demon | Mage | — | — | — | — | 3600 | 34-36 | — | 20000 | -20000 | 4 | PoisonImmune(Poison.Lethal), TML1 |
| CrystalElemental | a crystal elemental | Mage | — | — | — | — | 150 | 10-15 | P:50-60, F:40-50, C:40-50, E:55-70 | 6500 | -6500 | 5 | BleedImmune, PoisonImmune(Poison.Lethal), TML1 |
| DarknightCreeper | DarknightCreeper | Mage | — | — | — | — | 4000 | 22-26 | — | 22000 | -22000 | 3 | BleedImmune, PoisonImmune(Poison.Lethal), TML1 |
| DemonKnight | DemonKnight | Mage | — | — | — | — | 30000 | 17-21 | — | 28000 | -28000 | 6 | PoisonImmune(Poison.Lethal), TML1 |
| Devourer | a devourer of souls | Mage | — | — | — | — | 650 | 22-26 | P:45-55, F:25-35, C:15-25, Po:60-70, E:40-50 | 9500 | -9500 | 4 | PoisonImmune(Poison.Lethal) |
| FleshGolem | a flesh golem | Melee | — | — | — | — | 106-120 | 18-22 | P:50-60, F:25-35, C:15-25, Po:60-70, E:30-40 | 1000 | -1800 | 3 | BleedImmune, TML1 |
| FleshRenderer | a fleshrenderer | Melee | — | — | — | — | 4500 | 16-20 | P:80-90, F:50-60, C:50-60, E:70-80 | 23000 | -23000 | 2 | PoisonImmune(Poison.Lethal), TML1 |
| Gibberling | a gibberling | Melee | — | — | — | — | 85-99 | 12-17 | P:45-55, F:25-35, C:25-35, Po:10-20, E:30-40 | 1500 | -1500 | 2 | TML1 |
| GoreFiend | a gore fiend | Melee | — | — | — | — | 97-111 | 15-21 | P:35-45, F:25-35, C:15-25, Po:5-15, E:30-40 | 1500 | -1500 | 2 | BleedImmune |
| Impaler | Impaler | Melee | — | — | — | — | 5000 | 31-35 | — | 24000 | -24000 | 4 | PoisonImmune(Poison.Lethal), TML1 |
| MoundOfMaggots | a mound of maggots | Melee | — | — | — | — | — | 3-9 | — | 1000 | -1000 | 2 | PoisonImmune(Poison.Lethal), TML1 |
| PatchworkSkeleton | a patchwork skeleton | Melee | — | — | — | — | 58-72 | 18-22 | P:55-65, F:50-60, C:70-80, E:40-50 | 500 | -500 | 5 | BleedImmune, PoisonImmune(Poison.Lethal), TML1 |
| Ravager | a ravager | Melee | — | — | — | — | 161-175 | 15-20 | P:50-60, F:50-60, C:60-70, Po:30-40, E:20-30 | 3500 | -3500 | 5 | — |
| Revenant | a revenant | Melee | — | — | — | 3 | — | 16-17 | — | — | — | 3 | BleedImmune, PoisonImmune(Poison.Lethal) |
| ShadowKnight | ShadowKnight | Mage | — | — | — | — | 2000 | 20-30 | — | 25000 | -25000 | 5 | PoisonImmune(Poison.Lethal), TML1 |
| SkitteringHopper | a skittering hopper | Melee | Aggressor | Yes (0.0) | — | 1 | 31-45 | 3-5 | P:5-10, C:10-20, E:5-10 | 300 | — | 1 | TML1 |
| Treefellow | a treefellow | Melee | Evil | — | — | — | 118-132 | 12-16 | P:20-25, C:50-60, Po:30-35, E:20-30 | 500 | 1500 | 2 | BleedImmune |
| VampireBat | a vampire bat | Melee | — | — | — | — | 55-66 | 7-9 | P:35-45, F:15-25, C:15-25, Po:60-70, E:40-50 | 1000 | -1000 | 1 | — |
| WailingBanshee | a wailing banshee | Melee | — | — | — | — | 76-90 | 10-14 | P:50-60, F:25-30, C:70-80, Po:30-40, E:40-50 | 1500 | -1500 | 1 | BleedImmune |
| WandererOfTheVoid | a wanderer of the void | Mage | — | — | — | — | 351-400 | 11-13 | P:40-50, F:15-25, C:40-50, Po:50-75, E:40-50 | 20000 | -20000 | 4 | BleedImmune, PoisonImmune(Poison.Lethal) |

The DemonKnight has an extraordinary 30,000 HP and 28,000 fame, making it one of the most challenging AOS creatures. The FleshRenderer has exceptional physical resistance (80-90). The SkitteringHopper is uniquely tamable at 0.0 despite being an AOS creature.

---

## Ants

**11 creatures** — Solen hive creatures from the Stygian Abyss expansion. Red and black variants include Workers, Warriors, and Queens. All use `AI_Melee`, have negative karma, and fame ranges from 1,500-6,500.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AntLion | an ant lion | Melee | — | — | — | — | 151-162 | 7-21 | P:45-60, F:25-35, C:30-40, Po:40-50, E:30-35 | 4500 | -4500 | 4 | — |
| BlackSolenInfiltratorQueen | a black solen infiltrator | Melee | — | — | — | — | 151-162 | 10-15 | P:30-40, F:30-35, C:25-35, Po:35-40, E:25-30 | 6500 | -6500 | 5 | — |
| BlackSolenInfiltratorWarrior | a black solen infiltrator | Melee | — | — | — | — | 96-107 | 5-15 | P:20-35, F:20-35, C:10-25, Po:20-35, E:10-25 | 3000 | -3000 | 4 | — |
| BlackSolenQueen | a black solen queen | Melee | — | — | — | — | 151-162 | 10-15 | P:30-40, F:30-35, C:25-35, Po:35-40, E:25-30 | 4500 | -4500 | 4 | — |
| BlackSolenWarrior | a black solen warrior | Melee | — | — | — | — | 96-107 | 5-15 | P:20-35, F:20-35, C:10-25, Po:20-35, E:10-25 | 3000 | -3000 | 3 | — |
| BlackSolenWorker | a black solen worker | Melee | — | — | — | — | 58-72 | 5-7 | P:25-30, F:20-30, C:10-20, Po:10-20, E:20-30 | 1500 | -1500 | 2 | — |
| RedSolenInfiltratorQueen | a red solen infiltrator | Melee | — | — | — | — | 151-162 | 10-15 | P:30-40, F:30-35, C:25-35, Po:35-40, E:25-30 | 6500 | -6500 | 5 | — |
| RedSolenInfiltratorWarrior | a red solen infiltrator | Melee | — | — | — | — | 96-107 | 5-15 | P:20-35, F:20-35, C:10-25, Po:20-35, E:10-25 | 3000 | -3000 | 4 | — |
| RedSolenQueen | a red solen queen | Melee | — | — | — | — | 151-162 | 10-15 | P:30-40, F:30-35, C:25-35, Po:35-40, E:25-30 | 4500 | -4500 | 4 | — |
| RedSolenWarrior | a red solen warrior | Melee | — | — | — | — | 96-107 | 5-15 | P:20-35, F:20-35, C:10-25, Po:20-35, E:10-25 | 3000 | -3000 | 3 | — |
| RedSolenWorker | a red solen worker | Melee | — | — | — | — | 58-72 | 5-7 | P:25-30, F:20-30, C:10-20, Po:10-20, E:20-30 | 1500 | -1500 | 2 | — |

Solen creatures come in three castes: Workers (lowest fame, 58-72 HP), Warriors (medium, 96-107 HP), and Queens (highest, 151-162 HP). Infiltrator variants have slightly higher stats than regular Warriors.

---

## Arachnid

**8 creatures** — Spiders and Terathan, divided into magical and melee variants. Magic spiders (DreadSpider, TerathanAvenger, TerathanMatriarch) have exceptional poison damage (40-100). Melee spiders are more physically robust.

**Magic (3)** — High poison damage, many with PoisonImmune.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| DreadSpider | a dread spider | Mage | — | — | — | — | 118-132 | 5-17 | P:40-50, F:20-30, C:20-30, Po:90-100, E:20-30 | 5000 | -5000 | 3 | PoisonImmune(Poison.Lethal), TML3 |
| TerathanAvenger | a terathan avenger | Mage | — | — | — | — | 296-372 | 18-22 | P:40-50, F:30-40, C:35-45, Po:90-100, E:35-45 | 15000 | -15000 | 5 | PoisonImmune(Poison.Deadly), TML3 |
| TerathanMatriarch | a terathan matriarch | Mage | — | — | — | — | 190-243 | 11-14 | P:45-55, F:30-40, C:35-45, Po:40-50, E:35-45 | 10000 | -10000 | — | TML4 |

**Melee (5)** — Physical combatants, some tamable.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| FrostSpider | a frost spider | Melee | — | Yes (74.7) | 74.7 | 1 | 46-60 | 6-16 | P:25-30, F:5-10, C:40-50, Po:20-30, E:10-20 | 775 | -775 | 2 | — |
| GiantBlackWidow | a giant black wide | Melee | — | — | — | — | 46-60 | 5-17 | P:20-30, F:10-20, C:10-20, Po:50-60, E:10-20 | 3500 | -3500 | 2 | PoisonImmune(Poison.Deadly) |
| GiantSpider | a giant spider | Melee | — | Yes (59.1) | 59.1 | 1 | 46-60 | 5-13 | P:15-20, Po:25-35 | 600 | -600 | 1 | PoisonImmune(Poison.Regular) |
| TerathanDrone | a terathan drone | Melee | — | — | — | — | 22-39 | 6-12 | P:20-25, F:10-20, C:15-25, Po:30-40, E:15-25 | 2000 | -2000 | 2 | — |
| TerathanWarrior | a terathan warrior | Melee | — | — | — | — | 100-129 | 7-17 | P:30-35, F:20-30, C:25-35, Po:30-40, E:25-35 | 4000 | -4000 | 3 | TML1 |

---

## Elemental

**10 creatures** — Classic elemental creatures: Air, Fire, Ice, Water, Earth, Acid, Blood, Poison, plus ore-specific elementals. Most have `BleedImmune`. Magic elementals spellcast; melee elementals fight physically.

**Magic (8)** — Spellcasting elementals with diverse elemental affinities.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AcidElemental | an acid elemental | Mage | — | — | — | — | 196-213 | 9-15 | P:45-55, F:40-50, C:20-30, Po:10-20, E:30-40 | 10000 | -10000 | 4 | BleedImmune |
| AirElemental | an air elemental | Mage | — | — | — | — | 76-93 | 8-10 | P:35-45, F:15-25, C:10-20, Po:10-20, E:25-35 | 4500 | -4500 | 4 | BleedImmune, TML2 |
| BloodElemental | a blood elemental | Mage | — | — | — | — | 316-369 | 17-27 | P:55-65, F:20-30, C:40-50, Po:50-60, E:30-40 | 12500 | -12500 | 6 | TML5 |
| Efreet | an efreet | Mage | — | — | — | — | 196-213 | 11-13 | P:50-60, F:60-70, Po:30-40, E:40-50 | 10000 | -10000 | 5 | — |
| FireElemental | a fire elemental | Mage | — | — | — | — | 76-93 | 7-9 | P:35-45, F:60-80, C:5-10, Po:30-40, E:30-40 | 4500 | -4500 | 4 | BleedImmune, TML2 |
| IceElemental | an ice elemental | Mage | — | — | — | — | 94-111 | 10-21 | P:35-45, F:5-10, C:50-60, Po:20-30, E:20-30 | 4000 | -4000 | 4 | BleedImmune |
| PoisonElemental | a poison elemental | Mage | — | — | — | — | 256-309 | 12-18 | P:60-70, F:20-30, C:20-30, E:40-50 | 12500 | -12500 | 7 | BleedImmune, PoisonImmune(Poison.Lethal), TML5 |
| WaterElemental | a water elemental | Mage | — | — | — | — | 76-93 | 7-9 | P:35-45, F:10-25, C:10-25, Po:60-70, E:5-10 | 4500 | -4500 | 4 | BleedImmune, TML2 |

**Melee (2)** — Physical elementals, most with BleedImmune.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| EarthElemental | an earth elemental | Melee | — | — | — | — | 76-93 | 9-16 | P:30-35, F:10-20, C:10-20, Po:15-25, E:15-25 | 3500 | -3500 | 3 | BleedImmune, TML1 |
| SnowElemental | a snow elemental | Melee | — | — | — | — | 196-213 | 11-17 | P:45-55, F:10-15, C:60-70, Po:25-35, E:25-35 | 5000 | -5000 | 5 | BleedImmune |

---

## Humanoid

**75 creatures** — Largest monster category, divided into magical and melee combatants. Includes undead (skeletons, zombies, liches), demons (daemons, balrons, succubi), gargoyles, orcs, trolls, and many humanoids.

**Magic (29)** — Spellcasters: Liches, Daemons, Gargoyles, Wraiths, Mages, and more.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AncientLich | AncientLich | Mage | — | — | — | — | 560-595 | 15-27 | P:55-65, F:25-30, C:50-60, Po:50-60, E:25-30 | 23000 | -23000 | 6 | BleedImmune, PoisonImmune(Poison.Lethal), Unprovokable, TML5 |
| ArcaneDaemon | an arcane daemon | Mage | — | — | — | — | 101-115 | 12-16 | P:50-60, F:70-80, C:10-20, Po:50-60, E:30-40 | 7000 | -10000 | 5 | PoisonImmune(Poison.Deadly) |
| Balron | Balron | Mage | — | — | — | — | 592-711 | 22-29 | P:65-80, F:60-80, C:50-60, E:40-50 | 24000 | -24000 | 9 | PoisonImmune(Poison.Deadly), TML5 |
| Betrayer | a betrayer | Mage | — | — | — | — | 241-300 | 16-22 | P:60-70, F:60-70, C:60-70, Po:30-40, E:20-30 | 15000 | -15000 | 6 | PoisonImmune(Poison.Lethal), TML5 |
| Bogle | a bogle | Mage | — | — | — | — | 46-60 | 7-11 | — | 4000 | -4000 | 2 | BleedImmune, PoisonImmune(Poison.Lethal) |
| BoneMagi | a bone mage | Mage | — | — | — | — | 46-60 | 3-7 | P:35-40, F:20-30, C:50-60, Po:20-30, E:30-40 | 3000 | -3000 | 3 | BleedImmune, PoisonImmune(Poison.Regular) |
| Daemon | Daemon | Mage | — | — | — | — | 286-303 | 7-14 | P:45-60, F:50-60, C:30-40, Po:20-30, E:30-40 | 15000 | -15000 | 5 | CanFly, PoisonImmune(Poison.Regular), TML4 |
| ElderGazer | an elder gazer | Mage | — | — | — | — | 178-195 | 8-19 | P:45-55, F:60-70, C:40-50, Po:40-50, E:40-50 | 12500 | -12500 | 5 | — |
| EvilMage | EvilMage | Mage | — | — | — | — | 49-63 | 5-10 | P:15-20, F:5-10, Po:5-10, E:5-10 | 2500 | -2500 | 1 | — |
| EvilMageLord | EvilMageLord | Mage | — | — | — | — | 49-63 | 5-10 | P:35-40, F:30-40, C:30-40, Po:30-40, E:30-40 | 10500 | -10500 | 1 | — |
| FireGargoyle | FireGargoyle | Mage | — | — | — | — | 211-240 | 7-14 | P:30-35, F:50-60, Po:20-30, E:20-30 | 3500 | -3500 | 3 | CanFly, TML1 |
| Gargoyle | a gargoyle | Mage | — | — | — | — | 88-105 | 7-14 | P:30-35, F:25-35, C:5-10, Po:15-25 | 3500 | -3500 | 3 | CanFly, TML1 |
| GargoyleDestroyer | a gargoyle destroyer | Mage | — | — | — | — | 482-485 | 7-14 | P:40-60, F:60-70, C:15-25, Po:15-25, E:15-25 | 10000 | -10000 | 5 | CanFly |
| GargoyleEnforcer | a gargoyle enforcer | Mage | — | — | — | — | 482-485 | 7-14 | P:40-60, F:50-60, C:20-30, Po:25-35, E:15-25 | 5000 | -5000 | 5 | CanFly |
| Gazer | a gazer | Mage | — | — | — | — | 58-75 | 5-10 | P:35-40, F:40-50, C:20-30, Po:10-20, E:20-30 | 3500 | -3500 | 3 | TML1 |
| GolemController | GolemController | Mage | — | — | — | — | 76-90 | 6-12 | P:30-40, F:25-35, C:35-45, Po:5-15, E:15-25 | 4000 | -4000 | 1 | — |
| IceFiend | an ice fiend | Mage | — | — | — | — | 226-243 | 8-19 | P:55-65, F:10-20, C:60-70, Po:20-30, E:30-40 | 18000 | -18000 | 6 | CanFly, TML4 |
| Imp | an imp | Mage | — | Yes (83.1) | 83.1 | 2 | 55-70 | 10-14 | P:25-35, F:40-50, C:20-30, Po:30-40, E:30-40 | 2500 | -2500 | 3 | CanFly |
| Lich | a lich | Mage | — | — | — | — | 103-120 | 24-26 | P:40-60, F:20-30, C:50-60, Po:55-65, E:40-50 | 8000 | -8000 | 5 | BleedImmune, PoisonImmune(Poison.Lethal), TML3 |
| LichLord | a lich lord | Mage | — | — | — | — | 250-303 | 11-13 | P:40-50, F:30-40, C:50-60, Po:50-60, E:40-50 | 18000 | -18000 | 5 | BleedImmune, PoisonImmune(Poison.Lethal), TML4 |
| OrcishMage | an orcish mage | Mage | — | — | — | — | 70-90 | 4-14 | P:25-35, F:30-40, C:20-30, Po:30-40, E:30-40 | 3000 | -3000 | 3 | TML1 |
| RatmanMage | RatmanMage | Mage | — | — | — | — | 88-108 | 7-14 | P:40-45, F:10-20, C:10-20, Po:10-20, E:10-20 | 7500 | -7500 | 4 | — |
| SavageShaman | SavageShaman | Mage | — | — | — | — | — | 4-10 | P:30-40, F:20-30, C:20-30, Po:20-30, E:40-50 | 1000 | -1000 | — | — |
| Shade | a shade | Mage | — | — | — | — | 46-60 | 7-11 | P:25-30, C:15-25, Po:10-20 | 4000 | -4000 | 2 | BleedImmune, PoisonImmune(Poison.Lethal) |
| SkeletalMage | a skeletal mage | Mage | — | — | — | — | 46-60 | 3-7 | P:35-40, F:20-30, C:50-60, Po:20-30, E:30-40 | 3000 | -3000 | 3 | BleedImmune, PoisonImmune(Poison.Regular) |
| Spectre | a spectre | Mage | — | — | — | — | 46-60 | 7-11 | P:25-30, C:15-25, Po:10-20 | 4000 | -4000 | 2 | BleedImmune, PoisonImmune(Poison.Lethal) |
| Succubus | a succubus | Mage | — | — | — | — | 312-353 | 18-28 | P:80-90, F:70-80, C:40-50, Po:50-60, E:50-60 | 24000 | -24000 | 8 | TML5 |
| Titan | a titan | Mage | — | — | — | — | 322-351 | 13-16 | P:35-45, F:30-40, C:25-35, Po:30-40, E:30-40 | 11500 | -11500 | 4 | PoisonImmune(Poison.Regular), TML5 |
| Wraith | a wraith | Mage | — | — | — | — | 46-60 | 7-11 | P:25-30, C:15-25, Po:10-20 | 4000 | -4000 | 2 | BleedImmune, PoisonImmune(Poison.Lethal) |

The Balron has the highest fame (24,000) and virtual armor (9) among humanoid mages. AncientLich is Unprovokable with 23,000 fame. Many undead mages have BleedImmune and PoisonImmune.

**Melee (46)** — Physical combatants: Skeletons, Zombies, Ghouls, Orcs, Trolls, Cyclops, Ogres, Minotaurs, and more.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| ArcticOgreLord | an arctic ogre lord | Melee | — | — | — | — | 476-552 | 20-25 | P:45-55, C:60-70, E:40-50 | 15000 | -15000 | 5 | PoisonImmune(Poison.Regular), TML3 |
| BoneKnight | a bone knight | Melee | — | — | — | — | 118-150 | 8-18 | P:35-45, F:20-30, C:50-60, Po:20-30, E:30-40 | 3000 | -3000 | 4 | BleedImmune |
| Brigand | Brigand | Melee | — | — | — | — | — | 10-23 | — | 1000 | -1000 | — | — |
| ChaosDaemon | a chaos daemon | Melee | — | — | — | — | 91-110 | 12-17 | P:50-60, F:60-70, C:40-50, Po:20-30, E:20-30 | 3000 | -4000 | 1 | — |
| Cursed | Cursed | Melee | — | — | — | — | 91-120 | 5-13 | P:15-25, F:5-10, C:25-35, Po:5-10, E:5-10 | 1000 | -2000 | — | — |
| Cyclops | a cyclopean warrior | Melee | — | — | — | — | 202-231 | 7-23 | P:45-50, F:30-40, C:25-35, Po:30-40, E:30-40 | 4500 | -4500 | 4 | TML3 |
| Doppleganger | a doppleganger | Melee | — | — | — | — | 101-120 | 8-12 | P:50-60, F:10-20, C:40-50, Po:50-60, E:30-40 | 1000 | -1000 | 5 | — |
| ElfBrigand | ElfBrigand | Melee | — | — | — | — | — | 10-23 | — | 1000 | -1000 | — | — |
| EnslavedGargoyle | an enslaved gargoyle | Melee | — | — | — | — | 186-212 | 7-14 | P:30-40, F:50-70, C:15-25, Po:25-30, E:25-30 | 3500 | — | 3 | TML1 |
| Ettin | an ettin | Melee | — | — | — | — | 82-99 | 7-17 | P:35-40, F:15-25, C:40-50, Po:15-25, E:15-25 | 3000 | -3000 | 3 | TML1 |
| Executioner | Executioner | Melee | — | — | — | — | — | 8-10 | P:35-45, F:25-30, C:25-30, Po:10-20, E:10-20 | 5000 | -5000 | 4 | — |
| FrostTroll | a frost troll | Melee | — | — | — | — | 140-156 | 14-20 | P:45-55, C:40-50, Po:5-10, E:5-10 | 4000 | -4000 | 5 | TML1 |
| GazerLarva | a gazer larva | Melee | — | — | — | — | 36-47 | 2-9 | P:15-25 | 900 | -900 | 2 | — |
| Ghoul | a ghoul | Melee | — | — | — | — | 46-60 | 7-9 | P:25-30, C:20-30, Po:5-10, E:10-20 | 2500 | -2500 | 2 | BleedImmune, PoisonImmune(Poison.Regular) |
| GreaterMongbat | a greater mongbat | Melee | — | Yes (71.1) | 71.1 | 1 | 34-48 | 5-7 | P:15-25 | 450 | -450 | 1 | — |
| Guardian | Guardian | Archer | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HeadlessOne | a headless one | Melee | — | — | — | — | 16-30 | 5-10 | P:15-20 | 450 | -450 | 1 | — |
| HordeMinion | a horde minion | Melee | — | — | — | — | 10-24 | 5-10 | P:15-20, F:5-10 | 500 | -500 | 1 | — |
| Juggernaut | a blackthorn juggernaut | Melee | — | — | — | — | 181-240 | 12-19 | P:65-75, F:35-45, C:35-45, Po:15-25, E:10-20 | 12000 | -12000 | 7 | BleedImmune, PoisonImmune(Poison.Lethal), TML5 |
| KhaldunRevenant | a revenant | Melee | — | — | — | — | 241-300 | 20-30 | P:55-65, F:30-40, C:60-70, Po:20-30, E:20-30 | — | — | 6 | PoisonImmune(Poison.Lethal) |
| KhaldunSummoner | Zealot of Khaldun | Mage | — | — | — | — | 421-480 | 5-15 | P:35-40, F:25-30, C:50-60, Po:25-35, E:25-35 | 10000 | -10000 | 3 | Unprovokable |
| KhaldunZealot | Zealot of Khaldun | Melee | — | — | — | — | 448-470 | 15-25 | P:35-45, F:25-30, C:50-60, Po:25-35, E:25-35 | 10000 | -10000 | 4 | PoisonImmune(Poison.Deadly), Unprovokable |
| Moloch | a moloch | Melee | — | — | — | — | 171-200 | 15-23 | P:60-70, F:60-70, C:40-50, Po:20-30, E:20-30 | 7500 | -7500 | 3 | PoisonImmune(Poison.Regular) |
| Mongbat | a mongbat | Melee | — | Yes (0.0) | — | 1 | 4-6 | 1-2 | P:5-10 | 150 | -150 | 1 | CanFly |
| Mummy | a mummy | Melee | — | — | — | — | 208-222 | 13-23 | P:45-55, F:10-20, C:50-60, Po:20-30, E:20-30 | 4000 | -4000 | 5 | BleedImmune, PoisonImmune(Poison.Lesser) |
| Ogre | an ogre | Melee | — | — | — | — | 100-117 | 9-11 | P:30-35, F:15-25, C:15-25, Po:15-25 | 3000 | -3000 | 3 | TML1 |
| OgreLord | an ogre lord | Melee | — | — | — | — | 476-552 | 20-25 | P:45-55, F:30-40, C:30-40, Po:40-50, E:40-50 | 15000 | -15000 | 5 | PoisonImmune(Poison.Regular), TML3 |
| Orc | Orc | Melee | — | — | — | — | 58-72 | 5-7 | P:25-30, F:20-30, C:10-20, Po:10-20, E:20-30 | 1500 | -1500 | 2 | TML1 |
| OrcBomber | an orc bomber | Melee | — | — | — | — | 95-123 | 1-8 | P:25-35, F:30-40, C:15-25, Po:15-20, E:25-30 | 2500 | -2500 | 3 | — |
| OrcBrute | an orc brute | Melee | — | — | — | — | 476-552 | 20-25 | P:45-55, F:40-50, C:25-35, Po:25-35, E:25-35 | 15000 | -15000 | 5 | PoisonImmune(Poison.Lethal) |
| OrcCaptain | OrcCaptain | Melee | — | — | — | — | 67-87 | 5-15 | P:30-35, F:10-20, C:15-25, Po:5-10, E:5-10 | 2500 | -2500 | 3 | — |
| OrcishLord | an orcish lord | Melee | — | — | — | — | 95-123 | 4-14 | P:25-35, F:30-40, C:20-30, Po:30-40, E:30-40 | 2500 | -2500 | — | TML1 |
| Ratman | Ratman | Melee | — | — | — | — | 58-72 | 4-5 | P:25-30, F:10-20, C:10-20, Po:10-20, E:10-20 | 1500 | -1500 | 2 | — |
| RatmanArcher | RatmanArcher | Archer | — | — | — | — | 88-108 | 4-10 | P:40-55, F:10-20, C:10-20, Po:10-20, E:10-20 | 6500 | -6500 | 5 | — |
| RestlessSoul | a restless soul | Melee | — | — | — | — | 16-24 | 1-10 | P:15-25, F:5-15, C:25-40, Po:5-10, E:10-20 | 500 | -500 | 6 | BleedImmune |
| RottingCorpse | a rotting corpse | Melee | — | — | — | — | 1200 | 8-10 | P:35-45, F:20-30, C:50-70, Po:40-50, E:20-30 | 6000 | -6000 | 4 | BleedImmune, PoisonImmune(Poison.Lethal), TML5 |
| Savage | Savage | Melee | — | — | — | — | — | 23-27 | — | 1000 | -1000 | — | — |
| SavageRider | SavageRider | Melee | — | — | — | — | — | 29-34 | — | 1000 | -1000 | — | — |
| ShadowFiend | a shadow fiend | Melee | — | — | — | — | 28-33 | 10-22 | P:20-25, F:20-25, C:40-45, Po:60-70, E:5-10 | 1000 | -1000 | — | — |
| SkeletalKnight | a skeletal knight | Melee | — | — | — | — | 118-150 | 8-18 | P:35-45, F:20-30, C:50-60, Po:20-30, E:30-40 | 3000 | -3000 | 4 | BleedImmune |
| Skeleton | a skeleton | Melee | — | — | — | — | 34-48 | 3-7 | P:15-20, F:5-10, C:25-40, Po:25-35, E:5-15 | 450 | -450 | 1 | BleedImmune, PoisonImmune(Poison.Lesser) |
| SpectralArmour | a spectral armour | Melee | — | — | — | — | 178-201 | 10-22 | P:35-45, F:20-30, C:30-40, Po:20-30, E:20-30 | 7000 | -7000 | 4 | PoisonImmune(Poison.Regular) |
| StoneGargoyle | a stone gargoyle | Melee | — | — | — | — | 148-165 | 11-17 | P:45-55, F:20-30, C:10-20, Po:30-40, E:30-40 | 4000 | -4000 | 5 | TML2 |
| StrongMongbat | a mongbat | Melee | — | Yes (71.1) | 71.1 | 1 | 4-6 | 5-7 | P:15-25 | 150 | -150 | 1 | — |
| Troll | a troll | Melee | — | — | — | — | 106-123 | 8-14 | P:35-45, F:25-35, C:15-25, Po:5-15, E:5-15 | 3500 | -3500 | 4 | TML1 |
| Zombie | a zombie | Melee | — | — | — | — | 28-42 | 3-7 | P:15-20, C:20-30, Po:5-10 | 600 | -600 | 1 | BleedImmune, PoisonImmune(Poison.Regular) |

The Juggernaut has exceptional virtual armor (7) and is immune to Lethal poison. ArcticOgreLord and OgreLord share the highest fame (15,000) among humanoid melee. Most undead have BleedImmune.

---

## LBR (Lords Blackthorn Rising)

**12 creatures** — Exodus expansion creatures from the Malas. Divided into Exodus minions, Juka warriors, and Meer warriors.

**Exodus (2)** — High-fame minions of the Exodus.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| ExodusMinion | an exodus minion | Melee | — | — | — | — | 511-570 | 16-22 | P:60-70, F:40-50, C:15-25, Po:15-25, E:15-25 | 18000 | -18000 | 6 | PoisonImmune(Poison.Lethal) |
| ExodusOverseer | an exodus overseer | Melee | — | — | — | — | 331-390 | 13-19 | P:45-55, F:40-60, C:25-35, Po:25-35, E:25-35 | 10000 | -10000 | 5 | PoisonImmune(Poison.Lethal) |

**Jukas (5)** — Juka Lord, Mage, Warrior, and Chaos Dragoon variants.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| ChaosDragoon | a chaos dragoon | Melee | — | — | — | — | 176-225 | 24-26 | — | 5000 | -5000 | — | — |
| ChaosDragoonElite | a chaos dragoon elite | Mage | — | — | — | — | 276-350 | 29-34 | P:45-55, F:15-25, Po:25-35, E:25-35 | 8000 | -8000 | — | — |
| JukaLord | a juka lord | Archer | Closest | — | — | — | 241-300 | 10-12 | P:40-50, F:45-50, C:40-50, Po:20-25, E:40-50 | 15000 | -15000 | 2 | — |
| JukaMage | a juka mage | Mage | — | — | — | — | 121-180 | 4-10 | P:20-30, F:35-45, C:30-40, Po:10-20, E:35-45 | 15000 | -15000 | 1 | — |
| JukaWarrior | a juka warrior | Melee | — | — | — | — | 151-210 | 7-9 | P:40-50, F:30-40, C:25-35, Po:10-20, E:10-20 | 10000 | -10000 | 2 | — |

**Meers (5)** — Meer warriors and mages, aligned with the Good cause (positive karma).

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BaseEnraged | a rabbit | Melee | — | — | — | — | — | — | — | — | — | — | — |
| MeerCaptain | a meer captain | Archer | Evil | — | — | — | 58-66 | 5-15 | P:45-55, F:10-20, C:40-50, Po:35-45, E:35-45 | 2000 | 5000 | 2 | — |
| MeerEternal | a meer eternal | Mage | Evil | — | — | — | 250-303 | 11-13 | P:45-55, F:15-25, C:45-55, Po:30-40, E:30-40 | 18000 | 18000 | 3 | PoisonImmune(Poison.Lethal) |
| MeerMage | a meer mage | Mage | Evil | — | — | — | 103-120 | 24-26 | P:45-55, F:15-25, Po:25-35, E:25-35 | 8000 | 8000 | 1 | PoisonImmune(Poison.Lethal), TML3 |
| MeerWarrior | a meer warrior | Melee | Evil | — | — | — | 52-60 | 12-19 | P:35-45, F:5-15, C:30-40, Po:25-35, E:25-35 | 2000 | 5000 | 2 | — |

Meers have positive karma (2,000-18,000), reflecting their alignment with the forces of good. The MeerEternal has the highest fame (18,000) among LBR creatures.

---

## ML (Mercenary Lords)

**30 creatures** — Mercenary Lords expansion creatures from the Twisted Weald and Blighted Grove. Includes powerful named bosses, prism creatures, and twisted weald denizens.

**Animal (3)** — ML-specific animal creatures.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Ferret | a ferret | Animal | Aggressor | Yes (0.0) | — | 1 | 45-50 | 7-9 | P:45-50, F:10-14, C:30-40, Po:21-25, E:20-25 | — | — | — | — |
| RagingGrizzlyBear | a raging grizzly bear | Animal | Aggressor | — | — | — | 751-930 | 18-23 | P:50-70, C:30-50, Po:10-20, E:10-20 | 10000 | 10000 | 2 | — |
| Squirrel | a squirrell | Animal | Aggressor | Yes (0.0) | — | 1 | 42-50 | 1-2 | P:30-34, F:10-14, C:30-35, Po:20-25, E:20-25 | — | — | — | — |

RagingGrizzlyBear has positive karma (10,000), unlike most aggressive animals.

**Blighted Grove (1)** — The Blighted Grove dungeon boss.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Hydra | a hydra | Melee | — | — | — | — | 1480-1500 | 21-26 | P:65-75, F:70-85, C:25-35, Po:35-43, E:36-45 | — | — | — | TML5 |

**Humanoid/Magic (4)** — ML-specific spellcasters.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| FetidEssence | a fetid essence | Mage | — | — | — | — | 551-650 | 21-25 | P:40-50, F:40-50, C:40-50, Po:70-90, E:75-80 | 3700 | -3700 | — | PoisonImmune(Poison.Deadly) |
| InterredGrizzle | a interred grizzle | Mage | — | — | — | — | 1500 | 16-19 | P:35-55, F:20-65, C:55-80, Po:20-35, E:60-80 | 3700 | -3700 | — | — |
| MLDryad | a dryad | Mage | Evil | — | — | — | 304-321 | 11-20 | P:40-50, F:15-25, C:40-45, Po:30-40, E:25-35 | 5000 | 5000 | 2 | — |
| Satyr | a satyr | Animal | Aggressor | — | — | — | 350-400 | 13-24 | P:55-60, F:25-35, C:30-40, Po:30-40, E:30-40 | 5000 | — | 2 | — |

**Humanoid/Melee (8)** — ML-specific physical humanoids.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| CorruptedSoul | a corrupted soul | Melee | — | — | — | — | 61-69 | 4-40 | P:61-74, F:22-48, C:73-100, E:51-60 | 5000 | -5000 | — | BleedImmune |
| FeralTreefellow | a feral treefellow | Melee | Evil | — | — | — | 1170-1320 | 26-35 | P:60-70, C:70-80, Po:60-70, E:40-60 | 12500 | 12500 | 2 | BleedImmune |
| Minotaur | a minotaur | Melee | — | — | — | — | — | — | — | — | — | — | — |
| MinotaurCaptain | a minotaur captain | Melee | — | — | — | — | — | — | — | — | — | — | — |
| MinotaurScout | a minotaur scout | Melee | — | — | — | — | — | — | — | — | — | — | — |
| PestilentBandage | a pestilent bandage | Melee | — | — | — | — | — | — | — | — | — | — | — |
| TormentedMinotaur | Tormented Minotaur | Melee | — | — | — | — | 4000-4200 | 16-30 | — | 20000 | -20000 | — | PoisonImmune(Poison.Deadly), TML3 |
| Troglodyte | a troglodyte | Melee | — | — | — | — | — | — | — | — | — | — | — |

TormentedMinotaur has 20,000 fame and 4,000+ HP. FeralTreefellow has positive karma (12,500). Several ML humanoid entries have incomplete data.

**Misc/Magic (1)** — The Greater Dragon, one of the most tamable creatures.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| GreaterDragon | a greater dragon | Mage | — | Yes (104.7) | 104.7 | 5 | 1000-2000 | 24-33 | P:60-85, F:65-90, C:40-55, Po:40-60, E:50-75 | 22000 | -15000 | 6 | CanFly, TML5 |

The GreaterDragon requires 104.7 taming skill (5 control slots) and has exceptional fire resistance (65-90).

**Misc/Melee (1)**

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| CorrosiveSlime | a corrosive slime | Melee | — | Yes (23.1) | 23.1 | 1 | 15-19 | 1-5 | P:5-10, Po:15-20 | 300 | -300 | 8 | PoisonImmune(Poison.Regular) |

**Prism of Light (7)** — Crystal-based creatures from the ML Prism expansion.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| CorporealBrume | a corporeal brume | Melee | — | — | — | — | 1150-1250 | 21-25 | F:40-50, C:40-50, Po:50-60, E:30-40 | 12000 | -12000 | — | — |
| CrystalDaemon | a crystal daemon | Mage | — | — | — | — | 200-220 | 16-20 | P:20-40, F:0-20, C:60-80, Po:20-40, E:65-75 | 15000 | -15000 | — | — |
| CrystalLatticeSeeker | Crystal Lattice Seeker | Mage | — | — | — | — | 350-550 | 13-19 | P:80-90, F:40-50, C:40-50, Po:40-50, E:40-50 | 17000 | -17000 | — | TML5 |
| CrystalVortex | a crystal vortex | Melee | — | — | — | — | 350-400 | 15-20 | P:60-80, F:0-10, C:70-80, Po:40-50, E:60-90 | 17000 | -17000 | — | — |
| MantraEffervescence | a mantra effervescence | Mage | — | — | — | — | 150-250 | 21-25 | P:60-65, F:40-50, C:40-50, Po:50-60 | 6500 | -6500 | — | — |
| Protector | a Protector | Melee | — | — | — | — | 350-450 | 6-12 | P:30-40, F:20-30, C:35-40, Po:30-40, E:30-40 | 10000 | -10000 | — | — |
| UnfrozenMummy | an unfrozen mummy | Mage | — | — | — | — | 1500 | 16-20 | P:35-40, F:20-30, C:60-80, Po:20-30, E:70-80 | 25000 | -25000 | — | — |

CrystalVortex has exceptional energy resistance (60-90) and physical resistance (60-80). UnfrozenMummy has the highest fame (25,000) in this subcategory.

**Special (3)** — The three ML world bosses: Ilhenir, Meraktus, and Twaulo. All are Unprovokable.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Ilhenir | Ilhenir | Mage | — | — | — | — | 9000 | 21-28 | P:55-65, F:50-60, C:55-65, Po:70-90, E:65-75 | 50000 | -50000 | 4 | PoisonImmune(Poison.Lethal), Unprovokable, TML5 |
| Meraktus | Meraktus | Melee | — | — | — | — | 4100-4200 | 16-30 | P:65-90, F:65-70, C:50-60, Po:40-60, E:50-55 | 70000 | -70000 | 2 | PoisonImmune(Poison.Regular), Unprovokable, TML3 |
| Twaulo | Twaulo | Melee | — | — | — | — | 7500 | 19-24 | P:65-75, F:45-55, C:50-60, Po:50-60, E:50-60 | 50000 | 50000 | 5 | PoisonImmune(Poison.Regular), Unprovokable, TML5 |

These are among the most powerful creatures in the game. Meraktus has 70,000 fame (the highest of any creature). All three are Unprovokable, meaning they cannot be attacked first.

**Twisted Weald (1)**

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Changeling | a changeling | Mage | — | — | — | — | 201-211 | 9-15 | P:81-90, F:40-50, C:40-49, Po:40-50, E:43-50 | 15000 | -15000 | — | — |

---

## Mammal

**2 creatures** — HellHound and VorpalBunny.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| HellHound | a hell hound | Melee | — | Yes (85.5) | 85.5 | 1 | 66-125 | 11-17 | P:25-35, F:30-40, Po:10-20, E:10-20 | 3400 | -3400 | 3 | — |
| VorpalBunny | a vorpal bunny | Melee | — | — | — | — | 2000 | 1 | — | 1000 | — | 4 | — |

HellHounds have fire resistance (30-40) and require 85.5 taming skill.

---

## Misc

**16 creatures** — Miscellaneous monsters not fitting other categories: animated weapons, elementals, centaur, vortices, golems, slimes, and plant-like creatures.

**Magic (5)** — Wises, ethereals, and pixies.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| DarkWisp | a wisp | Mage | Aggressor | — | — | — | 118-135 | 17-18 | P:35-45, F:20-40, C:10-30, Po:5-10, E:50-70 | 4000 | -4000 | 4 | — |
| EtherealWarrior | EtherealWarrior | Mage | Evil | — | — | — | 352-471 | 13-19 | P:80-90, F:40-50, C:40-50, Po:40-50, E:40-50 | 7000 | 7000 | 1 | — |
| Pixie | Pixie | Mage | Evil | — | — | — | 13-18 | 9-15 | P:80-90, F:40-50, C:40-50, Po:40-50, E:40-50 | 7000 | 7000 | 1 | — |
| ShadowWisp | a shadow wisp | Mage | Aggressor | — | — | — | 10-24 | 5-10 | P:15-20, F:5-10, Po:5-10, E:15-20 | 500 | — | 1 | — |
| Wisp | a wisp | Mage | Aggressor | — | — | — | 118-135 | 17-18 | P:35-45, F:20-40, C:10-30, Po:5-10, E:50-70 | 4000 | — | 4 | — |

EtherealWarrior and Pixie have positive karma (7,000) despite being evil-aligned. DarkWisp and Wisp have exceptional energy resistance (50-70).

**Melee (11)** — Physical misc monsters including animated weapons, golems, slimes, and vortices.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AnimatedWeapon | an animated weapon | Melee | — | — | — | 4 | — | 14-18 | P:40-50, F:30-40, C:30-40, E:20-30 | — | — | — | BleedImmune, PoisonImmune(Poison.Lethal) |
| BladeSpirits | a blade spirit | Melee | — | — | — | — | — | 10-14 | P:30-40, F:40-50, C:30-40, E:20-30 | — | — | 4 | BleedImmune, PoisonImmune(Poison.Lethal) |
| Centaur | Centaur | Melee | Aggressor | — | — | — | 130-172 | 13-24 | P:45-55, F:35-45, C:25-35, Po:45-55, E:35-45 | 6500 | — | 5 | — |
| EnergyVortex | an energy vortex | Melee | — | — | — | — | — | 14-17 | P:60-70, F:40-50, C:40-50, Po:40-50, E:90-100 | — | — | 4 | BleedImmune, PoisonImmune(Poison.Lethal) |
| FrostOoze | a frost ooze | Melee | — | — | — | — | 13-17 | 3-9 | P:15-20, C:40-50, Po:20-30, E:10-20 | 450 | -450 | 3 | — |
| Golem | a golem | Melee | — | — | — | 3 | — | — | — | 10 | 10 | — | BleedImmune, PoisonImmune(Poison.Lethal) |
| PlagueBeast | a plague beast | Melee | — | — | — | — | 318-404 | 20-24 | P:45-55, F:40-50, C:25-35, Po:65-75, E:25-35 | 13000 | -13000 | 3 | PoisonImmune(Poison.Lethal) |
| PlagueBeastLord | a plague beast lord | Melee | — | — | — | — | 1800 | 20-25 | P:45-55, F:40-50, C:25-35, Po:75-85, E:25-35 | 2000 | -2000 | 5 | PoisonImmune(Poison.Lethal) |
| PlagueSpawn | a plague spawn | Melee | — | — | — | — | 121-180 | 11-17 | P:35-45, F:30-40, C:25-35, Po:65-75, E:25-35 | 1000 | -1000 | 2 | — |
| SandVortex | a sand vortex | Melee | — | — | — | — | 51-62 | 3-16 | P:80-90, F:60-70, C:60-70, Po:60-70, E:60-70 | 4500 | -4500 | 2 | — |
| Slime | a slime | Melee | — | Yes (23.1) | 23.1 | 1 | 15-19 | 1-5 | P:5-10, Po:10-20 | 300 | -300 | 8 | PoisonImmune(Poison.Lesser) |

EnergyVortex has exceptional energy resistance (90-100). SandVortex has very high resistances across all types (60-90). The Golem has only 10 fame but 10 karma (positive). PlagueBeastLord has 75-85 poison resistance.

---

## Ore Elementals

**8 creatures** — Elementals tied to specific ore types, all using `AI_Melee`. They have `BleedImmune` and TML1 tags, with fame ranging from 3,500-5,000.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AgapiteElemental | an agapite elemental | Melee | — | — | — | — | 136-153 | 28 | P:30-40, F:40-50, C:40-50, Po:30-40, E:10-20 | 3500 | -3500 | 3 | BleedImmune, TML1 |
| BronzeElemental | a bronze elemental | Melee | — | — | — | — | 136-153 | 9-16 | P:30-40, F:30-40, C:10-20, Po:70-80, E:20-30 | 5000 | -5000 | 2 | BleedImmune, TML1 |
| CopperElemental | a copper elemental | Melee | — | — | — | — | 136-153 | 9-16 | P:30-40, F:30-40, C:30-30, Po:20-30, E:10-20 | 4800 | -4800 | 2 | BleedImmune, TML1 |
| DullCopperElemental | a dull copper elemental | Melee | — | — | — | — | 136-153 | 9-16 | P:30-40, F:30-40, C:10-20, Po:20-30, E:20-30 | 3500 | -3500 | 2 | BleedImmune, TML1 |
| GoldenElemental | a golden elemental | Melee | — | — | — | — | 136-153 | 9-16 | P:60-75, F:10-20, C:30-40, Po:30-40, E:30-40 | 3500 | -3500 | 6 | BleedImmune, TML1 |
| ShadowIronElemental | a shadow iron elemental | Melee | — | — | — | — | 136-153 | 9-16 | P:30-40, F:30-40, C:20-30, Po:10-20, E:30-40 | 4500 | -4500 | 2 | BreathImmune, BleedImmune, PoisonImmune(Poison.Deadly), TML1 |
| ValoriteElemental | a valorite elemental | Melee | — | — | — | — | 136-153 | 28 | P:65-75, F:50-60, C:50-60, Po:50-60, E:40-50 | 3500 | -3500 | 3 | BleedImmune, TML1 |
| VeriteElemental | a verite elemental | Melee | — | — | — | — | 136-153 | 9-16 | P:30-40, F:10-20, C:50-60, Po:50-60, E:50-60 | 3500 | -3500 | 3 | BleedImmune, TML1 |

GoldenElemental and ValoriteElemental have high physical resistance (60-75, 65-75). BronzeElemental has exceptional poison resistance (70-80). ShadowIronElemental is uniquely BreathImmune and PoisonImmune(Poison.Deadly).

---

## Plant

**7 creatures** — Plant-like monsters: bog creatures, vines, and the reaper. Most have `PoisonImmune` flags.

**Magic (1)**

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Reaper | a reaper | Mage | — | — | — | — | 40-129 | 9-11 | P:35-45, F:15-25, C:10-20, Po:40-50, E:30-40 | 3500 | -3500 | 4 | PoisonImmune(Poison.Greater), TML2 |

**Melee (6)** — Bog creatures and vines.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BogThing | a bog thing | Melee | — | — | — | — | 481-540 | 10-23 | P:30-40, F:20-25, C:10-15, Po:40-50, E:20-25 | 8000 | -8000 | 2 | PoisonImmune(Poison.Lethal) |
| Bogling | a bogling | Melee | — | — | — | — | 58-72 | 5-7 | P:20-25, F:10-20, C:15-25, Po:15-25, E:15-25 | 450 | -450 | 2 | — |
| Corpser | a corpser | Melee | — | — | — | — | 94-108 | 10-23 | P:15-20, F:15-25, C:10-20, Po:20-30 | 1000 | -1000 | 1 | PoisonImmune(Poison.Lesser) |
| Quagmire | a quagmire | Melee | — | — | — | — | 91-105 | 10-14 | P:50-60, F:10-20, C:10-20, E:20-30 | 1500 | -1500 | 3 | PoisonImmune(Poison.Lethal) |
| SwampTentacle | a swamp tentacle | Melee | — | — | — | — | 58-72 | 6-12 | P:25-35, F:10-20, C:10-20, Po:60-80, E:10-20 | 3000 | -3000 | 2 | PoisonImmune(Poison.Greater) |
| WhippingVine | a whipping vine | Melee | — | — | — | — | — | 7-25 | P:75-85, F:15-25, C:15-25, Po:75-85, E:35-45 | 1000 | -1000 | 4 | PoisonImmune(Poison.Lethal) |

WhippingVine has exceptional physical and poison damage (75-85). BogThing has the highest fame (8,000) in this subcategory.

---

## Reptile

**21 creatures** — Dragons, serpents, and lizardfolk. Divided into magical (12) and melee (9) variants. Many have `CanFly` and high resistance values.

**Magic (12)** — Dragon-type spellcasters and serpent mages.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AncientWyrm | an ancient wyrm | Mage | — | — | — | — | 658-711 | 29-35 | P:65-75, F:80-90, C:70-80, Po:60-70, E:60-70 | 22500 | -22500 | 7 | CanFly, PoisonImmune(Poison.Regular), TML5 |
| DeepSeaSerpent | a deep sea serpent | Mage | — | — | — | — | 151-255 | 6-14 | P:30-40, F:70-80, C:40-50, Po:30-40, E:15-20 | 6000 | -6000 | 6 | — |
| Dragon | a dragon | Mage | — | Yes (93.9) | 93.9 | 3 | 478-495 | 16-22 | P:55-65, F:60-70, C:30-40, Po:25-35, E:35-45 | 15000 | -15000 | 6 | CanFly, TML4 |
| Leviathan | a leviathan | Mage | — | — | — | — | 1500 | 25-33 | P:55-65, F:45-55, C:45-55, Po:35-45, E:25-35 | 24000 | -24000 | 5 | TML5 |
| OphidianArchmage | OphidianArchmage | Mage | — | — | — | — | 169-183 | 5-10 | P:40-45, F:20-30, C:25-35, Po:35-40, E:25-35 | 11500 | -11500 | 4 | — |
| OphidianMage | OphidianMage | Mage | — | — | — | — | 109-123 | 5-10 | P:25-35, F:30-40, C:35-45, Po:40-50, E:35-45 | 4000 | -4000 | 3 | TML2 |
| OphidianMatriarch | an ophidian matriarch | Mage | — | — | — | — | 250-303 | 11-13 | P:45-55, F:30-40, C:35-45, Po:40-50, E:35-45 | 16000 | -16000 | 5 | PoisonImmune(Poison.Greater), TML4 |
| SeaSerpent | a sea serpent | Mage | — | — | — | — | 110-127 | 7-13 | P:25-35, F:50-60, C:30-40, Po:30-40, E:15-20 | 6000 | -6000 | 3 | TML2 |
| SerpentineDragon | a serpentine dragon | Mage | Evil | — | — | — | 480 | 5-12 | P:35-40, F:25-35, C:25-35, Po:25-35, E:25-35 | 15000 | 15000 | 3 | TML4 |
| ShadowWyrm | a shadow wyrm | Mage | — | — | — | — | 558-599 | 29-35 | P:65-75, F:50-60, C:45-55, Po:20-30, E:50-60 | 22500 | -22500 | 7 | CanFly, PoisonImmune(Poison.Deadly), TML5 |
| SkeletalDragon | a skeletal dragon | Mage | — | — | — | — | 558-599 | 29-35 | P:75-80, F:40-60, C:40-60, Po:70-80, E:40-60 | 22500 | -22500 | 8 | BleedImmune, PoisonImmune(Poison.Lethal) |
| WhiteWyrm | a white wyrm | Mage | — | Yes (96.3) | 96.3 | 3 | 433-456 | 17-25 | P:55-70, F:15-25, C:80-90, Po:40-50, E:40-50 | 18000 | -18000 | 6 | CanFly, TML4 |

AncientWyrm and ShadowWyrm have exceptional fire resistance (80-90, 50-60) and 22,500 fame. SkeletalDragon has the highest virtual armor (8) in this subcategory. Dragon and WhiteWyrm are tamable at 93.9 and 96.3 respectively. SerpentineDragon has positive karma (15,000).

**Melee (9)** — Dragons, drakes, and lizardfolk warriors.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Drake | a drake | Melee | — | Yes (84.3) | 84.3 | 2 | 241-258 | 11-17 | P:45-50, F:50-60, C:40-50, Po:20-30, E:30-40 | 5500 | -5500 | 4 | CanFly, TML2 |
| Harpy | a harpy | Melee | — | — | — | — | 58-72 | 5-7 | P:25-30, F:10-20, C:10-30, Po:20-30, E:10-20 | 2500 | -2500 | 2 | CanFly |
| Kraken | a kraken | Melee | — | — | — | — | 454-468 | 19-33 | P:45-55, F:30-40, C:30-40, Po:20-30, E:10-20 | 11000 | -11000 | 5 | TML4 |
| Lizardman | Lizardman | Melee | — | — | — | — | 58-72 | 5-7 | P:25-30, F:5-10, C:5-10, Po:10-20 | 1500 | -1500 | 2 | — |
| OphidianKnight | OphidianKnight | Melee | — | — | — | — | 266-342 | 16-19 | P:35-40, F:30-40, C:35-45, Po:90-100, E:35-45 | 10000 | -10000 | 4 | PoisonImmune(Poison.Lethal), TML3 |
| OphidianWarrior | OphidianWarrior | Melee | — | — | — | — | 128-155 | 5-11 | P:35-40, F:20-30, C:25-35, Po:30-40, E:25-35 | 4500 | -4500 | 3 | TML1 |
| Scorpion | a scorpion | Melee | — | Yes (47.1) | 47.1 | 1 | 50-63 | 5-10 | P:20-25, F:10-15, C:20-25, Po:40-50, E:10-15 | 2000 | -2000 | 2 | PoisonImmune(Poison.Greater) |
| StoneHarpy | a stone harpy | Melee | — | — | — | — | 178-192 | 8-16 | P:45-55, F:20-30, C:10-20, Po:30-40, E:30-40 | 4500 | -4500 | 5 | CanFly |
| Wyvern | a wyvern | Melee | — | — | — | — | 125-141 | 8-19 | P:35-45, F:30-40, C:20-30, Po:90-100, E:30-40 | 4000 | -4000 | 4 | CanFly, PoisonImmune(Poison.Deadly), TML2 |

OphidianKnight has exceptional poison resistance (90-100). Wyvern also has 90-100 poison damage. Drake is tamable at 84.3.

---

## SE (Samurai Empire)

**18 creatures** — Samurai Empire expansion creatures. Includes BakeKitsune, DeathwatchBeetles, ninjas, fan dancers, oni, and the boss Yamandon. Many have BleedImmune.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BakeKitsune | a bake kitsune | Mage | — | Yes (80.7) | 80.7 | 2 | 301-350 | 15-22 | P:40-60, F:70-90, C:40-60, Po:40-60, E:40-60 | 8000 | -8000 | — | — |
| DeathwatchBeetle | a deathwatch beetle | Melee | — | Yes (41.1) | 41.1 | 1 | 121-145 | 5-10 | P:35-40, F:15-30, C:15-30, Po:50-80, E:20-35 | 1400 | -1400 | — | — |
| DeathwatchBeetleHatchling | a deathwatch beetle hatchling | Melee | — | — | — | — | 51-60 | 2-8 | P:35-40, F:15-30, C:15-30, Po:20-40, E:20-35 | 700 | -700 | — | — |
| EliteNinja | an elite ninja | Melee | — | — | — | — | 251-350 | 12-20 | P:35-65, F:40-60, C:25-45, Po:40-60, E:35-55 | 8500 | -8500 | — | — |
| FanDancer | a fan dancer | Melee | — | — | — | — | 351-430 | 12-17 | P:40-60, F:50-70, C:50-70, Po:50-70, E:40-60 | 9000 | -9000 | — | — |
| Kappa | a kappa | Melee | — | — | — | — | 151-180 | 6-12 | P:35-50, F:35-50, C:25-50, Po:35-50, E:20-30 | 1700 | -1700 | — | — |
| KazeKemono | a kaze kemono | Mage | — | — | — | — | 251-330 | 15-20 | P:50-70, F:30-60, C:30-60, Po:50-70, E:60-80 | 8000 | -8000 | — | BleedImmune |
| LadyOfTheSnow | a lady of the snow | Mage | — | — | — | — | 596-625 | 13-20 | P:45-55, F:40-55, C:70-90, Po:60-70, E:65-85 | 15200 | -15200 | — | BleedImmune, TML4 |
| Oni | an oni | Mage | — | — | — | — | 401-530 | 14-20 | P:65-80, F:50-70, C:35-50, Po:45-70, E:45-65 | 12000 | -12000 | — | TML4 |
| RaiJu | a Rai-Ju | Melee | — | — | — | — | 201-280 | 12-15 | P:45-65, F:70-85, C:30-60, Po:50-70, E:60-80 | 8000 | -8000 | — | BleedImmune |
| RevenantLion | a Revenant Lion | Mage | — | — | — | — | 251-280 | 18-24 | P:40-60, F:20-30, C:50-60, Po:55-65, E:40-50 | 4000 | -4000 | — | BleedImmune, PoisonImmune(Poison.Greater)
| Ronin | a ronin | Melee | — | — | — | — | 301-400 | 17-25 | P:55-75, F:40-60, C:35-55, Po:50-70, E:55-75 | 8500 | -8500 | — | — |
| RuneBeetle | a rune beetle | Mage | — | Yes (93.9) | 93.9 | 3 | 301-360 | 15-22 | P:40-65, F:35-50, C:35-50, Po:75-95, E:40-60 | 15000 | -15000 | — | PoisonImmune(Poison.Greater)
| TsukiWolf | a tsuki wolf | Melee | — | — | — | — | 376-450 | 14-18 | P:40-60, F:50-70, C:50-70, Po:50-70, E:50-70 | 8500 | -8500 | — | — |
| Yamandon | a yamandon | Melee | — | — | — | — | 1601-1800 | 19-35 | P:65-85, F:70-90, C:50-70, Po:50-70, E:50-70 | 22000 | -22000 | — | PoisonImmune(Poison.Lethal), TML5 |
| YomotsuElder | a yomotsu elder | Melee | — | — | — | — | 801-900 | 19-27 | P:65-85, F:30-50, C:45-65, Po:35-55, E:25-50 | 12000 | -12000 | — | TML5 |
| YomotsuPriest | a yomotsu priest | Mage | — | — | — | — | 486-530 | 8-10 | P:65-85, F:30-50, C:45-65, Po:35-55, E:25-50 | 9000 | -9000 | — | — |
| YomotsuWarrior | a yomotsu warrior | Melee | — | — | — | — | 486-530 | 8-10 | P:65-85, F:30-50, C:45-65, Po:35-55, E:25-50 | 4200 | -4200 | — | TML3 |

Yamandon is the SE expansion boss with 22,000 fame and 1,601-1,800 HP. LadyOfTheSnow has exceptional cold resistance (70-90). RuneBeetle is tamable at 93.9 with poison immunity. BakeKitsune has exceptional fire resistance (70-90). Many SE creatures have BleedImmune.

---

## Summons

**5 creatures** — Spell-summoned creatures from magic spells. These are not tamable and have no fame/karma. They have `BleedImmune` and lower HP pools than their natural counterparts.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| SummonedAirElemental | an air elemental | Mage | — | — | — | 2 | 76-93 | 6-9 | P:35-45, F:15-25, C:10-20, Po:10-20, E:25-35 | — | — | 4 | BleedImmune |
| SummonedDaemon | SummonedDaemon | Mage | — | — | — | — | 286-303 | 14-21 | P:45-60, F:50-60, C:30-40, Po:20-30, E:30-40 | — | — | 5 | CanFly, PoisonImmune(Poison.Regular) |
| SummonedEarthElemental | an earth elemental | Melee | — | — | — | 2 | 76-93 | 14-21 | P:30-35, F:10-20, C:10-20, Po:15-25, E:15-25 | — | — | 3 | BleedImmune |
| SummonedFireElemental | a fire elemental | Mage | — | — | — | 4 | 76-93 | 9-14 | P:35-45, F:60-80, C:5-10, Po:30-40, E:30-40 | — | — | 4 | BleedImmune |
| SummonedWaterElemental | a water elemental | Mage | — | — | — | 3 | 76-93 | 12-16 | P:35-45, F:10-25, C:10-25, Po:60-70, E:5-10 | — | — | 4 | BleedImmune |

Summoned creatures are created by necromancy and magery spells. SummonedDaemon is the only one with CanFly. They use control slots (2-4) when summoned.

---

## See Also

- [Animals](animals.md) — Non-hostile fauna and mounts
- [NPCs](npcs.md) — Non-hostile characters (vendors, healers, guards)
- [Bosses](bosses.md) — Elite creatures and champion spawns
- [Creature Reference](reference/creature-reference.md) — Complete creature listing
- [Creature AI Types](reference/creature-ai-types.md) — AI type behavior descriptions
- [Combat](systems/combat.md) — Combat mechanics
- [Poisons](poisons.md) — Poison types and resistance
- [Monster Slaying](skills/monsterslaying.md) — Monster Slaying skill
- [Weapons](items/weapons.md) — Weapon damage types
- [Armor](items/armor.md) — Armor vs creature damage
