import React, { useEffect, useState } from "react";
import { Form, Button, Container, Alert } from "react-bootstrap";

function MedicationAddPage() {
  const [pharmacyId, setPharmacyId] = useState<number | null>(null);
  const [medicationName, setMedicationName] = useState("");
  const [medicationQuantity, setMedicationQuantity] = useState(0);
  const [medicationPrice, setMedicationPrice] = useState(0.0);
  const [image, setImage] = useState<File | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);

  const token = localStorage.getItem("token");

  const getPharmacyId = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacy/GetMyPharmacyId`,
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
        setPharmacyId(data);
      } else {
        throw new Error("There was an error retrieving the pharmacy ID.");
      }
    } catch (error: any) {
      setError(error.message);
    }
  };

  useEffect(() => {
    getPharmacyId();
  }, []);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    if (!pharmacyId) {
      setError("Pharmacy ID is not available.");
      return;
    }

    const formData = new FormData();
    formData.append("MedicationName", medicationName);
    formData.append("MedicationQuantity", medicationQuantity.toString());
    formData.append("MedicationPrice", medicationPrice.toString());
    if (image) formData.append("Image", image);
    formData.append("PharmacyId", pharmacyId.toString());

    try {
      const response = await fetch("http://localhost:5171/api/Medication/Add", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      });

      if (response.ok) {
        setSuccess(true);
        setMedicationName("");
        setMedicationQuantity(0);
        setMedicationPrice(0.0);
        setImage(null);
      } else {
        const errorData = await response.json();
        setError(
          errorData.message || "There was an error adding the medication."
        );
      }
    } catch (error: any) {
      setError(error.message);
    }
  };

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setImage(file);
  };

  return (
    <Container>
      <h2>Add Medication</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      {success && (
        <Alert variant="success">Medication added successfully!</Alert>
      )}
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="medicationName">
          <Form.Label>Medication Name</Form.Label>
          <Form.Control
            type="text"
            placeholder="Enter medication name"
            value={medicationName}
            onChange={(e) => setMedicationName(e.target.value)}
            required
          />
        </Form.Group>

        <Form.Group controlId="medicationQuantity">
          <Form.Label>Medication Quantity</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter medication quantity"
            value={medicationQuantity}
            onChange={(e) => setMedicationQuantity(parseInt(e.target.value))}
            required
          />
        </Form.Group>

        <Form.Group controlId="medicationPrice">
          <Form.Label>Medication Price</Form.Label>
          <Form.Control
            type="number"
            placeholder="Enter medication price"
            step="0.01"
            value={medicationPrice}
            onChange={(e) => setMedicationPrice(parseFloat(e.target.value))}
            required
          />
        </Form.Group>

        <Form.Group controlId="image">
          <Form.Label>Medication Image</Form.Label>
          <Form.Control type="file" onChange={handleImageChange} required />
        </Form.Group>

        <Button variant="primary" type="submit" className="mt-3">
          Add Medication
        </Button>
      </Form>
    </Container>
  );
}

export default MedicationAddPage;
