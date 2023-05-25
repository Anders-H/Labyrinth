using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Labyrinth.Labyrinth;
using Labyrinth.PlayerClasses;

namespace Labyrinth;

public class Game1 : Game
{
    private Renderer Renderer { get; }
    private GameState GameState { get; set; } = GameState.Running;
    private long _ticks;
    public const int TileSize = 32;
    public const int CellSize = 416;
    private Texture2D TilesTexture { get; set; }
    private SpriteFont SpriteFont { get; set; }
    public Labyrinth.Labyrinth Labyrinth { get; set; }
    public Player Player = new();
    public CurrentCellMatrix CurrentCells { get; } = new();
    private Camera Camera { get; } = new();
    private int VisitedCount { get; set; }
    private double VisitedPercent { get; set; }
    private KeyboardChecker Key { get; } = new();
    private Window PauseWindow { get; } = new(1, 1, 18, 13);
    private readonly string[] _statusLines = new string[10];

    public Game1()
    {

#if DEBUG
        const bool fullscreen = false;
        IsMouseVisible = true;
#else
        const bool fullscreen = true;
        IsMouseVisible = false;
#endif
        Renderer = new Renderer(this, 640, 480, fullscreen);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        Renderer.Initialize(this);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Renderer.CreateSpriteBatch(this);
        TilesTexture = Content.Load<Texture2D>("tiles");
        SpriteFont = Content.Load<SpriteFont>("SpriteFont");
        Labyrinth = new Labyrinth.Labyrinth(true);
        Player = new Player { RoomX = 24, RoomY = 24 };
        var freeTile = Labyrinth.Cells[24, 24].GetRandomFreeTile();
        Labyrinth.PositionObjects();
        CurrentCells.SetCells(Labyrinth.GetSurroundingCells(24, 24));
        Player.TileX = freeTile.X;
        Player.TileY = freeTile.Y;

        for (var i = 0; i < _statusLines.Length; i++)
            _statusLines[i] = "";
    }

