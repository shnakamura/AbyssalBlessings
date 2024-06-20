using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Common.Projectiles.Components;
using AbyssalBlessings.Content.Projectiles.Typeless;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Melee;

public class EidolicEdgeSoul : ModProjectile
{
    /// <summary>
    ///     The projectile's lifespan duration.
    /// </summary>
    /// <remarks>This is what <see cref="Projectile"/>.<see cref="Projectile.timeLeft"/> is initially set to.</remarks>
    public const int Lifespan = 600;

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
    
    public override void SetStaticDefaults() {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
    }
    
    public override void SetDefaults() {
        Projectile.DamageType = DamageClass.Melee;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;
        
        Projectile.width = 20;
        Projectile.height = 20;

        Projectile.alpha = 255;
        Projectile.timeLeft = Lifespan;
        
        Projectile.penetrate = 1;
        Projectile.extraUpdates = 1;
        
        Projectile.TryEnableComponent<ProjectileAfterimageRenderer>(c => {
            var baseTexture = ModContent.Request<Texture2D>(Texture + "_Afterimage");
            var baseData = new ProjectileAfterimageRenderer.AfterimageData() {
                Step = 2,
                Texture = baseTexture,
                Origin = baseTexture.Size() / 2f,
                Color = new Color(255, 244, 0, 0),
                OpacityModifier = static (index, length) => (index / (float)length) / 2f
            };
            
            var outlineTexture = ModContent.Request<Texture2D>(Texture + "_Outline");
            var outlineData = new ProjectileAfterimageRenderer.AfterimageData() {
                Step = 2,
                Texture = outlineTexture,
                Origin = outlineTexture.Size() / 2f
            };
            
            c.Data = new ProjectileAfterimageRenderer.AfterimageData[] {
                baseData,
                outlineData
            };
        });
        
        Projectile.TryEnableComponent<ProjectileFadeRenderer>();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }
    
    public override void AI() {
        if (InertiaModifier > 0f) {
            InertiaModifier -= 0.01f;
        }
        
        Projectile.rotation += Projectile.velocity.X * 0.05f;

        if (!Projectile.TryGetOwner(out var player)) {
            return;
        }
        
        var speed = 12f * SpeedModifier;
        var inertia = MathHelper.Lerp(20f, 80f, InertiaModifier) * SpeedModifier;
        
        var target = Projectile.FindTargetWithinRange(Distance);

        if (Projectile.timeLeft > Lifespan - Charge || Projectile.DistanceSQ(player.Center) > Distance * Distance) {
            var direction = Projectile.DirectionTo(player.Center);
                
            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * speed) / inertia;
        }
        else if (target != null) {
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, Distance, speed, inertia);
        }
    }

    public override bool PreDraw(ref Color lightColor) {
        var position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

        var bloom = Mod.Assets.Request<Texture2D>("Assets/Textures/Effects/Bloom").Value;
        var bloomColor = new Color(255, 244, 0, 0);

        Main.EntitySpriteDraw(
            bloom,
            position,
            null,
            Projectile.GetAlpha(bloomColor),
            Projectile.rotation,
            bloom.Size() / 2f,
            Projectile.scale / 3f,
            SpriteEffects.None
        );
        
        var texture = ModContent.Request<Texture2D>(Texture).Value;
        
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
