import { Link } from "react-router-dom";
import { EventSkeleton } from "../components/EventSkeleton";
import { useAuth } from "../context/AuthContext";
import { useDeleteEvent } from "../hooks/useDeleteEvent";
import { useEvents } from "../hooks/useEvents";

export const Dashboard = () => {
	const { user, logout } = useAuth();
	const { data: events, isLoading, isError } = useEvents();

	const { mutate: deleteEventFn, isPending: isDeleting } = useDeleteEvent();

	if (isError) {
		throw new Error("No se pudieron cargar los eventos"); // Forzamos al ErrorBoundary para que atrape
	}

	return (
		<div className="min-h-screen bg-slate-950 text-slate-200">
			{/* Top Navigation */}
			<nav className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-md sticky top-0 z-50">
				<div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
					<div className="flex items-center justify-between h-16">
						<div className="flex items-center gap-3">
							<div className="w-8 h-8 rounded bg-linear-to-tr from-blue-500 to-indigo-500 flex items-center justify-center shadow-lg shadow-blue-500/20">
								<span className="text-white font-bold text-lg">G</span>
							</div>
							<span className="font-bold text-xl tracking-tight text-white">
								Dashboard
							</span>
						</div>
						<div className="flex items-center gap-6">
							<div className="flex items-center gap-3">
								<div className="w-9 h-9 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-blue-400 font-bold">
									{user?.name?.charAt(0) || "U"}
								</div>
								<div className="hidden md:block text-sm">
									<p className="font-medium text-slate-200 leading-none">
										{user?.name}
									</p>
									<p className="text-slate-500 text-xs mt-1">{user?.email}</p>
								</div>
							</div>
							<button
								onClick={logout}
								className="text-sm text-slate-400 hover:text-red-400 transition-colors font-medium flex items-center gap-2"
							>
								<svg
									className="w-4 h-4"
									fill="none"
									stroke="currentColor"
									viewBox="0 0 24 24"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth="2"
										d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"
									></path>
								</svg>
								Salir
							</button>
						</div>
					</div>
				</div>
			</nav>

			{/* Main Content */}
			<main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10">
				<div className="mb-8 flex flex-col md:flex-row md:items-center justify-between gap-4">
					<div>
						<h1 className="text-3xl font-bold text-white tracking-tight">
							Resumen General
						</h1>
						<p className="text-slate-400 mt-1">
							Aquí está lo que está pasando con tus eventos hoy.
						</p>
					</div>
					<Link
						to="/settings"
						className="inline-flex items-center justify-center px-4 py-2 bg-slate-800 hover:bg-slate-700 border border-slate-700 rounded-xl text-sm font-medium transition-all"
					>
						<svg
							className="w-4 h-4 mr-2 text-slate-400"
							fill="none"
							stroke="currentColor"
							viewBox="0 0 24 24"
						>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"
							></path>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
							></path>
						</svg>
						Configuración
					</Link>
				</div>

				{/* Stat Cards */}
				<div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
					{[
						{
							label: "Eventos Activos",
							value: "12",
							trend: "+2",
							color: "blue",
						},
						{
							label: "Asistentes Totales",
							value: "1,240",
							trend: "+14%",
							color: "indigo",
						},
						{
							label: "Ingresos Estimados",
							value: "$4,500",
							trend: "+5%",
							color: "emerald",
						},
					].map((stat, i) => (
						<div
							key={i}
							className="bg-slate-900 border border-slate-800 rounded-2xl p-6 hover:border-slate-700 transition-all"
						>
							<p className="text-slate-400 text-sm font-medium">{stat.label}</p>
							<div className="mt-4 flex items-end gap-3">
								<h3 className="text-4xl font-bold text-white">{stat.value}</h3>
								<span
									className={`text-sm font-medium text-${stat.color}-400 mb-1`}
								>
									{stat.trend}
								</span>
							</div>
						</div>
					))}
				</div>

				{/* Events Section */}
				<div className="mb-6 flex justify-between items-center mt-12">
					<h2 className="text-xl font-semibold text-white">Tus Eventos</h2>
					{events && events.length > 0 && (
						<button className="px-4 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded-lg text-sm font-medium transition-colors">
							Nuevo Evento
						</button>
					)}
				</div>

				{isLoading ? (
					<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
						{/* Creamos un arreglo falso de 6 elementos para mostrar 6 tarjetas parpadeando */}
						{[1, 2, 3, 4, 5, 6].map((n) => (
							<EventSkeleton key={n} />
						))}
					</div>
				) : events && events.length > 0 ? (
					/* Events Grid */
					<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
						{events.map((event) => (
							<div
								key={event.id}
								className="bg-slate-900 border border-slate-800 rounded-2xl p-6 hover:border-slate-700 transition-all flex flex-col group relative overflow-hidden"
							>
								<div className="w-12 h-12 bg-linear-to-br from-blue-500/10 to-indigo-500/10 border border-blue-500/20 text-blue-400 rounded-xl mb-4 flex items-center justify-center group-hover:scale-110 transition-transform">
									<svg
										className="w-6 h-6"
										fill="none"
										stroke="currentColor"
										viewBox="0 0 24 24"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth="2"
											d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
										></path>
									</svg>
								</div>
								{/* Eliminar */}
								<div className="flex justify-between items-start mb-2">
									<h3 className="text-xl font-semibold text-white">
										{event.name}
									</h3>

									<button
										onClick={() => deleteEventFn(event.id)}
										disabled={isDeleting}
										className="text-slate-500 hover:text-red-500 transition-colors p-1"
										title="Eliminar evento"
									>
										<svg
											className="w-5 h-5"
											fill="none"
											stroke="currentColor"
											viewBox="0 0 24 24"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
											></path>
										</svg>
									</button>
								</div>
								<div className="space-y-3 mt-auto text-sm text-slate-400">
									<div className="flex items-center gap-3 mt-3">
										<svg
											className="w-4 h-4 text-slate-500 shrink-0"
											fill="none"
											stroke="currentColor"
											viewBox="0 0 24 24"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
											></path>
										</svg>
										<span>{new Date(event.date).toLocaleDateString()}</span>
									</div>
									<div className="flex items-center gap-3">
										<svg
											className="w-4 h-4 text-slate-500 shrink-0"
											fill="none"
											stroke="currentColor"
											viewBox="0 0 24 24"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
											></path>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
											></path>
										</svg>
										<span>{event.location}</span>
									</div>
									<div className="flex items-center gap-3">
										<svg
											className="w-4 h-4 text-slate-500 shrink-0"
											fill="none"
											stroke="currentColor"
											viewBox="0 0 24 24"
										>
											<path
												strokeLinecap="round"
												strokeLinejoin="round"
												strokeWidth="2"
												d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"
											></path>
										</svg>
										<span>{event.attendees.toLocaleString()} asistentes</span>
									</div>
								</div>

								<div className="mt-6 pt-4 border-t border-slate-800 flex justify-between items-center">
									<span
										className={`px-3 py-1 rounded-full text-xs font-semibold
										${event.status === "upcoming" ? "bg-blue-500/10 text-blue-400 border border-blue-500/20" : ""}
										${event.status === "active" ? "bg-emerald-500/10 text-emerald-400 border border-emerald-500/20" : ""}
										${event.status === "completed" ? "bg-slate-500/10 text-slate-400 border border-slate-500/20" : ""}
										${event.status === "cancelled" ? "bg-red-500/10 text-red-400 border border-red-500/20" : ""}`}
									>
										{event.status === "upcoming"
											? "Próximo"
											: event.status === "active"
												? "En progreso"
												: event.status === "completed"
													? "Finalizado"
													: "Cancelado"}
									</span>
									<span className="text-white font-medium text-sm">
										${event.revenue.toLocaleString()}
									</span>
								</div>
							</div>
						))}
					</div>
				) : (
					/* Empty State Area (Sin eventos) */
					<div className="h-96 rounded-2xl border-2 border-dashed border-slate-800 flex flex-col items-center justify-center bg-slate-900/30">
						<div className="w-16 h-16 rounded-full bg-slate-800 flex items-center justify-center mb-4">
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
									d="M12 6v6m0 0v6m0-6h6m-6 0H6"
								></path>
							</svg>
						</div>
						<h3 className="text-xl font-semibold text-white mb-2">
							Crear tu primer evento
						</h3>
						<p className="text-slate-400 max-w-sm text-center mb-6">
							Aún no tienes ningún evento programado. Empieza planeando tu
							primera actividad.
						</p>
						<button className="px-6 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded-lg font-medium transition-colors">
							Nuevo Evento
						</button>
					</div>
				)}
			</main>
		</div>
	);
};
