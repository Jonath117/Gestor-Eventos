export const LoadingFallback = () => {
	return (
		<div className="min-h-screen bg-slate-950 flex items-center justify-center">
			<div className="flex flex-col items-center gap-4">
				{/* Spinner */}
				<div className="relative w-16 h-16">
					<div className="absolute top-0 left-0 w-full h-full border-4 border-slate-800 rounded-full"></div>
					<div className="absolute top-0 left-0 w-full h-full border-4 border-blue-500 rounded-full border-t-transparent animate-spin"></div>
				</div>
				<span className="text-slate-400 font-medium animate-pulse">
					Cargando...
				</span>
			</div>
		</div>
	);
};
