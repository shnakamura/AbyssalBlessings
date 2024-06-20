using Terraria;

namespace AbyssalBlessings.Utilities.Extensions;

/// <summary>
///     Provides basic <see cref="Player" /> extensions.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    ///     Checks if the player has drowning collision.
    /// </summary>
    /// <remarks>
    ///     This only accounts for player collision, not actual drowning.
    /// </remarks>
    /// <param name="player">The player to check.</param>
    /// <returns><c>true</c> if the player has drowning collision; otherwise, <c>false</c>.</returns>
    public static bool IsDrowning(this Player player) {
        return Collision.DrownCollision(player.position, player.width, player.height, player.gravDir);
    }
}
