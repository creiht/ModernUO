# Animals

Animals are non-sentient fauna across all maps of Britannia, ranging from passive farm animals to aggressive predators. There are **54 animal types** organized into 10 subcategories, plus 5 animal-type familiars summoned by necromancy spells. Many animals are tamable via the Taming skill and useful as mounts, pets, pack animals, or for resource harvesting.

Animals are defined in `Projects/UOContent/Mobiles/Animals/` and inherit from `BaseCreature`. Their behavior is controlled by `AIType.Animal` (simple animal behavior) or `AIType.Melee` (for aggressive species).

---

## Key Concepts

- **Tamability** — Most animals can be tamed via the Taming skill. Difficulty ranges from `0.0` (town critters) to `89.1` (PredatorHellCat). Tamed animals follow their owner and can be set to various stances (Assist, Attack, Defend, etc.).
- **Control Slots** — Most animals use 1 control slot. Higher-tame-skill animals may require more slots.
- **Food Types** — Animals consume specific food types: Meat, FruitsAndVeggies, GrainsAndHay, Fish, Eggs, Gold, Leather, and Metal. Feeding correct food reduces taming difficulty.
- **Pack Instincts** — Certain animals share pack bonuses: Canine (wolves), Feline (cats), Bear, Equine, Bull, Ostard, Arachnid, Daemon. Pack members gain stat bonuses when near each other.
- **Hides/Meat drops** — Animals drop hides and meat on death, important for crafting and cooking. Hide type is defined per creature.
- **AI Types** — Most animals use `AI_Animal` (simple behavior). Aggressive or dangerous animals use `AI_Melee` instead.

---

## Bears

**4 creatures** — Aggressive predators with the **Bear** pack instinct. All tamable, dropping 12 hides each.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BlackBear | a black bear | Animal | Aggressor | Yes (35.1) | 35.1 | 1 | 46-60 | 4-10 | P:20-25, C:10-15, Po:5-10 | 450 | — | 2 | — |
| BrownBear | a brown bear | Animal | Aggressor | Yes (41.1) | 41.1 | 1 | 46-60 | 6-12 | P:20-30, C:15-20, Po:10-15 | 450 | — | 2 | — |
| GrizzlyBear | a grizzly bear | Animal | Aggressor | Yes (59.1) | 59.1 | 1 | 76-93 | 8-13 | P:25-35, C:15-25, Po:5-10, E:5-10 | 1000 | — | 2 | — |
| PolarBear | a polar bear | Animal | Aggressor | Yes (35.1) | 35.1 | 1 | 70-84 | 7-12 | P:25-35, C:60-80, Po:15-25, E:10-15 | 1500 | — | 1 | — |

Polar Bears have exceptional cold resistance (60-80) reflecting their arctic habitat. Grizzly Bears are the most balanced with moderate resistances across all types.

---

## Birds

**4 creatures** — Flying animals ranging from farm chickens to the mythical Phoenix.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Chicken | a chicken | Animal | Aggressor | Yes (0.0) | — | 1 | 3 | 1 | P:1-5 | 150 | — | 2 | CanFly |
| Crane | a crane | Animal | Aggressor | — | — | — | 26-35 | 1 | P:5 | — | 200 | 5 | — |
| Eagle | an eagle | Animal | Aggressor | Yes (17.1) | 17.1 | 1 | 20-27 | 5-10 | P:20-25, F:10-15, C:20-25, Po:5-10, E:5-10 | 300 | — | 2 | CanFly |
| Phoenix | a phoenix | Mage | Aggressor | — | — | — | 340-383 | 25 | P:45-55, F:60-70, Po:25-35, E:40-50 | 15000 | — | 6 | CanFly |

The Phoenix is the most powerful bird, using `AI_Mage` spellcasting with high fire resistance (60-70). It requires 6 control slots and has 15,000 fame. Cranes and the Phoenix are not tamable.

---

## Canines

