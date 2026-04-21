# Server Configuration Reference

Complete reference of all server configuration settings in ModernUO.

> **See also:** [Configuration (JsonConfig)](configuration-json.md) · [Feature Flags](feature-flags.md)

Configuration is stored in `Configuration/modernuo.json` relative to the server base directory.

ModernUO uses three configuration systems:

| System | Location | Purpose |
|--------|----------|---------|
| **ServerConfiguration** | `Configuration/modernuo.json` | Runtime-tunable settings via `ServerConfiguration.Get*()` |
| **JsonConfig** | `Configuration/*.json` | Complex data structures (email, assistants) |
| **Feature Flags** | `Configuration/FeatureFlags/*.json` | Runtime toggleable features with admin control |
| **Static Fields** | Source code | Hardcoded defaults (not user-editable) |

## How Configuration Works

ModernUO uses `ServerConfiguration.GetSetting<T>(key, defaultValue)` and `ServerConfiguration.GetOrUpdateSetting<T>(key, defaultValue)` to read settings.

| Method | Behavior |
|--------|----------|
| `GetSetting` | Returns stored value or default. Never writes to config file. |
| `GetOrUpdateSetting` | Returns stored value. If key doesn't exist, writes default to config file first. |

Settings are loaded from `Configuration/modernuo.json`. If a setting key is missing from the file, the default value is used.

## Dynamic Defaults

Some settings use runtime expressions as defaults, evaluated at startup:

| Expression | Meaning |
|------------|---------|
| `Core.AOS` | True if Age of Shadows expansion is active |
| `Core.LBR` | True if Lord Britannus Renaissance expansion is active |
| `Core.ML` | True if Mondain's Legacy expansion is active |
| `Core.SA` | True if Sacrament expansion is active |
| `Core.TOL` | True if Third Age of Britannia expansion is active |
| `Core.UOR` | True if Ultima Renaissance expansion is active |

## How Configuration Works

ModernUO uses `ServerConfiguration.GetSetting<T>(key, defaultValue)` and `ServerConfiguration.GetOrUpdateSetting<T>(key, defaultValue)` to read settings.

| Method | Behavior |
|--------|----------|
| `GetSetting` | Returns stored value or default. Never writes to config file. |
| `GetOrUpdateSetting` | Returns stored value. If key doesn't exist, writes default to config file first. |

Settings are loaded from `Configuration/modernuo.json`. If a setting key is missing from the file, the default value is used.

## Dynamic Defaults

Some settings use runtime expressions as defaults, evaluated at startup:

| Expression | Meaning |
|------------|---------|
| `Core.AOS` | True if Age of Shadows expansion is active |
| `Core.LBR` | True if Lord Britannus Renaissance expansion is active |
| `Core.ML` | True if Mondain's Legacy expansion is active |
| `Core.SA` | True if Sacrament expansion is active |
| `Core.TOL` | True if Third Age of Britannia expansion is active |
| `Core.UOR` | True if Ultima Renaissance expansion is active |

---

## Summary by System

| System | Settings |
|--------|----------|
| `accountGold` | 3 |
| `accountHandler` | 3 |
| `accountSecurity` | 1 |
| `actionDelay` | 1 |
| `asciiClickMessage` | 1 |
| `assistants` | 1 |
| `autoArchive` | 13 |
| `autosave` | 3 |
| `buffIcons` | 1 |
| `bulletinboards` | 3 |
| `chat` | 1 |
| `clientData` | 1 |
| `clientVerification` | 8 |
| `commandsystem` | 1 |
| `crashGuard` | 4 |
| `decay` | 4 |
| `ethics` | 1 |
| `expansion` | 1 |
| `factions` | 1 |
| `guards` | 1 |
| `guildClickMessage` | 1 |
| `houseDecay` | 1 |
| `insurance` | 1 |
| `maps` | 4 |
| `melee` | 1 |
| `movement` | 7 |
| `movementThrottle` | 7 |
| `murderSystem` | 5 |
| `netstate` | 1 |
| `network` | 2 |
| `opl` | 2 |
| `pages` | 1 |
| `pathfinding` | 1 |
| `pingServer` | 3 |
| `profanityProtection` | 2 |
| `questSystem` | 1 |
| `serverListing` | 3 |
| `spellCasting` | 1 |
| `stamina` | 9 |
| `stats` | 6 |
| `stealing` | 4 |
| `system` | 1 |
| `taming` | 1 |
| `testCenter` | 1 |
| `timer` | 2 |
| `uogateway` | 1 |
| `vendor` | 1 |
| `vetRewards` | 3 |
| `virtualChecks` | 1 |
| `visibleDamage` | 1 |
| `world` | 7 |

