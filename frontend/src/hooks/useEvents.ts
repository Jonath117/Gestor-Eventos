import { useQuery } from "@tanstack/react-query";
import { getEvents } from "../api/events";
import type { Event } from "../types/event";

// Hook Personalizado: Une TanStack Query con nuestro servicio API
export const useEvents = () => {
	return useQuery<Event[], Error>({
		queryKey: ["events"], // Clave única de caché para esta consulta
		queryFn: async () => {
			await new Promise((resolve) => setTimeout(resolve, 1000));

			//ejectuamos la pettcion
			return getEvents();
		},
	});
};
