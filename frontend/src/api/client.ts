import axios from "axios";

// Instanciamos el cliente de axios usando la variable de entorno
export const api = axios.create({
	baseURL: import.meta.env.VITE_API_URL,
	headers: {
		"Content-Type": "application/json",
	},
});
