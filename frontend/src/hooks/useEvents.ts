import { useQuery } from "@tanstack/react-query";
import { getEvents } from "../api/events";
import type { Event } from "../types/event";

// Hook Personalizado: Une TanStack Query con nuestro servicio API
export const useEvents = () => {
	return useQuery<Event[], Error>({
		queryKey: ["events"], // Clave única de caché para esta consulta
		queryFn: getEvents, // Función que realizará la petición real
	});
};
