import MedicationsInPharmacyComponent from "./MedicationsInPharmacyComponent";
import MyPharmacyComponent from "./MyPharmacyComponent";

function PharmacyOwnerHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <MyPharmacyComponent />
          <MedicationsInPharmacyComponent></MedicationsInPharmacyComponent>
        </div>
      </div>
    </>
  );
}

export default PharmacyOwnerHomePage;
