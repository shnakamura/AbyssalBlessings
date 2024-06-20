using AbyssalBlessings.Content.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Players;

public sealed class PlayerZephyrEffects : ModPlayer
{
    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
        HandleEffects();
    }

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
        HandleEffects();
    }

    private void HandleEffects() {
        if (Player.ownedProjectileCounts[ModContent.ProjectileType<ZephyrSquid>()] <= 0) {
            return;
        }

        var index = -1;

        foreach (var projectile in Main.ActiveProjectiles) {
            if (projectile.active && projectile.owner == Player.whoAmI && projectile.type == ModContent.ProjectileType<ZephyrSquid>()) {
                index = projectile.whoAmI;
                break;
            }
        }

        if (index <= -1) {
            return;
        }

        var pet = Main.projectile[index];

        if (pet.ModProjectile is not ZephyrSquid squid) {
            return;
        }

        squid.TriggerEffects();
    }
}
