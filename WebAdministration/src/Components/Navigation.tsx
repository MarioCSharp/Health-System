import { Navigate } from "react-router-dom";

function Navigation() {
  const logout = () => {
    const token = localStorage.getItem("token");

    if (token) {
      localStorage.removeItem("token");
      <Navigate to={"/login"}></Navigate>;
    }
  };

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
        <div className="container-fluid">
          <a className="navbar-brand" href="/">
            MedCare Administration
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
          <a className="nav-item" href="/my-calendar">
            Моят календар
          </a>
          <a className="nav-item" href="/laboratory/mine">
            Моята лаборатория
          </a>
          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav ms-auto">
              <li className="nav-item">
                <a
                  href=""
                  className="nav-link"
                  onClick={() => logout()}
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
