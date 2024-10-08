import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faStar,
  faChevronDown,
  faChevronUp,
} from "@fortawesome/free-solid-svg-icons";

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
    <div
      className="d-flex justify-content-center"
      style={{ minHeight: "100vh" }} // Centers the component vertically and horizontally
    >
      <div className="col-md-6 mx-md-3 mb-4">
        <h3 className="mb-4 text-center">Оценки на доктор #{doctorId}</h3>
        {comments.length > 0 ? (
          comments.map((comment) => (
            <div className="card mb-3" key={comment.id}>
              <div className="card-body">
                <div className="d-flex justify-content-between align-items-center">
                  <div className="d-flex align-items-center">
                    <FontAwesomeIcon
                      icon={faStar}
                      className="text-warning me-2"
                    />
                    <span>{comment.rating} от 5</span>
                  </div>
                  <button
                    className="btn btn-outline-primary btn-sm"
                    onClick={() => toggleCommentVisibility(comment.id)}
                  >
                    {visibleCommentId === comment.id ? (
                      <>
                        Скрий коментар{" "}
                        <FontAwesomeIcon icon={faChevronUp} className="ms-1" />
                      </>
                    ) : (
                      <>
                        Виж коментар{" "}
                        <FontAwesomeIcon
                          icon={faChevronDown}
                          className="ms-1"
                        />
                      </>
                    )}
                  </button>
                </div>
                {visibleCommentId === comment.id && (
                  <div className="mt-3">
                    <p className="mb-0">{comment.comment}</p>
                  </div>
                )}
              </div>
            </div>
          ))
        ) : (
          <div className="alert alert-warning text-center">
            Няма намерени оценки
          </div>
        )}
      </div>
    </div>
  );
}

export default DoctorCommentsComponent;