**Total:** 135 settings across 51 systems

---

## accountGold

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `accountGold.convertOnBank` | bool | `true` | GetSetting | Convert on bank | Projects/Server/IAccount.cs |
| `accountGold.convertOnTrade` | bool | `false` | GetSetting | Convert on trade | Projects/Server/IAccount.cs |
| `accountGold.enable` | bool (Core.X) | `Core.TOL` | GetSetting | Enable | Projects/Server/IAccount.cs |

## accountHandler

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `accountHandler.enableAutoAccountCreation` | bool | `true` | GetOrUpdateSetting | Enable auto account creation | Projects/UOContent/Accounting/AccountHandler.cs |
| `accountHandler.enablePlayerPasswordCommand` | bool | `false` | GetOrUpdateSetting | Enable player password command | Projects/UOContent/Accounting/AccountHandler.cs |
| `accountHandler.maxAccountsPerIP` | int | `1` | GetOrUpdateSetting | Max accounts per ip | Projects/UOContent/Accounting/AccountHandler.cs |

## accountSecurity

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `accountSecurity.encryptionAlgorithm` | PasswordProtectionAlgorithm | `PasswordProtectionAlgorithm.Argon2` | GetOrUpdateSetting | Encryption algorithm | Projects/UOContent/Accounting/Security/AccountSecurity.cs |

## actionDelay

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `actionDelay` | int | `Core.AOS ? 1000 : 500` | GetSetting | Action delay | Projects/UOContent/Configuration/ExpansionConfiguration.cs |

## asciiClickMessage

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `asciiClickMessage` | bool (Core.X) | `!Core.AOS` | GetSetting | Ascii click message | Projects/UOContent/Configuration/ExpansionConfiguration.cs |

## assistants

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `assistants.enableNegotiation` | bool | `false` | GetOrUpdateSetting | Enable negotiation | Projects/UOContent/Assistants/AssistantHandler.cs |

