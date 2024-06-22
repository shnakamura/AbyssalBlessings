using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Melee;

public class Mariana : ModProjectile
{
    public override void SetDefaults() {
        Projectile.DamageType = DamageClass.Melee;

        Projectile.friendly = true;

        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.penetrate = 1;
        Projectile.timeLeft = 180;
    }

    public override bool? CanHitNPC(NPC target) {
        return Projectile.timeLeft < 150 && target.CanBeChasedBy(Projectile);
    }

    public override void AI() {
        Lighting.AddLight(Projectile.Center, 0f, 0.2f, 0.8f);

        if (Projectile.timeLeft < 150) {
            CalamityUtils.HomeInOnNPC(Projectile, false, 600f, 9f, 20f);
        }
    }

    public override Color? GetAlpha(Color lightColor) {
        if (Projectile.timeLeft < 85) {
            var b2 = (byte)(Projectile.timeLeft * 3);
            var a2 = (byte)(100f * (b2 / 255f));
            return new Color(b2, b2, b2, a2);
        }

        return new Color(255, 255, 255, 100);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        for (var i = 0; i < 12; i++) {
            var vector3 = Vector2.UnitX * (0f - Projectile.width) / 2f;
            vector3 += -Vector2.UnitY.RotatedBy(i * (float)Math.PI / 6f) * new Vector2(8f, 16f);
            vector3 = vector3.RotatedBy(Projectile.rotation - (float)Math.PI / 2f);
            var num9 = Dust.NewDust(Projectile.Center, 0, 0, 221, 0f, 0f, 160);
            Main.dust[num9].scale = 1.1f;
            Main.dust[num9].noGravity = true;
            Main.dust[num9].position = Projectile.Center + vector3;
            Main.dust[num9].velocity = Projectile.velocity * 0.1f;
            Main.dust[num9].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num9].position) * 1.25f;
        }

        if (Projectile.velocity.X != oldVelocity.X) {
            Projectile.velocity.X = 0f - oldVelocity.X;
        }

        if (Projectile.velocity.Y != oldVelocity.Y) {
            Projectile.velocity.Y = 0f - oldVelocity.Y;
        }

        return false;
    }
}
