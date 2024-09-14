using HealthSyste.ReceptionChat.Models;
using HealthSystem.ReceptionChat.Hubs;

namespace HealthSyste.ReceptionChat.Services.RecepcionistService
{
    public class RecepcionistService : IRecepcionistService
    {
        private HttpClient httpClient;
        private readonly ChatHub chatHub;

        public RecepcionistService(ChatHub chatHub, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.chatHub = chatHub;
        }

        public async Task<List<RoomDisplayModel>> GetMyRooms(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new List<RoomDisplayModel>();
            }

            var doctorResponse = await httpClient.GetAsync($"http://admins/api/Recepcionist/GetHospitalId?userId={userId}");

            if (!doctorResponse.IsSuccessStatusCode)
            {
                return new List<RoomDisplayModel>();
            }

            var hospitalId = await doctorResponse.Content.ReadFromJsonAsync<int>();

            if (hospitalId <= 0)
            {
                return new List<RoomDisplayModel>();
            }

            var roomNames = chatHub.GetRoomsForHospital(hospitalId);

            var roomDisplayModels = roomNames.Select(roomName => new RoomDisplayModel
            {
                Key = roomName
            }).ToList();

            return roomDisplayModels;
        }

        public Task<List<(string, string)>> GetRoomMessages(string roomName)
        {
            var messages = chatHub.GetMessagesForRoom(roomName);
            return Task.FromResult(messages.ToList());
        }
    }
}
