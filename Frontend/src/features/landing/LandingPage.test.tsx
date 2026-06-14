import { fireEvent, render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { ThemeProvider, createTheme } from '@mui/material'
import { LandingPage } from './LandingPage'
import { useAuthClient } from '../../app/auth/useAuthClient'

vi.mock('../../app/auth/useAuthClient', () => ({
  useAuthClient: vi.fn(),
}))

vi.mock('../../app/theme/ThemeControls', () => ({
  ThemeControls: () => <div data-testid="theme-controls" />,
}))

const mockedUseAuthClient = vi.mocked(useAuthClient)

describe('LandingPage', () => {
  const login = vi.fn(async () => {})

  beforeEach(() => {
    login.mockReset()
    mockedUseAuthClient.mockReturnValue({
      isEnabled: () => true,
      login,
      logout: vi.fn(async () => {}),
      getAccessToken: vi.fn(async () => null),
      getCurrentUser: vi.fn(async () => null),
    })
  })

  function renderLanding(entry: string) {
    return render(
      <ThemeProvider theme={createTheme()}>
        <MemoryRouter initialEntries={[entry]}>
          <LandingPage />
        </MemoryRouter>
      </ThemeProvider>,
    )
  }

  it('shows login CTA and routes login to safe /app fallback for unsafe next param', () => {
    renderLanding('/?next=https://evil.example.org')

    const loginButton = screen.getByRole('button', { name: 'Login' })
    fireEvent.click(loginButton)

    expect(login).toHaveBeenCalledWith('/app')
  })

  it('passes safe app path in next param to login', () => {
    renderLanding('/?next=/app/dashboard')

    const loginButton = screen.getByRole('button', { name: 'Login' })
    fireEvent.click(loginButton)

    expect(login).toHaveBeenCalledWith('/app/dashboard')
  })

  it('does not render protected application shell content in anonymous landing state', () => {
    renderLanding('/')

    expect(screen.getByRole('button', { name: 'Login' })).toBeInTheDocument()
    expect(screen.queryByText('Donation Intake')).not.toBeInTheDocument()
    expect(screen.queryByText('Reconciliation Queue')).not.toBeInTheDocument()
    expect(screen.queryByText('Donor Dashboard')).not.toBeInTheDocument()
    expect(screen.queryByText('Animal Foundation Platform Colombia')).not.toBeInTheDocument()
  })
})
