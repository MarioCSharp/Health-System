import React, { useState } from "react";
import NextAppointmentsList from "../appointments/NextAppointmentsList";
import PastAppointmentsList from "../appointments/PastAppointmentsList";
import MyServicesComponent from "./MyServicesComponent";
import LaboratoryResultsComponent from "../laboratory/LaboratoryResultsComponent";
import MyCalendarComponent from "./MyCalendarComponent";

const DoctorHomePage: React.FC = () => {
  const [activeComponent, setActiveComponent] =
    useState<string>("nextAppointments");
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(false);

  const renderComponent = () => {
    switch (activeComponent) {
      case "nextAppointments":
        return <NextAppointmentsList />;
      case "pastAppointments":
        return <PastAppointmentsList />;
      case "services":
        return <MyServicesComponent />;
      case "calendar":
        return <MyCalendarComponent />;
      case "labResults":
        return <LaboratoryResultsComponent />;
      default:
        return <NextAppointmentsList />;
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
                  onClick={() => setActiveComponent("nextAppointments")}
                  className={`nav-link btn ${
                    activeComponent === "nextAppointments" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-calendar-check fa-2x"></i>
                  <div className="small">Предстоящи Прегледи</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("pastAppointments")}
                  className={`nav-link btn ${
                    activeComponent === "pastAppointments" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-calendar-times fa-2x"></i>
                  <div className="small">Прегледи</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("services")}
                  className={`nav-link btn ${
                    activeComponent === "services" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-stethoscope fa-2x"></i>
                  <div className="small">Моите Услуги</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("calendar")}
                  className={`nav-link btn ${
                    activeComponent === "calendar" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-calendar-alt fa-2x"></i>
                  <div className="small">Моят Календар</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("labResults")}
                  className={`nav-link btn ${
                    activeComponent === "labResults" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-vial fa-2x"></i>
                  <div className="small">Лабораторни Резултати</div>
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

export default DoctorHomePage;
