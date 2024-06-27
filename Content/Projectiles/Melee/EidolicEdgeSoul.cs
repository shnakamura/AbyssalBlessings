using AbyssalBlessings.Common.Graphics;
using AbyssalBlessings.Utilities.Extensions;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ID;
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
    public const int Charge = 10;

    /// <summary>
    ///     The projectile's minimum distance in pixel units required for attacking.
    /// </summary>
    public const float MinAttackDistance = 32f * 16f;
    
    /// <summary>
    ///     The sound played when the projectile hits an enemy.
    /// </summary>
    public static readonly SoundStyle HitSound = new($"{nameof(AbyssalBlessings)}/Assets/Sounds/Custom/EidolicSoulHit") {
        PitchVariance = 0.5f,
        MaxInstances = 5,
        Volume = 0.75f
    };

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

        Projectile.width = 32;
        Projectile.height = 32;

        Projectile.alpha = 255;
        Projectile.timeLeft = Lifespan;

        Projectile.penetrate = 1;
        Projectile.extraUpdates = 1;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        SoundEngine.PlaySound(in HitSound, target.Center);

        TriggerEffects();
        
        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info) {
        SoundEngine.PlaySound(in HitSound, target.Center);
        
        TriggerEffects();

        target.AddBuff(ModContent.BuffType<CrushDepth>(), 3 * 60);
    }

    public override void AI() {
        Projectile.rotation += Projectile.velocity.X * 0.05f;

        if (!Projectile.HasValidOwner()) {
            UpdateDeath();
            return;
        }

        var owner = Main.player[Projectile.owner];
        
        UpdateMovement(owner);
        UpdateOpacity();
    }

    public override bool PreDraw(ref Color lightColor) {
        ModContent.GetInstance<MeshSystem>().Meshes.Add(new TestTrail(Projectile));
        
        var bloom = ModContent.Request<Texture2D>($"{nameof(AbyssalBlessings)}/Assets/Textures/Effects/Bloom").Value;
        
        Main.EntitySpriteDraw(
            bloom,
            Projectile.GetDrawPosition(),
            Projectile.GetDrawFrame(),
            Projectile.GetAlpha(new Color(93, 203, 243, 0)) * 0.75f,
            Projectile.rotation,
            bloom.Size() / 2f + Projectile.GetDrawOriginOffset(),
            Projectile.scale / 2f,
            SpriteEffects.None
        );
        
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

    private void TriggerEffects() {

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

    private void UpdateMovement(Player player) {
        if (InertiaModifier > 0f) {
            InertiaModifier -= 0.01f;
        }

        var speed = 12f * SpeedModifier;
        var inertia = MathHelper.Lerp(20f, 80f, InertiaModifier) * SpeedModifier;

        var target = Projectile.FindTargetWithinRange(MinAttackDistance);

        if (Projectile.timeLeft < Lifespan - Charge && target != null) {
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, MinAttackDistance, speed, inertia);
        }
        else if (Projectile.DistanceSQ(player.Center) > MinAttackDistance * MinAttackDistance) {
            var direction = Projectile.DirectionTo(player.Center);

            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * speed) / inertia;
        }
    }
}
