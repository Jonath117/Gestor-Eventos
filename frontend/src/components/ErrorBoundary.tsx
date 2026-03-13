import { Component, type ReactNode } from "react";

interface Props {
	children: ReactNode;
}

interface State {
	hasError: boolean;
	error: Error | null;
}

export class ErrorBoundary extends Component<Props, State> {
	public state: State = {
		hasError: false,
		error: null,
	};

	public static getDerivedStateFromError(error: Error): State {
		// Actualiza el estado para que el siguiente renderizado muestre la interfaz de repuesto
		return { hasError: true, error };
	}

	public resetError = () => {
		this.setState({ hasError: false, error: null });
	};

	public render() {
		if (this.state.hasError) {
			return (
				<div className="min-h-screen bg-slate-950 flex items-center justify-center px-4 relative overflow-hidden">
					<div className="absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 w-200 h-200 bg-red-900/10 rounded-full blur-[120px] pointer-events-none"></div>

					<div className="w-full max-w-lg bg-slate-900 border border-slate-800 rounded-3xl p-8 relative z-10">
						<div className="w-16 h-16 bg-red-500/10 text-red-500 rounded-2xl flex items-center justify-center mb-6">
							<svg
								className="w-8 h-8"
								fill="none"
								viewBox="0 0 24 24"
								stroke="currentColor"
							>
								<path
									strokeLinecap="round"
									strokeLinejoin="round"
									strokeWidth={2}
									d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
								/>
							</svg>
						</div>

						<h1 className="text-2xl font-bold text-white mb-2">
							¡Ups! Algo salió mal
						</h1>
						<p className="text-slate-400 mb-6">
							Se ha producido un error inesperado en la aplicación. Hemos
							registrado el problema.
						</p>

						{this.state.error && (
							<div className="bg-slate-950 border border-slate-800 rounded-xl p-4 mb-8 overflow-auto max-h-40">
								<code className="text-sm text-red-400 font-mono">
									{this.state.error.message}
								</code>
							</div>
						)}

						<div className="flex gap-4">
							<button
								onClick={() => window.location.reload()}
								className="flex-1 py-3 px-4 bg-blue-600 hover:bg-blue-500 text-white rounded-xl font-medium transition-all"
							>
								Recargar la página
							</button>
							<button
								onClick={this.resetError}
								className="flex-1 py-3 px-4 bg-slate-800 hover:bg-slate-700 text-white rounded-xl font-medium transition-all"
							>
								Intentar de nuevo
							</button>
						</div>
					</div>
				</div>
			);
		}

		return this.props.children;
	}
}
