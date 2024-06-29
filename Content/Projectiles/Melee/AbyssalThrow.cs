using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Common.Graphics.Trails;
using AbyssalBlessings.Content.Projectiles.Typeless;
using AbyssalBlessings.Utilities.Extensions;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        
        ProjectileID.Sets.TrailingMode[Type] = 3;
        ProjectileID.Sets.TrailCacheLength[Type] = 25;
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
        TriggerEffects();
        
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        TriggerEffects();
        
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

    public override bool PreDraw(ref Color lightColor) {
        var bloom = ModContent.Request<Texture2D>($"{nameof(AbyssalBlessings)}/Assets/Textures/Effects/Bloom").Value;
        
        Main.EntitySpriteDraw(
            bloom,
            Projectile.GetDrawPosition(),
            null,
            Projectile.GetAlpha(new Color(93, 203, 243, 0)) * 0.75f,
            Projectile.rotation,
            bloom.Size() / 2f + Projectile.GetDrawOriginOffset(),
            Projectile.scale * 0.7f,
            SpriteEffects.None
        );

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
                var trail = new DoubleColorTrail(
                    Projectile, 
                    new Color(93, 203, 243),
                    new Color(72, 135, 205),
                    static progress => 20f * progress
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
                Projectile.GetAlpha(new Color(93, 203, 243))
            );

            GeneralParticleHandler.SpawnParticle(particle);
        }
    }
}
