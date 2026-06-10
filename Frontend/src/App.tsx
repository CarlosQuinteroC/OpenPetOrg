import { Box, Chip, Container, Paper, Stack, Typography } from '@mui/material'

function App() {
  return (
    <Container maxWidth="md" sx={{ py: 8 }}>
      <Paper variant="outlined" sx={{ p: 4 }}>
        <Stack spacing={2}>
          <Typography variant="h4" component="h1">
            PetOrg MVP Frontend Scaffold
          </Typography>
          <Typography color="text.secondary">
            React + Vite + MUI baseline with router and auth provider placeholders.
          </Typography>
          <Box>
            <Chip label="MVP Scope Guard: Social automation is out of scope" color="primary" />
          </Box>
        </Stack>
      </Paper>
    </Container>
  )
}

export default App
