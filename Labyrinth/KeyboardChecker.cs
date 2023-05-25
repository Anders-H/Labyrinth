using Microsoft.Xna.Framework.Input;

namespace Labyrinth;

public class KeyboardChecker
{
    private bool _escape;
    private bool _d;
    private bool _i;
    private bool _p;
    private bool _q;
    private bool _enter;
    private KeyboardState KeyboardState { get; set; }
    public void CheckState(long ticks)
    {
        KeyboardState = Keyboard.GetState();
    }
    public bool Down => KeyboardState.IsKeyDown(Keys.Down);
    public bool Up => KeyboardState.IsKeyDown(Keys.Up);
    public bool Left => KeyboardState.IsKeyDown(Keys.Left);
    public bool Right => KeyboardState.IsKeyDown(Keys.Right);
    public bool PressEsc
    {
        get
        {
            if (!_escape && KeyboardState.IsKeyDown(Keys.Escape)) { _escape = true; return true; }
            if (KeyboardState.IsKeyDown(Keys.Escape)) return false;
            _escape = false; return false;
        }
    }
    public bool PressD
    {
        get
        {
            if (!_d && KeyboardState.IsKeyDown(Keys.D)) { _d = true; return true; }
            if (KeyboardState.IsKeyDown(Keys.D)) return false;
            _d = false; return false;
        }
    }
    public bool PressI
    {
        get
        {
            if (!_i && KeyboardState.IsKeyDown(Keys.I)) { _i = true; return true; }
            if (KeyboardState.IsKeyDown(Keys.I)) return false;
            _i = false; return false;
        }
    }
    public bool PressP
    {
        get
        {
            if (!_p && KeyboardState.IsKeyDown(Keys.P)) { _p = true; return true; }
            if (KeyboardState.IsKeyDown(Keys.P)) return false;
            _p = false; return false;
        }
    }
    public bool PressQ
    {
        get
        {
            if (!_q && KeyboardState.IsKeyDown(Keys.Q)) { _q = true; return true; }
            if (KeyboardState.IsKeyDown(Keys.Q)) return false;
            _q = false; return false;
        }
    }
    public bool PressEnter
    {
        get
        {
            if (!_enter && KeyboardState.IsKeyDown(Keys.Enter)) { _enter = true; return true; }
            if (KeyboardState.IsKeyDown(Keys.Enter)) return false;
            _enter = false; return false;
        }
    }
}