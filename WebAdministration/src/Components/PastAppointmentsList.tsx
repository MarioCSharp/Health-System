import FeedbackComponent from "./FeedbackComponent";
import { useEffect, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import PrescriptionComponent from "./PrescriptionComponent";
import RecipeAddComponent from "./RecipeAddComponent";

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

  const redirecToAllAppointments = () => {
    navigate(`/past-appointments`);
  };

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
      <ul className="list-group">
        <h3>Минали часове</h3>
        {appointments?.length ? (
          appointments.slice(0, 5).map((appointment) => (
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
                  <button
                    className="btn btn-primary btn-sm mr-2"
                    onClick={() => handleAddRecipeClick(appointment.id)}
                  >
                    Добави рецепта
                  </button>
                </div>
              </div>
              {selectedPrescriptionId === appointment.id && (
                <PrescriptionComponent appointmentId={String(appointment.id)} />
              )}
              {selectedFeedbackId === appointment.id && (
                <FeedbackComponent appointmentId={String(appointment.id)} />
              )}
              {selectedRecipe === appointment.id && <RecipeAddComponent />}
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
          <a href="#" onClick={() => navigate(`/past-appointments`)}>
            Виж всички
          </a>
        </li>
      </ul>
    </div>
  );
}

export default PastAppointmentsList;
