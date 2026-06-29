# Example: domain process model (Forkfully, condensed)

> A condensed sample showing the **shape** of the output. The real model is longer and richer in
> hotspots. No code — problem space only.

## Scope

- **Initial command:** `CreateDinner`
- **Terminal event:** `«DinnerEnded»`
- **Out of scope (already established):** the user has registered and been approved as a host.

## Legend

`[Actor]` command · `Command` (blue) · `«Event»` (orange, past tense) · `(System)` consistency
boundary · `→ Policy` (purple) reactive rule · `{Read model}` (green) · `‹External›` (pink) ·
`HOTSPOT` (red).

## Flow

```
[Host] CreateDinner            (Dinner) → «DinnerCreated»
   → Policy: whenever «DinnerCreated», list it for guests
                               {Available dinners}  (green, shown to guests)

[Guest] ReserveDinner          (Dinner) → «DinnerReserved»
   HOTSPOT: what if capacity is exceeded between read and reserve?
   → Policy: whenever «DinnerReserved», issue a bill
                               (Bill) → «BillIssued»
   ‹Payments› charge the guest  → «GuestCharged»   (pink, external)

— pivotal —                    «DinnerStarted»
[Host] CloseDinner             (Dinner) → «DinnerEnded»
   → Policy: whenever «DinnerEnded», invite a review
[Guest] LeaveReview            (MenuReview) → «ReviewLeft»
```

## Hotspots carried forward

- Capacity race on reservation (consistency boundary — flag for aggregate design).
- Refund/cancellation flow not yet modeled.
- Who owns "approve as host" — a separate context? (→ strategic-ddd).
