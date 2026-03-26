import { api } from "../../../api/client";
import type {
	RegisterParticipantRequest,
	RegisterParticipantResponse,
} from "../types/participant";

export const registerParticipant = async (
	eventId: string,
	data: RegisterParticipantRequest,
): Promise<RegisterParticipantResponse> => {
	const response = await api.post<RegisterParticipantResponse>(
		`/events/${eventId}/participants`,
		data,
	);
	return response.data;
};
