# Backend Role Mapping (Keycloak)

This document defines the token/claim contract for backend authorization policies.

## Authorization Policies

- `DonorOnly` policy requires role claim `Donor`
- `StaffOnly` policy requires role claim `Staff`

Most current API endpoints are protected with `StaffOnly`.

## Expected Token Issuer/Audience

- Issuer: `http://localhost:8080/realms/petorg-dev`
- Audience: `petorg-api`

If issuer or audience does not match backend configuration, JWT validation fails.

## Role Claim Sources

The backend enriches `.NET` role claims (`http://schemas.microsoft.com/ws/2008/06/identity/claims/role`) from these Keycloak token locations:

1. Direct claim configured by `Authentication:ManagedIdentity:RoleClaimType` (default: `roles`)
2. `realm_access.roles`
3. `resource_access.{ResourceAccessClientId}.roles` (default client id: `petorg-api`)

This keeps policy checks stable even if Keycloak role emission differs between realm and client role mappings.

## Required Role Names

Role names are case-sensitive in policy intent and should be emitted exactly as:

- `Donor`
- `Staff`

## Practical Validation

Use `GET /api/me` with a bearer token and confirm the response includes expected role values.

Example expected payload for staff user:

```json
{
  "name": "staff.dev",
  "roles": ["Staff"]
}
```
