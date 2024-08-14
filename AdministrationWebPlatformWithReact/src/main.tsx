import React from "react";
import ReactDOM from "react-dom/client";
import "bootstrap/dist/css/bootstrap.css";
import Navigation from "./Components/Navigation";
import App from "./Components/App";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <Navigation />
    <App />
  </React.StrictMode>
);
