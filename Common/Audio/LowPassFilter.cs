using System;
using System.Reflection;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Audio;

public sealed class LowPassFilter : IAudioFilter
{
    private static readonly Action<SoundEffectInstance, float> ApplyLowPassFilterAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyLowPassFilter", BindingFlags.Instance | BindingFlags.NonPublic)
        .CreateDelegate<Action<SoundEffectInstance, float>>();
    
    public bool Enabled { get; private set; }

    void ILoadable.Load(Mod mod) {
        Enabled = false;
        
        if (!SoundEngine.IsAudioSupported) {
            mod.Logger.Warn($"{nameof(AudioManager)} was disabled: {nameof(SoundEngine)}.{nameof(SoundEngine.IsAudioSupported)} returned false.");
            return;
        }

        if (ApplyLowPassFilterAction == null) {
            mod.Logger.Warn($"{nameof(LowPassFilter)} was disabled: Failed to find internal FNA methods.");
            return;
        }
        
        Enabled = true;
    }

    void ILoadable.Unload() { }

    void IAudioFilter.Apply(SoundEffectInstance instance, in AudioParameters parameters) {
        if (!Enabled) {
            return;
        }
        
        var lowPass = parameters.LowPass;

        if (lowPass <= 0f || instance?.IsDisposed == true) {
            return;
        }

        ApplyLowPassFilterAction.Invoke(instance, 1f - lowPass);
    }
}
