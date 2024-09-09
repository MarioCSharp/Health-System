import { useState, useEffect } from "react";
import { Navigate, useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faClock, faUsers } from "@fortawesome/free-solid-svg-icons";
import { Spinner } from "react-bootstrap";

interface User {
  id: string;
  fullName: string;
  email: string;
}

function UsersList() {
  const [users, setUsers] = useState<User[]>([]);
  const [error, setError] = useState(false);
  const [loading, setLoading] = useState(true);
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
        setUsers(data.users.slice(0, 5));
      } else {
        throw new Error("There was an error getting all the users!");
      }
    } catch (error) {
      console.log("There was an error", error);
      setUsers([]);
      setError(true);
    } finally {
      setLoading(false);
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

  const redirectToAllUsers = () => {
    navigate(`/users`);
  };

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <div className="card shadow-lg rounded-3 border-0">
        <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
          <h3 className="mb-0">
            <FontAwesomeIcon icon={faUsers} /> Потребители
          </h3>
        </div>
        <div className="card-body">
          {loading ? (
            <div className="text-center">
              <Spinner animation="border" role="status">
                <span className="visually-hidden">Loading...</span>
              </Spinner>
            </div>
          ) : users.length > 0 ? (
            <ul className="list-group list-group-flush">
              {users.map((user) => (
                <li
                  className="list-group-item d-flex justify-content-between align-items-center"
                  key={user.id}
                >
                  <div className="d-flex align-items-center">
                    <FontAwesomeIcon
                      icon={faUser}
                      className="text-primary me-3"
                    />
                    <span>
                      <strong>{user.fullName}</strong>
                      <br />
                      <small className="text-muted">{user.email}</small>
                    </span>
                  </div>
                  <button
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => redirectToBooked(user.id)}
                  >
                    <FontAwesomeIcon icon={faClock} /> Записани часове
                  </button>
                </li>
              ))}
            </ul>
          ) : (
            <div className="alert alert-warning" role="alert">
              No users found.
            </div>
          )}
        </div>
        <div className="card-footer text-center">
          <button
            className="btn btn-outline-info"
            onClick={() => redirectToAllUsers()}
          >
            <FontAwesomeIcon icon={faUsers} /> Виж всички
          </button>
        </div>
      </div>
    </div>
  );
}

export default UsersList;
