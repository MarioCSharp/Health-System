import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  useLocation,
} from "react-router-dom";
import ProtectedRoute from "./ProtectedRoute";
import Home from "./Home";
import NotFound from "./NotFound";
import { AuthProvider } from "../auth/AuthContext";
import Login from "../auth/Login";
import AppointmentsComponent from "../appointments/AppointmentsComponent";
import PastAppointmentsList from "../appointments/PastAppointmentsList";
import AllNextAppointmentsList from "../appointments/AllNextAppointments";
import DoctorAppointments from "../appointments/DoctorAppointments";
import DoctorServicesComponent from "../doctor/DoctorServicesComponent";
import ServiceEditComponent from "../doctor/ServiceEditComponent";
import ServiceAddComponent from "../doctor/ServiceAddComponent";
import DoctorAddComponent from "../doctor/DoctorAddComponent";
import HospitalAddComponent from "../hospital/HospitalAddComponent";
import AllHospitalsComponents from "../hospital/AllHospitalsComponents";
import AllUsersComponent from "../admin/AllUsersComponent";
import AllAppointments from "../appointments/AllAppointments";
import DoctorCommentsComponent from "../director/DoctorCommentsComponent";
import MyCalendarComponent from "../doctor/MyCalendarComponent";
import LaboratoryResultsComponent from "../laboratory/LaboratoryResultsComponent";
import LaboratoryAddComponent from "../laboratory/LaboratoryAddComponent";
import RecipeByEgnComponent from "../pharmacy/RecipeByEgnComponent";
import AllMyRecepcionistsPage from "../hospital/AllMyRecepcionistsPage";
import AddRecepcionistComponent from "../hospital/AddRecepcionistComponent";
import PharmacyAddComponent from "../pharmacy/PharmacyAddComponent";
import PharmacistsInPharmacyComponent from "../pharmacy/PharmacistsInPharmacyComponent";
import PharmacistAddComponent from "../pharmacy/PharmacistAddComponent";
import AllPharmaciesComponent from "../pharmacy/AllPharmaciesComponent";
import MedicationAddPage from "../pharmacy/MedicationAddPage";
import MedicationsInPharmacyComponent from "../pharmacy/MedicationsInPharmacyComponent";
import OrdersInMyPharmacyComponent from "../pharmacy/OrdersInMyPharmacyComponent";
import BackButton from "./BackButtons";

const AppContent: React.FC = () => {
  const location = useLocation();
  const showBackButton =
    location.pathname !== "/" && location.pathname !== "/login";

  return (
    <div>
      {showBackButton && <BackButton />}

      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/" element={<ProtectedRoute />}>
          <Route path="/" element={<Home />} />
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
            element={<PharmacistAddComponent />}
          />
          <Route path="/pharmacies" element={<AllPharmaciesComponent />} />
          <Route path="/medication/add" element={<MedicationAddPage />} />
          <Route
            path="/medications"
            element={<MedicationsInPharmacyComponent />}
          />
          <Route path="/orders" element={<OrdersInMyPharmacyComponent />} />
        </Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </div>
  );
};

const App: React.FC = () => (
  <Router>
    <AuthProvider>
      <AppContent />
    </AuthProvider>
  </Router>
);

export default App;
