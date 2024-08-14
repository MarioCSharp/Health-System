import { useState, useEffect } from "react";
import { Navigate } from "react-router-dom";

interface Hospital {
  id: number;
  hospitalName: string;
}

interface User {
  id: string;
  fullName: string;
  email: string;
}

function AdminHomePage() {
  const [hospitals, setHospitals] = useState<Hospital[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [error, setError] = useState(false);

  const getHospitals = async () => {
    try {
      const response = await fetch(`http://localhost:5166/api/Hospital/All`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (response.ok) {
        const data = await response.json();
        setHospitals(data.hospitals);
      } else {
        throw new Error(
          "You are either not authorized or there is a problem in the system!"
        );
      }
    } catch (error) {
      console.log("There was an error", error);
      setHospitals([]);
      setError(true);
    }
  };

  const getUsers = async () => {
    try {
      const response = await fetch(
        "http://localhost:5166/api/Authentication/GetAllUsers",
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        setUsers(data.users);
      } else {
        throw new Error("There was an error getting all the users!");
      }
    } catch (error) {
      console.log("There was an error", error);
      setUsers([]);
      setError(true);
    }
  };

  useEffect(() => {
    getHospitals();
    getUsers();
  }, []);

  if (error) {
    return <Navigate to="not-found" replace />;
  }

  return (
    <>
      <div className="container">
        <div className="row">
          <div className="col-md-4 mx-md-3 mb-4">
            <ul className="list-group">
              <h3>Клиники</h3>
              {hospitals.length > 0 ? (
                hospitals.map((hospital) => (
                  <li
                    className="list-group-item d-flex justify-content-between align-items-center"
                    key={hospital.id}
                  >
                    <span>{hospital.hospitalName}</span>
                    <div>
                      <a
                        className="btn btn-warning btn-sm mr-2"
                        style={{ marginRight: "2px" }}
                      >
                        Редактирай
                      </a>
                      <a className="btn btn-danger btn-sm">Изтрий</a>
                    </div>
                  </li>
                ))
              ) : (
                <div className="col-12">
                  <div className="card mb-3">
                    <div className="card-body p-2">No hospitals found</div>
                  </div>
                </div>
              )}
              <li className="list-group-item">
                <a href="#">Виж всички</a>
              </li>
            </ul>
          </div>

          <div className="col-md-6 mx-md-3 mb-4">
            <ul className="list-group">
              <h3>Потребители</h3>
              {users.length > 0 ? (
                users.map((user) => (
                  <li
                    className="list-group-item d-flex justify-content-between align-items-center"
                    key={user.id}
                  >
                    <span>
                      {user.fullName} | {user.email}
                    </span>
                    <div>
                      <a
                        className="btn btn-warning btn-sm mr-2"
                        style={{ marginRight: "2px" }}
                      >
                        Редактирай
                      </a>
                      <a className="btn btn-danger btn-sm">Изтрий</a>
                    </div>
                  </li>
                ))
              ) : (
                <div className="col-12">
                  <div className="card mb-3">
                    <div className="card-body p-2">No users found</div>
                  </div>
                </div>
              )}
              <li className="list-group-item">
                <a href="#">Виж всички</a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </>
  );
}

export default AdminHomePage;
