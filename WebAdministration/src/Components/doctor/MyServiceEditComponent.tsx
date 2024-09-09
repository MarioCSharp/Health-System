import { useEffect, useState } from "react";

interface Props {
  serviceId: string;
}

function MyServiceEditComponent({ serviceId }: Props) {
  const token = localStorage.getItem("token");
  const [error, setError] = useState<boolean>();
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

  const handleSubmit = async () => {
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
      <h5>Редактиране на услуга #{serviceId}</h5>
      <form onSubmit={handleSubmit}>
        <div className="row">
          <div className="form-group">
            <label htmlFor="serviceName">Име на услугата:</label>
            <input
              type="text"
              id="serviceName"
              className="form-control"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>
        </div>

        <br></br>

        <div className="row">
          <div className="col-md-4 form-group">
            <label htmlFor="servicePrice">Цена:</label>
            <input
              type="number"
              id="servicePrice"
              className="form-control"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
            />
          </div>
          <div className="col-md-8 form-group">
            <label htmlFor="serviceLocation">Адрес:</label>
            <input
              type="text"
              id="serviceLocation"
              className="form-control"
              value={location}
              onChange={(e) => setLocation(e.target.value)}
            />
          </div>
        </div>

        <br></br>

        <div className="row">
          <div className="form-group">
            <label htmlFor="serviceDescription">Описание:</label>
            <textarea
              rows={3}
              id="serviceDescription"
              className="form-control"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
          </div>
        </div>
        <br></br>
        {error && (
          <p className="text-danger">
            Възникна грешка при редактиране на услугата!
          </p>
        )}
        <button type="submit" className="btn btn-primary mt-2">
          Запази промените
        </button>
      </form>
    </>
  );
}

export default MyServiceEditComponent;
