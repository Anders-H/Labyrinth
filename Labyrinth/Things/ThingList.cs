using System.Collections.Generic;
using System.Linq;

namespace Labyrinth.Things;

public class ThingList : List<Thing>
{
    public ThingList GetThingsAt(int x, int y)
    {
        var ret = new ThingList();
        ret.AddRange(this.Where(t => t.GridX == x && t.GridY == y).ToArray());
        return ret;
    }

    public ThingList GetThingsThatCanBePickedUpAt(int x, int y)
    {
        var ret = new ThingList();
        ret.AddRange(this.Where(t => t.GridX == x && t.GridY == y && t.CanBePickedUp).ToArray());
        return ret;
    }

    public void PutThingAt(int x, int y, Thing thing)
    {
        thing.GridX = x;
        thing.GridY = y;
        Add(thing);
    }
}