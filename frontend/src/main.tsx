import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";

// Creamos una instancia de QueryClient para manejar la caché de las peticiones
const queryClient = new QueryClient({
	defaultOptions: {
		queries: {
			refetchOnWindowFocus: false, // Evita que se recargue el API al cambiar de pestaña
			retry: 3, // Intentos de reconexión si una petición falla
		},
	},
});

createRoot(document.getElementById("root")!).render(
	<StrictMode>
		{/* Proveemos el contexto de TanStack Query a toda la aplicación */}
		<QueryClientProvider client={queryClient}>
			<App />
		</QueryClientProvider>
	</StrictMode>,
);
