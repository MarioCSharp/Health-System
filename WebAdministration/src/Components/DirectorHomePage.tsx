import DoctorsInMyHosptail from "./DoctorsInMyHosptail";
import MyHospitalComponent from "./MyHospitalComponent";

function DirectorHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <MyHospitalComponent></MyHospitalComponent>
          <DoctorsInMyHosptail></DoctorsInMyHosptail>
        </div>
      </div>
    </>
  );
}

export default DirectorHomePage;
