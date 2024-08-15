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
      const response = await fetch("http://localhost:5166/api/Service/Add", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          doctorId,
          name,
          price,
          location,
          description,
          token,
        }),
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
    <div className="container">
      <h2>Добавяне на услуга</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="name" className="form-label">
            Име на услугата
          </label>
          <input
            type="text"
            className="form-control"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="price" className="form-label">
            Цена на услугата
          </label>
          <input
            type="number"
            className="form-control"
            id="price"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="description" className="form-label">
            Описание на услугата
          </label>
          <input
            type="text"
            className="form-control"
            id="description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="location" className="form-label">
            Локация на услугата
          </label>
          <input
            type="text"
            className="form-control"
            id="location"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            required
          />
        </div>
        {error && (
          <div className="mb-3">
            <p style={{ color: "red" }}>
              There was an error processing your request.
            </p>
          </div>
        )}
        <button type="submit" className="btn btn-warning">
          Запази промените
        </button>
      </form>
    </div>
  );
}

export default ServiceAddComponent;
