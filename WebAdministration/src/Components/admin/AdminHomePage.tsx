import React, { useState } from "react";
import HospitalsList from "../hospital/HospitalsList";
import UsersList from "../auth/UsersList";
import PharmaciesComponent from "../pharmacy/PharmaciesComponent";

const AdminHomePage: React.FC = () => {
  // State to manage the active component
  const [activeComponent, setActiveComponent] = useState<string>("hospitals");

  // Function to render the active component based on state
  const renderComponent = () => {
    switch (activeComponent) {
      case "hospitals":
        return <HospitalsList />;
      case "users":
        return <UsersList />;
      case "pharmacies":
        return <PharmaciesComponent />;
      default:
        return <HospitalsList />;
    }
  };

  return (
    <div className="container-fluid">
      <div className="row">
        {/* Sidebar */}
        <nav className="col-md-2 col-lg-1 bg-light d-flex flex-column align-items-center py-4 vh-100">
          <ul className="nav flex-column text-center w-100">
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("hospitals")}
                  className={`nav-link btn ${
                    activeComponent === "hospitals" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-hospital fa-2x"></i>
                  <div className="small">Болници</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("users")}
                  className={`nav-link btn ${
                    activeComponent === "users" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-users fa-2x"></i>
                  <div className="small">Потребители</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("pharmacies")}
                  className={`nav-link btn ${
                    activeComponent === "pharmacies" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-pills fa-2x"></i>
                  <div className="small">Аптеки</div>
                </button>
              </div>
            </li>
          </ul>
        </nav>

        <main className="col-md-10 col-lg-11">
          <div className="row">
            <div className="col-12 mb-4">{renderComponent()}</div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default AdminHomePage;
