import { useState, useEffect } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUsers, faCalendarCheck } from "@fortawesome/free-solid-svg-icons";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function AllUsersComponent() {
  const [users, setUsers] = useState<User[]>([]);
  const [error, setError] = useState(false);
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  const getUsers = async () => {
    try {
      const response = await fetch(
        `http://localhost:5196/api/Authentication/GetAllUsers`,
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
        setUsers(data.users);
      } else {
        throw new Error("Възникна проблем при зареждането на потребителите!");
      }
    } catch (error) {
      console.log("Имаше грешка", error);
      setUsers([]);
      setError(true);
    }
  };

  useEffect(() => {
    getUsers();
  }, []);

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  const redirectToBooked = (id: string) => {
    navigate(`/appointments/${id}`);
  };

  return (
    <div className="container mt-4">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <div className="card shadow-sm">
            <div className="card-header bg-info text-white d-flex justify-content-between align-items-center">
              <h4>
                <FontAwesomeIcon icon={faUsers} className="me-2" />
                Потребители
              </h4>
              <span className="badge bg-light text-info">
                {users.length} потребители
              </span>
            </div>
            <div className="card-body p-4">
              {users.length > 0 ? (
                <ul className="list-group">
                  {users.map((user) => (
                    <li
                      className="list-group-item d-flex justify-content-between align-items-center"
                      key={user.id}
                    >
                      <span>
                        {user.fullName} | {user.email}
                      </span>
                      <button
                        className="btn btn-sm btn-primary"
                        onClick={() => redirectToBooked(user.id)}
                      >
                        <FontAwesomeIcon
                          icon={faCalendarCheck}
                          className="me-1"
                        />
                        Записани часове
                      </button>
                    </li>
                  ))}
                </ul>
              ) : (
                <div className="alert alert-warning text-center">
                  Няма намерени потребители
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AllUsersComponent;
