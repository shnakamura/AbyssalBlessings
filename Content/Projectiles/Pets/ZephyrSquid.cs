using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Pets;

public class ZephyrSquid : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for teleporting to the owner.
    /// </summary>
    public const float MinTeleportDistance = 100f * 16f;

    private Vector2 scale;

    public override void SetStaticDefaults() {
        Main.projPet[Type] = true;
    }

    public override void SetDefaults() {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 30;
        Projectile.height = 30;

        Projectile.alpha = 255;

        Projectile.penetrate = -1;
    }

    public override void AI() {
        Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);

        scale = Vector2.SmoothStep(scale, Vector2.One, 0.2f);

        Projectile.rotation = Projectile.velocity.X * 0.1f;

        FadeIn();

        if (!Projectile.TryGetOwner(out var owner) || !owner.HasBuff<Buffs.ZephyrSquid>()) {
            FadeOut();
            return;
        }

        UpdateMovement(owner);

        Projectile.timeLeft = 2;

        if (Projectile.DistanceSQ(owner.Center) <= MinTeleportDistance * MinTeleportDistance) {
            return;
        }

        Projectile.Center = owner.Center;
    }

    public override bool PreDraw(ref Color lightColor) {
        var texture = TextureAssets.Projectile[Type].Value;
        var origin = texture.Size() / 2f;
        var position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

        var effects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Main.EntitySpriteDraw(
            texture,
            position,
            null,
            Projectile.GetAlpha(lightColor),
            Projectile.rotation,
            origin,
            scale,
            effects
        );

        return false;
    }

    public void TriggerEffects() {
        var count = 5;

        for (var i = 0; i < count; i++) {
            var velocity = new Vector2(i - count / 2f, -6f);
            var dust = Dust.NewDustPerfect(Projectile.Center, DustID.Water, velocity);

            dust.noGravity = true;
            dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
            dust.alpha = 100;
        }

        scale = new Vector2(scale.X * 2f, scale.Y / 4f);
    }

    private void FadeIn() {
        if (Projectile.alpha <= 0) {
            return;
        }

        Projectile.alpha -= 5;
    }

    private void FadeOut() {
        Projectile.alpha += 5;

        if (Projectile.alpha < 255) {
            return;
        }

        Projectile.Kill();
    }

    private void UpdateMovement(Player owner) {
        var position = owner.Center - new Vector2(4f * 16f * owner.direction, 2f * 16f);
        var direction = Projectile.DirectionTo(position);

        var speed = 8f;
        var inertia = 40f;

        var difference = position - Projectile.Center;
        var length = difference.LengthSquared();

        var threshold = 32f;

        if (length > threshold * threshold) {
            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * speed) / inertia;
        }
        else {
            Projectile.velocity *= 0.9f;
        }
    }
}
