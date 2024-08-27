import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

interface Comment {
  id: number;
  rating: number;
  comment: string;
}

function DoctorCommentsComponent() {
  const { doctorId } = useParams<{ doctorId: string }>();
  const [error, setError] = useState<boolean>(false);
  const [comments, setComments] = useState<Comment[]>([]);
  const [visibleCommentId, setVisibleCommentId] = useState<number | null>(null);
  const token = localStorage.getItem("token");

  const getComments = async () => {
    try {
      const response = await fetch(
        `http://localhost:5025/api/Doctor/GetDoctorRatings?doctorId=${doctorId}`,
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
        setComments(data.ratings);
      } else {
        throw new Error("There was an error loading the ratings!");
      }
    } catch (error) {
      console.log("Error!", error);
      setError(true);
      setComments([]);
    }
  };

  useEffect(() => {
    getComments();
  }, []);

  const toggleCommentVisibility = (id: number) => {
    setVisibleCommentId((prevId) => (prevId === id ? null : id));
  };

  return (
    <div className="col-md-4 mx-md-3 mb-4">
      <ul className="list-group">
        <h3>Оценки на доктор #{doctorId}</h3>
        {comments.length > 0 ? (
          comments.map((comment) => (
            <li className="list-group-item" key={comment.id}>
              <div className="d-flex justify-content-between align-items-center">
                <span>{comment.rating} от 5</span>
                <button
                  className="btn btn-primary btn-sm"
                  onClick={() => toggleCommentVisibility(comment.id)}
                >
                  Виж коментар
                </button>
              </div>
              {visibleCommentId === comment.id && (
                <div className="mt-2">
                  <p className="mb-0">{comment.comment}</p>
                </div>
              )}
            </li>
          ))
        ) : (
          <div className="col-12">
            <div className="card mb-3">
              <div className="card-body p-2">No ratings found</div>
            </div>
          </div>
        )}
      </ul>
    </div>
  );
}

export default DoctorCommentsComponent;
