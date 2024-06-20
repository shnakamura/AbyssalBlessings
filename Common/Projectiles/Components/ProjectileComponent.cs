using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Projectiles.Components;

/// <summary>
///     Provides a basic implementation of a component that can be attached to a projectile.
/// </summary>
public abstract class ProjectileComponent : GlobalProjectile
{
    public sealed override bool InstancePerEntity { get; } = true;

    /// <summary>
    ///     Whether the component is enabled or not.
    /// </summary>
    public bool Enabled { get; set; }
}
