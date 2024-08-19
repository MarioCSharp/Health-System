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
            "Content-Type": "applicaiton/json",
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
      </ul>
    </div>
  );
}

export default AllNextAppointmentsList;
