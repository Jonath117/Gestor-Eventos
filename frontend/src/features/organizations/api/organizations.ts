import { api } from "../../../api/client";
import type { Organization } from "../types/organization";
import type {
	CreateOrganizationData,
	Organization,
} from "../types/organization";

export const createOrganization = async (
	data: CreateOrganizationData,
): Promise<{ id: string }> => {
	const response = await api.post<{ id: string }>("/organizations", data);
	return response.data;
};

export const getOrganizations = async (): Promise<Organization[]> => {
	const response = await api.get<Organization[]>("/organizations");
	return response.data;
};
