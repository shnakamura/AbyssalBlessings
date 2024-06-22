using AbyssalBlessings.Common.Projectiles.Components;
using Terraria;
using Terraria.DataStructures;

namespace AbyssalBlessings.Common.Graphics;

/// <summary>
///     Handles the fading out of a <see cref="Projectile"/>.
/// </summary>
public sealed class ProjectileFadeOut : ProjectileComponent
{
    public struct FadeData
    {
        /// <summary>
        ///     The step amount used to increase the opacity.
        /// </summary>
        public int StepAmount { get; set; } = 5;

        public FadeData() { }
    }

    public FadeData Data = new();

    public override void AI(Projectile projectile) {
        if (!Enabled) {
            return;
        }
    }
}
