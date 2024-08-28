import React, { useState } from "react";

interface RecipeDisplayModel {
  Id: number;
  PatientName?: string;
  DoctorName?: string;
}

const RecipeByEgnComponent: React.FC = () => {
  const [egn, setEgn] = useState<string>("");
  const [recipes, setRecipes] = useState<RecipeDisplayModel[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>("");

  const handleFetchRecipes = async () => {
    setLoading(true);
    setError("");

    try {
      const response = await fetch(
        `http://localhost:5250/api/GetRecipies?EGN=${encodeURIComponent(egn)}`,
        {
          method: "POST",
        }
      );
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const data = await response.json();
      setRecipes(data.Recipes);
    } catch (err) {
      setError("Failed to fetch recipes.");
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = (recipeId: number) => {
    const url = `http://localhost:5250/api/DownloadRecipe?recipeId=${recipeId}`;
    const a = document.createElement("a");
    a.href = url;
    a.download = `recipe_${recipeId}.txt`;
    a.click();
  };

  return (
    <div>
      <h1>Get Recipes by EGN</h1>
      <input
        type="text"
        value={egn}
        onChange={(e) => setEgn(e.target.value)}
        placeholder="Enter EGN"
      />
      <button onClick={handleFetchRecipes} disabled={loading}>
        {loading ? "Loading..." : "Fetch Recipes"}
      </button>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <div>
        {recipes.length > 0 && (
          <ul>
            {recipes.map((recipe) => (
              <li key={recipe.Id}>
                <div>
                  <p>Patient Name: {recipe.PatientName}</p>
                  <p>Doctor Name: {recipe.DoctorName}</p>
                  <button onClick={() => handleDownload(recipe.Id)}>
                    Download
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default RecipeByEgnComponent;
