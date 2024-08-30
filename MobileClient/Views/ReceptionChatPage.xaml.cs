using HealthProject.Services.AuthenticationService;
using HealthProject.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace HealthProject.Views;

[QueryProperty(nameof(HospitalId), "hospitalId")]
public partial class ReceptionChatPage : ContentPage
{
    private int hospitalId;
    private ReceptionChatViewModel viewModel;
    private readonly HubConnection _connection;
    private IAuthenticationService authenticationService;
    private string userId;
    private string userName;

    public int HospitalId
    {
        get => hospitalId;
        set
        {
            hospitalId = value;
            if (viewModel != null)
            {
                viewModel.HospitalId = value;
            }

            Task.Run(async () => await JoinUserRoomAsync());
        }
    }

    public ReceptionChatPage(IAuthenticationService authenticationService)
    {
        InitializeComponent();

        this.authenticationService = authenticationService;

        IsAuthenticated();

        _connection = new HubConnectionBuilder()
            .WithUrl("http://10.0.2.2:5091/chat") 
            .WithAutomaticReconnect() 
            .Build();

        _connection.On<string, string>("MessageReceived", (message, senderName) =>
        {
            Dispatcher.Dispatch(() =>
            {
                chatMessages.Text += $"{Environment.NewLine}{senderName}: {message}";
            });
        });

        Task.Run(async () =>
        {
            await StartConnectionAsync();
        });
    }

    private async Task StartConnectionAsync()
    {
        try
        {
            await _connection.StartAsync();
            await JoinUserRoomAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }

    private async void IsAuthenticated()
    {
        var auth = await authenticationService.IsAuthenticated();

        if (!auth.IsAuthenticated)
        {
            return;
        }

        this.userId = auth.UserId ?? "invalid";

        var name = await authenticationService.GetUserName(userId);

        this.userName = name;
    }

    private async Task JoinUserRoomAsync()
    {
        while (_connection.State == HubConnectionState.Connecting)
        {
            await Task.Delay(100);
        }

        if (hospitalId > 0 && !string.IsNullOrEmpty(userId) && _connection.State == HubConnectionState.Connected)
        {
            try
            {
                await _connection.InvokeAsync("JoinUserRoom", hospitalId, userId);
                Console.WriteLine("Successfully joined the room.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to join the room: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Cannot join room: hospitalId={hospitalId}, userId={userId}, connectionState={_connection.State}");
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(myChatMessage.Text) && _connection.State == HubConnectionState.Connected)
        {
            string roomName = $"Hospital_{hospitalId}_User_{userId}";
            await _connection.InvokeAsync("SendMessageToRoom", roomName, myChatMessage.Text, userName);
            myChatMessage.Text = string.Empty;
        }
        else
        {
            Console.WriteLine("Cannot send message: Connection not established or invalid data.");
        }
    }
}
