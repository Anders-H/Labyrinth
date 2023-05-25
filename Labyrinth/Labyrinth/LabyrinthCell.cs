using System;
using Labyrinth.Things;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Labyrinth.Labyrinth;

[Serializable]
public class LabyrinthCell
{
    private bool _isOuterDummy;
    public bool IsHall { get; set; }
    public int VisitedOrder { get; set; }
    public static int TilesWidth => 13;
    public static int TilesHeight => 13;
    private int[,] _tiles;
    private TileRectangle[,] _graphicalTiles;
    private bool _roomGenerated;
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool WallUp { get; set; }
    public bool WallRight { get; set; }
    public bool WallDown { get; set; }
    public bool WallLeft { get; set; }
    public LabyrinthCell IsPairedWith { get; set; }
    private static int _lastGeneratedTileSet = -1;
    private int TileSet { get; set; } = -1;
    public ThingList Things = new();

    public LabyrinthCell(int x, int y)
    {
        X = x;
        Y = y;
        WallUp = true;
        WallRight = true;
        WallDown = true;
        WallLeft = true;
    }

    public Point Position =>
        new Point(X, Y);

    public int WallsCount
    {
        get
        {
            var ret = 0;
            if (WallUp)
                ret++;
            if (WallRight)
                ret++;
            if (WallDown)
                ret++;
            if (WallLeft)
                ret++;
            return ret;
        }
    }

    public int[,] GetTiles(Labyrinth parent)
    {
        if (_tiles == null)
            GenerateTiles(parent);

        return _tiles;
    }

    private void GenerateTiles(Labyrinth parent)
    {
        _tiles = new int[TilesWidth, TilesHeight];
        //Create exists.
        var centerX = (int)Math.Floor((double)TilesWidth / 2);
        var centerY = (int)Math.Floor((double)TilesHeight / 2);

        for (var y = 0; y < TilesHeight; y++)
            for (var x = 0; x < TilesWidth; x++)
                _tiles[x, y] = 1;

        if (!WallUp)
            for (var i = 0; i <= centerY; i++)
                _tiles[centerX, i] = 0;

        if (!WallDown)
            for (var i = centerY; i < TilesHeight; i++)
                _tiles[centerX, i] = 0;

        if (!WallLeft)
            for (var i = 0; i <= centerX; i++)
                _tiles[i, centerY] = 0;

        if (!WallRight)
            for (var i = centerX; i < TilesWidth; i++)
                _tiles[i, centerY] = 0;

        if (WallsCount == 3)
            MakeRoom(parent);
        else if (WallsCount == 2 && Labyrinth.Random.Next(3) == 0)
            MakeRoom(parent);
        else if (WallsCount == 1 && Labyrinth.Random.Next(6) == 0)
            MakeRoom(parent);
    }

