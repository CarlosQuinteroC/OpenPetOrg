import { lazy } from 'react'
import { Box } from '@mui/material'
import { Navigate, Route, Routes } from 'react-router-dom'
import { LandingPage } from './features/landing/LandingPage'
import { AppShell } from './features/shell/AppShell'
import { ProtectedRoute } from './app/routing/ProtectedRoute'

const DonationFormPage = lazy(() => import('./features/donations/DonationFormPage').then((module) => ({ default: module.DonationFormPage })))
const ReconciliationQueuePage = lazy(() => import('./features/reconciliation/ReconciliationQueuePage').then((module) => ({ default: module.ReconciliationQueuePage })))
const DonorDashboardPage = lazy(() => import('./features/dashboard/DonorDashboardPage').then((module) => ({ default: module.DonorDashboardPage })))
const AnimalCaseTimelinePage = lazy(() =>
  import('./features/animal-cases/AnimalCaseTimelinePage').then((module) => ({ default: module.AnimalCaseTimelinePage })),
)

function App() {
  return (
    <Box sx={{ minHeight: '100vh', bgcolor: 'background.default' }}>
      <Routes>
        <Route path="/" element={<LandingPage />} />
        <Route path="/app" element={<ProtectedRoute />}>
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
