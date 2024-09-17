import React, { useState } from "react";
import { Form, Button, InputGroup, Alert, Row, Col } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUser,
  faIdCard,
  faUserMd,
  faFileUpload,
} from "@fortawesome/free-solid-svg-icons";

function RecipeAddComponent() {
  const [egn, setEgn] = useState<string>("");
  const [patientName, setPatientName] = useState<string>("");
  const [doctorName, setDoctorName] = useState<string>("");
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0]);
    }
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    if (!selectedFile || !egn || !patientName || !doctorName) {
      setError("Моля, попълнете всички полета и прикачете файл.");
      return;
    }

    const formData = new FormData();
    formData.append("FormFile", selectedFile);
    formData.append("PatientEGN", egn);
    formData.append("PatientName", patientName);
    formData.append("DoctorName", doctorName);

    const token = localStorage.getItem("token");

    try {
      const response = await fetch(
        `http://localhost:5250/api/Recipe/AddRecipe`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );

      if (response.ok) {
        setSuccess(true);
        setEgn("");
        setPatientName("");
        setDoctorName("");
        setSelectedFile(null);
        setError(null);
      } else {
        const errorData = await response.text();
        throw new Error(errorData);
      }
    } catch (error) {
      console.error(error);
      setError("Възникна грешка при добавянето на рецептата.");
    }
  };

  return (
    <div className="dropdown-component">
      {error && <Alert variant="danger">{error}</Alert>}
      {success && (
        <Alert variant="success">Рецептата е добавена успешно!</Alert>
      )}

      <Form onSubmit={handleSubmit}>
        <Row className="align-items-center">
          {/* EGN Input */}
          <Col md={3}>
            <Form.Group controlId="egn">
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faIdCard} />
                </InputGroup.Text>
                <Form.Control
                  type="text"
                  placeholder="ЕГН"
                  value={egn}
                  onChange={(e) => setEgn(e.target.value)}
                  required
                />
              </InputGroup>
            </Form.Group>
          </Col>

          {/* Patient Name Input */}
          <Col md={3}>
            <Form.Group controlId="patientName">
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faUser} />
                </InputGroup.Text>
                <Form.Control
                  type="text"
                  placeholder="Име на пациент"
                  value={patientName}
                  onChange={(e) => setPatientName(e.target.value)}
                  required
                />
              </InputGroup>
            </Form.Group>
          </Col>

          {/* Doctor Name Input */}
          <Col md={3}>
            <Form.Group controlId="doctorName">
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faUserMd} />
                </InputGroup.Text>
                <Form.Control
                  type="text"
                  placeholder="Име на лекар"
                  value={doctorName}
                  onChange={(e) => setDoctorName(e.target.value)}
                  required
                />
              </InputGroup>
            </Form.Group>
          </Col>

          {/* File Upload Input */}
          <Col md={3}>
            <Form.Group controlId="file">
              <InputGroup>
                <InputGroup.Text>
                  <FontAwesomeIcon icon={faFileUpload} />
                </InputGroup.Text>
                <Form.Control
                  type="file"
                  accept=".txt"
                  onChange={handleFileChange}
                  required
                />
              </InputGroup>
            </Form.Group>
          </Col>

          {/* Submit Button */}
          <Col md={12} className="text-end mt-3">
            <Button type="submit" variant="primary">
              <FontAwesomeIcon icon={faFileUpload} /> Изпрати
            </Button>
          </Col>
        </Row>
      </Form>
    </div>
  );
}

export default RecipeAddComponent;
