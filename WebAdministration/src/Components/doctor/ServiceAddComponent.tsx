import React, { useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";

function ServiceAddComponent() {
  const { doctorId } = useParams<{ doctorId: string }>();
  const [name, setName] = useState<string>("");
  const [price, setPrice] = useState<string>("");
  const [location, setLocation] = useState<string>("");
  const [description, setDescription] = useState<string>("");
  const [error, setError] = useState(false);

  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    try {
      const formData = new FormData();
      formData.append("DoctorId", doctorId!);
      formData.append("Name", name);
      formData.append("Price", price);
      formData.append("Description", description);
      formData.append("Location", location);
      formData.append("Token", token!);

      const response = await fetch("http://localhost:5046/api/Service/Add", {
        method: "POST",
        body: formData,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();

        if (data.success) {
          navigate(`/doctor/services/${doctorId}`);
        } else {
          throw new Error("The request was unsuccessful.");
        }
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setError(true);
    }
  };

  if (error) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <div className="container mt-5">
      <div className="card shadow-sm border-0 rounded-lg">
        <div className="card-header bg-light">
          <h2 className="text-center text-muted">Добавяне на услуга</h2>
        </div>
        <div className="card-body p-4">
          <form onSubmit={handleSubmit}>
            <div className="mb-4">
              <label htmlFor="name" className="form-label fw-bold">
                Име на услугата
              </label>
              <input
                type="text"
                className="form-control form-control-lg shadow-sm rounded"
                id="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
                style={{ borderColor: "#ccd0d5" }}
              />
            </div>
            <div className="mb-4">
              <label htmlFor="price" className="form-label fw-bold">
                Цена на услугата
              </label>
              <input
                type="number"
                className="form-control form-control-lg shadow-sm rounded"
                id="price"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
                required
                style={{ borderColor: "#ccd0d5" }}
              />
            </div>
            <div className="mb-4">
              <label htmlFor="description" className="form-label fw-bold">
                Описание на услугата
              </label>
              <textarea
                className="form-control form-control-lg shadow-sm rounded"
                id="description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                required
                style={{ borderColor: "#ccd0d5", minHeight: "100px" }}
              ></textarea>
            </div>
            <div className="mb-4">
              <label htmlFor="location" className="form-label fw-bold">
                Кабинет
              </label>
              <input
                type="text"
                className="form-control form-control-lg shadow-sm rounded"
                id="location"
                value={location}
                onChange={(e) => setLocation(e.target.value)}
                required
                style={{ borderColor: "#ccd0d5" }}
              />
            </div>
            {error && (
              <div className="mb-3 text-danger">
                There was an error processing your request.
              </div>
            )}
            <button
              type="submit"
              className="btn btn-primary btn-lg w-100 shadow-sm"
              style={{
                backgroundColor: "#007bff",
                borderColor: "#0056b3",
              }}
            >
              Запази промените
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}

export default ServiceAddComponent;
