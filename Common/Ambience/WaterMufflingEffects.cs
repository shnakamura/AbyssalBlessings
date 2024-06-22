using AbyssalBlessings.Common.Audio;
using AbyssalBlessings.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class WaterMufflingEffects : ModPlayer
{
    private float intensity;

    /// <summary>
    ///     The current intensity for sound muffling while underwater.
    /// </summary>
    /// <remarks>
    ///     Ranges from 0 (None) - 1 (Full).
    /// </remarks>
    public float Intensity {
        get => intensity;
        set => intensity = MathHelper.Clamp(value, 0f, 1f);
    }

    public override void PostUpdate() {
        if (Player.IsDrowning()) {
            Intensity += 0.01f;
        }
        else {
            Intensity -= 0.05f;
        }

        if (Intensity < 0f) {
            return;
        }

        AudioSystem.AddModifier(
            $"{nameof(AbyssalBlessings)}:{nameof(WaterMufflingEffects)}",
            60,
            (ref AudioParameters parameters, float progress) => parameters.LowPass = Intensity * progress
        );
    }
}