## autoArchive

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `autoArchive.archiveLocally` | bool | `true` | GetOrUpdateSetting | Archive locally | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.archivePath` | string | `"Archives"` | GetOrUpdateSetting | Archive path | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.backupMaxAge` | int | `30` | GetOrUpdateSetting | Backup max age | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.backupPath` | string | `"Backups"` | GetOrUpdateSetting | Backup path | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.compressionLevel` | int | `3` | GetOrUpdateSetting | Compression level | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.dailyRetention` | int | `30` | GetOrUpdateSetting | Daily retention | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.enableArchivePruning` | bool | `true` | GetOrUpdateSetting | Enable archive pruning | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.hourlyRetention` | int | `24` | GetOrUpdateSetting | Hourly retention | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.monthlyRetention` | int | `12` | GetOrUpdateSetting | Monthly retention | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.retryCount` | int | `3` | GetOrUpdateSetting | Retry count | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.retryDelayMs` | int | `500` | GetOrUpdateSetting | Retry delay ms | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.tempArchivePath` | string | `"temp"` | GetSetting | Temp archive path | Projects/UOContent/World Saves/AutoArchive.cs |
| `autoArchive.verifyArchives` | bool | `true` | GetOrUpdateSetting | Verify archives | Projects/UOContent/World Saves/AutoArchive.cs |

## autosave

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `autosave.enabled` | bool | `true` | GetOrUpdateSetting | Enabled | Projects/UOContent/World Saves/AutoSave.cs |
| `autosave.saveDelay` | TimeSpan | `TimeSpan.FromMinutes(5.0)` | GetOrUpdateSetting | Save delay | Projects/UOContent/World Saves/AutoSave.cs |
| `autosave.warningDelay` | TimeSpan | `TimeSpan.Zero` | GetOrUpdateSetting | Warning delay | Projects/UOContent/World Saves/AutoSave.cs |

## buffIcons

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `buffIcons.enable` | bool (Core.X) | `Core.ML` | GetOrUpdateSetting | Enable | Projects/UOContent/Engines/BuffIcons/BuffInfo.cs |

## bulletinboards

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `bulletinboards.creationTimeDelay` | TimeSpan | `TimeSpan.FromMinutes(2.0)` | GetOrUpdateSetting | Creation time delay | Projects/UOContent/Items/Bulletin Boards/BulletinBoard.cs |
| `bulletinboards.expireDuration` | TimeSpan | `TimeSpan.FromHours(6.0)` | GetOrUpdateSetting | Expire duration | Projects/UOContent/Items/Bulletin Boards/BulletinBoard.cs |
| `bulletinboards.replyDelay` | TimeSpan | `TimeSpan.FromSeconds(30.0)` | GetOrUpdateSetting | Reply delay | Projects/UOContent/Items/Bulletin Boards/BulletinBoard.cs |

## chat

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `chat.enabled` | bool | `false` | GetOrUpdateSetting | Enabled | Projects/UOContent/Engines/Chat/Chat.cs |

## clientData

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `clientData.clientVersion` | dynamic | `(ClientVersion)null` | GetSetting | Client version | Projects/Server/Client/UOClient.cs |

## clientVerification

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `clientVerification.ageLeniency` | TimeSpan | `TimeSpan.FromDays(10)` | GetOrUpdateSetting | Age leniency | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.allowedClientTypes` | ClientType | `ClientType.Classic \| ClientType.SA` | GetSetting | Allowed client types | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.enable` | bool | `true` | GetOrUpdateSetting | Enable | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.gameTimeLeniency` | TimeSpan | `TimeSpan.FromHours(25)` | GetOrUpdateSetting | Game time leniency | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.invalidClientResponse` | InvalidClientResponse | `InvalidClientResponse.Kick` | GetOrUpdateSetting | Invalid client response | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.kickDelay` | TimeSpan | `TimeSpan.FromSeconds(20.0)` | GetOrUpdateSetting | Kick delay | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.maxRequired` | dynamic | `(ClientVersion)null` | GetSetting | Max required | Projects/UOContent/Misc/ClientVerification.cs |
| `clientVerification.minRequired` | dynamic | `(ClientVersion)null` | GetSetting | Min required | Projects/UOContent/Misc/ClientVerification.cs |

## commandsystem

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `commandsystem.prefix` | string | `"["` | GetOrUpdateSetting | Prefix | Projects/UOContent/Commands/Handlers.cs |

## crashGuard

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `crashGuard.enabled` | bool | `true` | GetOrUpdateSetting | Enabled | Projects/UOContent/Misc/CrashGuard.cs |
| `crashGuard.generateReport` | bool | `true` | GetOrUpdateSetting | Generate report | Projects/UOContent/Misc/CrashGuard.cs |
| `crashGuard.restartServer` | bool | `true` | GetOrUpdateSetting | Restart server | Projects/UOContent/Misc/CrashGuard.cs |
| `crashGuard.saveBackup` | bool | `true` | GetOrUpdateSetting | Save backup | Projects/UOContent/Misc/CrashGuard.cs |

## decay

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `decay.bucketInterval` | TimeSpan | `TimeSpan.FromMinutes(5)` | GetSetting | Bucket interval | Projects/Server/Items/DecayScheduler.cs |
| `decay.jitterMaxMs` | int | `25` | GetSetting | Jitter max ms | Projects/Server/Items/DecayScheduler.cs |
| `decay.maxItemsPerTick` | int | `250` | GetSetting | Max items per tick | Projects/Server/Items/DecayScheduler.cs |
| `decay.tickInterval` | TimeSpan | `TimeSpan.FromMilliseconds(256)` | GetSetting | Tick interval | Projects/Server/Items/DecayScheduler.cs |

## ethics

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `ethics.enable` | bool | `false` | GetOrUpdateSetting | Enable | Projects/UOContent/Engines/Ethics/Core/Ethic.cs |

## expansion

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `expansion.forceOldAnimations` | bool | `false` | GetSetting | Force old animations | Projects/Server/ExpansionInfo.cs |

## factions

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `factions.enabled` | bool | `false` | GetSetting | Enabled | Projects/UOContent/Engines/Factions/Core/FactionSystem.cs |

## guards

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `guards.instantKill` | bool | `true` | GetSetting | Instant kill | Projects/UOContent/Mobiles/Guards/BaseGuard.cs |

## guildClickMessage

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `guildClickMessage` | bool (Core.X) | `!Core.AOS` | GetSetting | Guild click message | Projects/UOContent/Configuration/ExpansionConfiguration.cs |

## houseDecay

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `houseDecay.enable` | bool | `true` | GetOrUpdateSetting | Enable | Projects/UOContent/Multis/Houses/BaseHouse.cs |

## insurance

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `insurance.enable` | bool (Core.X) | `Core.AOS` | GetSetting | Enable | Projects/UOContent/Configuration/ExpansionConfiguration.cs |

## maps

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `maps.enableMapDiffPatches` | ClientVersion | `UOClient.ServerClientVersion < ClientVersion.Version6000` | GetSetting | Enable map diff patches | Projects/Server/TileMatrix/TileMatrixPatch.cs |
| `maps.enablePostHSMultiComponentFormat` | ClientVersion | `UOClient.ServerClientVersion == null \|\| UOClient.ServerCl...` | GetSetting | Enable post hs multi component format | Projects/Server/Client/MultiData.cs |
| `maps.enablePre6000Trammel` | dynamic | `isPre6000Trammel` | GetSetting | Enable pre6000trammel | Projects/Server/TileMatrix/TileMatrix.cs |
| `maps.enableStaticsDiffPatches` | ClientVersion | `UOClient.ServerClientVersion < ClientVersion.Version7090` | GetSetting | Enable statics diff patches | Projects/Server/TileMatrix/TileMatrixPatch.cs |

## melee

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `melee.enableInstaHit` | bool (Core.X) | `!Core.UOR` | GetSetting | Enable insta hit | Projects/UOContent/Items/Weapons/BaseWeapon.cs |

## movement

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `movement.delay.npcMaxIdle` | int | `25` | GetSetting | Npc max idle | Projects/UOContent/Mobiles/NPCSpeeds.cs |
| `movement.delay.npcMinIdle` | int | `15` | GetSetting | Npc min idle | Projects/UOContent/Mobiles/NPCSpeeds.cs |
| `movement.delay.runFoot` | int | `200` | GetOrUpdateSetting | Run foot | Projects/Server/Mobiles/Movement.cs |
| `movement.delay.runMount` | int | `100` | GetOrUpdateSetting | Run mount | Projects/Server/Mobiles/Movement.cs |
| `movement.delay.turn` | int | `0` | GetOrUpdateSetting | Turn | Projects/Server/Mobiles/Movement.cs |
| `movement.delay.walkFoot` | int | `400` | GetOrUpdateSetting | Walk foot | Projects/Server/Mobiles/Movement.cs |
| `movement.delay.walkMount` | int | `200` | GetOrUpdateSetting | Walk mount | Projects/Server/Mobiles/Movement.cs |

## movementThrottle

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `movementThrottle.debugLogging` | dynamic | `_debugLogging` | GetOrUpdateSetting | Debug logging | Projects/Server/Network/MovementThrottle.cs |
| `movementThrottle.definiteRateThreshold` | dynamic | `_definiteRateThreshold` | GetOrUpdateSetting | Definite rate threshold | Projects/Server/Network/MovementThrottle.cs |
| `movementThrottle.hardQueueLimit` | dynamic | `_hardQueueLimit` | GetOrUpdateSetting | Hard queue limit | Projects/Server/Network/MovementThrottle.cs |
| `movementThrottle.maxCredit` | dynamic | `_maxCredit` | GetOrUpdateSetting | Max credit | Projects/Server/Network/MovementThrottle.cs |
| `movementThrottle.minSamplesForRate` | dynamic | `_minSamplesForRate` | GetOrUpdateSetting | Min samples for rate | Projects/Server/Network/MovementThrottle.cs |
| `movementThrottle.movementHistorySize` | dynamic | `_movementHistorySize` | GetOrUpdateSetting | Movement history size | Projects/Server/Network/MovementThrottle.cs |
| `movementThrottle.suspiciousRateThreshold` | dynamic | `_suspiciousRateThreshold` | GetOrUpdateSetting | Suspicious rate threshold | Projects/Server/Network/MovementThrottle.cs |

## murderSystem

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `murderSystem.bountiesEnabled` | bool (Core.X) | `!Core.LBR` | GetOrUpdateSetting | Bounties enabled | Projects/UOContent/Engines/Player Murder System/PlayerMurderSystem.cs |
| `murderSystem.bountyExpiry` | TimeSpan | `TimeSpan.FromDays(14)` | GetOrUpdateSetting | Bounty expiry | Projects/UOContent/Engines/Player Murder System/PlayerMurderSystem.cs |
| `murderSystem.longTermMurderDuration` | TimeSpan | `TimeSpan.FromHours(40)` | GetOrUpdateSetting | Long term murder duration | Projects/UOContent/Engines/Player Murder System/PlayerMurderSystem.cs |
| `murderSystem.recentlyReportedDelay` | TimeSpan | `TimeSpan.FromMinutes(10)` | GetOrUpdateSetting | Recently reported delay | Projects/UOContent/Engines/Player Murder System/PlayerMurderSystem.cs |
| `murderSystem.shortTermMurderDuration` | TimeSpan | `TimeSpan.FromHours(8)` | GetOrUpdateSetting | Short term murder duration | Projects/UOContent/Engines/Player Murder System/PlayerMurderSystem.cs |

## netstate

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `netstate.packetLoggingPath` | string | `Path.Combine(Core.BaseDirectory, "Packets")` | GetSetting | Packet logging path | Projects/Server/Network/NetState/NetState.cs |

## network

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `network.encryptionDebug` | bool | `false` | GetSetting | Encryption debug | Projects/Server/Network/Encryption/EncryptionManager.cs |
| `network.encryptionMode` | EncryptionMode | `EncryptionMode.Both` | GetSetting | Encryption mode | Projects/Server/Network/Encryption/EncryptionManager.cs |

## opl

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `opl.enable` | bool (Core.X) | `Core.AOS` | GetSetting | Enable | Projects/UOContent/Configuration/ExpansionConfiguration.cs |
| `opl.enableForVendorBuy` | bool | `true` | GetSetting | Enable for vendor buy | Projects/UOContent/Mobiles/Vendors/BaseVendor.cs |

## pages

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `pages.discordWebhookUrl` | string? | `null` | GetOrUpdateSetting | Discord webhook url | Projects/UOContent/Engines/Help/PageDiscord.cs |

## pathfinding

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `pathfinding.enable` | bool | `true` | GetOrUpdateSetting | Enable | Projects/UOContent/Engines/Pathing/PathFollower.cs |

## pingServer

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `pingServer.enabled` | bool | `true` | GetOrUpdateSetting | Enabled | Projects/Server/Network/PingServer.cs |
| `pingServer.maxConnections` | int | `2048` | GetSetting | Max connections | Projects/Server/Network/PingServer.cs |
| `pingServer.port` | int | `12000` | GetSetting | Port | Projects/Server/Network/PingServer.cs |

## profanityProtection

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `profanityProtection.action` | ProfanityAction | `ProfanityAction.Disallow` | GetSetting | Action | Projects/UOContent/Misc/ProfanityProtection.cs |
| `profanityProtection.enabled` | bool | `false` | GetSetting | Enabled | Projects/UOContent/Misc/ProfanityProtection.cs |

## questSystem

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `questSystem.enableMLQuests` | bool (Core.X) | `Core.ML` | GetOrUpdateSetting | Enable ml quests | Projects/UOContent/Engines/ML Quests/MLQuestSystem.cs |

## serverListing

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `serverListing.address` | string? | `null` | GetOrUpdateSetting | Address | Projects/UOContent/Misc/ServerList.cs |
| `serverListing.autoDetect` | bool | `true` | GetOrUpdateSetting | Auto detect | Projects/UOContent/Misc/ServerList.cs |
| `serverListing.serverName` | string | `"ModernUO"` | GetOrUpdateSetting | Server name | Projects/UOContent/Misc/ServerList.cs |

## spellCasting

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `spellCasting.disableCastParalyze` | bool | `true` | GetSetting | Disable cast paralyze | Projects/Server/Mobiles/Mobile.cs |

## stamina

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `stamina.additionalLossWhenBelow` | double | `0.10` | GetOrUpdateSetting | Additional loss when below | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.baseOverweightLoss` | int | `5` | GetOrUpdateSetting | Base overweight loss | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.cannotRunWhenFatigued` | bool (Core.X) | `!Core.AOS` | GetOrUpdateSetting | Cannot run when fatigued | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.cannotWalkWhenFatigued` | bool | `false` | GetOrUpdateSetting | Cannot walk when fatigued | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.enableMountStamina` | bool | `true` | GetOrUpdateSetting | Enable mount stamina | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.globalEtherealMountStamina` | bool (Core.X) | `Core.ML` | GetSetting | Global ethereal mount stamina | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.stonesOverweightAllowance` | int | `4` | GetOrUpdateSetting | Stones overweight allowance | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.stonesPerOverweightLoss` | int | `25` | GetOrUpdateSetting | Stones per overweight loss | Projects/UOContent/Misc/StaminaSystem.cs |
| `stamina.useMountStaminaOnlyWhenOverloaded` | bool (Core.X) | `Core.SA` | GetSetting | Use mount stamina only when overloaded | Projects/UOContent/Misc/StaminaSystem.cs |

