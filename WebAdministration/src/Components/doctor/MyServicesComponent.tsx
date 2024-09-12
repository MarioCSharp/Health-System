import { useEffect, useState } from "react";
import MyServiceEditComponent from "./MyServiceEditComponent";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEdit,
  faTrash,
  faPlus,
  faExclamationCircle,
} from "@fortawesome/free-solid-svg-icons";

interface Service {
  id: number;
  name: string;
  price: number;
}

function MyServicesComponent() {
  const token = localStorage.getItem("token");
  const [services, setServices] = useState<Service[]>([]);
  const [error, setError] = useState<boolean>(false);
  const [selectedServiceId, setSelectedServiceId] = useState<number | null>();
  const [doctorId, setDoctorId] = useState<number | null>();
  const navigate = useNavigate();

  const getServices = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Service/GetDoctorServices`,
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
        setServices(data.services);
      } else {
        throw new Error("There was an error loading your services!");
      }
    } catch (error) {
      console.log(error);
      setError(true);
      setServices([]);
    }
  };

  const getDoctorId = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/GetDoctorId`,
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
        setDoctorId(data.id);
      } else {
        throw new Error("There was an error loading your services!");
      }
    } catch (error) {
      console.log(error);
      setError(true);
      setDoctorId(null);
    }
  };

  useEffect(() => {
    getServices();
    getDoctorId();
  }, []);

  const handleEditClick = (serviceId: number) => {
    setSelectedServiceId((prev) => (prev === serviceId ? null : serviceId));
  };

  const handleDeleteClick = async (serviceId: number) => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Service/Remove?id=${serviceId}`,
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
        if (!data.success) {
          throw new Error(
            "You are either not authorized or there is a problem in the system!"
          );
        }
        getServices();
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setServices([]);
      setError(true);
    }
  };

  return (
    <div className="col-md-7 mx-md-3 mb-4">
      <ul className="list-group shadow-lg p-4 bg-light rounded">
        <h4 className="mb-4 text-primary">Моите услуги</h4>
        {error && (
          <div
            className="alert alert-danger d-flex align-items-center"
            role="alert"
          >
            <FontAwesomeIcon icon={faExclamationCircle} className="me-2" />
            Възникна грешка при зареждане на услугите!
          </div>
        )}
        {services.length ? (
          services.map((service) => (
            <li
              className="list-group-item bg-white shadow-sm rounded border-0 mb-3 p-3"
              key={service.id}
            >
              <div className="d-flex justify-content-between align-items-center">
                <span className="fw-bold text-dark">
                  {service.name} | {service.price} лв
                </span>
                <div>
                  <button
                    className="btn btn-warning btn-sm me-2 text-white"
                    onClick={() => handleEditClick(service.id)}
                  >
                    <FontAwesomeIcon icon={faEdit} className="me-1" />
                    Редактирай
                  </button>
                  <button
                    className="btn btn-danger btn-sm"
                    onClick={() => handleDeleteClick(service.id)}
                  >
                    <FontAwesomeIcon icon={faTrash} className="me-1" />
                    Изтрий
                  </button>
                </div>
              </div>
              {selectedServiceId === service.id && (
                <MyServiceEditComponent serviceId={String(service.id)} />
              )}
            </li>
          ))
        ) : (
          <div className="card bg-light shadow-sm rounded mb-3">
            <div className="card-body text-center">
              <span className="text-muted">No services found</span>
            </div>
          </div>
        )}
        <li className="list-group-item text-center bg-light">
          <button
            className="btn btn-success btn-sm px-4 py-2"
            onClick={() => navigate(`/service/add/${doctorId}`)}
          >
            <FontAwesomeIcon icon={faPlus} className="me-1" />
            Добави услуга
          </button>
        </li>
      </ul>
    </div>
  );
}

export default MyServicesComponent;
