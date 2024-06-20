using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Accessories;

public class SirensPearl : ModItem
{
    public override void SetDefaults() {
        Item.noUseGraphic = true;
        Item.useTurn = true;

        Item.width = 22;
        Item.height = 24;
        
        Item.useTime = 15;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;

        Item.buffType = ModContent.BuffType<Content.Buffs.Siren>();
        Item.shoot = ModContent.ProjectileType<Content.Projectiles.Pets.Siren>();
    }

    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer) {
            player.AddBuff(Item.buffType, 3600);
        }
        
        return true;
    }
}
