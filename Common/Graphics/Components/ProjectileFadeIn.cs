using AbyssalBlessings.Common.Projectiles.Components;
using Terraria;
using Terraria.DataStructures;

namespace AbyssalBlessings.Common.Graphics.Components;

/// <summary>
///     Handles the fading in of a <see cref="Projectile"/>.
/// </summary>
public sealed class ProjectileFadeIn : ProjectileComponent
{
    public struct FadeData
    {
        /// <summary>
        ///     The step amount used to increase the opacity.
        /// </summary>
        public int StepAmount = 5;

        /// <summary>
        ///     Whether the projectile is fading in or not.
        /// </summary>
        public bool Fading;

        public FadeData() { }
    }

    public FadeData Data = new();

    public override void OnSpawn(Projectile projectile, IEntitySource source) {
        if (!Enabled) {
            return;
        }

        Data.Fading = true;
    }

    public override void AI(Projectile projectile) {
        if (!Enabled || !Data.Fading) {
            return;
        }

        projectile.alpha -= Data.StepAmount;

        if (projectile.alpha > 0) {
            return;
        }

        Data.Fading = false;
    }

    /// <summary>
    ///     Forcefully triggers a fade-in.
    /// </summary>
    public void Fade() {
        if (!Enabled) {
            return;
        }
        
        Data.Fading = true;
    }
}