**4 creatures** — Pack-hunting predators with the **Canine** pack instinct. All tamable.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| DireWolf | a dire wolf | Melee | — | Yes (83.1) | 83.1 | 1 | 58-72 | 11-17 | P:20-25, F:10-20, C:5-10, Po:5-10, E:10-15 | 2500 | -2500 | 2 | — |
| GreyWolf | a grey wolf | Animal | Aggressor | Yes (53.1) | 53.1 | 1 | 34-48 | 3-7 | P:15-20, F:10-15, C:20-25, Po:10-15, E:10-15 | 450 | — | 1 | — |
| TimberWolf | a timber wolf | Animal | Aggressor | Yes (23.1) | 23.1 | 1 | 34-48 | 5-9 | P:15-20, F:5-10, C:10-15, Po:5-10, E:5-10 | 450 | — | 1 | — |
| WhiteWolf | a white wolf | Animal | Aggressor | Yes (65.1) | 65.1 | 1 | 34-48 | 3-7 | P:15-20, F:10-15, C:20-25, Po:10-15, E:10-15 | 450 | — | 1 | — |

Dire Wolves use `AI_Melee` combat AI and have negative karma (-2500), making them evil-aligned. They require the highest taming skill (83.1) among canines.

---

## Cows

**2 creatures** — Passive farm animals, easiest to tame among larger animals.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Bull | a bull | Animal | Aggressor | Yes (71.1) | 71.1 | 1 | 50-64 | 4-9 | P:25-30, C:10-15 | 600 | — | 2 | — |
| Cow | a cow | Animal | Aggressor | Yes (11.1) | 11.1 | 1 | 18 | 1-4 | P:5-15 | 300 | — | 1 | — |

---

## Felines

**5 creatures** — Stealth predators with the **Feline** pack instinct. HellCat variants use `AI_Melee` and have fire resistance.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Cougar | a cougar | Animal | Aggressor | Yes (41.1) | 41.1 | 1 | 34-48 | 4-10 | P:20-25, F:5-10, C:10-15, Po:5-10 | 450 | — | 1 | — |
| HellCat | a hell cat | Melee | — | Yes (71.1) | 71.1 | 1 | 48-67 | 6-12 | P:25-35, F:80-90, E:15-20 | 1000 | -1000 | 3 | — |
| Panther | a panther | Animal | Aggressor | Yes (53.1) | 53.1 | 1 | 37-51 | 4-12 | P:20-25, F:5-10, C:10-15, Po:5-10 | 450 | — | 1 | — |
| PredatorHellCat | a hell cat | Melee | — | Yes (89.1) | 89.1 | 1 | 97-131 | 5-17 | P:25-35, F:30-40, E:5-15 | 2500 | -2500 | 3 | — |
| SnowLeopard | a snow leopard | Animal | Aggressor | Yes (53.1) | 53.1 | 1 | 34-48 | 3-9 | P:20-25, F:5-10, C:30-40, Po:10-20, E:20-30 | 450 | — | 2 | — |

HellCats are fiendish felines with exceptional fire resistance (80-90) and negative karma. PredatorHellCats are the most powerful feline variant, requiring 89.1 taming skill.

---

## Misc

