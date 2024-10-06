import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function LaboratoryAddComponent() {
  const [patientName, setPatientName] = useState<string>("");
  const token = localStorage.getItem("token");

  const [userNameId, setUserNameId] = useState<string>("");
  const [userPass, setUserPass] = useState<string>("");

  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault(); // Prevent default form submission behavior

    try {
      const formData = new FormData();
      formData.append("PatientName", patientName);

      const response = await fetch(
        `http://localhost:5250/api/LaboratoryResult/IssueResult`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );

      if (response.ok) {
        const data = await response.json();

        setUserNameId(data.userId);
        setUserPass(data.userPass);
        alert(
          "Име за проверка на потребителя: " +
            data.userId +
            " " +
            "Парола за проверка на потребителя: " +
            data.userPass
        );

        navigate(`/laboratory/mine`);
      } else {
        const errorData = await response.json();
        alert(
          "Error: " + errorData.message ||
            "There was an error adding the results"
        );
      }
    } catch (error) {
      console.error("Error:", error);
      alert("An unexpected error occurred. Please try again later.");
    }
  };

  return (
    <div className="container mt-5">
      <div className="card shadow-lg p-4">
        <div className="card-header bg-primary text-white text-center">
          <h3>
            <i className="fas fa-user-md me-2"></i> Добавяне на лабораторен
            резултат
          </h3>
        </div>
        <form onSubmit={handleSubmit} className="p-3">
          <div className="mb-4">
            <label htmlFor="patientName" className="form-label">
              <i className="fas fa-user me-2"></i>Име на пациента
            </label>
            <input
              type="text"
              className="form-control form-control-lg"
              id="patientName"
              value={patientName}
              onChange={(e) => setPatientName(e.target.value)}
              placeholder="Въведете името на пациента"
              required
            />
          </div>
          <div className="text-center">
            <button type="submit" className="btn btn-primary btn-lg">
              <i className="fas fa-save me-2"></i>Запази промените
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default LaboratoryAddComponent;
