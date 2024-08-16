import { useEffect, useState } from "react";

interface PrescriptionComponentProps {
  appointmentId: string;
}

function PrescriptionComponent({ appointmentId }: PrescriptionComponentProps) {
  const [fullName, setFullName] = useState("");
  const [dob, setDob] = useState("");
  const [egn, setEgn] = useState("");
  const [address, setAddress] = useState("");
  const [complaints, setComplaints] = useState("");
  const [diagnosis, setDiagnosis] = useState("");
  const [conditions, setConditions] = useState("");
  const [status, setStatus] = useState("");
  const [therapy, setTherapy] = useState("");
  const [tests, setTests] = useState("");
  const [doctorName, setDoctorName] = useState("");
  const [uin, setUin] = useState("");
  const [downloadLink, setDownloadLink] = useState<string | null>(null);

  const [hasPrescription, setHasPrescription] = useState<boolean>(false);

  const token = localStorage.getItem("token");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("FullName", fullName);
    formData.append("DateOfBirth", dob);
    formData.append("EGN", egn);
    formData.append("Address", address);
    formData.append("Complaints", complaints);
    formData.append("Diagnosis", diagnosis);
    formData.append("Conditions", conditions);
    formData.append("Status", status);
    formData.append("Therapy", therapy);
    formData.append("Tests", tests);
    formData.append("DoctorName", doctorName);
    formData.append("UIN", uin);
    formData.append("AppointmentId", appointmentId);
    if (token) {
      formData.append("Token", token);
    }

    try {
      const response = await fetch(
        "http://localhost:5166/api/Appointment/IssuePrescription",
        {
          method: "POST",
          body: formData,
        }
      );

      if (response.ok) {
        console.log("Response OK, generating download link...");

        const fileBlob = await response.blob();
        const url = window.URL.createObjectURL(fileBlob);

        console.log("Generated download link: ", url);
        setDownloadLink(url);
        setHasPrescription(true);
      } else {
        console.error(
          "Failed to issue prescription, status code: ",
          response.status
        );
        const errorText = await response.text();
        console.error("Error details: ", errorText);
      }
    } catch (error) {
      console.error("An error occurred while issuing the prescription", error);
    }
  };

  const checkIfAppointmentHasPrescription = async () => {
    try {
      const response = await fetch(
        `http://localhost:5166/api/Appointment/HasPrescription?token=${token!}&appointmentId=${appointmentId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        const fileBlob = await response.blob();
        const url = window.URL.createObjectURL(fileBlob);

        console.log("Generated download link: ", url);
        setDownloadLink(url);
        setHasPrescription(true);
      } else {
        throw new Error("There was an error loading this appointment");
      }
    } catch (error) {
      console.error("An error occurred while checking the prescription", error);
      setHasPrescription(false);
      setDownloadLink(null);
    }
  };

  useEffect(() => {
    checkIfAppointmentHasPrescription();
  }, []);

  return (
    <div className="mt-3">
      {hasPrescription === true ? (
        <>
          {downloadLink && (
            <a
              href={downloadLink}
              download={`Prescription_${appointmentId}.pdf`}
              className="btn btn-success mt-3"
            >
              Изтегли амбулаторен лист
            </a>
          )}
          <a className="btn btn-danger mt-3">Изтрий</a>
        </>
      ) : (
        <>
          <h5>Амбулаторен лист за преглед #{appointmentId}</h5>
          <form onSubmit={handleSubmit}>
            <div className="row">
              <div className="col-md-6 form-group">
                <label htmlFor="fullName">Име:</label>
                <input
                  type="text"
                  id="fullName"
                  className="form-control"
                  value={fullName}
                  onChange={(e) => setFullName(e.target.value)}
                />
              </div>
              <div className="col-md-6 form-group">
                <label htmlFor="dob">Дата на раждане:</label>
                <input
                  type="date"
                  id="dob"
                  className="form-control"
                  value={dob}
                  onChange={(e) => setDob(e.target.value)}
                />
              </div>
            </div>

            <div className="row">
              <div className="col-md-6 form-group">
                <label htmlFor="egn">ЕГН:</label>
                <input
                  type="text"
                  id="egn"
                  className="form-control"
                  value={egn}
                  onChange={(e) => setEgn(e.target.value)}
                />
              </div>
              <div className="col-md-6 form-group">
                <label htmlFor="address">Адрес:</label>
                <input
                  type="text"
                  id="address"
                  className="form-control"
                  value={address}
                  onChange={(e) => setAddress(e.target.value)}
                />
              </div>
            </div>

            <div className="row">
              <div className="col-md-6 form-group">
                <label htmlFor="complaints">Анамнеза:</label>
                <textarea
                  id="complaints"
                  className="form-control"
                  rows={3}
                  value={complaints}
                  onChange={(e) => setComplaints(e.target.value)}
                ></textarea>
              </div>
              <div className="col-md-6 form-group">
                <label htmlFor="diagnosis">Диагноза:</label>
                <textarea
                  id="diagnosis"
                  className="form-control"
                  rows={3}
                  value={diagnosis}
                  onChange={(e) => setDiagnosis(e.target.value)}
                ></textarea>
              </div>
            </div>

            <div className="row">
              <div className="col-md-6 form-group">
                <label htmlFor="conditions">Придружаващи заболявания:</label>
                <input
                  type="text"
                  id="conditions"
                  className="form-control"
                  value={conditions}
                  onChange={(e) => setConditions(e.target.value)}
                />
              </div>
              <div className="col-md-6 form-group">
                <label htmlFor="status">Обективно състояние:</label>
                <input
                  type="text"
                  id="status"
                  className="form-control"
                  value={status}
                  onChange={(e) => setStatus(e.target.value)}
                />
              </div>
            </div>

            <div className="form-group">
              <label htmlFor="therapy">Назначена терапия:</label>
              <textarea
                id="therapy"
                className="form-control"
                rows={4}
                value={therapy}
                onChange={(e) => setTherapy(e.target.value)}
              ></textarea>
            </div>

            <div className="form-group">
              <label htmlFor="tests">Назначени изследвания:</label>
              <textarea
                id="tests"
                className="form-control"
                rows={4}
                value={tests}
                onChange={(e) => setTests(e.target.value)}
              ></textarea>
            </div>

            <div className="form-group">
              <label htmlFor="doctorName">Лекар:</label>
              <input
                type="text"
                id="doctorName"
                className="form-control"
                value={doctorName}
                onChange={(e) => setDoctorName(e.target.value)}
              />
            </div>
            <div className="form-group">
              <label htmlFor="uin">УИН:</label>
              <input
                type="text"
                id="uin"
                className="form-control"
                value={uin}
                onChange={(e) => setUin(e.target.value)}
              />
            </div>

            <button type="submit" className="btn btn-primary mt-2">
              Издаване
            </button>
          </form>
        </>
      )}
    </div>
  );
}

export default PrescriptionComponent;
