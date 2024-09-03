import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function PharmacistAddComponent() {
  const { pharmacyId } = useParams<{ pharmacyId: string }>();
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUserId, setSelectedUserId] = useState<string>();
  const [error, setError] = useState(false);

  const [name, setName] = useState<string>("");
  const [email, setEmail] = useState<string>("");

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

  const handleSubmit = async () => {
    const form = new FormData();
    form.append("PharmacyId", pharmacyId!);
    form.append("Name", name);
    form.append("Email", email);
    form.append("UserId", selectedUserId!);

    try {
      const response = await fetch(`http://localhost:5171/api/Pharmacist/Add`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: form,
      });

      if (response.ok) {
        navigate(`/`);
      } else {
        throw new Error("There was an error adding the pharmacist.");
      }
    } catch (error) {
      alert(error);
    }
  };

  return (
    <div className="container">
      <h2>Добавяне на фармацевт</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="userSelect" className="form-label">
            Избери фармацевт
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
          <label htmlFor="name" className="form-label">
            Име на фармацевт
          </label>
          <input
            type="text"
            className="form-control"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="email" className="form-label">
            Имейл
          </label>
          <input
            type="text"
            className="form-control"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
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

export default PharmacistAddComponent;
