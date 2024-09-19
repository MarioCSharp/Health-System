import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPlusCircle,
  faPhone,
  faMapMarkerAlt,
  faUser,
} from "@fortawesome/free-solid-svg-icons";

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
        throw new Error("Не сте оторизирани или има проблем в системата!");
      }
    } catch (error) {
      console.log("Възникна грешка", error);
      setUsers([]);
      setError(true);
    }
  };

  useEffect(() => {
    getUsersWithNoRoles();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

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
        body: form,
      });

      if (response.ok) {
        navigate("/");
      } else {
        throw new Error("Възникна грешка при добавянето на аптеката");
      }
    } catch (error) {
      console.log(error);
      setError(true);
    }
  };

  return (
    <div className="container mt-5">
      <h2 className="text-center mb-4">
        <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
        Добавяне на аптека
      </h2>
      <form onSubmit={handleSubmit} className="bg-light p-4 rounded shadow">
        <div className="mb-3">
          <label htmlFor="userSelect" className="form-label">
            <FontAwesomeIcon icon={faUser} className="me-2" />
            Избери управител на аптеката
          </label>
          <select
            id="userSelect"
            className="form-select"
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
            <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
            Име на аптеката
          </label>
          <input
            type="text"
            className="form-control"
            id="pharmacyName"
            placeholder="Въведете име на аптеката"
            value={pharmacyName}
            onChange={(e) => setPharmacyName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="contactNumber" className="form-label">
            <FontAwesomeIcon icon={faPhone} className="me-2" />
            Номер за връзка
          </label>
          <input
            type="text"
            className="form-control"
            id="contactNumber"
            placeholder="Въведете номер за връзка"
            value={contactNumber}
            onChange={(e) => setContactNumber(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="location" className="form-label">
            <FontAwesomeIcon icon={faMapMarkerAlt} className="me-2" />
            Локация на аптеката
          </label>
          <input
            type="text"
            className="form-control"
            id="location"
            placeholder="Въведете локация"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            required
          />
        </div>
        {error && (
          <div className="alert alert-danger" role="alert">
            Възникна грешка при обработката на заявката.
          </div>
        )}
        <div className="text-center">
          <button type="submit" className="btn btn-warning btn-lg">
            <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
            Добавяне
          </button>
        </div>
      </form>
    </div>
  );
}

export default PharmacyAddComponent;
