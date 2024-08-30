import DoctorsInMyHosptail from "./DoctorsInMyHosptail";
import MyHospitalComponent from "./MyHospitalComponent";
import MyRecepcionistsComponent from "./MyRecepcionistsComponent";

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
