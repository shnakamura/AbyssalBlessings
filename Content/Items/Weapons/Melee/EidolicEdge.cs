using AbyssalBlessings.Content.Items.Materials;
using AbyssalBlessings.Content.Projectiles.Melee;
using AbyssalBlessings.Content.Rarities;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Weapons.Melee;

public class EidolicEdge : ModItem
{
    public override void SetDefaults() {
        Item.autoReuse = true;

        Item.DamageType = DamageClass.Melee;
        Item.damage = 610;
        Item.knockBack = 7;

        Item.width = 84;
        Item.height = 82;

        Item.UseSound = SoundID.Item1;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.value = Item.sellPrice(gold: 56);

        Item.shootSpeed = 10f;
        Item.shoot = ModContent.ProjectileType<EidolicEdgeSoul>();

        Item.rare = ModContent.RarityType<Abyssal>();
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
            var speedModifier = Main.rand.NextFloat(1f, 1.5f);
            var inertiaModifier = Main.rand.NextFloat(0.9f, 1f);

            Projectile.NewProjectile(
                source,
                position,
                velocity.RotatedBy(MathHelper.ToRadians(15f * i)),
                type,
                damage,
                knockback,
                player.whoAmI,
                speedModifier,
                inertiaModifier
            );
        }

        return false;
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
        var texture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

        var position = new Vector2(
            Item.position.X - Main.screenPosition.X + Item.width / 2f,
            Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height / 2f
        );

        Main.GetItemDrawFrame(Type, out _, out var frame);

        spriteBatch.Draw(
            texture,
            position,
            frame,
            Item.GetAlpha(Color.White),
            rotation,
            texture.Size() / 2f,
            scale,
            SpriteEffects.None,
            0f
        );
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<PrimordialTablet>(3)
            .AddIngredient<VoidEdge>()
            .AddIngredient<AuricBar>(2)
            .AddTile<DraedonsForge>()
            .Register();
    }
}