## stats

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `stats.gainChanceMultiplier` | double | `1.0` | GetOrUpdateSetting | Gain chance multiplier | Projects/UOContent/Skills/SkillCheck.cs |
| `stats.gainDelay` | conditional | `TimeSpan.FromMinutes(Core.ML ? 0.05 : 10)` | GetSetting | Gain delay | Projects/UOContent/Skills/SkillCheck.cs |
| `stats.petGainDelay` | TimeSpan | `TimeSpan.FromMinutes(5.0)` | GetSetting | Pet gain delay | Projects/UOContent/Skills/SkillCheck.cs |
| `stats.primaryStatGainChance` | double | `0.75` | GetSetting | Primary stat gain chance | Projects/UOContent/Skills/SkillCheck.cs |
| `stats.statMax` | int | `Core.LBR ? 125 : 100` | GetOrUpdateSetting | Stat max | Projects/UOContent/Skills/SkillCheck.cs |
| `stats.usePub45StatGain` | bool (Core.X) | `Core.ML` | GetSetting | Use pub45stat gain | Projects/UOContent/Skills/SkillCheck.cs |

## stealing

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `stealing.canStealContainers` | bool (Core.X) | `!Core.AOS` | GetSetting | Can steal containers | Projects/UOContent/Skills/Stealing.cs |
| `stealing.classicMode` | bool (Core.X) | `!Core.AOS` | GetSetting | Classic mode | Projects/UOContent/Skills/Stealing.cs |
| `stealing.maxWeightToSteal` | int | `10` | GetSetting | Max weight to steal | Projects/UOContent/Skills/Stealing.cs |
| `stealing.suspendOnMurder` | bool (Core.X) | `!Core.AOS` | GetSetting | Suspend on murder | Projects/UOContent/Skills/Stealing.cs |

