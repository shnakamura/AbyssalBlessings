namespace AbyssalBlessings.Common.Audio;

public struct AudioModifier
{
    public delegate void IntensityModifierDelegate(ref AudioParameters parameters, float progress);

    /// <summary>
    ///     The modifier's intensity modifier.
    /// </summary>
    public IntensityModifierDelegate IntensityModifier;

    /// <summary>
    ///     The modifier's current time left in ticks.
    /// </summary>
    public int TimeLeft { get; set; }
    
    /// <summary>
    ///     The modifier's lifespan duration.
    /// </summary>
    public int TimeMax { get; set; }

    /// <summary>
    ///     The modifier's context.
    /// </summary>
    /// <remarks>This behaves as a unique identifier for modifiers.</remarks>
    public readonly string Context;

    public AudioModifier(string context, int timeLeft, IntensityModifierDelegate intensityModifier) {
        Context = context;
        TimeLeft = timeLeft;
        TimeMax = timeLeft;
        IntensityModifier = intensityModifier;
    }
}
