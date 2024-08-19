import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./AuthContext";
import Login from "./Login";
import ProtectedRoute from "./ProtectedRoute";
import Home from "./Home";
import NotFound from "./NotFound";
import DoctorsDisplayPage from "./DoctorsDisplayPage";
import AppointmentsComponent from "./AppointmentsComponent";
import DoctorAppointments from "./DoctorAppointments";
import DoctorServicesComponent from "./DoctorServicesComponent";
import ServiceEditComponent from "./ServiceEditComponent";
import DoctorAddComponent from "./DoctorAddComponent";
import ServiceAddComponent from "./ServiceAddComponent";
import HospitalEditComponent from "./HospitalEditComponent";
import HospitalAddComponent from "./HospitalAddComponent";
import AllHospitalsComponents from "./AllHospitalsComponents";
import AllUsersComponent from "./AllUsersComponent";
import AllAppointments from "./AllAppointments";
import PastAppointmentsList from "./PastAppointmentsList";
import AllNextAppointmentsList from "./AllNextAppointments";

const App: React.FC = () => (
  <Router>
    <AuthProvider>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/" element={<ProtectedRoute />}>
          <Route path="/" element={<Home />} />
          <Route path="/doctors/:hospitalId" element={<DoctorsDisplayPage />} />
          <Route path="/appointments/:id" element={<AppointmentsComponent />} />
          <Route path="/past-appointments" element={<PastAppointmentsList />} />
          <Route
            path="/next-appointments"
            element={<AllNextAppointmentsList />}
          />
          <Route
            path="/doctor/appointments/:id"
            element={<DoctorAppointments />}
          />
          <Route
            path="/doctor/services/:doctorId"
            element={<DoctorServicesComponent />}
          />
          <Route path="/service/edit/:id" element={<ServiceEditComponent />} />
          <Route
            path="/service/add/:doctorId"
            element={<ServiceAddComponent />}
          />
          <Route
            path="/doctor/add/:hospitalId"
            element={<DoctorAddComponent />}
          />
          <Route
            path="/hospital/edit/:hospitalId"
            element={<HospitalEditComponent />}
          />
          <Route path="/hospital/add" element={<HospitalAddComponent />} />
          <Route path="/hospitals" element={<AllHospitalsComponents />} />
          <Route path="/users" element={<AllUsersComponent />} />
          <Route path="/appointments" element={<AllAppointments />} />
        </Route>

        <Route path="*" element={<NotFound />} />
      </Routes>
    </AuthProvider>
  </Router>
);

export default App;
