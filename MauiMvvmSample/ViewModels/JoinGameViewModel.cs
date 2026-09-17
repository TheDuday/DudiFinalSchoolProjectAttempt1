using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[QueryProperty(nameof(User), "user")]
public partial class JoinGameViewModel : ObservableObject
{
    private readonly IGameRepository gameRepository;
    [ObservableProperty]
    private User user;

    [ObservableProperty]
    private string gameCode ="";

    public JoinGameViewModel(IGameRepository gameRepository)
    {
        this.gameRepository = gameRepository;
    }

    [RelayCommand]
    private async Task JoinGameWithCodeAsync()
    {
        GameData? gameData = await gameRepository.GetByIdAsync(gameCode.ToLower()); //the game code is also the game id
        if (gameData == null)
        {
            await Shell.Current.DisplayAlert("Error", "didn't find game", "OK");
            return;
        }
        gameData.NonHostPlayerId = user.Id;
        gameData.GameStarted = true;
        await gameRepository.UpdateGameAsync(gameData);
        await Shell.Current.GoToAsync("game", new Dictionary<string, object>
        {
            { "user", user},
            { "gameData", gameData}
        });
    }
}