## system

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `system.localTimeZone` | dynamic | `TimeZoneInfo.Local.Id` | GetSetting | Local time zone | Projects/Server/TimeZones/TimeZoneHandler.cs |

## taming

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `taming.enableBonding` | bool (Core.X) | `Core.LBR` | GetSetting | Enable bonding | Projects/UOContent/Mobiles/BaseCreature.cs |

## testCenter

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `testCenter.enable` | bool | `false` | GetOrUpdateSetting | Enable | Projects/UOContent/Special Systems/Engines/TestCenter.cs |

## timer

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `timer.initialPoolCapacity` | int | `1024` | GetOrUpdateSetting | Initial pool capacity | Projects/Server/Timer/Timer.Pool.cs |
| `timer.maxPoolCapacity` | dynamic | `_poolCapacity * 16` | GetOrUpdateSetting | Max pool capacity | Projects/Server/Timer/Timer.Pool.cs |

## uogateway

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `uogateway.enabled` | bool | `true` | GetOrUpdateSetting | Enabled | Projects/UOContent/Network/UOGateway.cs |

## vendor

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `vendor.isInvulnerable` | bool (Core.X) | `Core.LBR` | GetSetting | Is invulnerable | Projects/UOContent/Mobiles/Vendors/BaseVendor.cs |