    private void MakeRoom(Labyrinth parent)
    {
        if (_roomGenerated)
            return;

        _roomGenerated = true;

        if (parent.HallSupport && !IsHall && Labyrinth.Random.Next(5) == 0) //In some cases great halls (2x1 or 1x2 cells) are generated.
        {
            switch (Labyrinth.Random.Next(4))
            {
                case 0:
                    if (!WallUp)
                    {
                        var neighbour = parent.GetNeighbourUp(X, Y);
                        if (neighbour != null && !neighbour.IsHall)
                        {
                            for (var y = 0; y < TilesHeight - 1; y++)
                                for (var x = 1; x < TilesWidth - 1; x++)
                                    _tiles[x, y] = 0;

                            for (var y = 1; y < TilesHeight; y++)
                                for (var x = 1; x < TilesWidth - 1; x++)
                                    neighbour._tiles[x, y] = 0;

                            IsHall = true;
                            neighbour._roomGenerated = true;
                            neighbour.IsHall = true;
                            IsPairedWith = neighbour;
                            neighbour.IsPairedWith = this;
                        }
                    }
                    break;
                case 1:
                    if (!WallDown)
                    {
                        var neighbour = parent.GetNeighbourDown(X, Y);
                        if (neighbour != null && !neighbour.IsHall)
                        {
                            for (var y = 1; y < TilesHeight; y++)
                                for (var x = 1; x < TilesWidth - 1; x++)
                                    _tiles[x, y] = 0;
                            for (var y = 0; y < TilesHeight - 1; y++)
                                for (var x = 1; x < TilesWidth - 1; x++)
                                    neighbour._tiles[x, y] = 0;
                            IsHall = true;
                            neighbour._roomGenerated = true;
                            neighbour.IsHall = true;
                            IsPairedWith = neighbour;
                            neighbour.IsPairedWith = this;
                        }
                    }
                    break;
                case 2:
                    if (!WallLeft)
                    {
                        var neighbour = parent.GetNeighbourLeft(X, Y);
                        if (neighbour != null && !neighbour.IsHall)
                        {
                            for (var y = 1; y < TilesHeight - 1; y++)
                                for (var x = 0; x < TilesWidth - 1; x++)
                                    _tiles[x, y] = 0;
                            for (var y = 1; y < TilesHeight - 1; y++)
                                for (var x = 1; x < TilesWidth; x++)
                                    neighbour._tiles[x, y] = 0;
                            IsHall = true;
                            neighbour._roomGenerated = true;
                            neighbour.IsHall = true;
                            IsPairedWith = neighbour;
                            neighbour.IsPairedWith = this;
                        }
                    }
                    break;
                case 3:
                    if (!WallRight)
                    {
                        var neighbour = parent.GetNeighbourRight(X, Y);
                        if (neighbour != null && !neighbour.IsHall)
                        {
                            for (var y = 1; y < TilesHeight - 1; y++)
                                for (var x = 1; x < TilesWidth; x++)
                                    _tiles[x, y] = 0;
                            for (var y = 1; y < TilesHeight - 1; y++)
                                for (var x = 0; x < TilesWidth - 1; x++)
                                    neighbour._tiles[x, y] = 0;
                            IsHall = true;
                            neighbour._roomGenerated = true;
                            neighbour.IsHall = true;
                            IsPairedWith = neighbour;
                            neighbour.IsPairedWith = this;
                        }
                    }
                    break;
            }
            if (IsHall)
                return;
        }

        //Single cell room generation.
        switch (Labyrinth.Random.Next(2))
        {
            case 0: //Large room
                for (var y = 1; y < TilesHeight - 1; y++)
                    for (var x = 1; x < TilesWidth - 1; x++)
                        _tiles[x, y] = 0;

                if (Labyrinth.Random.Next(5) == 0)
                {
                    //Fill everything but edges.
                    for (var y = 2; y < TilesHeight - 2; y++)
                        for (var x = 2; x < TilesWidth - 2; x++)
                            _tiles[x, y] = 1;

                    if (WallsCount == 3 && Labyrinth.Random.Next(2) == 0)
                    {
                        //Make horseshue.
                        for (var y = 3; y < TilesHeight - 3; y++)
                            for (var x = 3; x < TilesWidth - 3; x++)
                                _tiles[x, y] = 0;
                        if (!WallLeft)
                            for (var i = 3; i < TilesHeight - 3; i++)
                                _tiles[TilesWidth - 3, i] = 0;
                        else if (!WallRight)
                            for (var i = 3; i < TilesHeight - 3; i++)
                                _tiles[2, i] = 0;
                        else if (!WallDown)
                            for (var i = 3; i < TilesWidth - 3; i++)
                                _tiles[i, 2] = 0;
                        else if (!WallUp)
                            for (var i = 3; i < TilesWidth - 3; i++)
                                _tiles[i, TilesWidth - 3] = 0;
                    }
                }
                else
                {
                    //Rounded corners
                    if (Labyrinth.Random.Next(3) == 0)
                    {
                        _tiles[1, 1] = 1;
                        _tiles[TilesWidth - 2, 1] = 1;
                        _tiles[1, TilesHeight - 2] = 1;
                        _tiles[TilesWidth - 2, TilesHeight - 2] = 1;

                        if (Labyrinth.Random.Next(2) == 0)
                        {
                            _tiles[2, 1] = 1;
                            _tiles[1, 2] = 1;
                            _tiles[TilesWidth - 3, 1] = 1;
                            _tiles[TilesWidth - 2, 2] = 1;
                            _tiles[1, TilesHeight - 3] = 1;
                            _tiles[2, TilesHeight - 2] = 1;
                            _tiles[TilesWidth - 3, TilesHeight - 2] = 1;
                            _tiles[TilesWidth - 2, TilesHeight - 3] = 1;
                        }
                    }
                }
                break;
            case 1: //Small room
                for (var y = 3; y < TilesHeight - 3; y++)
                    for (var x = 3; x < TilesWidth - 3; x++)
                        _tiles[x, y] = 0;
                break;
            default:
                throw new Exception("Out of range.");
        }
    }

