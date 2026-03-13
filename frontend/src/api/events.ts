import type { Event } from "../types/event";
import { api } from "./client";

// Servicio: Única responsabilidad es hacer la petición HTTP y devolver los datos.
export const getEvents = async (): Promise<Event[]> => {
	const response = await api.get<Event[]>("/events");
	return response.data;
};

export const deleteEvent = async (id: string | number): Promise<void> => {
	const apiUrl = import.meta.env.VITE_API_URL;

	const response = await fetch(`${apiUrl}/events/${id}`, {
		method: "DELETE",
	});

	if (!response.ok) {
		throw new Error("No se pudo eliminar el evento");
	}
};
