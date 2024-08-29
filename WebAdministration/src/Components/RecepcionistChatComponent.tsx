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
    // Debugging log: Starting connection setup
    console.log("Setting up SignalR connection...");

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
          console.log("Fetched initial messages:", data);
        } else {
          console.error("Failed to fetch room messages");
        }
      } catch (error) {
        console.error("Error fetching room messages:", error);
      }
    };

    fetchRoomMessages();

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5091/chat", {
        accessTokenFactory: () => {
          const token = localStorage.getItem("token");
          if (!token) throw new Error("No access token available");
          return token;
        },
      })
      .withAutomaticReconnect()
      .build();

    newConnection
      .start()
      .then(() => {
        console.log("Connected to SignalR");
        setConnection(newConnection);

        // Listening for incoming messages
        newConnection.on("MessageReceived", (receivedMessage: string) => {
          console.log("New message received:", receivedMessage);
          setMessages((prevMessages) => [...prevMessages, receivedMessage]);
        });
      })
      .catch((err) => console.error("Error connecting to SignalR: ", err));

    return () => {
      if (newConnection) {
        newConnection
          .stop()
          .then(() => console.log("SignalR connection stopped"));
      }
    };
  }, [roomName]);

  const sendMessage = async () => {
    if (connection && message.trim()) {
      try {
        console.log("Sending message:", message);
        await connection.invoke("SendMessageToRoom", roomName, message);
        setMessage("");
      } catch (error) {
        console.error("Error sending message:", error);
      }
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
          onKeyDown={(e) => {
            if (e.key === "Enter") sendMessage();
          }}
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
