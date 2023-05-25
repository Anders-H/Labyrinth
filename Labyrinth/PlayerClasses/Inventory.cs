using Labyrinth.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labyrinth.PlayerClasses;

public class Inventory
{
    public const int SlotsWidth = 16;
    public const int SlotsHeight = 10;
    public Thing CurrentThingBeingPointedAt { get; set; }
    public string CurrentDescription { get; set; }
    private Thing[,] Slots { get; } = new Thing[SlotsWidth, SlotsHeight];
    
    public Thing PeekThingAt(int x, int y) =>
        Slots[x, y];
    
    public Thing GetThingAt(int x, int y)
    {
        var ret = Slots[x, y];
        Slots[x, y] = null;
        return ret;
    }
    
    public bool HasThingAt(int x, int y) =>
        Slots[x, y] != null;
    
    public Thing SetThingAt(int x, int y, Thing thing) =>
        Slots[x, y] = thing;
    
    public Thing SetThingAt(Point at, Thing thing) =>
        SetThingAt(at.X, at.Y, thing);
    
    public bool HasFreeSlot() => GetFreeSlot() != null;
    
    public Point? GetFreeSlot()
    {
        for (var y = 0; y < SlotsHeight; y++)
        for (var x = 0; x < SlotsWidth; x++)
            if (Slots[x, y] == null)
                return new Point(x, y);
        return null;
    }
    
    public void Draw(SpriteBatch sb, Texture2D texture)
    {
        for (var y = 0; y < SlotsHeight; y++)
            for (var x = 0; x < SlotsWidth; x++)
            {
                if (Slots[x, y] == null)
                    continue;

                var t = Slots[x, y];
                var destination = new Rectangle((x + 2)*Game1.TileSize, (y + 3)*Game1.TileSize, Game1.TileSize, Game1.TileSize);
                sb.Draw(texture, destination, t.TextureLocation.PhysicalRectangle, Color.White);
            }

    }
}