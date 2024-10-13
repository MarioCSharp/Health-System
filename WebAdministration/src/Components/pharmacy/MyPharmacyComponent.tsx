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

  return (
    <div className="col-md-8 mx-auto mb-4">
      <h3 className="text-center mb-4">Моята Аптека</h3>
      {pharmacy ? (
        <div className="card shadow-sm border-0 rounded-lg">
          <div className="card-body">
            <div className="d-flex justify-content-between align-items-center">
              <div>
                <h5 className="card-title">
                  <i className="fas fa-clinic-medical text-primary mr-2" style={{marginRight: "9px"}}></i>
                  {pharmacy.name}
                </h5>
                <p className="card-text text-muted mb-1">
                  <i className="fas fa-map-marker-alt text-danger mr-2" style={{marginRight: "20px"}}></i>
                  {pharmacy.location}
                </p>
                <p className="card-text text-muted mb-1">
                  <i className="fas fa-phone-alt text-success mr-2" style={{marginRight: "15px"}}></i>
                  {pharmacy.contactNumber}
                </p>
              </div>
              <div>
                <button
                  className="btn btn-outline-warning btn-sm"
                  onClick={() => handleEdit(pharmacy)}
                >
                  <i className="fas fa-edit mr-1"></i> Редактирай
                </button>
              </div>
            </div>
            {editingPharmacy && (
              <div className="mt-3">
                <PharmacyEditComponent pharmacy={editingPharmacy} />
              </div>
            )}
          </div>
        </div>
      ) : (
        <div className="alert alert-info text-center">
          <i className="fas fa-info-circle mr-2"></i> Няма намерена аптека
        </div>
      )}
    </div>
  );
}

export default MyPharmacyComponent;
