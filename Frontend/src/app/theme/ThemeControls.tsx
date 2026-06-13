import { FormControl, MenuItem, Select, Stack, ToggleButton, ToggleButtonGroup, Typography } from '@mui/material'
import type { PalettePreset, ThemeMode } from '../../theme/tokens'
import { useThemePreferences } from './themePreferenceContext'

const presets: Array<{ value: PalettePreset; label: string }> = [
  { value: 'classic', label: 'Classic' },
  { value: 'forest', label: 'Forest' },
  { value: 'ocean', label: 'Ocean' },
]

export function ThemeControls({ compact = false }: { compact?: boolean }) {
  const { mode, preset, setMode, setPreset } = useThemePreferences()

  const controlSize = compact ? 'small' : 'medium'

  return (
    <Stack
      direction={{ xs: 'column', sm: 'row' }}
      spacing={1}
      alignItems={{ xs: 'stretch', sm: 'center' }}
      aria-label="Theme controls"
    >
      <Typography variant="body2" color="text.secondary" sx={{ whiteSpace: 'nowrap' }}>
        Theme
      </Typography>

      <FormControl size={controlSize}>
        <Select
          value={preset}
          onChange={(event) => setPreset(event.target.value as PalettePreset)}
          inputProps={{ 'aria-label': 'Select palette preset' }}
        >
          {presets.map((item) => (
            <MenuItem key={item.value} value={item.value}>
              {item.label}
            </MenuItem>
          ))}
        </Select>
      </FormControl>

      <ToggleButtonGroup
        exclusive
        value={mode}
        size={controlSize}
        onChange={(_, value: ThemeMode | null) => {
          if (value) {
            setMode(value)
          }
        }}
        aria-label="Select color mode"
      >
        <ToggleButton value="light" aria-label="Light mode">
          Light
        </ToggleButton>
        <ToggleButton value="dark" aria-label="Dark mode">
          Dark
        </ToggleButton>
      </ToggleButtonGroup>
    </Stack>
  )
}
