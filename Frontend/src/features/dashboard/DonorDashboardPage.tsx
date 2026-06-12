import {
  Alert,
  Button,
  Chip,
  Divider,
  Grid,
  List,
  ListItem,
  ListItemText,
  Paper,
  Stack,
  Switch,
  TextField,
  Typography,
} from '@mui/material'
import { useMemo, useState } from 'react'
import { useApi } from '../../services/api/useApi'
import type { DonationDto, RecurringDonationDto } from '../../services/api/types'

type DashboardState = {
  donorId: string
  visibilityAt: string
  includePublicRecognition: boolean
}

const initialState: DashboardState = {
  donorId: '',
  visibilityAt: new Date().toISOString(),
  includePublicRecognition: true,
}

export function DonorDashboardPage() {
  const api = useApi()
  const [filters, setFilters] = useState<DashboardState>(initialState)
  const [donationIdInput, setDonationIdInput] = useState('')
  const [recurringIdInput, setRecurringIdInput] = useState('')
  const [recurringAmount, setRecurringAmount] = useState('60000')
  const [recurringCurrency, setRecurringCurrency] = useState('COP')
  const [recurringStartDate, setRecurringStartDate] = useState('2026-06-01')
  const [cancelledOnDate, setCancelledOnDate] = useState('2026-06-30')
  const [donations, setDonations] = useState<DonationDto[]>([])
  const [recurring, setRecurring] = useState<RecurringDonationDto[]>([])
  const [visibility, setVisibility] = useState<boolean | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [message, setMessage] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(false)

  const confirmedDonations = useMemo(
    () => donations.filter((entry) => entry.reconciliationStatus === 'confirmed'),
    [donations],
  )

  const loadDashboard = async () => {
    setError(null)
    setMessage(null)
    setVisibility(null)

    if (!filters.donorId.trim()) {
      setError('Donor ID is required to load dashboard data.')
      return
    }

    const donationIds = donationIdInput
      .split(/[\s,]+/)
      .map((value) => value.trim())
      .filter(Boolean)
    setIsLoading(true)
    try {
      const donationResults = await Promise.all(
        donationIds.map(async (id) => {
          const donation = await api.getDonationById(id)
          return donation.donorId === filters.donorId.trim() ? donation : null
        }),
      )
      setDonations(donationResults.filter((entry): entry is DonationDto => Boolean(entry)))

      if (filters.includePublicRecognition) {
        const response = await api.getPublicRecognitionVisibility(filters.donorId.trim(), filters.visibilityAt)
        setVisibility(response.isVisible)
      }
    } catch {
      setError('Failed to load donor dashboard data from API contracts.')
    } finally {
      setIsLoading(false)
    }
  }

  const enrollRecurring = async () => {
    setError(null)
    setMessage(null)

    if (!filters.donorId.trim()) {
      setError('Donor ID is required before enrolling recurring donations.')
      return
    }

    try {
      const response = await api.enrollRecurring({
        donorId: filters.donorId.trim(),
        amount: Number(recurringAmount),
        currency: recurringCurrency,
        startedOn: recurringStartDate,
      })
      setRecurring((previous) => [response, ...previous])
      setMessage(`Recurring enrollment created: ${response.recurringDonationId}`)
    } catch {
      setError('Unable to enroll recurring donation.')
    }
  }

  const cancelRecurring = async () => {
    setError(null)
    setMessage(null)

    if (!recurringIdInput.trim()) {
      setError('Recurring ID is required to cancel.')
      return
    }

    try {
      const response = await api.cancelRecurring(recurringIdInput.trim(), cancelledOnDate)
      setRecurring((previous) => {
        const updated = previous.filter((entry) => entry.recurringDonationId !== response.recurringDonationId)
        return [response, ...updated]
      })
      setMessage(`Recurring donation cancelled: ${response.recurringDonationId}`)
    } catch {
      setError('Unable to cancel recurring donation.')
    }
  }

  const markRecurringFailed = async () => {
    setError(null)
    setMessage(null)

    if (!recurringIdInput.trim()) {
      setError('Recurring ID is required to mark payment failed.')
      return
    }

    try {
      const response = await api.markRecurringPaymentFailed(recurringIdInput.trim())
      setRecurring((previous) => {
        const updated = previous.filter((entry) => entry.recurringDonationId !== response.recurringDonationId)
        return [response, ...updated]
      })
      setMessage(`Recurring payment marked as failed: ${response.recurringDonationId}`)
    } catch {
      setError('Unable to mark recurring payment as failed.')
    }
  }

  return (
    <Stack spacing={3}>
      <Paper variant="outlined" sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h5">Donor dashboard</Typography>
          <Typography color="text.secondary">
            Shows donor-specific transparency data: donation status, recurring lifecycle snapshot, receipt readiness, and consent visibility.
          </Typography>

          <Grid container spacing={2}>
            <Grid size={{ xs: 12, md: 6 }}>
              <TextField
                label="Donor ID"
                value={filters.donorId}
                onChange={(event) => setFilters((prev) => ({ ...prev, donorId: event.target.value }))}
                fullWidth
                required
              />
            </Grid>
            <Grid size={{ xs: 12, md: 6 }}>
              <TextField
                label="Visibility snapshot (ISO datetime)"
                value={filters.visibilityAt}
                onChange={(event) => setFilters((prev) => ({ ...prev, visibilityAt: event.target.value }))}
                fullWidth
              />
            </Grid>
            <Grid size={{ xs: 12, md: 6 }}>
              <TextField
                label="Donation IDs (for donor history)"
                value={donationIdInput}
                onChange={(event) => setDonationIdInput(event.target.value)}
                helperText="IDs separated by comma/space. Only this donor's records are shown."
                fullWidth
                multiline
                minRows={2}
              />
            </Grid>
            <Grid size={{ xs: 12, md: 6 }}>
              <TextField
                label="Recurring IDs (optional lifecycle checks)"
                value={recurringIdInput}
                onChange={(event) => setRecurringIdInput(event.target.value)}
                helperText="Single recurring ID for cancel/fail actions"
                fullWidth
              />
            </Grid>
            <Grid size={{ xs: 12, md: 3 }}>
              <TextField
                label="Recurring amount"
                type="number"
                value={recurringAmount}
                onChange={(event) => setRecurringAmount(event.target.value)}
                fullWidth
              />
            </Grid>
            <Grid size={{ xs: 12, md: 3 }}>
              <TextField
                label="Recurring currency"
                value={recurringCurrency}
                onChange={(event) => setRecurringCurrency(event.target.value)}
                fullWidth
              />
            </Grid>
            <Grid size={{ xs: 12, md: 3 }}>
              <TextField
                label="Start date"
                type="date"
                value={recurringStartDate}
                onChange={(event) => setRecurringStartDate(event.target.value)}
                fullWidth
                slotProps={{
                  inputLabel: {
                    shrink: true,
                  },
                }}
              />
            </Grid>
            <Grid size={{ xs: 12, md: 3 }}>
              <TextField
                label="Cancellation date"
                type="date"
                value={cancelledOnDate}
                onChange={(event) => setCancelledOnDate(event.target.value)}
                fullWidth
                slotProps={{
                  inputLabel: {
                    shrink: true,
                  },
                }}
              />
            </Grid>
          </Grid>

          <Stack direction="row" spacing={1} alignItems="center">
            <Switch
              checked={filters.includePublicRecognition}
              onChange={(event) =>
                setFilters((prev) => ({
                  ...prev,
                  includePublicRecognition: event.target.checked,
                }))
              }
            />
            <Typography>Load consent visibility snapshot</Typography>
          </Stack>

          <Button variant="contained" onClick={loadDashboard} disabled={isLoading}>
            {isLoading ? 'Loading dashboard...' : 'Load dashboard'}
          </Button>

          <Stack direction="row" spacing={1} flexWrap="wrap">
            <Button variant="outlined" onClick={enrollRecurring}>
              Enroll recurring
            </Button>
            <Button variant="outlined" color="warning" onClick={markRecurringFailed}>
              Mark recurring failed
            </Button>
            <Button variant="outlined" color="error" onClick={cancelRecurring}>
              Cancel recurring
            </Button>
          </Stack>
        </Stack>
      </Paper>

      {error ? <Alert severity="error">{error}</Alert> : null}
      {message ? <Alert severity="success">{message}</Alert> : null}

      <Paper variant="outlined" sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h6">Donation history</Typography>
          {!donations.length ? (
            <Typography color="text.secondary">No donor donation records loaded yet.</Typography>
          ) : (
            <List>
              {donations.map((donation) => (
                <ListItem key={donation.donationId} divider>
                  <ListItemText
                    primary={`${donation.amount.toLocaleString()} ${donation.currency} · ${donation.channel}`}
                    secondary={`Donation ${donation.donationId} · ${new Date(donation.occurredAt).toLocaleString()}`}
                  />
                  <Chip label={donation.reconciliationStatus} size="small" />
                </ListItem>
              ))}
            </List>
          )}

          <Divider />

          <Typography variant="h6">Receipt availability</Typography>
          {!confirmedDonations.length ? (
            <Typography color="text.secondary">
              No confirmed donations available. Final receipts remain hidden until reconciliation is confirmed.
            </Typography>
          ) : (
            <List>
              {confirmedDonations.map((donation) => (
                <ListItem key={`${donation.donationId}-receipt`}>
                  <ListItemText
                    primary={`Receipt eligible donation ${donation.donationId}`}
                    secondary="This donation can request final DIAN-oriented receipt issuance."
                  />
                </ListItem>
              ))}
            </List>
          )}

          <Divider />

          <Typography variant="h6">Recurring status</Typography>
          {!recurring.length ? (
            <Typography color="text.secondary">No recurring records loaded for this donor context.</Typography>
          ) : (
            <List>
              {recurring.map((entry) => (
                <ListItem key={entry.recurringDonationId}>
                  <ListItemText
                    primary={`Recurring ${entry.recurringDonationId}`}
                    secondary={`${entry.status} · started ${entry.startedOn}${entry.cancelledOn ? ` · cancelled ${entry.cancelledOn}` : ''}`}
                  />
                </ListItem>
              ))}
            </List>
          )}

          <Divider />

          <Typography variant="h6">Public recognition consent</Typography>
          {visibility === null ? (
            <Typography color="text.secondary">Visibility snapshot not loaded.</Typography>
          ) : visibility ? (
            <Alert severity="success">Donor is currently visible for public recognition.</Alert>
          ) : (
            <Alert severity="info">Donor is not publicly visible without explicit consent.</Alert>
          )}
        </Stack>
      </Paper>
    </Stack>
  )
}
