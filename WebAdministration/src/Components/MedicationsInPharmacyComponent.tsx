import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface MedicationDisplayModel {
  id: number;
  name: string | null;
  price: number;
  image: Uint8Array | null;
  quantity: number;
}

interface MedicationEditModel {
  id: number;
  name: string;
  price: number;
  quantity: number;
}

function MedicationsInPharmacyComponent() {
  const [medications, setMedications] = useState<MedicationDisplayModel[]>([]);
  const [editMedication, setEditMedication] =
    useState<MedicationEditModel | null>(null);
  const [addQuantityValues, setAddQuantityValues] = useState<{
    [key: number]: number;
  }>({});
  const navigate = useNavigate();

  const token = localStorage.getItem("token");

  const getMedications = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Medication/GetMyMedications`,
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
        setMedications(data);
      } else {
        throw new Error("Error loading the medications!");
      }
    } catch (error) {
      alert(error);
    }
  };

  useEffect(() => {
    getMedications();
  }, []);

  const renderImage = (image: Uint8Array | null) => {
    if (image) {
      const base64String = btoa(String.fromCharCode(...new Uint8Array(image)));
      return (
        <img src={`data:image/jpeg;base64,${base64String}`} alt="Medication" />
      );
    } else {
      return <span>No Image Available</span>;
    }
  };

  const handleEdit = (medication: MedicationDisplayModel) => {
    setEditMedication({
      id: medication.id,
      name: medication.name || "",
      price: medication.price,
      quantity: medication.quantity,
    });
  };

  const handleDelete = async (medicationId: number) => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Medication/Delete?medicationId=${medicationId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!response.ok) {
        throw new Error("There was an error deleting the pharmacy");
      }
      getMedications();
    } catch (error) {
      console.error("Error deleting pharmacy:", error);
      alert("Failed to delete pharmacy. Please try again.");
    }
  };

  const handleAddQuantity = async (medicationId: number) => {
    const quantityToAdd = addQuantityValues[medicationId];
    if (!quantityToAdd || quantityToAdd <= 0) {
      alert("Please enter a valid quantity.");
      return;
    }

    try {
      const response = await fetch(
        `http://localhost:5171/api/Medication/AddQuantity?medicationId=${medicationId}&quantity=${quantityToAdd}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        getMedications();
        setAddQuantityValues((prev) => ({ ...prev, [medicationId]: 0 }));
      } else {
        alert("Failed to add quantity. Please try again.");
      }
    } catch (error) {
      console.error("Error adding quantity:", error);
      alert("Failed to add quantity. Please try again.");
    }
  };

  const handleAddNew = () => {
    navigate(`/medication/add`);
  };

  const handleQuantityChange = (medicationId: number, value: number) => {
    setAddQuantityValues((prev) => ({ ...prev, [medicationId]: value }));
  };

  const handleEditChange = (
    field: keyof MedicationEditModel,
    value: string | number
  ) => {
    if (editMedication) {
      setEditMedication((prev) => ({
        ...prev!,
        [field]: value,
      }));
    }
  };

  const handleSaveEdit = async () => {
    if (!editMedication) return;

    try {
      const formData = new FormData();
      formData.append("id", editMedication.id.toString());
      formData.append("name", editMedication.name);
      formData.append("price", editMedication.price.toString());
      formData.append("quantity", editMedication.quantity.toString());

      const response = await fetch(
        `http://localhost:5171/api/Medication/Edit`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );

      if (response.ok) {
        getMedications();
        setEditMedication(null);
      } else {
        alert("Failed to save changes. Please try again.");
      }
    } catch (error) {
      console.error("Error saving changes:", error);
      alert("Failed to save changes. Please try again.");
    }
  };

  const handleCancelEdit = () => {
    setEditMedication(null);
  };

  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4">Medications List</h1>
      <div className="d-flex justify-content-end mb-3">
        <button className="btn btn-primary" onClick={handleAddNew}>
          Добави ново лекарство
        </button>
      </div>
      <table className="table table-striped table-bordered">
        <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">Наименование</th>
            <th scope="col">Цена</th>
            <th scope="col">Количество</th>
            <th scope="col">Снимка</th>
            <th scope="col">Действия</th>
            <th scope="col">Добави количество</th>
          </tr>
        </thead>
        <tbody>
          {medications.map((medication) => (
            <React.Fragment key={medication.id}>
              <tr>
                <th scope="row">{medication.id}</th>
                <td>{medication.name}</td>
                <td>${medication.price.toFixed(2)}</td>
                <td>{medication.quantity}</td>
                <td>{renderImage(medication.image)}</td>
                <td>
                  <button
                    className="btn btn-warning btn-sm me-2"
                    onClick={() => handleEdit(medication)}
                  >
                    Редактирай
                  </button>
                  <button
                    className="btn btn-danger btn-sm me-2"
                    onClick={() => handleDelete(medication.id)}
                  >
                    Изтрий
                  </button>
                </td>
                <td>
                  <input
                    type="number"
                    min="0"
                    className="form-control me-2 d-inline"
                    style={{ width: "100px" }}
                    value={addQuantityValues[medication.id] || 0}
                    onChange={(e) =>
                      handleQuantityChange(
                        medication.id,
                        parseInt(e.target.value)
                      )
                    }
                  />
                  <button
                    className="btn btn-success btn-sm"
                    onClick={() => handleAddQuantity(medication.id)}
                  >
                    Добави количество
                  </button>
                </td>
              </tr>
              {editMedication && editMedication.id === medication.id && (
                <tr>
                  <td colSpan={7}>
                    <div className="p-3 bg-light">
                      <h5>Edit Medication</h5>
                      <div className="mb-3">
                        <label
                          htmlFor={`name-${medication.id}`}
                          className="form-label"
                        >
                          Наименование
                        </label>
                        <input
                          type="text"
                          className="form-control"
                          id={`name-${medication.id}`}
                          value={editMedication.name}
                          onChange={(e) =>
                            handleEditChange("name", e.target.value)
                          }
                        />
                      </div>
                      <div className="mb-3">
                        <label
                          htmlFor={`price-${medication.id}`}
                          className="form-label"
                        >
                          Цена
                        </label>
                        <input
                          type="number"
                          className="form-control"
                          id={`price-${medication.id}`}
                          value={editMedication.price}
                          onChange={(e) =>
                            handleEditChange(
                              "price",
                              parseFloat(e.target.value)
                            )
                          }
                        />
                      </div>
                      <div className="mb-3">
                        <label
                          htmlFor={`quantity-${medication.id}`}
                          className="form-label"
                        >
                          Количество
                        </label>
                        <input
                          type="number"
                          className="form-control"
                          id={`quantity-${medication.id}`}
                          value={editMedication.quantity}
                          onChange={(e) =>
                            handleEditChange(
                              "quantity",
                              parseInt(e.target.value)
                            )
                          }
                        />
                      </div>
                      <button
                        className="btn btn-success me-2"
                        onClick={handleSaveEdit}
                      >
                        Запази промените
                      </button>
                      <button
                        className="btn btn-secondary"
                        onClick={handleCancelEdit}
                      >
                        Откажи
                      </button>
                    </div>
                  </td>
                </tr>
              )}
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default MedicationsInPharmacyComponent;
