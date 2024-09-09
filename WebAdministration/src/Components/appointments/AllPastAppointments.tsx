import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import PrescriptionComponent from "../doctor/PrescriptionComponent";
import FeedbackComponent from "../doctor/FeedbackComponent";

interface Appointment {
  id: number;
  serviceName: string;
  patientName: string;
  date: string;
}

function AllPastAppointmentsList() {
  const token = localStorage.getItem("token");
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [error, setError] = useState<boolean>(false);
  const [selectedFeedbackId, setSelectedFeedbackId] = useState<number | null>(
    null
  );
  const [selectedPrescriptionId, setSelectedPrescriptionId] = useState<
    number | null
  >(null);

  const getAppointments = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Appointment/GetPastAppointmentsByDoctorUserId`,
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
        setAppointments(data.appointments.slice);
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

  const handleFeedbackClick = (appointmentId: number) => {
    setSelectedFeedbackId((prev) =>
      prev === appointmentId ? null : appointmentId
    );
  };

  const handlePrescriptionClick = (appointmentId: number) => {
    setSelectedPrescriptionId((prev) =>
      prev === appointmentId ? null : appointmentId
    );
  };

  return (
    <div className="col-md-7 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Минали часове</h3>
        {appointments.length > 0 ? (
          appointments.map((appointment) => (
            <li
              className="list-group-item d-flex flex-column"
              key={appointment.id}
            >
              <div className="d-flex justify-content-between align-items-center">
                <span>
                  {appointment.serviceName} | {appointment.patientName} |{" "}
                  {appointment.date}
                </span>
                <div>
                  <button
                    className="btn btn-primary btn-sm mr-2"
                    onClick={() => handlePrescriptionClick(appointment.id)}
                  >
                    Издай амбулаторен лист
                  </button>
                  <button
                    className="btn btn-primary btn-sm mr-2"
                    onClick={() => handleFeedbackClick(appointment.id)}
                  >
                    Обратна връзка
                  </button>
                </div>
              </div>
              {selectedPrescriptionId === appointment.id && (
                <PrescriptionComponent appointmentId={String(appointment.id)} />
              )}
              {selectedFeedbackId === appointment.id && (
                <FeedbackComponent appointmentId={String(appointment.id)} />
              )}
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

export default AllPastAppointmentsList;
