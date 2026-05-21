import { createContext, type ReactNode, useContext } from "react";
import { useLocalStorage } from "../../../hooks/useLocalStorage";

export interface User {
	id: string;
	name: string;
	email: string;
}

interface AuthContextType {
	token: string | null;
	refreshToken: string | null;
	login: (token: string, refreshToken: string) => void;
	logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
	const [token, setToken] = useLocalStorage<string | null>("token", null);
	const [refreshToken, setRefreshToken] = useLocalStorage<string | null>(
		"refreshToken",
		null,
	);

	const login = (newToken: string, newRefreshToken: string) => {
		setToken(newToken);
		setRefreshToken(newRefreshToken);
	};

	const logout = () => {
		setToken(null);
		setRefreshToken(null);
		localStorage.removeItem("token");
		localStorage.removeItem("refreshToken");
	};

	return (
		<AuthContext.Provider value={{ token, refreshToken, login, logout }}>
			{children}
		</AuthContext.Provider>
	);
};

export const useAuth = (): AuthContextType => {
	const context = useContext(AuthContext);
	if (context === undefined) {
		throw new Error("useAuth debe usarse dentro de un AuthProvider");
	}
	return context;
};
