import { render, screen } from '@testing-library/react'
import { ThemeProvider, createTheme } from '@mui/material'
import { MemoryRouter } from 'react-router-dom'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import App from './App'
import { useAuthBootstrap } from './app/auth/authBootstrapContext'

vi.mock('./app/auth/authBootstrapContext', () => ({
  useAuthBootstrap: vi.fn(),
}))

vi.mock('./app/theme/ThemeControls', () => ({
  ThemeControls: () => <div data-testid="theme-controls" />,
}))

vi.mock('./features/landing/LandingPage', () => ({
  LandingPage: () => <div>Landing route content</div>,
}))

vi.mock('./features/donations/DonationFormPage', () => ({
  DonationFormPage: () => <div>Donation form route</div>,
}))

vi.mock('./features/reconciliation/ReconciliationQueuePage', () => ({
  ReconciliationQueuePage: () => <div>Reconciliation route</div>,
}))

vi.mock('./features/dashboard/DonorDashboardPage', () => ({
  DonorDashboardPage: () => <div>Dashboard route</div>,
}))

vi.mock('./features/animal-cases/AnimalCaseTimelinePage', () => ({
  AnimalCaseTimelinePage: () => <div>Animal cases route</div>,
}))

const mockedUseAuthBootstrap = vi.mocked(useAuthBootstrap)

function renderApp(entry: string) {
  return render(
    <ThemeProvider theme={createTheme()}>
      <MemoryRouter initialEntries={[entry]}>
        <App />
      </MemoryRouter>
    </ThemeProvider>,
  )
}

describe('App routing parity with lazy app routes', () => {
  beforeEach(() => {
    mockedUseAuthBootstrap.mockReset()
  })

  it('keeps public landing route eager', async () => {
    mockedUseAuthBootstrap.mockReturnValue({
      state: { status: 'anonymous', identity: null, errorMessage: null },
      retry: vi.fn(async () => {}),
      logout: vi.fn(async () => {}),
    })

    renderApp('/')

    expect(await screen.findByText('Landing route content')).toBeInTheDocument()
    expect(screen.queryByRole('progressbar', { name: 'Loading route content' })).not.toBeInTheDocument()
  })

  it('preserves protected route outcome for anonymous users', async () => {
    mockedUseAuthBootstrap.mockReturnValue({
      state: { status: 'anonymous', identity: null, errorMessage: null },
      retry: vi.fn(async () => {}),
      logout: vi.fn(async () => {}),
    })

    renderApp('/app/dashboard')

    expect(await screen.findByText('Landing route content')).toBeInTheDocument()
  })

  it('loads protected route content for authenticated users', async () => {
    mockedUseAuthBootstrap.mockReturnValue({
      state: {
        status: 'authenticated',
        identity: { id: 'subject-1', displayName: 'operator', email: null, roles: ['operator'] },
        errorMessage: null,
      },
      retry: vi.fn(async () => {}),
      logout: vi.fn(async () => {}),
    })

    renderApp('/app/reconciliation')

    expect(await screen.findByText('Reconciliation route')).toBeInTheDocument()
  })
})
