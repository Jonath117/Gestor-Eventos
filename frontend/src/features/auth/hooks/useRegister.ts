import { useMutation } from "@tanstack/react-query";
import {
	type RegisterCredentials,
	type RegisterResponse,
	registerUser,
} from "../api/auth";

export const useRegister = () => {
	return useMutation<RegisterResponse, Error, RegisterCredentials>({
		mutationFn: registerUser,
	});
};
