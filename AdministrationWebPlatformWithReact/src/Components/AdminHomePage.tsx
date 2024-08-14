import HospitalsList from "./HospitalsList";
import UsersList from "./UsersList";

function AdminHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <HospitalsList></HospitalsList>
          <UsersList></UsersList>
        </div>
      </div>
    </>
  );
}

export default AdminHomePage;
