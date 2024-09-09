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
        // Handle server-side error
        const errorData = await response.json();
        alert(
          "Error: " + errorData.message ||
            "There was an error adding the results"
        );
      }
    } catch (error) {
      // Handle network or other errors
      console.error("Error:", error);
      alert("An unexpected error occurred. Please try again later.");
    }
  };

  return (
    <div className="container">
      <h2>Добавяне на доктор</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="patientName" className="form-label">
            Име на пациента
          </label>
          <input
            type="text"
            className="form-control"
            id="patientName"
            value={patientName}
            onChange={(e) => setPatientName(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="btn btn-primary">
          Запази промените
        </button>
      </form>
    </div>
  );
}

export default LaboratoryAddComponent;
