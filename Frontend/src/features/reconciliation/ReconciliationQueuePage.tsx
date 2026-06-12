import {
  Alert,
  Button,
  Chip,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
  Typography,
} from '@mui/material'
import { useState } from 'react'
import { useApi } from '../../services/api/useApi'
import type { DonationDto } from '../../services/api/types'

function statusColor(status: string): 'default' | 'success' | 'warning' | 'error' {
  if (status === 'confirmed') return 'success'
  if (status === 'pending') return 'warning'
  if (status === 'exception') return 'error'
  return 'default'
}

export function ReconciliationQueuePage() {
  const api = useApi()
  const [donationIdsInput, setDonationIdsInput] = useState('')
  const [matchedDonorId, setMatchedDonorId] = useState('')
  const [rows, setRows] = useState<DonationDto[]>([])
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(false)

  const loadQueue = async () => {
    setError(null)
    setSuccess(null)
    const ids = donationIdsInput
      .split(/[\s,]+/)
      .map((value) => value.trim())
      .filter(Boolean)

    if (!ids.length) {
      setRows([])
      return
    }

    setIsLoading(true)
    try {
      const loaded = await Promise.all(ids.map((id) => api.getDonationById(id)))
      setRows(loaded)
    } catch {
      setError('Failed to load queue entries. Check donation IDs and API connectivity.')
    } finally {
      setIsLoading(false)
    }
  }

  const confirmDonation = async (donationId: string) => {
    setError(null)
    setSuccess(null)

    if (!matchedDonorId.trim()) {
      setError('Matched donor ID is required to confirm reconciliation.')
      return
    }

    try {
      const result = await api.confirmDonationMatch(donationId, {
        matchedDonorId: matchedDonorId.trim(),
        evidenceNote: 'Confirmed from reconciliation queue UI.',
      })

      setRows((previous) =>
        previous.map((row) =>
          row.donationId === donationId
            ? {
                ...row,
                donorId: result.matchedDonorId ?? row.donorId,
                reconciliationStatus: result.reconciliationStatus,
              }
            : row,
        ),
      )
      setSuccess(`Donation ${donationId} confirmed.`)
    } catch {
      setError('Unable to confirm reconciliation for the selected donation.')
    }
  }

  const markAmbiguous = async (donationId: string) => {
    setError(null)
    setSuccess(null)

    if (!matchedDonorId.trim()) {
      setError('Selected donor ID is required to register an ambiguous match.')
      return
    }

    try {
      const result = await api.markDonationAmbiguous(donationId, {
        selectedDonorId: matchedDonorId.trim(),
        resolutionNote: 'Marked as ambiguous from queue for manual follow-up.',
      })

      setRows((previous) =>
        previous.map((row) =>
          row.donationId === donationId
            ? {
                ...row,
                reconciliationStatus: result.reconciliationStatus,
              }
            : row,
        ),
      )
      setSuccess(`Donation ${donationId} moved to exception state.`)
    } catch {
      setError('Unable to move donation to exception state.')
    }
  }

  return (
    <Stack spacing={3}>
      <Paper variant="outlined" sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h5">Reconciliation queue</Typography>
          <Typography color="text.secondary">
            Supports unique-match confirmation and ambiguous exception handling from documented backend endpoints.
          </Typography>

          <TextField
            label="Donation IDs"
            value={donationIdsInput}
            onChange={(event) => setDonationIdsInput(event.target.value)}
            helperText="Paste donation IDs separated by comma or whitespace"
            fullWidth
            multiline
            minRows={2}
          />

          <TextField
            label="Matched/selected donor ID"
            value={matchedDonorId}
            onChange={(event) => setMatchedDonorId(event.target.value)}
            helperText="Required by reconciliation confirm and ambiguous actions"
            fullWidth
          />

          <Button variant="contained" onClick={loadQueue} disabled={isLoading}>
            {isLoading ? 'Loading queue...' : 'Load queue'}
          </Button>
        </Stack>
      </Paper>

      {error ? <Alert severity="error">{error}</Alert> : null}
      {success ? <Alert severity="success">{success}</Alert> : null}

      <Paper variant="outlined">
        <TableContainer>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>Donation ID</TableCell>
                <TableCell>Donor</TableCell>
                <TableCell>Amount</TableCell>
                <TableCell>Status</TableCell>
                <TableCell>Occurred At</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {rows.map((row) => (
                <TableRow key={row.donationId}>
                  <TableCell>{row.donationId}</TableCell>
                  <TableCell>{row.donorId}</TableCell>
                  <TableCell>
                    {row.amount.toLocaleString()} {row.currency}
                  </TableCell>
                  <TableCell>
                    <Chip size="small" label={row.reconciliationStatus} color={statusColor(row.reconciliationStatus)} />
                  </TableCell>
                  <TableCell>{new Date(row.occurredAt).toLocaleString()}</TableCell>
                  <TableCell align="right">
                    <Stack direction="row" spacing={1} justifyContent="flex-end">
                      <Button size="small" variant="outlined" onClick={() => markAmbiguous(row.donationId)}>
                        Mark ambiguous
                      </Button>
                      <Button size="small" variant="contained" onClick={() => confirmDonation(row.donationId)}>
                        Confirm
                      </Button>
                    </Stack>
                  </TableCell>
                </TableRow>
              ))}
              {!rows.length ? (
                <TableRow>
                  <TableCell colSpan={6}>
                    <Typography color="text.secondary">No queue items loaded yet.</Typography>
                  </TableCell>
                </TableRow>
              ) : null}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
    </Stack>
  )
}
