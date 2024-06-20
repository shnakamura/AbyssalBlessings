using CalamityMod.Items.LoreItems;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Lore;

public class PrimordialWyrmLore : LoreItem, ILocalizedModType
{
    // This is required because Calamity overrides the localization category for its items.
    string ILocalizedModType.LocalizationCategory { get; } = "Items";

    public override void SetDefaults() {
        base.SetDefaults();

        Item.width = 26;
        Item.height = 26;
    }
}
