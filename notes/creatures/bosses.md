# Bosses

Bosses are elite creatures that pose significant challenges and offer substantial rewards. They include champion spawns (with artifact drops), paragon variants, named bosses, and high-tier expansion monsters. Bosses are the primary source of endgame content, with unique loot tables, special abilities, and spawn mechanics.

Bosses are defined across `Projects/UOContent/Mobiles/Special/` (named bosses), `Projects/UOContent/Mobiles/Abilities/` (monster abilities), and high-fame creatures in `Projects/UOContent/Mobiles/Monsters/`.

---

## Key Concepts

- **Champions** — Spawn with `ChampionSkullType`, drop artifacts and power scrolls. Artifact drop rates: 5% unique, 10% shared, 15% decorative (30% total). Power scrolls drop on Felucca only: 5% level 20, 40% level 15, 55% level 10. Champions also grant Valor virtue points. Defined via `BaseChampion` class with unique/shared/decorative artifact lists.
- **Paragons** — Stat-boosted variants of regular creatures found on Ilshenar. Conversion multiplies: fame/karma ×1.4, skills ×1.2, Str ×1.05, Dex/Int ×1.2, Hits ×1.1, Speed ×1.2, +5 damage. Hue 0x501. Drops: paragon chest (10%), chocolatiering ingredient (20%), regular loot. Capped at 32,000 fame/karma. Conversion via `Paragon.Convert(BaseCreature bc)`.
- **Monster Abilities** — Special behaviors that bosses may possess. Single target: ColossalBlow, DrainLife, StunAttack, GraspingClaw. Area effects: DeathExplosion, DrainLifeAreaAttack, PoisonGasAreaAttack, FanningFire, BloodBathAttack. Counters: EnergyBoltCounter, FanThrowCounter, PoisonGasCounter, SummonPixiesCounter, ThrowHatchetCounter, ReflectPhysicalDamage. Persistent: MagicalBarrier, RuneCorruption.
- **Unprovokable** — Creatures that cannot be attacked first; they only defend. Common among high-tier bosses.
- **Fame Thresholds** — Boss monsters typically have fame above 10,000, with elite bosses exceeding 20,000-70,000. Fame indicates difficulty and reward potential.
- **Resistances** — Bosses often have exceptional resistances (60-100) across multiple types, making resistance management critical for combat.

---

## Named Bosses

**12 creatures** — Special named creatures in `Projects/UOContent/Mobiles/Special/`. These are unique bosses with defined stats, abilities, and lore.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Barracoon | Barracoon | Melee | — | — | — | — | 4200 | 25-35 | P:60-70, F:50-60, C:50-60, Po:40-50, E:40-50 | 22500 | -22500 | 7 | PoisonImmune(Poison.Deadly) |
| ChaosGuard | ChaosGuard | Melee | — | — | — | — | — | — | — | — | — | — | — | Shield guardian variant |
| DarkGuardian | a dark guardian | Mage | — | — | — | — | 150-180 | 43-48 | P:40-50, F:20-45, C:50-60, Po:20-45, E:30-40 | 5000 | -5000 | 5 | BleedImmune, PoisonImmune(Poison.Lethal), Unprovokable, TML2 |
| Harrower | the harrower | Mage | Closest | — | — | — | — | — | P:55-65, F:60-80, C:60-80, Po:60-80, E:60-80 | 22500 | -22500 | 6 | PoisonImmune(Poison.Lethal), Unprovokable |
| HarrowerTentacles | tentacles of the harrower | Melee | — | — | — | — | 541-600 | 13-20 | P:55-65, F:35-45, C:35-45, Po:35-45, E:35-45 | 15000 | -15000 | 6 | PoisonImmune(Poison.Lethal), Unprovokable |
| LordOaks | Lord Oaks | Mage | Evil | — | — | — | 3000 | 21-33 | P:85-90, F:60-70, C:60-70, Po:80-90, E:80-90 | 22500 | 22500 | 1 | CanFly, PoisonImmune(Poison.Deadly) |
| Mephitis | Mephitis | Melee | — | — | — | — | 3000 | 21-33 | P:75-80, F:60-70, C:60-70, E:60-70 | 22500 | -22500 | 8 | PoisonImmune(Poison.Lethal) |
| Neira | Neira | Mage | — | — | — | — | 4800 | 25-35 | P:25-30, F:35-45, C:50-60, Po:30-40, E:20-30 | 22500 | -22500 | 3 | PoisonImmune(Poison.Deadly) |
| Rikktor | Rikktor | Melee | — | — | — | — | 3000 | 28-55 | P:80-90, F:80-90, C:30-40, Po:80-90, E:80-90 | 22500 | -22500 | 1 | PoisonImmune(Poison.Lethal) |
| Semidar | Semidar | Mage | — | — | — | — | 1500 | 29-35 | P:20-30, F:50-60, C:20-30, Po:20-30, E:10-20 | 24000 | -24000 | 2 | PoisonImmune(Poison.Lethal), Unprovokable |
| ServantOfSemidar | a Servant of Semidar | Melee | None | — | — | — | — | — | — | — | — | — | — | — |
| Serado | Serado | Melee | — | — | — | — | 9000 | 29-35 | — | — | 22500 | -22500 | — | PoisonImmune(Poison.Lethal), TML5 |
| Silvani | Silvani | Mage | Evil | — | — | — | 600 | 27-38 | P:45-55, F:30-40, C:30-40, Po:40-50, E:40-50 | 20000 | 20000 | 5 | CanFly, PoisonImmune(Poison.Regular), Unprovokable, TML5 |

