# Proposal: Animal Foundation Platform Colombia

## Intent
Deliver a trustworthy MVP for Colombian animal foundations to manage donors and donations end-to-end, reduce reconciliation errors, and improve transparency/compliance for donor retention.

## Scope

### In Scope
- Managed authentication and role-based access (no custom auth core).
- Hybrid donation intake: online gateway payments plus offline/manual receipt registration.
- Reconciliation workflow linking funds and receipts to donor records.
- Donor dashboard with donation history, recurring status, and receipt/comprobante access.
- Recurring monthly donor lifecycle (enroll, status, cancellation).
- Public donor recognition with explicit, auditable consent visibility.

### Out of Scope
- Social-media automation, post generation, scheduling, or scraping in MVP.
- Advanced BI analytics and multi-country tax engine.

## Capabilities

### New Capabilities
- `identity-and-access`: Managed authentication, session security, and RBAC.
- `donation-intake-hybrid`: Online and offline donation capture workflows.
- `donation-reconciliation`: Confirmation and matching of funds to donors.
- `donor-dashboard`: Self-service donor visibility and receipt access.
- `recurring-donor-program`: Monthly donation subscription lifecycle.
- `donor-consent-public-recognition`: Auditable consent for public name display.
- `tax-receipt-compliance-co`: DIAN-aligned receipt/comprobante generation boundaries.

### Modified Capabilities
- None.

## Approach
Use a modular monolith with .NET 10 backend + PostgreSQL and React/Vite frontend with MUI. Integrate a managed IdP for security, define payment-provider adapter boundaries, and model reconciliation with explicit states that support online and offline flows.

## Affected Areas

| Area | Impact | Description |
|------|--------|-------------|
| `Backend/` | New | .NET 10 services for identity, donations, reconciliation, receipts |
| `Frontend/` | New | React donor/admin interfaces built with MUI |
| `Backend/Integrations/Payments` | New | Payment gateway adapter boundary |
| `Backend/Compliance/TaxReceipts` | New | DIAN-oriented receipt/comprobante logic |

## Risks

| Risk | Likelihood | Mitigation |
|------|------------|------------|
| Gateway/regulatory mismatch | Med | Adapter contract + sandbox validation first |
| Consent audit gaps | Med | Immutable consent events and admin audit trail |
| Reconciliation complexity | Med | Explicit state model + manual override controls |

## Rollback Plan
Feature-flag online recurring and gateway paths. If integrations fail, disable gateway adapters and continue offline donation + reconciliation as system of record without data loss.

## Dependencies
- Managed identity provider.
- At least one Colombian-compatible payment gateway.
- Accounting/legal validation for DIAN-required receipt fields.

## Success Criteria
- [ ] 100% of recorded funds are linked to donor records through reconciliation.
- [ ] Donors can access donation history and receipts/comprobantes in dashboard.
- [ ] Recurring enrollment and cancellation work without manual database edits.
- [ ] Public donor listings only show explicitly consented donors with audit history.
