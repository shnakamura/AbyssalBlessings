using AbyssalBlessings.Content.Projectiles.Typeless;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Melee;

public class AbyssalThrow : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float MinAttackDistance = 16f * 16f;

    public override void SetStaticDefaults() {
        ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = -1f;
        ProjectileID.Sets.YoyosMaximumRange[Type] = 400f;
        ProjectileID.Sets.YoyosTopSpeed[Type] = 18f;
    }

    public override void SetDefaults() {
        Projectile.DamageType = DamageClass.Melee;

        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.penetrate = -1;

        Projectile.aiStyle = ProjAIStyleID.Yoyo;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        var target = Projectile.FindTargetWithinRange(MinAttackDistance);

        if (target == null || !Main.rand.NextBool(10)) {
            return;
        }

        var velocity = new Vector2(10f).RotatedByRandom(MathHelper.TwoPi);

        Projectile.NewProjectile(
            Projectile.GetSource_FromAI(),
            Projectile.Center,
            velocity,
            ModContent.ProjectileType<AbyssalOrb>(),
            Projectile.damage,
            Projectile.knockBack,
            Projectile.owner
        );

        Projectile.netUpdate = true;
    }
}
