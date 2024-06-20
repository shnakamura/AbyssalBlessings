using AbyssalBlessings.Common.Projectiles.Components;
using Microsoft.Xna.Framework;
using Terraria;

namespace AbyssalBlessings.Common.Movement;

// TODO: Implement behavior.

/// <summary>
///     
/// </summary>
/// <remarks>
///     This is generally used by minions.
/// </remarks>
public sealed class ProjectileFlyingMovement : ProjectileComponent
{
    public sealed class MovementData;

    public MovementData? Data { get; set; } = new();
    
    public override void AI(Projectile projectile) {
        if (!Enabled) {
            return;
        }
    }
}
