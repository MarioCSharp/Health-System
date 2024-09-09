import NextAppointmentsList from "../appointments/NextAppointmentsList";
import PastAppointmentsList from "../appointments/PastAppointmentsList";
import MyServicesComponent from "./MyServicesComponent";

function DoctorHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <NextAppointmentsList></NextAppointmentsList>
          <PastAppointmentsList></PastAppointmentsList>
          <MyServicesComponent></MyServicesComponent>
        </div>
      </div>
    </>
  );
}

export default DoctorHomePage;
