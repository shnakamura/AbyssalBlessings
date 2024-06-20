using System;
using AbyssalBlessings.Common.Projectiles.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;

namespace AbyssalBlessings.Common.Graphics;

/// <summary>
///     Provides a component that handles rendering a projectile's afterimage textures.
/// </summary>
public sealed class ProjectileAfterimageRenderer : ProjectileComponent
{
    public sealed class AfterimageData
    {
        public delegate float ModifierDelegate(int index, int length);
        
        /// <summary>
        ///     The afterimage's texture.
        /// </summary>
        public Asset<Texture2D> Texture { get; set; }

        /// <summary>
        ///     The afterimage's origin.
        /// </summary>
        /// <remarks>Defaults to top right if not set.</remarks>
        public Vector2 Origin { get; set; }

        /// <summary>
        ///     The afterimage's frame.
        /// </summary>
        /// <remarks>Defaults to the texture's dimensions if not set.</remarks>
        public Rectangle? Frame { get; set; } = null;
        
        /// <summary>
        ///     The afterimage's color.
        /// </summary>
        /// <remarks>Defaults to the regular light color if not set.</remarks>
        public Color? Color { get; set; } = null;

        /// <summary>
        ///     The afterimage's step size.
        /// </summary>
        /// <remarks>Defaults to <c>1</c> if not set.</remarks>
        public int Step { get; set; } = 1;

        /// <summary>
        ///     Whether the afterimage's scale should be affected by <see cref="ScaleModifier"/>'s return value or not.
        /// </summary>
        /// <remarks>Defaults to <c>true</c>.</remarks>
        public bool ScaleUsesModifier { get; set; } = true;
        
        /// <summary>
        ///     Whether the afterimage's opacity should be affected by <see cref="OpacityModifier"/>'s return value or not.
        /// </summary>
        /// <remarks>Defaults to <c>true</c>.</remarks>
        public bool OpacityUsesModifier { get; set; } = true;
        
        /// <summary>
        ///     The afterimage's scale modifier.
        /// </summary>p
        /// <remarks>Defaults to <c>(index / length)</c>, which ranges from 1 - 0.</remarks>
        public ModifierDelegate ScaleModifier { get; set; } = (index, length) => 1f - index / (float)length;
        
        /// <summary>
        ///     The afterimage's opacity modifier.
        /// </summary>p
        /// <remarks>Defaults to <c>(index / length)</c>, which ranges from 1 - 0.</remarks>
        public ModifierDelegate OpacityModifier { get; set; } = (index, length) => 1f - index / (float)length;
    }

    /// <summary>
    ///     The custom data used to render the projectile's afterimages.
    /// </summary>
    public AfterimageData[] Data { get; set; }
    
    public override bool PreDraw(Projectile projectile, ref Color lightColor) {
        var draw = base.PreDraw(projectile, ref lightColor);
        
        if (!Enabled) {
            return draw;
        }

        var length = ProjectileID.Sets.TrailCacheLength[projectile.type];

        for (var i = 0; i < Data.Length; i++) {
            var data = Data[i];

            for (var j = 0; j < length; j += data.Step) {
                var position = projectile.oldPos[j] + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
                var modifier = data.ScaleModifier(j, length);

                var color = projectile.GetAlpha(data.Color ?? lightColor);

                if (data.OpacityUsesModifier) {
                    color *= modifier;
                }

                var scale = projectile.scale;

                if (data.ScaleUsesModifier) {
                    scale *= modifier;
                }

                Main.EntitySpriteDraw(
                    data.Texture.Value,
                    position,
                    data.Frame,
                    projectile.GetAlpha(color),
                    projectile.rotation,
                    data.Origin,
                    scale,
                    SpriteEffects.None
                );
            }
        }

        return draw;
    }
}
