import { useEffect, useState } from "react";
import MyHospitalEditComponent from "./MyHospitalEditComponent";
import MyHospitalDetailsComponent from "./MyHospitalDetailsComponent";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEdit,
  faInfoCircle,
  faHospital,
} from "@fortawesome/free-solid-svg-icons";

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
  const [selectedDetailsHospitalId, setSelectedDetailsHospitalId] = useState<
    number | null
  >(null);

  const token = localStorage.getItem("token");

  const getHospital = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Hospital/GetDirectorHospital?token=${token}`,
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

  const handleHospitalDetailsClick = (hospitalId: number) => {
    setSelectedDetailsHospitalId((prev) =>
      prev === hospitalId ? null : hospitalId
    );
  };

  return (
    <div className="col-md-5 mx-md-3 mb-4">
      <div className="card shadow-lg">
        <div className="card-header bg-primary text-white d-flex align-items-center">
          <FontAwesomeIcon icon={faHospital} className="me-2" />
          <h5 className="mb-0">Моята болница</h5>
        </div>
        <div className="card-body">
          {hospital ? (
            <>
              <h4 className="card-title text-center mb-4">
                {hospital.hospitalName}
              </h4>
              <div className="d-flex justify-content-center mb-3">
                <button
                  className="btn btn-outline-info me-2"
                  onClick={() =>
                    handleHospitalDetailsClick(Number(hospital.id))
                  }
                >
                  <FontAwesomeIcon icon={faInfoCircle} className="me-1" />
                  Информация
                </button>
                <button
                  className="btn btn-outline-warning"
                  onClick={() => handleHospitalEditClick(Number(hospital.id))}
                >
                  <FontAwesomeIcon icon={faEdit} className="me-1" />
                  Редактирай
                </button>
              </div>
              {selectedDetailsHospitalId === hospital.id && (
                <MyHospitalDetailsComponent hospitalId={String(hospital.id)} />
              )}
              {selectedEditHospitalId === hospital.id && (
                <MyHospitalEditComponent hospitalId={String(hospital.id)} />
              )}
            </>
          ) : (
            <div className="text-center text-muted">
              Зареждане на болницата...
            </div>
          )}
        </div>
        {error && (
          <div className="card-footer text-danger text-center">
            Грешка при зареждане на болницата!
          </div>
        )}
      </div>
    </div>
  );
}

export default MyHospitalComponent;
