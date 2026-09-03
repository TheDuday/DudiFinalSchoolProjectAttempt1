public class TileOccupant
{
    
}
public struct Int2
{
    public int x { get; set; }
    public int y { get; set; }
    public Int2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public static Int2 operator +(Int2 a, Int2 b)
    {
        return new Int2(a.x + b.x, a.y + b.y);
    }
    public static Int2 operator -(Int2 a, Int2 b)
    {
        return new Int2(a.x - b.x, a.y - b.y);
    }
    public static Int2 operator *(Int2 a, int b)
    {
        return new Int2(a.x * b, a.y * b);
    }
}
public class Bullet : TileOccupant
{
    public int speed { get; set; }
    public Int2 directionOffset { get; set; }
    public Bullet(Int2 directionOffset, int speed)
    {
        this.directionOffset = directionOffset;
        this.speed = speed;
    }
    
}
public class Player : TileOccupant
{
    public int Health { get; set; }
    public bool isHostingPlayer { get; set; } // Indicates if the player is the one who's hosting the game. if it's a solo game, 'false' will indicate the player is the opponent (computer)
    public Player(int health, bool isHostingPlayer)
    {
        Health = health;
        this.isHostingPlayer = isHostingPlayer;
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
    }
}
public enum TileType
{
    Empty,
    Wall,
    Player,
    Bullet
}
public class GameTile
{
    public TileType Type { get; set; }
    public TileOccupant? Occupant; //a null value means the tile is empty
    public Int2 position;
    public GameTile(TileType type)
    {
        Type = type;
        Occupant = null;
    }
    public GameTile(TileType type, TileOccupant occupant)
    {
        Type = type;
        Occupant = occupant;
    }
    public void SetOccupant(TileOccupant occupant)
    {
        if (occupant is Player p)
        {
            Type = TileType.Player;
        }
        else if (occupant is Bullet)
        {
            Type = TileType.Bullet;
        }
        else if (occupant == null)
        {
            Type = TileType.Empty;
        }
        Occupant = occupant;
    }
    public TileOccupant? RemoveOccupant() //returns the occupant that was removed, or null if there was no occupant
    {
        var occupant = Occupant;
        Occupant = null;
        Type = TileType.Empty;
        return occupant;
    }
}
public class GameBoard
{
    public GameTile[,] Tiles { get; }
    public GameBoard(int width, int height, List<Int2> wallPositions, Int2 hostPlayerPosition, Int2 opponentPlayerPosition, int playerHealth)
    {
        Tiles = new GameTile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new GameTile(TileType.Empty);
            }
        }
        // Set the wall positions
        foreach (var position in wallPositions)
        {
            Tiles[position.x, position.y] = new GameTile(TileType.Wall);
        }
        // Set the player positions
        Player hostPlayer = new Player(playerHealth, true);
        Player opponentPlayer = new Player(playerHealth, false);
        Tiles[hostPlayerPosition.x, hostPlayerPosition.y] = new GameTile(TileType.Player, hostPlayer);
        Tiles[opponentPlayerPosition.x, opponentPlayerPosition.y] = new GameTile(TileType.Player, opponentPlayer);
    }
}
public class GameState
{
    public int BulletDamage { get; set; }
    public GameBoard Board { get; set; }
    public void moveBullets()
    {
        foreach (GameTile tile in Board.Tiles)
        {
            if (tile.Occupant is Bullet bullet)
            {
                Int2 stepOffset = bullet.directionOffset;
                Int2 currentPosition = tile.position;
                bool bulletRemoved = false;
                for (int step = 0; step < bullet.speed; step++)
                {
                    if (!bulletRemoved)
                    {
                        currentPosition += stepOffset;
                        if (currentPosition.x < 0 || currentPosition.x >= Board.Tiles.GetLength(0) || currentPosition.y < 0 || currentPosition.y >= Board.Tiles.GetLength(1))
                        {
                            // Bullet has moved off the board
                            tile.RemoveOccupant();
                            bulletRemoved = true;
                        }
                        if (Board.Tiles[currentPosition.x, currentPosition.y].Type == TileType.Wall)
                        {
                            // Bullet has hit a wall
                            tile.RemoveOccupant();
                            bulletRemoved = true;
                        }
                        if (Board.Tiles[currentPosition.x, currentPosition.y].Occupant is Player player)
                        {
                            // Bullet has hit a player
                            player.TakeDamage(BulletDamage);
                            if (player.Health <= 0)
                            {
                                // Player has died
                                Board.Tiles[currentPosition.x, currentPosition.y].RemoveOccupant();
                            }
                            tile.RemoveOccupant();
                            bulletRemoved = true;
                        }
                    }
                }
                if (!bulletRemoved)
                {
                    // Move the bullet to the new position
                    Board.Tiles[currentPosition.x, currentPosition.y].SetOccupant(bullet);
                    tile.RemoveOccupant(); bulletRemoved = true;
                }
            }
        }
    }
    public int checkForWinner() //return 1 if hosting player won, 2 if opponent player won, 3if it's a draw, 0 if no one won yet
    {
        bool hostPlayerAlive = false;
        bool opponentPlayerAlive = false;
        foreach (GameTile tile in Board.Tiles)
        {
            if (tile.Occupant is Player player)
            {
                if (player.isHostingPlayer)
                {
                    hostPlayerAlive = true;
                }
                else
                {
                    opponentPlayerAlive = true;
                }
            }
        }
        if (hostPlayerAlive && opponentPlayerAlive)
        {
            return 0; // No one won yet
        }
        else if (hostPlayerAlive)
        {
            return 1; // Hosting player won
        }
        else if (opponentPlayerAlive)
        {
            return 2; // Opponent player won
        }
        else
        {
            return 3; // It's a draw
        }
    }


