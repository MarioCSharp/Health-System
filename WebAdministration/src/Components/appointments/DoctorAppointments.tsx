import React, { useEffect, useState } from "react";
import { Navigate, useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap

interface Appointment {
  id: number;
  date: string;
  name: string;
  serviceName: string;
}

function DoctorAppointments() {
  const { id } = useParams<{ id: string }>();
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [doctorFullName, setDoctorFullName] = useState<string>("");
  const [error, setError] = useState(false);

  const token = localStorage.getItem("token");

  const getBookings = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Appointment/GetAppointments?id=${id}`,
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

        setAppointments(data.appointments || []);
        setDoctorFullName(data.fullName || "");
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setAppointments([]);
      setError(true);
    }
  };

  useEffect(() => {
    getBookings();
  }, []);

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  const deleteAppointment = async (id: number) => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Appointment/RemoveAppointment?id=${id}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        getBookings();
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setAppointments([]);
      setError(true);
    }
  };

  return (
    <div className="container mt-4">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <h3 className="mb-4 text-center">
            Записани часове на {doctorFullName}
          </h3>
          <ul className="list-group">
            {appointments && appointments.length > 0 ? (
              appointments.map((appointment) => (
                <li
                  className="list-group-item d-flex justify-content-between align-items-center"
                  key={appointment.id}
                >
                  <div>
                    <strong>{appointment.date}</strong>
                    <span className="mx-2">|</span>
                    <span>{appointment.serviceName}</span>
                    <span className="mx-2">|</span>
                    <span>{appointment.name}</span>
                  </div>
                  <div>
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => deleteAppointment(appointment.id)}
                    >
                      <FontAwesomeIcon icon={faTrash} className="mr-1" />
                      Изтрий
                    </button>
                  </div>
                </li>
              ))
            ) : (
              <div className="col-12">
                <div className="card mb-3">
                  <div className="card-body p-2 text-center">
                    <p>No appointments found</p>
                  </div>
                </div>
              </div>
            )}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default DoctorAppointments;
