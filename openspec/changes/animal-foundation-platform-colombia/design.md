# Design: Animal Foundation Platform Colombia

## Technical Approach
Implement the MVP as a **.NET 10 modular monolith** (single API + PostgreSQL) with a **React + Vite + MUI** SPA. This directly maps to approved proposal/spec scope: managed IdP auth, hybrid donation intake, reconciliation-first receipt gating, donor transparency dashboard, recurring lifecycle, and auditable consent visibility. Social automation remains explicitly out of scope.

This approach prioritizes low operating cost and maintainability for small foundation teams by minimizing deployable units while keeping domain boundaries explicit.

## Architecture Decisions
| Option | Tradeoff | Decision |
|---|---|---|
| MUI component system vs shadcn/ui assembly | MUI speeds delivery for accessible, data-heavy screens; shadcn/ui adds customization flexibility with higher implementation overhead | Use **MUI** for MVP; keep feature-level UI boundaries to allow selective future replacement |
| Managed IdP vs custom password/JWT auth | Managed IdP reduces security/compliance burden and support overhead; custom auth increases attack surface and long-term maintenance | Use **managed IdP only**; no local-password auth in MVP |
| Single donation ledger with channel field vs separate online/offline ledgers | Single ledger simplifies reconciliation, reporting, and receipt issuance state; split stores increase synchronization complexity | Use **single donation ledger** with `channel` and reconciliation status |
| Final DIAN-oriented receipt at creation vs after reconciliation confirmation | Early final issuance risks non-compliant or incorrect receipts | Allow draft/pre-receipt, but **final issuance only when donation is reconciliation-confirmed** |
| Unified timeline table vs separate donation/case timelines | Unified timeline is simpler to start but causes mixed semantics and harder auditing | Keep **separate timeline event models** for donations and animal cases; compose read models at query layer |

## Data Flow
### Sequence: Donation Intake to Final Receipt
```text
Donor/Admin UI -> API Donations: create donation (online/offline)
API Donations -> PostgreSQL: store donation (status=pending)
Reconciliation Worker/Staff -> API Reconciliation: match donation to donor
API Reconciliation -> PostgreSQL: set status=confirmed|exception
Donor/Admin UI -> API Receipts: request final receipt
API Receipts -> PostgreSQL: verify status=confirmed
API Receipts -> Donor/Admin UI: issue final receipt (or block if not confirmed)
```

### Runtime Topology
```text
React SPA (MUI)
   |
   v
.NET 10 API (modules: Identity, Donors, Donations, Reconciliation, Recurring, Consent, Receipts, AnimalCases)
   |
   v
PostgreSQL (single DB, modular schema boundaries)
```

## File Changes
| File | Action | Description |
|---|---|---|
| `Backend/src/PetOrg.Api/PetOrg.Api.csproj` | Create | API host project and dependency roots |
| `Backend/src/PetOrg.Api/Program.cs` | Create | Startup, module registration, auth policy wiring |
| `Backend/src/PetOrg.Modules.Identity/` | Create | Managed IdP integration and role mapping |
| `Backend/src/PetOrg.Modules.Donations/` | Create | Hybrid intake commands/queries and donation ledger |
| `Backend/src/PetOrg.Modules.Reconciliation/` | Create | Matching workflow and exception handling |
| `Backend/src/PetOrg.Modules.Recurring/` | Create | Monthly recurring enrollment/status/cancel lifecycle |
| `Backend/src/PetOrg.Modules.Consent/` | Create | Consent events and auditable visibility decisions |
| `Backend/src/PetOrg.Modules.Receipts/` | Create | Draft/final receipt generation with confirmation gate |
| `Backend/src/PetOrg.Modules.AnimalCases/` | Create | Animal case lifecycle and independent case timeline |
| `Backend/src/PetOrg.Infrastructure.Persistence/` | Create | EF Core context, migrations, persistence abstractions |
| `Backend/tests/PetOrg.*.Tests/` | Create | Unit and integration test projects |
| `Frontend/package.json` | Create | React/Vite/MUI dependencies and scripts |
| `Frontend/src/app/providers.tsx` | Create | Auth and MUI provider composition |
| `Frontend/src/features/donations/` | Create | Intake, status, and donor-facing donation views |
| `Frontend/src/features/reconciliation/` | Create | Staff reconciliation and exception resolution UI |
| `Frontend/src/features/dashboard/` | Create | Donor history, recurring state, receipts |
| `Frontend/src/features/animal-cases/` | Create | Case management with separate timeline views |

## Interfaces / Contracts
```http
POST /api/donations
Body: { donorId, amount, currency, channel: "online"|"offline", reference?, occurredAt }
Returns: { donationId, reconciliationStatus: "pending"|"confirmed"|"exception" }

POST /api/reconciliation/{donationId}/confirm
Body: { matchedDonorId, evidenceNote }

POST /api/donors/{donorId}/consent/public-recognition
Body: { granted: boolean, effectiveAt, actorId }

POST /api/receipts/{donationId}/issue-final
Rule: only allowed when reconciliationStatus == "confirmed"
```

```ts
type DonationTimelineEvent = {
  donationId: string;
  type: "created" | "matched" | "confirmed" | "exception" | "receipt_issued";
  at: string;
  actorId?: string;
};

type AnimalCaseTimelineEvent = {
  caseId: string;
  type: "opened" | "status_changed" | "medical_update" | "adoption_update";
  at: string;
  actorId: string;
};
```

## Testing Strategy
| Layer | What to Test | Approach |
|---|---|---|
| Unit | Reconciliation state transitions, consent visibility rules, receipt gating, recurring status lifecycle | xUnit domain/service tests |
| Integration | IdP token validation, donation persistence, webhook ingestion, reconciliation confirmation boundaries | ASP.NET TestHost + PostgreSQL test container |
| E2E | Donor dashboard isolation, hybrid intake flow, exception resolution UX | Playwright against API + SPA test environment |

## Migration / Rollout
No migration required (greenfield baseline). Roll out in slices: (1) identity + donor base, (2) donations + reconciliation, (3) dashboard + recurring + receipts. Keep social automation disabled during MVP.

## Open Questions
- [ ] Which managed IdP (Auth0, Entra External ID, Clerk) best fits Colombia support and pricing?
- [ ] Which payment gateway adapter should be MVP-first for Colombian operations?
- [ ] Which DIAN receipt fields require legal/accounting sign-off before go-live?