## vetRewards

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `vetRewards.enable` | bool | `true` | GetOrUpdateSetting | Enable | Projects/UOContent/Engines/Veteran Rewards/RewardSystem.cs |
| `vetRewards.rewardInterval` | TimeSpan | `TimeSpan.FromDays(30.0)` | GetOrUpdateSetting | Reward interval | Projects/UOContent/Engines/Veteran Rewards/RewardSystem.cs |
| `vetRewards.skillCapRewards` | bool | `true` | GetOrUpdateSetting | Skill cap rewards | Projects/UOContent/Engines/Veteran Rewards/RewardSystem.cs |

## virtualChecks

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `virtualChecks.useEditGump` | bool (Core.X) | `Core.TOL` | GetSetting | Use edit gump | Projects/Server/Items/VirtualCheck.cs |

## visibleDamage

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `visibleDamage` | bool (Core.X) | `Core.AOS` | GetSetting | Visible damage | Projects/UOContent/Configuration/ExpansionConfiguration.cs |

## world

| Key | Type | Default | Get | Description | Source |
|-----|------|---------|-----|-------------|--------|
| `world.enableAutoRestart` | bool | `false` | GetOrUpdateSetting | Enable auto restart | Projects/UOContent/Misc/AutoRestart.cs |
| `world.savePath` | string | `"Saves"` | GetSetting | Save path | Projects/UOContent/World Saves/AutoArchive.cs, Projects/Server/World/World.cs |
| `world.tempSavePath` | string | `"temp"` | GetSetting | Temp save path | Projects/Server/World/World.cs |

