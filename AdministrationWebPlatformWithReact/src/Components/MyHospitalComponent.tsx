import { useEffect, useState } from "react";
import MyHospitalEditComponent from "./MyHospitalEditComponent";

interface Hospital {
  id: number;
  hospitalName: string;
}

function MyHospitalComponent() {
  const [hospital, setHospital] = useState<Hospital>();
  const [error, setError] = useState<boolean>();
  const [selectedEditHospitalId, setSelectedEditHospitalId] = useState<
    number | null
  >(null);

  const token = localStorage.getItem("token");

  const getHospital = async () => {
    try {
      const response = await fetch(
        `http://localhost:5166/api/Hospital/GetDirectorHospital?token=${token}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        const data = await response.json();

        setHospital(data.hospital);
      } else {
        throw new Error("There was an error searching for your hospital");
      }
    } catch (error) {
      console.log("Error!", error);
      setError(true);
    }
  };

  useEffect(() => {
    getHospital();
  }, []);

  const handleHospitalEditClick = (hospitalId: number) => {
    setSelectedEditHospitalId((prev) =>
      prev === hospitalId ? null : hospitalId
    );
  };

  return (
    <div className="col-md-5 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Моята болница</h3>
        <li className="list-group-item d-flex flex-column">
          <div className="d-flex justify-content-between align-items-center">
            {hospital?.hospitalName}
            <div>
              <a
                className="btn btn-warning"
                onClick={() => handleHospitalEditClick(Number(hospital?.id))}
              >
                Редактирай
              </a>
            </div>
          </div>
          {selectedEditHospitalId === hospital?.id && (
            <MyHospitalEditComponent hospitalId={String(hospital.id)} />
          )}
        </li>
        {error && <li className="list-group-item text-danger">Грешка!</li>}
      </ul>
    </div>
  );
}

export default MyHospitalComponent;
