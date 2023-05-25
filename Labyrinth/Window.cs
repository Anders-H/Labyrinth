using Labyrinth.Labyrinth;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labyrinth;

public class Window
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }
    private int TextX { get; set; }
    private int TextY { get; set; }
    private Color OverlayColor { get; }
    private readonly TileRectangle[,] _tiles;
    public Window(int xpos, int ypos, int width, int height)
    {
        X = xpos;
        Y = ypos;
        Width = width;
        Height = height;
        OverlayColor = Color.FromNonPremultiplied(255, 255, 255, 160);
        _tiles = new TileRectangle[width, height];
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                _tiles[x, y] = new TileRectangle(global::Labyrinth.Labyrinth.Labyrinth.Random.Next(4, 8), 18);
            else
                _tiles[x, y] = new TileRectangle(4, 1);
    }
    public void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        TextX = ((X + 1) * 32) + 4;
        TextY = ((Y + 1) * 32);
        var blackTile = new Rectangle(4 * 32, 1 * 32, 32, 32);
        var c = Color.FromNonPremultiplied(255, 255, 255, 127);
        for (var y = 0; y < 15; y++)
        for (var x = 0; x < 20; x++)
            spriteBatch.Draw(texture, new Rectangle(x * 32, y * 32, 32, 32), blackTile, c);
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            spriteBatch.Draw(texture, new Rectangle((X + x)*32, (Y + y)*32, 32, 32), _tiles[x, y].PhysicalRectangle, OverlayColor);
    }
    public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, string text, bool blankAfter)
    {
        spriteBatch.DrawString(spriteFont, text ?? "", new Vector2(TextX, TextY), Color.White);
        TextY += 14;
        if (blankAfter)
            TextY += 14;
    }
}