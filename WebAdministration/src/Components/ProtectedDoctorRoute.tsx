import React, { useEffect, useState } from "react";
import { Navigate, Outlet } from "react-router-dom";

const ProtectedDoctorRoute: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
  const [isDoctor, setIsDoctor] = useState<boolean | null>(null);

  const checkAuthentication = async () => {
    try {
      const token = localStorage.getItem("token");

      if (!token) {
        setIsAuthenticated(false);
        return;
      }
      console.log(localStorage.getItem("role"));
      const response = await fetch(
        `http://localhost:5196/api/Authentication/SecureIsAuthenticated`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();

        setIsAuthenticated(data.isAuthenticated);
      } else {
        throw new Error("Login failed");
      }
    } catch (error) {
      console.error("Failed to login", error);
      setIsAuthenticated(false);
    }
  };

  useEffect(() => {
    checkAuthentication();
  }, []);

  useEffect(() => {
    setIsDoctor(localStorage.getItem("roles") == "Doctor");
  }, [isAuthenticated]);

  if (isDoctor === null) {
    return <div>Loading...</div>;
  }

  if (!isDoctor) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};

export default ProtectedDoctorRoute;
