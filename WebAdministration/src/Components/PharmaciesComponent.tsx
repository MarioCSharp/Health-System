import React, { useEffect, useState } from "react";
import PharmacyEditComponent from "./PharmacyEditComponent";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEdit,
  faTrash,
  faUser,
  faMapMarkerAlt,
  faPlus,
  faUsers,
} from "@fortawesome/free-solid-svg-icons";
import { Spinner } from "react-bootstrap";

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
  const [loading, setLoading] = useState(true);
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
    } finally {
      setLoading(false);
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
      <div className="card shadow-lg rounded-3 border-0">
        <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
          <h3 className="mb-0">
            <FontAwesomeIcon icon={faUsers} /> Аптеки
          </h3>
        </div>
        <div className="card-body">
          {loading ? (
            <div className="text-center">
              <Spinner animation="border" role="status">
                <span className="visually-hidden">Loading...</span>
              </Spinner>
            </div>
          ) : pharmacies.length > 0 ? (
            <ul className="list-group list-group-flush">
              {pharmacies.map((pharmacy) => (
                <li
                  className="list-group-item d-flex justify-content-between align-items-center"
                  key={pharmacy.id}
                >
                  <div className="d-flex align-items-center">
                    <FontAwesomeIcon
                      icon={faMapMarkerAlt}
                      className="text-primary me-3"
                    />
                    <span>
                      <strong>{pharmacy.name}</strong>
                      <br />
                      <small className="text-muted">{pharmacy.location}</small>
                    </span>
                  </div>
                  <div>
                    <button
                      className="btn btn-outline-primary btn-sm me-2"
                      onClick={() => redirectToPharmacists(pharmacy.id)}
                    >
                      <FontAwesomeIcon icon={faUser} /> Фармацевти
                    </button>
                    <button
                      className="btn btn-outline-warning btn-sm me-2"
                      onClick={() => handleEdit(pharmacy)}
                    >
                      <FontAwesomeIcon icon={faEdit} /> Редактирай
                    </button>
                    <button
                      className="btn btn-outline-danger btn-sm"
                      onClick={() => handleDelete(pharmacy.id)}
                    >
                      <FontAwesomeIcon icon={faTrash} /> Изтрий
                    </button>
                  </div>
                  {editingPharmacy && editingPharmacy.id === pharmacy.id && (
                    <PharmacyEditComponent pharmacy={editingPharmacy} />
                  )}
                </li>
              ))}
            </ul>
          ) : (
            <div className="alert alert-warning" role="alert">
              No pharmacies found.
            </div>
          )}
        </div>
        <div className="card-footer text-center">
          <button
            className="btn btn-outline-info me-2"
            onClick={redirectToAddPharmacy}
          >
            <FontAwesomeIcon icon={faPlus} /> Добави аптека
          </button>
          <button
            className="btn btn-outline-info"
            onClick={redirectToAllPharmacies}
          >
            <FontAwesomeIcon icon={faUsers} /> Виж всички
          </button>
        </div>
      </div>
    </div>
  );
};

export default PharmaciesComponent;
