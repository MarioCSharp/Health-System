import React, { useEffect, useState } from "react";
import { Navigate, useParams } from "react-router-dom";

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
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Записани часове на {doctorFullName}</h3>
        {appointments && appointments.length > 0 ? (
          appointments.map((appointment) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={appointment.id}
            >
              <span>
                {appointment.date} | {appointment.serviceName} |{" "}
                {appointment.name}
              </span>
              <div>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => deleteAppointment(appointment.id)}
                >
                  Изтрий
                </a>
              </div>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No appointments found</div>
            </div>
          </div>
        )}
      </ul>
    </div>
  );
}

export default DoctorAppointments;
