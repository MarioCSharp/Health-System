import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./AuthContext";
import Login from "./Login";
import ProtectedRoute from "./ProtectedRoute";
import Home from "./Home";
import NotFound from "./NotFound"; // Import the NotFound component
import DoctorsDisplayPage from "./DoctorsDisplayPage";
import AppointmentsComponent from "./AppointmentsComponent";
import DoctorAppointments from "./DoctorAppointments";

const App: React.FC = () => {
  return (
    <Router>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/" element={<ProtectedRoute />}>
            <Route path="/" element={<Home />} />
            <Route
              path="/doctors/:hospitalId"
              element={<DoctorsDisplayPage />}
            />
            <Route
              path="/appointments/:id"
              element={<AppointmentsComponent />}
            />
            <Route
              path="/doctor/appointments/:id"
              element={<DoctorAppointments />}
            />
          </Route>

          <Route path="*" element={<NotFound />} />
        </Routes>
      </AuthProvider>
    </Router>
  );
};

export default App;
