import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createEvent } from "../api/events";
import type { CreateEventData } from "../types/event";

export const useCreateEvent = () => {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: (payload: CreateEventData) => createEvent(payload),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["events"] });
		},
	});
};
