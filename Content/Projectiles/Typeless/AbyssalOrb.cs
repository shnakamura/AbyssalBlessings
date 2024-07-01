using System;
using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Common.Graphics.Renderers;
using AbyssalBlessings.Common.Graphics.Trails;
using AbyssalBlessings.Utilities.Extensions;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Typeless;

public class AbyssalOrb : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float MinAttackDistance = 16f * 16f;

    /// <summary>
    ///     The projectile's lifespan duration in tick units.
    /// </summary>
    /// <remarks>
    ///     This is what <see cref="Projectile" />.<see cref="Projectile.timeLeft" /> is initially set to.
    /// </remarks>
    public const int Lifespan = 120;

    /// <summary>
    ///     The projectile's charge duration in tick units.
    /// </summary>
    public const int Charge = 30;

    /// <summary>
    ///     The sound played when the projectile hits an enemy.
    /// </summary>
    public static readonly SoundStyle HitSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Custom/AbyssalOrbHit", 4) {
        PitchVariance = 0.5f,
        MaxInstances = 5,
        Volume = 0.5f
    };

    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailingMode[Type] = 3;
        ProjectileID.Sets.TrailCacheLength[Type] = 20;
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
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        SoundEngine.PlaySound(in HitSound, target.Center);
        
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        SoundEngine.PlaySound(in HitSound, target.Center);

        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        Projectile.rotation += Projectile.velocity.X * 0.05f;

        if (!Projectile.HasValidOwner()) {
            UpdateDeath();
            return;
        }
        
        UpdateOpacity();
        UpdateMovement();
    }

    public override bool PreDraw(ref Color lightColor) {
        var texture = ModContent.Request<Texture2D>(Texture).Value;
        
        Main.EntitySpriteDraw(
            texture,
            Projectile.GetDrawPosition(),
            Projectile.GetDrawFrame(),
            Projectile.GetAlpha(Color.White),
            Projectile.rotation,
            texture.Size() / 2f + Projectile.GetDrawOriginOffset(),
            Projectile.scale,
            SpriteEffects.None
        );
        
        PixellatedRenderer.Queue(
            () => {
                var bloom = ModContent.Request<Texture2D>($"{nameof(AbyssalBlessings)}/Assets/Textures/Effects/Bloom").Value;
        
                Main.EntitySpriteDraw(
                    bloom,
                    Projectile.GetPixellatedDrawPosition(),
                    null,
                    Projectile.GetAlpha(new Color(220, 149, 0, 0)) * 0.75f,
                    Projectile.rotation,
                    bloom.Size() / 2f + Projectile.GetDrawOriginOffset(),
                    Projectile.scale * 0.4f,
                    SpriteEffects.None
                );
                
                var trail = new DoubleColorTrail(
                    Projectile, 
                    new Color(220, 149, 0),
                    new Color(174, 84, 1),
                    static progress => MathHelper.Lerp(10f, 30f, progress)
                );
                
                trail.Draw();
            }
        );

        return false;
    }
    
    private void TriggerEffects() {
        for (var i = 0; i < 3; i++) {
            var particle = new GlowOrbParticle(
                Projectile.Center,
                Main.rand.NextVector2Circular(2f, 2f),
                false,
                60,
                0.75f,
                Projectile.GetAlpha(new Color(220, 149, 0, 0))
            );

            GeneralParticleHandler.SpawnParticle(particle);
        }
    }

    private void UpdateOpacity() {
        if (Projectile.timeLeft < 255 / 5) {
            Projectile.alpha += 5;
        }
        else {
            Projectile.alpha -= 5;
        }

        Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);
    }

    private void UpdateDeath() {
        Projectile.velocity *= 0.9f;

        Projectile.alpha += 5;

        if (Projectile.alpha < 255) {
            return;
        }
        
        Projectile.Kill();
    }
    
    private void UpdateMovement() {
        if (Projectile.timeLeft > Lifespan - Charge) {
            Projectile.velocity *= 0.85f;
            return;
        }

        var target = Projectile.FindTargetWithinRange(MinAttackDistance);

        if (target == null || !target.CanBeChasedBy()) {
            UpdateDeath();
            return;
        }

        var direction = Projectile.DirectionTo(target.Center);
        var perpendicular = new Vector2(-direction.Y, direction.X) * MathF.Sin(Projectile.timeLeft * 0.1f) * 4f;

        var velocity = direction * 12f + perpendicular;

        Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, velocity, 0.2f);
    }
}
