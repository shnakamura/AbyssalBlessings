using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace AbyssalBlessings.Common.Graphics.Trails;

public struct DoubleColorTrail : ITrail
{
    private static readonly VertexStrip Strip = new();
    
    public delegate float TrailWidthCallback(float progress);
    
    public delegate Color TrailColorCallback(float progress);
    
    public Projectile Projectile { get; }

    public Color Start { get; }
    public Color End { get; }
    
    public TrailWidthCallback? WidthCallback { get; }
    public TrailColorCallback? ColorCallback { get; }

    public DoubleColorTrail(
        Projectile projectile, 
        Color start,
        Color end,
        TrailWidthCallback? widthCallback = null,
        TrailColorCallback? colorCallback = null
    ) {
        Projectile = projectile;
        Start = start;
        End = end;
        WidthCallback = widthCallback;
        ColorCallback = colorCallback;
    }
    
    public void Draw() {
        if (Projectile == null || !Projectile.active) {
            return;
        }
        
        var data = GameShaders.Misc["MagicMissile"];

        data.UseSaturation(-2.8f);
        data.UseOpacity(2f);
        
        data.Apply();

        Strip.PrepareStripWithProceduralPadding(
            Projectile.oldPos,
            Projectile.oldRot,
            GetStripColor,
            GetStripWidth,
            -Main.screenPosition + Projectile.Size / 2f
        );
        
        Strip.DrawTrail();
        
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }
    
    private Color GetStripColor(float progress) {
        var interpolation = Color.Lerp(
            Start,
            End,
            progress
        );

        var color = Projectile.GetAlpha(ColorCallback?.Invoke(progress) ?? interpolation * progress);
        
        return color;
    }

    private float GetStripWidth(float progress) {
        var width = WidthCallback?.Invoke(progress) ?? progress;
        
        return width;
    }
}
