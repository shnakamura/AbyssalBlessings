using AbyssalBlessings.Common.Projectiles.Components;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace AbyssalBlessings.Common.Graphics;

/// <summary>
///     Handles the fading in and out of a <see cref="Projectile" />.
/// </summary>
public sealed class ProjectileFadeRenderer : ProjectileComponent
{
    public sealed class FadeData
    {
        /// <summary>
        ///     The minimum opacity of the projectile.
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 (Visible) - 255 (Invisible). Defaults to 0.
        /// </remarks>
        public int MinimumOpacity { get; set; } = 0;

        /// <summary>
        ///     The maximum opacity of the projectile.
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 (Visible) - 255 (Invisible). Defaults to 255.
        /// </remarks>
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
    }

    /// <summary>
    ///     The custom data used to perform the projectile's fades.
    /// </summary>
    public FadeData? Data { get; set; } = new();

    public override void OnSpawn(Projectile projectile, IEntitySource source) {
        if (!Enabled || !Data.FadeIn) {
            return;
        }

        Data.FadingIn = true;
        Data.FadingOut = false;
    }

    public override void AI(Projectile projectile) {
        if (!Enabled) {
            return;
        }

        UpdateFadeIn(projectile);
        UpdateFadeOut(projectile);

        projectile.alpha = (int)MathHelper.Clamp(projectile.alpha, Data.MinimumOpacity, Data.MaximumOpacity);
    }
    
    /// <summary>
    ///     Forcefully triggers a fade-in.
    /// </summary>
    public void FadeIn() {
        if (!Enabled) {
            return;
        }

        Data.FadingIn = true;
        Data.FadingOut = false;
    }

    /// <summary>
    ///     Forcefully triggers a fade-out.
    /// </summary>
    public void FadeOut() {
        if (!Enabled) {
            return;
        }

        Data.FadingIn = false;
        Data.FadingOut = true;
    }
    
    private void UpdateFadeIn(Projectile projectile) {
        if (!Data.FadeIn || !Data.FadingIn) {
            return;
        }

        projectile.alpha -= Data.Amount;

        if (projectile.alpha > Data.MinimumOpacity) {
            return;
        }

        Data.FadingIn = false;
    }

    private void UpdateFadeOut(Projectile projectile) {
        if (!Data.FadeOut) {
            return;
        }
        
        if (projectile.timeLeft < 255 / Data.Amount) {
            Data.FadingOut = true;
            Data.FadingIn = false;
        }
        
        if (!Data.FadingOut) {
            return;
        }
        
        projectile.alpha += Data.Amount;

        if (projectile.alpha < Data.MaximumOpacity) {
            return;
        }
        
        projectile.Kill();

        Data.FadingOut = false;
    }
}
