export interface Organization {
	id: string;
	name: string;
	qrPaymentImageUrl?: string;
	createdAt: string;
}

export interface CreateOrganizationData {
	name: string;
	qrPaymentImageUrl?: string;
}
