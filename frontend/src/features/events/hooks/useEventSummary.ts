import { useQuery } from "@tanstack/react-query";
import { getEventSummary } from "../api/registration";

export const useEventSummary = (eventId: string) => {
	return useQuery({
		queryKey: ["eventSummary", eventId],
		queryFn: () => getEventSummary(eventId),
		enabled: !!eventId,
	});
};
