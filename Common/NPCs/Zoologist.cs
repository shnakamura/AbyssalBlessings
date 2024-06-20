using AbyssalBlessings.Content.Items.Accessories;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.NPCs;

public sealed class Zoologist : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
        return entity.type == NPCID.BestiaryGirl;
    }

    public override void ModifyShop(NPCShop shop) {
        shop.AddWithCustomValue<MagicalIceStone>(Item.buyPrice(1));
    }
}
