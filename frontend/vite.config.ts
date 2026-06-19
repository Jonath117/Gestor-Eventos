import tailwindcss from "@tailwindcss/vite";
import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

export default defineConfig({
	plugins: [react(), tailwindcss()],
	server: {
		proxy: {
			"/api": {
				target: process.env.VITE_API_URL
					? process.env.VITE_API_URL.replace(/\/api$/, "")
					: "http://localhost:5206",
				changeOrigin: true,
			},
		},
	},
});
