using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Melee;

public class OceanBeam : ModProjectile
{
    public override void SetStaticDefaults() {
        Main.projFrames[Projectile.type] = 4;

        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
    }

    public override void SetDefaults() {
        Projectile.DamageType = DamageClass.Melee;

        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.usesLocalNPCImmunity = true;

        Projectile.width = 14;
        Projectile.height = 14;

        Projectile.extraUpdates = 2;
        Projectile.penetrate = 3;
        Projectile.timeLeft = 600;
        Projectile.localNPCHitCooldown = 8;
    }

    public override void AI() {
        Projectile.frameCounter++;

        if (Projectile.frameCounter > 24) {
            Projectile.frame++;
            Projectile.frameCounter = 0;
        }

        if (Projectile.frame > 3) {
            Projectile.frame = 0;
        }

        Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);

        if (Projectile.ai[1]++ >= 60f) {
            Projectile.tileCollide = true;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        Projectile.penetrate--;

        if (Projectile.penetrate <= 0) {
            Projectile.Kill();
        }
        else {
            if (Projectile.velocity.X != oldVelocity.X) {
                Projectile.velocity.X = 0f - oldVelocity.X;
            }

            if (Projectile.velocity.Y != oldVelocity.Y) {
                Projectile.velocity.Y = 0f - oldVelocity.Y;
            }

            SoundEngine.PlaySound(in SoundID.Item10, Projectile.Center);
        }

        return false;
    }

    public override bool PreDraw(ref Color lightColor) {
        CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);

        return false;
    }

    public override void Kill(int timeLeft) {
        SoundEngine.PlaySound(in SoundID.Item10, Projectile.Center);

        for (var i = 0; i < 3; i++) {
            Dust.NewDust(
                Projectile.position + Projectile.velocity,
                Projectile.width,
                Projectile.height,
                DustID.BlueFairy,
                Projectile.oldVelocity.X * 0.25f,
                Projectile.oldVelocity.Y * 0.25f
            );

            Dust.NewDust(
                Projectile.position + Projectile.velocity,
                Projectile.width,
                Projectile.height,
                DustID.SilverCoin,
                Projectile.oldVelocity.X * 0.25f,
                Projectile.oldVelocity.Y * 0.25f
            );
        }
    }
}
