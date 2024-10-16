import React from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import "bootstrap/dist/css/bootstrap.min.css";

const BackButton: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();

  // Hide the back button on the home ("/") and login pages ("/login")
  if (location.pathname === "/" || location.pathname === "/login") {
    return null;
  }

  const goBack = () => {
    if (window.history.length > 1) {
      navigate(-1);
    } else {
      navigate("/"); // Fallback to the home page
    }
  };

  return (
    <div
      onClick={goBack}
      style={{
        position: "absolute",
        top: "70px",
        left: "30px",
        cursor: "pointer",
        color: "#007bff",
        display: "flex",
        alignItems: "center",
        gap: "8px",
        fontSize: "16px",
        zIndex: 1000, // Ensures it stays on top of other elements
      }}
      className="back-button"
    >
      <FontAwesomeIcon icon={faArrowLeft} />
      <span>Назад</span>
    </div>
  );
};

export default BackButton;
