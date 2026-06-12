import { createContext } from 'react'
import type { PetOrgApi } from './petOrgApi'

export const ApiContext = createContext<PetOrgApi | null>(null)
