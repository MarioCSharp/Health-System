import React, { useState, useEffect } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";

const ServiceEditComponent: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [serviceName, setServiceName] = useState<string>("");
  const [servicePrice, setServicePrice] = useState<string>("");
  const [serviceDesciption, setServiceDesciption] = useState<string>("");
  const [serviceLocation, setServiceLocation] = useState<string>("");
  const [error, setError] = useState(false);

  const navigate = useNavigate();

  const token = localStorage.getItem("token");

  const getService = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Service/Edit?id=${id}`,
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
        setServiceName(data.serviceName || "");
        setServicePrice(data.servicePrice || "");
        setServiceDesciption(data.serviceDescription || "");
        setServiceLocation(data.serviceLocation || "");
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

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    try {
      const formData = new FormData();
      formData.append("Id", id || "");
      formData.append("ServiceName", serviceName);
      formData.append("ServicePrice", servicePrice);
      formData.append("ServiceDesription", serviceDesciption);
      formData.append("ServiceLocation", serviceLocation);
      formData.append("Token", token || "");

      const response = await fetch(`http://localhost:5046/api/Service/Edit`, {
        method: "POST",
        body: formData,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();

        if (!data.success) {
          throw new Error(
            "You are either not authorized or there is a problem in the system!"
          );
        }

        navigate(`/`);
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

  useEffect(() => {
    getService();
  }, []);

  if (error) {
    return <Navigate to="/not-found" replace />;
  }

  return (
    <div className="container">
      <h2>Редактиране на услуга</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="serviceName" className="form-label">
            Име на услугата
          </label>
          <input
            type="text"
            className="form-control"
            id="serviceName"
            value={serviceName}
            onChange={(e) => setServiceName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="servicePrice" className="form-label">
            Цена на услугата
          </label>
          <input
            type="number"
            className="form-control"
            id="servicePrice"
            value={servicePrice}
            onChange={(e) => setServicePrice(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="serviceDesciption" className="form-label">
            Описание на услугата
          </label>
          <input
            type="text"
            className="form-control"
            id="serviceDesciption"
            value={serviceDesciption}
            onChange={(e) => setServiceDesciption(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="serviceLocation" className="form-label">
            Локация на услугата
          </label>
          <input
            type="text"
            className="form-control"
            id="serviceLocation"
            value={serviceLocation}
            onChange={(e) => setServiceLocation(e.target.value)}
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
};

export default ServiceEditComponent;
