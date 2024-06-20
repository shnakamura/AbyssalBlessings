using System;
using Terraria;

namespace AbyssalBlessings.Common.Items.Components;

/// <summary>
///     Provides basic item component extensions.
/// </summary>
public static class ItemComponentExtensions
{
    /// <summary>
    ///     Attempts to enable a component of type <typeparamref name="T"/> on the specified item and optionally initializes it.
    /// </summary>
    /// <typeparam name="T">The type of the item component.</typeparam>
    /// <param name="item">The item on which to enable the component.</param>
    /// <param name="initializer">The action to initialize the component.</param>
    /// <returns><c>true</c> if the component was successfully enabled; otherwise, <c>false</c>.</returns>
    public static bool TryEnableComponent<T>(this Item item, Action<T> initializer = null) where T : ItemComponent {
        if (!item.TryGetGlobalItem(out T component)) {
            return false;
        }

        component.Enabled = true;

        initializer?.Invoke(component);

        return true;
    }
}