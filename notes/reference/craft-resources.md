# Craft Resources

Complete reference of all **26 craft resources** in ModernUO, grouped by type.

**Source:** `Projects/UOContent/Misc/ResourceInfo.cs`

---

## Overview

| # | Resource | Type | Hue | Loc # |
|---|----------|------|-----|-------|
| 1 | Iron | Metal | 0x000 | 1053109 |
| 2 | Dull Copper | Metal | 0x973 | 1053108 |
| 3 | Shadow Iron | Metal | 0x966 | 1053107 |
| 4 | Copper | Metal | 0x96D | 1053106 |
| 5 | Bronze | Metal | 0x972 | 1053105 |
| 6 | Gold | Metal | 0x8A5 | 1053104 |
| 7 | Agapite | Metal | 0x979 | 1053103 |
| 8 | Verite | Metal | 0x89F | 1053102 |
| 9 | Valorite | Metal | 0x8AB | 1053101 |
| 10 | Normal | Leather | 0x000 | 1049353 |
| 11 | Spined | Leather | 0x283 (pre-AOS) / 0x8AC (AOS+) | 1049354 |
| 12 | Horned | Leather | 0x227 (pre-AOS) / 0x845 (AOS+) | 1049355 |
| 13 | Barbed | Leather | 0x1C1 (pre-AOS) / 0x851 (AOS+) | 1049356 |
| 14 | Red Scales | Scales | 0x66D | 1053129 |
| 15 | Yellow Scales | Scales | 0x8A8 | 1053130 |
| 16 | Black Scales | Scales | 0x455 | 1053131 |
| 17 | Green Scales | Scales | 0x851 | 1053132 |
| 18 | White Scales | Scales | 0x8FD | 1053133 |
| 19 | Blue Scales | Scales | 0x8B0 | 1053134 |
| 20 | Normal | Wood | 0x000 | 1011542 |
| 21 | Oak | Wood | 0x7DA | 1072533 |
| 22 | Ash | Wood | 0x4A7 | 1072534 |
| 23 | Yew | Wood | 0x4A8 | 1072535 |
| 24 | Heartwood | Wood | 0x4A9 | 1072536 |
| 25 | Bloodwood | Wood | 0x4AA | 1072538 |
| 26 | Frostwood | Wood | 0x47F | 1072539 |

---

## Metals (9)

