import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";

interface Appointment {
  id: number;
  serviceName: string;
  patientName: string;
  date: string;
}

function AllNextAppointmentsList() {
  const token = localStorage.getItem("token");
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [error, setError] = useState<boolean>(false);

  const getAppointments = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Appointment/GetNextAppointmentsByDoctorUserId`,
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
        setAppointments(data.appointments);
      } else {
        throw new Error("There was an error loading your appointments");
      }
    } catch (error) {
      console.log("Error!", error);
      setAppointments([]);
      setError(true);
    }
  };

  useEffect(() => {
    getAppointments();
  }, []);

  if (error) {
    return <Navigate to={"not-found"} />;
  }

  return (
    <div className="col-md-8 mx-auto mb-4">
      <div className="card shadow-sm">
        <div className="card-header bg-primary text-white">
          <h5 className="mb-0">
            <i className="fas fa-calendar-alt me-2"></i> Всички предстоящи
            часове
          </h5>
        </div>
        <ul className="list-group list-group-flush">
          {appointments.length > 0 ? (
            appointments.map((appointment) => (
              <li
                className="list-group-item d-flex justify-content-between align-items-center"
                key={appointment.id}
              >
                <div className="d-flex align-items-center">
                  <i className="fas fa-user-md text-primary me-2"></i>
                  <span>{appointment.patientName}</span>
                </div>
                <div className="d-flex align-items-center">
                  <i className="fas fa-stethoscope text-info me-2"></i>
                  <span>{appointment.serviceName}</span>
                </div>
                <div className="d-flex align-items-center">
                  <i className="fas fa-clock text-secondary me-2"></i>
                  <span>{appointment.date}</span>
                </div>
              </li>
            ))
          ) : (
            <li className="list-group-item text-center p-3">
              <p className="text-muted">No appointments found</p>
            </li>
          )}
        </ul>
      </div>
    </div>
  );
}

export default AllNextAppointmentsList;
