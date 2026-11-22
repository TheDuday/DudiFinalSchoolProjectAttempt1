using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Models;
using MauiMvvmSample.Services;

namespace MauiMvvmSample.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private SessionData sessionData;

    public HomeViewModel(IAuthService authService, SessionData sessionData)
    {
        _authService = authService;
        this.sessionData = sessionData;
        if (Shell.Current is AppShell appshell)
        {
            appshell.ShowMainMenu();
        }
    }
    [RelayCommand]
    private async Task GoToHostGameSettingsAsync()
    {
        await Shell.Current.GoToAsync("//hostGameSettings");
    }
    [RelayCommand]
    private async Task GoToJoinGameAsync()
    {
        await Shell.Current.GoToAsync("//joinGame");
    }
    //[RelayCommand]
    //private async Task EditEmailAsync()
    //{
    //    var newEmail = await Shell.Current.DisplayPromptAsync(
    //        "Edit Email",
    //        "Enter new email:",
    //        initialValue: User.Email);

    //    if (string.IsNullOrWhiteSpace(newEmail))
    //        return;

    //    //await _authService.UpdateProfileAsync(
    //    //    User,
    //    //    newEmail,
    //    //    User.Phone);

    //    OnPropertyChanged(nameof(User));
    //}

    //[RelayCommand]
    //private async Task EditPhoneAsync()
    //{
    //    var newPhone = await Shell.Current.DisplayPromptAsync(
    //        "Edit Phone",
    //        "Enter new phone:",
    //        initialValue: User.Phone);

    //    if (string.IsNullOrWhiteSpace(newPhone))
    //        return;

    //    //await _authService.UpdateProfileAsync(
    //    //    User,
    //    //    User.Email,
    //    //    newPhone);

    //    OnPropertyChanged(nameof(User));
    //}
}
