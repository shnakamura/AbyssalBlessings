using System;
using AbyssalBlessings.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Pets;

public class Siren : ModProjectile
{
    private ref float Timer => ref Projectile.ai[0];

    private Player Owner => Main.player[Projectile.owner];
    
    public override void SetStaticDefaults() {
        Main.projPet[Type] = true;
        
        ProjectileID.Sets.LightPet[Type] = true;
    }

    public override void SetDefaults() {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;
        
        Projectile.width = 30;
        Projectile.height = 30;
        
        Projectile.penetrate = -1;
    }

    public override void AI() {
        if (!Owner.active || Owner.dead || Owner.ghost || !Owner.HasBuff<Buffs.Siren>()) {
            Projectile.Kill();
            return;
        }
        
        Projectile.timeLeft = 2;
        
        Timer++;
        
        UpdateMovement();
        UpdateTeleport();
        
        Lighting.AddLight(Projectile.Center, 0f, 0.3f, 0.7f);
    }

    private void UpdateMovement() {
        var offset = new Vector2(
            0f,
            TileUtils.ToPixels(4) + MathF.Sin(Timer * 0.05f) * TileUtils.ToPixels(1) / 2f
        );

        var center = Owner.Center - offset;
        var distance = Vector2.DistanceSquared(Projectile.Center, center);

        var addon = Vector2.Zero;
        var boost = TileUtils.ToPixels(4);
        
        if (Owner.controlLeft) {
            addon.X -= boost;
        }

        if (Owner.controlRight) {
            addon.X += boost;
        }

        if (Owner.controlUp) {
            addon.Y -= boost;
        }

        if (Owner.controlDown) {
            addon.Y += boost * 2;
        }
        
        if (distance > TileUtils.ToPixels(10) * TileUtils.ToPixels(10)) {
            var velocity = Projectile.DirectionTo(center + addon) * 10f;

            Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, velocity, 0.15f);
        }
        else {
            Projectile.Center = Vector2.SmoothStep(Projectile.Center, center + addon, 0.15f);
            Projectile.velocity *= 0.95f;
        }

        var rotation = Projectile.velocity.X * 0.1f;

        Projectile.rotation = Projectile.rotation.AngleLerp(rotation, 0.1f);
    }

    private void UpdateTeleport() {
        var distance = Projectile.DistanceSQ(Owner.Center);
        var minDistance = TileUtils.ToPixels(100) * TileUtils.ToPixels(100);

        if (distance <= minDistance) {
            return;
        }

        Projectile.Center = Owner.Center;
    }
}
