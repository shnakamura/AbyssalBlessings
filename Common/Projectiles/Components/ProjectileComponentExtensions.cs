using System;
using Terraria;

namespace AbyssalBlessings.Common.Projectiles.Components;

/// <summary>
///     Provides basic projectile component extensions.
/// </summary>
public static class ProjectileComponentExtensions
{
    /// <summary>
    ///     Attempts to enable a projectile component of type <typeparamref name="T" /> on a given projectile and optionally initializes it.
    /// </summary>
    /// <typeparam name="T">The type of the component to be enabled.</typeparam>
    /// <param name="item">The projectile on which the component will be enabled.</param>
    /// <param name="initializer">An optional action to initialize the component.</param>
    /// <returns><c>true</c> if the component was successfully enabled; otherwise, <c>false</c>.</returns>
    public static bool TryEnableComponent<T>(this Projectile item, Action<T> initializer = null) where T : ProjectileComponent {
        if (!item.TryGetGlobalProjectile(out T component)) {
            return false;
        }

        component.Enabled = true;

        initializer?.Invoke(component);

        return true;
    }
}
