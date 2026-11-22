using CommunityToolkit.Mvvm.ComponentModel;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MauiMvvmSample.ViewModels
{
    public partial class WaitingRoomViewModel : ObservableObject
    {
        private readonly IGameRepository gameRepository;
        [ObservableProperty]
        private SessionData sessionData;
        public WaitingRoomViewModel(IGameRepository gameRepository, SessionData sessionData)
        {
            this.gameRepository = gameRepository;
            this.sessionData = sessionData;
            checkForJoined();
        }
        public async Task checkForJoined()
        {
            await Task.Delay(1000);//wait a second
            if (Shell.Current?.CurrentPage.GetType() != typeof(WaitingRoomPage)) //if we are in a different page, stop
                return;
            if (SessionData.user == null || SessionData.gameData == null)
            {
                checkForJoined();
                return;
            }
            SessionData.gameData = await gameRepository.GetByIdAsync(SessionData.gameData.GameId);
            if (string.IsNullOrEmpty(SessionData.gameData.NonHostPlayerId))
            {
                await Task.Delay(1000);//wait a second before trying again;
                checkForJoined();
            }
            else
            {
                await Shell.Current.GoToAsync("//game");
            }
        }
        public async Task OnNavigatedFromAsync() //if the host navigates out of the game (not into gamePage)
        {
            if (Shell.Current?.CurrentPage.GetType() != typeof(GamePage))
                await gameRepository.DeleteGameAsync(SessionData.gameData.GameId);
        }
    }
}
