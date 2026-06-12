import { Alert, Button, Grid, MenuItem, Paper, Stack, TextField, Typography } from '@mui/material'
import { type FormEvent, useMemo, useState } from 'react'
import { useApi } from '../../services/api/useApi'
import type { CreateDonationRequest, CreateDonationResponse } from '../../services/api/types'

type DonationFormState = {
  donorId: string
  amount: string
  currency: string
  channel: 'online' | 'offline'
  reference: string
  occurredAt: string
}

const defaultState: DonationFormState = {
  donorId: '',
  amount: '120000',
  currency: 'COP',
  channel: 'online',
  reference: '',
  occurredAt: new Date().toISOString().slice(0, 16),
}

function toIso(value: string): string {
  return new Date(value).toISOString()
}

function toPayload(state: DonationFormState): CreateDonationRequest {
  return {
    donorId: state.donorId,
    amount: Number(state.amount),
    currency: state.currency.trim().toUpperCase(),
    channel: state.channel,
    reference: state.reference.trim() || undefined,
    occurredAt: toIso(state.occurredAt),
  }
}

export function DonationFormPage() {
  const api = useApi()
  const [form, setForm] = useState<DonationFormState>(defaultState)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [result, setResult] = useState<CreateDonationResponse | null>(null)
  const [error, setError] = useState<string | null>(null)

  const canSubmit = useMemo(() => {
    return Boolean(form.donorId.trim()) && Number(form.amount) > 0 && Boolean(form.occurredAt)
  }, [form])

  const onSubmit = async (event: FormEvent) => {
    event.preventDefault()
    setError(null)
    setResult(null)

    if (!canSubmit) {
      setError('Donor, amount, and occurred date are required.')
      return
    }

    setIsSubmitting(true)
    try {
      const response = await api.createDonation(toPayload(form))
      setResult(response)
    } catch {
      setError('Failed to create donation. Verify API availability and request data.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Paper variant="outlined" sx={{ p: 3 }}>
      <Stack spacing={2} component="form" onSubmit={onSubmit}>
        <Typography variant="h5">Donation intake</Typography>
        <Typography color="text.secondary">
          Registers online and offline donations in the unified ledger using the backend donation contract.
        </Typography>

        <Grid container spacing={2}>
          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              label="Donor ID"
              value={form.donorId}
              onChange={(event) => setForm((prev) => ({ ...prev, donorId: event.target.value }))}
              fullWidth
              required
              helperText="Managed identity donor GUID"
            />
          </Grid>
          <Grid size={{ xs: 12, md: 3 }}>
            <TextField
              label="Amount"
              type="number"
              value={form.amount}
              onChange={(event) => setForm((prev) => ({ ...prev, amount: event.target.value }))}
              fullWidth
              required
            />
          </Grid>
          <Grid size={{ xs: 12, md: 3 }}>
            <TextField
              label="Currency"
              value={form.currency}
              onChange={(event) => setForm((prev) => ({ ...prev, currency: event.target.value }))}
              fullWidth
              required
            />
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <TextField
              label="Channel"
              value={form.channel}
              onChange={(event) =>
                setForm((prev) => ({
                  ...prev,
                  channel: event.target.value as 'online' | 'offline',
                }))
              }
              select
              fullWidth
              required
            >
              <MenuItem value="online">Online</MenuItem>
              <MenuItem value="offline">Offline</MenuItem>
            </TextField>
          </Grid>
          <Grid size={{ xs: 12, md: 8 }}>
            <TextField
              label="Reference"
              value={form.reference}
              onChange={(event) => setForm((prev) => ({ ...prev, reference: event.target.value }))}
              fullWidth
              helperText="Gateway or receipt reference"
            />
          </Grid>
          <Grid size={{ xs: 12, md: 6 }}>
            <TextField
              label="Occurred at"
              type="datetime-local"
              value={form.occurredAt}
              onChange={(event) => setForm((prev) => ({ ...prev, occurredAt: event.target.value }))}
              fullWidth
              required
              slotProps={{
                inputLabel: {
                  shrink: true,
                },
              }}
            />
          </Grid>
        </Grid>

        <Stack direction="row" spacing={1}>
          <Button type="submit" variant="contained" disabled={isSubmitting || !canSubmit}>
            {isSubmitting ? 'Saving...' : 'Create donation'}
          </Button>
          <Button type="button" variant="text" onClick={() => setForm(defaultState)}>
            Reset
          </Button>
        </Stack>

        {error ? <Alert severity="error">{error}</Alert> : null}
        {result ? (
          <Alert severity="success">
            Donation created: <strong>{result.donationId}</strong> ({result.reconciliationStatus})
          </Alert>
        ) : null}
      </Stack>
    </Paper>
  )
}
