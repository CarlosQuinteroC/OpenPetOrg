# Tasks: Animal Foundation Platform Colombia

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | 2,200–3,000 |
| 400-line budget risk | High |
| Chained PRs recommended | Yes |
| Suggested split | PR 1 → PR 2 → PR 3 → PR 4 → PR 5 → PR 6 |
| Delivery strategy | ask-on-risk |
| Chain strategy | feature-branch-chain |

Decision needed before apply: Yes
Chained PRs recommended: Yes
Chain strategy: feature-branch-chain
400-line budget risk: High

### Suggested Work Units

| Unit | Goal | Likely PR | Notes |
|------|------|-----------|-------|
| 1 | Foundation scaffold baseline (completed) | PR 1 ✅ | Base = feature/tracker branch |
| 2 | Backend domain + API layering (completed) | PR 2 ✅ | Base = PR 1 branch |
| 3 | Backend tests + Swagger/Postman gate (completed) | PR 3 ✅ | Base = PR 2 branch |
| 4 | Frontend feature integration baseline (completed) | PR 4 ✅ | Base = PR 3 branch |
| 5 | Keycloak integration (dev IdP + backend JWT + frontend auth + bearer wiring) | PR 5 | Base = PR 4 branch; auth slice only |
| 6 | Auth docs/runbooks + minimal E2E auth verification + remaining cleanup docs | PR 6 | Base = PR 5 branch |

## Phase 1: Foundation / Infrastructure (Completed PR1)

- [x] 1.1 Baseline solution, API host, persistence, migrations, and frontend app shell created under `Backend/` and `Frontend/`.

## Phase 2: Backend Core Implementation (Completed PR2)

- [x] 2.1 Donations, reconciliation, recurring, consent, and receipt modules implemented in `Backend/src/PetOrg.Modules.*/`.

## Phase 3: Backend Verification Gate (Completed PR3)

- [x] 3.1 Integration/unit test suites and Swagger/Postman baseline docs delivered in `Backend/tests/` and `docs/api/`.

## Phase 4: Frontend Baseline Integration (Completed PR4)

- [x] 4.1 Donation, reconciliation, dashboard, and animal-case views plus typed API clients implemented in `Frontend/src/`.

## Phase 5: Keycloak Integration (PR5)

- [ ] 5.1 Add Keycloak local dev bootstrap with realm/client seed docs in `deploy/keycloak/docker-compose.yml`, `deploy/keycloak/realm-export/`, and `docs/auth/keycloak-dev-setup.md`.
- [ ] 5.2 Update backend JWT config for Keycloak realm/client and role claim mapping in `Backend/src/PetOrg.Api/Security/ManagedIdentityOptions.cs`, `Backend/src/PetOrg.Api/Program.cs`, and `Backend/src/PetOrg.Api/appsettings*.json`.
- [ ] 5.3 Document backend role expectations (`Donor`, `Staff`) and token claim contract in `docs/auth/backend-role-mapping.md`.
- [ ] 5.4 Implement frontend auth client using the existing `AuthClient` abstraction with Keycloak login/logout/token retrieval in `Frontend/src/app/auth/authClient.ts`, `Frontend/src/app/auth/useAuthClient.ts`, and `Frontend/src/app/providers.tsx`.
- [ ] 5.5 Wire API bearer usage to real auth tokens in `Frontend/src/services/api/client.ts`, `Frontend/src/services/api/context.tsx`, and `Frontend/src/services/api/useApi.ts`.

## Phase 6: Auth Verification + Remaining Docs (PR6)

- [ ] 6.1 Add Swagger JWT authorize usage notes (Keycloak token flow + expected claim checks) in `docs/api/README.md`.
- [ ] 6.2 Update Postman usage for Keycloak token retrieval/variable refresh in `docs/api/postman/README.md`.
- [ ] 6.3 Add minimal E2E auth verification runbook for login, protected API call, role denial, and logout in `docs/runbooks/auth-e2e.md`.
- [ ] 6.4 Complete pending cleanup docs in `README.md`, `docs/mvp-boundaries.md`, `docs/runbooks/reconciliation-exceptions.md`, and `docs/runbooks/receipt-issuance.md`.
