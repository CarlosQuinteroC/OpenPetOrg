import { useContext } from 'react'
import { AuthClientContext } from './authContext'

export function useAuthClient() {
  const authClient = useContext(AuthClientContext)

  if (!authClient) {
    throw new Error('useAuthClient must be used inside AppProviders')
  }

  return authClient
}
