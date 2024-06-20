using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Rogue;

public class PrimordialReaper : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float Distance = 16f * 16f;
    
    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
    }
    
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

    public override bool PreDraw(ref Color lightColor) {
        var afterimage = ModContent.Request<Texture2D>(Texture + "_Afterimage").Value;
        
        var length = ProjectileID.Sets.TrailCacheLength[Type];
        
        var outline = ModContent.Request<Texture2D>(Texture + "_Outline").Value;

        for (var i = 0; i < length; i += 2) {
            var trailPosition = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            var trailProgress = 1f - i / (float)length;

            var trailColor = new Color(255, 244, 0, 0) * trailProgress;

            Main.EntitySpriteDraw(
                afterimage,
                trailPosition,
                null,
                Projectile.GetAlpha(trailColor),
                Projectile.rotation,
                afterimage.Size() / 2f,
                Projectile.scale * trailProgress,
                SpriteEffects.None
            );

            Main.EntitySpriteDraw(
                outline,
                trailPosition,
                null,
                Projectile.GetAlpha(trailColor),
                Projectile.rotation,
                outline.Size() / 2f,
                Projectile.scale * trailProgress,
                SpriteEffects.None
            );
        }
        
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
