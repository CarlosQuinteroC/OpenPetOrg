import { fireEvent, render, screen } from '@testing-library/react'
import { ThemeProvider, createTheme } from '@mui/material'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { AuthBootstrapProvider } from './AuthBootstrapProvider'
import { ProtectedRoute } from '../routing/ProtectedRoute'
import { AppShell } from '../../features/shell/AppShell'
import { useAuthClient } from './useAuthClient'
import { useApi } from '../../services/api/useApi'

vi.mock('./useAuthClient', () => ({
  useAuthClient: vi.fn(),
}))

vi.mock('../../services/api/useApi', () => ({
  useApi: vi.fn(),
}))

vi.mock('../../app/theme/ThemeControls', () => ({
  ThemeControls: () => <div data-testid="theme-controls" />,
}))

const mockedUseAuthClient = vi.mocked(useAuthClient)
const mockedUseApi = vi.mocked(useApi)

describe('Auth logout flow', () => {
  const logout = vi.fn(async () => {})

  beforeEach(() => {
    logout.mockReset()

    mockedUseAuthClient.mockReturnValue({
      isEnabled: () => true,
      login: vi.fn(async () => {}),
      logout,
      getAccessToken: vi.fn(async () => 'token'),
      getCurrentUser: vi.fn(async () => ({ id: 'subject-1', roles: ['operator'] })),
    })

    mockedUseApi.mockReturnValue(
      {
        getCurrentUser: vi.fn(async () => ({ subject: 'subject-1', email: null, roles: ['operator'] })),
      } as unknown as ReturnType<typeof useApi>,
    )
  })

  it('returns the user to public landing state after logout', async () => {
    render(
      <ThemeProvider theme={createTheme()}>
        <MemoryRouter initialEntries={['/app/dashboard']}>
          <AuthBootstrapProvider>
            <Routes>
              <Route path="/" element={<div>Public landing state</div>} />
              <Route path="/app" element={<ProtectedRoute />}>
                <Route element={<AppShell />}>
                  <Route path="dashboard" element={<div>Protected dashboard</div>} />
                </Route>
              </Route>
            </Routes>
          </AuthBootstrapProvider>
        </MemoryRouter>
      </ThemeProvider>,
    )

    expect(await screen.findByText('Protected dashboard')).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: 'Logout' }))

    expect(logout).toHaveBeenCalledTimes(1)
    expect(await screen.findByText('Public landing state')).toBeInTheDocument()
  })
})
