import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";

interface CartItem {
  id: number;
  itemName: string;
  itemPrice: number;
  itemImage: string | null;
  quantity: number;
}

interface Order {
  id: number;
  status: OrderStatus;
  name: string;
  location: string;
  contactNumber: string;
  totalPrice: number;
  cartItems: CartItem[];
}

enum OrderStatus {
  Placed = "Placed",
  Confirmed = "Confirmed",
  Processing = "Processing",
  Shipped = "Shipped",
  Delivered = "Delivered",
  Completed = "Completed",
  Canceled = "Canceled",
  Returned = "Returned",
  Refunded = "Refunded",
}

function OrdersInMyPharmacyComponent() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [pharmacyId, setPharmacyId] = useState<number | null>(null);
  const [expandedOrderIds, setExpandedOrderIds] = useState<number[]>([]);

  const token = localStorage.getItem("token");

  const getPharmacyId = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Pharmacy/GetMyPharmacyId`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        setPharmacyId(data);
      } else {
        throw new Error("There was an error loading the pharmacy ID.");
      }
    } catch (error) {
      alert(error);
    }
  };

  const getOrders = async () => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Orders/AllOrdersInPharmacy?pharmacyId=${pharmacyId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        const data = await response.json();
        setOrders(data);
      } else {
        throw new Error("There was an error loading the orders.");
      }
    } catch (error) {
      alert(error);
    }
  };

  const changeOrderStatus = async (orderId: number, newStatus: OrderStatus) => {
    try {
      const response = await fetch(
        `http://localhost:5171/api/Orders/ChangeStatus?orderId=${orderId}&newStatus=${newStatus}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.ok) {
        getOrders();
      } else {
        throw new Error("There was an error changing the order status.");
      }
    } catch (error) {
      alert(error);
    }
  };

  useEffect(() => {
    if (pharmacyId) {
      getOrders();
    }
  }, [pharmacyId]);

  useEffect(() => {
    getPharmacyId();
  }, []);

  const toggleOrderItems = (orderId: number) => {
    setExpandedOrderIds((prevIds) =>
      prevIds.includes(orderId)
        ? prevIds.filter((id) => id !== orderId)
        : [...prevIds, orderId]
    );
  };

  const getStatusColor = (status: OrderStatus) => {
    switch (status) {
      case OrderStatus.Placed:
        return "bg-light";
      case OrderStatus.Confirmed:
        return "bg-info";
      case OrderStatus.Processing:
        return "bg-warning";
      case OrderStatus.Shipped:
        return "bg-primary";
      case OrderStatus.Delivered:
        return "bg-success";
      case OrderStatus.Completed:
        return "bg-secondary";
      case OrderStatus.Canceled:
        return "bg-danger";
      case OrderStatus.Returned:
        return "bg-warning";
      case OrderStatus.Refunded:
        return "bg-dark text-white";
      default:
        return "bg-light";
    }
  };

  return (
    <div className="container mt-4">
      <h2>Поръчки</h2>
      <table className="table table-striped table-bordered">
        <thead>
          <tr>
            <th>Поръчка</th>
            <th>Статус</th>
            <th>Име</th>
            <th>Адрес</th>
            <th>Номер за връзка</th>
            <th>Сума</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <React.Fragment key={order.id}>
              <tr>
                <td>{order.id}</td>
                <td className={getStatusColor(order.status)}>
                  {order.status}
                  <div className="mt-2">
                    <select
                      className="form-select"
                      value={order.status}
                      onChange={(e) =>
                        changeOrderStatus(
                          order.id,
                          e.target.value as OrderStatus
                        )
                      }
                    >
                      {Object.values(OrderStatus).map((status) => (
                        <option key={status} value={status}>
                          {status}
                        </option>
                      ))}
                    </select>
                  </div>
                </td>
                <td>{order.name}</td>
                <td>{order.location}</td>
                <td>{order.contactNumber}</td>
                <td>${order.totalPrice.toFixed(2)}</td>
                <td>
                  <button
                    className="btn btn-primary"
                    onClick={() => toggleOrderItems(order.id)}
                  >
                    {expandedOrderIds.includes(order.id)
                      ? "Hide Items"
                      : "Show Items"}
                  </button>
                </td>
              </tr>
              {expandedOrderIds.includes(order.id) && (
                <tr>
                  <td colSpan={7}>
                    <ul>
                      {order.cartItems.map((item) => (
                        <li key={item.id}>
                          <strong>{item.itemName}</strong> - $
                          {item.itemPrice.toFixed(2)} (Кол: {item.quantity})
                        </li>
                      ))}
                    </ul>
                  </td>
                </tr>
              )}
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default OrdersInMyPharmacyComponent;
