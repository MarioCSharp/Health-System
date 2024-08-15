import { useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";

interface Service {
  id: number;
  name: string;
  price: number;
}

function DoctorServicesComponent() {
  const { doctorId } = useParams<{ doctorId: string }>();
  const [services, setServices] = useState<Service[]>([]);
  const [fullName, setFullName] = useState<string>("");
  const [error, setError] = useState(false);

  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const getServices = async () => {
    try {
      const response = await fetch(
        `http://localhost:5166/api/Service/AllById?id=${doctorId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        setServices(data.services);
        setFullName(data.fullName);
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

  useEffect(() => {
    getServices();
  }, []);

  const deleteService = async (id: number) => {
    try {
      const response = await fetch(
        `http://localhost:5166/api/Service/Remove?id=${id}&token=${token}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
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

  const redirectToEdit = (id: number) => {
    navigate(`/service/edit/${id}`);
  };

  const redirectToAddService = (id: number) => {
    navigate(`/service/add/${id}`);
  };

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Услуги на {fullName}</h3>
        {services.length > 0 ? (
          services.map((service) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={service.id}
            >
              <span>
                {service.name} | {service.price}лв
              </span>
              <div>
                <a
                  className="btn btn-warning btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToEdit(service.id)}
                >
                  Редактирай
                </a>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => deleteService(service.id)}
                >
                  Изтрий
                </a>
              </div>
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
          <a href="" onClick={() => redirectToAddService(Number(doctorId))}>
            Добави услуга
          </a>
        </li>
      </ul>
    </div>
  );
}

export default DoctorServicesComponent;
