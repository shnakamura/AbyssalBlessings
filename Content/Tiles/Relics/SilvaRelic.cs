using CalamityMod.Tiles.BaseTiles;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Tiles.Relics;

public class SilvaRelic : BaseBossRelic
{
    public override string RelicTextureName => Texture;

    public override int AssociatedItem => ModContent.ItemType<Items.Placeables.Relics.SilvaRelic>();
    
    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }
}
