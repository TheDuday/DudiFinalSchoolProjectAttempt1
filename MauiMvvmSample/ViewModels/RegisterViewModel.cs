using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Services;

namespace MauiMvvmSample.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _auth;

    public RegisterViewModel(IAuthService auth)
    {
        _auth = auth;

        if (Shell.Current is AppShell appshell)
        {
            appshell.HideMainMenu();
        }
    }

    [ObservableProperty] string username;
    [ObservableProperty] string email;
    [ObservableProperty] string phone;
    [ObservableProperty] string password;



    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Error", "Missing fields", "OK");
            return;
        }

        await _auth.RegisterAsync(Username, Email, Phone, Password);
        await Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    private async Task GoToLoginAsync()
    {
        await Shell.Current.GoToAsync("//login");
    }
}
