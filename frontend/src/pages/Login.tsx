import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export const Login = () => {
	const { login } = useAuth();
	const navigate = useNavigate();

	const handleLogin = () => {
		login({ id: "1", name: "Usuario Admin", email: "admin@gestor.com" });
		navigate("/dashboard");
	};

	return (
		<div className="min-h-screen bg-slate-950 flex items-center justify-center px-4 relative">
			{/* Soft background glow */}
			<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-200 h-200 bg-blue-900/20 rounded-full blur-[120px] pointer-events-none"></div>

			<div className="w-full max-w-md bg-slate-900/60 backdrop-blur-xl border border-slate-800 rounded-3xl p-8 shadow-2xl relative z-10">
				<div className="text-center mb-10">
					<div className="w-16 h-16 bg-blue-600/20 rounded-2xl mx-auto mb-4 flex items-center justify-center border border-blue-500/30">
						<svg
							className="w-8 h-8 text-blue-400"
							fill="none"
							stroke="currentColor"
							viewBox="0 0 24 24"
						>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"
							></path>
						</svg>
					</div>
					<h1 className="text-3xl font-bold text-white mb-2">
						Bienvenido de nuevo
					</h1>
					<p className="text-slate-400 text-sm">Inicia sesión para continuar</p>
				</div>

				<div className="space-y-6">
					<button
						onClick={handleLogin}
						className="w-full py-4 px-4 bg-linear-to-r from-blue-600 to-indigo-600 hover:from-blue-500 hover:to-indigo-500 text-white rounded-xl font-medium shadow-lg shadow-blue-500/25 transition-all transform hover:-translate-y-0.5 active:translate-y-0"
					>
						Simular Inicio de Sesión
					</button>
				</div>
			</div>
		</div>
	);
};
