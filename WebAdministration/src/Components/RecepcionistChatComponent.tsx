import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

interface RecepcionistChatComponentProps {
  roomName: string;
}

const RecepcionistChatComponent: React.FC<RecepcionistChatComponentProps> = ({
  roomName,
}) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [messages, setMessages] = useState<string[]>([]);
  const [message, setMessage] = useState<string>("");

  useEffect(() => {
    const fetchRoomMessages = async () => {
      const token = localStorage.getItem("token");
      try {
        const response = await fetch(
          `http://localhost:5091/api/RecepcionistChat/GetRoomMessages?roomName=${roomName}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        if (response.ok) {
          const data: string[] = await response.json();
          setMessages(data);
        } else {
          console.error("Failed to fetch room messages");
        }
      } catch (error) {
        console.error("Error fetching room messages:", error);
      }
    };

    fetchRoomMessages();

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://192.168.0.104:5091/chat", {
        accessTokenFactory: () => {
          const token = localStorage.getItem("token");
          if (!token) throw new Error("No access token available");
          return token;
        },
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    newConnection
      .start()
      .then(() => {
        console.log("Connected to SignalR");

        newConnection.on("MessageReceived", (message: string) => {
          setMessages((prevMessages) => [...prevMessages, message]);
        });
      })
      .catch((err) => console.log("Error connecting to SignalR: ", err));

    return () => {
      if (newConnection) {
        newConnection.stop();
      }
    };
  }, [roomName]);

  const sendMessage = async () => {
    if (connection && message) {
      await connection.invoke("SendMessageToRoom", roomName, message);
      setMessage("");
    }
  };

  return (
    <div className="mt-4">
      <div className="input-group mb-3">
        <input
          type="text"
          className="form-control"
          placeholder="Enter your message"
          value={message}
          onChange={(e) => setMessage(e.target.value)}
        />
        <button className="btn btn-primary" onClick={sendMessage}>
          Send Message
        </button>
      </div>
      <div>
        <h4>Chat Messages</h4>
        <ul className="list-group">
          {messages.map((msg, index) => (
            <li key={index} className="list-group-item">
              {msg}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default RecepcionistChatComponent;
