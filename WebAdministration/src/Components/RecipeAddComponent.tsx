import React, { useState } from "react";

function RecipeAddComponent() {
  const [egn, setEgn] = useState<string>("");
  const [patientName, setPatientName] = useState<string>("");
  const [doctorName, setDoctorName] = useState<string>("");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleSubmit = async () => {
    if (!selectedFile || !egn || !patientName || !doctorName) return;

    const formData = new FormData();
    formData.append("FormFile", selectedFile);
    formData.append("PatientEGN", egn);
    formData.append("PatientName", patientName);
    formData.append("DoctorName", doctorName);

    // Log form data
    for (const pair of formData.entries()) {
      console.log(`${pair[0]}: ${pair[1]}`);
    }

    const token = localStorage.getItem("token");

    try {
      const response = await fetch(
        `http://localhost:5250/api/Recipe/AddRecipe`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );

      if (response.ok) {
        console.log("Recipe added successfully");
      } else {
        const errorData = await response.text();
        console.error("Error response from server:", errorData);
        throw new Error("There was an error adding the recipe");
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <input
        type="text"
        placeholder="EGN"
        value={egn}
        onChange={(e) => setEgn(e.target.value)}
      />
      <input
        type="text"
        placeholder="Patient Name"
        value={patientName}
        onChange={(e) => setPatientName(e.target.value)}
      />
      <input
        type="text"
        placeholder="Doctor Name"
        value={doctorName}
        onChange={(e) => setDoctorName(e.target.value)}
      />
      <input type="file" accept=".txt" onChange={handleFileChange} />
      <button className="btn btn-success btn-sm" onClick={handleSubmit}>
        Submit Recipe
      </button>
    </div>
  );
}

export default RecipeAddComponent;
