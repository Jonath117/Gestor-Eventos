import { api } from "../../../api/client";
import type { CreateEventData, Event } from "../types/event";

export const getEvents = async (): Promise<Event[]> => {
	const response = await api.get<Event[]>("/events");
	return response.data;
};

export const getEvent = async (id: string): Promise<Event> => {
	const response = await api.get<Event>(`/events/${id}`);
	return response.data;
};

export const createEvent = async (
	eventData: CreateEventData,
): Promise<{ id: string }> => {
	const response = await api.post<{ id: string }>("/events", eventData);
	return response.data;
};

export const deleteEvent = async (id: string | number): Promise<void> => {
	await api.delete(`/events/${id}`);
};
