import { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faSave,
  faEdit,
  faExclamationTriangle,
} from "@fortawesome/free-solid-svg-icons";

interface Props {
  serviceId: string;
}

function MyServiceEditComponent({ serviceId }: Props) {
  const token = localStorage.getItem("token");
  const [error, setError] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [price, setPrice] = useState<string>("");
  const [location, setLocation] = useState<string>("");
  const [description, setDescription] = useState<string>("");

  const getService = async () => {
    try {
      const response = await fetch(
        `http://localhost:5046/api/Service/Edit?id=${serviceId}`,
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
        setName(data.serviceName || "");
        setPrice(data.servicePrice || "");
        setLocation(data.serviceLocation || "");
        setDescription(data.serviceDescription || "");
      } else {
        throw new Error("There was an error loading the service!");
      }
    } catch (error) {
      console.log(error);
      setError(true);
    }
  };

  useEffect(() => {
    getService();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Id", serviceId);
    formData.append("ServiceName", name);
    formData.append("ServicePrice", price);
    formData.append("ServiceDesription", description);
    formData.append("ServiceLocation", location);

    try {
      const response = await fetch(`http://localhost:5046/api/Service/Edit`, {
        method: "POST",
        body: formData,
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.ok) {
        getService();
      } else {
        throw new Error("There was an error loading the service!");
      }
    } catch (error) {
      console.log(error);
      setError(true);
    }
  };

  return (
    <>
      <h4 className="mb-4">
        <FontAwesomeIcon icon={faEdit} className="me-2" />
        Редактиране на услуга #{serviceId}
      </h4>
      <form onSubmit={handleSubmit} className="shadow p-4 bg-light rounded">
        <div className="mb-3">
          <label htmlFor="serviceName" className="form-label">
            Име на услугата:
          </label>
          <input
            type="text"
            id="serviceName"
            className="form-control"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Въведете име на услугата"
          />
        </div>

        <div className="row mb-3">
          <div className="col-md-4">
            <label htmlFor="servicePrice" className="form-label">
              Цена:
            </label>
            <input
              type="number"
              id="servicePrice"
              className="form-control"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
              placeholder="Въведете цена"
            />
          </div>
          <div className="col-md-8">
            <label htmlFor="serviceLocation" className="form-label">
              Адрес:
            </label>
            <input
              type="text"
              id="serviceLocation"
              className="form-control"
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              placeholder="Въведете адрес"
            />
          </div>
        </div>

        <div className="mb-3">
          <label htmlFor="serviceDescription" className="form-label">
            Описание:
          </label>
          <textarea
            rows={3}
            id="serviceDescription"
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="Въведете описание на услугата"
          />
        </div>

        {error && (
          <div className="alert alert-danger mt-3">
            <FontAwesomeIcon icon={faExclamationTriangle} className="me-2" />
            Възникна грешка при редактиране на услугата!
          </div>
        )}

        <button type="submit" className="btn btn-success mt-3">
          <FontAwesomeIcon icon={faSave} className="me-2" />
          Запази промените
        </button>
      </form>
    </>
  );
}

export default MyServiceEditComponent;