**16 creatures** — Diverse animals including pack animals, amphibians, and exotic species. Dolphins have positive karma (2000) while Gaman has negative karma (-2000).

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Boar | a boar | Animal | Aggressor | Yes (29.1) | 29.1 | 1 | 15 | 3-6 | P:10-15, F:5-10, Po:5-10 | 300 | — | 1 | — |
| BullFrog | a bull frog | Animal | Aggressor | Yes (23.1) | 23.1 | 1 | 28-42 | 1-2 | P:5-10 | 350 | — | 6 | — |
| Dolphin | a dolphin | Animal | Aggressor | — | — | — | 15-27 | 3-6 | P:15-20, F:70-80, C:25-30, Po:10-15, E:10-15 | 500 | 2000 | 1 | — |
| Gaman | a gaman | Animal | Aggressor | Yes (68.7) | 68.7 | 1 | 131-160 | 6-11 | P:50-70, F:30-50, C:30-50, Po:40-60, E:30-50 | 2000 | -2000 | — | — |
| GiantToad | a giant toad | Melee | — | Yes (77.1) | 77.1 | 1 | 46-60 | 5-17 | P:20-25, F:5-10, E:5-10 | 750 | -750 | 2 | — |
| Goat | a goat | Animal | Aggressor | Yes (11.1) | 11.1 | 1 | 12 | 3-4 | P:5-15 | 150 | — | 1 | — |
| Gorilla | a gorilla | Animal | Aggressor | Yes (0.0) | — | 1 | 38-51 | 4-10 | P:20-25, F:5-10, C:10-15 | 450 | — | 2 | — |
| GreatHart | a great hart | Animal | Aggressor | Yes (59.1) | 59.1 | 1 | 27-41 | 5-9 | P:20-25, C:5-10 | 300 | — | 2 | — |
| Hind | a hind | Animal | Aggressor | Yes (23.1) | 23.1 | 1 | 15-29 | 4 | P:5-15 | 300 | — | 8 | — |
| Llama | a llama | Animal | Aggressor | Yes (35.1) | 35.1 | 1 | 15-27 | 3-5 | P:15-20 | 300 | — | 1 | — |
| MountainGoat | a mountain goat | Animal | Aggressor | Yes (0.0) | — | 1 | 20-33 | 3-7 | P:10-20, F:5-10, C:10-20, Po:10-15, E:10-15 | 300 | — | 1 | — |
| PackHorse | a pack horse | Animal | Aggressor | Yes (11.1) | 11.1 | 1 | 61-80 | 5-11 | P:20-25, F:10-15, C:20-25, Po:10-15, E:10-15 | — | 200 | 1 | — |
| PackLlama | a pack llama | Animal | Aggressor | Yes (11.1) | 11.1 | 1 | 50 | 2-6 | P:25-35, F:10-15, C:10-15, Po:10-15, E:10-15 | — | 200 | 1 | — |
| Pig | a pig | Animal | Aggressor | Yes (11.1) | 11.1 | 1 | 12 | 2-4 | P:10-15 | 150 | — | 1 | — |
| Sheep | a sheep | Animal | Aggressor | Yes (11.1) | 11.1 | 1 | 12 | 1-2 | P:5-10 | 300 | — | 6 | — |
| Walrus | a walrus | Animal | Aggressor | Yes (35.1) | 35.1 | 1 | 14-17 | 4-10 | P:20-25, F:5-10, C:20-25, Po:5-10, E:5-10 | 150 | — | 1 | — |

PackHorses and PackLlamas are tameable at 11.1 with positive karma (200), designed as pack animals. The GiantToad uses `AI_Melee` combat AI. Gorillas and Mountain Goats are tamable at `0.0`.

---

## Reptiles

**9 creatures** — Cold-blooded predators, many with high elemental resistances. Most use `AI_Melee` combat AI. Several have poison immunity.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Alligator | an alligator | Melee | — | Yes (47.1) | 47.1 | 1 | 46-60 | 5-15 | P:25-35, F:5-10, Po:5-10 | 600 | -600 | 3 | — |
| GiantSerpent | a giant snake | Melee | — | — | — | — | 112-129 | 7-17 | P:30-35, F:5-10, C:10-20, Po:70-90, E:10-20 | 2500 | -2500 | 3 | PoisonImmune(Poison.Greater) |
| IceSerpent | a giant ice serpent | Melee | — | — | — | — | 130-147 | 7-17 | P:30-35, C:80-90, Po:15-25, E:10-20 | 3500 | -3500 | 3 | — |
| IceSnake | an ice snake | Melee | — | — | — | — | — | 4-12 | P:20-25, C:80-90, Po:60-70, E:30-40 | 900 | -900 | 3 | — |
| LavaLizard | a lava lizard | Melee | — | Yes (80.7) | 80.7 | 1 | 76-90 | 6-24 | P:35-45, F:30-45, Po:25-35, E:25-35 | 3000 | -3000 | 4 | — |
| LavaSerpent | a lava serpent | Melee | — | — | — | — | 232-249 | 10-22 | P:35-45, F:70-80, Po:30-40, E:10-20 | 4500 | -4500 | 4 | — |
| LavaSnake | a lava snake | Melee | — | — | — | — | 28-32 | 1-8 | P:20-25, F:30-40, Po:20-30, E:10-20 | 600 | -600 | 2 | — |
| SilverSerpent | a silver serpent | Melee | — | — | — | — | 97-216 | 5-21 | P:35-45, F:5-10, C:5-10, E:5-10 | 7000 | -7000 | 4 | PoisonImmune(Poison.Lethal) |
| Snake | a snake | Melee | — | Yes (59.1) | 59.1 | 1 | 15-19 | 1-4 | P:15-20, Po:20-30 | 300 | -300 | 1 | PoisonImmune(Poison.Lesser) |

