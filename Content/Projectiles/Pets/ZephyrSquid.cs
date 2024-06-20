using System;
using AbyssalBlessings.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Pets;

public class ZephyrSquid : ModProjectile
{
    private ref float Timer => ref Projectile.ai[0];
    
    private Player Owner => Main.player[Projectile.owner];

    public Vector2 Scale { get; set; } = Vector2.One;
    
    public override void SetStaticDefaults() {
        Main.projPet[Projectile.type] = true;
    }

    public override void SetDefaults() {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 30;
        Projectile.height = 30;
        
        Projectile.penetrate = -1;
    }

    public override void OnSpawn(IEntitySource source) {
        TriggerEffects();
    }

    public override void OnKill(int timeLeft) {
        TriggerEffects();
    }

    public override bool? CanCutTiles() {
        return true;
    }

    public override void AI() { 
        if (!Owner.active || Owner.dead || Owner.ghost || !Owner.HasBuff<Buffs.ZephyrSquid>()) {
            Projectile.Kill();
            return;
        }
        
        Projectile.timeLeft = 2;

        Timer++;

        UpdateMovement();
        UpdateTeleport();

        Projectile.rotation = Projectile.velocity.X * 0.1f;

        Scale = Vector2.SmoothStep(Scale, Vector2.One, 0.2f);
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
            lightColor,
            Projectile.rotation,
            origin,
            Scale,
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

        Scale /= 2f;
    }

    private void UpdateMovement() {
        var center = Owner.Center;
        var distance = Vector2.DistanceSquared(Projectile.Center, center);

        if (distance > 10f * 16f * 10f * 16f) {
            var velocity = Projectile.DirectionTo(center) * 10f;

            Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, velocity, 0.1f);
        }
        else {
            Projectile.Center = Vector2.SmoothStep(Projectile.Center, center, 0.1f);
            Projectile.velocity *= 0.95f;
        }
    }

    private void UpdateTeleport() {
        var distance = Projectile.DistanceSQ(Owner.Center);
        var minDistance = 100f * 16f * 100f * 16f;

        if (distance <= minDistance) {
            return;
        }

        Projectile.Center = Owner.Center;
    }
}
