import React, { useState } from "react";
import DoctorsInMyHospital from "../doctor/DoctorsInMyHosptail";
import MyReceptionistsComponent from "../hospital/MyRecepcionistsComponent";
import MyHospitalComponent from "./MyHospitalComponent";

const DirectorHomePage: React.FC = () => {
  const [activeComponent, setActiveComponent] = useState<string>("myHospital");
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(false);

  const renderComponent = () => {
    switch (activeComponent) {
      case "myHospital":
        return <MyHospitalComponent />;
      case "doctors":
        return <DoctorsInMyHospital />;
      case "receptionists":
        return <MyReceptionistsComponent />;
      default:
        return <MyHospitalComponent />;
    }
  };

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  return (
    <div className="container-fluid">
      <div className="row">
        {/* Sidebar */}
        <nav
          className={`col-md-2 col-lg-1 bg-light d-flex flex-column align-items-center py-4 vh-100 sidebar ${
            isSidebarOpen ? "open" : ""
          }`}
        >
          <ul className="nav flex-column text-center w-100">
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("myHospital")}
                  className={`nav-link btn ${
                    activeComponent === "myHospital" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-hospital-user fa-2x"></i>
                  <div className="small">Моята Болница</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("doctors")}
                  className={`nav-link btn ${
                    activeComponent === "doctors" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-user-md fa-2x"></i>
                  <div className="small">Лекари</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("receptionists")}
                  className={`nav-link btn ${
                    activeComponent === "receptionists" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-concierge-bell fa-2x"></i>
                  <div className="small">Рецепционисти</div>
                </button>
              </div>
            </li>
          </ul>
        </nav>

        {/* Main content */}
        <main className="col-md-10 col-lg-11">
          {/* Toggle button for sidebar */}
          <button
            className="btn btn-primary d-md-none mb-3"
            onClick={toggleSidebar}
          >
            {isSidebarOpen ? "Hide Sidebar" : "Show Sidebar"}
          </button>
          <div className="row">
            <div className="col-12 mb-4">{renderComponent()}</div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default DirectorHomePage;