    public bool TileIsObstacle(Labyrinth parent, int x, int y) =>
        GetTiles(parent)[x, y] > 0;

    public Point GetRandomFreeTile()
    {
        do
        {
            var x = Labyrinth.Random.Next(TilesWidth);
            var y = Labyrinth.Random.Next(TilesHeight);

            if (_tiles[x, y] == 0)
                return new Point(x, y);

        } while (true);
    }

    public TileRectangle[,] GetGraphicalTiles()
    {
        if (_graphicalTiles == null)
        {
            _graphicalTiles = new TileRectangle[13, 13];
            if (_isOuterDummy)
            {
                for (var y = 0; y < 13; y++)
                    for (var x = 0; x < 13; x++)
                        _graphicalTiles[x, y] = new TileRectangle(39, 16);
            }
            else
            {
                //Select a tile set.
                if (_lastGeneratedTileSet < 0 || Labyrinth.Random.Next(100) == 0)
                    _lastGeneratedTileSet = Labyrinth.Random.Next(4);
            }
            GetGraphicalTiles(_lastGeneratedTileSet);
        }

        if (IsPairedWith != null && IsPairedWith.TileSet != TileSet)
            IsPairedWith.GetGraphicalTiles(_lastGeneratedTileSet);

        return _graphicalTiles;
    }

