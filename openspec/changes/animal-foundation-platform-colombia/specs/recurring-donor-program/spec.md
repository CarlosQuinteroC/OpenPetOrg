# recurring-donor-program Specification

## Purpose
Define monthly recurring donation enrollment, state transitions, and failure visibility.

## Requirements

### Requirement: Monthly Recurring Lifecycle
The system MUST support recurring monthly donation enrollment, active status tracking, and cancellation without manual database intervention.

#### Scenario: Enrollment then cancellation
- GIVEN a donor eligible for recurring donations
- WHEN the donor enrolls and later cancels
- THEN status transitions from active to cancelled and effective dates are retained

#### Scenario: Failed recurring charge
- GIVEN an active recurring donation with a failed cycle charge
- WHEN cycle processing is completed
- THEN the recurring record is marked payment-failed and remains auditable
