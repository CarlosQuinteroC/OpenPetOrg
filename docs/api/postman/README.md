# Postman API Verification Pack (PR3 Gate)

This folder contains backend API verification artifacts for **PR3**.

## Files

- `animal-foundation-platform-colombia.postman_collection.json`
- `animal-foundation-platform-colombia.postman_environment.json`

## Import

1. Open Postman.
2. Import the collection and environment files.
3. Select environment **PetOrg Local Backend**.

## Required Variables

- `base_url`: API URL (default: `http://localhost:5000`)
- `jwt_token`: bearer token from managed IdP
- `donor_id`: test donor GUID

`donation_id` and `recurring_id` are auto-populated by collection tests after creation calls.

## Suggested Run Order

1. Auth → `Get current user`
2. Donations → `Create donation`
3. Reconciliation → `Confirm unique donor match` or `Mark ambiguous match`
4. Receipts → `Issue final receipt` (after confirmation only)
5. Recurring → `Enroll recurring donor`
6. Consent → `Append public recognition consent` then `Get public recognition visibility`

## Backend Gate Checklist

- `dotnet test Backend/PetOrg.sln` passes.
- Swagger UI available at `/swagger`.
- Collection requests map to implemented backend endpoints.
- Frontend implementation starts only after this backend gate is complete.
