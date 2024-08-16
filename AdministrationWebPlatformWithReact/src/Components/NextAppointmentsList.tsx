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
        `http://localhost:5166/api/Appointment/GetNextAppointmentsByDoctorUserId?token=${token}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "applicaiton/json",
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

  const redirecToAllAppointments = () => {
    navigate("/appointments");
  };

  if (error) {
    <Navigate to={"not-found"}></Navigate>;
  }

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Предстоящи часове</h3>
        {appointments.length > 0 ? (
          appointments.map((appointment) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={appointment.id}
            >
              <span>
                {appointment.serviceName} | {appointment.patientName} |{" "}
                {appointment.date}
              </span>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No appointments found</div>
            </div>
          </div>
        )}
        <li className="list-group-item">
          <a href="" onClick={() => redirecToAllAppointments()}>
            Виж всички
          </a>
        </li>
      </ul>
    </div>
  );
}

export default NextAppointmentsList;
