export interface EventSummary {
	id: string;
	name: string;
	startDate: string;
	endDate: string;
}

export interface Event extends EventSummary {
	maxCapacity: number;
	currentParticipantsCount: number;
}

export interface CreateEventData {
	name: string;
	startDate: string;
	endDate: string;
	maxCapacity: number;
	organizationId: string;
}
