# Feature Flags

> **See also:** [Configuration Reference](configuration.md) · [Configuration (JsonConfig)](configuration-json.md)

Feature flags provide runtime toggleable features with admin control. They are stored in `Configuration/FeatureFlags/` and managed via the `FeatureFlagManager`.

## How Feature Flags Work

Feature flags are defined as static boolean properties in the code. When changed via admin commands, the `FeatureFlagManager` persists the changes and syncs the values across the server.

**Source files:**
- Server flags: `Server/FeatureFlags.cs`
- Content flags: `UOContent/Engines/FeatureFlags/ContentFeatureFlags.cs`
- Settings: `UOContent/Engines/FeatureFlags/FeatureFlagSettings.cs`

## Server Feature Flags

**Source:** `Server/FeatureFlags.cs`

These flags control core server functionality hot paths.

| Flag | Default | Description |
|------|---------|-------------|
| `PlayerTrading` | `true` | Enable player-to-player trading |
| `PvPCombat` | `true` | Enable player-versus-player combat |
| `BankAccess` | `true` | Enable bank access |
| `SpeedhackDetection` | `false` | Enable speedhack detection |

## Content Feature Flags

**Source:** `UOContent/Engines/FeatureFlags/ContentFeatureFlags.cs`

These flags control content-specific features in hot paths.

| Flag | Default | Description |
|------|---------|-------------|
| `VendorPurchase` | `true` | Enable vendor purchasing |
| `VendorSell` | `true` | Enable vendor selling |
| `PlayerVendors` | `true` | Enable player vendors |
| `HousePlacement` | `true` | Enable house placement |
| `BoatPlacement` | `true` | Enable boat placement |
| `BulkOrders` | `true` | Enable bulk orders |
| `PassiveDetectHidden` | `true` | Enable passive detect hidden |

## Feature Flag Settings

**Source:** `UOContent/Engines/FeatureFlags/FeatureFlagSettings.cs`

Settings that control feature flag behavior.

| Setting | Default | Description |
|---------|---------|-------------|
| `RequiredAccessLevel` | `Administrator` | Minimum access level to manage feature flags |
| `LogChanges` | `true` | Log feature flag changes |
| `BroadcastChangesToStaff` | `true` | Broadcast changes to all staff members |
| `SavePath` | `Configuration/FeatureFlags` | Directory for feature flag data files |

## Feature Flag Block Entries

Feature flags can also block specific types of content (gumps, items, skills, spells, etc.). Each block entry includes:

| Property | Type | Description |
|----------|------|-------------|
| `ResolvedType` | Type | The type being blocked |
| `Reason` | string | Reason for the block |
| `Active` | bool | Whether the block is active |
| `CreatedAt` | DateTime | When the block was created |
| `CreatedBy` | string | Who created the block |

### Item Block Entries

Additional properties for item blocks:

| Property | Type | Description |
|----------|------|-------------|
| `BlockUse` | bool | Block item use |
| `BlockEquip` | bool | Block item equipping |
| `BlockContainerAccess` | bool | Block container access |

## Default Blocked Messages

| Message Type | Default |
|--------------|---------|
| Gump blocked | `"This feature is temporarily disabled."` |
| Item use blocked | `"This item cannot be used at this time."` |
| Item equip blocked | `"This item cannot be equipped at this time."` |
| Container blocked | `"This container cannot be opened at this time."` |
| Skill disabled | `"This skill is temporarily disabled."` |
| Spell disabled | `"This spell is temporarily disabled."` |
