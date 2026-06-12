import { ApiClient } from './client'
import type {
  ApiMeResponse,
  ConfirmDonationMatchRequest,
  CreateDonationRequest,
  CreateDonationResponse,
  DonationDto,
  DonorConsentEventDto,
  EnrollRecurringRequest,
  IssueFinalReceiptRequest,
  IssueFinalReceiptResponse,
  PublicRecognitionVisibilityResponse,
  ReconciliationResponse,
  RecurringDonationDto,
  ResolveAmbiguousMatchRequest,
  UpdatePublicRecognitionConsentRequest,
} from './types'

export class PetOrgApi {
  private readonly apiClient: ApiClient

  constructor(apiClient: ApiClient) {
    this.apiClient = apiClient
  }

  getCurrentUser(): Promise<ApiMeResponse> {
    return this.apiClient.request<ApiMeResponse>('/api/me')
  }

  createDonation(request: CreateDonationRequest): Promise<CreateDonationResponse> {
    return this.apiClient.request<CreateDonationResponse>('/api/donations/', {
      method: 'POST',
      body: request,
    })
  }

  getDonationById(donationId: string): Promise<DonationDto> {
    return this.apiClient.request<DonationDto>(`/api/donations/${donationId}`)
  }

  confirmDonationMatch(donationId: string, request: ConfirmDonationMatchRequest): Promise<ReconciliationResponse> {
    return this.apiClient.request<ReconciliationResponse>(`/api/reconciliation/${donationId}/confirm`, {
      method: 'POST',
      body: request,
    })
  }

  markDonationAmbiguous(donationId: string, request: ResolveAmbiguousMatchRequest): Promise<ReconciliationResponse> {
    return this.apiClient.request<ReconciliationResponse>(`/api/reconciliation/${donationId}/ambiguous`, {
      method: 'POST',
      body: request,
    })
  }

  enrollRecurring(request: EnrollRecurringRequest): Promise<RecurringDonationDto> {
    return this.apiClient.request<RecurringDonationDto>('/api/recurring/', {
      method: 'POST',
      body: request,
    })
  }

  cancelRecurring(recurringDonationId: string, cancelledOn: string): Promise<RecurringDonationDto> {
    return this.apiClient.request<RecurringDonationDto>(`/api/recurring/${recurringDonationId}/cancel`, {
      method: 'POST',
      body: { cancelledOn },
    })
  }

  markRecurringPaymentFailed(recurringDonationId: string): Promise<RecurringDonationDto> {
    return this.apiClient.request<RecurringDonationDto>(`/api/recurring/${recurringDonationId}/mark-payment-failed`, {
      method: 'POST',
    })
  }

  appendPublicRecognitionConsent(
    donorId: string,
    request: UpdatePublicRecognitionConsentRequest,
  ): Promise<DonorConsentEventDto> {
    return this.apiClient.request<DonorConsentEventDto>(`/api/donors/${donorId}/consent/public-recognition/`, {
      method: 'POST',
      body: request,
    })
  }

  getPublicRecognitionVisibility(donorId: string, at?: string): Promise<PublicRecognitionVisibilityResponse> {
    const query = at ? `?at=${encodeURIComponent(at)}` : ''
    return this.apiClient.request<PublicRecognitionVisibilityResponse>(
      `/api/donors/${donorId}/public-recognition/visibility${query}`,
    )
  }

  issueFinalReceipt(donationId: string, request: IssueFinalReceiptRequest): Promise<IssueFinalReceiptResponse> {
    return this.apiClient.request<IssueFinalReceiptResponse>(`/api/receipts/${donationId}/issue-final`, {
      method: 'POST',
      body: request,
    })
  }
}
