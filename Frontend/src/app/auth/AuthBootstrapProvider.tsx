import { Alert, Box, Button, CircularProgress, Stack, Typography } from '@mui/material'
import {
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
  type PropsWithChildren,
} from 'react'
import { useApi } from '../../services/api/useApi'
import { AuthBootstrapContext, type AuthBootstrapContextValue } from './authBootstrapContext'
import { useAuthClient } from './useAuthClient'
import { mapApiMeToIdentity } from './mapApiMe'
import type { AuthState } from './model'

const initialState: AuthState = {
  status: 'loading',
  identity: null,
  errorMessage: null,
}

function getErrorMessage(error: unknown): string {
  if (error instanceof Error && error.message) {
    return error.message
  }

  return 'Unable to validate your session right now. Please retry.'
}

export function AuthBootstrapProvider({ children }: PropsWithChildren) {
  const authClient = useAuthClient()
  const api = useApi()
  const [state, setState] = useState<AuthState>(initialState)
  const inFlightBootstrapRef = useRef<Promise<void> | null>(null)

  const runBootstrap = useCallback(async () => {
    if (inFlightBootstrapRef.current) {
      return inFlightBootstrapRef.current
    }

    const currentRun = (async () => {
      setState((previous) => ({
        ...previous,
        status: 'loading',
        errorMessage: null,
      }))

      try {
        const tokenUser = await authClient.getCurrentUser()

        if (!tokenUser) {
          setState({
            status: 'anonymous',
            identity: null,
            errorMessage: null,
          })
          return
        }

        const me = await api.getCurrentUser()
        setState({
          status: 'authenticated',
          identity: mapApiMeToIdentity(me),
          errorMessage: null,
        })
      } catch (error: unknown) {
        setState({
          status: 'error',
          identity: null,
          errorMessage: getErrorMessage(error),
        })
      } finally {
        inFlightBootstrapRef.current = null
      }
    })()

    inFlightBootstrapRef.current = currentRun
    return currentRun
  }, [api, authClient])

  useEffect(() => {
    void runBootstrap()
  }, [runBootstrap])

  const logout = useCallback(async () => {
    await authClient.logout()
    setState({
      status: 'anonymous',
      identity: null,
      errorMessage: null,
    })
  }, [authClient])

  const contextValue = useMemo<AuthBootstrapContextValue>(
    () => ({
      state,
      retry: runBootstrap,
      logout,
    }),
    [logout, runBootstrap, state],
  )

  return <AuthBootstrapContext.Provider value={contextValue}>{children}</AuthBootstrapContext.Provider>
}

export function AuthGuardLoadingState() {
  return (
    <Stack spacing={2} alignItems="center" sx={{ py: 8 }}>
      <CircularProgress aria-label="Loading session" />
      <Typography color="text.secondary">Checking your session...</Typography>
    </Stack>
  )
}

export function AuthGuardErrorState({ onRetry, message }: { onRetry: () => void; message?: string | null }) {
  return (
    <Box sx={{ py: 4 }}>
      <Alert
        severity="error"
        action={
          <Button color="inherit" size="small" onClick={onRetry}>
            Retry
          </Button>
        }
      >
        {message ?? 'We could not validate your session. Please retry.'}
      </Alert>
    </Box>
  )
}
