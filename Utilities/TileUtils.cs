namespace AbyssalBlessings.Utilities;

/// <summary>
///     Basic tile utilities.
/// </summary>
public static class TileUtils
{
    /// <summary>
    ///     The size of a tile in pixels.
    /// </summary>
    public const float TilePixelSize = 16f;

    /// <summary>
    ///     Converts tile units to pixel units.
    /// </summary>
    /// <param name="tiles">The amount of tile units.</param>
    /// <returns>The converted amount of tile units as pixel units.</returns>
    public static float ToPixels(int tiles) {
        return tiles * TilePixelSize;
    }
}
