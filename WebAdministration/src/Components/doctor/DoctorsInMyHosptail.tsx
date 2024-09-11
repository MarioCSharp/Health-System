import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faComments,
  faStethoscope,
  faClock,
  faTrash,
  faPlus,
  faList,
} from "@fortawesome/free-solid-svg-icons";

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
        setDoctors(data.doctors); // Display top 5 doctors
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

  const redirectToComments = (doctorId: number) => {
    navigate(`/doctor/comments/${hospitalId}`);
  };

  useEffect(() => {
    getDoctors();
    getHospitalIdByDirector();
  }, []);

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <h3 className="mb-4">Доктори</h3>
      {doctors.length > 0 ? (
        doctors.map((doctor) => (
          <div className="card mb-3" key={doctor.id}>
            <div className="card-body d-flex justify-content-between align-items-center">
              <div>
                <h5 className="mb-0">{doctor.fullName}</h5>
                <small className="text-muted">{doctor.specialization}</small>
              </div>
              <div>
                <button
                  className="btn btn-outline-info btn-sm me-2"
                  onClick={() => redirectToComments(doctor.id)}
                >
                  <FontAwesomeIcon icon={faComments} className="me-1" />
                  Оценки
                </button>
                <button
                  className="btn btn-outline-info btn-sm me-2"
                  onClick={() => redirectToServices(doctor.id)}
                >
                  <FontAwesomeIcon icon={faStethoscope} className="me-1" />
                  Услуги
                </button>
                <button
                  className="btn btn-outline-warning btn-sm me-2"
                  onClick={() => redirectToAppointments(doctor.id)}
                >
                  <FontAwesomeIcon icon={faClock} className="me-1" />
                  Часове
                </button>
                <button
                  className="btn btn-outline-danger btn-sm"
                  onClick={() => removeDoctor(doctor.id)}
                >
                  <FontAwesomeIcon icon={faTrash} className="me-1" />
                  Изтрий
                </button>
              </div>
            </div>
          </div>
        ))
      ) : (
        <div className="alert alert-warning text-center">No doctors found</div>
      )}

      <div className="d-flex justify-content-between mt-3">
        <button className="btn btn-primary" onClick={redirectToAdd}>
          <FontAwesomeIcon icon={faPlus} className="me-1" />
          Добави доктор
        </button>
      </div>
    </div>
  );
}

export default DoctorsInMyHosptail;
