import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUserPlus,
  faEnvelope,
  faUser,
} from "@fortawesome/free-solid-svg-icons";
import "bootstrap/dist/css/bootstrap.min.css";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function AddReceptionistComponent() {
  const token = localStorage.getItem("token");
  const [users, setUsers] = useState<User[]>([]);
  const [hospitalId, setHospitalId] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [selectedUserId, setSelectedUserId] = useState<string>("");

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
    }
  };

  const getHospitalId = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Hospital/GetDirectorHospitalId`,
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
        setHospitalId(data.hospitalId);
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setHospitalId("");
    }
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    try {
      const form = new FormData();
      form.append("userId", selectedUserId);
      form.append("hospitalId", hospitalId);
      form.append("name", name);

      const response = await fetch(
        `http://localhost:5025/api/Recepcionist/Add`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: form,
        }
      );

      if (response.ok) {
        setName("");
        setSelectedUserId("");
        alert("Receptionist added successfully!");
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
    }
  };

  useEffect(() => {
    getUsersWithNoRoles();
    getHospitalId();
  }, []);

  return (
    <div className="container mt-5">
      <h2 className="text-primary mb-4">
        <FontAwesomeIcon icon={faUserPlus} /> Добавяне на рецепционист
      </h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group mb-3">
          <label htmlFor="name" className="form-label">
            <FontAwesomeIcon icon={faUser} /> Име:
          </label>
          <input
            type="text"
            className="form-control"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Въведи име на рецепционист"
            required
          />
        </div>
        <div className="form-group mb-3">
          <label htmlFor="user" className="form-label">
            <FontAwesomeIcon icon={faEnvelope} /> Избери потребител:
          </label>
          <select
            className="form-control"
            id="user"
            value={selectedUserId}
            onChange={(e) => setSelectedUserId(e.target.value)}
            required
          >
            <option value="">Избери потребител</option>
            {users.map((user) => (
              <option key={user.id} value={user.id}>
                {user.fullName} ({user.email})
              </option>
            ))}
          </select>
        </div>
        <button type="submit" className="btn btn-primary mt-3">
          <FontAwesomeIcon icon={faUserPlus} /> Добавяне
        </button>
      </form>
    </div>
  );
}

export default AddReceptionistComponent;
