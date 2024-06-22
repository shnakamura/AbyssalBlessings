using AbyssalBlessings.Content.Projectiles.Melee;
using CalamityMod.Items;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Weapons.Melee;

public class AquaticDissolution : ModItem
{
    public override void SetStaticDefaults() {
        // ((ModItem)this).DisplayName.SetDefault("Aquatic Dissolution");
        // ((ModItem)this).Tooltip.SetDefault("Fires whaling spears from the sky that bounce off tiles");
    }

    public override void SetDefaults() {
        Item.autoReuse = true;
        Item.useTurn = true;

        Item.DamageType = DamageClass.Melee;
        Item.damage = 125;
        Item.knockBack = 6f;

        Item.width = 50;
        Item.height = 72;

        Item.UseSound = SoundID.Item60;
        Item.useAnimation = 16;
        Item.useTime = 16;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.shootSpeed = 12f;
        Item.shoot = ModContent.ProjectileType<OceanBeam>();

        Item.rare = ModContent.RarityType<Turquoise>();
        Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
    }

    public override void ModifyShootStats(
        Player player,
        ref Vector2 position,
        ref Vector2 velocity,
        ref int type,
        ref int damage,
        ref float knockback
    ) {
        position = Main.MouseWorld;
    }

    public override bool Shoot(
        Player player,
        EntitySource_ItemUse_WithAmmo source,
        Vector2 position,
        Vector2 velocity,
        int type,
        int damage,
        float knockback
    ) {
        for (var i = 0; i < 3; i++) {
            Projectile.NewProjectile(
                source,
                position.X + Main.rand.Next(-30, 31),
                position.Y - 600f,
                0f,
                8f,
                type,
                damage,
                knockback,
                player.whoAmI
            );
        }

        return false;
    }

    public override void AddRecipes() { }

    public override void MeleeEffects(Player player, Rectangle hitbox) {
        if (!Main.rand.NextBool(5)) {
            return;
        }

        var dust = Dust.NewDustDirect(
            new Vector2(hitbox.X, hitbox.Y),
            hitbox.Width,
            hitbox.Height,
            DustID.Water,
            player.direction * 2f,
            0f,
            150,
            Main.DiscoColor,
            1.3f
        );

        dust.velocity *= 0.2f;
        dust.noGravity = true;
    }
}
