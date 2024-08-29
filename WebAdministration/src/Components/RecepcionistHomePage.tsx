import React, { useState, useEffect } from "react";
import RecepcionistChatComponent from "./RecepcionistChatComponent";

const RecepcionistHomePage = () => {
  const [rooms, setRooms] = useState<string[]>([]);
  const [selectedRoom, setSelectedRoom] = useState<string>("");

  useEffect(() => {
    const fetchRooms = async () => {
      const token = localStorage.getItem("token");
      const response = await fetch(
        "http://localhost:5091/api/RecepcionistChat/GetMyRooms",
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      if (response.ok) {
        const data = await response.json();
        setRooms(data.rooms);
      } else {
        console.error("Failed to fetch rooms");
      }
    };

    fetchRooms();
  }, []);

  const handleRoomClick = (roomName: string) => {
    setSelectedRoom(roomName);
  };

  return (
    <div className="container">
      <h2 className="mt-4">Receptionist Home Page</h2>
      <div className="row">
        <div className="col-md-4">
          <h4>Rooms</h4>
          <ul className="list-group">
            {rooms && rooms.length > 0 ? (
              rooms.map((room, index) => (
                <li
                  key={index}
                  className={`list-group-item ${
                    selectedRoom === room ? "active" : ""
                  }`}
                  onClick={() => handleRoomClick(room)}
                >
                  {room}
                </li>
              ))
            ) : (
              <li>No rooms available</li>
            )}
          </ul>
        </div>
        <div className="col-md-8">
          {selectedRoom ? (
            <RecepcionistChatComponent roomName={selectedRoom} />
          ) : (
            <div>Please select a room to view messages</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecepcionistHomePage;
