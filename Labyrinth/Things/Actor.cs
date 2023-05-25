using Labyrinth.Labyrinth;

namespace Labyrinth.Things;

public abstract class Actor
{
    public TileRectangle TextureLocation { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public abstract bool IsObstacle { get; }
    public abstract bool CanBePickedUp { get; }
    public abstract string Name { get; }
}