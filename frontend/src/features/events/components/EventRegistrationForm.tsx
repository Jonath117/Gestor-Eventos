import { isAxiosError } from "axios";
import { useState } from "react";
import { LoadingFallback } from "../../../components/LoadingFallback";
import { useEvent } from "../hooks/useEvent";
import { useRegisterParticipant } from "../hooks/useRegisterParticipant";

interface EventRegistrationFormProps {
	eventId: string;
}

export const EventRegistrationForm = ({
	eventId,
}: EventRegistrationFormProps) => {
	const {
		data: event,
		isLoading: isLoadingEvent,
		isError: isEventError,
		error: eventError,
	} = useEvent(eventId);

	const {
		mutate: register,
		isPending,
		isError: isRegisterError,
		error: registerError,
		isSuccess,
		data: registerData,
		reset,
	} = useRegisterParticipant(eventId);

	const [fullName, setFullName] = useState("");
	const [email, setEmail] = useState("");

	const handleSubmit = (e: React.FormEvent) => {
		e.preventDefault();
		register({ fullName, email });
	};

	// --- Estado de carga del evento ---
	if (isLoadingEvent) {
		return <LoadingFallback />;
	}

	// --- Evento no encontrado (404) ---
	if (isEventError) {
		const is404 =
			isAxiosError(eventError) && eventError.response?.status === 404;
		return (
			<div className="text-center py-16">
				<div className="w-20 h-20 mx-auto mb-6 bg-red-500/10 rounded-full flex items-center justify-center border border-red-500/20">
					<svg
						className="w-10 h-10 text-red-400"
						fill="none"
						stroke="currentColor"
						viewBox="0 0 24 24"
					>
						<path
							strokeLinecap="round"
							strokeLinejoin="round"
							strokeWidth="2"
							d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L4.082 16.5c-.77.833.192 2.5 1.732 2.5z"
						/>
					</svg>
				</div>
				<h2 className="text-2xl font-bold text-white mb-2">
					{is404 ? "Evento no encontrado" : "Error al cargar el evento"}
				</h2>
				<p className="text-slate-400">
					{is404
						? "El evento que buscas no existe o fue eliminado."
						: "Ocurrió un error inesperado. Intenta de nuevo más tarde."}
				</p>
			</div>
		);
	}

	// --- Evento ya finalizó ---
	if (event && event.date < new Date().toISOString()) {
		return (
			<div className="text-center py-16">
				<div className="w-20 h-20 mx-auto mb-6 bg-amber-500/10 rounded-full flex items-center justify-center border border-amber-500/20">
					<svg
						className="w-10 h-10 text-amber-400"
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
				</div>
				<h2 className="text-2xl font-bold text-white mb-2">
					Este evento ya ha finalizado
				</h2>
				<p className="text-slate-400">
					El evento <span className="text-white font-medium">{event.name}</span>{" "}
					ya terminó y no acepta nuevas inscripciones.
				</p>
			</div>
		);
	}

	// --- Evento cancelado (removed: no status field in current API) ---

	// --- Cupos agotados ---
	if (event && event.currentParticipantsCount >= event.maxCapacity) {
		return (
			<div className="text-center py-16">
				<div className="w-20 h-20 mx-auto mb-6 bg-orange-500/10 rounded-full flex items-center justify-center border border-orange-500/20">
					<svg
						className="w-10 h-10 text-orange-400"
						fill="none"
						stroke="currentColor"
						viewBox="0 0 24 24"
					>
						<path
							strokeLinecap="round"
							strokeLinejoin="round"
							strokeWidth="2"
							d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z"
						/>
					</svg>
				</div>
				<h2 className="text-2xl font-bold text-white mb-2">Cupos agotados</h2>
				<p className="text-slate-400">
					Lamentablemente, el evento{" "}
					<span className="text-white font-medium">{event.name}</span> ya no
					tiene cupos disponibles.
				</p>
			</div>
		);
	}

	// --- Registro exitoso ---
	if (isSuccess && registerData) {
		return (
			<div className="text-center py-16">
				<div className="w-20 h-20 mx-auto mb-6 bg-emerald-500/10 rounded-full flex items-center justify-center border border-emerald-500/20 animate-bounce-once">
					<svg
						className="w-10 h-10 text-emerald-400"
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
				</div>
				<h2 className="text-2xl font-bold text-white mb-2">
					¡Registro exitoso!
				</h2>
				<p className="text-slate-400 mb-4">
					Te has registrado exitosamente al evento{" "}
					<span className="text-white font-medium">{event?.name}</span>.
				</p>
				<div className="inline-block bg-slate-800/60 border border-slate-700 rounded-xl px-6 py-3">
					<span className="text-xs text-slate-500 uppercase tracking-wider font-medium">
						ID de participante
					</span>
					<p className="text-emerald-400 font-mono text-sm mt-1">
						{registerData.participantId}
					</p>
				</div>
			</div>
		);
	}

	// --- Helper para mostrar el error de registro ---
	const getRegisterErrorMessage = (): string => {
		if (!isRegisterError || !registerError) return "";

		if (isAxiosError(registerError)) {
			const status = registerError.response?.status;
			const serverMessage =
				(registerError.response?.data as { error?: string })?.error ?? "";

			if (status === 404) {
				return "El evento no fue encontrado.";
			}
			if (status === 400) {
				return serverMessage || "No se pudo completar el registro.";
			}
			if (status === 429) {
				return "Demasiados intentos. Por favor, espera un momento antes de volver a intentarlo.";
			}
		}

		return "Ocurrió un error inesperado. Intenta de nuevo.";
	};

	// --- Formulario de registro ---
	return (
		<div>
			{/* Encabezado del evento */}
			{event && (
				<div className="text-center mb-10">
					<div className="w-16 h-16 bg-emerald-600/20 rounded-2xl mx-auto mb-4 flex items-center justify-center border border-emerald-500/30">
						<svg
							className="w-8 h-8 text-emerald-400"
							fill="none"
							stroke="currentColor"
							viewBox="0 0 24 24"
						>
							<path
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z"
							/>
						</svg>
					</div>
					<h1 className="text-3xl font-bold text-white mb-2">
						Inscripción al evento
					</h1>
					<p className="text-slate-400 text-sm">{event.name}</p>
					<p className="text-slate-500 text-xs mt-1">
						{new Date(event.date).toLocaleDateString("es", {
							year: "numeric",
							month: "long",
							day: "numeric",
						})}
					</p>
				</div>
			)}

			<div className="space-y-6">
				{/* Error banner */}
				{isRegisterError && (
					<div
						id="registration-error"
						className="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-red-400 text-sm"
					>
						{getRegisterErrorMessage()}
					</div>
				)}

				<form
					id="registration-form"
					onSubmit={handleSubmit}
					className="space-y-4"
				>
					<div>
						<label
							htmlFor="fullName"
							className="block text-sm font-medium text-slate-400 mb-1"
						>
							Nombre completo
						</label>
						<input
							id="fullName"
							type="text"
							value={fullName}
							onChange={(e) => setFullName(e.target.value)}
							onFocus={() => {
								if (isRegisterError) reset();
							}}
							placeholder="Juan Pérez"
							className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white placeholder-slate-600 focus:outline-none focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500 transition-colors"
							required
						/>
					</div>

					<div>
						<label
							htmlFor="email"
							className="block text-sm font-medium text-slate-400 mb-1"
						>
							Correo electrónico
						</label>
						<input
							id="email"
							type="email"
							value={email}
							onChange={(e) => setEmail(e.target.value)}
							onFocus={() => {
								if (isRegisterError) reset();
							}}
							placeholder="juan.perez@ejemplo.com"
							className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white placeholder-slate-600 focus:outline-none focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500 transition-colors"
							required
						/>
					</div>

					<button
						id="registration-submit"
						type="submit"
						disabled={isPending}
						className="w-full mt-6 py-4 px-4 bg-linear-to-r from-emerald-600 to-teal-600 hover:from-emerald-500 hover:to-teal-500 disabled:opacity-50 disabled:cursor-not-allowed text-white rounded-xl font-medium shadow-lg shadow-emerald-500/25 transition-all transform hover:-translate-y-0.5 active:translate-y-0 disabled:hover:translate-y-0"
					>
						{isPending ? "Registrando..." : "Registrarme"}
					</button>
				</form>
			</div>
		</div>
	);
};
