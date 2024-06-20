namespace AbyssalBlessings.Utilities;

/// <summary>
///     Basic time utilities.
/// </summary>
public static class TimeUtils
{
    /// <summary>
    ///     The amount of ticks in a second.
    /// </summary>
    public const int TicksInSecond = 60;

    /// <summary>
    ///     Converts ticks to seconds.
    /// </summary>
    /// <param name="ticks">The amount of ticks.</param>
    /// <returns>The converted amount of ticks to seconds.</returns>
    public static int ToSeconds(int ticks) {
        return ticks * TicksInSecond;
    }
}
