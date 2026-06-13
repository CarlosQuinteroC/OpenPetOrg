# Postman API Verification Pack

This folder contains API verification artifacts used through PR3-PR6.

## Quick Path (Keycloak Token + Collection Run)

1. Boot Keycloak realm `petorg-dev` (`docs/auth/keycloak-dev-setup.md`).
2. Request an access token from Keycloak token endpoint.
3. Paste token into `jwt_token` environment variable.
4. Run the collection in the suggested order.
5. Confirm authorization outcomes (`401` without token, `403` with wrong role, success with required role).

## Files

- `animal-foundation-platform-colombia.postman_collection.json`
- `animal-foundation-platform-colombia.postman_environment.json`

## Import

1. Open Postman.
2. Import the collection and environment files.
3. Select environment **PetOrg Local Backend**.

## Required Variables

- `base_url`: API URL (default: `http://localhost:5000`)
- `jwt_token`: bearer token from managed IdP
- `donor_id`: test donor GUID

### Optional Keycloak Variables (recommended)

- `keycloak_url`: `http://localhost:8080`
- `keycloak_realm`: `petorg-dev`
- `keycloak_token_url`: `{{keycloak_url}}/realms/{{keycloak_realm}}/protocol/openid-connect/token`
- `keycloak_client_id`: `petorg-frontend`
- `keycloak_username`: `staff.dev` or `donor.dev`
- `keycloak_password`: seeded user password

`donation_id` and `recurring_id` are auto-populated by collection tests after creation calls.

## Keycloak Token Retrieval (Manual)

Run this request (outside collection or in a scratch tab):

```bash
curl -X POST "http://localhost:8080/realms/petorg-dev/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  --data "client_id=petorg-frontend" \
  --data "grant_type=password" \
  --data "username=staff.dev" \
  --data "password=Staff123!"
```

Copy `access_token` into `jwt_token`.

> Note: password grant is documented here only for local verification convenience with seeded users.

## Suggested Run Order

1. Auth → `Get current user`
2. Donations → `Create donation`
3. Reconciliation → `Confirm unique donor match` or `Mark ambiguous match`
4. Receipts → `Issue final receipt` (after confirmation only)
5. Recurring → `Enroll recurring donor`
6. Consent → `Append public recognition consent` then `Get public recognition visibility`

## Backend Gate Checklist

- `dotnet test Backend/PetOrg.sln` passes.
- Swagger UI available at `/swagger`.
- Collection requests map to implemented backend endpoints.
- Frontend implementation starts only after this backend gate is complete.

## Auth Verification Checklist (PR6)

- [ ] `GET /api/me` with valid `staff.dev` token returns `200` and role `Staff`.
- [ ] `POST /api/donations` with staff token returns `201`.
- [ ] `POST /api/donations` with donor token is denied (`403`).
- [ ] `GET /api/me` without bearer token is denied (`401`).
