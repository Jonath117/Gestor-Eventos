import { api } from "../../../api/client";

export interface RequestOtpData {
	email: string;
	fullName: string;
}

export interface VerifyOtpData {
	email: string;
	otp: string;
}

export interface SubmitRegistrationData {
	email: string;
	fullName: string;
	phone?: string;
}

export const requestRegistrationOtp = async (
	eventId: string,
	data: RequestOtpData,
) => {
	const response = await api.post(`/registration/${eventId}/request-otp`, data);
	return response.data;
};

export const verifyRegistrationOtp = async (
	eventId: string,
	data: VerifyOtpData,
) => {
	const response = await api.post(`/registration/${eventId}/verify-otp`, data);
	return response.data;
};

export const submitRegistration = async (
	eventId: string,
	data: SubmitRegistrationData,
) => {
	const response = await api.post(`/registration/${eventId}/submit`, data);
	return response.data;
};

export const getEventOrders = async (eventId: string) => {
	const response = await api.get(`/registration/${eventId}/orders`);
	return response.data;
};

export const updateOrderStatus = async (orderId: string, status: number) => {
	const response = await api.patch(`/registration/orders/${orderId}/status`, {
		status,
	});
	return response.data;
};
