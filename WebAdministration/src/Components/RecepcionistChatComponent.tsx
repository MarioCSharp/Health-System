import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

interface RecepcionistChatComponentProps {
  roomName: string;
}

interface Message {
  sentByUserName: string;
  message: string;
}

const RecepcionistChatComponent: React.FC<RecepcionistChatComponentProps> = ({
  roomName,
}) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(
    null
  );
  const [messages, setMessages] = useState<Message[]>([]);
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
          const data: Message[] = await response.json();
          setMessages(data);
        } else {
          console.error("Failed to fetch room messages");
        }
      } catch (error) {
        console.error("Error fetching room messages:", error);
      }
    };

    if (roomName) {
      fetchRoomMessages();
    }

    if (!connection) {
      const newConnection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5091/chat", {
          accessTokenFactory: () => localStorage.getItem("token") || "",
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

      newConnection.on(
        "MessageReceived",
        (receivedMessage: string, senderName: string) => {
          setMessages((prevMessages) => [
            ...prevMessages,
            { sentByUserName: senderName, message: receivedMessage },
          ]);
          console.log("New message received.");
        }
      );

      newConnection
        .start()
        .then(async () => {
          setConnection(newConnection);
          try {
            await newConnection.invoke("JoinReceptionistRoom", roomName);
            console.log(`Joined room: ${roomName} successfully`);
          } catch (err) {
            console.error("Error joining room: ", err);
          }
        })
        .catch((err) => console.error("Error connecting to SignalR: ", err));
    }

    return () => {
      if (connection) {
        connection.stop().then(() => console.log("SignalR connection stopped"));
      }
    };
  }, [roomName, connection]);

  const sendMessage = async (): Promise<void> => {
    if (connection && message.trim()) {
      try {
        await connection.invoke(
          "SendMessageToRoom",
          roomName,
          message,
          "Рецепция"
        );
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
              <strong>{msg.sentByUserName}:</strong> {msg.message}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default RecepcionistChatComponent;
