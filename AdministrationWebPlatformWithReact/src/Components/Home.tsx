import { Navigate } from "react-router-dom";
import AdminHomePage from "./AdminHomePage";

function Home() {
  const role = localStorage.getItem("role");

  let content;

  switch (role) {
    case "Administrator":
      content = <AdminHomePage />;
      break;
    case "Doctor":
      content = <h1>Welcome Doctor!</h1>;
      break;
    default:
      <Navigate to="/not-found" replace />;
      break;
  }

  return <>{content}</>;
}

export default Home;
