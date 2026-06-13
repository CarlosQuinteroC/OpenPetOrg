# PetOrg - Animal Foundation Platform Colombia (MVP)

Open-source fullstack platform for small animal foundations to manage donors, donations, reconciliation, recurring contributions, consent visibility, and receipt issuance.

## Quick Start

1. Start backend (`Backend/`) and frontend (`Frontend/`).
2. For auth-enabled flow, bootstrap Keycloak: `docs/auth/keycloak-dev-setup.md`.
3. Verify API/auth baseline with Swagger and Postman docs in `docs/api/`.

## Documentation Map

- `docs/mvp-boundaries.md` - MVP in-scope and out-of-scope boundaries.
- `docs/auth/keycloak-dev-setup.md` - local Keycloak bootstrap and seeded users.
- `docs/auth/backend-role-mapping.md` - backend token/role contract.
- `docs/api/README.md` - Swagger auth verification path.
- `docs/api/postman/README.md` - Postman verification with Keycloak token flow.
- `docs/runbooks/auth-e2e.md` - minimal end-to-end auth verification steps and evidence.
- `docs/runbooks/reconciliation-exceptions.md` - operator path for ambiguous reconciliation.
- `docs/runbooks/receipt-issuance.md` - final receipt issuance gate checks.

## MVP Guardrails

- Managed IdP authentication only (no local password auth in MVP).
- Final receipt issuance only for reconciliation-confirmed donations.
- Social automation is out of MVP scope.
