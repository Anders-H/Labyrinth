using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Labyrinth.Labyrinth;

[Serializable]
public class Labyrinth
{
    internal bool HallSupport { get; }
    public static int Width => 50;
    public static int Height => 50;
    public LabyrinthCell[,] Cells { get; } = new LabyrinthCell[Width, Height];
    public static Random Random { get; } = new();

    public Labyrinth(bool hallSupport)
    {
        HallSupport = hallSupport;

        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                Cells[x, y] = new LabyrinthCell(x, y);
        
        Func<int, int, List<Point>> getNeighbours = (x, y) =>
        {
            var ret = new List<Point>();
            if (y > 0 && Cells[x, y - 1].WallsCount >= 4)
                ret.Add(Cells[x, y - 1].Position);
            if (x < Width - 1 && Cells[x + 1, y].WallsCount >= 4)
                ret.Add(Cells[x + 1, y].Position);
            if (y < Height - 1 && Cells[x, y + 1].WallsCount >= 4)
                ret.Add(Cells[x, y + 1].Position);
            if (x > 0 && Cells[x - 1, y].WallsCount >= 4)
                ret.Add(Cells[x - 1, y].Position);
            return ret;
        };
        
        Action<LabyrinthCell, Point> knockWall = (cell1, cell2) =>
        {
            if (cell2.Y < cell1.Y)
            {
                cell1.WallUp = false;
                Cells[cell2.X, cell2.Y].WallDown = false;
                return;
            }
            if (cell2.X > cell1.X)
            {
                cell1.WallRight = false;
                Cells[cell2.X, cell2.Y].WallLeft = false;
                return;
            }
            if (cell2.Y > cell1.Y)
            {
                cell1.WallDown = false;
                Cells[cell2.X, cell2.Y].WallUp = false;
                return;
            }
            if (cell2.X < cell1.X)
            {
                cell1.WallLeft = false;
                Cells[cell2.X, cell2.Y].WallRight = false;
            }
        };

        var queue = new Queue<LabyrinthCell>();
        var currentCell = Cells[Random.Next(Width), Random.Next(Height)];
        queue.Enqueue(currentCell);

        while (queue.Count > 0)
        {
            var neighbours = getNeighbours(currentCell.X, currentCell.Y);
            if (neighbours.Count > 0)
            {
                var neighbour = neighbours[Random.Next(neighbours.Count)];
                knockWall(currentCell, neighbour);
                queue.Enqueue(currentCell);
                currentCell = Cells[neighbour.X, neighbour.Y];
            }
            else
            {
                currentCell = queue.Dequeue();
            }
        }

        //If halls are used, all tiles must be preaccessed.
        if (hallSupport)
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    Cells[x, y].GetTiles(this);
    }

    internal LabyrinthCell GetNeighbourUp(int x, int y) =>
        y <= 0 ? null : Cells[x, y - 1];

    internal LabyrinthCell GetNeighbourDown(int x, int y) =>
        y < Height - 1 ? null : Cells[x, y + 1];
    
    internal LabyrinthCell GetNeighbourLeft(int x, int y) =>
        y <= 0 ? null : Cells[x - 1, y];

    internal LabyrinthCell GetNeighbourRight(int x, int y) =>
        y < Width ? null : Cells[x + 1, y];

    public LabyrinthCell[,] GetSurroundingCells(int x, int y)
    {
        var ret = new LabyrinthCell[3, 3];
        var retx = 0;
        var rety = 0;

        for (var row = y - 1; row <= y + 1; row++)
        {
            for (var col = x - 1; col <= x + 1; col++)
            {
                if (col >= 0 && col < Width && row >= 0 && row < Height)
                {
                    ret[retx, rety] = Cells[col, row];
                }
                else
                {
                    ret[retx, rety] = new LabyrinthCell(col, row);
                    ret[retx, rety].MakeDeadEnd();
                }

                retx++;
            }

            retx = 0;
            rety++;
        }

        return ret;
    }

    public void PositionObjects()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var distanceFromStart = Math.Max(Math.Abs(x - 24), Math.Abs(y - 24));
                if (distanceFromStart < 10)
                    Cells[x, y].AddRocks(6 - (distanceFromStart / 2));
            }
        }
    }
}