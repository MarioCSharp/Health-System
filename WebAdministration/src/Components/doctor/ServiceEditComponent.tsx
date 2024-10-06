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
    <div className="container mt-5">
      <div className="card shadow-lg p-4">
        <div className="card-header bg-primary text-white text-center">
          <h3>
            <i className="fas fa-edit me-2"></i> Редактиране на услуга
          </h3>
        </div>
        <form onSubmit={handleSubmit} className="p-3">
          <div className="mb-4">
            <label htmlFor="serviceName" className="form-label">
              <i className="fas fa-briefcase me-2"></i> Име на услугата
            </label>
            <input
              type="text"
              className="form-control form-control-lg"
              id="serviceName"
              value={serviceName}
              onChange={(e) => setServiceName(e.target.value)}
              placeholder="Въведете името на услугата"
              required
            />
          </div>
          <div className="mb-4">
            <label htmlFor="servicePrice" className="form-label">
              <i className="fas fa-money-bill-wave me-2"></i> Цена на услугата
            </label>
            <input
              type="number"
              className="form-control form-control-lg"
              id="servicePrice"
              value={servicePrice}
              onChange={(e) => setServicePrice(e.target.value)}
              placeholder="Въведете цената на услугата"
              required
            />
          </div>
          <div className="mb-4">
            <label htmlFor="serviceDesciption" className="form-label">
              <i className="fas fa-info-circle me-2"></i> Описание на услугата
            </label>
            <input
              type="text"
              className="form-control form-control-lg"
              id="serviceDesciption"
              value={serviceDesciption}
              onChange={(e) => setServiceDesciption(e.target.value)}
              placeholder="Въведете описание"
              required
            />
          </div>
          <div className="mb-4">
            <label htmlFor="serviceLocation" className="form-label">
              <i className="fas fa-map-marker-alt me-2"></i> Локация на услугата
            </label>
            <input
              type="text"
              className="form-control form-control-lg"
              id="serviceLocation"
              value={serviceLocation}
              onChange={(e) => setServiceLocation(e.target.value)}
              placeholder="Въведете локацията"
              required
            />
          </div>
          {error && (
            <div className="alert alert-danger">
              Възникна грешка при обработката на вашата заявка.
            </div>
          )}
          <div className="text-center">
            <button type="submit" className="btn btn-warning btn-lg">
              <i className="fas fa-save me-2"></i> Запази промените
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ServiceEditComponent;
