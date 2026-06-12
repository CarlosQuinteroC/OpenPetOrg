import { createContext } from 'react'
import type { AuthClient } from './authClient'

export const AuthClientContext = createContext<AuthClient | null>(null)
