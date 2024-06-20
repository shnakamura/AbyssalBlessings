using AbyssalBlessings.Content.Projectiles.Typeless;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Melee;

public class AbyssalThrow : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float Distance = 16f * 16f;

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
        foreach (var npc in Main.ActiveNPCs) {
            if (!npc.active || !npc.CanBeChasedBy() || npc.DistanceSQ(Projectile.Center) > Distance * Distance) {
                continue;
            }
            
            if (!Main.rand.NextBool(10)) {
                continue;
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
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        var texture = ModContent.Request<Texture2D>(Texture).Value;
        
        var position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
        
        Main.EntitySpriteDraw(
            texture,
            position,
            null,
            Projectile.GetAlpha(lightColor),
            Projectile.rotation,
            texture.Size() / 2f,
            Projectile.scale,
            SpriteEffects.None
        );

        var glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
        var glowColor = new Color(255, 255, 255, 0);

        Main.EntitySpriteDraw(
            glow,
            position,
            null,
            Projectile.GetAlpha(glowColor),
            Projectile.rotation,
            glow.Size() / 2f,
            Projectile.scale,
            SpriteEffects.None
        );

        var outline = ModContent.Request<Texture2D>(Texture + "_Outline").Value;

        Main.EntitySpriteDraw(
            outline,
            position,
            null,
            Projectile.GetAlpha(Color.White),
            Projectile.rotation,
            outline.Size() / 2f,
            Projectile.scale,
            SpriteEffects.None
        );

        return false;
    }
}