Lava creatures have exceptional fire resistance (30-80). Ice serpents have high cold resistance (80-90). The SilverSerpent is immune to Lethal poison and has the highest fame (7000) among reptiles.

---

## Rodents

**4 creatures** — Small creatures, including tamable rabbits and poisonous rats.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| GiantRat | a giant rat | Melee | — | Yes (29.1) | 29.1 | 1 | 26-39 | 4-8 | P:15-20, F:5-10, Po:25-35 | 300 | -300 | 1 | — |
| JackRabbit | a jack rabbit | Animal | Aggressor | Yes (0.0) | — | 1 | 9 | 1-2 | P:2-5 | 150 | — | 4 | — |
| Rabbit | a rabbit | Animal | Aggressor | Yes (0.0) | — | 1 | 4-6 | 1 | P:5-10 | 150 | — | 6 | — |
| SewerRat | a sewer rat | Melee | — | Yes (0.0) | — | 1 | 6 | 1-2 | P:5-10, Po:15-25, E:5-10 | 300 | -300 | 6 | — |

GiantRats and SewerRats use `AI_Melee` and have poison damage. Rabbits and JackRabbits are tamable at `0.0` with very low stats.

---

## Slimes

**1 creature** — An easter egg creature.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Jwilson | a jwilson | Melee | — | — | — | — | — | — | — | — | — | 8 | — |

---

## Town Critters

**5 creatures** — Companion animals found in towns, all tamable at `0.0` with minimal stats.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Bird | a tropical bird | Animal | Aggressor | Yes (0.0) | — | 1 | — | — | — | 150 | — | — | — |
| Cat | a cat | Animal | Aggressor | Yes (0.0) | — | 1 | 6 | 1 | P:5-10 | — | 150 | 8 | — |
| Dog | a dog | Animal | Aggressor | Yes (0.0) | — | 1 | 17-22 | 4-7 | P:10-15 | — | 300 | 1 | — |
| Parrot | a parrot | Animal | Aggressor | Yes (0.0) | — | 1 | — | — | — | — | — | — | — |
| Rat | a rat | Animal | Aggressor | Yes (0.0) | — | 1 | 6 | 1-2 | P:5-10, Po:5-10 | 150 | -150 | 6 | — |

---

## Animal-Type Familiars

**5 creatures** — Animal familiars summoned by necromancy spells. These are not tamable and use `AI_Melee` combat AI. They follow their summoner and attack enemies.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| DarkWolfFamiliar | a dark wolf | Melee | — | — | — | — | — | — | — | — | — | — | — |
| DeathAdder | a death adder | Melee | — | — | — | — | — | — | — | — | — | — | — |
| HordeMinionFamiliar | a horde minion | Melee | — | — | — | — | — | — | — | — | — | — | — |
| ShadowWispFamiliar | a shadow wisp | Melee | — | — | — | — | — | — | — | — | — | — | — |
| VampireBatFamiliar | a vampire bat | Melee | — | — | — | — | — | — | — | — | — | — | — |

Familiars are summoned via necromancy spells and persist for a limited duration. They are defined in `Projects/UOContent/Mobiles/Familiars/`.

---

## See Also

- [Taming](skills/taming.md) — Taming skill mechanics and progression
- [Animal Lore](skills/animallore.md) — Animal Lore skill for identifying creatures
- [Combat](systems/combat.md) — Combat mechanics and mounted combat
- [Tools](items/tools.md) — Whips and other animal handling tools
- [Creature Reference](reference/creature-reference.md) — Complete creature listing
- [Creature AI Types](reference/creature-ai-types.md) — AI type behavior descriptions
