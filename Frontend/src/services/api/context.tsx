import { useMemo, type PropsWithChildren } from 'react'
import { ApiClient } from './client'
import { PetOrgApi } from './petOrgApi'
import { useAuthClient } from '../../app/auth/useAuthClient'
import { ApiContext } from './apiContext'

function getApiBaseUrl(): string {
  const envUrl = import.meta.env.VITE_API_BASE_URL as string | undefined
  return envUrl?.trim() ? envUrl.trim() : 'http://localhost:5000'
}

export function ApiProvider({ children }: PropsWithChildren) {
  const authClient = useAuthClient()

  const api = useMemo(() => {
    const client = new ApiClient({
      baseUrl: getApiBaseUrl(),
      authClient,
    })
    return new PetOrgApi(client)
  }, [authClient])

  return <ApiContext.Provider value={api}>{children}</ApiContext.Provider>
}
