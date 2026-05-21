export interface EventSummary {
	id: string;
	name: string;
	startDate: string;
	endDate: string;
}

export interface Event extends EventSummary {
	maxCapacity: number;
	createdAt: string;
}

export interface CreateEventData {
	name: string;
	startDate: string;
	endDate: string;
	maxCapacity: number;
	organizationId: string;
}
