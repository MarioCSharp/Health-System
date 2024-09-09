import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import PharmacyEditComponent from "./PharmacyEditComponent";

interface Pharmacy {
  id: number;
  name: string;
  location: string;
  contactNumber: string;
}

function MyPharmacyComponent() {
  const [pharmacy, setPharmacy] = useState<Pharmacy | undefined>(undefined);
  const [editingPharmacy, setEditingPharmacy] = useState<Pharmacy | null>(null);

  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const getMyPharmacy = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacy/GetMyPharmacy`,
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

        setPharmacy(data);
      } else {
        throw new Error("There was an error getting your pharmacy.");
      }
    } catch (error) {
      alert(error);
    }
  };

  useEffect(() => {
    getMyPharmacy();
  }, []);

  const handleEdit = (pharmacy: Pharmacy) => {
    setEditingPharmacy(pharmacy);
  };

  const redirectToPharmacists = (pharmacyId: number) => {
    navigate(`/pharmacists/${pharmacyId}`);
  };

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Аптеки</h3>
        {pharmacy ? (
          <li
            className="list-group-item d-flex justify-content-between align-items-center"
            key={pharmacy.id}
          >
            <span>
              {pharmacy.name} | {pharmacy.location}
            </span>
            <div>
              <a
                className="btn btn-primary btn-sm"
                style={{ marginRight: "2px" }}
                onClick={() => redirectToPharmacists(pharmacy.id)}
              >
                Фармацевти
              </a>
              <a
                className="btn btn-warning btn-sm mr-2"
                style={{ marginRight: "2px" }}
                onClick={() => handleEdit(pharmacy)}
              >
                Редактирай
              </a>
            </div>
            {editingPharmacy && (
              <PharmacyEditComponent pharmacy={editingPharmacy} />
            )}
          </li>
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No pharmacies found</div>
            </div>
          </div>
        )}
      </ul>
    </div>
  );
}

export default MyPharmacyComponent;
