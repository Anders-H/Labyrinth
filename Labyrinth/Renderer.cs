using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Labyrinth;

public class Renderer
{
    private GraphicsDeviceManager G { get; }
    private SpriteBatch SpriteBatch { get; set; }
    public int VirtualWidth { get; }
    public int VirtualHeight { get; }
    public int OutputWidth { get; }
    public int OutputHeight { get; }
    public bool Fullscreen { get; }
    private RenderTarget2D RenderTarget { get; set; }

    public Renderer(Game game, int virtualWidth, int virtualHeight, bool fullscreen)
    {
        G = new GraphicsDeviceManager(game);
        VirtualWidth = virtualWidth;
        VirtualHeight = virtualHeight;
        Fullscreen = fullscreen;
        if (fullscreen)
        {
            OutputWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            OutputHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            G.PreferredBackBufferWidth = OutputWidth;
            G.PreferredBackBufferHeight = OutputHeight;
            G.IsFullScreen = true;
        }
        else
        {
            var w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            if (w >= VirtualWidth * 3 && h >= VirtualHeight * 3)
            {
                OutputWidth = VirtualWidth * 3;
                OutputHeight = VirtualHeight * 3;
            }
            else if (w >= VirtualWidth * 2 && h >= VirtualHeight * 2)
            {
                OutputWidth = VirtualWidth * 2;
                OutputHeight = VirtualHeight * 2;
            }
            else
            {
                OutputWidth = w;
                OutputHeight = h;
            }
            G.PreferredBackBufferWidth = OutputWidth;
            G.PreferredBackBufferHeight = OutputHeight;
            G.IsFullScreen = false;
        }
    }

    public void Initialize(Game game) =>
        RenderTarget = new RenderTarget2D(game.GraphicsDevice, VirtualWidth, VirtualHeight);
    
    public SpriteBatch CreateSpriteBatch(Game game) =>
        SpriteBatch ?? (SpriteBatch = new SpriteBatch(game.GraphicsDevice));
    
    public SpriteBatch BeginDraw()
    {
        G.GraphicsDevice.SetRenderTarget(RenderTarget);
        G.GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin();
        return SpriteBatch;
    }

    public void EndDraw()
    {
        SpriteBatch.End();
        G.GraphicsDevice.SetRenderTarget(null);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        SpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, OutputWidth, OutputHeight), new Rectangle(0, 0, VirtualWidth, VirtualHeight), Color.White);
        SpriteBatch.End();
    }
}