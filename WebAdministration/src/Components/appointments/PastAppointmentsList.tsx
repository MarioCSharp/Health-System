import { useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import PrescriptionComponent from "../doctor/PrescriptionComponent";
import FeedbackComponent from "../doctor/FeedbackComponent";
import RecipeAddComponent from "../doctor/RecipeAddComponent";

interface Appointment {
  id: number;
  serviceName: string;
  patientName: string;
  date: string;
}

function PastAppointmentsList() {
  const token = localStorage.getItem("token");
  const [appointments, setAppointments] = useState<Appointment[]>([]);
  const [error, setError] = useState<boolean>(false);
  const [selectedFeedbackId, setSelectedFeedbackId] = useState<number | null>(
    null
  );
  const [selectedPrescriptionId, setSelectedPrescriptionId] = useState<
    number | null
  >(null);
  const [selectedRecipe, setSelectedRecipe] = useState<number | null>();

  const navigate = useNavigate();

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

  const handleAddRecipeClick = (appointmentId: number) => {
    setSelectedRecipe((prev) =>
      prev === appointmentId ? null : appointmentId
    );
  };

  return (
    <div className="col-md-7 mx-md-3 mb-4">
      <div className="card shadow-sm">
        <div className="card-header bg-primary text-white">
          <h5 className="mb-0">
            <i className="fas fa-history me-2"></i> Минали часове
          </h5>
        </div>
        <ul className="list-group list-group-flush">
          {appointments?.length ? (
            appointments.map((appointment) => (
              <li
                className="list-group-item d-flex flex-column"
                key={appointment.id}
              >
                <div className="d-flex justify-content-between align-items-center">
                  <span>
                    <i className="fas fa-user-md text-primary me-2"></i>
                    {appointment.patientName} | {appointment.serviceName} |{" "}
                    {appointment.date}
                  </span>
                  <div>
                    <button
                      className="btn btn-info btn-sm me-2"
                      onClick={() => handlePrescriptionClick(appointment.id)}
                    >
                      <i className="fas fa-file-medical me-1"></i> Издай
                      амбулаторен лист
                    </button>
                    <button
                      className="btn btn-warning btn-sm me-2"
                      onClick={() => handleFeedbackClick(appointment.id)}
                    >
                      <i className="fas fa-comment-medical me-1"></i> Обратна
                      връзка
                    </button>
                    <button
                      className="btn btn-success btn-sm me-2"
                      onClick={() => handleAddRecipeClick(appointment.id)}
                    >
                      <i className="fas fa-prescription-bottle-alt me-1"></i>{" "}
                      Добави рецепта
                    </button>
                  </div>
                </div>
                {selectedPrescriptionId === appointment.id && (
                  <div className="mt-2">
                    <PrescriptionComponent
                      appointmentId={String(appointment.id)}
                    />
                  </div>
                )}
                {selectedFeedbackId === appointment.id && (
                  <div className="mt-2">
                    <FeedbackComponent appointmentId={String(appointment.id)} />
                  </div>
                )}
                {selectedRecipe === appointment.id && (
                  <div className="mt-2">
                    <RecipeAddComponent />
                  </div>
                )}
              </li>
            ))
          ) : (
            <li className="list-group-item text-center p-3">
              <p className="text-muted">No past appointments found</p>
            </li>
          )}
        </ul>
        <div className="card-footer text-center">
          <button
            className="btn btn-outline-primary"
            onClick={() => navigate(`/past-appointments`)}
          >
            <i className="fas fa-calendar-alt me-2"></i> Виж всички
          </button>
        </div>
      </div>
    </div>
  );
}

export default PastAppointmentsList;
