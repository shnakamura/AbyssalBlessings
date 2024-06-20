using System;
using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Common.Projectiles.Components;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
    public static readonly SoundStyle HitSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Custom/AbyssHit", 4) {
        PitchVariance = 0.5f,
        MaxInstances = 3,
        Volume = 0.7f
    };

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

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        Projectile.rotation += Projectile.velocity.X * 0.05f;

        if (!Projectile.TryGetOwner(out _)) {
            UpdateDeath();
            return;
        }

        UpdateMovement();
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

    private void UpdateMovement() {
        if (Projectile.timeLeft > Lifespan - Charge) {
            Projectile.velocity *= 0.85f;
            return;
        }

        var target = Projectile.FindTargetWithinRange(Distance);

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
