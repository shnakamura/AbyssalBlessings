using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace AbyssalBlessings.Utilities.Extensions;

/// <summary>
///     Provides basic <see cref="Projectile" /> extensions.
/// </summary>
public static class ProjectileExtensions
{
    /// <summary>
    ///     Gets the projectile's position as screen coordinates.
    /// </summary>
    /// <remarks>
    ///     This automatically accounts for draw offsets that are normally applied to projectiles.
    /// </remarks>
    /// <param name="projectile">The projectile instance to get the draw position from.</param>
    /// <param name="centered">Whether the draw position should be calculated using the projectile's center or not.</param>
    /// <returns>The projectile's position converted into screen cordinates.</returns>
    public static Vector2 GetDrawPosition(this Projectile projectile, bool centered = true) {
        return (centered ? projectile.Center : projectile.position)
            - Main.screenPosition
            + new Vector2(0f, projectile.gfxOffY)
            + new Vector2(projectile.ModProjectile == null ? 0f : projectile.ModProjectile.DrawOffsetX, 0f);
    }

    /// <summary>
    ///     Gets the projectile's old position at given index as screen coordinates.
    /// </summary>
    /// <remarks>
    ///     This automatically accounts for draw offsets that are normally applied to projectiles.
    /// </remarks>
    /// <param name="projectile">The projectile instance to get the draw position from.</param>
    /// <param name="i">The index of the current old position.</param>
    /// <param name="centered">Whether the draw position should be calculated using the projectile's center or not.</param>
    /// <returns>The projectile's old draw position at given index converted into screen coordinates.</returns>
    public static Vector2 GetOldDrawPosition(this Projectile projectile, int i, bool centered = true) {
        return projectile.oldPos[i]
            + (centered ? projectile.Size / 2f : Vector2.Zero)
            - Main.screenPosition
            + new Vector2(0f, projectile.gfxOffY)
            + new Vector2(projectile.ModProjectile == null ? 0f : projectile.ModProjectile.DrawOffsetX, 0f);
    }

    /// <summary>
    ///     Gets the projectile's origin offset.
    /// </summary>
    /// <remarks>
    ///     This is only applied to modded projectiles to ensure compatibility. Precisely, this returns a <see cref="Vector2" /> composed by
    ///     <see crepf="Terraria.ModLoader.ModPprojectile.DrawOriginOffsetX" /> and <see cref="Terraria.ModLoader.ModProjectile.DrawOriginOffsetY" />.
    /// </remarks>
    /// <param name="projectile">The projectile instance to get the origin offset from.</param>
    /// <returns>The projectile's origin offset.</returns>
    public static Vector2 GetDrawOriginOffset(this Projectile projectile) {
        return projectile.ModProjectile == null
            ? Vector2.Zero
            : new Vector2(projectile.ModProjectile.DrawOriginOffsetX, projectile.ModProjectile.DrawOriginOffsetY);
    }

    /// <summary>
    ///     Gets the projectile's source rectangle.
    /// </summary>
    /// <param name="projectile">The projectile instance to get the source rectangle from.</param>
    /// <returns>The projectile's source rectangle.</returns>
    public static Rectangle GetDrawFrame(this Projectile projectile) {
        return TextureAssets.Projectile[projectile.type].Value.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
    }

    /// <summary>
    ///     Whether the projectile has a valid owner or not.
    /// </summary>
    /// <remarks>
    ///     This checks if the projectile's owner is existent, is active, is not dead, and is not a ghost.
    /// </remarks>
    /// <param name="projectile">The projectile instance to check the validation from.</param>
    /// <returns><c>true</c> if the projectile has a valid owner; otherwise, <c>false</c>.</returns>
    public static bool HasValidOwner(this Projectile projectile) {
        var owner = Main.player[projectile.owner];

        return owner != null && owner.active && !owner.dead && !owner.ghost;
    }
}
