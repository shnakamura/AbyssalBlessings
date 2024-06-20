using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Items.Components;

/// <summary>
///     Provides a basic implementation of a component that can be attached to an item.
/// </summary>
public abstract class ItemComponent : GlobalItem
{
    public sealed override bool InstancePerEntity { get; } = true;

    /// <summary>
    ///     Whether the component is enabled or not.
    /// </summary>
    public bool Enabled { get; set; }
}
