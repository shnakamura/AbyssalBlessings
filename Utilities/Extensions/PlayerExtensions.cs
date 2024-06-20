using Terraria;

namespace AbyssalBlessings.Utilities.Extensions;

/// <summary>
///     Basic player extensions.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    ///     Checks if the player has drowning collision.
    /// </summary>
    /// <remarks>This will not necessarily mean the player is drowning, only its collision.</remarks>
    /// <param name="player">The player to check.</param>
    /// <returns>Whether the player has drowning collision or not.</returns>
    public static bool IsDrowning(this Player player) {
        return Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
    }
}
