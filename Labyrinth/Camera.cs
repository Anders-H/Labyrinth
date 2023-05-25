using System;

namespace Labyrinth;

public class Camera
{
    public int ViewOffsetX { get; set; } = -272;
    public int ViewOffsetY { get; set; } = -224;
    private int ScrollSpeedX { get; set; } = 1;
    private int ScrollSpeedY { get; set; } = 1;
    private int ScrollSpeedChangedX { get; set; }
    private int ScrollSpeedChangedY { get; set; }

    public void AdjustCameraOffset(int playerX, int playerY)
    {
        ScrollSpeedChangedX++;
        ScrollSpeedChangedY++;
        var distanceX = playerX - 304;
        var distanceY = playerY - 224;

        if (distanceX > 0)
            ViewOffsetX -= ScrollSpeedX;
        else if (distanceX < 0)
            ViewOffsetX += ScrollSpeedX;

        if (distanceY > 0)
            ViewOffsetY -= ScrollSpeedY;
        else if (distanceY < 0)
            ViewOffsetY += ScrollSpeedY;

        if (Math.Abs(distanceX) > 100 && ScrollSpeedChangedX > 10 && ScrollSpeedX < 4)
        {
            ScrollSpeedX++;
            ScrollSpeedChangedX = 0;
        }
        else if (ScrollSpeedChangedX > 10 && ScrollSpeedX > 1)
        {
            ScrollSpeedX--;
            ScrollSpeedChangedX = 0;
        }

        if (Math.Abs(distanceY) > 100 && ScrollSpeedChangedY > 10 && ScrollSpeedY < 4)
        {
            ScrollSpeedY++;
            ScrollSpeedChangedY = 0;
        }
        else if (ScrollSpeedChangedY > 10 && ScrollSpeedY > 1)
        {
            ScrollSpeedY--;
            ScrollSpeedChangedY = 0;
        }

        if (ScrollSpeedX > Math.Abs(distanceX))
            ScrollSpeedX = 1;

        if (ScrollSpeedY > Math.Abs(distanceY))
            ScrollSpeedY = 1;
    }
}