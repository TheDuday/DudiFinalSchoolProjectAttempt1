namespace MauiMvvmSample;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // רישום Routes לניווט
        Routing.RegisterRoute("home", typeof(Views.HomePage));
        Routing.RegisterRoute("register", typeof(Views.RegisterPage));

    }
}