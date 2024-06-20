using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Weapons.Rogue;

public class PrimordialReaper : ModItem
{
    public override void SetDefaults() {
        Item.noMelee = true;
        Item.autoReuse = true;
        Item.noUseGraphic = true;

        Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        Item.damage = 100;
        Item.knockBack = 4f;

        Item.width = 80;
        Item.height = 84;

        Item.UseSound = SoundID.Item1;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.value = Item.sellPrice(gold: 56);

        Item.shootSpeed = 12f;
        Item.shoot = ModContent.ProjectileType<Projectiles.Rogue.PrimordialReaper>();
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
        var texture = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

        var position = new Vector2(
            Item.position.X - Main.screenPosition.X + Item.width / 2f,
            Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height / 2f
        );

        Main.GetItemDrawFrame(Item.type, out _, out var frame);

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
}
