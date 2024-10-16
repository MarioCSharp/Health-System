import { useState, useEffect } from "react";
import { Navigate, useNavigate } from "react-router-dom";

interface Hospital {
  id: number;
  hospitalName: string;
}

function AllHospitalsComponents() {
  const [hospitals, setHospitals] = useState<Hospital[]>([]);
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  const token = localStorage.getItem("token");

  const getHospitals = async () => {
    try {
      const response = await fetch(`http://localhost:5025/api/Hospital/All`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setHospitals(data.hospitals.slice(0, 5));
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

  const removeHospital = async (id: number) => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Hospital/Remove?id=${id}`,
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

        const result = data.success;

        if (result) {
          getHospitals();
        } else {
          throw new Error(
            "You are either not authorized or there is a problem in the system!"
          );
        }
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

  const redirectToDoctors = (hospitalId: number) => {
    navigate(`/doctors/${hospitalId}`);
  };

  const redirectToEdit = (hospitalId: number) => {
    navigate(`/hospital/edit/${hospitalId}`);
  };

  const redirectToAdd = () => {
    navigate(`/hospital/add`);
  };

  const redirectToAll = () => {
    navigate(`/hospitals`);
  };

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Клиники</h3>
        {hospitals.length > 0 ? (
          hospitals.map((hospital) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={hospital.id}
            >
              <span>{hospital.hospitalName}</span>
              <div>
                <a
                  className="btn btn-primary btn-sm"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToDoctors(hospital.id)}
                >
                  Доктори
                </a>
                <a
                  className="btn btn-warning btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToEdit(hospital.id)}
                >
                  Редактирай
                </a>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => removeHospital(hospital.id)}
                >
                  Изтрий
                </a>
              </div>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No hospitals found</div>
            </div>
          </div>
        )}
        <li className="list-group-item">
          <a href="" onClick={() => redirectToAdd()}>
            Добави болница
          </a>
        </li>
      </ul>
    </div>
  );
}

export default AllHospitalsComponents;
