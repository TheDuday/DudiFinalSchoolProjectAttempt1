using MauiMvvmSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMvvmSample.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task<string> CreateGameAsync(GameState currentGameState, string HostPlayerId); //return game id of created game
        Task<GameData?> GetByIdAsync(string id);
        Task UpdateGameAsync(GameData gameData);
        Task DeleteGameAsync(string gameId);
    }
}
