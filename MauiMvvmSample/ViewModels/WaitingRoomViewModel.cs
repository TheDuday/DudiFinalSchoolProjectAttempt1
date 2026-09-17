using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class WaitingRoomViewModel : ObservableObject
    {
        private readonly IGameRepository gameRepository;
        [ObservableProperty]
        private User user;
        [ObservableProperty]
        private GameData gameData;
        public WaitingRoomViewModel(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
            checkForJoined();
        }
        public async Task checkForJoined()
        {
            if (user == null || gameData == null)
            {
                await Task.Delay(1000);//wait a second before trying again;
                checkForJoined();
            }
            gameData = await gameRepository.GetByIdAsync(gameData.GameId);
            if (string.IsNullOrEmpty(gameData.NonHostPlayerId))
            {
                await Task.Delay(1000);//wait a second before trying again;
                checkForJoined();
            }
            else
            {
                gameData = await gameRepository.GetByIdAsync(gameData.GameId);
                await Shell.Current.GoToAsync("game", new Dictionary<string, object>
                {
                    { "user", user}, 
                    { "gameData", gameData}
                });
            }
        }
    }
}
