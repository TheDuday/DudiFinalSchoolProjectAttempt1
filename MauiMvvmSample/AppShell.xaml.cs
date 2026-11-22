using MauiMvvmSample.Views;

namespace MauiMvvmSample;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Shell.SetFlyoutItemIsVisible(HomeItem, true);
        Shell.SetFlyoutItemIsVisible(HostGameSettingsItem, true);
        Shell.SetFlyoutItemIsVisible(JoinGameItem, true);
        Shell.SetFlyoutItemIsVisible(TutorialItem, true);
    }
    public void ShowMainMenu()
    {
        FlyoutBehavior = FlyoutBehavior.Flyout;
        CurrentItem = HomeItem;
    }
    public void HideMainMenu()
    {
        FlyoutBehavior = FlyoutBehavior.Disabled;
    }

}