import axios from "axios";

// Instanciamos el cliente de axios usando la variable de entorno
export const api = axios.create({
	baseURL: import.meta.env.VITE_API_URL,
	headers: {
		"Content-Type": "application/json",
	},
});

// Interceptor para inyectar el token en peticiones a /api/events
api.interceptors.request.use(
	(config) => {
		const storedToken = localStorage.getItem("token");
		if (storedToken && config.url?.includes("/events")) {
			try {
				// useLocalStorage guarda valores serializados con JSON.stringify
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
