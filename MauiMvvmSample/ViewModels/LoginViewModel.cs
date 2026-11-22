using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;

namespace MauiMvvmSample.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private SessionData sessionData;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    public LoginViewModel(IAuthService authService, SessionData sessionData)
    {
        _authService = authService;
        this.sessionData = sessionData;

        if (Shell.Current is AppShell appshell)
        {
            appshell.HideMainMenu();
        }
    }

    [RelayCommand]
    
    private async Task LoginAsync()
    {
        try
        {
            var user = await _authService.LoginAsync(
                Email,
                Password);

            if (user == null)
            {
                await Shell.Current.DisplayAlert(
                    "Login",
                    "Invalid email or password.",
                    "OK");

                return;
            }

            sessionData.user = user;

            await Shell.Current.GoToAsync("//home");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                ex.Message,
                "OK");
        }
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await Shell.Current.GoToAsync("//register");
    }
}