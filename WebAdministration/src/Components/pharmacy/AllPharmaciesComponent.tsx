import { useEffect, useState } from "react";
import PharmacyEditComponent from "./PharmacyEditComponent";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faTrash, faUserMd } from "@fortawesome/free-solid-svg-icons";

interface Pharmacy {
  id: number;
  name: string;
  location: string;
  contactNumber: string;
}

function AllPharmaciesComponent() {
  const [pharmacies, setPharmacies] = useState<Pharmacy[]>([]);
  const [editingPharmacy, setEditingPharmacy] = useState<Pharmacy | null>(null);
  const token = localStorage.getItem("token");

  const navigate = useNavigate();

  const getPharmacies = async () => {
    try {
      const response = await fetch(`http://localhost:5171/api/Pharmacy/All`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.ok) {
        const data = await response.json();
        setPharmacies(data);
      } else {
        throw new Error("Възникна грешка при зареждането на аптеките");
      }
    } catch (error) {
      alert(error);
    }
  };

  useEffect(() => {
    getPharmacies();
  }, []);

  const handleDelete = async (pharmacyId: number) => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacy/Delete?id=${pharmacyId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        getPharmacies();
      } else {
        throw new Error("Възникна грешка при изтриването на аптеката");
      }
    } catch (error) {
      alert(error);
    }
  };

  const handleEdit = (pharmacy: Pharmacy) => {
    setEditingPharmacy(pharmacy);
  };

  const redirectToPharmacists = (pharmacyId: number) => {
    navigate(`/pharmacists/${pharmacyId}`);
  };

  return (
    <div className="container mt-4">
      <h3 className="mb-4 text-center">Аптеки</h3>
      <div className="row justify-content-center">
        {pharmacies.length > 0 ? (
          <ul className="list-group col-md-8">
            {pharmacies.map((pharmacy) => (
              <li
                className="list-group-item d-flex justify-content-between align-items-center"
                key={pharmacy.id}
              >
                <div>
                  <strong>{pharmacy.name}</strong> | {pharmacy.location}
                </div>
                <div>
                  <button
                    className="btn btn-sm btn-primary me-2"
                    onClick={() => redirectToPharmacists(pharmacy.id)}
                  >
                    <FontAwesomeIcon icon={faUserMd} className="me-1" />
                    Фармацевти
                  </button>
                  <button
                    className="btn btn-sm btn-warning me-2"
                    onClick={() => handleEdit(pharmacy)}
                  >
                    <FontAwesomeIcon icon={faEdit} className="me-1" />
                    Редактирай
                  </button>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDelete(pharmacy.id)}
                  >
                    <FontAwesomeIcon icon={faTrash} className="me-1" />
                    Изтрий
                  </button>
                </div>
                {editingPharmacy && editingPharmacy.id === pharmacy.id && (
                  <PharmacyEditComponent pharmacy={editingPharmacy} />
                )}
              </li>
            ))}
          </ul>
        ) : (
          <div className="col-md-8">
            <div className="alert alert-info text-center" role="alert">
              Няма намерени аптеки
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default AllPharmaciesComponent;
