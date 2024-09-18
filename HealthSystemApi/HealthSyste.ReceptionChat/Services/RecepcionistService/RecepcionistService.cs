using HealthSyste.ReceptionChat.Models;
using HealthSystem.ReceptionChat.Hubs;

namespace HealthSyste.ReceptionChat.Services.RecepcionistService
{
    /// <summary>
    /// Service responsible for handling receptionist-related operations such as retrieving rooms and messages.
    /// </summary>
    public class RecepcionistService : IRecepcionistService
    {
        private HttpClient httpClient;
        private readonly ChatHub chatHub;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecepcionistService"/> class.
        /// </summary>
        /// <param name="chatHub">An instance of <see cref="ChatHub"/> for managing chat rooms and messages.</param>
        /// <param name="httpClient">HTTP client used for making API requests.</param>
        public RecepcionistService(ChatHub chatHub, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.chatHub = chatHub;
        }

        /// <summary>
        /// Retrieves the list of rooms associated with the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user (receptionist).</param>
        /// <returns>A list of <see cref="RoomDisplayModel"/> objects representing the rooms.</returns>
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

        /// <summary>
        /// Retrieves the messages associated with the specified room.
        /// </summary>
        /// <param name="roomName">The name of the room to get messages from.</param>
        /// <returns>A list of messages with their associated sender names.</returns>
        public Task<List<(string, string)>> GetRoomMessages(string roomName)
        {
            var messages = chatHub.GetMessagesForRoom(roomName);
            return Task.FromResult(messages.ToList());
        }
    }
}
