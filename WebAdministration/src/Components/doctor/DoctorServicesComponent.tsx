import { useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash, faPlus } from "@fortawesome/free-solid-svg-icons";
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap

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
        `http://localhost:5046/api/Service/AllById?id=${doctorId}`,
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
        `http://localhost:5046/api/Service/Remove?id=${id}`,
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
    <div className="container mt-4">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <h3 className="mb-4 text-center">Услуги на {fullName}</h3>
          <ul className="list-group">
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
                    <button
                      className="btn btn-warning btn-sm mr-2"
                      onClick={() => redirectToEdit(service.id)}
                      style={{marginRight: "2px"}}
                    >
                      <FontAwesomeIcon icon={faEdit} className="mr-1" />
                      Редактирай
                    </button>
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => deleteService(service.id)}
                    >
                      <FontAwesomeIcon icon={faTrash} className="mr-1" />
                      Изтрий
                    </button>
                  </div>
                </li>
              ))
            ) : (
              <div className="col-12">
                <div className="card mb-3">
                  <div className="card-body p-2 text-center">
                    <p>No services found</p>
                  </div>
                </div>
              </div>
            )}
            <li className="list-group-item text-center">
              <button
                className="btn btn-primary btn-sm"
                onClick={() => redirectToAddService(Number(doctorId))}
              >
                <FontAwesomeIcon icon={faPlus} className="mr-1" />
                Добави услуга
              </button>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
}

export default DoctorServicesComponent;
