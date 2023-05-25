using Microsoft.Xna.Framework;

namespace Labyrinth.Labyrinth;

public class TileRectangle
{
    public int X { get; set; }
    public int Y { get; set; }

    public TileRectangle() : this(0, 0)
    {
    }

    public TileRectangle(int x, int y)
    {
        X = x; Y = y;
    }

    public Rectangle PhysicalRectangle =>
        new(X * 32, Y * 32, 32, 32);
}