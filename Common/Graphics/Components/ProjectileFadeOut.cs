using AbyssalBlessings.Common.Projectiles.Components;
using Terraria;

namespace AbyssalBlessings.Common.Graphics.Components;

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
        public int StepAmount = 5;
        
        /// <summary>
        ///     Whether the projectile is fading in or not.
        /// </summary>
        public bool Fading;
        
        public FadeData() { }
    }

    public FadeData Data = new();

    public override void AI(Projectile projectile) {
        if (!Enabled) {
            return;
        }

        var shouldFade = !Data.Fading && projectile.timeLeft < 255 / Data.StepAmount;

        if (shouldFade) {
            Data.Fading = true;
        }

        if (!Data.Fading) {
            return;
        }
        
        projectile.alpha -= Data.StepAmount;

        if (projectile.alpha < 255) {
            return;
        }

        projectile.Kill();
    }
    
    /// <summary>
    ///     Forcefully triggers a fade-out.
    /// </summary>
    public void Fade() {
        if (!Enabled) {
            return;
        }
        
        Data.Fading = true;
    }
}
