export interface Event {
	id: string;
	name: string;
	date: string;
	location: string;
	status: "upcoming" | "active" | "completed" | "cancelled";
	attendees: number;
	revenue: number;
}
