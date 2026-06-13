# Receipt Issuance Runbook

Operational checks for issuing final donation receipts.

## Preconditions

- Donation exists.
- Donation reconciliation status is `confirmed`.
- Required donor and donation fields are present.

## Steps

1. Verify donation status through reconciliation view/API.
2. If status is not confirmed, stop and resolve reconciliation first.
3. Call final issuance endpoint (`POST /api/receipts/{donationId}/issue-final`).
4. Confirm issued receipt number and persisted state.

## Expected Outcomes

- Confirmed donation -> final receipt issued (`2xx`).
- Pending/exception donation -> final receipt blocked.

## Troubleshooting

- If issuance fails for a confirmed donation, verify receipt payload completeness and donation linkage integrity.
