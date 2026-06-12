namespace PetOrg.Modules.Receipts.Contracts;

public sealed record IssueFinalReceiptResponse(
    Guid ReceiptId,
    Guid DonationId,
    string ReceiptNumber,
    string Status,
    DateTimeOffset IssuedAt);
