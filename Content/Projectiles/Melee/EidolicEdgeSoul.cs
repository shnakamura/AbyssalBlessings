using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Melee;

public class EidolicEdgeSoul : ModProjectile
{
    /// <summary>
    ///     The projectile's lifespan duration.
    /// </summary>
    /// <remarks>
    ///     This is what <see cref="Projectile.timeLeft" /> is initially set to.
    /// </remarks>
    public const int Lifespan = 300;

    /// <summary>
    ///     The projectile's charge duration in tick units.
    /// </summary>
    public const int Charge = 20;

    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float Distance = 32f * 16f;

    /// <summary>
    ///     The projectile's speed modifier assigned by the item.
    /// </summary>
    public ref float SpeedModifier => ref Projectile.ai[0];

    /// <summary>
    ///     The projectile's inertia modifier assigned by the item.
    /// </summary>
    public ref float InertiaModifier => ref Projectile.ai[1];

    public override void SetDefaults() {
        Projectile.DamageType = DamageClass.Melee;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 32;
        Projectile.height = 32;

        Projectile.alpha = 255;
        Projectile.timeLeft = Lifespan;

        Projectile.penetrate = 1;
        Projectile.extraUpdates = 1;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);

        Projectile.rotation += Projectile.velocity.X * 0.05f;

        if (Projectile.timeLeft > 255 / 5) {
            FadeIn();
        }

        if (Projectile.timeLeft < 255 / 5) {
            FadeOut();
        }

        if (!Projectile.TryGetOwner(out var player)) {
            FadeOut();
            return;
        }

        UpdateMovement(player);
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

    private void UpdateMovement(Player player) {
        if (InertiaModifier > 0f) {
            InertiaModifier -= 0.01f;
        }

        var speed = 12f * SpeedModifier;
        var inertia = MathHelper.Lerp(20f, 80f, InertiaModifier) * SpeedModifier;

        var target = Projectile.FindTargetWithinRange(Distance);

        if (Projectile.timeLeft < Lifespan - Charge && target != null) {
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, Distance, speed, inertia);
        }
        else if (Projectile.DistanceSQ(player.Center) > Distance * Distance) {
            var direction = Projectile.DirectionTo(player.Center);

            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * speed) / inertia;
        }
    }
}
