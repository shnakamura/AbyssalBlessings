using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Accessories;

public class MagicalIceStone : ModItem
{
    private sealed class MagicalIceStonePlayer : ModPlayer
    {
        /// <summary>
        ///     Whether the player is wearing the accessory or not.
        /// </summary>
        public bool Wearing { get; set; }

        public override void ResetEffects() {
            Wearing = false;
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
            if (!Wearing) {
                return;
            }

            var item = ModContent.GetInstance<MagicalIceStone>();

            Player.head = EquipLoader.GetEquipSlot(Mod, item.Name, EquipType.Head);
            Player.body = EquipLoader.GetEquipSlot(Mod, item.Name, EquipType.Body);
            Player.legs = EquipLoader.GetEquipSlot(Mod, item.Name, EquipType.Legs);
        }
    }

    public override void Load() {
        if (Main.netMode == NetmodeID.Server) {
            return;
        }

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetStaticDefaults() {
        var equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        var equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        var equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
        ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
        ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
        ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
    }

    public override void SetDefaults() {
        Item.accessory = true;
        Item.vanity = true;

        Item.width = 34;
        Item.height = 42;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (!player.TryGetModPlayer(out MagicalIceStonePlayer modPlayer)) {
            return;
        }

        modPlayer.Wearing = true;
    }
}
