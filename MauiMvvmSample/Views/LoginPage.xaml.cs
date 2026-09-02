using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
