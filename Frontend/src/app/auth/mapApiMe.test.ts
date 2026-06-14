import { describe, expect, it } from 'vitest'
import { mapApiMeToIdentity } from './mapApiMe'

describe('mapApiMeToIdentity', () => {
  it('falls back to subject when email is null', () => {
    const identity = mapApiMeToIdentity({
      subject: 'user-subject-123',
      email: null,
      roles: ['admin', 'foundation_operator'],
    })

    expect(identity).toEqual({
      id: 'user-subject-123',
      displayName: 'user-subject-123',
      email: null,
      roles: ['admin', 'foundation_operator'],
    })
  })

  it('derives displayName from email local-part and preserves roles', () => {
    const identity = mapApiMeToIdentity({
      subject: 'fallback-subject',
      email: 'maria.foundation@example.org',
      roles: ['viewer'],
    })

    expect(identity.displayName).toBe('maria.foundation')
    expect(identity.roles).toEqual(['viewer'])
    expect(identity.id).toBe('fallback-subject')
  })
})
