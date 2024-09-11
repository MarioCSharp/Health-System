import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function DoctorAddComponent() {
  const { hospitalId } = useParams<{ hospitalId: string }>();
  const [users, setUsers] = useState<User[]>([]);
  const [specialization, setSpecialization] = useState<string>("");
  const [about, setAbout] = useState<string>("");
  const [contactNumber, setContactNumber] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [fullName, setFullName] = useState<string>("");
  const [selectedUserId, setSelectedUserId] = useState<string>("");
  const [error, setError] = useState(false);

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
    e.preventDefault();
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/Add?specialization=${specialization}&userId=${selectedUserId}&about=${about}&contactNumber=${contactNumber}&email=${email}&fullName=${fullName}&hospitalId=${hospitalId}`,
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
        if (!data.success) {
          throw new Error(
            "You are either not authorized or there is a problem in the system!"
          );
        }
        navigate(`/doctors/${hospitalId}`);
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

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <div className="card shadow-sm">
            <div className="card-header bg-primary text-white">
              <h3 className="mb-0">Добавяне на доктор</h3>
            </div>
            <div className="card-body">
              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label htmlFor="userSelect" className="form-label">
                    Избери потребител
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
                  <label htmlFor="fullName" className="form-label">
                    Име на доктора
                  </label>
                  <input
                    type="text"
                    className="form-control"
                    id="fullName"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="email" className="form-label">
                    Email на доктора
                  </label>
                  <input
                    type="email"
                    className="form-control"
                    id="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="contactNumber" className="form-label">
                    Телефон за връзка на доктора
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
                  <label htmlFor="specialization" className="form-label">
                    Специализация на доктора
                  </label>
                  <input
                    type="text"
                    className="form-control"
                    id="specialization"
                    value={specialization}
                    onChange={(e) => setSpecialization(e.target.value)}
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="about" className="form-label">
                    Повече информация за доктора
                  </label>
                  <textarea
                    className="form-control"
                    id="about"
                    rows={3}
                    value={about}
                    onChange={(e) => setAbout(e.target.value)}
                    required
                  />
                </div>
                {error && (
                  <div className="alert alert-danger" role="alert">
                    Възникна грешка. Моля, опитайте отново.
                  </div>
                )}
                <div className="d-flex justify-content-end">
                  <button type="submit" className="btn btn-primary">
                    Запази промените
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default DoctorAddComponent;
