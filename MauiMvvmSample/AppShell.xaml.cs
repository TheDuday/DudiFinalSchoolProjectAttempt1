using MauiMvvmSample.Views;

namespace MauiMvvmSample;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // רישום Routes לניווט
        Routing.RegisterRoute("home", typeof(HomePage));
        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("hostGameSettings", typeof(HostGameSettingsPage));
        Routing.RegisterRoute("waitingRoom", typeof(WaitingRoomPage));
        Routing.RegisterRoute("joinGame", typeof(JoinGamePage));
        Routing.RegisterRoute("game", typeof(GamePage));
    }
}