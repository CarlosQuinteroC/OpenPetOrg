import { Box, Button, Container, Link as MuiLink, Stack, Typography } from '@mui/material'
import { useMemo } from 'react'
import { useLocation } from 'react-router-dom'
import { useAuthClient } from '../../app/auth/useAuthClient'

function resolveNextTarget(nextRaw: string | null): string {
  if (!nextRaw) {
    return '/app'
  }

  if (!nextRaw.startsWith('/app')) {
    return '/app'
  }

  return nextRaw
}

export function LandingPage() {
  const authClient = useAuthClient()
  const location = useLocation()

  const loginTarget = useMemo(() => {
    const params = new URLSearchParams(location.search)
    return resolveNextTarget(params.get('next'))
  }, [location.search])

  const loginDisabled = !authClient.isEnabled()

  return (
    <Box component="main" sx={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <Box
        component="header"
        sx={{ borderBottom: 1, borderColor: 'divider', bgcolor: 'background.paper' }}
      >
        <Container maxWidth="lg" sx={{ py: 2, display: 'flex', alignItems: 'center', gap: 2 }}>
          <Typography variant="h6" component="p" sx={{ fontWeight: 700 }}>
            PetOrg
          </Typography>
          <Button
            sx={{ ml: 'auto' }}
            variant="contained"
            size="small"
            onClick={() => void authClient.login(loginTarget)}
            disabled={loginDisabled}
          >
            Login
          </Button>
        </Container>
      </Box>

      <Container
        maxWidth="lg"
        sx={{ flex: 1, py: { xs: 8, md: 12 }, display: 'flex', alignItems: 'center' }}
      >
        <Stack spacing={3} sx={{ maxWidth: 760 }}>
          <Typography component="h1" variant="h2" sx={{ fontSize: { xs: '2.2rem', md: '3.2rem' } }}>
            Trust-first operations for animal welfare foundations.
          </Typography>
          <Typography color="text.secondary" variant="h6" sx={{ fontWeight: 400 }}>
            Manage donation intake, reconciliation flows, donor transparency, and animal case timelines in one
            secure platform.
          </Typography>
          <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
            <Button
              size="large"
              variant="contained"
              onClick={() => void authClient.login(loginTarget)}
              disabled={loginDisabled}
            >
              Continue with PetOrg Account
            </Button>
            <Button size="large" variant="outlined" href="#trust-signals">
              Why teams trust PetOrg
            </Button>
          </Stack>
        </Stack>
      </Container>

      <Box component="section" id="trust-signals" sx={{ borderTop: 1, borderColor: 'divider', py: 4 }}>
        <Container maxWidth="lg">
          <Stack direction={{ xs: 'column', md: 'row' }} spacing={3}>
            <Stack spacing={0.5}>
              <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                Secure identity
              </Typography>
              <Typography color="text.secondary">Keycloak-backed login and role-aware access boundaries.</Typography>
            </Stack>
            <Stack spacing={0.5}>
              <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                Audit-ready timelines
              </Typography>
              <Typography color="text.secondary">Donation and animal case events remain traceable over time.</Typography>
            </Stack>
            <Stack spacing={0.5}>
              <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                Operational focus
              </Typography>
              <Typography color="text.secondary">Purpose-built for day-to-day nonprofit workflows.</Typography>
            </Stack>
          </Stack>
        </Container>
      </Box>

      <Box component="footer" sx={{ borderTop: 1, borderColor: 'divider', py: 2 }}>
        <Container maxWidth="lg" sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
          <MuiLink href="#" underline="hover" color="text.secondary">
            Documentation
          </MuiLink>
          <MuiLink href="#" underline="hover" color="text.secondary">
            Help
          </MuiLink>
          <MuiLink href="#" underline="hover" color="text.secondary">
            Status
          </MuiLink>
        </Container>
      </Box>
    </Box>
  )
}
