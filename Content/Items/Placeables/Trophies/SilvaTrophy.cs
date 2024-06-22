using Terraria.ModLoader;
using Terraria;

namespace AbyssalBlessings.Content.Items.Placeables.Trophies;

public class SilvaTrophy : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Trophies.SilvaTrophy>());

        Item.width = 32;
        Item.height = 32;

        Item.value = Item.buyPrice(gold: 1);
    }
}
