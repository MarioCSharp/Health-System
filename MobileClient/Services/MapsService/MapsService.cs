namespace HealthProject.Services.MapsService
{
    public class MapsService
    {
        public async Task OpenGoogleMaps(string address)
        {
            try
            {
                var encodedAddress = Uri.EscapeDataString(address);
                var mapsUrl = $"https://www.google.com/maps/search/?api=1&query={encodedAddress}";

                await Launcher.OpenAsync(mapsUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
