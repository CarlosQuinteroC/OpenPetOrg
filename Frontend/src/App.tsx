import { AppBar, Box, Button, Container, Stack, Toolbar, Typography } from '@mui/material'
import { Link, Navigate, Route, Routes, useLocation } from 'react-router-dom'
import { DonationFormPage } from './features/donations/DonationFormPage'
import { ReconciliationQueuePage } from './features/reconciliation/ReconciliationQueuePage'
import { DonorDashboardPage } from './features/dashboard/DonorDashboardPage'
import { AnimalCaseTimelinePage } from './features/animal-cases/AnimalCaseTimelinePage'

const navItems = [
  { to: '/donations/new', label: 'Donation Intake' },
  { to: '/reconciliation', label: 'Reconciliation Queue' },
  { to: '/dashboard', label: 'Donor Dashboard' },
  { to: '/animal-cases/timeline', label: 'Animal Case Timeline' },
]

function Navigation() {
  const location = useLocation()

  return (
    <AppBar position="sticky" color="inherit" elevation={0} sx={{ borderBottom: 1, borderColor: 'divider' }}>
      <Toolbar sx={{ gap: 1, flexWrap: 'wrap' }}>
        <Typography variant="h6" sx={{ mr: 2 }}>
          PetOrg MVP
        </Typography>
        {navItems.map((item) => {
          const isActive = location.pathname.startsWith(item.to)

          return (
            <Button
              key={item.to}
              component={Link}
              to={item.to}
              variant={isActive ? 'contained' : 'text'}
              size="small"
            >
              {item.label}
            </Button>
          )
        })}
      </Toolbar>
    </AppBar>
  )
}

function App() {
  return (
    <Box sx={{ minHeight: '100vh', bgcolor: 'background.default' }}>
      <Navigation />
      <Container maxWidth="lg" sx={{ py: 3 }}>
        <Stack spacing={1} sx={{ mb: 3 }}>
          <Typography variant="h4" component="h1">
            Animal Foundation Platform Colombia
          </Typography>
          <Typography color="text.secondary">
            Frontend integration against backend contracts. MVP scope excludes social automation.
          </Typography>
        </Stack>

        <Routes>
          <Route path="/" element={<Navigate to="/donations/new" replace />} />
          <Route path="/donations/new" element={<DonationFormPage />} />
          <Route path="/reconciliation" element={<ReconciliationQueuePage />} />
          <Route path="/dashboard" element={<DonorDashboardPage />} />
          <Route path="/animal-cases/timeline" element={<AnimalCaseTimelinePage />} />
        </Routes>
      </Container>
    </Box>
  )
}

export default App
