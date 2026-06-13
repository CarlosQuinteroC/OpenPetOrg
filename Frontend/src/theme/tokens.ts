export type PalettePreset = 'classic' | 'forest' | 'ocean'
export type ThemeMode = 'light' | 'dark'

export type ReferenceTokens = {
  brand: {
    50: string
    100: string
    400: string
    500: string
    700: string
  }
  accent: {
    100: string
    400: string
    600: string
  }
  neutral: {
    0: string
    50: string
    100: string
    300: string
    600: string
    800: string
    900: string
  }
  success: string
  warning: string
  error: string
  info: string
}

export type SemanticTokens = {
  canvas: string
  surface: string
  surfaceMuted: string
  textPrimary: string
  textSecondary: string
  borderSubtle: string
  focusRing: string
  ctaPrimaryBg: string
  ctaPrimaryText: string
  ctaSecondaryBorder: string
  ctaSecondaryText: string
}

export type ComponentTokens = {
  radius: {
    sm: number
    md: number
    lg: number
  }
  focus: {
    width: number
    offset: number
  }
  shell: {
    headerHeight: number
  }
}

export type ThemeTokenPack = {
  preset: PalettePreset
  mode: ThemeMode
  reference: ReferenceTokens
  semantic: SemanticTokens
  component: ComponentTokens
}

export const DEFAULT_THEME_MODE: ThemeMode = 'light'
export const DEFAULT_PALETTE_PRESET: PalettePreset = 'classic'

const presetReferenceTokens: Record<PalettePreset, ReferenceTokens> = {
  classic: {
    brand: { 50: '#EAF3FF', 100: '#D4E7FF', 400: '#3D87E3', 500: '#1F6FCC', 700: '#0F4D91' },
    accent: { 100: '#E3F2FD', 400: '#52A6FF', 600: '#1976D2' },
    neutral: {
      0: '#FFFFFF',
      50: '#F8FAFC',
      100: '#EEF2F6',
      300: '#C9D2DE',
      600: '#506072',
      800: '#253344',
      900: '#15202D',
    },
    success: '#23836A',
    warning: '#A96A11',
    error: '#BB2F3B',
    info: '#2A6ABD',
  },
  forest: {
    brand: { 50: '#EAF8F1', 100: '#D6EFDF', 400: '#49A874', 500: '#2E8B57', 700: '#1E613D' },
    accent: { 100: '#E1F5EC', 400: '#3FBF84', 600: '#218C5A' },
    neutral: {
      0: '#FFFFFF',
      50: '#F7FAF8',
      100: '#EDF3EF',
      300: '#C2D2C6',
      600: '#516454',
      800: '#2A3B2D',
      900: '#19271C',
    },
    success: '#247B55',
    warning: '#9F6A13',
    error: '#B23442',
    info: '#2F6A93',
  },
  ocean: {
    brand: { 50: '#ECF9FF', 100: '#D8EEFA', 400: '#369DC6', 500: '#1E7FA8', 700: '#145776' },
    accent: { 100: '#E2F8FF', 400: '#36C2CC', 600: '#1F8F97' },
    neutral: {
      0: '#FFFFFF',
      50: '#F6FAFC',
      100: '#EAF1F4',
      300: '#BFCCD4',
      600: '#4B616D',
      800: '#213742',
      900: '#132630',
    },
    success: '#258370',
    warning: '#9D6E16',
    error: '#B83B4A',
    info: '#216A8A',
  },
}

const componentTokens: ComponentTokens = {
  radius: {
    sm: 8,
    md: 12,
    lg: 18,
  },
  focus: {
    width: 3,
    offset: 2,
  },
  shell: {
    headerHeight: 64,
  },
}

export function getThemeTokenPack(preset: PalettePreset, mode: ThemeMode): ThemeTokenPack {
  const reference = presetReferenceTokens[preset]

  const semantic: SemanticTokens =
    mode === 'light'
      ? {
          canvas: reference.neutral[50],
          surface: reference.neutral[0],
          surfaceMuted: reference.neutral[100],
          textPrimary: reference.neutral[900],
          textSecondary: reference.neutral[600],
          borderSubtle: reference.neutral[300],
          focusRing: reference.accent[400],
          ctaPrimaryBg: reference.brand[500],
          ctaPrimaryText: reference.neutral[0],
          ctaSecondaryBorder: reference.brand[500],
          ctaSecondaryText: reference.brand[700],
        }
      : {
          canvas: '#0D141B',
          surface: '#111C25',
          surfaceMuted: '#162431',
          textPrimary: '#ECF3FA',
          textSecondary: '#B5C5D3',
          borderSubtle: '#2A4154',
          focusRing: reference.accent[400],
          ctaPrimaryBg: reference.brand[400],
          ctaPrimaryText: '#07131E',
          ctaSecondaryBorder: reference.accent[400],
          ctaSecondaryText: '#DDF4FF',
        }

  return {
    preset,
    mode,
    reference,
    semantic,
    component: componentTokens,
  }
}
