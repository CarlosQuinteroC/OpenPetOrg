import type { ApiMeResponse } from '../../services/api/types'
import type { UserIdentity } from './model'

function resolveDisplayName(email: string | null, subject: string): string {
  if (!email) {
    return subject
  }

  const localPart = email.split('@')[0]?.trim()
  return localPart || subject
}

export function mapApiMeToIdentity(me: ApiMeResponse): UserIdentity {
  return {
    id: me.subject,
    displayName: resolveDisplayName(me.email, me.subject),
    email: me.email,
    roles: me.roles,
  }
}
