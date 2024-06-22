using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Items.Weapons.Melee;

public class Mariana : ModItem
{
    public override void SetDefaults() {
        Item.autoReuse = true;
        Item.useTurn = true;

        Item.DamageType = DamageClass.Melee;
        Item.damage = 90;
        Item.scale = 1.5f;
        Item.knockBack = 6.5f;

        Item.width = 54;
        Item.height = 62;

        Item.UseSound = SoundID.Item1;
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.value = Item.buyPrice(gold: 60);
        Item.rare = ItemRarityID.Lime;
    }

    public override void AddRecipes() { }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
        var amount = Main.rand.Next(2, 4);

        for (var i = 0; i < amount; i++) {
            var velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));

            while (velocity.X == 0f && velocity.Y == 0f) {
                velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
            }

            velocity.Normalize();
            velocity *= Main.rand.Next(70, 101) * 0.1f;

            Projectile.NewProjectile(
                Item.GetSource_OnHurt(player),
                target.Center,
                velocity,
                ModContent.ProjectileType<Projectiles.Melee.Mariana>(),
                hit.Damage,
                hit.Knockback,
                player.whoAmI
            );
        }

        for (var i = 0; i < 30; i++) {
            var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueTorch, 0f, 0f, 100, default, 2f);

            dust.velocity *= 3f;

            if (!Main.rand.NextBool(2)) {
                continue;
            }

            dust.scale = 0.5f;
            dust.fadeIn = 1f + Main.rand.Next(10) * 0.1f;
        }

        for (var i = 0; i < 50; i++) {
            var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueTorch, 0f, 0f, 100, default, 3f);

            dust.noGravity = true;
            dust.velocity *= 5f;

            dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueTorch, 0f, 0f, 100, default, 2f);

            dust.velocity *= 2f;
        }
    }

    public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
        var amount = Main.rand.Next(2, 4);

        for (var i = 0; i < amount; i++) {
            var velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));

            while (velocity.X == 0f && velocity.Y == 0f) {
                velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
            }

            velocity.Normalize();
            velocity *= Main.rand.Next(70, 101) * 0.1f;

            Projectile.NewProjectile(
                Item.GetSource_OnHurt(player),
                target.Center,
                velocity,
                ModContent.ProjectileType<Projectiles.Melee.Mariana>(),
                hurtInfo.Damage,
                hurtInfo.Knockback,
                player.whoAmI
            );
        }

        for (var i = 0; i < 30; i++) {
            var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueTorch, 0f, 0f, 100, default, 2f);

            dust.velocity *= 3f;

            if (!Main.rand.NextBool(2)) {
                continue;
            }

            dust.scale = 0.5f;
            dust.fadeIn = 1f + Main.rand.Next(10) * 0.1f;
        }

        for (var i = 0; i < 50; i++) {
            var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueTorch, 0f, 0f, 100, default, 3f);

            dust.noGravity = true;
            dust.velocity *= 5f;

            dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueTorch, 0f, 0f, 100, default, 2f);

            dust.velocity *= 2f;
        }
    }

    public override void MeleeEffects(Player player, Rectangle hitbox) {
        if (!Main.rand.NextBool(3)) {
            return;
        }

        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.BlueTorch);
    }
}
