import type { AuthClient } from '../../app/auth/authClient'
import type { ApiError } from './types'

type ApiClientOptions = {
  baseUrl: string
  authClient: AuthClient
}

type RequestOptions = {
  method?: 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE'
  body?: unknown
}

export class ApiClient {
  private readonly baseUrl: string
  private readonly authClient: AuthClient

  constructor(options: ApiClientOptions) {
    this.baseUrl = options.baseUrl.replace(/\/$/, '')
    this.authClient = options.authClient
  }

  async request<TResponse>(path: string, options?: RequestOptions): Promise<TResponse> {
    const token = await this.authClient.getAccessToken()
    const headers = new Headers({
      Accept: 'application/json',
    })

    if (token) {
      headers.set('Authorization', `Bearer ${token}`)
    }

    if (options?.body !== undefined) {
      headers.set('Content-Type', 'application/json')
    }

    const response = await fetch(`${this.baseUrl}${path}`, {
      method: options?.method ?? 'GET',
      headers,
      body: options?.body === undefined ? undefined : JSON.stringify(options.body),
    })

    if (!response.ok) {
      let details: unknown
      try {
        details = await response.json()
      } catch {
        details = undefined
      }

      const apiError: ApiError = {
        status: response.status,
        message: response.statusText || 'Request failed',
        details,
      }

      throw apiError
    }

    if (response.status === 204) {
      return undefined as TResponse
    }

    return (await response.json()) as TResponse
  }
}
