using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Audio;

/// <summary>
///     Provides an audio filter contract used in <see cref="AudioSystem" />.
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
    /// <remarks>
    ///     This automatically checks for <see cref="Enabled" /> before applying the audio filter.
    /// </remarks>
    /// <param name="instance">The sound effect instance to apply.</param>
    /// <param name="parameters">The sound parameters to apply.</param>
    void Apply(SoundEffectInstance instance, in AudioParameters parameters);
}
