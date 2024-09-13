import React, { useState } from "react";
import RecipeByEgnComponent from "./RecipeByEgnComponent";
import OrdersInMyPharmacyComponent from "./OrdersInMyPharmacyComponent";
import MedicationsInPharmacyComponent from "./MedicationsInPharmacyComponent";

const PharmacistHomePage: React.FC = () => {
  const [activeComponent, setActiveComponent] = useState<string>("orders");
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(false);

  const renderComponent = () => {
    switch (activeComponent) {
      case "orders":
        return <OrdersInMyPharmacyComponent />;
      case "recipes":
        return <RecipeByEgnComponent />;
      case "medications":
        return <MedicationsInPharmacyComponent />;
      default:
        return <OrdersInMyPharmacyComponent />;
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
                  onClick={() => setActiveComponent("orders")}
                  className={`nav-link btn ${
                    activeComponent === "orders" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-receipt fa-2x"></i>
                  <div className="small">Поръчки</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("recipes")}
                  className={`nav-link btn ${
                    activeComponent === "recipes" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-prescription-bottle-alt fa-2x"></i>
                  <div className="small">Рецепти</div>
                </button>
              </div>
            </li>
            <li className="nav-item mb-4">
              <div className="d-flex justify-content-center">
                <button
                  onClick={() => setActiveComponent("medications")}
                  className={`nav-link btn ${
                    activeComponent === "medications" ? "active" : ""
                  } text-dark`}
                >
                  <i className="fas fa-pills fa-2x"></i>
                  <div className="small">Наличност</div>
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

export default PharmacistHomePage;