Named bosses have distinctive mechanics. The Harrower spawns HarrowerTentacles (600 HP, 13-20 damage). LordOaks and Silvani have positive karma (+22500, +20000) and CanFly. Rikktor has exceptional resistances (80-90) across most types. Serado has the highest HP (9000) among named bosses.

---

## Paragons

**Dynamic variants** — Paragons are not standalone creatures but stat-boosted versions of regular creatures. They are created via the `Paragon.Convert(BaseCreature bc)` method and appear on Ilshenar.

Paragons apply the following multipliers to base creatures:
- **Fame/Karma** ×1.4 (capped at 32,000)
- **Skills** ×1.2
- **Strength** ×1.05
- **Dexterity/Intelligence** ×1.2
- **Hits** ×1.1
- **Speed** ×1.2 (reduced movement time)
- **Damage** +5 (flat addition)
- **Hue** changes to 0x501 (golden)

Drop table:
- **Paragon chest** — 10% chance, contains artifacts including GoldBricks, PhillipsWoodenSteed, AlchemistsBauble, ArcticDeathDealer, BlazeOfDeath, BowOfTheJukaKing, GwennosHarp, LunaLance, NightsKiss, OrcishVisage, ShieldOfInvulnerability, StaffOfPower, WrathOfTheDryad, and more
- **Chocolatiering ingredient** — 20% chance
- **Regular loot** — Standard creature drops

Paragons use fast regeneration (0.5s intervals) with CPU-saving pauses (2s).

---

## Champions

**Champion spawns** — Champions are dynamic spawns created via the `BaseChampion` system. They spawn with a `ChampionSkullType` that determines their artifact drop table.

Artifact drop mechanics (30% total drop rate):
- **Unique artifacts** — 5% chance, one from the champion's unique list
- **Shared artifacts** — 10% chance (cumulative), one from the shared list
- **Decorative artifacts** — 15% chance (cumulative), one from the decorative list

Power scroll drops (Felucca only):
- **Level 20** — 5% chance
- **Level 15** — 40% chance
- **Level 10** — 55% chance

Champions grant Valor virtue points to damage dealers. Up to 6 power scrolls can be distributed among players with looting rights. Power scrolls bypass the standard virtue delay for Valor gains.

Each `BaseChampion` subclass defines:
- `SkullType` — Determines artifact pool
- `UniqueList` — One-time unique artifacts
- `SharedList` — Common artifacts
- `DecorativeList` — Decorative/statuette artifacts
- `StatueTypes` — MonsterStatuetteType options

---

## High-Tier Expansion Bosses

**Top-tier creatures** from expansion packs with fame above 10,000. These represent the most challenging regular (non-champion, non-paragon) creatures in the game.

