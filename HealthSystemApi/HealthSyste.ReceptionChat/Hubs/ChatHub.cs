using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HealthSystem.ReceptionChat.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<int, HashSet<string>> HospitalRooms =
            new ConcurrentDictionary<int, HashSet<string>>();

        public async Task JoinUserRoom(int hospitalId, int userId)
        {
            string roomName = $"Hospital_{hospitalId}_User_{userId}";
            AddRoom(hospitalId, roomName);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task JoinReceptionistRoom(int hospitalId, int userId)
        {
            string roomName = $"Hospital_{hospitalId}_User_{userId}";
            AddRoom(hospitalId, roomName);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(int hospitalId, int userId, string message)
        {
            string roomName = $"Hospital_{hospitalId}_User_{userId}";
            Console.WriteLine($"Message to {roomName}: {message}");
            await Clients.Group(roomName).SendAsync("MessageReceived", message);
        }

        public IEnumerable<string> GetRoomsForHospital(int hospitalId)
        {
            if (HospitalRooms.TryGetValue(hospitalId, out var rooms))
            {
                return rooms;
            }
            return Enumerable.Empty<string>();
        }

        private void AddRoom(int hospitalId, string roomName)
        {
            var rooms = HospitalRooms.GetOrAdd(hospitalId, _ => new HashSet<string>());
            rooms.Add(roomName);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}