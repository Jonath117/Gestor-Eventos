import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteEvent } from "../api/events";

export const useDeleteEvent = () => {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: deleteEvent,
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["events"] });
		},
	});
};
