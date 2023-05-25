using Labyrinth.Labyrinth;

namespace Labyrinth.Things;

public class Rock : Thing
{
    public override bool IsObstacle => false;
    public override bool CanBePickedUp => true;
    public override string Name => "Rock";
    public Rock()
    {
        switch (Labyrinth.Labyrinth.Random.Next(2))
        {
            case 0:
                TextureLocation = new TileRectangle(8, 11);
                break;
            case 1:
                TextureLocation = new TileRectangle(9, 11);
                break;
        }
    }
}