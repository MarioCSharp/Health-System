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
        <a className="navbar-brand" href="/">
          MedCare Administration
        </a>
        <button
          className="navbar-toggler"
          type="button"
          data-toggle="collapse"
          data-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <a href="" onClick={() => logout()} style={{ color: "white" }}>
          Излизане
        </a>
      </nav>
    </>
  );
}

export default Navigation;
