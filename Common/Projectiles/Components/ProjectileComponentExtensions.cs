using System;
using Terraria;

namespace AbyssalBlessings.Common.Projectiles.Components;

/// <summary>
///     Provides basic projectile component extensions.
/// </summary>
public static class ProjectileComponentExtensions
{
    /// <summary>
    ///     Attempts to enable a component of type <typeparamref name="T"/> on the specified projectile and optionally initializes it.
    /// </summary>
    /// <typeparam name="T">The type of the projectile component.</typeparam>
    /// <param name="item">The projectile on which to enable the component.</param>
    /// <param name="initializer">The action to initialize the component.</param>
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