using Labyrinth.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labyrinth.Labyrinth;

public class CurrentCellMatrix
{
    private LabyrinthCell[,] _currentCells;
    public LabyrinthCell CenterCell => _currentCells[1, 1];
    public LabyrinthCell LeftCell => _currentCells[0, 1];
    public LabyrinthCell RightCell => _currentCells[2, 1];
    public LabyrinthCell TopCell => _currentCells[1, 0];
    public LabyrinthCell BottomCell => _currentCells[1, 2];

    public void Draw(int cameraOffsetX, int cameraOffsetY, SpriteBatch sb, Texture2D texture)
    {
        var x = cameraOffsetX;
        var y = cameraOffsetY;
        for (var cellY = 0; cellY < 3; cellY++)
        {
            for (var cellX = 0; cellX < 3; cellX++)
            {
                _currentCells[cellX, cellY].DrawCellTiles(sb, texture, x, y, _currentCells[cellX, cellY].VisitedOrder > 0 ? Color.White : Color.Gray);
                _currentCells[cellX, cellY].DrawThings(sb, texture, x, y, _currentCells[cellX, cellY].VisitedOrder > 0 ? Color.White : Color.Gray);
                x += Game1.CellSize;
            }

            x = cameraOffsetX;
            y += Game1.CellSize;
        }
    }

    public void SetCells(LabyrinthCell[,] surroundingCells) =>
        _currentCells = surroundingCells;

    public void PutThingAt(int x, int y, Thing thing)
    {
        if (x >= 0 && y >= 0 && x < LabyrinthCell.TilesWidth && y < LabyrinthCell.TilesHeight)
            _currentCells[1, 1].Things.PutThingAt(x, y, thing);
        else if (x < 0 && y < 0) //Top left.
            _currentCells[0, 0].Things.PutThingAt(x + LabyrinthCell.TilesWidth, y + LabyrinthCell.TilesHeight, thing);
        else if (x < 0 && y >= LabyrinthCell.TilesHeight) // Bottom left
            _currentCells[0, 2].Things.PutThingAt(x + LabyrinthCell.TilesWidth, y - LabyrinthCell.TilesHeight, thing);
        else if (x < 0) // Left
            _currentCells[0, 1].Things.PutThingAt(x + LabyrinthCell.TilesWidth, y, thing);
        else if (x >= LabyrinthCell.TilesWidth && y < 0) //Top right.
            _currentCells[2, 0].Things.PutThingAt(x - LabyrinthCell.TilesWidth, y + LabyrinthCell.TilesHeight, thing);
        else if (x >= LabyrinthCell.TilesWidth && y >= LabyrinthCell.TilesHeight) // Bottom right
            _currentCells[2, 2].Things.PutThingAt(x - LabyrinthCell.TilesWidth, y - LabyrinthCell.TilesHeight, thing);
        else if (x >= LabyrinthCell.TilesWidth) // Right
            _currentCells[2, 1].Things.PutThingAt(x - LabyrinthCell.TilesWidth, y, thing);
        else if (y < 0) // Top
            _currentCells[1, 0].Things.PutThingAt(x, y + LabyrinthCell.TilesHeight, thing);
        else if (y >= LabyrinthCell.TilesHeight) // Bottom
            _currentCells[1, 2].Things.PutThingAt(x, y - LabyrinthCell.TilesHeight, thing);
    }