    //if firingBullet -> create a bullet in the direction "directionOffset", with speed "bulletSpeed"
    //if !firingBullet -> just move the player in the direction "directionOffset"
    public int playerMove(Int2 directionOffset, bool firingBullet, int bulletSpeed, bool hostPlayerTurn) //return 1 if hosting player won, 2 if opponent player won, 3 if it's a draw, 0 if no one won yet
    {
        GameTile movingPlayerTile = null;
        foreach (GameTile tile in Board.Tiles)
        {
            if (tile.Occupant is Player player)
            {
                if (player.isHostingPlayer == hostPlayerTurn)
                {
                    movingPlayerTile = tile;
                    break;
                }
            }
        }
        if (movingPlayerTile == null) //we assume there is always a player for the current turn, meaning this shouldn't happen, so this is for debugging
        {
            throw new Exception("No player found for the current turn.");
        }
        Int2 targetTilePosition = movingPlayerTile.position + directionOffset;

        //if we move into a bullet, the bullet is supposed to move first, and then we move into the tile that the bullet was in, so we don't want to move the player into the bullet's tile until the bullet has been moved
        //we do this because if we move the bullets first, and we have a bullet moving into a player, we have a problem with assigning the player spot to the bullet because the player is still there
        bool makeMoveAfterBulletsMove = false; 
        if (firingBullet)
        {
            Bullet newBullet = new Bullet(directionOffset, bulletSpeed);
            if (targetTilePosition.x >= 0 && targetTilePosition.x < Board.Tiles.GetLength(0) && targetTilePosition.y >= 0 && targetTilePosition.y < Board.Tiles.GetLength(1))
            {
                if (Board.Tiles[targetTilePosition.x, targetTilePosition.y].Type == TileType.Empty)
                {
                    Board.Tiles[targetTilePosition.x, targetTilePosition.y].SetOccupant(newBullet);
                }
            }
        }
        else
        {
            if (targetTilePosition.x >= 0 && targetTilePosition.x < Board.Tiles.GetLength(0) && targetTilePosition.y >= 0 && targetTilePosition.y < Board.Tiles.GetLength(1))
            {
                if (Board.Tiles[targetTilePosition.x, targetTilePosition.y].Type == TileType.Empty)
                {
                    Board.Tiles[targetTilePosition.x, targetTilePosition.y].SetOccupant(movingPlayerTile.Occupant);
                    movingPlayerTile.RemoveOccupant();
                }
            }
        }
        moveBullets();
        return checkForWinner();
    }
}