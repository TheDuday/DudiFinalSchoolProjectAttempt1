using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiMvvmSample
{
    public static class GameStateStringHelper
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static string GameStateToString(GameState state)
        {
            GameStateWithJaggedArray convertedState = new GameStateWithJaggedArray(state);
            return JsonConvert.SerializeObject(convertedState, settings);
        }

        public static GameState? StringToGameState(string convertedStateString)
        {
            var convertedState = JsonConvert.DeserializeObject<GameStateWithJaggedArray>(convertedStateString, settings);
            return convertedState.ToGameState();
        }
    }
    //all kinds of conversion to and from string that I tried don't work with 2d arrays so these classes have a jagged array (array array) instead, because that will work.
    public class GameStateWithJaggedArray
    {
        public int movesPlayed { get; set; }
        public int BulletDamage { get; set; }
        public GameBoardWithJaggedArray Board { get; set; }
        public GameStateWithJaggedArray() { }
        public GameStateWithJaggedArray(GameState state)
        {
            this.movesPlayed = state.movesPlayed;
            this.BulletDamage = state.BulletDamage;
            this.Board = new GameBoardWithJaggedArray(state.Board);
        }
        public GameState ToGameState()
        {
            GameState result = new GameState();
            result.BulletDamage = BulletDamage;
            result.movesPlayed = movesPlayed;
            result.Board = Board.ToGameBoard();
            return result;
        }
    }
    public class GameBoardWithJaggedArray
    {
        public GameBoardWithJaggedArray() { }
        public GameTile[][] Tiles { get; set; }
        public GameBoardWithJaggedArray(GameBoard gameBoard)
        {
            Tiles = new GameTile[gameBoard.Tiles.GetLength(0)][];
            for (int i = 0; i < gameBoard.Tiles.GetLength(0); i++)
            {
                Tiles[i] = new GameTile[gameBoard.Tiles.GetLength(1)];
                for (int j = 0; j < gameBoard.Tiles.GetLength(1); j++)
                {
                    Tiles[i][j] = gameBoard.Tiles[i, j];
                }
            }
        }
        public GameBoard ToGameBoard()
        {
            GameBoard result = new GameBoard();
            result.Tiles = new GameTile[Tiles.Length, Tiles[0].Length];
            for (int i = 0; i < Tiles.Length; i++)
            {
                for (int j = 0; j < Tiles[0].Length; j++)
                {
                    result.Tiles[i, j] = Tiles[i][j];
                }
            }
            return result;
        }
    }
}
