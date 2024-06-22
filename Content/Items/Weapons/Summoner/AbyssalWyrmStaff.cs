using AbyssalBlessings.Content.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Weapons.Summoner;

public class AbyssalWyrmStaff : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Summon;

        Item.width = 84;
        Item.height = 86;
        
        Item.rare = ModContent.RarityType<Abyssal>();
    }

    public override void ModifyShootStats(
        Player player,
        ref Vector2 position,
        ref Vector2 velocity,
        ref int type,
        ref int damage,
        ref float knockback
    ) {
        position = Main.MouseWorld;
    }
}
