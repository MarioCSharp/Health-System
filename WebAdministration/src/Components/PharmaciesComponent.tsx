import { useEffect, useState } from "react";
import PharmacyEditComponent from "./PharmacyEditComponent";
import { useNavigate } from "react-router-dom";

interface Pharmacy {
  id: number;
  name: string;
  location: string;
  contactNumber: string;
}

function PharmaciesComponent() {
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
        setPharmacies(data.slice(0, 5));
      } else {
        throw new Error("There was an error loading the pharmacies");
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
        throw new Error("There was an error deleting the pharmacy");
      }
    } catch (error) {
      alert(error);
    }
  };

  const redirectToAllPharmacies = () => {};

  const handleEdit = (pharmacy: Pharmacy) => {
    setEditingPharmacy(pharmacy);
  };

  const redirectToPharmacists = (pharmacyId: number) => {
    navigate(`/pharmacists/${pharmacyId}`);
  };

  const redirectToAddPharmacy = () => {
    navigate(`/pharmacy/add}`);
  };

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Аптеки</h3>
        {pharmacies.length > 0 ? (
          pharmacies.map((pharmacy) => (
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
                  Фармацефти
                </a>
                <a
                  className="btn btn-warning btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => handleEdit(pharmacy)}
                >
                  Редактирай
                </a>
                <a
                  className="btn btn-danger btn-sm"
                  onClick={() => handleDelete(pharmacy.id)}
                >
                  Изтрий
                </a>
              </div>
              {editingPharmacy && (
                <PharmacyEditComponent pharmacy={editingPharmacy} />
              )}
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No pharmacies found</div>
            </div>
          </div>
        )}
        <li className="list-group-item">
          <a
            href=""
            onClick={() => redirectToAddPharmacy()}
            style={{ marginRight: "14px" }}
          >
            Добави аптека
          </a>
          <a href="" onClick={() => redirectToAllPharmacies()}>
            Виж всички
          </a>
        </li>
      </ul>
    </div>
  );
}

export default PharmaciesComponent;