Runic Min/Max Intensity values differ between ML (Mondain's Legacy) and pre-ML. ML values shown first, pre-ML in parentheses.

| Resource | Hue | Loc # | Armor Phys | Armor Fire | Armor Cold | Armor Poison | Armor Energy | Armor Durability | Armor Luck | Armor Lower Req | Weapon Fire Dmg | Weapon Cold Dmg | Weapon Poison Dmg | Weapon Energy Dmg | Weapon Durability | Weapon Luck | Weapon Lower Req | Runic Min Attr | Runic Max Attr | Runic Min Int (ML) | Runic Max Int (ML) | Runic Min Int (pre-ML) | Runic Max Int (pre-ML) |
|----------|-----|-------|------------|------------|------------|--------------|--------------|------------------|------------|-----------------|-----------------|-----------------|-------------------|-------------------|-------------------|-------------|------------------|------------------|------------------|--------------------|--------------------|----------------------|----------------------|
| Iron | 0x000 | 1053109 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| Dull Copper | 0x973 | 1053108 | 6 | 0 | 0 | 0 | 0 | 50 | 0 | 20 | 0 | 0 | 0 | 0 | 100 | 0 | 50 | 1 | 2 | 40 | 100 | 10 | 35 |
| Shadow Iron | 0x966 | 1053107 | 2 | 1 | 0 | 0 | 5 | 100 | 0 | 0 | 0 | 20 | 0 | 0 | 50 | 0 | 0 | 2 | 2 | 45 | 100 | 20 | 45 |
| Copper | 0x96D | 1053106 | 1 | 1 | 0 | 5 | 2 | 0 | 0 | 0 | 0 | 0 | 10 | 20 | 0 | 0 | 0 | 2 | 3 | 50 | 100 | 25 | 50 |
| Bronze | 0x972 | 1053105 | 3 | 0 | 5 | 1 | 1 | 0 | 0 | 0 | 40 | 0 | 0 | 0 | 0 | 0 | 0 | 3 | 3 | 55 | 100 | 30 | 65 |
| Gold | 0x8A5 | 1053104 | 1 | 1 | 2 | 0 | 2 | 0 | 40 | 30 | 0 | 0 | 0 | 0 | 0 | 40 | 50 | 3 | 4 | 60 | 100 | 35 | 75 |
| Agapite | 0x979 | 1053103 | 2 | 3 | 2 | 2 | 2 | 0 | 0 | 0 | 0 | 30 | 0 | 20 | 0 | 0 | 0 | 4 | 4 | 65 | 100 | 40 | 80 |
| Verite | 0x89F | 1053102 | 3 | 3 | 2 | 3 | 1 | 0 | 0 | 0 | 0 | 0 | 40 | 20 | 0 | 0 | 0 | 4 | 5 | 70 | 100 | 45 | 90 |
| Valorite | 0x8AB | 1053101 | 4 | 0 | 3 | 3 | 3 | 50 | 0 | 0 | 10 | 20 | 10 | 20 | 0 | 0 | 0 | 5 | 5 | 85 | 100 | 50 | 100 |

---

## Leather (4)

Hue values differ between pre-AOS and AOS+ eras. Spined, Horned, and Barbed have different hues depending on `Core.AOS`.

| Resource | Hue (pre-AOS) | Hue (AOS+) | Loc # | Armor Phys | Armor Fire | Armor Cold | Armor Poison | Armor Energy | Armor Durability | Armor Luck | Armor Lower Req | Weapon Fire Dmg | Weapon Cold Dmg | Weapon Poison Dmg | Weapon Energy Dmg | Weapon Durability | Weapon Luck | Weapon Lower Req | Runic Min Attr | Runic Max Attr | Runic Min Int (ML) | Runic Max Int (ML) | Runic Min Int (pre-ML) | Runic Max Int (pre-ML) |
|----------|---------------|------------|-------|------------|------------|------------|--------------|--------------|------------------|------------|-----------------|-----------------|-----------------|-------------------|-------------------|-------------------|-------------|------------------|------------------|------------------|--------------------|--------------------|----------------------|----------------------|
| Normal | 0x000 | 0x000 | 1049353 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| Spined | 0x283 | 0x8AC | 1049354 | 5 | 0 | 0 | 0 | 0 | 0 | 40 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 3 | 40 | 100 | 20 | 40 |
| Horned | 0x227 | 0x845 | 1049355 | 2 | 3 | 2 | 2 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3 | 4 | 45 | 100 | 30 | 70 |
| Barbed | 0x1C1 | 0x851 | 1049356 | 2 | 1 | 2 | 3 | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4 | 5 | 50 | 100 | 40 | 100 |

---

## Scales (6)

Scales have mixed positive and negative resist modifiers, trading one resistance for another.

| Resource | Hue | Loc # | Armor Phys | Armor Fire | Armor Cold | Armor Poison | Armor Energy | Armor Durability | Armor Luck | Armor Lower Req | Weapon Fire Dmg | Weapon Cold Dmg | Weapon Poison Dmg | Weapon Energy Dmg | Weapon Durability | Weapon Luck | Weapon Lower Req | Runic Min Attr | Runic Max Attr | Runic Min Int (ML) | Runic Max Int (ML) | Runic Min Int (pre-ML) | Runic Max Int (pre-ML) |
|----------|-----|-------|------------|------------|------------|--------------|--------------|------------------|------------|-----------------|-----------------|-----------------|-------------------|-------------------|-------------------|-------------|------------------|------------------|------------------|--------------------|--------------------|----------------------|----------------------|
| Red Scales | 0x66D | 1053129 | 0 | 10 | -3 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| Yellow Scales | 0x8A8 | 1053130 | -3 | 0 | 0 | 0 | 0 | 0 | 20 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| Black Scales | 0x455 | 1053131 | 10 | 0 | 0 | 0 | -3 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| Green Scales | 0x851 | 1053132 | 0 | -3 | 0 | 10 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| White Scales | 0x8FD | 1053133 | -3 | 0 | 10 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |
| Blue Scales | 0x8B0 | 1053134 | 0 | 0 | 0 | -3 | 10 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | — | — | — | — |

---

## Wood (7)

All wood types use `CraftAttributeInfo.Blank` — no attribute bonuses are defined for any wood resource.

| Resource | Hue | Loc # | Armor Phys | Armor Fire | Armor Cold | Armor Poison | Armor Energy | Armor Durability | Armor Luck | Armor Lower Req | Weapon Fire Dmg | Weapon Cold Dmg | Weapon Poison Dmg | Weapon Energy Dmg | Weapon Durability | Weapon Luck | Weapon Lower Req | Runic Min Attr | Runic Max Attr | Runic Min Int | Runic Max Int | Runic Min Int (pre-ML) | Runic Max Int (pre-ML) |
|----------|-----|-------|------------|------------|------------|--------------|--------------|------------------|------------|-----------------|-----------------|-----------------|-------------------|-------------------|-------------------|-------------|------------------|------------------|------------------|---------------|---------------|----------------------|----------------------|
| Normal | 0x000 | 1011542 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Oak | 0x7DA | 1072533 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Ash | 0x4A7 | 1072534 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Yew | 0x4A8 | 1072535 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Heartwood | 0x4A9 | 1072536 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Bloodwood | 0x4AA | 1072538 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Frostwood | 0x47F | 1072539 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |

---

## Notes

- **`Core.ML`** — Runic Min/Max Intensity values differ between ML (Mondain's Legacy) and pre-ML eras. All metals and leathers use ML values when `Core.ML` is true.
- **`Core.AOS`** — Leather hue values for Spined, Horned, and Barbed differ between pre-AOS and AOS+ eras. The table shows both.
- **Blank resources** — Iron, Normal Leather, Normal Wood, and all 6 specialty woods have no attribute bonuses (`CraftAttributeInfo.Blank`).
- **Scale resist tradeoffs** — Each scale type provides +10 to one resistance and -3 to another (e.g., Red Scales: +10 Fire, -3 Cold).
- **Metal progression** — Metals form a linear progression from Iron (no bonuses) to Valorite (highest runic attributes and balanced resistances).
