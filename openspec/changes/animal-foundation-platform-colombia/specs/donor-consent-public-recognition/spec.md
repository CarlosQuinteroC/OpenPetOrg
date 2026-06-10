# donor-consent-public-recognition Specification

## Purpose
Define explicit-consent controls for public donor-name visibility and auditability.

## Requirements

### Requirement: Consent-Controlled Public Name Visibility
The system MUST display donor names publicly only when explicit consent is present and MUST retain auditable consent history.

#### Scenario: Consent granted then revoked
- GIVEN a donor who grants consent and later revokes it
- WHEN public recognition output is generated
- THEN the donor name appears only during consented periods

#### Scenario: Missing explicit consent
- GIVEN a donor without explicit public-recognition consent
- WHEN the public donor list is generated
- THEN the system SHALL NOT display that donor's name
