import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faHospital,
  faEdit,
  faTrashAlt,
  faList,
  faUserMd,
} from "@fortawesome/free-solid-svg-icons";
import HospitalAddComponent from "./HospitalAddComponent";
import HospitalEditComponent from "./HospitalEditComponent";
import { Spinner } from "react-bootstrap";
import DoctorsDisplayPage from "../doctor/DoctorsDisplayPage";

interface Hospital {
  id: number;
  hospitalName: string;
}

function HospitalsList() {
  const [hospitals, setHospitals] = useState<Hospital[]>([]);
  const [allHospitals, setAllHospitals] = useState<Hospital[]>([]);
  const [error, setError] = useState(false);
  const [showAddHospital, setShowAddHospital] = useState(false);
  const [activeHospitalId, setActiveHospitalId] = useState<number | null>(null);
  const [showEdit, setShowEdit] = useState(false);
  const [showDoctors, setShowDoctors] = useState(false);
  const [loading, setLoading] = useState(true);
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
        setAllHospitals(data.hospitals);
        setHospitals(data.hospitals.slice(0, 5)); // Show only the first 5 initially
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setHospitals([]);
      setError(true);
    } finally {
      setLoading(false);
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
        if (data.success) {
          getHospitals(); // Refresh the list after removal
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

  const toggleDoctors = (hospitalId: number) => {
    setActiveHospitalId(hospitalId);
    setShowDoctors((prevState) =>
      hospitalId === activeHospitalId ? !prevState : true
    );
    setShowEdit(false); // Ensure the Edit section is hidden when showing doctors
  };

  const toggleEdit = (hospitalId: number) => {
    setActiveHospitalId(hospitalId);
    setShowEdit((prevState) =>
      hospitalId === activeHospitalId ? !prevState : true
    );
    setShowDoctors(false); // Ensure the Doctors section is hidden when showing edit form
  };

  const showAllHospitals = () => {
    setHospitals(allHospitals);
    setShowAddHospital(false); // Hide add form when showing all
  };

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <div className="card shadow-lg rounded-3 border-0">
        <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
          <h3 className="mb-0">
            <FontAwesomeIcon icon={faHospital} /> Клиники
          </h3>
        </div>
        <div className="card-body">
          {loading ? (
            <div className="text-center">
              <Spinner animation="border" role="status">
                <span className="visually-hidden">Зареждане...</span>
              </Spinner>
            </div>
          ) : hospitals.length > 0 ? (
            <ul className="list-group list-group-flush">
              {hospitals.map((hospital) => (
                <li
                  className="list-group-item border-0 shadow-sm mb-2"
                  key={hospital.id}
                >
                  <div className="d-flex justify-content-between align-items-center">
                    <span className="fw-bold">{hospital.hospitalName}</span>
                    <div>
                      <button
                        className="btn btn-outline-primary btn-sm me-2"
                        onClick={() => toggleDoctors(hospital.id)}
                      >
                        <FontAwesomeIcon icon={faUserMd} /> Доктори
                      </button>
                      <button
                        className="btn btn-outline-warning btn-sm me-2"
                        onClick={() => toggleEdit(hospital.id)}
                      >
                        <FontAwesomeIcon icon={faEdit} /> Редактирай
                      </button>
                      <button
                        className="btn btn-outline-danger btn-sm"
                        onClick={() => removeHospital(hospital.id)}
                      >
                        <FontAwesomeIcon icon={faTrashAlt} /> Изтрий
                      </button>
                    </div>
                  </div>

                  {/* Doctors section appears under hospital */}
                  {activeHospitalId === hospital.id && showDoctors && (
                    <div className="mt-3">
                      <div className="bg-light p-3 rounded">
                        <h5>Доктори в {hospital.hospitalName}</h5>
                        <DoctorsDisplayPage hospitalId={hospital.id} />
                      </div>
                    </div>
                  )}

                  {/* Edit section appears under hospital */}
                  {activeHospitalId === hospital.id && showEdit && (
                    <div className="mt-3">
                      <div className="bg-light p-3 rounded">
                        <HospitalEditComponent hospitalId={hospital.id} />
                      </div>
                    </div>
                  )}
                </li>
              ))}
            </ul>
          ) : (
            <div className="alert alert-warning" role="alert">
              No hospitals found.
            </div>
          )}
        </div>
        <div className="card-footer text-center">
          <button
            className="btn btn-outline-info"
            onClick={() => setShowAddHospital(!showAddHospital)}
            style={{ marginRight: "5px" }}
          >
            <FontAwesomeIcon icon={faList} />{" "}
            {showAddHospital ? "Скрий формата" : "Добави болница"}
          </button>
          <button className="btn btn-outline-info" onClick={showAllHospitals}>
            <FontAwesomeIcon icon={faList} /> Виж всички
          </button>
        </div>
        {showAddHospital && (
          <div className="collapse show mt-3">
            <HospitalAddComponent />
          </div>
        )}
      </div>
    </div>
  );
}

export default HospitalsList;
