import {
  Alert,
  Divider,
  List,
  ListItem,
  ListItemText,
  Paper,
  Stack,
  TextField,
  Typography,
} from '@mui/material'
import { useMemo, useState } from 'react'
import type { AnimalCaseTimelineEvent, DonationTimelineEvent } from '../../services/api/types'

const sampleDonationTimeline: DonationTimelineEvent[] = [
  {
    donationId: '11111111-1111-1111-1111-111111111111',
    type: 'created',
    at: '2026-06-01T13:00:00Z',
    actorId: 'staff-intake-01',
  },
  {
    donationId: '11111111-1111-1111-1111-111111111111',
    type: 'confirmed',
    at: '2026-06-02T10:00:00Z',
    actorId: 'staff-reconciliation-01',
  },
  {
    donationId: '11111111-1111-1111-1111-111111111111',
    type: 'receipt_issued',
    at: '2026-06-02T11:00:00Z',
    actorId: 'staff-receipts-01',
  },
]

const sampleAnimalCaseTimeline: AnimalCaseTimelineEvent[] = [
  {
    caseId: 'a6f9fb65-0c89-4de2-a5dc-bf69653ad8b0',
    type: 'opened',
    at: '2026-06-01T09:30:00Z',
    actorId: 'case-worker-01',
  },
  {
    caseId: 'a6f9fb65-0c89-4de2-a5dc-bf69653ad8b0',
    type: 'medical_update',
    at: '2026-06-02T16:15:00Z',
    actorId: 'vet-01',
  },
  {
    caseId: 'a6f9fb65-0c89-4de2-a5dc-bf69653ad8b0',
    type: 'status_changed',
    at: '2026-06-03T14:20:00Z',
    actorId: 'case-worker-01',
  },
]

function matches(input: string, value: string): boolean {
  return !input.trim() || value.toLowerCase().includes(input.trim().toLowerCase())
}

export function AnimalCaseTimelinePage() {
  const [donationFilter, setDonationFilter] = useState('')
  const [caseFilter, setCaseFilter] = useState('')

  const donationEvents = useMemo(
    () => sampleDonationTimeline.filter((event) => matches(donationFilter, event.donationId)),
    [donationFilter],
  )
  const animalCaseEvents = useMemo(
    () => sampleAnimalCaseTimeline.filter((event) => matches(caseFilter, event.caseId)),
    [caseFilter],
  )

  return (
    <Stack spacing={3}>
      <Paper variant="outlined" sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h5">Animal case timeline</Typography>
          <Typography color="text.secondary">
            Displays independent timeline views for donations and animal cases to preserve audit semantics.
          </Typography>
          <Alert severity="info">
            This page renders separated timeline models as required by design. Live API query wiring can be added when case endpoints are exposed.
          </Alert>
        </Stack>
      </Paper>

      <Paper variant="outlined" sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h6">Donation timeline events</Typography>
          <TextField
            label="Filter by donation ID"
            value={donationFilter}
            onChange={(event) => setDonationFilter(event.target.value)}
            fullWidth
          />
          <List>
            {donationEvents.map((event) => (
              <ListItem key={`${event.donationId}-${event.at}-${event.type}`} divider>
                <ListItemText
                  primary={`${event.type} · ${new Date(event.at).toLocaleString()}`}
                  secondary={`Donation ${event.donationId} · Actor ${event.actorId ?? 'system'}`}
                />
              </ListItem>
            ))}
            {!donationEvents.length ? (
              <ListItem>
                <ListItemText primary="No donation timeline events for this filter." />
              </ListItem>
            ) : null}
          </List>
        </Stack>
      </Paper>

      <Divider />

      <Paper variant="outlined" sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h6">Animal case timeline events</Typography>
          <TextField
            label="Filter by case ID"
            value={caseFilter}
            onChange={(event) => setCaseFilter(event.target.value)}
            fullWidth
          />
          <List>
            {animalCaseEvents.map((event) => (
              <ListItem key={`${event.caseId}-${event.at}-${event.type}`} divider>
                <ListItemText
                  primary={`${event.type} · ${new Date(event.at).toLocaleString()}`}
                  secondary={`Case ${event.caseId} · Actor ${event.actorId}`}
                />
              </ListItem>
            ))}
            {!animalCaseEvents.length ? (
              <ListItem>
                <ListItemText primary="No animal case timeline events for this filter." />
              </ListItem>
            ) : null}
          </List>
        </Stack>
      </Paper>
    </Stack>
  )
}
