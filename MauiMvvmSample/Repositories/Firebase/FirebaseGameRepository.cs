using Firebase.Auth;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using MauiMvvmSample.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MauiMvvmSample.Repositories
{
    internal class FirebaseGameRepository : IGameRepository
    {
        private const string ProjectId = "finalschoolprojectattempt1fb";
        private const string ApiKey =
           "AIzaSyCqflpIy3cmIdXXgFbuA3ii4ucZOZa_9JM";
        private readonly HttpClient _http = new();

        public FirebaseGameRepository(HttpClient http)
        {
            _http = http;
        }
        public async Task<string> CreateGameAsync(GameState currentGameState, string HostPlayerId) //returns created gameId
        {
            string Id = GameData.GenerateGameDataId();

            var url =
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}" +
            $"/databases/(default)/documents/games/{Id}";

            var document = new
            {
                fields = new
                {
                    gameString = new { stringValue = GameStateStringHelper.GameStateToString(currentGameState) },
                    hostPlayerId = new { stringValue = HostPlayerId },
                    nonHostPlayerId = new { stringValue = "" },
                    gameStarted = new { booleanValue = false },
                    gameEnded = new { booleanValue = false }
                }
            };

            var response = await _http.PatchAsJsonAsync(url, document);

            Debug.WriteLine(response);

            response.EnsureSuccessStatusCode();

            return Id;
        }
        public async Task<GameData?> GetByIdAsync(string id)
        {
            var url =
                $"https://firestore.googleapis.com/v1/projects/{ProjectId}" +
                $"/databases/(default)/documents/games/{id}";

            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var document =
                await response.Content.ReadFromJsonAsync<FireStoreGameDocument>();

            if (document?.Fields == null)
                return null;

            GameState currentGameState = GameStateStringHelper.StringToGameState(document.Fields.GameString.StringValue);

            return new GameData
            {
                GameId = id,
                CurrentGameState = currentGameState,
                HostPlayerId = document.Fields.HostPlayerId.StringValue,
                NonHostPlayerId = document.Fields.NonHostPlayerId.StringValue,
                GameStarted = (bool)document.Fields.GameStarted.BooleanValue,
                GameEnded = (bool)document.Fields.GameEnded.BooleanValue
            };
        }
        public async Task UpdateGameAsync(GameData gameData)
        {
            var url =
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}" +
            $"/databases/(default)/documents/games/{gameData.GameId}";

            var document = new
            {
                fields = new
                {
                    gameString = new { stringValue = GameStateStringHelper.GameStateToString(gameData.CurrentGameState) },
                    hostPlayerId = new { stringValue = gameData.HostPlayerId },
                    nonHostPlayerId = new { stringValue = gameData.NonHostPlayerId },
                    gameStarted = new { booleanValue = gameData.GameStarted },
                    gameEnded = new { booleanValue = gameData.GameEnded }
                }
            };

            var response = await _http.PatchAsJsonAsync(url, document);

            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteGameAsync(string gameId)
        {
            var url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}" +
            $"/databases/(default)/documents/games/{gameId}";

            var response = await _http.DeleteAsync(url);

            response.EnsureSuccessStatusCode();
        }
    }
}
