import MyServicesComponent from "./MyServicesComponent";
import NextAppointmentsList from "./NextAppointmentsList";
import PastAppointmentsList from "./PastAppointmentsList";

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
