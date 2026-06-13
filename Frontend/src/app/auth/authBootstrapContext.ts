import { createContext, useContext } from 'react'
import type { AuthState } from './model'

export type AuthBootstrapContextValue = {
  state: AuthState
  retry: () => Promise<void>
  logout: () => Promise<void>
}

export const AuthBootstrapContext = createContext<AuthBootstrapContextValue | null>(null)

export function useAuthBootstrap() {
  const context = useContext(AuthBootstrapContext)

  if (!context) {
    throw new Error('useAuthBootstrap must be used inside AuthBootstrapProvider')
  }

  return context
}