    protected override void Update(GameTime gameTime)
    {
        if (CurrentCells.CenterCell.VisitedOrder <= 0)
        {
            VisitedCount++;
            CurrentCells.CenterCell.VisitedOrder = VisitedCount;
            VisitedPercent = (VisitedCount / 2500.0) * 100.0;
            switch (VisitedCount)
            {
                case 3:
                case 5:
                case 10:
                case 20:
                case 50:
                case 100:
                case 200:
                case 500:
                case 1000:
                case 2000:
                    AddStatus($"You have visited {VisitedCount} rooms. {2500 - VisitedCount} rooms to go.");
                    break;
                case 2500:
                    AddStatus("You have completed the game! Well done!");
                    break;
            }
        }
        Key.CheckState(_ticks);
        if (_ticks == 50)
            AddStatus("Labyrinth programmed by Anders Hesselbom.");
        else if (_ticks == 100)
            AddStatus("Tile graphics by Chris Hamons.");
        switch (GameState)
        {
            case GameState.Running:
                if (_ticks % 8 == 0)
                {
                    Player.UserWalk(Key, CurrentCells, Camera, Labyrinth);
                }
                if (Key.PressEsc)
                    GameState = GameState.Paused;
                else if (Key.PressI)
                {
                    Player.CursorTileX = 0;
                    Player.CursorTileY = 0;
                    GameState = GameState.Inventory;
                }
                else if (Key.PressP)
                {
                    GameState = GameState.PickUp;
                    Player.CursorTileX = Player.TileX;
                    Player.CursorTileY = Player.TileY;
                }
                break;
            case GameState.Paused:
                if (Key.PressEsc)
                    GameState = GameState.Running;
                else if (Key.PressI)
                {
                    Player.CursorTileX = 0;
                    Player.CursorTileY = 0;
                    GameState = GameState.Inventory;
                }
                else if (Key.PressP)
                {
                    GameState = GameState.PickUp;
                    Player.CursorTileX = Player.TileX;
                    Player.CursorTileY = Player.TileY;
                }
                else if (Key.PressQ)
                    Exit();
                break;
            case GameState.Inventory:
                if (_ticks % 8 == 0)
                {
                    Player.ControlInventoryCursor(Key, CurrentCells);
                }
                if (Key.PressEsc || Key.PressI)
                    GameState = GameState.Running;
                Player.Inventory.CurrentThingBeingPointedAt = Player.Inventory.PeekThingAt(Player.CursorTileX, Player.CursorTileY);
                Player.Inventory.CurrentDescription = Player.Inventory.CurrentThingBeingPointedAt == null ? "Esc - Continue" : $"{Player.Inventory.CurrentThingBeingPointedAt.Name}: D - Drop, Esc - Continue";
                if (Key.PressD && Player.Inventory.CurrentThingBeingPointedAt != null)
                {
                    Player.Inventory.CurrentThingBeingPointedAt = Player.Inventory.GetThingAt(Player.CursorTileX, Player.CursorTileY);
                    CurrentCells.PutThingAt(Player.TileX, Player.TileY, Player.Inventory.CurrentThingBeingPointedAt);
                    AddStatus($"You have dropped: {Player.Inventory.CurrentThingBeingPointedAt.Name}");
                    Player.Inventory.CurrentThingBeingPointedAt = null;
                    GameState = GameState.Running;
                }
                break;
            case GameState.PickUp:
                if (_ticks % 8 == 0)
                {
                    Player.ControlPickupCursor(Key, CurrentCells);
                }
                if (Key.PressEsc)
                    GameState = GameState.Running;
                if (Key.PressEnter)
                {
                    var o = CurrentCells.GetThingsThatCanBePickedUpAt(Player.CursorTileX, Player.CursorTileY);
                    if (o.Count <= 0)
                        o = CurrentCells.GetThingsAt(Player.CursorTileX, Player.CursorTileY);
                    if (o.Count <= 0)
                        AddStatus("Return to game.");
                    else
                    {
                        if (Player.Inventory.HasFreeSlot())
                        {
                            if (o.First().CanBePickedUp)
                            {
                                AddStatus($"You have picked up: {o.First().Name}");
                                // ReSharper disable once PossibleInvalidOperationException
                                Player.Inventory.SetThingAt(Player.Inventory.GetFreeSlot().Value, o.First());
                                CurrentCells.RemoveThing(o.First());
                            }
                            else
                                AddStatus($"The object {o.First().Name} can not be picked up.");
                        }
                        else
                            AddStatus("You have no free slots in your inventory. You need to drop something first.");
                    }
                    GameState = GameState.Running;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _ticks++;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var spriteBatch = Renderer.BeginDraw();
        //Draw background tiles.
        CurrentCells.Draw(Camera.ViewOffsetX, Camera.ViewOffsetY, spriteBatch, TilesTexture);
        //Draw player.
        var playerX = Camera.ViewOffsetX + CellSize + (TileSize * Player.TileX);
        var playerY = Camera.ViewOffsetY + CellSize + (TileSize * Player.TileY);
        spriteBatch.Draw(TilesTexture, new Rectangle(playerX, playerY, 32, 32), Player.TextureLocation.PhysicalRectangle, GameState == GameState.PickUp && _ticks % 50 < 25 ? Color.FromNonPremultiplied(255, 255, 255, 50) : Color.White);
        //Heads up display.
        if (CurrentCells.CenterCell.VisitedOrder > 0)
            spriteBatch.DrawString(SpriteFont, $"Dungeon completed: {VisitedPercent:000.0}% - This is your room number {CurrentCells.CenterCell.VisitedOrder}.", new Vector2(2, 0), Color.White);
        else
            spriteBatch.DrawString(SpriteFont, $"Dungeon completed: {VisitedPercent:000.0}%", new Vector2(2, 0), Color.White);
        //Statustext.
        var statusY = 463;
        var color = 255;
        foreach (var t in _statusLines)
        {
            spriteBatch.DrawString(SpriteFont, t, new Vector2(2, statusY), Color.FromNonPremultiplied(255, 255, 255, color));
            statusY -= 14;
            color -= 20;
        }
        //Modes.
        switch (GameState)
        {
            case GameState.Paused:
                PauseWindow.Draw(spriteBatch, TilesTexture);
                PauseWindow.DrawString(spriteBatch, SpriteFont, "GAME PAUSED", true);
                PauseWindow.DrawString(spriteBatch, SpriteFont, "I - Inventory", false);
                PauseWindow.DrawString(spriteBatch, SpriteFont, "P - Pick up", false);
                PauseWindow.DrawString(spriteBatch, SpriteFont, "Q - Quit game", true);
                PauseWindow.DrawString(spriteBatch, SpriteFont, "Esc - Continue", false);
                break;
            case GameState.Inventory:
            {
                PauseWindow.Draw(spriteBatch, TilesTexture);
                PauseWindow.DrawString(spriteBatch, SpriteFont, "INVENTORY", false);
                PauseWindow.DrawString(spriteBatch, SpriteFont, Player.Inventory.CurrentDescription, false);
                var cursorX = TileSize * Player.CursorTileX;
                var cursorY = TileSize * Player.CursorTileY;
                spriteBatch.Draw(TilesTexture, new Rectangle(cursorX + (TileSize * 2), cursorY + (TileSize * 3), 32, 32), Player.PickupCursorLocation.PhysicalRectangle, _ticks % 20 < 10 ? Color.FromNonPremultiplied(255, 255, 255, 50) : Color.White);
                Player.Inventory.Draw(spriteBatch, TilesTexture);
            }
                break;
            case GameState.PickUp:
            {
                var cursorX = Camera.ViewOffsetX + CellSize + (TileSize * Player.CursorTileX);
                var cursorY = Camera.ViewOffsetY + CellSize + (TileSize * Player.CursorTileY);
                spriteBatch.Draw(TilesTexture, new Rectangle(cursorX, cursorY, 32, 32), Player.PickupCursorLocation.PhysicalRectangle, _ticks % 20 < 10 ? Color.FromNonPremultiplied(255, 255, 255, 50) : Color.White);
                var things = CurrentCells.GetThingsThatCanBePickedUpAt(Player.CursorTileX, Player.CursorTileY);
                if (things.Count <= 0)
                    CurrentCells.GetThingsAt(Player.CursorTileX, Player.CursorTileY);
                var description = things.Count <= 0 ? "Nothing" : (things.First().CanBePickedUp ? things.First().Name : $"{things.First().Name} (can't be picked up)");
                spriteBatch.DrawString(SpriteFont, $"Pick up: {description}{(things.Count > 1 ? " (more objects under)" : "")}", new Vector2(2, 14), Color.White);
            }
                break;
            case GameState.Running:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Renderer.EndDraw();
        base.Draw(gameTime);
        //Offset adjustments for next draw.
        Camera.AdjustCameraOffset(playerX, playerY);
    }

    private void AddStatus(string text)
    {
        for (var i = 8; i >= 0; i--)
            _statusLines[i + 1] = _statusLines[i];
        _statusLines[0] = text;
    }
}