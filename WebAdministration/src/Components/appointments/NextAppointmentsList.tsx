import { useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";

interface Appointment {
  id: number;
  serviceName: string;
  patientName: string;
  date: string;
}

function NextAppointmentsList() {
  const token = localStorage.getItem("token");
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [error, setError] = useState<boolean>(false);

  const navigate = useNavigate();

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
        setAppointments(data.appointments.slice(0, 5));
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

  const redirectToAllAppointments = () => {
    navigate("/next-appointments");
  };

  if (error) {
    return <Navigate to={"not-found"} />;
  }

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <div className="card shadow-sm">
        <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
          <h5 className="mb-0">
            <i className="fas fa-calendar-alt me-2"></i> Предстоящи часове
          </h5>
          <button
            className="btn btn-light btn-sm"
            onClick={redirectToAllAppointments}
          >
            Виж всички
          </button>
        </div>
        <ul className="list-group list-group-flush">
          {appointments?.length ? (
            appointments.map((appointment) => (
              <li
                className="list-group-item d-flex justify-content-between align-items-center"
                key={appointment.id}
              >
                <div className="d-flex align-items-center">
                  <i className="fas fa-user-md text-primary me-2"></i>
                  <span>{appointment.patientName}</span>
                </div>
                <div>
                  <i className="fas fa-clock text-secondary me-2"></i>
                  {appointment.date}
                </div>
              </li>
            ))
          ) : (
            <div className="text-center p-3">
              <p className="text-muted">No appointments found</p>
            </div>
          )}
        </ul>
      </div>
    </div>
  );
}

export default NextAppointmentsList;
