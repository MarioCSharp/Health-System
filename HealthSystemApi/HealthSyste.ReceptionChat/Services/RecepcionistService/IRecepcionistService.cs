using HealthSyste.ReceptionChat.Models;

namespace HealthSyste.ReceptionChat.Services.RecepcionistService
{
    public interface IRecepcionistService
    {
        Task<List<RoomDisplayModel>> GetMyRooms(string userId);
        Task<List<string>> GetRoomMessages(string roomName);
    }
}
