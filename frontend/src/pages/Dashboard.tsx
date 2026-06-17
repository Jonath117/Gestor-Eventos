import { useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../features/auth/context/AuthContext";
import { EventDetailModal } from "../features/events/components/EventDetailModal";
import { EventSkeleton } from "../features/events/components/EventSkeleton";
import { useDeleteEvent } from "../features/events/hooks/useDeleteEvent";
import { useEvents } from "../features/events/hooks/useEvents";
import { useOrganizations } from "../features/organizations/hooks/useOrganizations";

export const Dashboard = () => {
	const { logout } = useAuth();

	const [activeTenantId, setActiveTenantId] = useState<string | null>(() => {
		const stored = localStorage.getItem("tenantId");
		if (!stored) return null;
		try {
			return stored.startsWith('"') ? JSON.parse(stored) : stored;
		} catch {
			return stored;
		}
	});

	const {
		data: organizations,
		isLoading: isLoadingOrgs,
		isError: isOrgsError,
	} = useOrganizations();

	const { data: events, isLoading, isError } = useEvents(activeTenantId);
	const { mutate: deleteEventFn, isPending: isDeleting } = useDeleteEvent();

	const [selectedEvent, setSelectedEvent] = useState<{
		id: string;
		name: string;
	} | null>(null);

	const [isTenantSelectorOpen, setIsTenantSelectorOpen] = useState(false);

	const selectTenant = (id: string) => {
		localStorage.setItem("tenantId", id);
		setActiveTenantId(id);
		setIsTenantSelectorOpen(false);
	};

	if (isError || isOrgsError) {
		throw new Error("No se pudieron cargar los datos");
	}

	if (isLoadingOrgs) {
		return (
			<div className="min-h-screen bg-slate-950 flex flex-col items-center justify-center gap-4">
				<div className="w-12 h-12 rounded-full border-4 border-blue-500 border-t-transparent animate-spin" />
				<p className="text-slate-400 text-sm animate-pulse">
					Cargando organizaciones...
				</p>
			</div>
		);
	}

	if (!organizations || organizations.length === 0) {
		return (
			<div className="min-h-screen bg-slate-950 text-slate-200 flex items-center justify-center p-6">
				<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-150 h-150 bg-blue-900/10 rounded-full blur-[100px] pointer-events-none" />
				<div className="max-w-md w-full bg-slate-900/60 backdrop-blur-xl border border-slate-800 rounded-3xl p-8 shadow-2xl relative z-10 text-center">
					<div className="w-16 h-16 bg-blue-500/10 border border-blue-500/20 text-blue-400 rounded-2xl mx-auto mb-6 flex items-center justify-center">
						<svg
							className="w-8 h-8"
							fill="none"
							stroke="currentColor"
							viewBox="0 0 24 24"
						>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"
							/>
						</svg>
					</div>
					<h1 className="text-2xl font-bold text-white mb-3">
						Crea tu Organización
					</h1>
					<p className="text-slate-400 text-sm mb-8 leading-relaxed">
						Para comenzar a crear y gestionar eventos, primero necesitas
						configurar una organización. Las organizaciones te permiten
						organizar equipos y controlar accesos.
					</p>
					<Link
						to="/organizations/new"
						className="block w-full py-3 px-4 bg-blue-600 hover:bg-blue-500 text-white rounded-xl font-semibold shadow-lg shadow-blue-500/20 transition-all text-center animate-pulse"
					>
						Crear Nueva Organización
					</Link>
					<button
						onClick={logout}
						className="mt-6 text-sm text-slate-500 hover:text-slate-400 font-medium transition-colors"
					>
						Salir de la cuenta
					</button>
				</div>
			</div>
		);
	}

	const activeOrg = organizations.find((org) => org.id === activeTenantId);
	const hasValidTenant = !!activeOrg;

	if (!hasValidTenant) {
		return (
			<div className="min-h-screen bg-slate-950 text-slate-200 flex items-center justify-center p-6">
				<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-150 h-150 bg-indigo-900/10 rounded-full blur-[100px] pointer-events-none" />
				<div className="max-w-2xl w-full bg-slate-900/60 backdrop-blur-xl border border-slate-800 rounded-3xl p-8 md:p-10 shadow-2xl relative z-10">
					<div className="text-center mb-8">
						<h1 className="text-3xl font-bold text-white mb-2">
							Selecciona tu Organización
						</h1>
						<p className="text-slate-400 text-sm">
							Elige la organización con la que deseas trabajar hoy para ver sus
							eventos.
						</p>
					</div>

					<div className="grid grid-cols-1 sm:grid-cols-2 gap-4 max-h-[350px] overflow-y-auto pr-1">
						{organizations.map((org) => (
							<button
								key={org.id}
								onClick={() => selectTenant(org.id)}
								className="flex flex-col items-start p-5 bg-slate-800/40 hover:bg-slate-800/80 border border-slate-700/50 hover:border-blue-500/50 rounded-2xl transition-all text-left group"
							>
								<div className="w-10 h-10 bg-blue-500/10 border border-blue-500/20 text-blue-400 rounded-xl mb-3 flex items-center justify-center group-hover:scale-105 transition-transform">
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
											d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"
										/>
									</svg>
								</div>
								<span className="font-semibold text-white group-hover:text-blue-400 transition-colors">
									{org.name}
								</span>
								<span className="text-xs text-slate-500 mt-1">
									ID: {org.id.substring(0, 8)}...
								</span>
							</button>
						))}

						<Link
							to="/organizations/new"
							className="flex flex-col items-center justify-center p-5 border border-dashed border-slate-700 hover:border-blue-500/50 rounded-2xl hover:bg-slate-800/20 transition-all text-center group min-h-[120px]"
						>
							<div className="w-10 h-10 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-slate-400 group-hover:text-blue-400 group-hover:border-blue-500/30 mb-2 transition-all">
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
										d="M12 6v6m0 0v6m0-6h6m-6 0H6"
									/>
								</svg>
							</div>
							<span className="font-medium text-slate-300 group-hover:text-white transition-colors">
								Crear nueva organización
							</span>
						</Link>
					</div>

					<div className="border-t border-slate-800/60 mt-8 pt-6 flex justify-between items-center">
						<button
							onClick={logout}
							className="text-sm text-slate-500 hover:text-red-400 font-medium transition-colors"
						>
							Cerrar sesión
						</button>
					</div>
				</div>
			</div>
		);
	}

	return (
		<div className="min-h-screen bg-slate-950 text-slate-200">
			{/* Top Navigation */}
			<nav className="border-b border-slate-800 bg-slate-900/50 backdrop-blur-md sticky top-0 z-50">
				<div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
					<div className="flex items-center justify-between h-16">
						<div className="flex items-center gap-3">
							<div className="w-8 h-8 rounded bg-linear-to-tr from-blue-500 to-indigo-500 flex items-center justify-center shadow-lg shadow-blue-500/20 animate-pulse">
								<span className="text-white font-bold text-lg">G</span>
							</div>
							<span className="font-bold text-xl tracking-tight text-white hidden md:inline">
								Dashboard
							</span>
							<span className="text-slate-700 font-medium hidden md:inline">
								/
							</span>

							{/* Organization Switcher Dropdown */}
							<div className="relative">
								<button
									onClick={() => setIsTenantSelectorOpen(!isTenantSelectorOpen)}
									className="flex items-center gap-2 px-3 py-1.5 rounded-xl bg-slate-900/80 border border-slate-800 hover:border-slate-700 text-sm font-medium text-slate-200 transition-all hover:bg-slate-850"
								>
									<span className="max-w-[150px] truncate">
										{activeOrg?.name}
									</span>
									<svg
										className={`w-4 h-4 text-slate-500 transition-transform ${isTenantSelectorOpen ? "rotate-180" : ""}`}
										fill="none"
										stroke="currentColor"
										viewBox="0 0 24 24"
									>
										<path
											strokeLinecap="round"
											strokeLinejoin="round"
											strokeWidth="2"
											d="M19 9l-7 7-7-7"
										/>
									</svg>
								</button>

								{isTenantSelectorOpen && (
									<>
										{/* Overlay to close the dropdown */}
										<div
											className="fixed inset-0 z-10"
											onClick={() => setIsTenantSelectorOpen(false)}
										/>
										<div className="absolute left-0 mt-2 w-64 bg-slate-900/95 backdrop-blur-md border border-slate-800 rounded-2xl shadow-2xl py-2 z-20 animate-in fade-in-50 slide-in-from-top-1 duration-200">
											<p className="text-slate-500 text-[10px] font-semibold tracking-wider uppercase px-4 py-2">
												Mis Organizaciones
											</p>
											<div className="max-h-60 overflow-y-auto px-2 space-y-1">
												{organizations?.map((org) => (
													<button
														key={org.id}
														onClick={() => selectTenant(org.id)}
														className={`w-full flex items-center justify-between px-3 py-2 rounded-xl text-sm font-medium text-left transition-colors ${org.id === activeTenantId ? "bg-blue-600/10 text-blue-400 border border-blue-500/25" : "text-slate-400 hover:text-white hover:bg-slate-800/50"}`}
													>
														<span className="truncate">{org.name}</span>
														{org.id === activeTenantId && (
															<svg
																className="w-4 h-4 shrink-0 text-blue-400"
																fill="none"
																stroke="currentColor"
																viewBox="0 0 24 24"
															>
																<path
																	strokeLinecap="round"
																	strokeLinejoin="round"
																	strokeWidth="3"
																	d="M5 13l4 4L19 7"
																/>
															</svg>
														)}
													</button>
												))}
											</div>
											<div className="border-t border-slate-800 mt-2 pt-2 px-2">
												<Link
													to="/organizations/new"
													className="flex items-center gap-2 px-3 py-2 rounded-xl text-sm font-medium text-slate-400 hover:text-white hover:bg-slate-800/50 transition-colors"
													onClick={() => setIsTenantSelectorOpen(false)}
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
															d="M12 4v16m8-8H4"
														/>
													</svg>
													Crear Organización
												</Link>
												<button
													onClick={() => {
														localStorage.removeItem("tenantId");
														setActiveTenantId(null);
														setIsTenantSelectorOpen(false);
													}}
													className="w-full flex items-center gap-2 px-3 py-2 rounded-xl text-sm font-medium text-slate-400 hover:text-red-400 hover:bg-red-500/5 transition-colors text-left"
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
															d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4-4m-4 4l4 4"
														/>
													</svg>
													Cambiar de Organización
												</button>
											</div>
										</div>
									</>
								)}
							</div>
						</div>
						<div className="flex items-center gap-6">
							<div className="flex items-center gap-3">
								<div className="w-9 h-9 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-blue-400 font-bold">
									U
								</div>
								<div className="hidden md:block text-sm">
									<p className="font-medium text-slate-200 leading-none">
										Administrador
									</p>
									<p className="text-slate-500 text-medium mt-1">
										admin@gestor.com
									</p>
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
									/>
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
							/>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"
							/>
						</svg>
						Configuración
					</Link>
				</div>

				{/* Stat Cards */}
				<div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
					{[
						{
							label: "Eventos Activos",
							value: events ? events.length.toString() : "0",
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
					<div className="flex gap-3">
						<Link
							to="/organizations/new"
							className="px-4 py-2 bg-slate-800 hover:bg-slate-700 text-white rounded-lg text-sm font-medium transition-colors"
						>
							Nueva Org
						</Link>
						<Link
							to="/events/new"
							className="px-4 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded-lg text-sm font-medium transition-colors"
						>
							Nuevo Evento
						</Link>
					</div>
				</div>
				{isLoading ? (
					<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
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
								className="bg-slate-900 border border-slate-800 rounded-2xl p-6 hover:border-slate-700 transition-all flex flex-col group relative overflow-hidden cursor-pointer"
								onClick={() =>
									setSelectedEvent({ id: event.id, name: event.name })
								}
								onKeyDown={(e) => {
									if (e.key === "Enter" || e.key === " ")
										setSelectedEvent({ id: event.id, name: event.name });
								}}
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
										/>
									</svg>
								</div>
								{/* Eliminar */}
								<div className="flex justify-between items-start mb-2">
									<h3 className="text-xl font-semibold text-white">
										{event.name}
									</h3>

									<button
										onClick={(e) => {
											e.stopPropagation();
											deleteEventFn(event.id);
										}}
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
											/>
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
											/>
										</svg>
										<span>
											{new Date(event.startDate).toLocaleDateString("es", {
												year: "numeric",
												month: "long",
												day: "numeric",
											})}
										</span>
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
												d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"
											/>
										</svg>
										<span>
											{new Date(event.startDate).toLocaleTimeString("es", {
												hour: "2-digit",
												minute: "2-digit",
											})}
										</span>
									</div>
								</div>

								<div className="mt-6 pt-4 border-t border-slate-800 flex items-center justify-center gap-2 text-sm text-slate-500 group-hover:text-blue-400 transition-colors">
									<span>Toca para ver más detalles</span>
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
											d="M9 5l7 7-7 7"
										/>
									</svg>
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
								/>
							</svg>
						</div>
						<h3 className="text-xl font-semibold text-white mb-2">
							Crear tu primer evento
						</h3>
						<p className="text-slate-400 max-w-sm text-center mb-6">
							Aún no tienes ningún evento programado. Empieza planeando tu
							primera actividad.
						</p>
						<Link
							to="/events/new"
							className="px-6 py-2 bg-blue-600 hover:bg-blue-500 text-white rounded-lg font-medium transition-colors"
						>
							Nuevo Evento
						</Link>
					</div>
				)}
			</main>

			{/* Event Detail Modal */}
			{selectedEvent && (
				<EventDetailModal
					eventId={selectedEvent.id}
					eventName={selectedEvent.name}
					onClose={() => setSelectedEvent(null)}
				/>
			)}
		</div>
	);
};
