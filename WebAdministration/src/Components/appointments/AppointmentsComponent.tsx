import { useEffect, useState } from "react";
import { Navigate, useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt, faCalendarAlt } from "@fortawesome/free-solid-svg-icons";

interface Appointment {
  id: number;
  date: string;
  name: string;
  serviceName: string;
}

function AppointmentsComponent() {
  const { id } = useParams<{ id: string }>();
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [userFullName, setUserFullName] = useState<string>("");
  const [error, setError] = useState(false);
  const token = localStorage.getItem("token");

  const getBookings = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Appointment/GetUserAppointments?userId=${id}`,
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
        setUserFullName(data.fullName || "");
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

  return (
    <div className="container mt-4">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <div className="card shadow-sm">
            <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
              <h4>
                <FontAwesomeIcon icon={faCalendarAlt} className="me-2" />
                Часове на {userFullName}
              </h4>
              <span className="badge bg-light text-primary">
                {appointments.length} часове
              </span>
            </div>
            <div className="card-body p-4">
              {appointments && appointments.length > 0 ? (
                <ul className="list-group">
                  {appointments.map((appointment) => (
                    <li
                      className="list-group-item d-flex justify-content-between align-items-center"
                      key={appointment.id}
                    >
                      <span>
                        {appointment.date} | {appointment.serviceName} |{" "}
                        {appointment.name}
                      </span>
                      <button className="btn btn-sm btn-outline-danger">
                        <FontAwesomeIcon icon={faTrashAlt} /> Изтрий
                      </button>
                    </li>
                  ))}
                </ul>
              ) : (
                <div className="alert alert-info text-center">
                  Няма намерени часове
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AppointmentsComponent;
