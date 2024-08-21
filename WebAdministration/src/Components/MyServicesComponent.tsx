import { useEffect, useState } from "react";
import MyServiceEditComponent from "./MyServiceEditComponent";
import { redirect, useNavigate } from "react-router-dom";

interface Service {
  id: number;
  name: string;
  price: number;
}

function MyServicesComponent() {
  const token = localStorage.getItem("token");
  const [services, setServices] = useState<Service[]>([]);
  const [error, setError] = useState<boolean>();

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
        const success = data.success;

        if (!success) {
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

  const handleAddClick = () => {
    navigate(`/service/add/${doctorId}`);
  };

  return (
    <div className="col-md-7 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Моите услуги</h3>
        {services?.length ? (
          services.map((service) => (
            <li className="list-group-item d-flex flex-column" key={service.id}>
              <div className="d-flex justify-content-between align-items-center">
                <span>
                  {service.name} | {service.price} лв
                </span>
                <div>
                  <button
                    className="btn btn-warning btn-sm mr-2"
                    onClick={() => handleEditClick(service.id)}
                  >
                    Редактирай
                  </button>
                  <button
                    className="btn btn-danger btn-sm mr-2"
                    onClick={() => handleDeleteClick(service.id)}
                  >
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
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No services found</div>
            </div>
          </div>
        )}
        <li className="list-group-item">
          <a href="#" onClick={() => navigate(`/service/add/${doctorId}`)}>
            Добави услуга
          </a>
        </li>
      </ul>
    </div>
  );
}

export default MyServicesComponent;
