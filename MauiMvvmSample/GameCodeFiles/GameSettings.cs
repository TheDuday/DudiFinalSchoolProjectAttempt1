using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameSettings
{
    public int wallAmount;
    public int width;
    public int height;
    public int maxHealth;
    public Int2 hostPlayerPos;
    public Int2 nonHostPlayerPos;
    public int bulletDamage;
    public GameSettings(int wallAmount, int width, int height, int maxHealth)
    {
        this.wallAmount = wallAmount;
        this.width = width;
        this.height = height;
        hostPlayerPos = new Int2(0, 0);
        nonHostPlayerPos = new Int2(width - 1, height - 1);
        this.maxHealth = maxHealth;
        this.bulletDamage = 1;
    }
    public GameState createGame()
    {
        GameState result = new GameState();
        result.movesPlayed = 0;
        result.BulletDamage = bulletDamage;

        List<Int2> wallPos;
        do
        {
            wallPos = createRandomWallPos();
        } while (!canPlayersReachEachother(width, height, hostPlayerPos, nonHostPlayerPos, wallPos));

        GameBoard gameBoard = new GameBoard(width, height, wallPos, hostPlayerPos, nonHostPlayerPos, maxHealth);
        result.Board = gameBoard;
        return result;
    }
    public bool canPlayersReachEachother(int width, int height, Int2 hostPlayerPos, Int2 nonHostPlayerPos, List<Int2> wallPos)
    {
        //bool array is intialized with false values
        bool[,] startingInfo = new bool[width, height];
        startingInfo[nonHostPlayerPos.x, nonHostPlayerPos.y] = true;
        return PosThatCanReachPoint(startingInfo, nonHostPlayerPos, wallPos)[hostPlayerPos.x, hostPlayerPos.y];
    }
    private bool[,] PosThatCanReachPoint(bool[,] previousInformation, Int2 point, List<Int2> forbiddenPos)
    {
        bool[,] changedInformation = (bool[,])previousInformation.Clone();
        for (int i = 0; i < changedInformation.GetLength(0); i++)
        {
            for (int j = 0; j < changedInformation.GetLength(1); j++)
            {
                if (previousInformation[i, j])
                {
                    //players can only reach eachother in moves of up, down, left, right
                    if (!forbiddenPos.Contains(new Int2(i + 1, j)) && isInRange(i + 1, j))
                        changedInformation[i + 1, j] = true;
                    if (!forbiddenPos.Contains(new Int2(i - 1, j)) && isInRange(i - 1, j))
                        changedInformation[i - 1, j] = true;
                    if (!forbiddenPos.Contains(new Int2(i, j + 1)) && isInRange(i, j + 1))
                        changedInformation[i, j + 1] = true;
                    if (!forbiddenPos.Contains(new Int2(i, j - 1)) && isInRange(i, j - 1))
                        changedInformation[i, j - 1] = true;
                }
            }
        }
        bool changedAnything = false;
        for (int i = 0; i < previousInformation.GetLength(0); i++)
        {
            for (int j = 0; j < previousInformation.GetLength(1); j++)
            {
                if (previousInformation[i, j] != changedInformation[i, j]) changedAnything = true;
            }
        }

        if (changedAnything)
            return PosThatCanReachPoint(changedInformation, point, forbiddenPos);
        else
            return changedInformation;
    }
    public List<Int2> createRandomWallPos()
    {
        List<Int2> wallPos = new List<Int2>();
        Random rand = new Random();
        for (int i = 0; i < wallAmount; i++)
        {
            Int2 randPos = new Int2(rand.Next(width), rand.Next(height));
            if (wallPos.Contains(randPos) || randPos == hostPlayerPos || randPos == nonHostPlayerPos)
            {
                i--;
                continue;
            }
            wallPos.Add(randPos);
        }
        return wallPos;
    }
    public bool isInRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}