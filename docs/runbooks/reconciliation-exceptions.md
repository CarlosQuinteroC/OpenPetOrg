# Reconciliation Exceptions Runbook

Operational steps for handling ambiguous donation-to-donor matches.

## Trigger

Use this runbook when reconciliation cannot produce a unique donor match.

## Steps

1. Review donation reference, amount, and timestamps.
2. Compare candidate donor records and evidence quality.
3. Mark donation as ambiguous exception (do not auto-confirm).
4. Record resolution note with chosen donor if manually resolved.
5. Re-run confirmation flow once evidence is sufficient.

## Guardrails

- Never force-confirm without clear donor evidence.
- Keep exception notes auditable and specific.

## Verification

- Ambiguous flow remains in exception state until explicit resolution.
- Receipt final issuance remains blocked while donation is not confirmed.
