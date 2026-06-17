import type { ChangeEvent } from "react";
import { type FormEvent, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useCreateEvent } from "../features/events/hooks/useCreateEvent";
import type { CreateEventData } from "../features/events/types/event";
import { getOrganizations } from "../features/organizations/api/organizations";
import type { Organization } from "../features/organizations/types/organization";

export const CreateEvent = () => {
	const navigate = useNavigate();
	const { mutate, isPending } = useCreateEvent();
	const [organizations, setOrganizations] = useState<Organization[]>([]);
	const [isLoadingOrgs, setIsLoadingOrgs] = useState(true);

	const [formData, setFormData] = useState<CreateEventData>({
		name: "",
		startDate: "",
		endDate: "",
		maxCapacity: 0,
		organizationId: "",
	});

	useEffect(() => {
		const fetchOrgs = async () => {
			try {
				const data = await getOrganizations();
				setOrganizations(data);

				const storedTenantId = localStorage.getItem("tenantId");
				let activeId = "";
				if (storedTenantId) {
					try {
						activeId = storedTenantId.startsWith('"')
							? JSON.parse(storedTenantId)
							: storedTenantId;
					} catch {
						activeId = storedTenantId;
					}
				}

				if (activeId && data.some((org) => org.id === activeId)) {
					setFormData((prev) => ({ ...prev, organizationId: activeId }));
				} else if (data.length > 0) {
					setFormData((prev) => ({ ...prev, organizationId: data[0].id }));
				}
			} catch (error) {
				console.error("Error fetching organizations:", error);
			} finally {
				setIsLoadingOrgs(false);
			}
		};
		fetchOrgs();
	}, []);

	const handleChange = (
		e: ChangeEvent<HTMLInputElement | HTMLSelectElement>,
	) => {
		const { name, value } = e.target;
		setFormData((prev) => ({
			...prev,
			[name]: name === "maxCapacity" ? Number(value) : value,
		}));
	};

	const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
		e.preventDefault();

		if (!formData.organizationId) {
			alert("Por favor selecciona una organización.");
			return;
		}

		mutate(formData, {
			onSuccess: () => {
				navigate("/dashboard");
			},
		});
	};

	return (
		<div className="min-h-screen bg-slate-950 text-slate-200 p-6">
			<div className="max-w-xl mx-auto bg-slate-900 border border-slate-800 rounded-2xl p-8">
				<h1 className="text-2xl font-bold text-white mb-4">
					Crear Nuevo Evento
				</h1>
				<form onSubmit={handleSubmit} className="space-y-4">
					<div>
						<label className="block text-sm text-slate-300 mb-1">
							Organización
						</label>
						{isLoadingOrgs ? (
							<div className="h-10 bg-slate-800 rounded-lg animate-pulse" />
						) : (
							<select
								name="organizationId"
								value={formData.organizationId}
								onChange={handleChange}
								required
								className="w-full rounded-lg border border-slate-700 bg-slate-800 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
							>
								<option value="" disabled>
									Selecciona una organización
								</option>
								{organizations.map((org) => (
									<option key={org.id} value={org.id}>
										{org.name}
									</option>
								))}
							</select>
						)}
						{organizations.length === 0 && !isLoadingOrgs && (
							<p className="text-xs text-amber-400 mt-1">
								No tienes organizaciones. Crea una primero.
							</p>
						)}
					</div>

					<div>
						<label className="block text-sm text-slate-300 mb-1">Nombre</label>
						<input
							name="name"
							value={formData.name}
							onChange={handleChange}
							required
							className="w-full rounded-lg border border-slate-700 bg-slate-800 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
						/>
					</div>
					<div className="grid grid-cols-1 md:grid-cols-2 gap-4">
						<div>
							<label className="block text-sm text-slate-300 mb-1">
								Fecha de Inicio
							</label>
							<input
								name="startDate"
								type="datetime-local"
								value={formData.startDate}
								onChange={handleChange}
								required
								className="w-full rounded-lg border border-slate-700 bg-slate-800 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
							/>
						</div>
						<div>
							<label className="block text-sm text-slate-300 mb-1">
								Fecha de Fin
							</label>
							<input
								name="endDate"
								type="datetime-local"
								value={formData.endDate}
								onChange={handleChange}
								required
								className="w-full rounded-lg border border-slate-700 bg-slate-800 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
							/>
						</div>
					</div>
					<div>
						<label className="block text-sm text-slate-300 mb-1">
							Capacidad máxima
						</label>
						<input
							name="maxCapacity"
							type="number"
							min={1}
							value={formData.maxCapacity}
							onChange={handleChange}
							required
							className="w-full rounded-lg border border-slate-700 bg-slate-800 px-3 py-2 text-white focus:outline-none focus:ring-2 focus:ring-blue-500"
						/>
					</div>
					<div className="flex gap-2 pt-4">
						<button
							className="flex-1 px-4 py-2 bg-blue-600 hover:bg-blue-500 rounded-lg text-white font-semibold transition"
							type="submit"
							disabled={isPending || organizations.length === 0}
						>
							{isPending ? "Creando..." : "Crear Evento"}
						</button>
						<button
							className="flex-1 px-4 py-2 bg-slate-700 hover:bg-slate-600 rounded-lg text-white font-semibold transition"
							type="button"
							onClick={() => navigate("/dashboard")}
						>
							Cancelar
						</button>
					</div>
				</form>
			</div>
		</div>
	);
};
