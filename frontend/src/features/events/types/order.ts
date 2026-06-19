export type OrderStatus = 0 | 1 | 2 | 3;
// 0: PendingVerification, 1: PaymentPending, 2: Confirmed, 3: Rejected

export interface ParticipantDto {
	id: string;
	fullName: string;
	phone?: string;
}

export interface OrderDto {
	id: string;
	contactEmail: string;
	status: OrderStatus;
	createdAt: string;
	participants: ParticipantDto[];
}
