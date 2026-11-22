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
    public partial class GameViewModel : ObservableObject
    {
        private readonly IGameRepository gameRepository;
        [ObservableProperty]
        private SessionData sessionData;
        public GameViewModel(IGameRepository gameRepository, SessionData sessionData)
        {
            this.gameRepository = gameRepository;
            this.sessionData = sessionData;

            UpdateLoop();
        }
        private async Task UpdateLoop()
        {
            await Task.Delay(1000);
            sessionData.gameData = await gameRepository.GetByIdAsync(sessionData.gameData.GameId);
            if (sessionData.gameData != null)
            {
                if (sessionData.gameData.GameEnded)
                {
                    await Task.Delay(2000); //this delay gives time for the other user to get the gameData before it is deleted
                    await gameRepository.DeleteGameAsync(sessionData.gameData.GameId);
                    GameResult gameResult = sessionData.gameData.CurrentGameState.checkForWinner();
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

                    gameEndedNormally = true;

                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    UpdateLoop();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Game Result", "unknown", "Home");
                await Shell.Current.GoToAsync("//home");
            }
        }
        private bool IsHostPlayer()
        {
            return sessionData.gameData.HostPlayerId == sessionData.user.Id;
        }
        private async Task MakeMoveAsync(int offsetX, int offsetY, bool firingBullet, int bulletSpeed)
        {
            bool isHost = IsHostPlayer();
            bool hostPlayerTurn = sessionData.gameData.CurrentGameState.movesPlayed % 2 == 0;

            if (isHost != hostPlayerTurn)
                return;
            GameResult gameResult = sessionData.gameData.CurrentGameState.playerMove(new Int2(offsetX, offsetY), firingBullet, bulletSpeed, hostPlayerTurn);
            if (gameResult != GameResult.Ongoing)
            {
                sessionData.gameData.GameEnded = true;
            }
            await gameRepository.UpdateGameAsync(sessionData.gameData);
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

        private bool gameEndedNormally = false;
        public async Task OnDisappearingAsync() //when the page is closed, we make sure the game is ended with the other player winning
        {
            foreach (var tile in sessionData.gameData.CurrentGameState.Board.Tiles)
            {
                if (tile.Occupants.Where(o => o is Player p && p.isHostingPlayer == IsHostPlayer()).Any())
                {
                    tile.RemoveOccupants();
                }
            }
            sessionData.gameData.GameEnded = true;
            if (sessionData.gameData != null && !gameEndedNormally)
            {
                await gameRepository.UpdateGameAsync(sessionData.gameData);
            }
        }
    }
}
