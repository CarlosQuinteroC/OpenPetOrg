# donor-dashboard Specification

## Purpose
Define donor self-service transparency for contributions, statuses, and receipt access.

## Requirements

### Requirement: Donor Transparency Dashboard
The system MUST provide each donor a dashboard showing donation history, reconciliation/confirmation status, recurring status, and available receipts.

#### Scenario: Donor views own history
- GIVEN an authenticated donor
- WHEN the donor opens the dashboard
- THEN the system shows only that donor's records and receipt documents

#### Scenario: Donor without receipt-ready donations
- GIVEN an authenticated donor with no confirmed donations
- WHEN the donor opens receipt history
- THEN the system shows an empty state and SHALL NOT expose unavailable receipts
