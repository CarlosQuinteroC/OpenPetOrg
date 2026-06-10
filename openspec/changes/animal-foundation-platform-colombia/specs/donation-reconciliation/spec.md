# donation-reconciliation Specification

## Purpose
Define reconciliation rules that link confirmed fund movements to donor records or controlled exceptions.

## Requirements

### Requirement: Donation-to-Donor Reconciliation
The system MUST link every confirmed fund movement to exactly one donor record or place it in a reviewable exception state.

#### Scenario: Unique match confirms donation
- GIVEN an unmatched donation with a unique donor match
- WHEN reconciliation is run
- THEN the donation is marked confirmed and linked to that donor

#### Scenario: Ambiguous donor match
- GIVEN an unmatched donation with multiple plausible donor matches
- WHEN reconciliation is run
- THEN the donation is moved to exception state and SHALL NOT be auto-confirmed