    public ThingList GetThingsAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < LabyrinthCell.TilesWidth && y < LabyrinthCell.TilesHeight)
            return _currentCells[1, 1].Things.GetThingsAt(x, y);

        if (x < 0 && y < 0) //Top left.
            return _currentCells[0, 0].Things.GetThingsAt(x + LabyrinthCell.TilesWidth, y + LabyrinthCell.TilesHeight);

        if (x < 0 && y >= LabyrinthCell.TilesHeight) // Bottom left
            return _currentCells[0, 2].Things.GetThingsAt(x + LabyrinthCell.TilesWidth, y - LabyrinthCell.TilesHeight);

        if (x < 0) // Left
            return _currentCells[0, 1].Things.GetThingsAt(x + LabyrinthCell.TilesWidth, y);

        if (x >= LabyrinthCell.TilesWidth && y < 0) //Top right.
            return _currentCells[2, 0].Things.GetThingsAt(x - LabyrinthCell.TilesWidth, y + LabyrinthCell.TilesHeight);

        if (x >= LabyrinthCell.TilesWidth && y >= LabyrinthCell.TilesHeight) // Bottom right
            return _currentCells[2, 2].Things.GetThingsAt(x - LabyrinthCell.TilesWidth , y - LabyrinthCell.TilesHeight);

        if (x >= LabyrinthCell.TilesWidth) // Right
            return _currentCells[2, 1].Things.GetThingsAt(x - LabyrinthCell.TilesWidth, y);

        if (y < 0) // Top
            return _currentCells[1, 0].Things.GetThingsAt(x, y + LabyrinthCell.TilesHeight);

        if (y >= LabyrinthCell.TilesHeight) // Bottom
            return _currentCells[1, 2].Things.GetThingsAt(x, y - LabyrinthCell.TilesHeight);

        return null;
    }
    public ThingList GetThingsThatCanBePickedUpAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < LabyrinthCell.TilesWidth && y < LabyrinthCell.TilesHeight)
            return _currentCells[1, 1].Things.GetThingsThatCanBePickedUpAt(x, y);

        if (x < 0 && y < 0) //Top left.
            return _currentCells[0, 0].Things.GetThingsThatCanBePickedUpAt(x + LabyrinthCell.TilesWidth, y + LabyrinthCell.TilesHeight);

        if (x < 0 && y >= LabyrinthCell.TilesHeight) // Bottom left
            return _currentCells[0, 2].Things.GetThingsThatCanBePickedUpAt(x + LabyrinthCell.TilesWidth, y - LabyrinthCell.TilesHeight);

        if (x < 0) // Left
            return _currentCells[0, 1].Things.GetThingsThatCanBePickedUpAt(x + LabyrinthCell.TilesWidth, y);
        if (x >= LabyrinthCell.TilesWidth && y < 0) //Top right.
            return _currentCells[2, 0].Things.GetThingsThatCanBePickedUpAt(x - LabyrinthCell.TilesWidth, y + LabyrinthCell.TilesHeight);

        if (x >= LabyrinthCell.TilesWidth && y >= LabyrinthCell.TilesHeight) // Bottom right
            return _currentCells[2, 2].Things.GetThingsThatCanBePickedUpAt(x - LabyrinthCell.TilesWidth, y - LabyrinthCell.TilesHeight);

        if (x >= LabyrinthCell.TilesWidth) // Right
            return _currentCells[2, 1].Things.GetThingsThatCanBePickedUpAt(x - LabyrinthCell.TilesWidth, y);

        if (y < 0) // Top
            return _currentCells[1, 0].Things.GetThingsThatCanBePickedUpAt(x, y + LabyrinthCell.TilesHeight);

        if (y >= LabyrinthCell.TilesHeight) // Bottom
            return _currentCells[1, 2].Things.GetThingsThatCanBePickedUpAt(x, y - LabyrinthCell.TilesHeight);

        return null;
    }

    public void RemoveThing(Thing thing)
    {
        _currentCells[0, 0].Things.Remove(thing);
        _currentCells[1, 0].Things.Remove(thing);
        _currentCells[2, 0].Things.Remove(thing);
        _currentCells[0, 1].Things.Remove(thing);
        _currentCells[1, 1].Things.Remove(thing);
        _currentCells[2, 1].Things.Remove(thing);
        _currentCells[0, 2].Things.Remove(thing);
        _currentCells[1, 2].Things.Remove(thing);
        _currentCells[2, 2].Things.Remove(thing);
    }
}