using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Buffs;

public class ZephyrSquid : ModBuff
{
    public override void SetStaticDefaults() {
        Main.vanityPet[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }
    
    public override void Update(Player player, ref int buffIndex) {
        var unused = false;
        
        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<Projectiles.Pets.ZephyrSquid>());
    }
}
