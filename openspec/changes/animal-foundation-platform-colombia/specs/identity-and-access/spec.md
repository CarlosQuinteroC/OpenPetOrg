# identity-and-access Specification

## Purpose
Define secure managed authentication and role-governed access for donor and staff workflows.

## Requirements

### Requirement: Managed Identity Authentication
The system MUST authenticate users through a managed identity provider and SHALL enforce role-based access to donor and staff capabilities. The system SHALL NOT provide custom local-password authentication in MVP.

#### Scenario: Authorized user signs in
- GIVEN a user with valid managed-identity credentials and an assigned role
- WHEN the user starts a session
- THEN the system grants access only to capabilities allowed for that role

#### Scenario: Invalid or revoked token
- GIVEN a token that is invalid, expired, or revoked
- WHEN a protected endpoint is requested
- THEN the system denies access and returns no protected data
