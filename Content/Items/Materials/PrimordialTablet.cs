using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Materials;

public class PrimordialTablet : ModItem
{
    public override void SetDefaults() {
        Item.maxStack = Item.CommonMaxStack;

        Item.width = 66;
        Item.height = 56;
    }
}
