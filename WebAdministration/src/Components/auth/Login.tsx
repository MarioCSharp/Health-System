import React, { useState } from "react";
import { useAuth } from "./AuthContext";

const Login: React.FC = () => {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      setError(null);
      await login(email, password);
    } catch (err) {
      setError("Login failed. Please check your email and password.");
    }
  };

  return (
    <div
      className="d-flex justify-content-center align-items-center"
      style={{ height: "calc(100vh - 56px)" }}
    >
      <div className="container" style={{ maxWidth: "500px" }}>
        <div
          className="card border-light shadow"
          style={{ padding: "40px 20px" }}
        >
          <div className="card-body">
            <h2 className="text-center mb-5">Вписване</h2>
            <form onSubmit={handleSubmit}>
              <div className="mb-4">
                <label htmlFor="email" className="form-label">
                  Имейл
                </label>
                <input
                  type="email"
                  className="form-control"
                  id="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
              <div className="mb-4">
                <label htmlFor="password" className="form-label">
                  Парола
                </label>
                <input
                  type="password"
                  className="form-control"
                  id="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </div>
              {error && (
                <div className="mb-4">
                  <p style={{ color: "red" }}>{error}</p>
                </div>
              )}
              <button type="submit" className="btn btn-primary w-100">
                Вписване
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
