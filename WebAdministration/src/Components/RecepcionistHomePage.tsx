import React, { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import RecepcionistChatComponent from "./RecepcionistChatComponent";

interface Room {
  key: string;
}

const RecepcionistHomePage = () => {
  const [rooms, setRooms] = useState<string[]>([]);
  const [selectedRoom, setSelectedRoom] = useState<string>("");
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );

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
        setRooms(data.rooms.map((room: Room) => room.key));
      } else {
        console.error("Failed to fetch rooms");
      }
    };

    fetchRooms();

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5091/chat")
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    newConnection.on("RoomCreated", (roomName: string) => {
      setRooms((prevRooms) => [...prevRooms, roomName]);
      console.log("New room created:", roomName);
    });

    newConnection
      .start()
      .then(() => {
        setConnection(newConnection);
        console.log("Connected to SignalR");
      })
      .catch((err) => console.error("Error connecting to SignalR: ", err));

    return () => {
      if (newConnection) {
        newConnection
          .stop()
          .then(() => console.log("SignalR connection stopped"));
      }
    };
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
