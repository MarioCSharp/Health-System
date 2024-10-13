import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface Pharmacist {
  id: number;
  name: string;
  email: string;
}

function PharmacistsInPharmacyComponent() {
  const [pharmacists, setPharmacists] = useState<Pharmacist[]>([]);
  const [pharmacyId, setPharmacyId] = useState<string | null>(null);

  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  // Fetch the pharmacy ID associated with the current user
  const getMyPharmacyId = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacy/GetMyPharmacyId`,
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
        setPharmacyId(data); // Set the pharmacy ID
      } else {
        throw new Error("Failed to retrieve pharmacy ID");
      }
    } catch (error) {
      alert(error);
    }
  };

  // Fetch pharmacists based on the pharmacy ID
  const getPharmacists = async (pharmacyId: string) => {
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
    // Fetch the pharmacy ID when the component mounts
    const fetchPharmacyIdAndPharmacists = async () => {
      await getMyPharmacyId();
    };

    fetchPharmacyIdAndPharmacists();
  }, []);

  useEffect(() => {
    // Fetch the pharmacists once we have the pharmacy ID
    if (pharmacyId) {
      getPharmacists(pharmacyId);
    }
  }, [pharmacyId]);

  const redirectToAdd = () => {
    if (pharmacyId) {
      navigate(`/pharmacist/add/${pharmacyId}`);
    }
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
        // Refresh the list of pharmacists after deletion
        if (pharmacyId) {
          getPharmacists(pharmacyId);
        }
      } else {
        throw new Error("There was an error deleting the pharmacy");
      }
    } catch (error) {
      alert(error);
    }
  };

  return (
    <div className="col-md-8 mx-auto mb-4">
      <h3 className="text-center mb-4">
        <i className="fas fa-user-nurse mr-2 text-primary"></i> Фармацевти
      </h3>
      {pharmacists.length > 0 ? (
        pharmacists.map((pharmacist) => (
          <div className="card shadow-sm mb-3" key={pharmacist.id}>
            <div className="card-body d-flex justify-content-between align-items-center">
              <div>
                <h5 className="mb-1">
                  <i className="fas fa-user text-info mr-2" style={{marginRight: "8px"}}></i>
                  {pharmacist.name}
                </h5>
                <p className="text-muted mb-0">
                  <i className="fas fa-envelope text-secondary mr-2" style={{marginRight: "9px"}}></i>
                  {pharmacist.email}
                </p>
              </div>
              <div>
                <button
                  className="btn btn-danger btn-sm"
                  onClick={() => handleDelete(pharmacist.id)}
                >
                  <i className="fas fa-trash-alt mr-1"></i> Изтрий
                </button>
              </div>
            </div>
          </div>
        ))
      ) : (
        <div className="alert alert-info text-center">
          <i className="fas fa-info-circle mr-2"></i> Няма намерени фармацевти
        </div>
      )}
      <div className="text-center mt-4">
        <button
          className="btn btn-success"
          onClick={redirectToAdd}
          disabled={!pharmacyId} // Disable if pharmacy ID is not fetched yet
        >
          <i className="fas fa-plus-circle mr-1"></i> Добави фармацевт
        </button>
      </div>
    </div>
  );
}

export default PharmacistsInPharmacyComponent;
