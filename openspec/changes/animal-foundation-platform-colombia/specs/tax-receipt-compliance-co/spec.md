# tax-receipt-compliance-co Specification

## Purpose
Define issuance boundaries for Colombia-oriented donation receipt/comprobante compliance.

## Requirements

### Requirement: DIAN-Oriented Receipt/Comprobante
The system MUST issue donation receipts/comprobantes with required donor and donation fields aligned to DIAN expectations for confirmed donations.

#### Scenario: Receipt requested before confirmation
- GIVEN a donation that is not yet reconciliation-confirmed
- WHEN a final receipt is requested
- THEN the system blocks final issuance until confirmation criteria are met

#### Scenario: Confirmed donation receipt issuance
- GIVEN a donation that satisfies confirmation criteria
- WHEN the final receipt is requested
- THEN the system issues a receipt including required donor and donation fields
