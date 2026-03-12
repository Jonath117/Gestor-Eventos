import { Link } from "react-router-dom";

export const Settings = () => {
	return (
		<div className="min-h-screen bg-slate-950 text-slate-200">
			{/* Top Bar Navigation */}
			<div className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-md">
				<div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex items-center">
					<Link
						to="/dashboard"
						className="text-slate-400 hover:text-white transition-colors flex items-center text-sm font-medium mr-4"
					>
						<svg
							className="w-5 h-5 mr-1"
							fill="none"
							stroke="currentColor"
							viewBox="0 0 24 24"
						>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M15 19l-7-7 7-7"
							></path>
						</svg>
						Volver
					</Link>
					<h1 className="text-xl font-semibold text-white">Configuración</h1>
				</div>
			</div>

			<main className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
				<div className="bg-slate-900 border border-slate-800 rounded-2xl overflow-hidden mb-8">
					<div className="p-6 border-b border-slate-800">
						<h2 className="text-lg font-semibold text-white">Perfil General</h2>
						<p className="text-sm text-slate-400 mt-1">
							Actualiza tu información personal y foto de perfil.
						</p>
					</div>
					<div className="p-6 space-y-6">
						<div className="flex items-center gap-6">
							<div className="w-20 h-20 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center flex-shrink-0">
								<svg
									className="w-8 h-8 text-slate-500"
									fill="none"
									stroke="currentColor"
									viewBox="0 0 24 24"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth="2"
										d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
									></path>
								</svg>
							</div>
							<div>
								<button className="px-4 py-2 border border-slate-700 hover:bg-slate-800 bg-slate-900 text-sm font-medium rounded-lg text-white transition-colors">
									Cambiar Avatar
								</button>
							</div>
						</div>

						<div className="grid grid-cols-1 md:grid-cols-2 gap-6">
							<div>
								<label className="block text-sm font-medium text-slate-300 mb-2">
									Nombre Completo
								</label>
								<input
									type="text"
									defaultValue="Usuario Prueba"
									className="w-full px-4 py-2.5 bg-slate-950 border border-slate-700 rounded-xl text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
								/>
							</div>
							<div>
								<label className="block text-sm font-medium text-slate-300 mb-2">
									Correo Electrónico
								</label>
								<input
									type="email"
									defaultValue="test@test.com"
									className="w-full px-4 py-2.5 bg-slate-950 border border-slate-700 rounded-xl text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
								/>
							</div>
						</div>
					</div>
					<div className="p-4 bg-slate-900/50 border-t border-slate-800 flex justify-end">
						<button className="px-6 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded-lg text-sm font-medium transition-colors">
							Guardar Cambios
						</button>
					</div>
				</div>

				<div className="bg-slate-900 border border-slate-800 rounded-2xl overflow-hidden">
					<div className="p-6 border-b border-slate-800">
						<h2 className="text-lg font-semibold text-white">Preferencias</h2>
					</div>
					<div className="p-6">
						<div className="flex items-center justify-between py-3 border-b border-slate-800/50">
							<div>
								<h3 className="text-sm font-medium text-slate-200">
									Notificaciones Email
								</h3>
								<p className="text-xs text-slate-400 mt-1">
									Recibir correos sobre nuevos registros.
								</p>
							</div>
							<div className="w-11 h-6 bg-blue-600 rounded-full flex items-center p-1 cursor-pointer">
								<div className="w-4 h-4 rounded-full bg-white transform translate-x-5 transition-transform"></div>
							</div>
						</div>
						<div className="flex items-center justify-between py-3">
							<div>
								<h3 className="text-sm font-medium text-slate-200">
									Modo Oscuro
								</h3>
								<p className="text-xs text-slate-400 mt-1">
									Activar tema por defecto oscuro en todo el sistema.
								</p>
							</div>
							<div className="w-11 h-6 bg-blue-600 rounded-full flex items-center p-1 cursor-pointer">
								<div className="w-4 h-4 rounded-full bg-white transform translate-x-5 transition-transform"></div>
							</div>
						</div>
					</div>
				</div>
			</main>
		</div>
	);
};
