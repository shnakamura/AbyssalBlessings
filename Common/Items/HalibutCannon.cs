using AbyssalBlessings.Content.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Items;

public sealed class HalibutCannon : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
        return entity.type == ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.HalibutCannon>();
    }

    public override void SetDefaults(Item entity) {
        entity.rare = ModContent.RarityType<Abyssal>();
    }
}
