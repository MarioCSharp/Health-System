import { useState, useEffect } from "react";
import { Navigate } from "react-router-dom";

interface Hospital {
  id: number;
  hospitalName: string;
}

function AdminHomePage() {
  const [hospitals, setHospitals] = useState<Hospital[]>([]);
  const [error, setError] = useState(false);

  const getHospitals = async () => {
    try {
      const response = await fetch(`http://localhost:5166/api/Hospital/All`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.ok) {
        const data = await response.json();
        setHospitals(data.hospitals);
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setHospitals([]);
      setError(true);
    }
  };

  useEffect(() => {
    getHospitals();
  }, []);

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  return (
    <>
      <ul>
        {hospitals.length > 0 ? (
          hospitals.map((hospital) => (
            <li key={hospital.id}>{hospital.hospitalName}</li>
          ))
        ) : (
          <li>No hospitals found</li>
        )}
      </ul>
    </>
  );
}

export default AdminHomePage;
