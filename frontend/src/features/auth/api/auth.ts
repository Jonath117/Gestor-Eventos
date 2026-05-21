import { api } from "../../../api/client";

export interface LoginCredentials {
	email: string;
	password?: string;
}

export interface LoginResponse {
	accessToken: string;
	refreshToken: string;
}

export interface RegisterCredentials {
	email: string;
	password?: string;
}

export interface RegisterResponse {
	id: string;
	email: string;
}

export const loginUser = async (
	credentials: LoginCredentials,
): Promise<LoginResponse> => {
	const response = await api.post<LoginResponse>(
		"/identity/login",
		credentials,
	);
	return response.data;
};

export const registerUser = async (
	credentials: RegisterCredentials,
): Promise<RegisterResponse> => {
	const response = await api.post<RegisterResponse>(
		"/identity/register",
		credentials,
	);
	return response.data;
};
