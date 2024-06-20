using AbyssalBlessings.Common.Projectiles.Components;
using Terraria;

namespace AbyssalBlessings.Common.Movement;

// TODO: Maybe give this a more generalized use, so it is not exclusive to player distances.

/// <summary>
///     Handles teleporting a <see cref="Projectile"/> if it's too far away from its owner.
/// </summary>
/// <remarks>
///     This is generally used by minions.
/// </remarks>
public sealed class ProjectileOwnerTeleport : ProjectileComponent
{
    public sealed class TeleportData
    {
        /// <summary>
        ///     The projectile's minimum distance in pixel units required for teleporting.
        /// </summary>
        public float Distance { get; set; } = 100f * 16f;
    }
    
    public delegate void TeleportCallback(Projectile projectile, Player owner);

    /// <summary>
    ///     The component's callback for projectile teleporting.
    /// </summary>
    /// <remarks>
    ///     This is triggered whenever the projectile teleports.
    /// </remarks>
    public event TeleportCallback? OnTeleport; 

    /// <summary>   
    ///     The custom data used to perform the projectile's teleports.
    /// </summary>
    public TeleportData? Data { get; set; } = new();

    public override void AI(Projectile projectile) {
        if (!Enabled || !projectile.TryGetOwner(out var owner)) {
            return;
        }

        var distance = projectile.DistanceSQ(owner.Center);

        if (distance < Data.Distance * Data.Distance) {
            return;
        }
        
        projectile.Center = owner.Center;
        
        OnTeleport?.Invoke(projectile, owner);
    }
}
