import { alpha, createTheme, type Theme } from '@mui/material/styles'
import { getThemeTokenPack, type PalettePreset, type ThemeMode } from './tokens'

export type CreateAppThemeOptions = {
  mode: ThemeMode
  preset: PalettePreset
}

export function createAppTheme({ mode, preset }: CreateAppThemeOptions): Theme {
  const tokenPack = getThemeTokenPack(preset, mode)

  return createTheme({
    palette: {
      mode,
      primary: {
        main: tokenPack.semantic.ctaPrimaryBg,
        contrastText: tokenPack.semantic.ctaPrimaryText,
      },
      secondary: {
        main: tokenPack.reference.accent[400],
      },
      background: {
        default: tokenPack.semantic.canvas,
        paper: tokenPack.semantic.surface,
      },
      text: {
        primary: tokenPack.semantic.textPrimary,
        secondary: tokenPack.semantic.textSecondary,
      },
      divider: tokenPack.semantic.borderSubtle,
      success: {
        main: tokenPack.reference.success,
      },
      warning: {
        main: tokenPack.reference.warning,
      },
      error: {
        main: tokenPack.reference.error,
      },
      info: {
        main: tokenPack.reference.info,
      },
    },
    shape: {
      borderRadius: tokenPack.component.radius.md,
    },
    typography: {
      button: {
        textTransform: 'none',
        fontWeight: 600,
      },
    },
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            backgroundColor: tokenPack.semantic.canvas,
            color: tokenPack.semantic.textPrimary,
          },
          ':focus-visible': {
            outline: `${tokenPack.component.focus.width}px solid ${tokenPack.semantic.focusRing}`,
            outlineOffset: tokenPack.component.focus.offset,
          },
          '@media (prefers-reduced-motion: reduce)': {
            '*, *::before, *::after': {
              animationDuration: '0.01ms !important',
              animationIterationCount: '1 !important',
              transitionDuration: '0.01ms !important',
              scrollBehavior: 'auto !important',
            },
          },
        },
      },
      MuiButton: {
        styleOverrides: {
          root: {
            borderRadius: tokenPack.component.radius.sm,
          },
          contained: {
            boxShadow: 'none',
            '&:hover': {
              boxShadow: 'none',
            },
          },
          outlined: {
            borderColor: tokenPack.semantic.ctaSecondaryBorder,
            color: tokenPack.semantic.ctaSecondaryText,
            '&:hover': {
              borderColor: tokenPack.semantic.focusRing,
              backgroundColor: alpha(tokenPack.semantic.focusRing, 0.08),
            },
          },
        },
      },
      MuiAppBar: {
        styleOverrides: {
          colorInherit: {
            backgroundColor: tokenPack.semantic.surface,
          },
        },
      },
      MuiAlert: {
        styleOverrides: {
          standardError: {
            backgroundColor: alpha(tokenPack.reference.error, 0.12),
            color: tokenPack.semantic.textPrimary,
          },
        },
      },
    },
  })
}
