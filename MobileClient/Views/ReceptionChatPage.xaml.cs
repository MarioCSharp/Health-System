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

            // Join the user's unique room
            JoinUserRoom();
        }
    }

    public ReceptionChatPage(IAuthenticationService authenticationService)
    {
        InitializeComponent();

        this.authenticationService = authenticationService;

        var auth = authenticationService.IsAuthenticated().Result;

        if (!auth.IsAuthenticated)
        {
            return;
        }

        this.userId = auth.UserId ?? "invalid";

        _connection = new HubConnectionBuilder()
            .WithUrl("http://192.168.0.104:5091/chat")
            .Build();

        _connection.On<string>("MessageReceived", (message) =>
        {
            Dispatcher.Dispatch(() =>
            {
                chatMessages.Text += $"{Environment.NewLine}{message}";
            });
        });

        Task.Run(async () =>
        {
            await _connection.StartAsync();
            JoinUserRoom();
        });
    }

    private async void JoinUserRoom()
    {
        if (hospitalId > 0 && !string.IsNullOrEmpty(userId) && _connection.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync("JoinUserRoom", hospitalId, userId);
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(myChatMessage.Text))
        {
            await _connection.InvokeAsync("SendMessage", hospitalId, userId, myChatMessage.Text);
            myChatMessage.Text = string.Empty;
        }
    }
}
