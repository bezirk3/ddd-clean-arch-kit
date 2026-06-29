namespace Forkfully.Contracts.Bills;

public record BillResponse(
    string Id,
    string DinnerId,
    string GuestId,
    decimal Amount,
    string Currency);
