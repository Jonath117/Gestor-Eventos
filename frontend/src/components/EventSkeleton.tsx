export const EventSkeleton = () => {
	return (
		<div className="bg-slate-900 border border-slate-800 rounded-2xl p-6 flex flex-col animate-pulse relative overflow-hidden">
			{/* Ícono superior */}
			<div className="w-12 h-12 bg-slate-800 rounded-xl mb-4"></div>

			{/* Título del evento */}
			<div className="h-6 bg-slate-800 rounded w-3/4 mb-2"></div>

			{/* Detalles (fecha, ubicación, asistentes) */}
			<div className="space-y-4 mt-auto pt-4">
				<div className="flex items-center gap-3">
					<div className="w-4 h-4 bg-slate-800 rounded-full shrink-0"></div>
					<div className="h-4 bg-slate-800 rounded w-1/2"></div>
				</div>
				<div className="flex items-center gap-3">
					<div className="w-4 h-4 bg-slate-800 rounded-full shrink-0"></div>
					<div className="h-4 bg-slate-800 rounded w-2/3"></div>
				</div>
				<div className="flex items-center gap-3">
					<div className="w-4 h-4 bg-slate-800 rounded-full shrink-0"></div>
					<div className="h-4 bg-slate-800 rounded w-1/3"></div>
				</div>
			</div>

			{/* Sección inferior (Estado e Ingresos) */}
			<div className="mt-6 pt-4 border-t border-slate-800 flex justify-between items-center">
				<div className="w-20 h-6 bg-slate-800 rounded-full"></div>
				<div className="w-16 h-5 bg-slate-800 rounded"></div>
			</div>
		</div>
	);
};
