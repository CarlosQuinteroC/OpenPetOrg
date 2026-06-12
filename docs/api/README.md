# Backend API Documentation Gate (PR3)

This document defines the backend verification + API docs gate required before frontend implementation (PR4).

## Swagger

- Endpoint: `GET /swagger/v1/swagger.json`
- UI: `/swagger`
- Auth: `Bearer` JWT configured in Swagger security scheme.

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
