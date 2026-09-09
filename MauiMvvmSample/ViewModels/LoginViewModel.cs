using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;

namespace MauiMvvmSample.ViewModels;

public partial class LoginViewModel : ObservableObject
{
   // private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    //public LoginViewModel(IUserRepository userRepository)
    //{
    //    _userRepository = userRepository;
    //}

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
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

            await Shell.Current.GoToAsync(
                "home",
                new Dictionary<string, object>
                {
                { "user", user }
                });
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
        await Shell.Current.GoToAsync("register");
    }


}

