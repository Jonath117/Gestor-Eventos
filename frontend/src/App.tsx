import { BrowserRouter, Route, Routes } from "react-router-dom";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { AuthProvider } from "./context/AuthContext";

function App() {
	return (
		<BrowserRouter>
			<AuthProvider>
				<Routes>
					{/* <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} /> */}

					<Route element={<ProtectedRoute />}>
						{/* <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/settings" element={<Settings />} /> */}
					</Route>
				</Routes>
			</AuthProvider>
		</BrowserRouter>
	);
}

export default App;
