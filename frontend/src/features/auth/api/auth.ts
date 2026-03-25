import { api } from "../../../api/client";

export interface LoginCredentials {
	email: string;
	password?: string;
}

export interface LoginResponse {
	token: string;
}

export const loginUser = async (
	credentials: LoginCredentials,
): Promise<LoginResponse> => {
	// Consumir el endpoint específico de login
	const response = await api.post<LoginResponse>(
		"/identity/login",
		credentials,
	);
	return response.data;
};
