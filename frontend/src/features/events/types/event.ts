export interface EventSummary {
	id: string;
	name: string;
	date: string;
}

export interface Event extends EventSummary {
	maxCapacity: number;
	currentParticipantsCount: number;
}

export interface CreateEventData {
	name: string;
	date: string;
	maxCapacity: number;
}
