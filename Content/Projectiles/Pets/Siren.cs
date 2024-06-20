using AbyssalBlessings.Common.Movement;
using AbyssalBlessings.Common.Projectiles.Components;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.Projectiles.Pets;

public class Siren : ModProjectile
{
    private ref float Timer => ref Projectile.ai[0];

    private Player Owner => Main.player[Projectile.owner];

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

        Projectile.penetrate = -1;

        Projectile.TryEnableComponent<ProjectileOwnerTeleport>();
    }

    public override void AI() {
        if (!Owner.active || Owner.dead || Owner.ghost || !Owner.HasBuff<Buffs.Siren>()) {
            Projectile.Kill();
            return;
        }

        Projectile.timeLeft = 2;

        Timer++;

        UpdateMovement();

        Lighting.AddLight(Projectile.Center, 0f, 0.3f, 0.7f);
    }

    private void UpdateMovement() {
        var center = Owner.Center - new Vector2(0f, 8f * 16f);
        var distance = Vector2.DistanceSquared(Projectile.Center, center);

        var addon = Vector2.Zero;
        var boost = 4f * 16f;

        if (Owner.controlLeft) {
            addon.X -= boost;
        }

        if (Owner.controlRight) {
            addon.X += boost;
        }

        if (Owner.controlUp) {
            addon.Y -= boost;
        }

        if (Owner.controlDown) {
            addon.Y += boost * 2;
        }
    }
}
