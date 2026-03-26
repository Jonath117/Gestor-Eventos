import { useMutation } from "@tanstack/react-query";
import {
	type LoginCredentials,
	type LoginResponse,
	loginUser,
} from "../api/auth";

export const useLogin = () => {
	return useMutation<LoginResponse, Error, LoginCredentials>({
		mutationFn: loginUser,
	});
};
