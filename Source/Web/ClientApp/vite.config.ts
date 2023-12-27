import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import fs from "fs";

// https://vitejs.dev/config/
export default defineConfig((env) => ({
	plugins: [react()],
	server: {
		host: "localhost",
		port: 3000,
		https: {
			key: env.command === "serve" ? fs.readFileSync("./localhost.key") : "",
			cert: env.command === "serve" ? fs.readFileSync("./localhost.pem") : "",
		},
		proxy: {
			"/api": { target: "https://localhost:7002", changeOrigin: true, secure: false },
			"/hangfire": { target: "https://localhost:7002", changeOrigin: false, secure: false },
		},
		strictPort: true,
	},
}));
