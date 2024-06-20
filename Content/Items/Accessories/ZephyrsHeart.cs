using AbyssalBlessings.Common.Players;
using AbyssalBlessings.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Accessories;

public class ZephyrsHeart : ModItem
{
    public override void SetDefaults() {
        Item.noUseGraphic = true;
        Item.useTurn = true;

        Item.width = 30;
        Item.height = 32;

        Item.useTime = 15;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;

        Item.buffType = ModContent.BuffType<ZephyrSquid>();
        Item.shoot = ModContent.ProjectileType<Projectiles.Pets.ZephyrSquid>();
    }

    public override bool? UseItem(Player player) {
        if (player.whoAmI == Main.myPlayer) {
            player.AddBuff(Item.buffType, 3600);
        }

        return true;
    }
}