    private TileRectangle[,] GetGraphicalTiles(int tileSet)
    {
        _graphicalTiles ??= new TileRectangle[13, 13];

        TileSet = tileSet;

        switch (tileSet)
        {
            case 0: //Rock 1.
                for (var y = 0; y < 13; y++)
                {
                    for (var x = 0; x < 13; x++)
                    {
                        switch (_tiles[x, y])
                        {
                            case 0:
                                _graphicalTiles[x, y] = new TileRectangle(29 + Labyrinth.Random.Next(8), 13);
                                break;
                            default:
                                if (AllNeighboursAreWalls(x, y))
                                    _graphicalTiles[x, y] = new TileRectangle(4, 1);
                                else
                                    _graphicalTiles[x, y] = new TileRectangle(22 + Labyrinth.Random.Next(6), 13);
                                break;
                        }
                    }
                }
                break;
            case 1: //Dirt 1.
                for (var y = 0; y < 13; y++)
                {
                    for (var x = 0; x < 13; x++)
                    {
                        switch (_tiles[x, y])
                        {
                            case 0:
                                var floor = Labyrinth.Random.Next(9);
                                if (floor < 8)
                                    _graphicalTiles[x, y] = new TileRectangle(Labyrinth.Random.Next(8), 14);
                                else
                                    _graphicalTiles[x, y] = new TileRectangle(63, 13);
                                break;
                            default:
                                if (AllNeighboursAreWalls(x, y))
                                    _graphicalTiles[x, y] = new TileRectangle(4, 1);
                                else
                                    _graphicalTiles[x, y] = new TileRectangle(4 + Labyrinth.Random.Next(4), 16);
                                break;
                        }
                    }
                }
                break;
            case 2: //Rock 2
                for (var y = 0; y < 13; y++)
                {
                    for (var x = 0; x < 13; x++)
                    {
                        switch (_tiles[x, y])
                        {
                            case 0:
                                _graphicalTiles[x, y] = new TileRectangle(25 + Labyrinth.Random.Next(10), 14);
                                break;
                            default:
                                if (AllNeighboursAreWalls(x, y))
                                    _graphicalTiles[x, y] = new TileRectangle(4, 1);
                                else
                                    _graphicalTiles[x, y] = new TileRectangle(8 + Labyrinth.Random.Next(8), 16);
                                break;
                        }
                    }
                }
                break;
            case 3: //Outside 1
                for (var y = 0; y < 13; y++)
                {
                    for (var x = 0; x < 13; x++)
                    {
                        switch (_tiles[x, y])
                        {
                            case 0:
                                _graphicalTiles[x, y] = new TileRectangle(14 + Labyrinth.Random.Next(8), 13);
                                break;
                            default:
                                if (AllNeighboursAreWalls(x, y))
                                    _graphicalTiles[x, y] = new TileRectangle(4, 1);
                                else
                                {
                                    switch (Labyrinth.Random.Next(12))
                                    {
                                        case 0:
                                            _graphicalTiles[x, y] = new TileRectangle(56, 16);
                                            break;
                                        case 1:
                                            _graphicalTiles[x, y] = new TileRectangle(57, 16);
                                            break;
                                        case 2:
                                            _graphicalTiles[x, y] = new TileRectangle(58, 16);
                                            break;
                                        case 3:
                                            _graphicalTiles[x, y] = new TileRectangle(59, 16);
                                            break;
                                        case 4:
                                            _graphicalTiles[x, y] = new TileRectangle(60, 16);
                                            break;
                                        case 5:
                                            _graphicalTiles[x, y] = new TileRectangle(61, 16);
                                            break;
                                        case 6:
                                            _graphicalTiles[x, y] = new TileRectangle(62, 16);
                                            break;
                                        case 7:
                                            _graphicalTiles[x, y] = new TileRectangle(63, 16);
                                            break;
                                        case 8:
                                            _graphicalTiles[x, y] = new TileRectangle(0, 17);
                                            break;
                                        case 9:
                                            _graphicalTiles[x, y] = new TileRectangle(1, 17);
                                            break;
                                        case 10:
                                            _graphicalTiles[x, y] = new TileRectangle(2, 17);
                                            break;
                                        case 11:
                                            _graphicalTiles[x, y] = new TileRectangle(3, 17);
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
                break;
        }

        return _graphicalTiles;
    }

    private bool AllNeighboursAreWalls(int x, int y)
    {
        var wallCount = 0;

        if (y <= 0 || _tiles[x, y - 1] > 0) //Above
            wallCount++;

        if (y <= 0 || x >= 12 || _tiles[x + 1, y - 1] > 0) //Above left
            wallCount++;

        if (x >= 12 || _tiles[x + 1, y] > 0) //Left
            wallCount++;

        if (x >= 12 || y >= 12 || _tiles[x + 1, y + 1] > 0) //Below left
            wallCount++;

        if (y >= 12 || _tiles[x, y + 1] > 0) // Below
            wallCount++;

        if (y >= 12 || x == 0 || _tiles[x - 1, y + 1] > 0) //Below right
            wallCount++;

        if (x <= 0 || _tiles[x - 1, y] > 0) //Right
            wallCount++;

        if (x <= 0 || y <= 0 || _tiles[x - 1, y - 1] > 0) //Above right
            wallCount++;

        return wallCount == 8;

    }
    public bool PlayerCanMoveTo(int x, int y)
    {
        if (_tiles[x, y] > 0)
            return false;
        if (_isOuterDummy)
            return false;
        return true;
    }

    internal void MakeDeadEnd() =>
        _isOuterDummy = true;

    private bool CellIsFree(int x, int y) =>
        _tiles[x, y] == 0 && Things.GetThingsAt(x, y).Count <= 0;

    private void AddThing(Thing thing)
    {
        for (var i = 0; i < 50; i++)
        {
            var x = Labyrinth.Random.Next(0, TilesWidth);
            var y = Labyrinth.Random.Next(0, TilesHeight);

            if (!CellIsFree(x, y))
                continue;

            thing.GridX = x;
            thing.GridY = y;
            Things.Add(thing);
            return;
        }
    }

    public void AddRocks(int maxCount)
    {
        for (var i = 0; i < maxCount; i++)
            AddThing(new Rock());
    }

    public void DrawCellTiles(SpriteBatch sb, Texture2D texture, int x, int y, Color color)
    {
        var physicalX = x;
        var physicalY = y;
        var tiles = GetGraphicalTiles();

        for (var tileY = 0; tileY < TilesHeight; tileY++)
        {
            for (var tileX = 0; tileX < TilesWidth; tileX++)
            {
                sb.Draw(texture, new Rectangle(physicalX, physicalY, Game1.TileSize, Game1.TileSize), tiles[tileX, tileY].PhysicalRectangle, color);
                physicalX += Game1.TileSize;
            }

            physicalX = x;
            physicalY += Game1.TileSize;
        }
    }

    public void DrawThings(SpriteBatch sb, Texture2D texture, int x, int y, Color color)
    {
        foreach (var thing in Things)
            sb.Draw(texture, new Rectangle((thing.GridX * 32) + x, (thing.GridY * 32) + y, Game1.TileSize, Game1.TileSize), thing.TextureLocation.PhysicalRectangle, color);
    }
}