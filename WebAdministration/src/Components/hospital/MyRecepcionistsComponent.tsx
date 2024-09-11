import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash, faPlus, faList } from "@fortawesome/free-solid-svg-icons";

interface Recepcionist {
  id: number;
  name: string;
}

function MyRecepcionistsComponent() {
  const [recepcionists, setRecepcionists] = useState<Recepcionist[]>([]);
  const token = localStorage.getItem("token");

  const navigate = useNavigate();

  const getRecepcionists = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Recepcionist/GetMyRecepcionists`,
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
        setRecepcionists(data.recepcionists);
      } else {
        throw new Error("Failed loading the recepcionists!");
      }
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    getRecepcionists();
  }, []);

  const handleDelete = async (recepcionistId: number) => {
    try {
      const deleteResponse = await fetch(
        `http://localhost:5025/api/Recepcionist/Delete?id=${recepcionistId}`,
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (deleteResponse.ok) {
        getRecepcionists();
      } else {
        throw new Error("There was an error deleting the recepcionist.");
      }
    } catch (error) {
      console.log(error);
    }
  };

  const redirectToAllRecepcionists = async () => {
    navigate(`/recepcionists/all`);
  };

  const redirectToAddRecepcionists = async () => {
    navigate(`/recepcionists/add`);
  };

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <h3 className="mb-4">Рецепционисти</h3>
      {recepcionists.length > 0 ? (
        recepcionists.map((recepcionist) => (
          <div className="card mb-3" key={recepcionist.id}>
            <div className="card-body d-flex justify-content-between align-items-center">
              <h5 className="mb-0">{recepcionist.name}</h5>
              <button
                className="btn btn-outline-danger btn-sm"
                onClick={() => handleDelete(recepcionist.id)}
              >
                <FontAwesomeIcon icon={faTrash} className="me-1" />
                Изтрий
              </button>
            </div>
          </div>
        ))
      ) : (
        <div className="alert alert-warning text-center">
          Няма намерени рецепционисти
        </div>
      )}

      <div className="d-flex justify-content-between mt-3">
        <button
          className="btn btn-primary"
          onClick={redirectToAddRecepcionists}
        >
          <FontAwesomeIcon icon={faPlus} className="me-1" />
          Добави рецепционист
        </button>
      </div>
    </div>
  );
}

export default MyRecepcionistsComponent;
