import React, { useEffect, useState } from "react";
import {
  Form,
  Button,
  Container,
  Alert,
  Col,
  Row,
  InputGroup,
} from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPills,
  faDollarSign,
  faWeight,
  faCamera,
} from "@fortawesome/free-solid-svg-icons";

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
        throw new Error("Възникна грешка при получаване на ID на аптеката.");
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
      setError("ID на аптеката не е налично.");
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
          errorData.message || "Възникна грешка при добавяне на лекарството."
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
    <Container className="mt-5">
      <Row className="justify-content-center">
        <Col md={6}>
          <h2 className="text-center mb-4">
            <FontAwesomeIcon icon={faPills} /> Добавяне на Лекарство
          </h2>
          {error && <Alert variant="danger">{error}</Alert>}
          {success && (
            <Alert variant="success">Лекарството е добавено успешно!</Alert>
          )}
          <Form onSubmit={handleSubmit} className="p-4 shadow rounded bg-light">
            <Form.Group controlId="medicationName">
              <Form.Label>Име на Лекарството</Form.Label>
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faPills} />
                </InputGroup.Text>
                <Form.Control
                  type="text"
                  placeholder="Въведи името на лекарството"
                  value={medicationName}
                  onChange={(e) => setMedicationName(e.target.value)}
                  required
                />
              </InputGroup>
            </Form.Group>

            <Form.Group controlId="medicationQuantity" className="mt-3">
              <Form.Label>Количество</Form.Label>
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faWeight} />
                </InputGroup.Text>
                <Form.Control
                  type="number"
                  placeholder="Въведи количество"
                  value={medicationQuantity}
                  onChange={(e) =>
                    setMedicationQuantity(parseInt(e.target.value))
                  }
                  required
                />
              </InputGroup>
            </Form.Group>

            <Form.Group controlId="medicationPrice" className="mt-3">
              <Form.Label>Цена</Form.Label>
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faDollarSign} />
                </InputGroup.Text>
                <Form.Control
                  type="number"
                  placeholder="Въведи цена"
                  step="0.01"
                  value={medicationPrice}
                  onChange={(e) =>
                    setMedicationPrice(parseFloat(e.target.value))
                  }
                  required
                />
              </InputGroup>
            </Form.Group>

            <Form.Group controlId="image" className="mt-3">
              <Form.Label>Снимка на Лекарството</Form.Label>
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faCamera} />
                </InputGroup.Text>
                <Form.Control
                  type="file"
                  onChange={handleImageChange}
                  required
                />
              </InputGroup>
            </Form.Group>

            <Button variant="primary" type="submit" className="mt-4 w-100">
              <FontAwesomeIcon icon={faPills} /> Добави Лекарство
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
}

export default MedicationAddPage;
