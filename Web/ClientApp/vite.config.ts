import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import fs from 'fs';

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
    strictPort: true,
    proxy: {
      '/api': { target: 'https://localhost:7000', secure: false },
    }
  }
}));
