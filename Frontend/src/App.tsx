import { Box } from '@mui/material'
import { Navigate, Route, Routes } from 'react-router-dom'
import { DonationFormPage } from './features/donations/DonationFormPage'
import { ReconciliationQueuePage } from './features/reconciliation/ReconciliationQueuePage'
import { DonorDashboardPage } from './features/dashboard/DonorDashboardPage'
import { AnimalCaseTimelinePage } from './features/animal-cases/AnimalCaseTimelinePage'
import { LandingPage } from './features/landing/LandingPage'
import { AppShell } from './features/shell/AppShell'
import { AppRouteBoundary } from './app/routing/AppRouteBoundary'

function App() {
  return (
    <Box sx={{ minHeight: '100vh', bgcolor: 'background.default' }}>
      <Routes>
        <Route path="/" element={<LandingPage />} />
        <Route path="/app" element={<AppRouteBoundary />}>
          <Route element={<AppShell />}>
            <Route index element={<Navigate to="donations/new" replace />} />
            <Route path="donations/new" element={<DonationFormPage />} />
            <Route path="reconciliation" element={<ReconciliationQueuePage />} />
            <Route path="dashboard" element={<DonorDashboardPage />} />
            <Route path="animal-cases/timeline" element={<AnimalCaseTimelinePage />} />
          </Route>
        </Route>
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Box>
  )
}

export default App
