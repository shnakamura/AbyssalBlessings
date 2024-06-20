using Terraria.Audio;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class WaterSoundEffects : ModPlayer
{
    /// <summary>
    ///     The sound used for water splashes.
    /// </summary>
    public static readonly SoundStyle WaterSplashSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Ambience/WaterSplash");

    public override void PostUpdate() {
        // The game sets Player.wetCount to 10 whenever the player exits/enters water. We check for 5 to make the splash play midway through.     
        if (Player.wetCount != 5) {
            return;
        }

        SoundEngine.PlaySound(in WaterSplashSound, Player.Center);
    }
}
