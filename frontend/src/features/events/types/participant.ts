export interface RegisterParticipantRequest {
	fullName: string;
	email: string;
}

export interface RegisterParticipantResponse {
	participantId: string;
}
