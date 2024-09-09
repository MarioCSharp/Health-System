import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function PharmacyAddComponent() {
  const [users, setUsers] = useState<User[]>([]);
  const [error, setError] = useState(false);
  const [pharmacyName, setPharmacyName] = useState<string>("");
  const [location, setLocation] = useState<string>("");
  const [contactNumber, setContactNumber] = useState<string>("");
  const [selectedUserId, setSelectedUserId] = useState<string>("");
  const token = localStorage.getItem("token");

  const navigate = useNavigate();

  const getUsersWithNoRoles = async () => {
    try {
      const response = await fetch(
        `http://localhost:5196/api/Account/GetAccountsWithNoRoles`,
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

        setUsers(data.users || []);
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setUsers([]);
      setError(true);
    }
  };

  useEffect(() => {
    getUsersWithNoRoles();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); // Prevent the form from submitting and reloading the page.

    const form = new FormData();
    form.append("PharmacyName", pharmacyName);
    form.append("Location", location);
    form.append("ContactNumber", contactNumber);
    form.append("OwnerUserId", selectedUserId);

    try {
      const response = await fetch(`http://localhost:5171/api/Pharmacy/Add`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: form, // No need to manually set Content-Type here.
      });

      if (response.ok) {
        navigate("/"); // Navigate back to the main page on success.
      } else {
        throw new Error("There was an error adding the pharmacy");
      }
    } catch (error) {
      console.log(error);
      setError(true);
    }
  };

  return (
    <div className="container">
      <h2>Добавяне на аптека</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="userSelect" className="form-label">
            Избери управител на аптеката
          </label>
          <select
            id="userSelect"
            className="form-control"
            value={selectedUserId || ""}
            onChange={(e) => setSelectedUserId(e.target.value)}
            required
          >
            <option value="" disabled>
              Избери потребител
            </option>
            {users.map((user) => (
              <option key={user.id} value={user.id}>
                {user.fullName} - {user.email}
              </option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label htmlFor="pharmacyName" className="form-label">
            Име на аптеката
          </label>
          <input
            type="text"
            className="form-control"
            id="pharmacyName"
            value={pharmacyName}
            onChange={(e) => setPharmacyName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="contactNumber" className="form-label">
            Номер за връзка
          </label>
          <input
            type="text"
            className="form-control"
            id="contactNumber"
            value={contactNumber}
            onChange={(e) => setContactNumber(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="location" className="form-label">
            Локация на аптеката
          </label>
          <input
            type="text"
            className="form-control"
            id="location"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            required
          />
        </div>
        {error && (
          <div className="mb-3">
            <p style={{ color: "red" }}>
              There was an error processing your request.
            </p>
          </div>
        )}
        <button type="submit" className="btn btn-warning">
          Добавяне
        </button>
      </form>
    </div>
  );
}

export default PharmacyAddComponent;
