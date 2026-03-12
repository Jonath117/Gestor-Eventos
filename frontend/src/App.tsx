import { lazy, Suspense } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { ErrorBoundary } from "./components/ErrorBoundary";
import { LoadingFallback } from "./components/LoadingFallback";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { PublicRoute } from "./components/PublicRoute";
import { AuthProvider } from "./context/AuthContext";

// Carga Perezosa (Lazy Loading) de las páginas
const Home = lazy(() =>
	import("./pages/Home").then((module) => ({ default: module.Home })),
);
const Login = lazy(() =>
	import("./pages/Login").then((module) => ({ default: module.Login })),
);
const Dashboard = lazy(() =>
	import("./pages/Dashboard").then((module) => ({ default: module.Dashboard })),
);
const Settings = lazy(() =>
	import("./pages/Settings").then((module) => ({ default: module.Settings })),
);
const NotFound = lazy(() =>
	import("./pages/NotFound").then((module) => ({ default: module.NotFound })),
);

function App() {
	return (
		<ErrorBoundary>
			<BrowserRouter>
				<AuthProvider>
					<Suspense fallback={<LoadingFallback />}>
						<Routes>
							{/* Rutas Públicas */}
							<Route element={<PublicRoute />}>
								<Route path="/" element={<Home />} />
								<Route path="/login" element={<Login />} />
							</Route>

							{/* Rutas Protegidas */}
							<Route element={<ProtectedRoute />}>
								<Route path="/dashboard" element={<Dashboard />} />
								<Route path="/settings" element={<Settings />} />
							</Route>

							{/* Ruta 404 para URLs que no existen */}
							<Route path="*" element={<NotFound />} />
						</Routes>
					</Suspense>
				</AuthProvider>
			</BrowserRouter>
		</ErrorBoundary>
	);
}

export default App;
