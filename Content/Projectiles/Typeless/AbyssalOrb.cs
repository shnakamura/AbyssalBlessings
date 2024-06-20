using System;
using System.IO;
using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Common.Projectiles.Components;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Typeless;

public class AbyssalOrb : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float Distance = 16f * 16f;
    
    /// <summary>
    ///     The projectile's lifespan duration in tick units.
    /// </summary>
    /// <remarks>This is what <see cref="Projectile"/>.<see cref="Projectile.timeLeft"/> is initially set to.</remarks>
    public const int Lifespan = 120;

    /// <summary>
    ///     The projectile's charge duration in tick units.
    /// </summary>
    public const int Charge = 30;
    
    /// <summary>
    ///     The sound played when the projectile hits an enemy.
    /// </summary>
    public static readonly SoundStyle HitSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Custom/AbyssHit", 4) {
        PitchVariance = 0.5f,
        MaxInstances = 3,
        Volume = 0.7f
    };

    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
    }

    public override void SetDefaults() {
        Projectile.DamageType = DamageClass.Generic;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;
        
        Projectile.width = 10;
        Projectile.height = 10;

        Projectile.alpha = 255;
        Projectile.timeLeft = Lifespan;

        Projectile.TryEnableComponent<ProjectileFadeRenderer>();
    }
    
    public override void OnKill(int timeLeft) {
        var particle = new GlowOrbParticle(
            Projectile.Center + Main.rand.NextVector2Circular(4f, 4f),
            Main.rand.NextVector2Circular(4f, 4f),
            false,
            60,
            1f,
            Projectile.GetAlpha(new Color(255, 244, 0))
        );

        GeneralParticleHandler.SpawnParticle(particle);

        SoundEngine.PlaySound(in HitSound, Projectile.Center);
    }
    
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        Projectile.rotation += Projectile.velocity.X * 0.05f;
        
        if (Projectile.timeLeft > Lifespan - Charge) {
            Projectile.velocity *= 0.85f;
            return;
        }

        var target = Projectile.FindTargetWithinRange(Distance);

        if (target == null || !target.CanBeChasedBy()) {
            Projectile.velocity *= 0.85f;
            
            Projectile.GetGlobalProjectile<ProjectileFadeRenderer>().FadeOut(true);
            return;
        }
        
        var direction = Projectile.DirectionTo(target.Center);
        var perpendicular = new Vector2(-direction.Y, direction.X) * MathF.Sin(Projectile.timeLeft * 0.1f) * 4f;

        var velocity = direction * 12f + perpendicular;

        Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, velocity, 0.2f);
    }

    public override bool PreDraw(ref Color lightColor) {
        var texture = ModContent.Request<Texture2D>(Texture).Value;
        
        var position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

        var length = ProjectileID.Sets.TrailCacheLength[Type];
        
        var bloom = Mod.Assets.Request<Texture2D>("Assets/Textures/Effects/Bloom").Value;
        var bloomColor = new Color(255, 244, 0, 0);
        
        for (var i = 0; i < length; i += 2) {
            var trailPosition = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            var trailProgress = 1f - i / (float)length;

            Main.EntitySpriteDraw(
                bloom,
                trailPosition,
                null,
                Projectile.GetAlpha(bloomColor),
                Projectile.rotation,
                bloom.Size() / 2f,
                Projectile.scale / 6f * trailProgress,
                SpriteEffects.None
            );
            
            var trailColor = new Color(255, 255, 255, 0) * trailProgress;

            Main.EntitySpriteDraw(
                texture,
                trailPosition,
                null,
                Projectile.GetAlpha(trailColor),
                Projectile.rotation,
                texture.Size() / 2f,
                Projectile.scale * trailProgress,
                SpriteEffects.None
            );
        }
        
        Main.EntitySpriteDraw(
            bloom,
            position,
            null,
            Projectile.GetAlpha(bloomColor),
            Projectile.rotation,
            bloom.Size() / 2f,
            Projectile.scale / 6f,
            SpriteEffects.None
        );
        
        Main.EntitySpriteDraw(
            texture,
            position,
            null,
            Projectile.GetAlpha(Color.White),
            Projectile.rotation,
            texture.Size() / 2f,
            Projectile.scale,
            SpriteEffects.None
        );
        
        return false;
    }
}
