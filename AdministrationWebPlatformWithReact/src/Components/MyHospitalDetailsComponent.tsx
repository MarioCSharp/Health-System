import { useEffect, useState } from "react";

interface Props {
  hospitalId: string;
}

interface Hospital {
  hospitalName: string;
  location: string;
  contactNumber: string;
}

function MyHospitalDetailsComponent({ hospitalId }: Props) {
  const [hospital, setHospital] = useState<Hospital | null>();
  const [error, setError] = useState<boolean>();

  const getHospitalInfo = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Hospital/Details?id=${hospitalId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        const data = await response.json();

        const hospitalInstance: Hospital = {
          hospitalName: data.hospitalName,
          location: data.location,
          contactNumber: data.contactNumber,
        };

        setHospital(hospitalInstance);
      } else {
        throw new Error("There was an error loading your hospital!");
      }
    } catch (error) {
      console.log(error);
      setHospital(null);
      setError(true);
    }
  };

  useEffect(() => {
    getHospitalInfo();
  }, []);

  return (
    <>
      <h5>Информация за болница #{hospitalId}</h5>
      <div className="row">
        <div className="form-group">
          <label htmlFor="hospitalName">Име на болницата:</label>
          <input
            type="text"
            id="hospitalName"
            className="form-control"
            value={hospital?.hospitalName}
            contentEditable={false}
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
            value={hospital?.contactNumber}
            contentEditable={false}
          />
        </div>
        <div className="col-md-8 form-group">
          <label htmlFor="hospitalLocation">Адрес на болницата:</label>
          <input
            type="text"
            id="hospitalLocation"
            className="form-control"
            value={hospital?.location}
            contentEditable={false}
          />
        </div>
      </div>
      <br></br>
      {error && (
        <p className="text-danger">
          Възникна грешка при зареждане на болницата!
        </p>
      )}
    </>
  );
}

export default MyHospitalDetailsComponent;
