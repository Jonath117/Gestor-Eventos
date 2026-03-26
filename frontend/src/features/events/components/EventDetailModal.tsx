import { useState } from "react";
import { useEvent } from "../hooks/useEvent";

interface EventDetailModalProps {
	eventId: string;
	eventName: string;
	onClose: () => void;
}

export const EventDetailModal = ({
	eventId,
	eventName,
	onClose,
}: EventDetailModalProps) => {
	const [copied, setCopied] = useState(false);
	const { data: event, isLoading } = useEvent(eventId);

	const registrationUrl = `${window.location.origin}/events/${eventId}/register`;

	const handleCopyLink = async () => {
		try {
			await navigator.clipboard.writeText(registrationUrl);
			setCopied(true);
			setTimeout(() => setCopied(false), 2000);
		} catch {
			const textarea = document.createElement("textarea");
			textarea.value = registrationUrl;
			document.body.appendChild(textarea);
			textarea.select();
			document.execCommand("copy");
			document.body.removeChild(textarea);
			setCopied(true);
			setTimeout(() => setCopied(false), 2000);
		}
	};

	const spotsLeft = event
		? event.maxCapacity - event.currentParticipantsCount
		: 0;
	const occupancyPercent =
		event && event.maxCapacity > 0
			? Math.round((event.currentParticipantsCount / event.maxCapacity) * 100)
			: 0;

	return (
		<div
			className="fixed inset-0 z-[100] flex items-center justify-center p-4"
			onClick={onClose}
			onKeyDown={(e) => {
				if (e.key === "Escape") onClose();
			}}
		>
			{/* Backdrop */}
			<div className="absolute inset-0 bg-black/60 backdrop-blur-sm" />

			{/* Modal */}
			<div
				className="relative bg-slate-900 border border-slate-700 rounded-2xl shadow-2xl shadow-black/50 w-full max-w-lg"
				onClick={(e) => e.stopPropagation()}
				onKeyDown={(e) => e.stopPropagation()}
			>
				{/* Header */}
				<div className="flex items-start justify-between p-6 pb-0">
					<div className="flex items-center gap-4">
						<div className="w-12 h-12 bg-linear-to-br from-blue-500/10 to-indigo-500/10 border border-blue-500/20 text-blue-400 rounded-xl flex items-center justify-center shrink-0">
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
						<div>
							<h2 className="text-xl font-bold text-white">
								{event?.name ?? eventName}
							</h2>
						</div>
					</div>
					<button
						onClick={onClose}
						className="text-slate-500 hover:text-white transition-colors p-1 rounded-lg hover:bg-slate-800"
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
								d="M6 18L18 6M6 6l12 12"
							/>
						</svg>
					</button>
				</div>

				{/* Content */}
				<div className="p-6 space-y-5">
					{isLoading ? (
						<div className="space-y-3">
							{[1, 2, 3, 4].map((n) => (
								<div
									key={n}
									className="h-5 bg-slate-800 rounded animate-pulse"
									style={{ width: `${60 + n * 8}%` }}
								/>
							))}
						</div>
					) : event ? (
						<>
							{/* Event details */}
							<div className="space-y-3">
								{/* Fecha */}
								<div className="flex items-center gap-3 text-sm">
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
									<span className="text-slate-300">
										{new Date(event.date).toLocaleDateString("es", {
											weekday: "long",
											year: "numeric",
											month: "long",
											day: "numeric",
										})}
									</span>
								</div>

								{/* Hora */}
								<div className="flex items-center gap-3 text-sm">
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
									<span className="text-slate-300">
										{new Date(event.date).toLocaleTimeString("es", {
											hour: "2-digit",
											minute: "2-digit",
										})}
									</span>
								</div>

								{/* Participantes inscritos */}
								<div className="flex items-center gap-3 text-sm">
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
										/>
									</svg>
									<span className="text-slate-300">
										{event.currentParticipantsCount} / {event.maxCapacity}{" "}
										inscritos
									</span>
								</div>

								{/* Cupos disponibles */}
								<div className="flex items-center gap-3 text-sm">
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
											d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
										/>
									</svg>
									<span
										className={
											spotsLeft > 0 ? "text-emerald-400" : "text-red-400"
										}
									>
										{spotsLeft > 0
											? `${spotsLeft} cupos disponibles`
											: "Sin cupos disponibles"}
									</span>
								</div>
							</div>

							{/* Occupancy bar */}
							<div>
								<div className="flex justify-between text-xs text-slate-500 mb-1.5">
									<span>Ocupación</span>
									<span>{occupancyPercent}%</span>
								</div>
								<div className="h-2 bg-slate-800 rounded-full overflow-hidden">
									<div
										className={`h-full rounded-full transition-all ${
											occupancyPercent >= 90
												? "bg-red-500"
												: occupancyPercent >= 60
													? "bg-amber-500"
													: "bg-emerald-500"
										}`}
										style={{ width: `${occupancyPercent}%` }}
									/>
								</div>
							</div>
						</>
					) : null}

					{/* Divider */}
					<div className="border-t border-slate-800" />

					{/* Registration link section */}
					<div>
						<p className="text-sm text-slate-400 mb-3">
							<svg
								className="w-4 h-4 inline-block mr-1.5 -mt-0.5 text-indigo-400"
								fill="none"
								stroke="currentColor"
								viewBox="0 0 24 24"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth="2"
									d="M13.828 10.172a4 4 0 00-5.656 0l-4 4a4 4 0 105.656 5.656l1.102-1.101m-.758-4.899a4 4 0 005.656 0l4-4a4 4 0 00-5.656-5.656l-1.1 1.1"
								/>
							</svg>
							Comparte este enlace con los asistentes para que puedan
							inscribirse al evento de forma pública.
						</p>
						<div className="flex items-center gap-2 bg-slate-800/60 border border-slate-700 rounded-xl px-4 py-3">
							<code className="text-sm text-indigo-300 font-mono truncate flex-1">
								{registrationUrl}
							</code>
							<button
								onClick={handleCopyLink}
								className={`shrink-0 p-2 rounded-lg transition-all ${
									copied
										? "bg-emerald-500/10 text-emerald-400 border border-emerald-500/20"
										: "bg-slate-700 hover:bg-slate-600 text-slate-300 hover:text-white border border-transparent"
								}`}
								title="Copiar enlace de inscripción"
							>
								{copied ? (
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
											d="M5 13l4 4L19 7"
										/>
									</svg>
								) : (
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
											d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z"
										/>
									</svg>
								)}
							</button>
						</div>
						{copied && (
							<p className="text-xs text-emerald-400 mt-2 flex items-center gap-1">
								<svg
									className="w-3 h-3"
									fill="none"
									stroke="currentColor"
									viewBox="0 0 24 24"
								>
									<path
										strokeLinecap="round"
										strokeLinejoin="round"
										strokeWidth="2"
										d="M5 13l4 4L19 7"
									/>
								</svg>
								¡Enlace copiado al portapapeles!
							</p>
						)}
					</div>
				</div>
			</div>
		</div>
	);
};
