import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

interface Pharmacist {
  id: number;
  name: string;
  email: string;
}

function PharmacistsInPharmacyComponent() {
  const { pharmacyId } = useParams<{ pharmacyId: string }>();
  const [pharmacists, setPharmacists] = useState<Pharmacist[]>([]);

  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const getPharmacists = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacist/AllInPharmacy?pharmacyId=${pharmacyId}`,
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

        setPharmacists(data);
      } else {
        throw new Error("There was an error loading the pharmacists");
      }
    } catch (error) {
      alert(error);
    }
  };

  useEffect(() => {
    getPharmacists();
  }, []);

  const redirectToAdd = () => {
    navigate(`/pharmacist/add/${pharmacyId}`);
  };

  const handleDelete = async (pharmacistId: number) => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacist/Delete?pharmacistId=${pharmacistId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        getPharmacists();
      } else {
        throw new Error("There was an error deleting the pharmacy");
      }
    } catch (error) {
      alert(error);
    }
  };

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Фармацевти</h3>
        {pharmacists.length > 0 ? (
          pharmacists.map((pharmacist) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={pharmacist.id}
            >
              <p>
                {pharmacist.name} | {pharmacist.email}
              </p>
              <div>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => handleDelete(pharmacist.id)}
                >
                  Изтрий
                </a>
              </div>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No pharmacists found</div>
            </div>
          </div>
        )}
        <li className="list-group-item">
          <a
            href=""
            onClick={() => redirectToAdd()}
            style={{ marginRight: "14px" }}
          >
            Добави фармацевт
          </a>
        </li>
      </ul>
    </div>
  );
}

export default PharmacistsInPharmacyComponent;
