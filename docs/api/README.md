# Backend API Documentation Gate (PR3)

This document defines the backend verification + API docs gate required before frontend implementation (PR4).

## Swagger

- Endpoint: `GET /swagger/v1/swagger.json`
- UI: `/swagger`
- Auth: `Bearer` JWT configured in Swagger security scheme.

### Swagger Authorize (PR5 Keycloak Touchpoint)

For local Keycloak-enabled flow:

1. Obtain a Keycloak access token for realm `petorg-dev` and audience `petorg-api`.
2. Open Swagger UI (`/swagger`) and click **Authorize**.
3. Paste token as `Bearer {access_token}`.
4. Call `GET /api/me` to confirm role claims (`Donor` or `Staff`) are present.
5. Call protected endpoints and verify role policy behavior (`StaffOnly` currently enforced on most module routes).

Backend auth config touchpoints used by this flow:

- `Backend/src/PetOrg.Api/appsettings*.json` → `Authentication:ManagedIdentity`
- `Backend/src/PetOrg.Api/Program.cs` → JWT bearer validation + role claim enrichment
- `Backend/src/PetOrg.Api/Security/ManagedIdentityOptions.cs` → issuer/audience/role mapping settings

## Test Gate

Run:

```bash
dotnet test Backend/PetOrg.sln
```

Expected in PR3:

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
