import HospitalsList from "./HospitalsList";
import PharmaciesComponent from "./PharmaciesComponent";
import UsersList from "./UsersList";

function AdminHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <HospitalsList></HospitalsList>
          <UsersList></UsersList>
          <PharmaciesComponent></PharmaciesComponent>
        </div>
      </div>
    </>
  );
}

export default AdminHomePage;
