using Labyrinth.Labyrinth;
using Labyrinth.Things;

namespace Labyrinth.PlayerClasses;

public class Player : Actor
{
    public int RoomX { get; set; }
    public int RoomY { get; set; }
    public int TileX { get; set; }
    public int TileY { get; set; }
    public int CursorTileX { get; set; }
    public int CursorTileY { get; set; }
    public override bool IsObstacle => true;
    public override bool CanBePickedUp => false;
    public override string Name => "Player";
    public TileRectangle PickupCursorLocation = new(3, 1);
    public Inventory Inventory { get; }

    public Player()
    {
        TextureLocation = new TileRectangle(9, 47);
        Inventory = new Inventory();
    }

    public void UserWalk(KeyboardChecker key, CurrentCellMatrix cells, Camera camera, global::Labyrinth.Labyrinth.Labyrinth labyrinth)
    {
        if (key.Up)
        {
            if (TileY <= 0)
            {
                if (cells.TopCell.PlayerCanMoveTo(TileX, 12))
                {
                    RoomY--;
                    cells.SetCells(labyrinth.GetSurroundingCells(RoomX, RoomY));
                    TileY = 12;
                    camera.ViewOffsetY -= Game1.CellSize;
                }
            }
            else
            {
                if (cells.CenterCell.PlayerCanMoveTo(TileX, TileY - 1))
                    TileY--;
            }
        }
        else if (key.Down)
        {
            if (TileY >= 12)
            {
                if (cells.BottomCell.PlayerCanMoveTo(TileX, 0))
                {
                    RoomY++;
                    cells.SetCells(labyrinth.GetSurroundingCells(RoomX, RoomY));
                    TileY = 0;
                    camera.ViewOffsetY += Game1.CellSize;
                }
            }
            else
            {
                if (cells.CenterCell.PlayerCanMoveTo(TileX, TileY + 1))
                    TileY++;
            }
        }
        if (key.Left)
        {
            if (TileX <= 0)
            {
                if (cells.LeftCell.PlayerCanMoveTo(12, TileY))
                {
                    RoomX--;
                    cells.SetCells(labyrinth.GetSurroundingCells(RoomX, RoomY));
                    TileX = 12;
                    camera.ViewOffsetX -= Game1.CellSize;
                }
            }
            else
            {
                if (cells.CenterCell.PlayerCanMoveTo(TileX - 1, TileY))
                    TileX--;
            }
        }
        if (key.Right)
        {
            if (TileX >= 12)
            {
                if (cells.RightCell.PlayerCanMoveTo(0, TileY))
                {
                    RoomX++;
                    cells.SetCells(labyrinth.GetSurroundingCells(RoomX, RoomY));
                    TileX = 0;
                    camera.ViewOffsetX += Game1.CellSize;
                }
            }
            else
            {
                if (cells.CenterCell.PlayerCanMoveTo(TileX + 1, TileY))
                    TileX++;
            }
        }
    }

    public void ControlPickupCursor(KeyboardChecker key, CurrentCellMatrix cells)
    {
        if (key.Up && CursorTileY > TileY - 1)
            CursorTileY--;
        else if (key.Down && CursorTileY < TileY + 1)
            CursorTileY++;

        if (key.Left && CursorTileX > TileX - 1)
            CursorTileX--;
        else if (key.Right && CursorTileX < TileX + 1)
            CursorTileX++;
    }

    public void ControlInventoryCursor(KeyboardChecker key, CurrentCellMatrix cells)
    {
        if (key.Up && CursorTileY > 0)
            CursorTileY--;
        else if (key.Down && CursorTileY < Inventory.SlotsHeight - 1)
            CursorTileY++;

        if (key.Left && CursorTileX > 0)
            CursorTileX--;
        else if (key.Right && CursorTileX < Inventory.SlotsWidth - 1)
            CursorTileX++;
    }
}