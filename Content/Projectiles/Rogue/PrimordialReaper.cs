using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Rogue;

public class PrimordialReaper : ModProjectile
{
    public override void SetDefaults() {
        Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 50;
        Projectile.height = 50;

        Projectile.penetrate = -1;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        
    }
}
