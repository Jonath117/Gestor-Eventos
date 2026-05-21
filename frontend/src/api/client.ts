import axios from "axios";

// Instanciamos el cliente de axios usando la variable de entorno
export const api = axios.create({
	baseURL: import.meta.env.VITE_API_URL,
	headers: {
		"Content-Type": "application/json",
	},
});

api.interceptors.request.use(
	(config) => {
		const storedToken = localStorage.getItem("token");
		const isAuthRequest =
			config.url?.includes("/login") || config.url?.includes("/register");

		if (storedToken && !isAuthRequest) {
			try {
				const token = JSON.parse(storedToken);
				if (token) {
					config.headers.Authorization = `Bearer ${token}`;
				}
			} catch (error) {
				console.error("Error parsing token from localStorage", error);
			}
		}
		return config;
	},
	(error) => {
		return Promise.reject(error);
	},
);

api.interceptors.response.use(
	(response) => response,
	(error) => {
		if (axios.isAxiosError(error) && error.response?.status === 401) {
			localStorage.removeItem("token");
			localStorage.removeItem("refreshToken");
			window.location.href = "/login";
		}
		return Promise.reject(error);
	},
);
