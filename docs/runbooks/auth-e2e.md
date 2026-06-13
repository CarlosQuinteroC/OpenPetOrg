# Auth E2E Runbook (PR6)

Minimal end-to-end verification for Keycloak-backed authentication and authorization.

## Scope

- Login with seeded Keycloak users
- Protected API access with valid token
- Role denial on staff-protected endpoint
- Logout completion

## Prerequisites

- Docker running
- Keycloak started from `deploy/keycloak/docker-compose.yml`
- Backend running on `http://localhost:5000`
- Swagger available at `http://localhost:5000/swagger`

## Test Accounts

- Staff: `staff.dev` / `Staff123!` (`Staff` role)
- Donor: `donor.dev` / `Donor123!` (`Donor` role)

## Verification Steps

### 1) Start local IdP

```bash
docker compose -f deploy/keycloak/docker-compose.yml up -d
```

Expected: Keycloak admin and realm endpoints reachable.

### 2) Obtain Staff token

```bash
curl -X POST "http://localhost:8080/realms/petorg-dev/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  --data "client_id=petorg-frontend" \
  --data "grant_type=password" \
  --data "username=staff.dev" \
  --data "password=Staff123!"
```

Expected: response includes non-empty `access_token`.

### 3) Protected call with Staff token

- In Swagger **Authorize**, paste `Bearer {staff_access_token}`.
- Call `GET /api/me`.

Expected: `200 OK`, payload roles include `Staff`.

### 4) Staff-only endpoint success

Call `POST /api/donations` with staff token and valid payload.

Expected: `201 Created`.

### 5) Role denial with Donor token

Repeat token request with `donor.dev` / `Donor123!` and authorize with donor token.

Call `POST /api/donations`.

Expected: `403 Forbidden`.

### 6) Missing token denial

Call `GET /api/me` without bearer token.

Expected: `401 Unauthorized`.

### 7) Logout

- Frontend flow: trigger logout from auth client.
- API validation: repeat `GET /api/me` without token.

Expected: no authenticated access (`401`).

## Evidence (Current PR6)

| Check | Evidence | Result |
|---|---|---|
| Missing token denied | `AuthEndpointsTests.Protected_endpoint_without_token_returns_unauthorized` | ✅ |
| Staff token allowed on staff endpoint | `AuthEndpointsTests.Staff_role_can_access_staff_protected_donation_endpoint` | ✅ |
| Donor denied on staff endpoint | `AuthEndpointsTests.Donor_role_cannot_access_staff_only_endpoint` | ✅ |

Command executed:

```bash
dotnet test Backend/PetOrg.sln --filter "FullyQualifiedName~PetOrg.IntegrationTests.Auth.AuthEndpointsTests"
```

Result: `Passed: 3, Failed: 0`.

## Troubleshooting

- `401` with seemingly valid token: verify Keycloak authority, issuer, and audience in `Backend/PetOrg/PetOrg/appsettings*.json`.
- `403` for expected staff flow: verify token includes `Staff` in `realm_access.roles` or `resource_access.petorg-api.roles`.
- Token retrieval fails: confirm Keycloak container healthy and realm import completed.
