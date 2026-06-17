import { useQuery } from "@tanstack/react-query";
import { getOrganizations } from "../api/organizations";
import type { Organization } from "../types/organization";

export const useOrganizations = () => {
	return useQuery<Organization[], Error>({
		queryKey: ["organizations"],
		queryFn: async () => {
			return getOrganizations();
		},
	});
};
