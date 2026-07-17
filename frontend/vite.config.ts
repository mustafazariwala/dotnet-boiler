import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // During development, React calls /api/todos and Vite forwards it to ASP.NET Core.
      "/api": "http://localhost:5189"
    }
  },
  build: {
    // The production React build is copied into the .NET API so one server can host both.
    outDir: "../src/DotnetBoiler.Api/wwwroot",
    emptyOutDir: true
  }
});
