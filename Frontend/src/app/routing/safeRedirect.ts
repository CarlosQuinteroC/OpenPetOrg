const DEFAULT_APP_PATH = '/app'

function isSafeRelativePath(path: string): boolean {
  if (!path.startsWith('/')) {
    return false
  }

  if (path.startsWith('//')) {
    return false
  }

  return true
}

export function resolveSafeAppPath(nextRaw: string | null | undefined): string {
  if (!nextRaw) {
    return DEFAULT_APP_PATH
  }

  if (!isSafeRelativePath(nextRaw)) {
    return DEFAULT_APP_PATH
  }

  if (nextRaw !== '/app' && !nextRaw.startsWith('/app/')) {
    return DEFAULT_APP_PATH
  }

  return nextRaw
}

export function toLoginRedirect(nextAppPath: string): string {
  const safePath = resolveSafeAppPath(nextAppPath)
  const params = new URLSearchParams({ next: safePath })
  return `/?${params.toString()}`
}
