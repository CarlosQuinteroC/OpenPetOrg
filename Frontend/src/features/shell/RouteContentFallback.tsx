import { Stack, CircularProgress, Typography } from '@mui/material'

export function RouteContentFallback() {
  return (
    <Stack spacing={1.5} alignItems="center" sx={{ py: 6 }}>
      <CircularProgress size={28} aria-label="Loading route content" />
      <Typography color="text.secondary">Loading route content...</Typography>
    </Stack>
  )
}
