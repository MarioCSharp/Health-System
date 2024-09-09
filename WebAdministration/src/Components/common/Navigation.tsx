import { Navigate } from "react-router-dom";
import { useEffect, useState } from "react";

function Navigation() {
  const [role, setRole] = useState<string>("");

  useEffect(() => {
    const userRole = localStorage.getItem("role");
    setRole(userRole!);
  }, []);

  const logout = () => {
    const token = localStorage.getItem("token");

    if (token) {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      window.location.href = "/login";
    }
  };

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
        <div className="container-fluid">
          <a className="navbar-brand" href="/">
            MedCare Администрация
          </a>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav me-auto">
              {role === "Doctor" && (
                <>
                  <li className="nav-item">
                    <a
                      className="nav-link text-white"
                      style={{ marginRight: "15px" }}
                      href="/my-calendar"
                    >
                      Моят календар
                    </a>
                  </li>
                  <li className="nav-item">
                    <a
                      className="nav-link text-white"
                      style={{ marginRight: "15px" }}
                      href="/laboratory/mine"
                    >
                      Моята лаборатория
                    </a>
                  </li>
                </>
              )}
              {(role === "Pharmacist" || role === "PharmacyOwner") && (
                <>
                  <li className="nav-item">
                    <a className="nav-link text-white" href="/orders">
                      Поръчки
                    </a>
                  </li>
                  <li className="nav-item">
                    <a className="nav-link text-white" href="/medications">
                      Лекарства
                    </a>
                  </li>
                  <li className="nav-item">
                    <a className="nav-link text-white" href="/recipes">
                      Електронна рецепта
                    </a>
                  </li>
                </>
              )}
            </ul>
            <ul className="navbar-nav ms-auto">
              <li className="nav-item">
                <a
                  href="#"
                  className="nav-link"
                  onClick={logout}
                  style={{ color: "white" }}
                >
                  Излизане
                </a>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </>
  );
}

export default Navigation;
