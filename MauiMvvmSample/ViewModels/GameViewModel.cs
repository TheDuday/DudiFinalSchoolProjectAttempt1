using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMvvmSample.ViewModels
{
    [QueryProperty(nameof(User), "user")]
    [QueryProperty(nameof(GameData), "gameData")]
    public partial class GameViewModel : ObservableObject
    {
        private readonly IGameRepository gameRepository;
        [ObservableProperty]
        private User user;
        [ObservableProperty]
        private GameData gameData;
        public GameViewModel(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
            UpdateLoop();
        }
        private async Task UpdateLoop()
        {
            await Task.Delay(2000);
            gameData = await gameRepository.GetByIdAsync(gameData.GameId);
            if (gameData != null)
            {
                if (gameData.GameEnded)
                {
                    await Task.Delay(3000); //this delay gives time for the other user to get the gameData before it is deleted
                    await gameRepository.DeleteGameAsync(gameData.GameId);
                    GameResult gameResult = gameData.CurrentGameState.checkForWinner();
                    string resultText = "";
                    bool isHost = IsHostPlayer();
                    switch (gameResult)
                    {
                        case GameResult.HostPlayerWon:
                            if (isHost)
                                resultText = "You win!!";
                            else
                                resultText = "Defeat";
                            break;
                        case GameResult.NonHostPlayerWon:
                            if (isHost)
                                resultText = "Defeat";
                            else
                                resultText = "You win!!";
                            break;
                        case GameResult.Draw:
                            resultText = "it's a draw";
                            break;
                    }

                    await Shell.Current.DisplayAlert("Game Result", resultText, "Home");
                    await Shell.Current.GoToAsync("home", new Dictionary<string, object>
                    {
                        { "user", user}
                    });
                }
                else
                {
                    UpdateLoop();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Game Result", "unknown", "Home");
                await Shell.Current.GoToAsync("home", new Dictionary<string, object>
                    {
                        { "user", user}
                    });
            }
        }
        private bool IsHostPlayer()
        {
            return gameData.HostPlayerId == user.Id;
        }
        private async Task MakeMoveAsync(int offsetX, int offsetY, bool firingBullet, int bulletSpeed)
        {
            bool isHost = IsHostPlayer();
            bool hostPlayerTurn = gameData.CurrentGameState.movesPlayed % 2 == 0;

            if (isHost != hostPlayerTurn)
                return;
            GameResult gameResult = gameData.CurrentGameState.playerMove(new Int2(offsetX, offsetY), firingBullet, bulletSpeed, hostPlayerTurn);
            if (gameResult != GameResult.Ongoing)
            {
                GameData.GameEnded = true;
            }
            await gameRepository.UpdateGameAsync(gameData);
        }
        [RelayCommand]
        private async Task MoveRightAsync()
        {
            await MakeMoveAsync(1, 0, false, 0);
        }
        [RelayCommand]
        private async Task MoveLeftAsync()
        {
            await MakeMoveAsync(-1, 0, false, 0);
        }
        [RelayCommand]
        private async Task MoveUpAsync()
        {
            await MakeMoveAsync(0, -1, false, 0);
        }
        [RelayCommand]
        private async Task MoveDownAsync()
        {
            await MakeMoveAsync(0, 1, false, 0);
        }
        [RelayCommand]
        private async Task ShootRightAsync()
        {
            await MakeMoveAsync(1, 0, true, 1);
        }
        [RelayCommand]
        private async Task ShootLeftAsync()
        {
            await MakeMoveAsync(-1, 0, true, 1);
        }
        [RelayCommand]
        private async Task ShootUpAsync()
        {
            await MakeMoveAsync(0, -1, true, 1);
        }
        [RelayCommand]
        private async Task ShootDownAsync()
        {
            await MakeMoveAsync(0, 1, true, 1);
        }
        [RelayCommand]
        private async Task ShootUpRightAsync()
        {
            await MakeMoveAsync(1, -1, true, 1);
        }
        [RelayCommand]
        private async Task ShootUpLeftAsync()
        {
            await MakeMoveAsync(-1, -1, true, 1);
        }
        [RelayCommand]
        private async Task ShootDownRightAsync()
        {
            await MakeMoveAsync(1, 1, true, 1);
        }
        [RelayCommand]
        private async Task ShootDownLeftAsync()
        {
            await MakeMoveAsync(-1, 1, true, 1);
        }
        [RelayCommand]
        private async Task ShootRight2Async()
        {
            await MakeMoveAsync(1, 0, true, 2);
        }
        [RelayCommand]
        private async Task ShootLeft2Async()
        {
            await MakeMoveAsync(-1, 0, true, 2);
        }
        [RelayCommand]
        private async Task ShootUp2Async()
        {
            await MakeMoveAsync(0, -1, true, 2);
        }
        [RelayCommand]
        private async Task ShootDown2Async()
        {
            await MakeMoveAsync(0, 1, true, 2);
        }
        [RelayCommand]
        private async Task ShootUpRight2Async()
        {
            await MakeMoveAsync(1, -1, true, 2);
        }
        [RelayCommand]
        private async Task ShootUpLeft2Async()
        {
            await MakeMoveAsync(-1, -1, true, 2);
        }
        [RelayCommand]
        private async Task ShootDownRight2Async()
        {
            await MakeMoveAsync(1, 1, true, 2);
        }
        [RelayCommand]
        private async Task ShootDownLeft2Async()
        {
            await MakeMoveAsync(-1, 1, true, 2);
        }
    }
}
