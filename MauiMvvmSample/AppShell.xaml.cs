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
        Routing.RegisterRoute("tutorial", typeof(TutorialPage));
    }
    public void ShowMainMenu()
    {
        Shell.SetFlyoutItemIsVisible(HomeItem, true);
        Shell.SetFlyoutItemIsVisible(HostGameSettingsItem, true);
        Shell.SetFlyoutItemIsVisible(JoinGameItem, true);
        Shell.SetFlyoutItemIsVisible(TutorialItem, true);

        FlyoutBehavior = FlyoutBehavior.Flyout;
        CurrentItem = HomeItem;
    }
    public void HideMainMenu()
    {
        FlyoutBehavior = FlyoutBehavior.Disabled;
    }
}