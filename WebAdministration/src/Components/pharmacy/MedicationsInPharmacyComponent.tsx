import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

interface MedicationDisplayModel {
  id: number;
  name: string | null;
  price: number;
  image: string | null;
  quantity: number;
}

interface MedicationEditModel {
  id: number;
  name: string;
  price: number;
  quantity: number;
  image?: File | null;
}

function MedicationsInPharmacyComponent() {
  const [medications, setMedications] = useState<MedicationDisplayModel[]>([]);
  const [editMedication, setEditMedication] =
    useState<MedicationEditModel | null>(null);
  const [addQuantityValues, setAddQuantityValues] = useState<{
    [key: number]: number;
  }>({});
  const [imageFile, setImageFile] = useState<File | null>(null); // State for the selected image file
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

        console.log(data);
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

  const renderImage = (image: string | null) => {
    const imageStyle: React.CSSProperties = {
      width: "100px",
      height: "100px",
      objectFit: "cover" as React.CSSProperties["objectFit"], // Correctly type the objectFit property
    };

    if (image) {
      return (
        <img
          src={`data:image/jpeg;base64,${image}`}
          alt="Medication"
          style={imageStyle}
        />
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
      image: null,
    });
    setImageFile(null);
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
        throw new Error("There was an error deleting the medication");
      }
      getMedications();
    } catch (error) {
      console.error("Error deleting medication:", error);
      alert("Failed to delete medication. Please try again.");
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
    navigate("/medication/add");
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

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setImageFile(event.target.files[0]); // Store the selected image file
    }
  };

  const handleSaveEdit = async () => {
    if (!editMedication) return;

    try {
      const formData = new FormData();
      formData.append("Id", editMedication.id.toString());
      formData.append("Name", editMedication.name);
      formData.append("Price", editMedication.price.toString());
      formData.append("Quantity", editMedication.quantity.toString());
      if (imageFile) {
        formData.append("Image", imageFile); // Append the image file if available
      }

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
        setImageFile(null);
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
    setImageFile(null); // Reset the image file on cancel
  };

  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4">Лекарства</h1>
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
                <td>{medication.price.toFixed(2)} лв</td>
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
                      <div className="mb-3">
                        <label
                          htmlFor={`image-${medication.id}`}
                          className="form-label"
                        >
                          Изображение
                        </label>
                        <input
                          type="file"
                          className="form-control"
                          id={`image-${medication.id}`}
                          accept="image/*"
                          onChange={handleImageChange}
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
