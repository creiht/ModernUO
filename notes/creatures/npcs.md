# NPCs

NPCs are non-hostile characters that serve essential gameplay functions across all maps of Britannia. There are **360+ NPC types** organized into vendors, townfolk, guards, healers, hireable companions, and familiars. These characters form the backbone of the game's economy, quest system, and progression.

NPCs are defined in `Projects/UOContent/Mobiles/` subdirectories (`Vendors/`, `Townfolk/`, `Guards/`, `Healers/`, `Hireables/`, `Familiars/`) and inherit from `BaseCreature` or `BaseVendor`. Their behavior is controlled by `AIType.Vendor` (shopkeepers), `AIType.Melee` (guards, hireables), `AIType.Animal` (townfolk performers), and `AIType.Mage` (healers).

---

## Key Concepts

- **BaseVendor** — All shopkeepers inherit from `BaseVendor` (extends `BaseCreature`). They use `AI_Vendor` AI type and `FightMode.None`. Vendors are invulnerable by configuration and have two hidden backpacks for buy stock and resale items.
- **SBInfo System** — Each vendor type has an `SBInfo` subclass that defines what it sells (`BuyInfo`) and buys (`SellInfo`). There are 61+ SBInfo files covering every profession. A single vendor can aggregate multiple SBInfo objects (e.g., a Tokuno vendor also sells SE expansion items).
- **GenericBuy/GenericSell** — `GenericBuyInfo` defines sellable items with price, stock quantity, display item ID, and hue. `GenericSellInfo` defines buyable items at ~52% of the sell price (buy price = sell price × 1.9). Price scalars default to `100 + Town.Tax` for regional price variation.
- **VendorAI** — Listens for speech keywords `*buy*`, `*sell*`, `*vendor buy*`, `*vendor sell*` to trigger shop interfaces. Vendors are immune to bard music.
- **Player Vendors** — `PlayerVendor`, `RentedVendor`, and `PlayerBarkeeper` allow players to run their own shops with custom inventory.
- **BaseGuard Orders** — Tamed guards recognize 12 order types: Come, Drop, Follow, Friend, Guard, Attack, Patrol, Release, Stay, Stop, Transfer, Rename. Guards spawn in clusters within 15 tiles of a crime target and teleport using particle effects.
- **Hireables** — Temporary combat companions hired with gold payment. Cost is calculated from the sum of combat/healing skills divided by 35, plus 1. They use 2 control slots, pay every 30 minutes, and walk away if unpaid.
- **Familiars** — Summoned via Necromancy spells. They follow their master within a home range, hide when the master hides, match the master's warmode, and auto-release when the master dies.
- **Evil Healers** — Hostile variants (`EvilHealer`, `EvilWanderingHealer`) that use Mage AI and Aggressor fight mode, part of the Ethics system.

---

## Townfolk

**19 creatures** — Narrative characters with roles in towns and quests. Most have Melee AI, Aggressor fight mode, 200 fame, and 4000 karma. Performers (Gypsy, Actor, Artist, Sculptor, HarborMaster) use Animal AI with None fight mode, making them passive.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Actor | Actor | Animal | None | — | — | — | — | — | — | — | — | — | — |
| Artist | Artist | Animal | None | — | — | — | — | — | — | — | — | — | — |
| BaseEscortable | BaseEscortable | Melee | Aggressor | — | — | — | — | — | — | 200 | 4000 | — | — |
| BrideGroom | BrideGroom | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| EscortableMage | EscortableMage | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Gypsy | Gypsy | Animal | None | — | — | — | — | — | — | — | — | — | — |
| HarborMaster | HarborMaster | Animal | None | — | — | — | — | — | — | — | — | — | — |
| Merchant | Merchant | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Messenger | Messenger | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Minter | Minter | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Ninja | Ninja | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Noble | Noble | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Peasant | Peasant | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Prisoner | Prisoner | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Samurai | Samurai | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| Sculptor | Sculptor | Animal | None | — | — | — | — | — | — | — | — | — | — |
| SeekerOfAdventure | SeekerOfAdventure | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| TownCrier | TownCrier | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |

The Actor, Artist, Sculptor, Gypsy, and HarborMaster use `AI_Animal` with `FightMode.None`, reflecting their non-combatant roles as entertainers and observers. The BaseEscortable defines the template for quest-related escort NPCs with 200 fame and 4000 karma. TownCriers serve as the central announcement system for server events.

---

## Vendors

