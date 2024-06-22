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

        if (Projectile.timeLeft > 150) {
            return;
        }
        
        CalamityUtils.HomeInOnNPC(Projectile, false, 600f, 9f, 20f);
    }

    public override Color? GetAlpha(Color lightColor) {
        if (Projectile.timeLeft < 85) {
            var value = (byte)(Projectile.timeLeft * 3);
            var alpha = (byte)(100f * (value / 255f));
            
            return new Color(value, value, value, alpha);
        }

        return new Color(255, 255, 255, 100);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        SoundEngine.PlaySound(in SoundID.Item10, Projectile.Center);
        
        for (var i = 0; i < 12; i++) {
            var offset = Vector2.UnitX * (0f - Projectile.width) / 2f;
            
            offset += -Vector2.UnitY.RotatedBy(i * MathHelper.Pi / 6f) * new Vector2(8f, 16f);
            offset = offset.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
            
            var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.FireworkFountain_Blue, 0f, 0f, 160);
            
            dust.scale = 1.1f;
            dust.noGravity = true;
            dust.position = Projectile.Center + offset;
            dust.velocity = Projectile.velocity * 0.1f;
            dust.velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - dust.position) * 1.25f;
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
