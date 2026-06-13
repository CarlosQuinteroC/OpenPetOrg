import { useMemo, useState, type PropsWithChildren } from 'react'
import { CssBaseline, ThemeProvider } from '@mui/material'
import { BrowserRouter } from 'react-router-dom'
import { createAuthClient } from './auth/authClient'
import { ApiProvider } from '../services/api/context'
import { AuthClientContext } from './auth/authContext'
import { AuthBootstrapProvider } from './auth/AuthBootstrapProvider'
import { createAppTheme } from '../theme/createAppTheme'
import {
  DEFAULT_PALETTE_PRESET,
  DEFAULT_THEME_MODE,
  type PalettePreset,
  type ThemeMode,
} from '../theme/tokens'
import { ThemePreferenceContext } from './theme/themePreferenceContext'

const THEME_STORAGE_KEY = 'petorg.theme.preferences.v1'

type StoredThemePreferences = {
  mode: ThemeMode
  preset: PalettePreset
}

function isThemeMode(value: unknown): value is ThemeMode {
  return value === 'light' || value === 'dark'
}

function isPalettePreset(value: unknown): value is PalettePreset {
  return value === 'classic' || value === 'forest' || value === 'ocean'
}

function readStoredPreferences(): StoredThemePreferences {
  if (typeof window === 'undefined') {
    return {
      mode: DEFAULT_THEME_MODE,
      preset: DEFAULT_PALETTE_PRESET,
    }
  }

  const raw = window.localStorage.getItem(THEME_STORAGE_KEY)

  if (!raw) {
    return {
      mode: DEFAULT_THEME_MODE,
      preset: DEFAULT_PALETTE_PRESET,
    }
  }

  try {
    const parsed = JSON.parse(raw) as Partial<StoredThemePreferences>
    return {
      mode: isThemeMode(parsed.mode) ? parsed.mode : DEFAULT_THEME_MODE,
      preset: isPalettePreset(parsed.preset) ? parsed.preset : DEFAULT_PALETTE_PRESET,
    }
  } catch {
    return {
      mode: DEFAULT_THEME_MODE,
      preset: DEFAULT_PALETTE_PRESET,
    }
  }
}

function persistPreferences(next: StoredThemePreferences) {
  if (typeof window === 'undefined') {
    return
  }

  window.localStorage.setItem(THEME_STORAGE_KEY, JSON.stringify(next))
}

function AuthProvider({ children }: PropsWithChildren) {
  const authClient = useMemo(() => createAuthClient(), [])

  return (
    <AuthClientContext.Provider value={authClient}>
      {children}
    </AuthClientContext.Provider>
  )
}

export function AppProviders({ children }: PropsWithChildren) {
  const [preferences, setPreferences] = useState<StoredThemePreferences>(readStoredPreferences)

  const theme = useMemo(
    () =>
      createAppTheme({
        mode: preferences.mode,
        preset: preferences.preset,
      }),
    [preferences.mode, preferences.preset],
  )

  const themePreferences = useMemo(
    () => ({
      mode: preferences.mode,
      preset: preferences.preset,
      setMode: (mode: ThemeMode) => {
        setPreferences((current) => {
          if (current.mode === mode) {
            return current
          }

          const next = { ...current, mode }
          persistPreferences(next)
          return next
        })
      },
      setPreset: (preset: PalettePreset) => {
        setPreferences((current) => {
          if (current.preset === preset) {
            return current
          }

          const next = { ...current, preset }
          persistPreferences(next)
          return next
        })
      },
    }),
    [preferences.mode, preferences.preset],
  )

  return (
    <ThemePreferenceContext.Provider value={themePreferences}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <BrowserRouter>
          <AuthProvider>
            <ApiProvider>
              <AuthBootstrapProvider>{children}</AuthBootstrapProvider>
            </ApiProvider>
          </AuthProvider>
        </BrowserRouter>
      </ThemeProvider>
    </ThemePreferenceContext.Provider>
  )
}
