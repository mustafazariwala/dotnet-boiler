import React from "react";
import ReactDOM from "react-dom/client";
import { App } from "./App";
import "./styles.css";

// React starts here and renders into the <div id="root"> from index.html.
// TypeScript comparison to backend code: this is the frontend entry point, like Program.cs is for ASP.NET Core.
ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
