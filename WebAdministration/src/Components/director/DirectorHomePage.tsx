import DoctorsInMyHosptail from "../doctor/DoctorsInMyHosptail";
import MyRecepcionistsComponent from "../hospital/MyRecepcionistsComponent";
import MyHospitalComponent from "./MyHospitalComponent";

function DirectorHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <MyHospitalComponent></MyHospitalComponent>
          <DoctorsInMyHosptail></DoctorsInMyHosptail>
          <MyRecepcionistsComponent></MyRecepcionistsComponent>
        </div>
      </div>
    </>
  );
}

export default DirectorHomePage;
