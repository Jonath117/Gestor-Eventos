import { useMutation } from "@tanstack/react-query";
import { registerParticipant } from "../api/participants";
import type { RegisterParticipantRequest } from "../types/participant";

export const useRegisterParticipant = (eventId: string) => {
	return useMutation({
		mutationFn: (data: RegisterParticipantRequest) =>
			registerParticipant(eventId, data),
	});
};
