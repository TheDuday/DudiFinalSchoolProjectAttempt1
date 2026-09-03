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
    public int timesMoved { get; set; } = 0;
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
    Wall
}
public class GameTile
{
    public TileType type { get; set; }
    public List<TileOccupant> Occupants;
    public Int2 position;
    public GameTile(TileType type)
    {
        this.type = type;
        Occupants = new List<TileOccupant>();
    }
    public GameTile(TileType type, TileOccupant occupant)
    {
        this.type = type;
        Occupants = new List<TileOccupant> { occupant };
    }
    public void AddOccupant(TileOccupant occupant)
    {
        Occupants.Add(occupant);
    }
    public void RemoveOccupants()
    {
        Occupants.Clear();
    }
    public void handleCollisions(int bulletDamage)
    {
        if (type == TileType.Wall)
        {
            RemoveOccupants();
        }
        else
        {
            var playerOccupants = Occupants.Where(o => o is Player);
            if (playerOccupants.Count() > 0)
            {
                if (playerOccupants.Count() > 1)
                {
                    RemoveOccupants(); //both players die if they occupy the same tile
                }
                //now we know we have exactly one player
                else
                {
                    var player = ((Player)playerOccupants.First());
                    for (int i = Occupants.Count - 1; i >= 0; i--)
                    {
                        if (Occupants[i] is Bullet bullet)
                        {
                            Occupants.Remove(bullet);
                            player.TakeDamage(bulletDamage);
                            if (player.Health <= 0)
                            {
                                Occupants.Remove(player);
                            }
                        }
                    }
                }
            }
        }
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
            Tiles[position.x, position.y].type = TileType.Wall;
        }
        // Set the player positions
        Player hostPlayer = new Player(playerHealth, true);
        Player opponentPlayer = new Player(playerHealth, false);
        Tiles[hostPlayerPosition.x, hostPlayerPosition.y].Occupants.Add(hostPlayer);
        Tiles[opponentPlayerPosition.x, opponentPlayerPosition.y].Occupants.Add(opponentPlayer);
    }
}
public class GameState
{
    public int BulletDamage { get; set; }
    public GameBoard Board { get; set; }
    public bool IsInBounds(Int2 position)
    {
        return position.x >= 0 && position.x < Board.Tiles.GetLength(0) && position.y >= 0 && position.y < Board.Tiles.GetLength(1);
    }
    public void resetBulletCounters()
    {
        foreach (GameTile tile in Board.Tiles)
        {
            foreach (TileOccupant occupant in tile.Occupants)
            {
                if (occupant is Bullet bullet)
                {
                    bullet.timesMoved = 0;
                }
            }
        }
    }
    public void handleCollisions()
    {
        foreach (GameTile tile in Board.Tiles)
        {
            tile.handleCollisions(BulletDamage);
        }
    }
    public void moveBulletsOneStep()
    {
        foreach (GameTile tile in Board.Tiles)
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (TileOccupant occupant in tile.Occupants)
            {
                if (occupant is Bullet bullet)
                {
                    if (bullet.timesMoved >= bullet.speed)
                    {
                        continue; // Skip this bullet if we already moved it the amount needed for this turn
                    }
                    Int2 targetTilePosition = tile.position + bullet.directionOffset;
                    if (IsInBounds(targetTilePosition))
                    {
                        GameTile targetTile = Board.Tiles[targetTilePosition.x, targetTilePosition.y];
                        targetTile.AddOccupant(bullet);
                        bulletsToRemove.Add(bullet);
                        bullet.timesMoved++;
                    }
                    else
                    {
                        bulletsToRemove.Add(bullet); // Bullet goes out of bounds, remove it
                    }
                }
            }
            foreach (var bullet in bulletsToRemove)
            {
                tile.Occupants.Remove(bullet);
            }
        }
    }
    public void moveBullets()
    {
        resetBulletCounters();
        for (int i = 0; i < Math.Max(Board.Tiles.GetLength(0), Board.Tiles.GetLength(1)); i++) //a bullet can at most move the length of the board in one turn, so we move the bullets that many times to ensure all bullets have moved the amount they need to for this turn
        {
            moveBulletsOneStep();
            handleCollisions(); //handle collisions after each step of bullet movement to ensure that bullets that collide with players or walls are removed before they can move further
        }
    }
    public int checkForWinner() //return 1 if hosting player won, 2 if opponent player won, 3if it's a draw, 0 if no one won yet
    {
        bool hostPlayerAlive = false;
        bool opponentPlayerAlive = false;
        foreach (GameTile tile in Board.Tiles)
        {
            foreach (var occupant in tile.Occupants)
            {
                if (occupant is Player player)
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
            var tilePlayers = tile.Occupants.Where(occupant => occupant is Player player);
            if (tilePlayers.Count() > 0)
            {
                if (((Player)tilePlayers.First()).isHostingPlayer == hostPlayerTurn)
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
        if (firingBullet)
        {
            Bullet newBullet = new Bullet(directionOffset, bulletSpeed);
            movingPlayerTile.AddOccupant(newBullet);
        }
        else
        {
            if (IsInBounds(targetTilePosition))
            {
                GameTile targetTile = Board.Tiles[targetTilePosition.x, targetTilePosition.y];
                var player = movingPlayerTile.Occupants.First(o => o is Player);
                movingPlayerTile.Occupants.Remove(player);
                targetTile.AddOccupant(player);
            }
        }
        moveBullets();
        return checkForWinner();
    }
}