import { alpha, AppBar, Box, Button, Container, Stack, Toolbar, Typography, useTheme } from '@mui/material'
import { Suspense } from 'react'
import { Link, Outlet, useLocation } from 'react-router-dom'
import { useAuthBootstrap } from '../../app/auth/authBootstrapContext'
import { ThemeControls } from '../../app/theme/ThemeControls'
import { RouteContentFallback } from './RouteContentFallback'

const navItems = [
  { to: '/app/donations/new', label: 'Donation Intake' },
  { to: '/app/reconciliation', label: 'Reconciliation Queue' },
  { to: '/app/dashboard', label: 'Donor Dashboard' },
  { to: '/app/animal-cases/timeline', label: 'Animal Case Timeline' },
]

export function AppShell() {
  const theme = useTheme()
  const location = useLocation()
  const {
    state: { identity },
    logout,
  } = useAuthBootstrap()

  return (
    <>
      <AppBar position="sticky" color="inherit" elevation={0} sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Toolbar sx={{ gap: 1, flexWrap: 'wrap' }}>
          <Typography variant="h6" component="p" sx={{ mr: 2 }}>
            PetOrg MVP
          </Typography>
          <ThemeControls compact />
          {navItems.map((item) => {
            const isActive = location.pathname.startsWith(item.to)

            return (
              <Button
                key={item.to}
                component={Link}
                to={item.to}
                variant={isActive ? 'contained' : 'text'}
                size="small"
                sx={
                  isActive
                    ? {
                        '&:hover': {
                          backgroundColor: 'primary.main',
                        },
                      }
                    : undefined
                }
              >
                {item.label}
              </Button>
            )
          })}
          <Box sx={{ ml: 'auto' }}>
            {identity ? (
              <Button size="small" variant="outlined" onClick={() => void logout()}>
                Logout
              </Button>
            ) : null}
          </Box>
        </Toolbar>
      </AppBar>

      <Container component="main" maxWidth="lg" sx={{ py: 3 }}>
        <Stack spacing={1} sx={{ mb: 3 }}>
          <Typography variant="h4" component="h1">
            Animal Foundation Platform Colombia
          </Typography>
          <Typography color="text.secondary">
            Frontend integration against backend contracts. MVP scope excludes social automation.
          </Typography>
          {identity ? (
            <Typography variant="body2" color="text.secondary">
              Signed in as <strong>{identity.displayName}</strong>
            </Typography>
          ) : null}
        </Stack>

        <Box
          sx={{
            border: 1,
            borderColor: 'divider',
            borderRadius: 2,
            p: 2,
            backgroundColor: alpha(theme.palette.background.paper, theme.palette.mode === 'dark' ? 0.9 : 1),
          }}
        >
          <Suspense fallback={<RouteContentFallback />}>
            <Outlet />
          </Suspense>
        </Box>
      </Container>
    </>
  )
}