### Age of Shadows (AOS)

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AbysmalHorror | an abysmal horror | Mage | — | — | — | — | 6000 | 13-17 | P:30-35, C:50-55, Po:60-65, E:77-80 | 26000 | -26000 | 5 | PoisonImmune(Poison.Lethal), TML1 |
| BoneDemon | a bone demon | Mage | — | — | — | — | 3600 | 34-36 | — | 20000 | -20000 | 4 | PoisonImmune(Poison.Lethal), TML1 |
| DarknightCreeper | DarknightCreeper | Mage | — | — | — | — | 4000 | 22-26 | — | 22000 | -22000 | 3 | BleedImmune, PoisonImmune(Poison.Lethal), TML1 |
| DemonKnight | DemonKnight | Mage | — | — | — | — | 30000 | 17-21 | — | 28000 | -28000 | 6 | PoisonImmune(Poison.Lethal), TML1 |
| FleshRenderer | a fleshrenderer | Melee | — | — | — | — | 4500 | 16-20 | P:80-90, F:50-60, C:50-60, E:70-80 | 23000 | -23000 | 2 | PoisonImmune(Poison.Lethal), TML1 |
| Impaler | Impaler | Melee | — | — | — | — | 5000 | 31-35 | — | 24000 | -24000 | 4 | PoisonImmune(Poison.Lethal), TML1 |
| ShadowKnight | ShadowKnight | Mage | — | — | — | — | 2000 | 20-30 | — | 25000 | -25000 | 5 | PoisonImmune(Poison.Lethal), TML1 |
| WandererOfTheVoid | a wanderer of the void | Mage | — | — | — | — | 351-400 | 11-13 | P:40-50, F:15-25, C:40-50, Po:50-75, E:40-50 | 20000 | -20000 | 4 | BleedImmune, PoisonImmune(Poison.Lethal) |

The DemonKnight has extraordinary 30,000 HP and 28,000 fame. The FleshRenderer has exceptional physical resistance (80-90).

### Monster Legacy (ML)

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AncientLich | AncientLich | Mage | — | — | — | — | 560-595 | 15-27 | P:55-65, F:25-30, C:50-60, Po:50-60, E:25-30 | 23000 | -23000 | 6 | BleedImmune, PoisonImmune(Poison.Lethal), Unprovokable, TML5 |
| Balron | Balron | Mage | — | — | — | — | 592-711 | 22-29 | P:65-80, F:60-80, C:50-60, E:40-50 | 24000 | -24000 | 9 | PoisonImmune(Poison.Deadly), TML5 |
| Betrayer | a betrayer | Mage | — | — | — | — | 241-300 | 16-22 | P:60-70, F:60-70, C:60-70, Po:30-40, E:20-30 | 15000 | -15000 | 6 | PoisonImmune(Poison.Lethal), TML5 |
| Ilhenir | Ilhenir | Mage | — | — | — | — | 9000 | 21-28 | P:55-65, F:50-60, C:55-65, Po:70-90, E:65-75 | 50000 | -50000 | 4 | PoisonImmune(Poison.Lethal), Unprovokable, TML5 |
| Meraktus | Meraktus | Melee | — | — | — | — | 4100-4200 | 16-30 | P:65-90, F:65-70, C:50-60, Po:40-60, E:50-55 | 70000 | -70000 | 2 | PoisonImmune(Poison.Regular), Unprovokable, TML3 |
| Succubus | a succubus | Mage | — | — | — | — | 312-353 | 18-28 | P:80-90, F:70-80, C:40-50, Po:50-60, E:50-60 | 24000 | -24000 | 8 | TML5 |
| Twaulo | Twaulo | Melee | — | — | — | — | 7500 | 19-24 | P:65-75, F:45-55, C:50-60, Po:50-60, E:50-60 | 50000 | 50000 | 5 | PoisonImmune(Poison.Regular), Unprovokable, TML5 |
| UnfrozenMummy | an unfrozen mummy | Mage | — | — | — | — | 1500 | 16-20 | P:35-40, F:20-30, C:60-80, Po:20-30, E:70-80 | 25000 | -25000 | — | — |

Meraktus has the highest fame in the game at 70,000. Ilhenir and Twaulo have 50,000 fame. All ML bosses have high resistances (50-90 range).

### Samurai Empire (SE)

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| LadyOfTheSnow | a lady of the snow | Mage | — | — | — | — | 596-625 | 13-20 | P:45-55, F:40-55, C:70-90, Po:60-70, E:65-85 | 15200 | -15200 | — | BleedImmune, TML4 |
| Oni | an oni | Mage | — | — | — | — | 401-530 | 14-20 | P:65-80, F:50-70, C:35-50, Po:45-70, E:45-65 | 12000 | -12000 | — | TML4 |
| RuneBeetle | a rune beetle | Mage | — | Yes (93.9) | 93.9 | 3 | 301-360 | 15-22 | P:40-65, F:35-50, C:35-50, Po:75-95, E:40-60 | 15000 | -15000 | — | PoisonImmune(Poison.Greater) |
| Yamandon | a yamandon | Melee | — | — | — | — | 1601-1800 | 19-35 | P:65-85, F:70-90, C:50-70, Po:50-70, E:50-70 | 22000 | -22000 | — | PoisonImmune(Poison.Lethal), TML5 |

