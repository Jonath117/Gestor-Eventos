import type { Event } from "../types/event";
import { api } from "./client";

// Servicio: Única responsabilidad es hacer la petición HTTP y devolver los datos.
export const getEvents = async (): Promise<Event[]> => {
	const response = await api.get<Event[]>("/events");
	return response.data;
};
