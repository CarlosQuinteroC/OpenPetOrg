# donation-intake-hybrid Specification

## Purpose
Define unified intake behavior for online payments and offline/manual donation receipts.

## Requirements

### Requirement: Hybrid Donation Registration
The system MUST register online payments and offline/manual receipts in one donation ledger with source type and traceable status.

#### Scenario: Online donation recorded
- GIVEN a successful online payment event
- WHEN a donation record is created
- THEN the ledger stores donor reference, amount, channel=online, and initial reconciliation status

#### Scenario: Offline donation missing required data
- GIVEN staff submits an offline donation without required donor or amount fields
- WHEN validation is executed
- THEN the system rejects the record and SHALL return actionable validation feedback
