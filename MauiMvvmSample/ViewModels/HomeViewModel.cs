using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Models;
using MauiMvvmSample.Services;

namespace MauiMvvmSample.ViewModels;

[QueryProperty(nameof(User), "user")]
public partial class HomeViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public HomeViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    private User user;

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
