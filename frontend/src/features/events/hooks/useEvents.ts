import { useQuery } from "@tanstack/react-query";
import { getEvents } from "../api/events";
import type { EventSummary } from "../types/event";

// Hook Personalizado: Une TanStack Query con nuestro servicio API
export const useEvents = (tenantId: string | null) => {
	return useQuery<EventSummary[], Error>({
		queryKey: ["events", tenantId], // Clave única de caché para esta consulta
		queryFn: async () => {
			if (!tenantId) return [];
			await new Promise((resolve) => setTimeout(resolve, 1000));

			//ejectuamos la pettcion
			return getEvents();
		},
		enabled: !!tenantId,
	});
};
