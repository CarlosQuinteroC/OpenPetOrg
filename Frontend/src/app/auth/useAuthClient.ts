import { useContext } from 'react'
import { AuthClientContext } from './authContext'
import type { AuthClient } from './authClient'

export function useAuthClient(): AuthClient {
  const authClient = useContext(AuthClientContext)

  if (!authClient) {
    throw new Error('useAuthClient must be used inside AppProviders')
  }

  return authClient
}
