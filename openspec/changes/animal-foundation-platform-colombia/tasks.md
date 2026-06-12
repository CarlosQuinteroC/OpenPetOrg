# Tasks: Animal Foundation Platform Colombia

## Review Workload Forecast

| Field | Value |
|-------|-------|
| Estimated changed lines | 1,800–2,500 |
| 400-line budget risk | High |
| Chained PRs recommended | Yes |
| Suggested split | PR 1 → PR 2 → PR 3 → PR 4 → PR 5 |
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
| 2 | Backend domain + API layering (controllers/services/repositories + DTOs + EF) | PR 2 | Base = PR 1 branch; backend only |
| 3 | Backend verification + Swagger + Postman artifacts | PR 3 | Base = PR 2 branch; backend docs/tests gate |
| 4 | Frontend integration against documented API contracts | PR 4 | Base = PR 3 branch; starts only after Unit 3 |
| 5 | Final docs/runbooks and MVP boundary hardening | PR 5 | Base = PR 4 branch |

## Phase 1: Foundation / Infrastructure (Completed PR1)

- [x] 1.1 Create `Backend/PetOrg.sln` with `Backend/src/PetOrg.Api`, `PetOrg.Modules.*`, and `PetOrg.Infrastructure.Persistence` projects.
- [x] 1.2 Implement `Backend/src/PetOrg.Api/Program.cs` with managed IdP JWT auth and Donor/Staff authorization policies.
- [x] 1.3 Add `Backend/src/PetOrg.Infrastructure.Persistence/PetOrgDbContext.cs` and EF mappings for donations, recurring, consent, receipts, and animal cases.
- [x] 1.4 Add `Backend/src/PetOrg.Infrastructure.Persistence/Entities/DonationTimelineEvent.cs` and `.../AnimalCaseTimelineEvent.cs`.
- [x] 1.5 Create `Backend/src/PetOrg.Infrastructure.Persistence/Migrations/*_InitialMvp.cs` and `Backend/scripts/dev-up.ps1`.
- [x] 1.6 Bootstrap frontend baseline in `Frontend/package.json`, `Frontend/vite.config.ts`, `Frontend/src/main.tsx`, and `Frontend/src/app/providers.tsx`.

## Phase 2: Backend Core Implementation (PR2)

- [x] 2.1 Implement donation controller/service/repository + DTO binding in `Backend/src/PetOrg.Modules.Donations/{Endpoints,Application,Infrastructure,Contracts}/`.
- [x] 2.2 Implement reconciliation controller/service/repository flow in `Backend/src/PetOrg.Modules.Reconciliation/` for unique-match confirm and ambiguous exceptions.
- [x] 2.3 Implement recurring lifecycle handlers in `Backend/src/PetOrg.Modules.Recurring/Application/` and persistence updates in `PetOrg.Infrastructure.Persistence`.
- [x] 2.4 Implement consent audit and donor recognition visibility flow in `Backend/src/PetOrg.Modules.Consent/` with immutable history.
- [x] 2.5 Implement receipt final-issuance gate in `Backend/src/PetOrg.Modules.Receipts/Application/IssueFinalReceiptHandler.cs` (confirmed-only rule).

## Phase 3: Backend Testing + API Documentation Gate (PR3)

- [x] 3.1 Add integration tests in `Backend/tests/PetOrg.IntegrationTests/{Auth,Donations,Reconciliation}/` for spec scenarios (access control, hybrid intake, ambiguous match).
- [x] 3.2 Add unit tests in `Backend/tests/PetOrg.UnitTests/{Recurring,Consent,Receipts,Timeline}/` for lifecycle, consent audit, receipt gate, and timeline separation.
- [x] 3.3 Integrate Swagger in `Backend/src/PetOrg.Api/Program.cs` and `Backend/src/PetOrg.Api/Swagger/` with endpoint examples and auth scheme.
- [x] 3.4 Publish Postman artifacts in `docs/api/postman/animal-foundation-platform-colombia.postman_collection.json` and `docs/api/postman/README.md` from tested endpoints.
- [x] 3.5 Gate: mark frontend start only after `dotnet test` passes and Swagger + Postman docs are reviewed.

## Phase 4: Frontend Integration After Backend Baseline (PR4)

- [x] 4.1 Implement `Frontend/src/features/donations/DonationFormPage.tsx` using documented donation endpoint contracts.
- [x] 4.2 Implement `Frontend/src/features/reconciliation/ReconciliationQueuePage.tsx` and `dashboard/DonorDashboardPage.tsx` against backend DTOs.
- [x] 4.3 Implement `Frontend/src/features/animal-cases/AnimalCaseTimelinePage.tsx` with separated timeline views.
- [x] 4.4 Add `Frontend/src/services/api/` typed clients aligned with Swagger schema and Postman examples.

## Phase 5: Cleanup / Documentation (PR5)

- [ ] 5.1 Update `README.md` and `docs/mvp-boundaries.md` with backend-first workflow and social automation exclusion.
- [ ] 5.2 Add `docs/runbooks/reconciliation-exceptions.md` and `docs/runbooks/receipt-issuance.md` with operator verification steps.
