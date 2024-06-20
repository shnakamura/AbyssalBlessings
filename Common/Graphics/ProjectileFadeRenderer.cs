using AbyssalBlessings.Common.Projectiles.Components;
using Microsoft.Xna.Framework;
using Terraria;

namespace AbyssalBlessings.Common.Graphics;

/// <summary>
///     Provides a component that handles a projectile's fading in and out.
/// </summary>
public sealed class ProjectileFadeRenderer : ProjectileComponent
{
    public sealed class FadeData
    {
        /// <summary>
        ///     The minimum opacity of the projectile. Ranges from 0 (Visible) - 255 (Invisible).
        /// </summary>
        public int MinimumOpacity { get; set; } = 0;

        /// <summary>
        ///     The maximum opacity of the projectile. Ranges from 0 (Visible) - 255 (Invisible).
        /// </summary>
        public int MaximumOpacity { get; set; } = 255;
    
        /// <summary>
        ///     The step amount for increasing/decreasing the opacity while performing a fade in/out.
        /// </summary>
        public int Amount { get; set; } = 5;
        
        /// <summary>
        ///     Whether the projectile should fade in or not.
        /// </summary>
        public bool FadeIn { get; set; } = true;
        
        /// <summary>
        ///     Whether the projectile is currently fading in or not.
        /// </summary>
        public bool FadingIn { get; set; }
        
        /// <summary>
        ///     Whether the projectile should fade out or not.
        /// </summary>
        public bool FadeOut { get; set; } = true;
        
        /// <summary>
        ///     Whether the projectile is currently fading out or not.
        /// </summary>
        public bool FadingOut { get; set; }
        
        /// <summary>
        ///     Whether the projectile should be killed upon reaching minimum opacity or not.
        /// </summary>
        public bool KillFadeOut { get; set; }
    }

    /// <summary>
    ///     The custom data used to apply the projectile's fades.
    /// </summary>
    public FadeData? Data { get; set; } = new();
    
    public override void AI(Projectile projectile) {
        if (!Enabled) {
            return;
        }

        if (projectile.timeLeft > 255 / Data.Amount) {
            Data.FadingIn = true;
            Data.FadingOut = false;
        }

        if (projectile.timeLeft < 255 / Data.Amount) {
            Data.FadingIn = false;
            Data.FadingOut = true;
        }
        
        if (Data.FadeIn && Data.FadingIn) {
            projectile.alpha -= Data.Amount;
        }

        if (Data.FadeOut && Data.FadingOut) {
            projectile.alpha += Data.Amount;
            
            if (!Data.KillFadeOut || projectile.alpha < Data.MaximumOpacity) {
                return;
            }
            
            projectile.Kill();
        }

        projectile.alpha = (int)MathHelper.Clamp(projectile.alpha, Data.MinimumOpacity, Data.MaximumOpacity);
    }
    
    /// <summary>
    ///     Forcefully triggers a projectile fade-in.
    /// </summary>
    public void FadeIn() {
        if (!Enabled) {
            return;
        }

        Data.FadingIn = true;
        Data.FadingOut = false;
    }

    /// <summary>
    ///     Forcefully triggers a projectile fade-out.
    /// </summary>
    /// <param name="kill">Whether the projectile should be killed upon reaching minimum opacity or not.</param>
    public void FadeOut(bool kill) {
        if (!Enabled) {
            return;
        }

        Data.FadingIn = false;
        Data.FadingOut = true;
        
        Data.KillFadeOut = kill;
    }
}
