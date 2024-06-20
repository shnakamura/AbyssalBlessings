using System;
using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Common.Movement;
using AbyssalBlessings.Common.Projectiles.Components;
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

        Projectile.TryEnableComponent<ProjectileOwnerTeleport>(c => c.OnTeleport += (_, _) => {
            TriggerEffects();
        });

        Projectile.TryEnableComponent<ProjectileFadeRenderer>(c => c.Data.FadeOut = false);
    }

    public override void AI() {
        scale = Vector2.SmoothStep(scale, Vector2.One, 0.2f);

        Projectile.rotation = Projectile.velocity.X * 0.1f;

        if (!Projectile.TryGetOwner(out var owner)) {
            UpdateDeath();
            return;
        }
        
        UpdateMovement(owner);

        Projectile.timeLeft = 2;
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
    
    private void UpdateDeath() {
        if (Projectile.TryGetGlobalProjectile(out ProjectileFadeRenderer component)) {
            component.FadeOut();
        }
        else {
            Projectile.Kill();
        }

        Projectile.velocity *= 0.9f;
    }

    private void UpdateMovement(Player owner) {
        var position = owner.Center - new Vector2(64f * owner.direction, 32f);
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
