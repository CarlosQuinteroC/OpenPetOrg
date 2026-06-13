import { Navigate, Outlet, useLocation } from 'react-router-dom'
import {
  AuthGuardErrorState,
  AuthGuardLoadingState,
} from '../auth/AuthBootstrapProvider'
import { useAuthBootstrap } from '../auth/authBootstrapContext'
import { toLoginRedirect } from './safeRedirect'

export function ProtectedRoute() {
  const location = useLocation()
  const {
    state: { status, errorMessage },
    retry,
  } = useAuthBootstrap()

  if (status === 'loading') {
    return <AuthGuardLoadingState />
  }

  if (status === 'error') {
    return <AuthGuardErrorState onRetry={() => void retry()} message={errorMessage} />
  }

  if (status !== 'authenticated') {
    const next = `${location.pathname}${location.search}${location.hash}`
    return <Navigate to={toLoginRedirect(next)} replace />
  }

  return <Outlet />
}
