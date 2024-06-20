using AbyssalBlessings.Common.Projectiles.Components;
using Terraria;

namespace AbyssalBlessings.Common.Movement;

/// <summary>
///     
/// </summary>
/// <remarks>
///     This is generally used by minions.
/// </remarks>
public sealed class ProjectileFlyingMovement : ProjectileComponent
{
    public override void AI(Projectile projectile) {
        if (!Enabled) {
            return;
        }
    }
}
