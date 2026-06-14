import { fireEvent, render, screen } from '@testing-library/react'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { lazy, type ComponentType } from 'react'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { ThemeProvider, createTheme } from '@mui/material'
import { AppShell } from './AppShell'
import { useAuthBootstrap } from '../../app/auth/authBootstrapContext'

vi.mock('../../app/auth/authBootstrapContext', () => ({
  useAuthBootstrap: vi.fn(),
}))

vi.mock('../../app/theme/ThemeControls', () => ({
  ThemeControls: () => <div data-testid="theme-controls" />,
}))

const mockedUseAuthBootstrap = vi.mocked(useAuthBootstrap)

describe('AppShell', () => {
  const logout = vi.fn(async () => {})

  beforeEach(() => {
    logout.mockReset()
    mockedUseAuthBootstrap.mockReturnValue({
      state: {
        status: 'authenticated',
        identity: {
          id: 'subject-1',
          displayName: 'operator',
          email: 'operator@example.org',
          roles: ['operator'],
        },
        errorMessage: null,
      },
      retry: vi.fn(async () => {}),
      logout,
    })
  })

  it('triggers logout from shell top bar action', () => {
    render(
      <ThemeProvider theme={createTheme()}>
        <MemoryRouter initialEntries={['/app/dashboard']}>
          <Routes>
            <Route path="/app" element={<AppShell />}>
              <Route path="dashboard" element={<div>Protected dashboard</div>} />
            </Route>
          </Routes>
        </MemoryRouter>
      </ThemeProvider>,
    )

    fireEvent.click(screen.getByRole('button', { name: 'Logout' }))

    expect(logout).toHaveBeenCalledTimes(1)
  })

  it('shows scoped fallback while lazy route content is loading', async () => {
    const LazyDashboard = lazy(
      async () =>
        new Promise<{ default: ComponentType }>((resolve) => {
          setTimeout(() => {
            resolve({
              default: () => <div>Lazy protected dashboard</div>,
            })
          }, 0)
        }),
    )

    render(
      <ThemeProvider theme={createTheme()}>
        <MemoryRouter initialEntries={['/app/dashboard']}>
          <Routes>
            <Route path="/app" element={<AppShell />}>
              <Route path="dashboard" element={<LazyDashboard />} />
            </Route>
          </Routes>
        </MemoryRouter>
      </ThemeProvider>,
    )

    expect(await screen.findByRole('progressbar', { name: 'Loading route content' })).toBeInTheDocument()
    expect(await screen.findByText('Lazy protected dashboard')).toBeInTheDocument()
  })
})
