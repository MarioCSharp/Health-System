using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HealthSystem.ReceptionChat.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<int, HashSet<string>> HospitalRooms =
            new ConcurrentDictionary<int, HashSet<string>>();

        private static readonly ConcurrentDictionary<string, List<(string, string)>> RoomMessages =
            new ConcurrentDictionary<string, List<(string, string)>>();

        public async Task JoinUserRoom(int hospitalId, string userId)
        {
            string roomName = $"Hospital_{hospitalId}_User_{userId}";
            AddRoom(hospitalId, roomName);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task JoinReceptionistRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessageToRoom(string roomName, string message, string name)
        {
            AddMessageToRoom(roomName, message, name);

            await Clients.Group(roomName).SendAsync("MessageReceived", message, name);
        }

        public IEnumerable<string> GetRoomsForHospital(int hospitalId)
        {
            if (HospitalRooms.TryGetValue(hospitalId, out var rooms))
            {
                return rooms;
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<(string, string)> GetMessagesForRoom(string roomName)
        {
            if (RoomMessages.TryGetValue(roomName, out var messages))
            {
                return messages;
            }

            return Enumerable.Empty<(string, string)>();
        }

        private void AddRoom(int hospitalId, string roomName)
        {
            var rooms = HospitalRooms.GetOrAdd(hospitalId, _ => new HashSet<string>());
            rooms.Add(roomName);
            Clients.All.SendAsync("RoomCreated", roomName);
        }

        private void AddMessageToRoom(string roomName, string message, string name)
        {
            var messages = RoomMessages.GetOrAdd(roomName, _ => new List<(string, string)>());
            messages.Add((message, name));
        }

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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
