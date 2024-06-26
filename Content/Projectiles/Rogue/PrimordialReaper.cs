using AbyssalBlessings.Utilities.Extensions;
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

    public override bool PreDraw(ref Color lightColor) {
        var afterimage = ModContent.Request<Texture2D>(Texture + "_Afterimage").Value;

        var length = ProjectileID.Sets.TrailCacheLength[Type];
        
        for (var i = 0; i < length; i += 2) {
            var progress = 1f - i / (float)length;
            
            var color = Color.Lerp(new Color(255, 244, 0), new Color(93, 203, 243), progress);
            
            Main.EntitySpriteDraw(
                afterimage,
                Projectile.GetOldDrawPosition(i),
                Projectile.GetDrawFrame(),
                Projectile.GetAlpha(color) * progress,
                Projectile.oldRot[i],
                afterimage.Size() / 2f + Projectile.GetDrawOriginOffset(),
                Projectile.scale,
                SpriteEffects.None
            );
        }

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
        
        return false;
    }
}
