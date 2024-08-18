import { useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";

interface Doctor {
  id: number;
  userId: string;
  email: string;
  fullName: string;
  specialization: string;
}

function DoctorsDisplayPage() {
  const { hospitalId } = useParams<{ hospitalId: string }>();
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
  }, []);

  if (error) {
    return <Navigate to="not-found" replace />;
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
        getDoctors();
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

  const redirectToAppointments = (id: number) => {
    navigate(`/doctor/appointments/${id}`);
  };

  const redirectToServices = (id: number) => {
    navigate(`/doctor/services/${id}`);
  };
  const redirecToAddDoctor = () => {
    navigate(`/doctor/add/${hospitalId}`);
  };

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Доктори</h3>
        {doctors.length > 0 ? (
          doctors.map((doctor) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={doctor.id}
            >
              <span>
                {doctor.fullName} | {doctor.email} |{doctor.specialization}
              </span>
              <div>
                <a
                  className="btn btn-primary btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToAppointments(doctor.id)}
                >
                  Часове
                </a>
                <a
                  className="btn btn-warning btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToServices(doctor.id)}
                >
                  Услуги
                </a>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => deleteDoctor(doctor.id)}
                >
                  Изтрий
                </a>
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
        <li className="list-group-item">
          <a href="" onClick={() => redirecToAddDoctor()}>
            Добави доктор
          </a>
        </li>
      </ul>
    </div>
  );
}

export default DoctorsDisplayPage;
