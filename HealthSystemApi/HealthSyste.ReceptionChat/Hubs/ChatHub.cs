using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HealthSystem.ReceptionChat.Hubs
{
    /// <summary>
    /// Hub responsible for handling real-time chat functionality, such as managing rooms and sending messages.
    /// </summary>
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<int, HashSet<string>> HospitalRooms =
            new ConcurrentDictionary<int, HashSet<string>>();

        private static readonly ConcurrentDictionary<string, List<(string, string)>> RoomMessages =
            new ConcurrentDictionary<string, List<(string, string)>>();

        /// <summary>
        /// Adds a user to a specific room based on their hospital ID and user ID.
        /// </summary>
        /// <param name="hospitalId">The hospital ID to associate the room with.</param>
        /// <param name="userId">The user ID to associate with the room.</param>
        public async Task JoinUserRoom(int hospitalId, string userId)
        {
            string roomName = $"Hospital_{hospitalId}_User_{userId}";
            AddRoom(hospitalId, roomName);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// Adds a receptionist to a specific room.
        /// </summary>
        /// <param name="roomName">The name of the room to join.</param>
        public async Task JoinReceptionistRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// Sends a message to a specific room.
        /// </summary>
        /// <param name="roomName">The name of the room to send the message to.</param>
        /// <param name="message">The message content.</param>
        /// <param name="name">The name of the sender.</param>
        public async Task SendMessageToRoom(string roomName, string message, string name)
        {
            AddMessageToRoom(roomName, message, name);
            await Clients.Group(roomName).SendAsync("MessageReceived", message, name);
        }

        /// <summary>
        /// Gets the list of rooms for a specific hospital.
        /// </summary>
        /// <param name="hospitalId">The hospital ID to get rooms for.</param>
        /// <returns>A list of room names associated with the specified hospital.</returns>
        public IEnumerable<string> GetRoomsForHospital(int hospitalId)
        {
            if (HospitalRooms.TryGetValue(hospitalId, out var rooms))
            {
                return rooms;
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Retrieves the list of messages for a specific room.
        /// </summary>
        /// <param name="roomName">The name of the room to get messages from.</param>
        /// <returns>A list of messages and the corresponding senders in the specified room.</returns>
        public IEnumerable<(string, string)> GetMessagesForRoom(string roomName)
        {
            if (RoomMessages.TryGetValue(roomName, out var messages))
            {
                return messages;
            }

            return Enumerable.Empty<(string, string)>();
        }

        /// <summary>
        /// Adds a new room to the hospital's list of rooms.
        /// </summary>
        /// <param name="hospitalId">The hospital ID to associate the room with.</param>
        /// <param name="roomName">The name of the room to add.</param>
        private void AddRoom(int hospitalId, string roomName)
        {
            var rooms = HospitalRooms.GetOrAdd(hospitalId, _ => new HashSet<string>());
            rooms.Add(roomName);
            Clients.All.SendAsync("RoomCreated", roomName);
        }

        /// <summary>
        /// Adds a message to a specific room's message history.
        /// </summary>
        /// <param name="roomName">The name of the room to add the message to.</param>
        /// <param name="message">The message content.</param>
        /// <param name="name">The name of the sender.</param>
        private void AddMessageToRoom(string roomName, string message, string name)
        {
            var messages = RoomMessages.GetOrAdd(roomName, _ => new List<(string, string)>());
            messages.Add((message, name));
        }

        /// <summary>
        /// Deletes a room and notifies all clients about the room deletion.
        /// </summary>
        /// <param name="hospitalId">The ID of the hospital to which the room belongs.</param>
        /// <param name="roomName">The name of the room to be deleted.</param>
        public async Task DeleteRoom(int hospitalId, string roomName)
        {
            if (HospitalRooms.TryGetValue(hospitalId, out var rooms) && rooms.Remove(roomName))
            {
                RoomMessages.TryRemove(roomName, out _);

                await Clients.Group(roomName).SendAsync("RoomDeleted", roomName);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                await Clients.All.SendAsync("RoomDeleted", roomName);
            }
        }

        /// <summary>
        /// Handles the disconnection of a client.
        /// </summary>
        /// <param name="exception">The exception that caused the disconnection, if any.</param>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
