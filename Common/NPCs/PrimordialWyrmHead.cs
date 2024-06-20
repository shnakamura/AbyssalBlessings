using AbyssalBlessings.Content.Items.Consumables;
using AbyssalBlessings.Content.Items.Materials;
using AbyssalBlessings.Content.Items.Weapons.Melee;
using AbyssalBlessings.Content.Items.Weapons.Rogue;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.NPCs;

public sealed class PrimordialWyrmHead : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
        return entity.type == ModContent.NPCType<CalamityMod.NPCs.PrimordialWyrm.PrimordialWyrmHead>();
    }

    public override void SetDefaults(NPC entity) {
        entity.width = 300;
        entity.height = 200;
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialTablet>()));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<XerocsKey>()));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AbyssalThrow>()));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrimordialReaper>()));
    }

    public override void DrawEffects(NPC npc, ref Color drawColor) {
        var particle = new GlowOrbParticle(
            npc.Center + Main.rand.NextVector2Circular(4f, 4f) - new Vector2(0f, 48f).RotatedBy(npc.rotation),
            Main.rand.NextVector2Circular(8f, 8f),
            false,
            60,
            1f,
            npc.GetAlpha(new Color(255, 244, 0))
        );

        GeneralParticleHandler.SpawnParticle(particle);
    }

    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        var texture = Mod.Assets.Request<Texture2D>("Assets/Textures/NPCs/PrimordialWyrmHead").Value;

        var position = npc.Center - Main.screenPosition + new Vector2(0f, npc.gfxOffY);

        Main.EntitySpriteDraw(
            texture,
            position,
            null,
            npc.GetAlpha(drawColor),
            npc.rotation,
            texture.Size() / 2f,
            npc.scale,
            SpriteEffects.None
        );

        var glow = Mod.Assets.Request<Texture2D>("Assets/Textures/NPCs/PrimordialWyrmHead_Glow").Value;

        spriteBatch.End();
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.Additive,
            Main.DefaultSamplerState,
            default,
            Main.Rasterizer,
            default,
            Main.GameViewMatrix.TransformationMatrix
        );

        Main.EntitySpriteDraw(
            glow,
            position,
            null,
            npc.GetAlpha(Color.White),
            npc.rotation,
            glow.Size() / 2f,
            npc.scale,
            SpriteEffects.None
        );

        spriteBatch.End();
        spriteBatch.Begin(
            default,
            default,
            Main.DefaultSamplerState,
            default,
            Main.Rasterizer,
            default,
            Main.GameViewMatrix.TransformationMatrix
        );

        var outline = Mod.Assets.Request<Texture2D>("Assets/Textures/NPCs/PrimordialWyrmHead_Outline").Value;

        Main.EntitySpriteDraw(
            outline,
            position,
            null,
            npc.GetAlpha(Color.White),
            npc.rotation,
            outline.Size() / 2f,
            npc.scale,
            SpriteEffects.None
        );

        return false;
    }
}
