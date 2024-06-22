using AbyssalBlessings.Common.Projectiles.Components;
using Terraria;
using Terraria.DataStructures;

namespace AbyssalBlessings.Common.Graphics;

/// <summary>
///     Handles the fading in of a <see cref="Projectile"/>.
/// </summary>
public sealed class ProjectileFadeIn : ProjectileComponent
{
    public struct FadeData
    {
        /// <summary>
        ///     The initial opacity of the projectile.
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 (Visible) - 255 (Invisible).
        /// </remarks>
        public int InitialOpacity { get; set; } = 255;
        
        /// <summary>
        ///     The step amount used to increase the opacity.
        /// </summary>
        public int StepAmount { get; set; } = 5;
        
        /// <summary>
        ///     Whether the projectile is fading in or not.
        /// </summary>
        public bool Fading { get; set; }
        
        public FadeData() { }
    }

    public FadeData Data = new();

    public override void SetDefaults(Projectile entity) {
        if (!Enabled) {
            return;
        }

        entity.alpha = Data.InitialOpacity;
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source) {
        if (!Enabled) {
            return;
        }

        Data.Fading = true;
    }

    public override void OnKill(Projectile projectile, int timeLeft) {
        if (!Enabled) {
            return;
        }
        
        Data.Fading = false;
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
}
