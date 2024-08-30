import React, { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";
import RecepcionistChatComponent from "./RecepcionistChatComponent";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash } from "@fortawesome/free-solid-svg-icons";

interface Room {
  key: string;
}

const RecepcionistHomePage = () => {
  const [rooms, setRooms] = useState<string[]>([]);
  const [selectedRoom, setSelectedRoom] = useState<string>("");
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [hospitalId, setHospitalId] = useState<number | null>(null);

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

    const getHospitalId = async () => {
      const token = localStorage.getItem("token");
      try {
        const resp = await fetch(
          `http://localhost:5025/api/Recepcionist/GetHospitalAndUserId`,
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (resp.ok) {
          const responseData = await resp.json();
          setHospitalId(responseData.hospitalId);
        } else {
          throw new Error("Error loading the hospitalId.");
        }
      } catch (error) {
        console.log(error);
      }
    };

    fetchRooms();
    getHospitalId();

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5091/chat")
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    newConnection.on("RoomCreated", (roomName: string) => {
      setRooms((prevRooms) => {
        if (!prevRooms.includes(roomName)) {
          return [...prevRooms, roomName];
        }
        return prevRooms;
      });
      console.log("New room created:", roomName);
    });

    newConnection.on("RoomDeleted", (roomName: string) => {
      setRooms((prevRooms) => prevRooms.filter((room) => room !== roomName));
      console.log("Room deleted:", roomName);
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

  const handleDeleteRoom = async (roomName: string, e: React.MouseEvent) => {
    e.stopPropagation(); // Prevents the click event from triggering handleRoomClick
    const token = localStorage.getItem("token");

    if (hospitalId !== null) {
      const response = await fetch(
        `http://localhost:5091/api/RecepcionistChat/DeleteRoom?roomName=${roomName}&hospitalId=${hospitalId}`,
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      if (response.ok) {
        setRooms((prevRooms) => prevRooms.filter((room) => room !== roomName));
        window.location.reload();
      } else {
        console.error("Failed to delete room");
      }
    } else {
      console.error("Hospital ID is not set.");
    }
  };

  return (
    <div className="container">
      <h2 className="mt-4">Рецепция</h2>
      <div className="row">
        <div className="col-md-4">
          <h4>Чат стаи</h4>
          <ul className="list-group">
            {rooms && rooms.length > 0 ? (
              rooms.map((room, index) => (
                <li
                  key={index}
                  className={`list-group-item ${
                    selectedRoom === room ? "active" : ""
                  }`}
                  onClick={() => handleRoomClick(room)}
                  style={{ cursor: "pointer" }}
                >
                  <div className="d-flex justify-content-between align-items-center">
                    <span>{room}</span>
                    <FontAwesomeIcon
                      icon={faTrash}
                      className="text-danger"
                      onClick={(e) => handleDeleteRoom(room, e)}
                    />
                  </div>
                </li>
              ))
            ) : (
              <li>Няма отворени стаи</li>
            )}
          </ul>
        </div>
        <div className="col-md-8">
          {selectedRoom ? (
            <RecepcionistChatComponent roomName={selectedRoom} />
          ) : (
            <div>Избери стая за да видиш съобщенията</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default RecepcionistHomePage;
