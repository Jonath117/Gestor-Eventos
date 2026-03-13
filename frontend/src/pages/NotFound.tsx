import { Link } from "react-router-dom";

export const NotFound = () => {
	return (
		<div className="min-h-screen bg-slate-950 flex items-center justify-center px-4 relative overflow-hidden">
			{/* Background */}
			<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-150 h-150 bg-red-900/10 rounded-full blur-[100px] pointer-events-none"></div>

			<div className="relative z-10 text-center max-w-md">
				<div className="text-9xl font-black text-transparent bg-clip-text bg-linear-to-br from-slate-700 to-slate-500 mb-4 opacity-50">
					404
				</div>
				<h1 className="text-3xl font-bold text-white mb-4">
					Página no encontrada
				</h1>
				<p className="text-slate-400 mb-8">
					Lo sentimos, la ruta que estás intentando buscar no existe o ha sido
					movida a otro lugar.
				</p>
				<div className="flex justify-center gap-4">
					<Link
						to="/"
						className="px-6 py-3 bg-blue-600 hover:bg-blue-500 text-white rounded-xl font-medium transition-all transform hover:-translate-y-0.5 shadow-lg shadow-blue-500/20"
					>
						Ir al Inicio
					</Link>
					<button
						onClick={() => window.history.back()}
						className="px-6 py-3 bg-slate-800 hover:bg-slate-700 text-white rounded-xl font-medium transition-all border border-slate-700 hover:border-slate-500"
					>
						Volver Atrás
					</button>
				</div>
			</div>
		</div>
	);
};