---

## Code-Defined Settings (Not User-Editable)

These settings use static readonly fields or hardcoded values in source code. They cannot be modified through `modernuo.json`.

### Harvesting

| Setting | Type | Default | File | Description |
|---------|------|---------|------|-------------|
| `BankWidth` | int | — | `HarvestDefinition.cs` | Harvest bank width |
| `BankHeight` | int | — | `HarvestDefinition.cs` | Harvest bank height |
| `MinTotal` | int | — | `HarvestDefinition.cs` | Min items per harvest |
| `MaxTotal` | int | — | `HarvestDefinition.cs` | Max items per harvest |
| `MinRespawn` | TimeSpan | — | `HarvestDefinition.cs` | Min respawn time |
| `MaxRespawn` | TimeSpan | — | `HarvestDefinition.cs` | Max respawn time |
| `MaxRange` | int | — | `HarvestDefinition.cs` | Harvest range |
| `ConsumedPerHarvest` | int | — | `HarvestDefinition.cs` | Items consumed per harvest |
| `Skill` | SkillName | — | `HarvestDefinition.cs` | Required skill |

### Spawn System

| Setting | Type | Default | File | Description |
|---------|------|---------|------|-------------|
| `DefaultMinDelay` | TimeSpan | `5 min` | `BaseSpawner.cs:51` | Default min spawn delay |
| `DefaultMaxDelay` | TimeSpan | `10 min` | `BaseSpawner.cs:52` | Default max spawn delay |

