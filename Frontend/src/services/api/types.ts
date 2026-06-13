export type ApiError = {
  status: number
  message: string
  details?: unknown
}

export type ApiMeResponse = {
  subject: string
  email: string | null
  roles: string[]
}

export type CreateDonationRequest = {
  donorId: string
  amount: number
  currency: string
  channel: 'online' | 'offline'
  reference?: string
  occurredAt: string
}

export type CreateDonationResponse = {
  donationId: string
  reconciliationStatus: 'pending' | 'confirmed' | 'exception' | string
}

export type DonationDto = {
  donationId: string
  donorId: string
  amount: number
  currency: string
  channel: 'online' | 'offline' | string
  reconciliationStatus: 'pending' | 'confirmed' | 'exception' | string
  reference?: string | null
  occurredAt: string
}

export type ConfirmDonationMatchRequest = {
  matchedDonorId: string
  evidenceNote?: string
}

export type ResolveAmbiguousMatchRequest = {
  selectedDonorId: string
  resolutionNote?: string
}

export type ReconciliationResponse = {
  donationId: string
  reconciliationStatus: 'pending' | 'confirmed' | 'exception' | string
  matchedDonorId?: string | null
}

export type EnrollRecurringRequest = {
  donorId: string
  amount: number
  currency: string
  startedOn: string
}

export type CancelRecurringRequest = {
  cancelledOn: string
}

export type RecurringDonationDto = {
  recurringDonationId: string
  donorId: string
  amount: number
  currency: string
  status: 'active' | 'cancelled' | 'payment-failed' | string
  startedOn: string
  cancelledOn?: string | null
}

export type UpdatePublicRecognitionConsentRequest = {
  granted: boolean
  effectiveAt: string
  actorId: string
  note?: string
}

export type DonorConsentEventDto = {
  eventId: string
  donorId: string
  granted: boolean
  effectiveAt: string
  actorId: string
  note?: string | null
}

export type PublicRecognitionVisibilityResponse = {
  donorId: string
  isVisible: boolean
  determinedAt: string
  sourceEventId?: string | null
}

export type IssueFinalReceiptRequest = {
  receiptNumber: string
}

export type IssueFinalReceiptResponse = {
  receiptId: string
  donationId: string
  receiptNumber: string
  status: string
  issuedAt: string
}

export type ReceiptIssueFailure = {
  code: 'donation_not_found' | 'donation_not_confirmed' | 'missing_donor' | string
  message: string
}

export type DonationTimelineEvent = {
  donationId: string
  type: 'created' | 'matched' | 'confirmed' | 'exception' | 'receipt_issued'
  at: string
  actorId?: string
}

export type AnimalCaseTimelineEvent = {
  caseId: string
  type: 'opened' | 'status_changed' | 'medical_update' | 'adoption_update'
  at: string
  actorId: string
}
