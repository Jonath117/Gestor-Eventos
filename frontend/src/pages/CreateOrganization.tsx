import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createOrganization } from "../features/organizations/api/organizations";

export const CreateOrganization = () => {
	const navigate = useNavigate();
	const [isLoading, setIsLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [formData, setFormData] = useState({
		name: "",
		qrPaymentImageUrl: "",
	});

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		setIsLoading(true);
		setError(null);

		try {
			const result = await createOrganization(formData);
			localStorage.setItem("tenantId", result.id);
			navigate("/dashboard");
		} catch (err) {
			setError("Error al crear la organización. Intenta de nuevo.");
			console.error(err);
		} finally {
			setIsLoading(false);
		}
	};

	return (
		<div className="min-h-screen bg-slate-950 text-slate-200 p-6">
			<div className="max-w-xl mx-auto bg-slate-900 border border-slate-800 rounded-2xl p-8">
				<h1 className="text-2xl font-bold text-white mb-6">
					Crear Organización
				</h1>

				{error && (
					<div className="bg-red-500/10 border border-red-500/20 text-red-400 p-4 rounded-xl mb-6">
						{error}
					</div>
				)}

				<form onSubmit={handleSubmit} className="space-y-6">
					<div>
						<label className="block text-sm font-medium text-slate-400 mb-2">
							Nombre de la Organización
						</label>
						<input
							type="text"
							required
							value={formData.name}
							onChange={(e) =>
								setFormData({ ...formData, name: e.target.value })
							}
							placeholder="Ej. Mi Empresa de Eventos"
							className="w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-white focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all"
						/>
					</div>

					<div>
						<label className="block text-sm font-medium text-slate-400 mb-2">
							URL de Imagen QR de Pago (Opcional)
						</label>
						<input
							type="url"
							value={formData.qrPaymentImageUrl}
							onChange={(e) =>
								setFormData({ ...formData, qrPaymentImageUrl: e.target.value })
							}
							placeholder="https://ejemplo.com/qr.png"
							className="w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-white focus:outline-none focus:ring-2 focus:ring-blue-500 transition-all"
						/>
					</div>

					<div className="flex gap-4 pt-4">
						<button
							type="submit"
							disabled={isLoading}
							className="flex-1 bg-blue-600 hover:bg-blue-500 disabled:opacity-50 text-white font-semibold py-3 rounded-xl transition-all shadow-lg shadow-blue-500/20"
						>
							{isLoading ? "Creando..." : "Crear Organización"}
						</button>
						<button
							type="button"
							onClick={() => navigate("/dashboard")}
							className="flex-1 bg-slate-800 hover:bg-slate-700 text-white font-semibold py-3 rounded-xl transition-all"
						>
							Cancelar
						</button>
					</div>
				</form>
			</div>
		</div>
	);
};
