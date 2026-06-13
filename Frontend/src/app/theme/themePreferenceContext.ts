import { createContext, useContext } from 'react'
import type { PalettePreset, ThemeMode } from '../../theme/tokens'

export type ThemePreferenceContextValue = {
  mode: ThemeMode
  preset: PalettePreset
  setMode: (mode: ThemeMode) => void
  setPreset: (preset: PalettePreset) => void
}

export const ThemePreferenceContext = createContext<ThemePreferenceContextValue | null>(null)

export function useThemePreferences() {
  const context = useContext(ThemePreferenceContext)

  if (!context) {
    throw new Error('useThemePreferences must be used inside AppProviders')
  }

  return context
}
