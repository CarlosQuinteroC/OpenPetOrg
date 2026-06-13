export type UserIdentity = {
  id: string
  displayName: string
  email: string | null
  roles: string[]
}

export type AuthStatus = 'loading' | 'anonymous' | 'authenticated' | 'error'

export type AuthState = {
  status: AuthStatus
  identity: UserIdentity | null
  errorMessage: string | null
}
