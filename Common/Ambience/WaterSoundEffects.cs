using AbyssalBlessings.Utilities;
using AbyssalBlessings.Utilities.Extensions;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Ambience;

[Autoload(Side = ModSide.Client)]
public sealed class WaterSoundEffects : ModPlayer
{
    private float volume;

    private SlotId submergedSoundSlot;
    
    public static readonly SoundStyle WaterSubmergedSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Ambience/WaterSubmergedLoop") {
        IsLooped = true,
        Volume = 0.75f
    };
    
    public static readonly SoundStyle WaterSplashSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Ambience/WaterSplash");

    public float Volume {
        get => volume;
        set => volume = MathHelper.Clamp(value, 0f, 1f);
    }
    
    public override void PostUpdate() {
        UpdateSubmerged();
        UpdateSplash();
    }

    private void UpdateSubmerged() {
        if (Player.IsDrowning()) {
            Volume += 0.01f;
        }
        else {
            Volume -= 0.05f;
        }

        if (Volume <= 0f) {
            return;
        }
        
        AudioUtils.UpdateSoundLoop(ref submergedSoundSlot, in WaterSubmergedSound, Volume);
    }

    private void UpdateSplash() {
        // The game sets Player.wetCount to 10 whenever the player exits/enters water. We check for 5 to make the splash play midway through.     
        if (Player.wetCount != 5) {
            return;
        }

        SoundEngine.PlaySound(in WaterSplashSound, Player.Center);
    }
}
