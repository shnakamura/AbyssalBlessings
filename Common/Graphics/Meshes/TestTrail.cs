using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace AbyssalBlessings.Common.Graphics;

public struct TestTrail : IMesh
{
    private static VertexStrip Strip { get; } = new();

    public Vector2[] Positions { get; }
    
    public float[] Rotations { get; }
    
    public TestTrail(Projectile projectile) {
        Positions = projectile.oldPos;
        Rotations = projectile.oldRot;
    }
    
    void IMesh.Draw() {
        var data = GameShaders.Misc["RainbowRod"];

        data.Apply();
        
        Strip.PrepareStrip(
            Positions,
            Rotations,
            StripColor,
            StripWidth,
            -(Main.screenPosition + Main.ScreenSize.ToVector2() / 4f)
        );
        
        Strip.DrawTrail();
       
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }
    
    private static Color StripColor(float progressOnStrip) {
        var alpha = (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
        var result = Color.Lerp(Color.White, Color.Violet, Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true)) * alpha;
        
        result.A /= 2;
        
        return result;
    }

    private static float StripWidth(float progressOnStrip) {
        var progress = Utils.GetLerpValue(0f, 0.07f, progressOnStrip, true);

        return MathHelper.Lerp(26f, 32f, Utils.GetLerpValue(0f, 0.2f, progressOnStrip, true)) * progress;
    }
}
