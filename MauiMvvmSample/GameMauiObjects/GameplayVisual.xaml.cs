namespace MauiMvvmSample.GameMauiObjects;

using Microsoft.Maui.Graphics.Platform;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;
public partial class GameplayVisual : ContentView
{
    public enum GameDrawableType
    {
        ground, 
        wall, 
        host_player_l,
        host_player_r,
        non_host_player_l,
        non_host_player_r,
        bullet_u, 
        bullet_ur,
        bullet_r,
        bullet_dr,
        bullet_d,
        bullet_dl,
        bullet_l,
        bullet_ul
    }
    public async Task LoadImages()
    {
        var drawableImages = ((MyDrawable)gameplayGraphicsView.Drawable).drawableToImage;
        GameDrawableType[] drawableTypes = (GameDrawableType[])Enum.GetValues(typeof(GameDrawableType));
        foreach (var drawableType in drawableTypes)
        {
            string filename = $"{drawableType}.png";
            var image = await LoadAppImageAsync(filename);
            drawableImages[drawableType] = image;
        }
        ((MyDrawable)gameplayGraphicsView.Drawable).imagesLoaded = true;
    }
	public GameplayVisual()
	{
		InitializeComponent();
        gameplayGraphicsView.Drawable = new MyDrawable(gameplayGraphicsView);
        LoadImages();
    }
    public class MyDrawable : IDrawable
    {
        public Dictionary<GameDrawableType, IImage> drawableToImage = new Dictionary<GameDrawableType, IImage>();
        public bool imagesLoaded = false;
        public GraphicsView parent;
        public GameState currentGameState { get; set; }
        public async Task tryDrawAgainLater(ICanvas canvas, RectF dirtyRect)
        {
            await Task.Delay(500);
            parent.Invalidate();
        }
        public MyDrawable(GraphicsView parent)
        {
            this.parent = parent;
        }
        public void Draw(ICanvas canvas, RectF dirtyRect) //we assume the drawing area is a square meaning width and height of the drawing area are the same
        {
            if (!imagesLoaded)
            {
                tryDrawAgainLater(canvas, dirtyRect);
                return;
            }
            
            //sky color
            canvas.FillColor = Colors.LightCyan;
            canvas.FillRectangle(dirtyRect);

            float imageHeightWidthRatio = 1.5f; //each tile images will have 3 stacked sections, each with the height of half the width, so in total we get a height of 3 * (1/2) * width, the non middle section act as extentions to the tile, making it possible to draw high walls and things below the floor

            //our grid is drawn diagonally, meaning we need to calculate the width of it once drawn, and each positive step horizontally or vertically in the original grid means moving 1/2 the width of a tile in the drawn grid.
            //so to get the width of each tile needed to maximize the drawn area, we get the amount of steps, with that we get the width/2, and then we multiply by 2
            float imageWidths = dirtyRect.Width / (currentGameState.Board.Tiles.GetLength(0) + currentGameState.Board.Tiles.GetLength(1)) * 2;
            float imageHeights = imageWidths * imageHeightWidthRatio;
            Int2 XPosStep = new Int2((int)(imageWidths / 2), (int)(imageWidths / 4));
            Int2 YPosStep = new Int2((int)(-imageWidths / 2), (int)(imageWidths / 4));
            float totalWidth = (currentGameState.Board.Tiles.GetLength(0) + currentGameState.Board.Tiles.GetLength(1)) * XPosStep.x;
            float totalHeight = totalWidth / 2;

            for (int x = 0; x < currentGameState.Board.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < currentGameState.Board.Tiles.GetLength(1); y++)
                {
                    Int2 drawPos = new Int2(XPosStep.x * currentGameState.Board.Tiles.GetLength(1), (int)((totalWidth - totalHeight) / 2));
                    drawPos += XPosStep * x + YPosStep * y; //now we are at the top of the wanted tile
                    drawPos += new Int2(-(int)(imageWidths / 2), -(int)(imageWidths / 2));
                    List<IImage> images = WhatToDraw_Ordered(currentGameState.Board.Tiles[x, y]);
                    foreach (IImage image in images)
                    {
                        canvas.DrawImage(image, drawPos.x, drawPos.y, imageWidths, imageHeights);
                    }
                }
            }
        }
        public List<IImage> WhatToDraw_Ordered(GameTile tile)
        {
            List<IImage> result = new List<IImage>();
            GameDrawableType terrainType = tile.type == TileTerrainType.Empty ? GameDrawableType.ground : GameDrawableType.wall;
            IImage terrainImage = drawableToImage[terrainType];
            result.Add(terrainImage);
            foreach (TileOccupant occupant in tile.Occupants)
            {
                if (occupant is Player p)
                {
                    if (p.isHostingPlayer)
                    {
                        if (p.facingRight)
                            result.Add(drawableToImage[GameDrawableType.host_player_r]);
                        else
                            result.Add(drawableToImage[GameDrawableType.host_player_l]);
                    }
                    else
                    {
                        if (p.facingRight)
                            result.Add(drawableToImage[GameDrawableType.non_host_player_r]);
                        else
                            result.Add(drawableToImage[GameDrawableType.non_host_player_l]);
                    }
                }
                else if (occupant is Bullet b)
                {
                    int dirX = b.directionOffset.x;
                    int dirY = b.directionOffset.y;
                    if (dirX > 0)
                    {
                        if (dirY > 0)
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_dr]);
                        }
                        else if (dirY == 0)
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_r]);
                        }
                        else
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_ur]);
                        }
                    }
                    else if (dirX == 0)
                    {
                        if (dirY > 0)
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_d]);
                        }
                        else if (dirY == 0)
                        {
                            throw new Exception("Bullet cannot have direction components both be zero");
                        }
                        else
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_u]);
                        }
                    }
                    else
                    {
                        if (dirY > 0)
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_dl]);
                        }
                        else if (dirY == 0)
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_l]);
                        }
                        else
                        {
                            result.Add(drawableToImage[GameDrawableType.bullet_ul]);
                        }
                    }
                }
            }
            return result;
        }
    }
    public void setGameState(GameState state)
    {
        ((MyDrawable)gameplayGraphicsView.Drawable).currentGameState = state;
    }
    public GameResult ApplyMove(Int2 directionOffset, bool firingBullet, int bulletSpeed, bool hostPlayerTurn)
    {
        return ((MyDrawable)gameplayGraphicsView.Drawable).currentGameState.playerMove(directionOffset, firingBullet, bulletSpeed, hostPlayerTurn);
    }
    public void DrawGameState()
    {
        gameplayGraphicsView.Invalidate();
    }
    public static async Task<IImage> LoadAppImageAsync(string filename)
    {
        using Stream stream = await FileSystem.OpenAppPackageFileAsync(filename);
        return PlatformImage.FromStream(stream);
    }
}