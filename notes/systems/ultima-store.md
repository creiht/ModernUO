# Ultima Store

The Ultima Store is the microtransaction integration for ModernUO, providing a client-side storefront for cosmetic and convenience items purchased with real money. The server-side implementation consists of a single packet handler that intercepts the client's store open request. As of the current implementation, the store is not active and returns a placeholder message to players.

**Source Files:**
- `Projects/UOContent/Engines/UltimaStore/UltimaStorePackets.cs` (18 lines) — packet handler registration and stub response

---

## Packet Protocol

### Incoming: Store Open Request (`0xFA`)

| Field | Value |
|-------|-------|
| Packet ID | `0xFA` |
| Length | 1 byte |
| In-Game Only | `true` (handler only active when client is logged in) |

When the client opens the Ultima Store UI, it sends packet `0xFA` to the server. The server handler is registered via:

```csharp
IncomingPackets.Register(0xFA, 1, true, &UltimaStoreOpenRequest);
```

### Current Handler

The `UltimaStoreOpenRequest` method (`UltimaStorePackets.cs:13-16`) receives the packet and sends a message to the requesting player:

```csharp
public static void UltimaStoreOpenRequest(NetState state, SpanReader reader)
{
    state.Mobile.SendMessage("Ultima Store is not currently available.");
}
```

The `reader` payload is empty (1-byte packet with just the packet ID). The handler receives the `NetState` to access the player's `Mobile` for sending the response message.

---

## Protocol Flag

The Ultima Store is gated behind the `UltimaStore` protocol flag (`0x00004000`), defined in `ProtocolChanges.cs:38`. This flag is included in version `7.0.50.0`:

```csharp
UltimaStore = 0x00004000,
Version70500 = Version704565 | UltimaStore,
```

Clients running version 7.0.50.0 or later will include this flag and be able to send the `0xFA` packet. Older clients will not have the store UI and cannot trigger the packet.

---

## Integration Notes

### Server-Side Only

The current implementation is purely server-side packet handling. The Ultima Store UI is rendered entirely by the client. The server's role is to:

1. Receive the store open request
2. Validate access (currently a placeholder check)
3. Respond with the store inventory/pricing data (not yet implemented)
4. Process purchase transactions when the client submits them

### Future Implementation

A production-ready implementation would need to add:

- **Authentication**: Verify the player's account is in good standing (no bans, etc.)
- **Store Catalog**: Fetch the list of available items from a backend service or config
- **Purchase Processing**: Handle client purchase requests, validate payment, and grant items
- **Inventory Delivery**: Deliver purchased items to the player's account (backpack or account bank)
- **Error Handling**: Graceful responses for declined payments, unavailable items, etc.

The packet registration pattern (`IncomingPackets.Register`) is already in place for extending with additional handlers for purchase requests and responses.

---

## Cross-References

- `ProtocolChanges.cs:38,53` — UltimaStore protocol flag definition and version 7.0.50.0 inclusion
