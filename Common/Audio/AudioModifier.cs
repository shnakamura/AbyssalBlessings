namespace AbyssalBlessings.Common.Audio;

public sealed class AudioModifier
{
    public delegate void ModifierCallback(ref AudioParameters parameters, float progress);

    /// <summary>
    ///     The modifier's context.
    /// </summary>
    /// <remarks>This acts as a unique identifier for modifiers.</remarks>
    public readonly string Context;

    /// <summary>
    ///     The modifier's callback for modifying audio parameters.
    /// </summary>
    /// <remarks>
    ///     This is triggered whenever the modifier is active.
    /// </remarks>
    public ModifierCallback Modifier;

    /// <summary>
    ///     The modifier's current time left in ticks.
    /// </summary>
    public int TimeLeft { get; set; }

    /// <summary>
    ///     The modifier's lifespan duration in ticks.
    /// </summary>
    public int TimeMax { get; set; }

    public AudioModifier(string context, int timeLeft, ModifierCallback modifier) {
        Context = context;
        TimeLeft = timeLeft;
        TimeMax = timeLeft;
        Modifier = modifier;
    }
}
