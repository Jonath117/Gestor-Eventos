import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getEventOrders, updateOrderStatus } from "../api/registration";
import type { OrderDto } from "../types/order";

export const useEventOrders = (eventId: string) => {
	return useQuery<OrderDto[], Error>({
		queryKey: ["eventOrders", eventId],
		queryFn: () => getEventOrders(eventId),
	});
};

export const useUpdateOrderStatus = () => {
	const queryClient = useQueryClient();

	return useMutation({
		mutationFn: ({ orderId, status }: { orderId: string; status: number }) =>
			updateOrderStatus(orderId, status),
		onSuccess: () => {
			queryClient.invalidateQueries({ queryKey: ["eventOrders"] });
		},
	});
};
