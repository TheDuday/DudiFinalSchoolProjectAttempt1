using SQLite;
using System.Security.Cryptography;

namespace MauiMvvmSample.Models;

public class GameData
{
    public string GameId { get; set; }
    public string HostPlayerId { get; set; }
    public string NonHostPlayerId { get; set; }
    public bool GameStarted { get; set; }
    public bool GameEnded { get; set; }
    public GameState CurrentGameState { get; set; }
    public static string GenerateGameDataId()
    {
        string Id = Guid.NewGuid().ToString().ToLower();
        return Id.Substring(0, 8);
    }
}