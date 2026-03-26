import { useQuery } from "@tanstack/react-query";
import { getEvent } from "../api/events";
import type { Event } from "../types/event";

// Hook para obtener un solo evento por su ID
export const useEvent = (eventId: string) => {
	return useQuery<Event, Error>({
		queryKey: ["events", eventId],
		queryFn: () => getEvent(eventId),
		enabled: !!eventId,
	});
};
