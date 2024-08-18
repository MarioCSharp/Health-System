import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface Doctor {
  id: number;
  fullName: string;
  specialization: string;
}

function DoctorsInMyHosptail() {
  const [doctors, setDoctors] = useState<Doctor[]>([]);
  const [error, setError] = useState<boolean>(false);
  const [hospitalId, setHospitalId] = useState<number | null>();

  const navigate = useNavigate();

  const token = localStorage.getItem("token");

  const getDoctors = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/AllByDirector`,
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

        setDoctors(data.doctors.slice(0, 5));
      } else {
        throw new Error("There was an error loading the doctors");
      }
    } catch (error) {
      console.log("Error!", error);
      setDoctors([]);
      setError(true);
    }
  };

  const getHospitalIdByDirector = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/HospitalIdByDirector`,
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

        setHospitalId(data.hospitalId);
      } else {
        throw new Error("There was an error getting the hospital id.");
      }
    } catch (error) {
      console.log("Error!", error);
      setDoctors([]);
      setError(true);
    }
  };

  const removeDoctor = async (doctorId: number) => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/Remove?id=${doctorId}`,
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
        throw new Error("There was an error removing this doctor!");
      }
    } catch (error) {
      console.log("Error!", error);
      setError(true);
    }
  };

  const redirectToAdd = async () => {
    navigate(`/doctor/add/${hospitalId}`);
  };

  const redirectToAppointments = (doctorId: number) => {
    navigate(`/doctor/appointments/${doctorId}`);
  };

  const redirectToServices = (doctorId: number) => {
    navigate(`/doctor/services/${doctorId}`);
  };

  const redirectToAll = () => {
    navigate(`/doctors/${hospitalId}`);
  };

  useEffect(() => {
    getDoctors();
    getHospitalIdByDirector();
  }, []);

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Доктори</h3>
        {doctors.length > 0 ? (
          doctors.map((doctor) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={doctor.id}
            >
              <span>
                {doctor.fullName} | {doctor.specialization}
              </span>
              <div>
                <a
                  className="btn btn-primary btn-sm"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToServices(doctor.id)}
                >
                  Услуги
                </a>
                <a
                  className="btn btn-warning btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToAppointments(doctor.id)}
                >
                  Часове
                </a>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => removeDoctor(doctor.id)}
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
          <a href="" onClick={() => redirectToAdd()}>
            Добави доктор
          </a>
          <a href="" onClick={() => redirectToAll()}>
            Виж всички
          </a>
        </li>
      </ul>
    </div>
  );
}

export default DoctorsInMyHosptail;