Per-spawner settings are defined in `Distribution/Data/Spawns/**/*.json`:

| Setting | Type | Description |
|---------|------|-------------|
| `count` | int | Spawn count |
| `minDelay` | TimeSpan | Min spawn delay |
| `maxDelay` | TimeSpan | Max spawn delay |
| `homeRange` | int | Home range |
| `walkingRange` | int | Walking range |
| `entries` | array | Spawn entries |

### ConPVP

| Setting | Type | Default | File | Description |
|---------|------|---------|------|-------------|
| `CombatDelay` | TimeSpan | `30 sec` | `ConPVP/` | Combat delay |
| `AutoTieDelay` | TimeSpan | `15 min` | `ConPVP/` | Auto-tie delay |
| `SliceInterval` | TimeSpan | `12 sec` | `ConPVP/` | Slice interval |

### Virtues

| Setting | Type | Default | File | Description |
|---------|------|---------|------|-------------|
| `Compassion.LossDelay` | TimeSpan | `7 days` | `Compassion.cs:10` | Compassion loss delay |
| `Honor.UseDelay` | TimeSpan | `5 min` | `Honor.cs:12` | Honor use delay |
| `Justice.LossDelay` | TimeSpan | `7 days` | `Justice.cs:22` | Justice loss delay |
| `Sacrifice.GainDelay` | TimeSpan | `1 day` | `Sacrifice.cs:12` | Sacrifice gain delay |
| `Sacrifice.LossDelay` | TimeSpan | `7 days` | `Sacrifice.cs:13` | Sacrifice loss delay |
| `Valor.LossDelay` | TimeSpan | `7 days` | `Valor.cs:12` | Valor loss delay |

---

## Expansion-Gated Settings

Settings whose defaults depend on the active expansion (`Core.XXX`):

| Key | Expansion Dependency | Description |
|-----|---------------------|-------------|
| `accountGold.enable` | `Core.TOL` | Account gold only available in TOL+ |
| `accountGold.convertOnBank` | `Core.TOL` | Account gold bank conversion |
| `accountGold.convertOnTrade` | `Core.TOL` | Account gold trade conversion |
| `stats.statMax` | `Core.LBR ? 125 : 100` | Stat cap increases in LBR |
| `stats.gainDelay` | `Core.ML ? 0.05 : 10` min | Faster stat gains in ML |
| `stats.usePub45StatGain` | `Core.ML` | Pub 45 stat gain rates |
| `stamina.useMountStaminaOnlyWhenOverloaded` | `Core.SA` | Mount stamina behavior in SA+ |
| `stamina.globalEtherealMountStamina` | `Core.ML` | Global ethereal in ML+ |
| `melee.enableInstaHit` | `!Core.UOR` | Insta-hit disabled in UOR+ |
| `insurance.enable` | `Core.AOS` | Insurance introduced in AOS |
| `visibleDamage` | `Core.AOS` | Visible damage in AOS+ |
| `opl.enable` | `Core.AOS` | OPL in AOS+ |
| `actionDelay` | `Core.AOS ? 1000 : 500` | Action delay differs by expansion |
| `buffIcons.enable` | `Core.ML` | Buff icons in ML+ |
| `questSystem.enableMLQuests` | `Core.ML` | ML quests |
| `vendor.isInvulnerable` | `Core.LBR` | Vendor invulnerability |
| `taming.enableBonding` | `Core.LBR` | Pet bonding |
| `virtualChecks.useEditGump` | `Core.TOL` | Edit gump |
| `guildClickMessage` | `!Core.AOS` | Legacy click messages |
| `asciiClickMessage` | `!Core.AOS` | Legacy click messages |
| `stealing.classicMode` | `!Core.AOS` | Classic stealing |
| `stealing.suspendOnMurder` | `!Core.AOS` | Stealing on murder |
| `stealing.canStealContainers` | `!Core.AOS` | Steal containers |
| `murderSystem.bountiesEnabled` | `!Core.LBR` | Bounties disabled in LBR+ |
| `stamina.cannotRunWhenFatigued` | `!Core.AOS` | Fatigue running |
