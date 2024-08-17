import { useEffect, useState } from "react";

interface Props {
  hospitalId: string;
}
function MyHospitalEditComponent({ hospitalId }: Props) {
  const [hospitalName, setHospitalName] = useState("");
  const [hospitalLocation, setHospitalLocation] = useState("");
  const [hospitalContactNumber, setHospitalContactNumber] = useState("");

  const [error, setError] = useState(false);

  const token = localStorage.getItem("token");

  const getHospital = async () => {
    try {
      const response = await fetch(
        `http://localhost:5166/api/Hospital/GetHospital?token=${token}&hospitalId=${hospitalId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        const data = await response.json();

        setHospitalContactNumber(data.contactNumber);
        setHospitalLocation(data.location);
        setHospitalName(data.name);
      }
    } catch (error) {
      console.log("Error!", error);
      setError(true);
    }
  };

  const handleSubmit = async () => {
    const formData = new FormData();
    formData.append("Id", hospitalId);
    formData.append("HospitalName", hospitalName);
    formData.append("HospitalLocation", hospitalLocation);
    formData.append("HospitalContactNumber", hospitalContactNumber);
    formData.append("Token", token!);

    try {
      const response = await fetch("http://localhost:5166/api/Hospital/Edit", {
        method: "POST",
        body: formData,
      });

      if (response.ok) {
        getHospital();
      }
    } catch (error) {
      console.log("Error!", error);
      setError(true);
    }
  };

  useEffect(() => {
    getHospital();
  }, []);

  return (
    <>
      <h5>Редактиране на болница #{hospitalId}</h5>
      <form onSubmit={handleSubmit}>
        <div className="row">
          <div className="form-group">
            <label htmlFor="hospitalName">Име на болницата:</label>
            <input
              type="text"
              id="hospitalName"
              className="form-control"
              value={hospitalName}
              onChange={(e) => setHospitalName(e.target.value)}
            />
          </div>
        </div>

        <br></br>

        <div className="row">
          <div className="col-md-4 form-group">
            <label htmlFor="hospitalContactNumber">Номер за връзка:</label>
            <input
              type="text"
              id="hospitalContactNumber"
              className="form-control"
              value={hospitalContactNumber}
              onChange={(e) => setHospitalContactNumber(e.target.value)}
            />
          </div>
          <div className="col-md-8 form-group">
            <label htmlFor="hospitalLocation">Адрес на болницата:</label>
            <input
              type="text"
              id="hospitalLocation"
              className="form-control"
              value={hospitalLocation}
              onChange={(e) => setHospitalLocation(e.target.value)}
            />
          </div>
        </div>
        <br></br>
        {error && (
          <p className="text-danger">
            Възникна грешка при редактиране на болницата!
          </p>
        )}
        <button type="submit" className="btn btn-primary mt-2">
          Запази промените
        </button>
      </form>
    </>
  );
}

export default MyHospitalEditComponent;
