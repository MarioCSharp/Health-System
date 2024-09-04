import MedicationsInPharmacyComponent from "./MedicationsInPharmacyComponent";
import MyPharmacyComponent from "./MyPharmacyComponent";
import OrdersInMyPharmacyComponent from "./OrdersInMyPharmacyComponent";

function PharmacyOwnerHomePage() {
  return (
    <>
      <div className="container">
        <div className="row">
          <MyPharmacyComponent />
        </div>
      </div>
    </>
  );
}

export default PharmacyOwnerHomePage;
