# Keycloak Local Development Setup

This guide bootstraps a local Keycloak realm for PR5 auth integration.

## Prerequisites

- Docker Desktop (or Docker Engine + Compose plugin)
- Backend running on `http://localhost:5000`
- Frontend running on `http://localhost:5173`

## Boot Keycloak

From repo root:

```bash
docker compose -f deploy/keycloak/docker-compose.yml up -d
```

Keycloak Admin Console:

- URL: `http://localhost:8080/admin`
- User: `admin`
- Password: `admin`

The container imports `deploy/keycloak/realm-export/petorg-dev-realm.json` at startup.

## Seeded Realm/Clients

- Realm: `petorg-dev`
- Frontend public client (PKCE): `petorg-frontend`
- API bearer-only client: `petorg-api`

## Seeded Test Users

- Staff user: `staff.dev` / `Staff123!` (realm role: `Staff`)
- Donor user: `donor.dev` / `Donor123!` (realm role: `Donor`)

## Required Frontend Environment Variables

Create `Frontend/.env.local`:

```env
VITE_API_BASE_URL=http://localhost:5000
VITE_AUTH_ENABLED=true
VITE_KEYCLOAK_URL=http://localhost:8080
VITE_KEYCLOAK_REALM=petorg-dev
VITE_KEYCLOAK_CLIENT_ID=petorg-frontend
```

## Backend Auth Defaults

The backend `appsettings*.json` in this slice default to:

- Authority: `http://localhost:8080/realms/petorg-dev`
- Audience: `petorg-api`
- Role mapping from realm/client role claims into `.NET` role claims

## Smoke Test

1. Start backend and frontend.
2. Open frontend and trigger login.
3. Sign in as `staff.dev`.
4. Call `GET /api/me` from UI-backed flow and confirm returned roles include `Staff`.
5. Call a staff-protected endpoint (for example donation creation) and verify `200/201`.

## Shutdown

```bash
docker compose -f deploy/keycloak/docker-compose.yml down
```
