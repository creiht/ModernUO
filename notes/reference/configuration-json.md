# Configuration (JsonConfig)

> **See also:** [Configuration Reference](configuration.md) · [Feature Flags](feature-flags.md)

ModernUO uses JsonConfig for complex data structures that require more than simple key-value pairs. These settings are stored in separate JSON files under `Configuration/`.

## Email Configuration

**File:** `Configuration/email-settings.json`
**Source:** `UOContent/Configuration/EmailConfiguration.cs`

Used for sending crash reports, speech logs, and GM support emails via SMTP.

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| `enabled` | bool | `false` | Enable email system |
| `fromAddress` | string | `"support@modernuo.com"` | From email address |
| `fromName` | string | `"ModernUO Team"` | From display name |
| `crashAddress` | string | `"crashes@modernuo.com"` | Crash report email |
| `crashName` | string | `"Crash Log"` | Crash report name |
| `speechLogPageAddress` | string | `"support@modernuo.com"` | Speech log email |
| `speechLogPageName` | string | `"GM Support Conversation"` | Speech log name |
| `emailServer` | string | `"smtp.gmail.com"` | SMTP server |
| `emailPort` | int | `465` | SMTP port |
| `emailUsername` | string | `"support@modernuo.com"` | SMTP username |
| `emailPassword` | string | `"Some Password 123"` | SMTP password |
| `emailSendRetryCount` | int | `5` | Send retry count |
| `emailSendRetryDelay` | int | `3` | Send retry delay (seconds) |

## Assistant Configuration

**File:** `Configuration/assistants.json`
**Source:** `UOContent/Assistants/AssistantConfiguration.cs`

Controls behavior for UO assistant tools (UOSteam, Razor) that negotiate features with the server.

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| `warnOnFailure` | bool | `true` | Warn player when assistant negotiation fails |
| `kickOnFailure` | bool | `true` | Kick player when assistant negotiation fails |
| `disallowedFeatures` | AssistantFlags | `None` | Disallowed assistant features (bitmask) |
| `disconnectDelay` | TimeSpan | `15 sec` | Delay before disconnecting on failure |
| `warningMessage` | string | *See below* | Warning message shown to player |

### Default Warning Message

The default warning message informs players they need to update their assistant tool and enable feature negotiation:

```html
The server was unable to negotiate features with your assistant. You must 
download and run an updated version of <A HREF="https://uosteam.com">UOSteam</A> 
or <A HREF="https://github.com/markdwags/Razor/releases/latest">Razor</A>.
<br><br>
Make sure you've checked the option <B>Negotiate features with server</B>, 
once you have this box checked you may log in and play normally.
<br><br>
You will be disconnected shortly.
```

### Assistant Features (AssistantFeatures enum)

| Flag | Description |
|------|-------------|
| `None` | No features disallowed |
| `GumpNavigation` | Gump navigation feature |
| `LabelColors` | Label color support |
| `PetCommands` | Pet command support |
| `SpellList` | Spell list feature |
| `All` | All features |

See `AssistantFeatures` enum in `UOContent/Assistants/` for the complete list.
