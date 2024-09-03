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
import DoctorCommentsComponent from "./DoctorCommentsComponent";
import MyCalendarComponent from "./MyCalendarComponent";
import LaboratoryResultsComponent from "./LaboratoryResultsComponent";
import LaboratoryAddComponent from "./LaboratoryAddComponent";
import RecipeByEgnComponent from "./RecipeByEgnComponent";
import AllMyRecepcionistsPage from "./AllMyRecepcionistsPage";
import AddRecepcionistComponent from "./AddRecepcionistComponent";
import PharmacyAddComponent from "./PharmacyAddComponent";
import PharmacistsInPharmacyComponent from "./PharmacistsInPharmacyComponent";

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
          <Route
            path="/doctor/comments/:doctorId"
            element={<DoctorCommentsComponent />}
          />
          <Route path="/my-calendar" element={<MyCalendarComponent />} />
          <Route
            path="/laboratory/mine"
            element={<LaboratoryResultsComponent />}
          />
          <Route path="/laboratory/add" element={<LaboratoryAddComponent />} />
          <Route path="/recipes" element={<RecipeByEgnComponent />} />
          <Route
            path="/recepcionists/all"
            element={<AllMyRecepcionistsPage />}
          />
          <Route
            path="/recepcionists/add"
            element={<AddRecepcionistComponent />}
          />
          <Route path="/pharmacy/add" element={<PharmacyAddComponent />} />
          <Route
            path="/pharmacists/:pharmacyId"
            element={<PharmacistsInPharmacyComponent />}
          />
          <Route
            path="/pharmacist/add/:pharmacyId"
            element={<PharmacyAddComponent />}
          />
        </Route>

        <Route path="*" element={<NotFound />} />
      </Routes>
    </AuthProvider>
  </Router>
);

export default App;