RuneBeetle is uniquely tamable at 93.9 among SE bosses. Yamandon has exceptional resistances (50-90). LadyOfTheSnow has extreme cold resistance (70-90).

### Reptile Bosses

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| AncientWyrm | an ancient wyrm | Mage | — | — | — | — | 658-711 | 29-35 | P:65-75, F:80-90, C:70-80, Po:60-70, E:60-70 | 22500 | -22500 | 7 | CanFly, PoisonImmune(Poison.Regular), TML5 |
| Leviathan | a leviathan | Mage | — | — | — | — | 1500 | 25-33 | P:55-65, F:45-55, C:45-55, Po:35-45, E:25-35 | 24000 | -24000 | 5 | TML5 |
| SkeletalDragon | a skeletal dragon | Mage | — | — | — | — | 558-599 | 29-35 | P:75-80, F:40-60, C:40-60, Po:70-80, E:40-60 | 22500 | -22500 | 8 | BleedImmune, PoisonImmune(Poison.Lethal) |

These flying reptilian bosses have exceptional resistances and are among the most challenging creatures. SkeletalDragon has the highest VA (8) among reptile bosses.

### Other Notable Bosses

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| GreaterDragon | a greater dragon | Mage | — | Yes (104.7) | 104.7 | 5 | 1000-2000 | 24-33 | P:60-85, F:65-90, C:40-55, Po:40-60, E:50-75 | 22000 | -15000 | 6 | CanFly, TML5 |
| TormentedMinotaur | Tormented Minotaur | Melee | — | — | — | — | 4000-4200 | 16-30 | — | 20000 | -20000 | — | PoisonImmune(Poison.Deadly), TML3 |
| BloodElemental | a blood elemental | Mage | — | — | — | — | 316-369 | 17-27 | P:55-65, F:20-30, C:40-50, Po:50-60, E:30-40 | 12500 | -12500 | 6 | TML5 |
| PoisonElemental | a poison elemental | Mage | — | — | — | — | 256-309 | 12-18 | P:60-70, F:20-30, C:20-30, E:40-50 | 12500 | -12500 | 7 | BleedImmune, PoisonImmune(Poison.Lethal), TML5 |
| Juggernaut | a blackthorn juggernaut | Melee | — | — | — | — | 181-240 | 12-19 | P:65-75, F:35-45, C:35-45, Po:15-25, E:10-20 | 12000 | -12000 | 7 | BleedImmune, PoisonImmune(Poison.Lethal), TML5 |

GreaterDragon is the most tamable boss at 104.7 taming skill. The Blood and Poison Elementals are high-tier elementals with TML5 tags.

---

## Monster Abilities

**Special boss behaviors** — Bosses may possess one or more monster abilities that trigger during combat. Abilities are defined in `Projects/UOContent/Mobiles/Abilities/`.

### Single Target Abilities
- **ColossalBlow** — Powerful melee attack with increased damage
- **DrainLife** — Steals health from target on hit
- **StunAttack** — Chance to stun target on melee hit
- **GraspingClaw** — Claw attack with special effects

### Area Effects
- **DeathExplosion** — Explodes on death, damaging nearby creatures
- **DrainLifeAreaAttack** — Area drain life effect
- **PoisonGasAreaAttack** — Releases poisonous gas in an area
- **FanningFire** — Cone-shaped fire attack
- **BloodBathAttack** — Blood-themed area attack

### Counters
- **EnergyBoltCounter** — Counters spellcasting with energy bolts
- **FanThrowCounter** — Counters melee with fan throws
- **PoisonGasCounter** — Counters with poison gas
- **SummonPixiesCounter** — Counters by summoning pixies
- **ThrowHatchetCounter** — Counters with hatchet throws
- **ReflectPhysicalDamage** — Reflects physical damage back to attacker

### Persistent Effects
- **MagicalBarrier** — Maintains a protective magical shield
- **RuneCorruption** — Corrupts nearby runes

Abilities are grouped via `MonsterAbilityGroup` and triggered by `MonsterAbilityTrigger` based on combat conditions (hit chance, timer, etc.).

---

## See Also

- [Taming](../skills/taming.md) — Taming high-skill creatures
- [Combat](../systems/combat.md) — Combat mechanics and resistance management
- [Virtues](../systems/virtues.md) — Valor virtue and champion skulls
- [Jewels](../items/jewels.md) — Artifacts and champion loot
- [Creature Reference](../reference/creature-reference.md) — Complete creature listing
