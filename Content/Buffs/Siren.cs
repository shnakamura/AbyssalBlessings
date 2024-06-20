using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Buffs;

public class Siren : ModBuff
{
    public override void SetStaticDefaults() {
        Main.lightPet[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }
    
    public override void Update(Player player, ref int buffIndex) {
        var unused = false;
        
        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<Projectiles.Pets.Siren>());
    }
}
