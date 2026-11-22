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


namespace MauiMvvmSample.ViewModels
{

    public partial class HostGameSettingsViewModel : ObservableObject
    {
        private readonly IGameRepository _gameRepository;
        [ObservableProperty]
        private SessionData sessionData;

        [ObservableProperty]
        private string heightString = string.Empty;

        [ObservableProperty]
        private string widthString = string.Empty;

        [ObservableProperty]
        private string wallAmountString = string.Empty;

        [ObservableProperty]
        private string playerHealthString = string.Empty;
        public HostGameSettingsViewModel(IGameRepository gameRepository, SessionData sessionData)
        {
            _gameRepository = gameRepository;
            this.sessionData = sessionData;
        }


        [RelayCommand]
        private async Task CreateGameAsync()
        {
            try
            {
                int height = int.Parse(heightString);
                int width = int.Parse(widthString);
                int wallAmount = int.Parse(wallAmountString);
                int playerHealth = int.Parse(playerHealthString);
                if (wallAmount > width * height / 2 || width > 10 || height > 10 || playerHealth > 10
                    || wallAmount < 0 || width < 2 || height < 2 || playerHealth < 1)
                {
                    await Shell.Current.DisplayAlert("Error", "invalid values, enter values in range", "OK");
                    HeightString = string.Empty;
                    WidthString = string.Empty;
                    WallAmountString = string.Empty;
                    PlayerHealthString = string.Empty;
                }
                else
                {
                    GameSettings settings = new GameSettings(wallAmount, width, height, playerHealth);
                    GameState startingGameState = settings.createGame();
                    string gameId = await _gameRepository.CreateGameAsync(startingGameState, sessionData.user.Id);
                    GameData gameData = await _gameRepository.GetByIdAsync(gameId);

                    if (gameData == null)
                    {
                        throw new Exception("firebase didn't return object that it has when asked for");
                    }

                    sessionData.gameData = gameData;

                    await Shell.Current.GoToAsync("//waitingRoom");
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "invalid values, enter whole numbers", "OK");
            }
        }
    }
}