**55 creatures** — Shopkeepers by profession, all using `AI_Vendor` and `FightMode.None`. They are the primary interface for the game's economy, selling crafting supplies, weapons, armor, food, potions, and specialized goods.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| Alchemist | the alchemist | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| AnimalTrainer | the animal trainer | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Architect | the architect | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Armorer | the armorer | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Baker | the baker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Banker | the banker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Bard | the bard | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Barkeeper | the barkeeper | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Beekeeper | the beekeeper | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Blacksmith | the blacksmith | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Bowyer | the bowyer | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Butcher | the butcher | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Carpenter | the carpenter | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Cobbler | the cobbler | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Cook | the cook | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| CustomHairstylist | the hairstylist | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Farmer | the farmer | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Fisherman | the fisher | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Furtrader | the furtrader | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Glassblower | the alchemist | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| GolemCrafter | the golem crafter | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| GypsyMaiden | the gypsy maiden | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| HairStylist | the hair stylist | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Herbalist | the herbalist | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| HolyMage | the Holy Mage | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| InnKeeper | the innkeeper | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| IronWorker | the iron worker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Jeweler | the jeweler | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| KeeperOfChivalry | the Keeper of Chivalry | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| LeatherWorker | the leather worker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Mage | the mage | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Mapmaker | the mapmaker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Miller | the miller | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Miner | the miner | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Monk | the Monk | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| PlayerBarkeeper | the barkeeper | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Provisioner | the provisioner | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Rancher | the rancher | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Ranger | the ranger | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| RealEstateBroker | the real estate broker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Scribe | the scribe | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Shipwright | the shipwright | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| StoneCrafter | the stone crafter | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Tailor | the tailor | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Tanner | the tanner | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| TavernKeeper | the tavern keeper | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Thief | the thief | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Tinker | the tinker | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Vagabond | the vagabond | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| VarietyDealer | the variety dealer | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Veterinarian | the vet | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Waiter | the waiter | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Weaponsmith | the weaponsmith | Vendor | None | — | — | — | — | — | — | — | — | — | — |
| Weaver | the weaver | Vendor | None | — | — | — | — | — | — | — | — | — | — |

Vendors use the `SBInfo` system for shop definitions. Each profession has a dedicated SBInfo file (e.g., `SBBlacksmith.cs`, `SBHealer.cs`) in `Projects/UOContent/Mobiles/Vendors/SBInfo/`. Armor and weapon vendors are further split into 8 armor types (Chainmail, Helmet, Leather, Metal Shields, Plate, Ringmail, Studded, Wooden Shields) and 8 weapon types (Axe, Knife, Mace, Polearm, Ranged, Spear/Fork, Staves, Sword). Tokuno map vendors additionally sell SE expansion items via `SBSEArmor` and `SBSEWeapons`. Vendors auto-restock every hour (`RestockDelay` defaults to 1 hour). The Banker NPC extends BaseVendor and provides the banking system.

---

## Guards

**3 creatures** — City protection system. Guards spawn when crimes are committed, pursue offenders, and use non-lethal force (ArcherGuard shoots arrows; WarriorGuard uses melee). When a guard dies, three additional guards spawn to replace it.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| ArcherGuard | the guard | Melee | — | — | — | — | 100 | 125 | — | — | — | — | — | Mounted archer with 120 Archery, uses bow and arrow |
| BaseGuard | the guard | Melee | — | — | — | — | — | — | — | — | — | — | — | Abstract base class for guard behavior |
| WarriorGuard | the guard | Melee | — | — | — | — | — | — | — | — | — | — | — | Melee guard with studded armor |

Guards inherit from `Mobile` directly (not `BaseCreature`). ArcherGuard rides a Horse, wears Studded armor, and uses a Bow with 250 arrows. All guards have 120 Anatomy, 120 Tactics, and 120 MagicResist. The `GuardsInstantKill` configuration setting controls whether guards can deliver lethal blows. Guards teleport to targets using particle effects (0x3728) when out of range.

---

## Healers

