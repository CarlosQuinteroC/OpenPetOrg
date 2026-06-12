import { useContext } from 'react'
import { ApiContext } from './apiContext'

export function useApi() {
  const api = useContext(ApiContext)

  if (!api) {
    throw new Error('useApi must be used inside ApiProvider')
  }

  return api
}
