using AbyssalBlessings.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Weapons.Melee;

public class AbyssalThrow : ModItem
{
    public override void SetStaticDefaults() {
        ItemID.Sets.Yoyo[Type] = true;
        ItemID.Sets.GamepadExtraRange[Item.type] = 15;
        ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
    }

    public override void SetDefaults() {
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.channel = true;

        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.damage = 250;
        Item.knockBack = 4f;

        Item.width = 46;
        Item.height = 42;

        Item.UseSound = SoundID.Item1;
        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Shoot;

        Item.value = Item.sellPrice(gold: 56);

        Item.shoot = ModContent.ProjectileType<Projectiles.Melee.AbyssalThrow>();

        Item.rare = ModContent.RarityType<Abyssal>();
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
}
