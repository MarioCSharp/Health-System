import React, { useEffect, useState } from "react";
import PharmacyEditComponent from "./PharmacyEditComponent";
import { useNavigate } from "react-router-dom";

// Define a Pharmacy interface for typing
interface Pharmacy {
  id: number;
  name: string;
  location: string;
  contactNumber: string;
}

const PharmaciesComponent: React.FC = () => {
  const [pharmacies, setPharmacies] = useState<Pharmacy[]>([]);
  const [editingPharmacy, setEditingPharmacy] = useState<Pharmacy | null>(null);
  const token = localStorage.getItem("token");

  const navigate = useNavigate();

  const getPharmacies = async (): Promise<void> => {
    try {
      const response = await fetch("http://localhost:5171/api/Pharmacy/All", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error("There was an error loading the pharmacies");
      }

      const data: Pharmacy[] = await response.json();
      setPharmacies(data.slice(0, 5));
    } catch (error) {
      console.error("Error fetching pharmacies:", error);
      alert("Failed to fetch pharmacies. Please try again.");
    }
  };

  useEffect(() => {
    getPharmacies();
  }, []);

  const handleDelete = async (pharmacyId: number): Promise<void> => {
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

      if (!response.ok) {
        throw new Error("There was an error deleting the pharmacy");
      }

      // Refetch the updated list of pharmacies
      getPharmacies();
    } catch (error) {
      console.error("Error deleting pharmacy:", error);
      alert("Failed to delete pharmacy. Please try again.");
    }
  };

  const handleEdit = (pharmacy: Pharmacy) => {
    setEditingPharmacy(pharmacy);
  };

  const redirectToPharmacists = (pharmacyId: number) => {
    navigate(`/pharmacists/${pharmacyId}`);
  };

  const redirectToAddPharmacy = () => {
    navigate(`/pharmacy/add`);
  };

  const redirectToAllPharmacies = () => {
    navigate(`/pharmacies`);
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
                <button
                  className="btn btn-primary btn-sm"
                  style={{ marginRight: "2px" }}
                  onClick={() => redirectToPharmacists(pharmacy.id)}
                >
                  Фармацевти
                </button>
                <button
                  className="btn btn-warning btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => handleEdit(pharmacy)}
                >
                  Редактирай
                </button>
                <button
                  className="btn btn-danger btn-sm"
                  onClick={() => handleDelete(pharmacy.id)}
                >
                  Изтрий
                </button>
              </div>
              {editingPharmacy && editingPharmacy.id === pharmacy.id && (
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
          <button
            className="btn btn-link"
            onClick={redirectToAddPharmacy}
            style={{ marginRight: "14px" }}
          >
            Добави аптека
          </button>
          <button className="btn btn-link" onClick={redirectToAllPharmacies}>
            Виж всички
          </button>
        </li>
      </ul>
    </div>
  );
};

export default PharmaciesComponent;
