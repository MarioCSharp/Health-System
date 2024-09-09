import { Navigate } from "react-router-dom";
import AdminHomePage from "../admin/AdminHomePage";
import DoctorHomePage from "../doctor/DoctorHomePage";
import DirectorHomePage from "../director/DirectorHomePage";
import RecepcionistHomePage from "../hospital/RecepcionistHomePage";
import PharmacyOwnerHomePage from "../pharmacy/PharmacyOwnerHomePage";
import PharmacistHomePage from "../pharmacy/PharmacistHomePage";

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
    case "Recepcionist":
      content = <RecepcionistHomePage />;
      break;
    case "PharmacyOwner":
      content = <PharmacyOwnerHomePage />;
      break;
    case "Pharmacist":
      content = <PharmacistHomePage />;
      break;
    default:
      <Navigate to="/not-found" replace />;
      break;
  }

  return <>{content}</>;
}

export default Home;
