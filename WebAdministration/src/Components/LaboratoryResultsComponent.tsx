import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface Result {
  id: number;
  patientName: string;
  date: string;
}

function LaboratoryResultsComponent() {
  const [results, setResults] = useState<Result[]>([]);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [uploadingResultId, setUploadingResultId] = useState<number | null>(
    null
  );

  const navigate = useNavigate();
  const token = localStorage.getItem("token");

  const getResults = async () => {
    try {
      const response = await fetch(
        `http://localhost:5250/api/LaboratoryResult/GetDoctorResults`,
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
        setResults(data.results);
      } else {
        throw new Error("There was an error loading the results");
      }
    } catch (error) {
      console.log(error);
      setResults([]);
    }
  };

  useEffect(() => {
    getResults();
  }, []);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleAddFile = async (resultId: number) => {
    if (!selectedFile) {
      alert("Please select a file first.");
      return;
    }

    const formData = new FormData();
    formData.append("resultId", resultId.toString());
    formData.append("file", selectedFile);

    try {
      const response = await fetch(
        `http://localhost:5250/api/LaboratoryResult/AddFile`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );

      if (response.ok) {
        alert("File uploaded successfully");
        setSelectedFile(null); // Clear the file after successful upload
        setUploadingResultId(null);
      } else {
        throw new Error("There was an error uploading the file");
      }
    } catch (error) {
      console.error(error);
      alert("Failed to upload the file. Please try again.");
    }
  };

  const handleAddResults = () => {
    navigate(`/laboratory/add`);
  };

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Лабораторни резултати</h3>
        <button onClick={handleAddResults} className="btn btn-primary">
          Добави запис
        </button>
        {results.length > 0 ? (
          results.map((result) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={result.id}
            >
              <span>
                {result.patientName} - {result.date}
              </span>
              <div>
                <input
                  type="file"
                  onChange={handleFileChange}
                  style={{
                    display: uploadingResultId === result.id ? "block" : "none",
                  }}
                />
                <button
                  className="btn btn-primary btn-sm"
                  style={{ marginRight: "2px" }}
                  onClick={() => {
                    setUploadingResultId(result.id);
                  }}
                >
                  Добави файл
                </button>
                {uploadingResultId === result.id && (
                  <button
                    className="btn btn-success btn-sm"
                    onClick={() => handleAddFile(result.id)}
                  >
                    Качи файл
                  </button>
                )}
              </div>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No results found</div>
            </div>
          </div>
        )}
      </ul>
    </div>
  );
}

export default LaboratoryResultsComponent;
