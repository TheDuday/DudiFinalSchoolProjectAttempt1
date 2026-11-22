using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class JoinGameViewModel : ObservableObject
{
    private readonly IGameRepository gameRepository;
    [ObservableProperty]
    private SessionData sessionData;

    [ObservableProperty]
    private string gameCode ="";

    public JoinGameViewModel(IGameRepository gameRepository, SessionData sessionData)
    {
        this.gameRepository = gameRepository;
        this.sessionData = sessionData;
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
        gameData.NonHostPlayerId = sessionData.user.Id;
        gameData.GameStarted = true;
        await gameRepository.UpdateGameAsync(gameData);
        sessionData.gameData = gameData;
        await Shell.Current.GoToAsync("//game");
    }
}