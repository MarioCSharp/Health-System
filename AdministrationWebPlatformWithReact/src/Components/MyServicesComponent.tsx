import { useEffect, useState } from "react";
import MyServiceEditComponent from "./MyServiceEditComponent";

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

  const getServices = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Service/GetDoctorServices`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authentication: `Bearer ${token}`,
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

  useEffect(() => {
    getServices();
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

  return (
    <div className="col-md-7 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Моите услуги</h3>
        {services.length > 0 ? (
          services.map((service) => (
            <li className="list-group-item d-flex flex-column" key={service.id}>
              <div className="d-flex justify-content-between align-items-center">
                <span>
                  {service.name} | {service.price}
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
              <div className="card-body p-2">No appointments found</div>
            </div>
          </div>
        )}
        <li className="list-group-item">
          <a href="">Добави услуга</a>
        </li>
      </ul>
    </div>
  );
}

export default MyServicesComponent;
