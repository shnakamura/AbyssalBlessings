using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Pets;

public class Siren : ModProjectile
{
    /// <summary>
    ///     The projectile's minimum distance in pixel units required for teleporting to the owner.
    /// </summary>
    public const float MinTeleportDistance = 100f * 16f;
    
    public override void SetStaticDefaults() {
        Main.projPet[Type] = true;

        ProjectileID.Sets.LightPet[Type] = true;
    }

    public override void SetDefaults() {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 30;
        Projectile.height = 30;

        Projectile.alpha = 255;

        Projectile.penetrate = -1;
    }

    public override void AI() {
        Projectile.alpha = (int)MathHelper.Clamp(Projectile.alpha, 0, 255);

        Projectile.rotation = Projectile.velocity.X * 0.1f;
        
        FadeIn();
        
        if (!Projectile.TryGetOwner(out var owner) || !owner.HasBuff<Buffs.Siren>()) {
            FadeOut();
            return;
        }
        
        Projectile.timeLeft = 2;

        UpdateMovement(owner);
        
        Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.5f);

        if (Projectile.DistanceSQ(owner.Center) <= MinTeleportDistance * MinTeleportDistance) {
            return;
        }

        Projectile.Center = owner.Center;
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
    
    private void UpdateMovement(Player owner) {
        var addon = Vector2.Zero;
        var boost = 8f * 16f;

        if (owner.controlLeft) {
            addon.X -= boost;
        }

        if (owner.controlRight) {
            addon.X += boost;
        }

        if (owner.controlUp) {
            addon.Y -= boost;
        }

        if (owner.controlDown) {
            addon.Y += boost * 2;
        }
        
        var position = owner.Center - new Vector2(0f, 8f * 16f) + addon;
        var direction = Projectile.DirectionTo(position);
        
        var speed = 12f;
        var inertia = 10f;

        var difference = position - Projectile.Center;
        var length = difference.LengthSquared();
        
        var threshold = 32f;

        if (length > threshold * threshold) {
            Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * speed) / inertia;
        }
        else {
            Projectile.velocity *= 0.9f;
        }
    }
}
