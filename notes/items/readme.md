# Items

Items are the fundamental objects that populate ModernUO's world. They can be worn, carried, used, equipped, traded, and destroyed. Every item has properties that define its behavior, appearance, and interactions with game systems.

## Item Categories

### [Weapons](weapons.md)
Melee and ranged weapons with quality systems (Low/Regular/Exceptional), slayer bonuses, weapon abilities, and durability mechanics. BaseWeapon supports AosAttributes (24), AosWeaponAttributes (25), AosElementAttributes (7), and 31 weapon abilities. Includes complete damage formulas for AOS and pre-AOS eras, slayer system with 30 entries across 7 groups, ranged weapon mechanics, and crafting integration.

### [Armor](armor.md)
Protective equipment providing Armor Rating (AR), resistance bonuses, and stat modifiers. 13 material types across 7 body types (Gorget, Gloves, Helmet, Arms, Legs, Chest, Shield). Quality system (Low/Regular/Exceptional), 6 durability levels, 6 protection levels, meditation allowance, stat requirements with LowerStatReq, AosAttributes (24), AosArmorAttributes (4), AosSkillBonuses, crafting integration with resource bonuses, damage absorption mechanics, gender restrictions, scissoring, and identification system.

### [Clothing](clothing.md)
Non-armor wearable items with durability, dyeability, scissoring, and optional stat bonuses. Multiple layer types across head, torso, legs, extremities, and accessories. Quality system (Low/Regular/Exceptional), durability via hit points, dye customization, 24 AosAttributes, 4 AosArmorAttributes, 7 AosElementAttributes, AosSkillBonuses, stat requirements with LowerStatReq, gender/race restrictions, arcane equipment charges, faction integration, and crafting with resource-based hue.

### [Jewels](jewels.md)
9 gem types (StarSapphire through Diamond) used as crafting components, trade resources, and jewelry enchantment. Jewelry implements `BaseJewel` with AosAttributes (24), AosElementAttributes (7), AosSkillBonuses, durability via hit points, stat modifiers on equip, and Tinkering crafting integration (54 recipes across 9 gem types × 6 jewelry pieces). 5 jewelry layers: Ring (0x08), Bracelet (0x0E), Earrings (0x12), Necklace (0x0A).

### [Food](food.md)
Consumable items that restore hunger and stamina via fill factor mechanics (stomach capacity 0-20). Features pre-cooked foods, raw ingredients with cooking system (heat sources, cooking levels), beverage/refill system with quantity tracking (mugs, pitchers, bottles), poison application on food, bowl system with container return, EndlessDecanter auto-refill mechanic, chocolate/candy toothache system, holiday foods, farming crop integration (wheat→flour mill), plant watering, and BAC (drunk) mechanics.

### [Tools](tools.md)
Crafting tools used across 11 skills (blacksmithing, carpentry, tailoring, tinkering, etc.). Two hierarchies: `BaseTool` (crafting) and `BaseHarvestTool` (mining, lumberjacking). Quality system (Low/Regular/Exceptional), uses remaining with scaling, runic tools with up to 32 random properties (weapon/armor/hat/jewel/spellbook slots), one-tool-at-a-time equipment restriction, maker's mark, and ICraftable integration.

### [Books](books.md)
Spellbooks (store spells), scrolls (consumable magic), and lore books (flavor/readable content).

### [Containers](containers.md)
Core storage engine with 20+ specialized container types. Class hierarchy: `Item → Container → BaseContainer → Backpack/BankBox/Bag/Chests/Furniture/etc.` Weight & capacity system (GlobalMaxWeight=400, GlobalMaxItems=125), hierarchical weight limits, totals tracking (Gold/Items/Weight). Security: ISecurable interface, SecureLevel (Owner/CoOwners/Friends/Anyone/Guild), house lockdown integration. Special types: TreasureMapChest (3-hour expiry, guardian system, artifacts), ParagonChest (5 levels with escalating loot), FillableContainer (dynamic respawning), MarkContainer (rune marking with auto-relock), SalvageBag (material conversion), Strongbox (co-owner decay), Bedroll (safe logout), Campfire (safety radius). TrappableContainer (TrapType: Magic/Explosion/Dart/Poison), LockableContainer with ILockpickable (lock levels, magic lock). Feature flag integration for access blocking.

## Key Systems

- **Layers**: Equipment slots (Head, Torso, Legs, Arms, etc.) that determine where items are worn
- **Weight**: All items have weight; exceeding capacity affects movement speed
- **Stacking**: Some items (resources, food, potions) stack in containers
- **LootType**: Controls whether items can be looted by other players (Regular, Blessed, Deed, etc.)
- **Quality**: Items can be Low, Regular, or Exceptional — affecting durability, bonuses, and value
- **Durability**: Items have hit points and degrade with use; repairable with appropriate tools
- **Attributes**: AosAttributes (stat bonuses, resistances, skill bonuses), AosWeaponAttributes (slayers, abilities)
- **Crafting Integration**: Many items are crafted through the 11 craft definitions with ECA modes
