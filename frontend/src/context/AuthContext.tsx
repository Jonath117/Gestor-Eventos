import { createContext, type ReactNode, useContext } from "react";
import { useLocalStorage } from "../hooks/useLocalStorage";

export interface User {
	id: string;
	name: string;
	email: string;
}

interface AuthContextType {
	user: User | null;
	login: (userData: User) => void;
	logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
	const [user, setUser] = useLocalStorage<User | null>("user", null);

	const login = (userData: User) => {
		setUser(userData);
	};

	const logout = () => {
		setUser(null);

		localStorage.removeItem("user");
	};

	return (
		<AuthContext.Provider value={{ user, login, logout }}>
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
