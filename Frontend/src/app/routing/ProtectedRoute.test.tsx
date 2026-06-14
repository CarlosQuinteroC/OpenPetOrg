import { fireEvent, render, screen } from '@testing-library/react'
import { MemoryRouter, Route, Routes, useLocation } from 'react-router-dom'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { ProtectedRoute } from './ProtectedRoute'
import { useAuthBootstrap } from '../auth/authBootstrapContext'

vi.mock('../auth/authBootstrapContext', () => ({
  useAuthBootstrap: vi.fn(),
}))

const mockedUseAuthBootstrap = vi.mocked(useAuthBootstrap)

describe('ProtectedRoute', () => {
  beforeEach(() => {
    mockedUseAuthBootstrap.mockReset()
  })

  it('redirects anonymous users from /app to landing with safe next parameter', async () => {
    mockedUseAuthBootstrap.mockReturnValue({
      state: { status: 'anonymous', identity: null, errorMessage: null },
      retry: vi.fn(async () => {}),
      logout: vi.fn(async () => {}),
    })

    function LandingProbe() {
      const location = useLocation()
      return <div>{`Landing ${location.search}`}</div>
    }

    render(
      <MemoryRouter initialEntries={['/app/reconciliation?tab=open']}>
        <Routes>
          <Route path="/" element={<LandingProbe />} />
          <Route path="/app" element={<ProtectedRoute />}>
            <Route path="reconciliation" element={<div>Protected content</div>} />
          </Route>
        </Routes>
      </MemoryRouter>,
    )

    expect(await screen.findByText(/Landing/)).toBeInTheDocument()
    expect(screen.getByText(/next=%2Fapp%2Freconciliation%3Ftab%3Dopen/)).toBeInTheDocument()
  })

  it('renders protected outlet for authenticated users', async () => {
    mockedUseAuthBootstrap.mockReturnValue({
      state: {
        status: 'authenticated',
        identity: { id: 'sub-1', displayName: 'sub-1', email: null, roles: ['operator'] },
        errorMessage: null,
      },
      retry: vi.fn(async () => {}),
      logout: vi.fn(async () => {}),
    })

    render(
      <MemoryRouter initialEntries={['/app/reconciliation']}>
        <Routes>
          <Route path="/app" element={<ProtectedRoute />}>
            <Route path="reconciliation" element={<div>Protected content</div>} />
          </Route>
        </Routes>
      </MemoryRouter>,
    )

    expect(await screen.findByText('Protected content')).toBeInTheDocument()
  })

  it('shows loading state feedback while auth bootstrap is in progress', () => {
    mockedUseAuthBootstrap.mockReturnValue({
      state: { status: 'loading', identity: null, errorMessage: null },
      retry: vi.fn(async () => {}),
      logout: vi.fn(async () => {}),
    })

    render(
      <MemoryRouter initialEntries={['/app/dashboard']}>
        <Routes>
          <Route path="/app" element={<ProtectedRoute />}>
            <Route path="dashboard" element={<div>Protected content</div>} />
          </Route>
        </Routes>
      </MemoryRouter>,
    )

    expect(screen.getByRole('progressbar', { name: 'Loading session' })).toBeInTheDocument()
    expect(screen.getByText('Checking your session...')).toBeInTheDocument()
  })

  it('shows error state with retry action when auth bootstrap fails', () => {
    const retry = vi.fn(async () => {})
    mockedUseAuthBootstrap.mockReturnValue({
      state: { status: 'error', identity: null, errorMessage: 'Session validation failed.' },
      retry,
      logout: vi.fn(async () => {}),
    })

    render(
      <MemoryRouter initialEntries={['/app/dashboard']}>
        <Routes>
          <Route path="/app" element={<ProtectedRoute />}>
            <Route path="dashboard" element={<div>Protected content</div>} />
          </Route>
        </Routes>
      </MemoryRouter>,
    )

    fireEvent.click(screen.getByRole('button', { name: 'Retry' }))

    expect(screen.getByText('Session validation failed.')).toBeInTheDocument()
    expect(retry).toHaveBeenCalledTimes(1)
  })
})
