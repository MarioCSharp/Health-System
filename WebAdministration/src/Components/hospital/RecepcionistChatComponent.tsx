import React, { useEffect, useState, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import { CSSProperties } from "react"; // Import CSSProperties from React

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
  const messagesEndRef = useRef<HTMLDivElement | null>(null);

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
          scrollToBottom(); // Scroll to the latest message
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

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  // Define styles using CSSProperties to avoid TypeScript errors
  const chatBoxStyle: CSSProperties = {
    maxHeight: "400px",
    overflowY: "auto",
    border: "1px solid #ddd",
    borderRadius: "8px",
    padding: "10px",
    backgroundColor: "#f9f9f9",
    marginBottom: "20px",
  };

  const chatBubbleStyle = (isSentByReception: boolean): CSSProperties => ({
    maxWidth: "70%",
    padding: "8px 12px",
    borderRadius: "15px",
    marginBottom: "8px",
    backgroundColor: isSentByReception ? "#007bff" : "#e9ecef",
    color: isSentByReception ? "white" : "black",
    textAlign: isSentByReception
      ? ("right" as CSSProperties["textAlign"])
      : ("left" as CSSProperties["textAlign"]),
    alignSelf: isSentByReception ? "flex-end" : "flex-start",
  });

  return (
    <div className="mt-4">
      <div className="input-group mb-3">
        <input
          type="text"
          className="form-control"
          placeholder="Въведи своето съобщение..."
          value={message}
          onChange={(e) => setMessage(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") sendMessage();
          }}
        />
        <button className="btn btn-primary" onClick={sendMessage}>
          <FontAwesomeIcon icon={faPaperPlane} /> Изпрати
        </button>
      </div>
      <div style={chatBoxStyle}>
        <h4>Съобщения</h4>
        <ul className="list-group">
          {messages.map((msg, index) => (
            <li
              key={index}
              className="list-group-item"
              style={{
                display: "flex",
                justifyContent:
                  msg.sentByUserName === "Рецепция" ? "flex-end" : "flex-start",
                border: "none",
                background: "transparent",
              }}
            >
              <div style={chatBubbleStyle(msg.sentByUserName === "Рецепция")}>
                <strong>{msg.sentByUserName}: </strong>
                {msg.message}
              </div>
            </li>
          ))}
        </ul>
        <div ref={messagesEndRef} />
      </div>
    </div>
  );
};

export default RecepcionistChatComponent;
