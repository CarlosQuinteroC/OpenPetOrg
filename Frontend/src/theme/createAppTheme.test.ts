import { describe, expect, it } from 'vitest'
import { createAppTheme } from './createAppTheme'

describe('createAppTheme', () => {
  it('preserves semantic intent across light and dark modes for the same preset', () => {
    const light = createAppTheme({ mode: 'light', preset: 'classic' })
    const dark = createAppTheme({ mode: 'dark', preset: 'classic' })

    expect(light.palette.mode).toBe('light')
    expect(dark.palette.mode).toBe('dark')
    expect(light.palette.primary.main).toBe('#1F6FCC')
    expect(dark.palette.primary.main).toBe('#3D87E3')
    expect(light.palette.text.primary).not.toBe(dark.palette.text.primary)
    expect(light.palette.background.default).not.toBe(dark.palette.background.default)
  })

  it('applies preset switching through token-based palette values', () => {
    const classic = createAppTheme({ mode: 'light', preset: 'classic' })
    const forest = createAppTheme({ mode: 'light', preset: 'forest' })

    expect(classic.palette.primary.main).toBe('#1F6FCC')
    expect(forest.palette.primary.main).toBe('#2E8B57')
    expect(classic.palette.secondary.main).toBe('#52A6FF')
    expect(forest.palette.secondary.main).toBe('#3FBF84')
  })

  it('keeps reduced-motion and focus-visible accessibility baselines enabled', () => {
    const theme = createAppTheme({ mode: 'light', preset: 'ocean' })
    const baseline = theme.components?.MuiCssBaseline?.styleOverrides as
      | Record<string, unknown>
      | undefined

    expect(baseline).toBeDefined()
    expect(baseline?.[':focus-visible']).toBeDefined()
    expect(baseline?.['@media (prefers-reduced-motion: reduce)']).toBeDefined()
  })
})
