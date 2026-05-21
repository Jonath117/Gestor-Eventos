import { isAxiosError } from "axios";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useRegister } from "../features/auth/hooks/useRegister";

export const Register = () => {
	const navigate = useNavigate();
	const {
		mutate: registerMutation,
		isPending,
		isError,
		error,
		reset,
	} = useRegister();

	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [confirmPassword, setConfirmPassword] = useState("");
	const [localError, setLocalError] = useState<string | null>(null);

	const handleSubmit = (e: React.FormEvent) => {
		e.preventDefault();
		setLocalError(null);

		if (password !== confirmPassword) {
			setLocalError("Las contraseñas no coinciden");
			return;
		}

		registerMutation(
			{ email, password },
			{
				onSuccess: () => {
					// After registration, redirect to login
					// or we could automatically login the user
					navigate("/login", {
						state: { message: "Registro exitoso. Por favor, inicia sesión." },
					});
				},
			},
		);
	};

	return (
		<div className="overflow-hidden min-h-screen bg-slate-950 flex items-center justify-center px-4 relative">
			{/* Soft background glow */}
			<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-200 h-200 bg-indigo-900/20 rounded-full blur-[120px] pointer-events-none"></div>

			<div className="w-full max-w-md bg-slate-900/60 backdrop-blur-xl border border-slate-800 rounded-3xl p-8 shadow-2xl relative z-10">
				<div className="text-center mb-10">
					<div className="w-16 h-16 bg-indigo-600/20 rounded-2xl mx-auto mb-4 flex items-center justify-center border border-indigo-500/30">
						<svg
							className="w-8 h-8 text-indigo-400"
							fill="none"
							stroke="currentColor"
							viewBox="0 0 24 24"
						>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z"
							></path>
						</svg>
					</div>
					<h1 className="text-3xl font-bold text-white mb-2">Crea tu cuenta</h1>
					<p className="text-slate-400 text-sm">
						Únete para empezar a gestionar tus eventos
					</p>
				</div>

				<div className="space-y-6">
					{(isError || localError) && (
						<div className="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-red-400 text-sm">
							{localError ||
								(isAxiosError(error) && error.response?.status === 409
									? "El usuario ya existe"
									: "Ocurrió un error al registrarse")}
						</div>
					)}
					<form onSubmit={handleSubmit} className="space-y-4">
						<div>
							<label className="block text-sm font-medium text-slate-400 mb-1">
								Email
							</label>
							<input
								type="email"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								onFocus={() => {
									if (isError) reset();
									setLocalError(null);
								}}
								className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500 transition-colors"
								required
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-slate-400 mb-1">
								Contraseña
							</label>
							<input
								type="password"
								value={password}
								onChange={(e) => setPassword(e.target.value)}
								onFocus={() => {
									if (isError) reset();
									setLocalError(null);
								}}
								className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500 transition-colors"
								required
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-slate-400 mb-1">
								Confirmar Contraseña
							</label>
							<input
								type="password"
								value={confirmPassword}
								onChange={(e) => setConfirmPassword(e.target.value)}
								onFocus={() => {
									if (isError) reset();
									setLocalError(null);
								}}
								className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500 transition-colors"
								required
							/>
						</div>
						<button
							type="submit"
							disabled={isPending}
							className="w-full mt-6 py-4 px-4 bg-linear-to-r from-indigo-600 to-violet-600 hover:from-indigo-500 hover:to-violet-500 disabled:opacity-50 disabled:cursor-not-allowed text-white rounded-xl font-medium shadow-lg shadow-indigo-500/25 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:hover:translate-y-0"
						>
							{isPending ? "Creando cuenta..." : "Registrarse"}
						</button>
					</form>
					<div className="text-center mt-6">
						<p className="text-slate-400 text-sm">
							¿Ya tienes una cuenta?{" "}
							<Link
								to="/login"
								className="text-indigo-400 hover:text-indigo-300 font-medium transition-colors"
							>
								Inicia sesión
							</Link>
						</p>
					</div>
				</div>
			</div>
		</div>
	);
};
