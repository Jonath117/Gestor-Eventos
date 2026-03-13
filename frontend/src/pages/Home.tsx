import { Link } from "react-router-dom";

export const Home = () => {
	return (
		<div className="min-h-screen bg-slate-950 flex flex-col items-center justify-center relative overflow-hidden">
			{/* Background Gradient Orbs */}
			<div className="absolute top-[-10%] left-[-10%] w-96 h-96 bg-indigo-600 rounded-full mix-blend-multiply filter blur-[128px] opacity-20 animate-pulse"></div>
			<div className="absolute bottom-[-10%] right-[-10%] w-96 h-96 bg-blue-600 rounded-full mix-blend-multiply filter blur-[128px] opacity-20 animate-pulse delay-700"></div>

			<div className="relative z-10 text-center px-6">
				<h1 className="text-5xl md:text-7xl font-extrabold text-transparent bg-clip-text bg-linear-to-r from-blue-400 to-indigo-400 tracking-tight mb-4">
					Campeando
				</h1>
				<p className="text-lg md:text-xl text-slate-300 max-w-2xl mx-auto mb-10 font-medium">
					La plataforma moderna y eficiente para planificar, administrar y
					llevar a cabo tus eventos con total éxito.
				</p>

				<div className="flex flex-col sm:flex-row gap-4 justify-center">
					<Link
						to="/login"
						className="px-8 py-3 rounded-full bg-blue-600 hover:bg-blue-500 text-white font-semibold transition-all shadow-[0_0_20px_rgba(37,99,235,0.4)] hover:shadow-[0_0_30px_rgba(37,99,235,0.6)] transform hover:-translate-y-1"
					>
						Iniciar Sesión
					</Link>
					<button className="px-8 py-3 rounded-full bg-slate-800 hover:bg-slate-700 text-white font-semibold transition-all border border-slate-700 hover:border-slate-500">
						Saber más
					</button>
				</div>
			</div>
		</div>
	);
};
