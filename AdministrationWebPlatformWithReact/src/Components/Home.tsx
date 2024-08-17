import { Navigate } from "react-router-dom";
import AdminHomePage from "./AdminHomePage";
import DoctorHomePage from "./DoctorHomePage";
import DirectorHomePage from "./DirectorHomePage";

function Home() {
  const role = localStorage.getItem("role");

  let content;

  switch (role) {
    case "Administrator":
      content = <AdminHomePage />;
      break;
    case "Doctor":
      content = <DoctorHomePage />;
      break;
    case "Director":
      content = <DirectorHomePage />;
      break;
    default:
      <Navigate to="/not-found" replace />;
      break;
  }

  return <>{content}</>;
}

export default Home;
