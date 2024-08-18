import { useState } from "react";

interface Props {
  appointmentId: string;
}

function FeedbackComponent({ appointmentId }: Props) {
  const [comment, setComment] = useState<string>("");
  const [message, setMessage] = useState<boolean>(false);
  const [error, setError] = useState<boolean>(false);

  const token = localStorage.getItem("token");

  const handleSubmit = async () => {
    try {
      const formData = new FormData();
      formData.append("Comment", comment);
      formData.append("AppointmentId", appointmentId);
      formData.append("Token", token!);

      const response = await fetch(
        `http://localhost:5046/api/Appointment/AddComment`,
        {
          method: "POST",
          body: formData,
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();

        if (data.success) {
          setMessage(true);
          setComment("");
        } else {
          throw new Error("There was an error adding your comment");
        }
      }
    } catch (error) {
      setError(true);
      console.log("Error!", error);
    }
  };

  return (
    <>
      <div className="mt-3">
        <h5>Коментар за час #{appointmentId}</h5>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="prescription">Коментар:</label>
            <textarea
              id="comment"
              className="form-control"
              rows={3}
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              required
            ></textarea>
          </div>
          {message && (
            <p className="text-success">Добавихте коментар успешно!</p>
          )}
          <button type="submit" className="btn btn-primary mt-2">
            Изпрати
          </button>
        </form>
      </div>
    </>
  );
}

export default FeedbackComponent;
