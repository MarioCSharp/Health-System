import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faTrashAlt,
  faClipboardList,
  faCog,
} from "@fortawesome/free-solid-svg-icons";

interface Doctor {
  id: number;
  userId: string;
  email: string;
  fullName: string;
  specialization: string;
}

interface DoctorsDisplayPageProps {
  hospitalId: number;
}

const DoctorsDisplayPage: React.FC<DoctorsDisplayPageProps> = ({
  hospitalId,
}) => {
  const [doctors, setDoctors] = useState<Doctor[]>([]);
  const [error, setError] = useState(false);
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const getDoctors = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Hospital/GetDoctors?id=${hospitalId}`,
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
        setDoctors(data.doctors);
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setDoctors([]);
      setError(true);
    }
  };

  useEffect(() => {
    getDoctors();
  }, [hospitalId]);

  if (error) {
    return <div>There was an error loading the doctors.</div>;
  }

  const deleteDoctor = async (id: number) => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/Remove?id=${id}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        getDoctors(); // Refetch doctors after deletion
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setDoctors([]);
      setError(true);
    }
  };

  return (
    <div className="col-md-8 mx-md-3 mb-4">
      <ul className="list-group">
        {doctors.length > 0 ? (
          doctors.map((doctor) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center border-0 shadow-sm mb-2"
              key={doctor.id}
            >
              <div className="d-flex flex-column flex-grow-1">
                <span className="fw-bold">{doctor.fullName}</span>
                <span>{doctor.email}</span>
                <span className="text-muted">{doctor.specialization}</span>
              </div>
              <div className="d-flex flex-wrap gap-2">
                <button
                  className="btn btn-primary btn-sm"
                  onClick={() => navigate(`/doctor/appointments/${doctor.id}`)}
                >
                  <FontAwesomeIcon icon={faClipboardList} /> Часове
                </button>
                <button
                  className="btn btn-warning btn-sm"
                  onClick={() => navigate(`/doctor/services/${doctor.id}`)}
                >
                  <FontAwesomeIcon icon={faCog} /> Услуги
                </button>
                <button
                  className="btn btn-danger btn-sm"
                  onClick={() => deleteDoctor(doctor.id)}
                >
                  <FontAwesomeIcon icon={faTrashAlt} /> Изтрий
                </button>
              </div>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No doctors found</div>
            </div>
          </div>
        )}
      </ul>
    </div>
  );
};

export default DoctorsDisplayPage;
