using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Audio;

/// <summary>
///     Provides implementations for custom audio filters.
/// </summary>
[Autoload(Side = ModSide.Client)]
public interface IAudioFilter : ILoadable
{
    /// <summary>
    ///     Whether the audio filter is enabled or not.
    /// </summary>
    bool Enabled { get; }
    
    /// <summary>
    ///     Applies the audio filter to a given sound effect instance.
    /// </summary>
    /// <param name="instance">The sound effect instance to apply.</param>
    /// <param name="parameters">The sound parameters to apply.</param>
    /// <remarks>This automatically checks for <see cref="Enabled"/> before applying the audio filter.</remarks>
    void Apply(SoundEffectInstance instance, in AudioParameters parameters);
}