**7 creatures** — Healing services for players. Standard healers (`Healer`, `WanderingHealer`, `PricedHealer`) extend `BaseVendor` with Mage AI and Aggressor fight mode when not invulnerable. They have high stats (Str 304-400, Dex 102-150, Int 204-300), all 40-50 resistances, and skills in Anatomy (75-97.5), EvalInt (82-100), Healing (75-97.5), Magery (82-100), MagicResist (82-100), and Tactics (82-100).

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BaseHealer | BaseHealer | Mage | Aggressor | — | — | — | — | 10-23 | P:40-50, F:40-50, C:40-50, Po:40-50, E:40-50 | 1000 | 10000 | — | Wears yellow robe, offers healing/resurrection |
| EvilHealer | EvilHealer | Mage | — | — | — | — | — | — | — | — | — | — | Hostile variant (Ethics system) |
| EvilWanderingHealer | EvilWanderingHealer | Mage | — | — | — | — | — | — | — | — | — | — | Hostile wandering variant |
| FortuneTeller | FortuneTeller | — | — | — | — | — | — | — | — | — | — | — | Offers fortune services |
| Healer | Healer | Mage | Aggressor | — | — | — | — | — | — | — | — | — | Fixed-location healer |
| PricedHealer | PricedHealer | Mage | Aggressor | — | — | — | — | — | — | — | — | — | Charges gold for resurrection |
| WanderingHealer | WanderingHealer | Mage | Aggressor | — | — | — | — | — | — | — | — | — | Appears randomly in towns |

Standard healers offer free healing to young players (when hits < hits max) and resurrection services. `WanderingHealer` NPCs appear randomly in towns. `FortuneTeller` provides fortune-telling services. Healers carry bandages (5-10), heal potions, and cure potions. The `_price` field on BaseHealer controls resurrection cost (0 = free). EvilHealer variants are hostile and part of the Ethics system.

---

## Hireables

**11 creatures** — Temporary combat companions hired with gold. All use 2 control slots and follow the Aggressor fight mode. Payment is calculated from total combat skills and deducted every 30 minutes; unpaid hires walk away.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BaseHire | BaseHire | Melee | Aggressor | — | — | 2 | — | — | — | — | — | — | — | Abstract base class |
| HireBard | HireBard | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HireBardArcher | HireBardArcher | Archer | — | — | — | — | — | 5-10 | — | 100 | 100 | — | — | Ranged bard |
| HireBeggar | HireBeggar | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HireFighter | HireFighter | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HireMage | HireMage | Mage | — | — | — | — | — | 10-23 | — | 100 | 100 | — | — | Spellcasting hire |
| HirePaladin | HirePaladin | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HirePeasant | HirePeasant | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HireRanger | HireRanger | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HireRangerArcher | HireRangerArcher | Archer | — | — | — | — | — | 13-24 | — | 100 | 125 | — | — | Ranged ranger |
| HireSailor | HireSailor | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |
| HireThief | HireThief | Melee | Aggressor | — | — | — | — | — | — | — | — | — | — |

Hireables are hired by giving them gold (drag and drop or context menu). The daily cost is `(Anatomy + Tactics + Macing + Swords + Fencing + Archery + MagicResist + Healing + Magery + Parry) / 35 + 1`. They keep items on death and store up to 8 gold upon hiring. Hires last until they die or stop being paid.

---

## Familiars

**6 creatures** — Summoned companions from Necromancy spells. They use `AI_Melee`, follow their master within `RangeHome` tiles, hide when the master hides, and auto-release (dropping inventory) when the master dies.

| Creature | Default Name | AI | Fight | Tamable | Min Tame | Slots | HP | Damage | Resistances | Fame | Karma | VA | Special |
|----------|-------------|-----|-------|---------|----------|-------|-----|--------|-------------|------|-------|-----|---------|
| BaseFamiliar | BaseFamiliar | Melee | — | — | — | — | — | — | — | — | — | — | — | Abstract base class |
| DarkWolf | a dark wolf | Melee | — | — | — | — | — | — | — | — | — | — | — |
| DeathAdder | a death adder | Melee | — | — | — | — | — | — | — | — | — | — | — |
| HordeMinion | a horde minion | Melee | — | — | — | — | — | — | — | — | — | — | — |
| ShadowWisp | a shadow wisp | Melee | — | — | — | — | — | — | — | — | — | — | — |
| VampireBat | a vampire bat | Melee | — | — | — | — | — | — | — | — | — | — | — |

Familiars have `PoisonImmune(Lethal)`, `BardImmune`, `Commandable = false`, `IgnoreMobiles = true`, and slow speed (0.1). They cannot be released to the wild — the master must use the Release context menu entry to dismiss them. Familiars match their master's warmode and combatant when within 5 tiles.

---

## See Also

- [Creature Reference](reference/creature-reference.md) — Complete creature listing
- [Taming](skills/taming.md) — Taming and pet control mechanics
- [Combat](systems/combat.md) — Combat systems
- [Crafting](systems/crafting.md) — Crafting professions and vendor relationships
- [Necromancy](spells/necromancy.md) — Familiar summoning spells
- [Ethics](systems/ethics.md) — Evil Healer alignment system
