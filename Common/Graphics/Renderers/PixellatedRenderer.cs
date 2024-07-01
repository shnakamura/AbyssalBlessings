using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Graphics.Renderers;

/// <summary>
///     Handles registration and rendering of pixellated content.
/// </summary>
[Autoload(Side = ModSide.Client)]
public sealed class PixellatedRenderer : ModSystem
{
    private static List<Action> Actions { get; } = new();
    
    /// <summary>
    ///     The render target used for drawing pixellated content.
    /// </summary>
    /// <remarks>
    ///     This has half the screen size and is rendered at full
    ///     screen size, which results in the pixellated effect.
    /// </remarks>
    public static RenderTarget2D Target { get; private set; }

    public override void Load() {
        Main.RunOnMainThread(
            () => {
                Target = new(
                    Main.graphics.GraphicsDevice,
                    Main.screenWidth / 2,
                    Main.screenHeight / 2
                );
            }
        );
        
        On_Main.DrawProjectiles += (orig, self) => {
            DrawTarget();
            
            orig(self);
        };

        Main.OnResolutionChanged += ResizeTarget;
    }

    public override void Unload() {
        Main.OnResolutionChanged -= ResizeTarget;
        
        Main.RunOnMainThread(() => Target?.Dispose());
    }

    public override void PostUpdateEverything() {
        var device = Main.graphics.GraphicsDevice;

        var bindings = device.GetRenderTargets();

        device.SetRenderTarget(Target);
        device.Clear(Color.Transparent);
        
        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);

        foreach (var action in Actions) {
            action?.Invoke();
        }
        
        Main.spriteBatch.End();
        
        device.SetRenderTargets(bindings);

        Actions.Clear();
    }
    
    /// <summary>
    ///     Queues an action to be executed during the next rendering update.
    /// </summary>
    /// <param name="action">The action to queue.</param>
    public static void Queue(Action action) {
        Actions.Add(action);
    }

    private static void ResizeTarget(Vector2 size) {
        Main.RunOnMainThread(
            () => {
                Target?.Dispose();

                Target = new(
                    Main.graphics.GraphicsDevice,
                    (int)(size.X / 2f),
                    (int)(size.Y / 2f)
                );
            }
        );
    }

    private static void DrawTarget() {
        if (Target == null || Target.IsDisposed) {
            return;
        }

        Main.spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix
        );

        Main.spriteBatch.Draw(Target, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

        Main.spriteBatch.End();
    }
}
