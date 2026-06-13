# Backend API Documentation Gate

This document defines the API verification path used across PR3-PR6.

## Quick Path (Keycloak + Swagger)

1. Start Keycloak with the seeded `petorg-dev` realm (`docs/auth/keycloak-dev-setup.md`).
2. Sign in with seeded `staff.dev` / `Staff123!` and obtain an access token (audience `petorg-api`).
3. Open `/swagger`, click **Authorize**, and paste `Bearer {access_token}`.
4. Call `GET /api/me` and confirm role output includes `Staff` or `Donor`.
5. Run one staff-protected endpoint (for example `POST /api/donations`) and confirm role policy behavior.

## Swagger

- Endpoint: `GET /swagger/v1/swagger.json`
- UI: `/swagger`
- Auth: `Bearer` JWT configured in Swagger security scheme (`Authorization: Bearer {token}`).

### Swagger Authorize (Keycloak)

For local Keycloak flow:

1. Obtain a Keycloak access token for realm `petorg-dev` and audience `petorg-api`.
2. Open Swagger UI (`/swagger`) and click **Authorize**.
3. Paste token as `Bearer {access_token}`.
4. Call `GET /api/me` to confirm role claims (`Donor` or `Staff`) are present.
5. Call protected endpoints and verify role policy behavior (`StaffOnly` enforced on module routes in current slice).
6. If `401` appears, verify issuer/audience alignment in appsettings; if `403` appears, verify token roles.

Backend auth config touchpoints used by this flow:

- `Backend/src/PetOrg.Api/appsettings*.json` → `Authentication:ManagedIdentity`
- `Backend/src/PetOrg.Api/Program.cs` → JWT bearer validation + role claim enrichment
- `Backend/src/PetOrg.Api/Security/ManagedIdentityOptions.cs` → issuer/audience/role mapping settings

## Auth Claim Checks (Minimum)

Use `GET /api/me` as the contract probe.

Expected fields:

- `name`: non-empty authenticated principal
- `roles`: includes at least one of `Donor` or `Staff`

Expected outcomes:

- Missing/invalid token -> `401 Unauthorized`
- Valid token without required role for endpoint -> `403 Forbidden`
- Valid token with required role -> `2xx`

## Test Gate

Run:

```bash
dotnet test Backend/PetOrg.sln
```

Expected baseline:

- Integration tests for auth, donations, reconciliation pass.
- Unit tests for recurring lifecycle, consent audit, receipt issuance gate, and timeline separation pass.

## Postman Artifacts

See `docs/api/postman/`.

- Collection: `animal-foundation-platform-colombia.postman_collection.json`
- Environment: `animal-foundation-platform-colombia.postman_environment.json`
- Usage: `docs/api/postman/README.md`

## Frontend Start Gate

Frontend integration tasks (phase 4) can start only when:

1. Backend tests are green (`dotnet test` pass).
2. Swagger docs are exposed and reviewed.
3. Postman collection + environment are published and usable.
