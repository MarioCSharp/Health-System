import React, { useEffect, useState } from "react";

interface Recepcionist {
  id: number;
  name: string;
}

function AllMyRecepcionistsPage() {
  const [recepcionists, setRecepcionists] = useState<Recepcionist[]>([]);
  const token = localStorage.getItem("token");

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

  return (
    <div className="col-md-6 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Рецепционисти</h3>
        {recepcionists.length > 0 ? (
          recepcionists.map((recepcionist) => (
            <li
              className="list-group-item d-flex justify-content-between align-items-center"
              key={recepcionist.id}
            >
              <span>{recepcionist.name}</span>
              <div>
                <a
                  className="btn btn-danger btn-sm mr-2"
                  style={{ marginRight: "2px" }}
                  onClick={() => handleDelete(recepcionist.id)}
                >
                  Изтрий
                </a>
              </div>
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">Няма намерени рецепционисти</div>
            </div>
          </div>
        )}
      </ul>
    </div>
  );
}

export default AllMyRecepcionistsPage;
