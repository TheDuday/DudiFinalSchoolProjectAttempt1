using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MauiMvvmSample.Services;

namespace MauiMvvmSample.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    //private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string username;
    [ObservableProperty]
    private string password;

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
        var user = await _authService.LoginAsync(Username, Password);
        if (user == null)
            // return;
            await Shell.Current.GoToAsync("register");
        else
            await Shell.Current.GoToAsync("home",
                new Dictionary<string, object> { { "user", user } });
    
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await Shell.Current.GoToAsync("register");
    }


}

