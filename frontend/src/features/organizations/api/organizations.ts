import { api } from "../../../api/client";
import type { Organization } from "../types/organization";

export const getOrganizations = async (): Promise<Organization[]> => {
	const response = await api.get<Organization[]>("/organizations");
	return response.data;
};
