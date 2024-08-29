import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";

interface RecipeDisplayModel {
  id: number;
  patientName?: string;
  doctorName?: string;
}

const RecipeByEgnComponent: React.FC = () => {
  const [egn, setEgn] = useState<string>("");
  const [recipes, setRecipes] = useState<RecipeDisplayModel[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>("");
  const token = localStorage.getItem("token");

  const handleFetchRecipes = async () => {
    setLoading(true);
    setError("");

    try {
      const response = await fetch(
        `http://localhost:5250/api/Recipe/GetRecipies?EGN=${encodeURIComponent(
          egn
        )}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const data = await response.json();
      setRecipes(data.recipes);
    } catch (err) {
      setError("Failed to fetch recipes.");
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = async (recipeId: number) => {
    const url = `http://localhost:5250/api/Recipe/DownloadRecipe?recipeId=${recipeId}`;

    try {
      const response = await fetch(url, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error("Failed to download recipe.");
      }

      const blob = await response.blob();
      const downloadUrl = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = downloadUrl;
      a.download = `recipe_${recipeId}.txt`;
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch (err) {
      setError("Failed to download recipe.");
    }
  };

  return (
    <div className="container mt-5">
      <div className="card shadow-sm">
        <div className="card-body">
          <h1 className="card-title text-center mb-4">Get Recipes by EGN</h1>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              value={egn}
              onChange={(e) => setEgn(e.target.value)}
              placeholder="Enter EGN"
            />
          </div>
          <div className="text-center">
            <button
              className="btn btn-primary"
              onClick={handleFetchRecipes}
              disabled={loading}
            >
              {loading ? "Loading..." : "Fetch Recipes"}
            </button>
          </div>
          {error && <div className="alert alert-danger mt-3">{error}</div>}
        </div>
      </div>
      {recipes.length > 0 && (
        <div className="mt-4">
          <ul className="list-group">
            {recipes.map((recipe) => (
              <li
                key={recipe.id}
                className="list-group-item d-flex justify-content-between align-items-center"
              >
                <div>
                  <p className="mb-1">
                    <strong>Patient Name:</strong> {recipe.patientName}
                  </p>
                  <p className="mb-1">
                    <strong>Doctor Name:</strong> {recipe.doctorName}
                  </p>
                </div>
                <button
                  className="btn btn-outline-primary"
                  onClick={() => handleDownload(recipe.id)}
                >
                  Download
                </button>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default RecipeByEgnComponent;
