import { useMutation } from "@tanstack/react-query";
import {
	requestRegistrationOtp,
	submitRegistration,
	verifyRegistrationOtp,
} from "../api/registration";

export const useRequestOtp = (eventId: string) => {
	return useMutation({
		mutationFn: (data: { email: string; fullName: string }) =>
			requestRegistrationOtp(eventId, data),
	});
};

export const useVerifyOtp = (eventId: string) => {
	return useMutation({
		mutationFn: (data: { email: string; otp: string }) =>
			verifyRegistrationOtp(eventId, data),
	});
};

export const useSubmitRegistration = (eventId: string) => {
	return useMutation({
		mutationFn: (data: { email: string; fullName: string; phone?: string }) =>
			submitRegistration(eventId, data),
	});
};
