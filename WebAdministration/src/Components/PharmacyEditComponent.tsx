import { useState } from "react";

interface Pharmacy {
  id: number;
  name: string;
  location: string;
  contactNumber: string;
}

interface PharmacyEditProps {
  pharmacy: Pharmacy;
}

function PharmacyEditComponent({ pharmacy }: PharmacyEditProps) {
  const [pharmacyName, setPharmacyName] = useState(pharmacy.name);
  const [pharmacyContactNumber, setPharmacyContactNumber] = useState(
    pharmacy.contactNumber
  );
  const [pharmacyLocation, setPharmacyLocation] = useState(pharmacy.location);
  const [error, setError] = useState<string | null>(null);

  const token = localStorage.getItem("token");

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    setError(null);

    const form = new FormData();
    form.append("Id", pharmacy.id.toString());
    form.append("Name", pharmacyName);
    form.append("Location", pharmacyLocation);
    form.append("ContactNumber", pharmacyContactNumber);

    try {
      const response = await fetch(`http://localhost:5171/api/Pharmacy/Edit`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: form,
      });

      if (!response.ok) {
        throw new Error("Failed to update the pharmacy");
      }

      alert("Pharmacy updated successfully!");
    } catch (error) {
      setError("There was an error updating the pharmacy.");
    }
  };

  return (
    <>
      <h5>Редактиране на аптека #{pharmacy.id}</h5>
      <form onSubmit={handleSubmit}>
        <div className="row">
          <div className="form-group">
            <label htmlFor="pharmacyName">Име на аптеката:</label>
            <input
              type="text"
              id="pharmacyName"
              className="form-control"
              value={pharmacyName}
              onChange={(e) => setPharmacyName(e.target.value)}
            />
          </div>
        </div>

        <br></br>

        <div className="row">
          <div className="col-md-4 form-group">
            <label htmlFor="pharmacyContactNumber">Номер за връзка:</label>
            <input
              type="text"
              id="pharmacyContactNumber"
              className="form-control"
              value={pharmacyContactNumber}
              onChange={(e) => setPharmacyContactNumber(e.target.value)}
            />
          </div>
          <div className="col-md-8 form-group">
            <label htmlFor="pharmacyLocation">Адрес на аптеката:</label>
            <input
              type="text"
              id="pharmacyLocation"
              className="form-control"
              value={pharmacyLocation}
              onChange={(e) => setPharmacyLocation(e.target.value)}
            />
          </div>
        </div>

        <br></br>
        {error && (
          <p className="text-danger">
            Възникна грешка при редактиране на аптека!
          </p>
        )}
        <button type="submit" className="btn btn-primary mt-2">
          Запази промените
        </button>
      </form>
    </>
  );
}

export default PharmacyEditComponent;
