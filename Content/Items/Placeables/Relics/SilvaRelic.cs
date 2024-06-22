using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Placeables.Relics;

public class SilvaRelic : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Relics.SilvaRelic>());

        Item.master = true;

        Item.width = 30;
        Item.height = 50;
        
        Item.rare = ItemRarityID.Master;
        Item.value = Item.buyPrice(gold: 5);
    }
}
