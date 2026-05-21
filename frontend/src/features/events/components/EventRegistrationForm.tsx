import { isAxiosError } from "axios";
import { useEffect, useState } from "react";
import { LoadingFallback } from "../../../components/LoadingFallback";
import { useEvent } from "../hooks/useEvent";
import {
	useRequestOtp,
	useSubmitRegistration,
	useVerifyOtp,
} from "../hooks/useRegistration";

interface EventRegistrationFormProps {
	eventId: string;
}

type RegistrationStep = "DETAILS" | "OTP" | "VERIFIED" | "SUCCESS";

export const EventRegistrationForm = ({
	eventId,
}: EventRegistrationFormProps) => {
	const {
		data: event,
		isLoading: isLoadingEvent,
		isError: isEventError,
		error: eventError,
	} = useEvent(eventId);

	// Registration hooks
	const { mutate: requestOtp, isPending: isRequestingOtp } =
		useRequestOtp(eventId);
	const { mutate: verifyOtp, isPending: isVerifyingOtp } =
		useVerifyOtp(eventId);
	const { mutate: submitRegistration, isPending: isSubmitting } =
		useSubmitRegistration(eventId);

	// Local state
	const [step, setStep] = useState<RegistrationStep>("DETAILS");
	const [fullName, setFullName] = useState("");
	const [email, setEmail] = useState("");
	const [phone, setPhone] = useState("");
	const [otp, setOtp] = useState("");
	const [error, setError] = useState<string | null>(null);
	const [timer, setTimer] = useState(0);
	const [orderId, setOrderId] = useState<string | null>(null);

	// Timer logic
	useEffect(() => {
		let interval: ReturnType<typeof setInterval> | undefined;
		if (timer > 0) {
			interval = setInterval(() => {
				setTimer((prev) => prev - 1);
			}, 1000);
		}
		return () => {
			if (interval) clearInterval(interval);
		};
	}, [timer]);

	// Handlers
	const handleRequestOtp = (e: React.FormEvent) => {
		e.preventDefault();
		setError(null);
		requestOtp(
			{ email, fullName },
			{
				onSuccess: () => {
					setStep("OTP");
					setTimer(180); // 3 minutes
				},
				onError: (err) => {
					setError("Error al solicitar OTP. Reintenta.");
					console.error(err);
				},
			},
		);
	};

	const handleVerifyOtp = () => {
		setError(null);
		verifyOtp(
			{ email, otp },
			{
				onSuccess: () => {
					setStep("VERIFIED");
					setTimer(0);
				},
				onError: (err) => {
					if (isAxiosError(err) && err.response?.status === 400) {
						setError("OTP inválido o expirado.");
					} else {
						setError("Error de verificación.");
					}
				},
			},
		);
	};

	const handleSubmitRegistration = () => {
		setError(null);
		submitRegistration(
			{ email, fullName, phone },
			{
				onSuccess: (data) => {
					const result = data as { orderId: string };
					setOrderId(result.orderId);
					setStep("SUCCESS");
				},
				onError: (err) => {
					setError("Error al procesar la inscripción.");
					console.error(err);
				},
			},
		);
	};

	// --- Component Views ---

	if (isLoadingEvent) return <LoadingFallback />;

	if (isEventError) {
		const is404 =
			isAxiosError(eventError) && eventError.response?.status === 404;
		return (
			<div className="text-center py-16">
				<h2 className="text-2xl font-bold text-white mb-2">
					{is404 ? "Evento no encontrado" : "Error al cargar el evento"}
				</h2>
			</div>
		);
	}

	if (step === "SUCCESS") {
		return (
			<div className="text-center py-16 animate-fade-in">
				<div className="w-20 h-20 mx-auto mb-6 bg-emerald-500/10 rounded-full flex items-center justify-center border border-emerald-500/20">
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
					¡Inscripción Exitosa!
				</h2>
				<p className="text-slate-400 mb-6">
					Te has registrado al evento <strong>{event?.name}</strong>.
				</p>
				<div className="bg-slate-800/50 p-4 rounded-xl border border-slate-700">
					<p className="text-xs text-slate-500 uppercase font-bold mb-1">
						Orden de Registro
					</p>
					<p className="text-emerald-400 font-mono text-sm">{orderId}</p>
				</div>
			</div>
		);
	}

	return (
		<div>
			{/* Event Header */}
			{event && (
				<div className="text-center mb-10">
					<h1 className="text-3xl font-bold text-white mb-2">{event.name}</h1>
					<p className="text-slate-400 text-sm">Registro Público Individual</p>
					<p className="text-slate-500 text-xs mt-1">
						{new Date(event.startDate).toLocaleDateString("es", {
							year: "numeric",
							month: "long",
							day: "numeric",
						})}
					</p>
				</div>
			)}

			<div className="space-y-6">
				{error && (
					<div className="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-red-400 text-sm">
						{error}
					</div>
				)}

				{step === "DETAILS" && (
					<form onSubmit={handleRequestOtp} className="space-y-4">
						<div>
							<label className="block text-sm font-medium text-slate-400 mb-1">
								Nombre Completo
							</label>
							<input
								type="text"
								value={fullName}
								onChange={(e) => setFullName(e.target.value)}
								className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500"
								required
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-slate-400 mb-1">
								Email
							</label>
							<input
								type="email"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500"
								required
							/>
						</div>
						<div>
							<label className="block text-sm font-medium text-slate-400 mb-1">
								Teléfono (Opcional)
							</label>
							<input
								type="tel"
								value={phone}
								onChange={(e) => setPhone(e.target.value)}
								className="w-full bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:border-emerald-500 focus:ring-1 focus:ring-emerald-500"
							/>
						</div>
						<button
							type="submit"
							disabled={isRequestingOtp}
							className="w-full py-4 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl font-medium transition-all"
						>
							{isRequestingOtp ? "Solicitando..." : "Solicitar OTP"}
						</button>
					</form>
				)}

				{step === "OTP" && (
					<div className="space-y-4">
						<div className="text-center">
							<p className="text-slate-300 text-sm mb-4">
								Introduce el código OTP enviado (revisa la consola del backend)
							</p>
							<div className="text-2xl font-bold text-emerald-400 mb-2">
								{Math.floor(timer / 60)}:
								{(timer % 60).toString().padStart(2, "0")}
							</div>
							<p className="text-xs text-slate-500">Tiempo restante</p>
						</div>
						<input
							type="text"
							value={otp}
							maxLength={6}
							onChange={(e) => setOtp(e.target.value)}
							placeholder="000000"
							className="w-full text-center text-2xl tracking-[1em] bg-slate-900 border border-slate-700 rounded-xl px-4 py-3 text-white focus:border-emerald-500"
						/>
						<button
							onClick={handleVerifyOtp}
							disabled={isVerifyingOtp || otp.length < 6 || timer === 0}
							className="w-full py-4 bg-emerald-600 hover:bg-emerald-500 text-white rounded-xl font-medium transition-all"
						>
							{isVerifyingOtp ? "Verificando..." : "Verificar OTP"}
						</button>
						{timer === 0 && (
							<button
								onClick={() => setStep("DETAILS")}
								className="w-full text-emerald-400 text-sm hover:underline"
							>
								OTP expirado. Volver a empezar.
							</button>
						)}
					</div>
				)}

				{step === "VERIFIED" && (
					<div className="space-y-6">
						<div className="flex items-center gap-3 p-4 bg-emerald-500/10 border border-emerald-500/30 rounded-2xl">
							<div className="w-10 h-10 bg-emerald-500/20 rounded-full flex items-center justify-center">
								<svg
									className="w-6 h-6 text-emerald-400"
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
							<div>
								<p className="text-emerald-400 font-bold">
									Identidad Verificada
								</p>
								<p className="text-slate-400 text-xs">
									Puedes proceder con la inscripción.
								</p>
							</div>
						</div>
						<button
							onClick={handleSubmitRegistration}
							disabled={isSubmitting}
							className="w-full py-4 bg-linear-to-r from-blue-600 to-indigo-600 hover:from-blue-500 hover:to-indigo-500 text-white rounded-xl font-bold shadow-lg shadow-blue-500/25 transition-all transform hover:-translate-y-1"
						>
							{isSubmitting ? "Procesando..." : "Finalizar Inscripción"}
						</button>
					</div>
				)}
			</div>
		</div>
	);
};
