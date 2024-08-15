import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function HospitalAddComponent() {
  const [hospitalName, setHospitalName] = useState<string>("");
  const [hospitalLocation, setHospitalLocation] = useState<string>("");
  const [hospitalContactNumber, setHospitalContactNumber] =
    useState<string>("");
  const [selectedUserId, setSelectedUserId] = useState<string>("");
  const [users, setUsers] = useState<User[]>([]);

  const token = localStorage.getItem("token");
  const [error, setError] = useState(false);
  const navigate = useNavigate();

  const getUsersWithNoRoles = async () => {
    try {
      const response = await fetch(
        `http://localhost:5166/api/Account/GetAccountsWithNoRoles?token=${token}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
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

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const formData = new FormData();
    formData.append("HospitalName", hospitalName);
    formData.append("Location", hospitalLocation);
    formData.append("ContactNumber", hospitalContactNumber);
    formData.append("OwnerId", selectedUserId);
    formData.append("Token", token!);

    try {
      const response = await fetch(`http://localhost:5166/api/Hospital/Add`, {
        method: "POST",
        body: formData,
      });

      if (response.ok) {
        const data = await response.json();

        if (!data.success) {
          throw new Error(
            "You are either not authorized or there is a problem in the system!"
          );
        }

        navigate("/");
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setError(true);
    }
  };

  useEffect(() => {
    getUsersWithNoRoles();
  }, []);

  return (
    <div className="container">
      <h2>Добавяне на болница</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="userSelect" className="form-label">
            Избери управител на болницата
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
          <label htmlFor="hospitalName" className="form-label">
            Име на болницата
          </label>
          <input
            type="text"
            className="form-control"
            id="hospitalName"
            value={hospitalName}
            onChange={(e) => setHospitalName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="hospitalContactNumber" className="form-label">
            Номер за връзка
          </label>
          <input
            type="text"
            className="form-control"
            id="hospitalContactNumber"
            value={hospitalContactNumber}
            onChange={(e) => setHospitalContactNumber(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="hospitalLocation" className="form-label">
            Локация на болницата
          </label>
          <input
            type="text"
            className="form-control"
            id="hospitalLocation"
            value={hospitalLocation}
            onChange={(e) => setHospitalLocation(e.target.value)}
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

export default HospitalAddComponent;
