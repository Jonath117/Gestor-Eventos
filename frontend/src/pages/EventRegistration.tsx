import { useParams } from "react-router-dom";
import { ErrorBoundary } from "../components/ErrorBoundary";
import { EventRegistrationForm } from "../features/events/components/EventRegistrationForm";

export const EventRegistration = () => {
	const { eventId } = useParams<{ eventId: string }>();

	if (!eventId) {
		return (
			<div className="min-h-screen bg-slate-950 flex items-center justify-center px-4">
				<div className="text-center">
					<h2 className="text-2xl font-bold text-white mb-2">
						Evento no especificado
					</h2>
					<p className="text-slate-400">
						No se proporcionó un identificador de evento válido.
					</p>
				</div>
			</div>
		);
	}

	return (
		<ErrorBoundary>
			<div className="min-h-screen bg-slate-950 flex items-center justify-center px-4 relative overflow-hidden">
				{/* Soft background glow */}
				<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-200 h-200 bg-emerald-900/20 rounded-full blur-[120px] pointer-events-none" />

				<div className="w-full max-w-md bg-slate-900/60 backdrop-blur-xl border border-slate-800 rounded-3xl p-8 shadow-2xl relative z-10">
					<EventRegistrationForm eventId={eventId} />
				</div>
			</div>
		</ErrorBoundary>
	);
};
