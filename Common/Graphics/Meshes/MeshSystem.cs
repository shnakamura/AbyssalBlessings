using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Graphics;

/// <summary>
///     Handles the registration and rendering of <see cref="IMesh"/> instances.
/// </summary>
[Autoload(Side = ModSide.Client)]
public sealed class MeshSystem : ModSystem
{
    /// <summary>
    ///     The meshes currently queued for rendering.
    /// </summary>
    public List<IMesh> Meshes { get; } = new();

    /// <summary>
    ///     The render target used for rendering meshes.
    /// </summary>
    /// <remarks>
    ///     This is mainly used for applying pixellation effects on meshes.
    /// </remarks>
    public RenderTarget2D Target { get; private set; }

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
            orig(self);

            if (Target == null || Target.IsDisposed) {
                return;
            }

            Main.spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointWrap,
                DepthStencilState.None,
                RasterizerState.CullNone
            );

            Main.spriteBatch.Draw(Target, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            Main.spriteBatch.End();
        };

        Main.OnPreDraw += DrawMeshes;
        Main.OnResolutionChanged += ResizeTarget;
    }

    public override void Unload() {
        Main.OnPreDraw -= DrawMeshes;
        Main.OnResolutionChanged -= ResizeTarget;
        
        Main.RunOnMainThread(() => Target?.Dispose());
    }

    private void ResizeTarget(Vector2 size) {
        Main.RunOnMainThread(
            () => {
                Target.Dispose();

                Target = new(
                    Main.graphics.GraphicsDevice,
                    (int)(size.X / 2),
                    (int)(size.Y / 2)
                );
            }
        );
    }

    private void DrawMeshes(GameTime gameTime) {
        var device = Main.graphics.GraphicsDevice;

        var bindings = device.GetRenderTargets();

        device.SetRenderTarget(Target);
        device.Clear(Color.Transparent);

        Main.spriteBatch.Begin(
            SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix
        );

        foreach (var mesh in Meshes) {
            mesh.Draw();
        }
        
        Main.spriteBatch.End();

        device.SetRenderTargets(bindings);

        Meshes.Clear();
    }
}
