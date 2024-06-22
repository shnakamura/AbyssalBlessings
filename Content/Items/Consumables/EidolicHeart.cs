using AbyssalBlessings.Common.Players;
using AbyssalBlessings.Content.Items.Materials;
using AbyssalBlessings.Content.Rarities;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Consumables;

public class EidolicHeart : ModItem
{
    /// <summary>
    ///     The amount of health that this item increases.
    /// </summary>
    public const int HealthAddon = 100;

    public override void SetStaticDefaults() {
        ItemID.Sets.SortingPriorityBossSpawns[Type] = 20;
    }

    public override void SetDefaults() {
        Item.consumable = true;

        Item.maxStack = 20;

        Item.width = 44;
        Item.height = 38;

        Item.useTime = 15;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;
        
        Item.rare = ModContent.RarityType<Abyssal>();
    }

    public override bool CanUseItem(Player player) {
        if (!player.TryGetModPlayer(out CalamityPlayer modPlayer)) {
            return false;
        }

        var full = player.ConsumedLifeCrystals == Player.LifeCrystalMax && player.ConsumedLifeFruit == Player.LifeFruitMax;

        if (!full) {
            return false;
        }

        return !modPlayer.bOrange || !modPlayer.mFruit || !modPlayer.eBerry || modPlayer.dFruit;
    }

    public override bool? UseItem(Player player) {
        if (!player.TryGetModPlayer(out PlayerEidolicHearts modPlayer)) {
            return null;
        }

        player.UseHealthMaxIncreasingItem(HealthAddon);

        modPlayer.EidolicHeartsConsumed++;

        return true;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<PrimordialTablet>(3)
            .AddIngredient(ItemID.LifeFruit, 5)
            .AddIngredient<Lumenyl>(20)
            .AddIngredient<AscendantSpiritEssence>()
            .AddTile<DraedonsForge>()
            .Register();
    }
}
