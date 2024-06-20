using System;
using CalamityMod.NPCs.PrimordialWyrm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.NPCs;

public sealed class PrimordialWyrmBody : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
        return entity.type == ModContent.NPCType<CalamityMod.NPCs.PrimordialWyrm.PrimordialWyrmBody>()
            || entity.type == ModContent.NPCType<PrimordialWyrmBodyAlt>();
    }

    public override void SetDefaults(NPC entity) {
        entity.width = 100;
        entity.height = 130;
    }

    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        var texture = Mod.Assets.Request<Texture2D>("Assets/Textures/NPCs/PrimordialWyrmBody").Value;

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

        var glow = Mod.Assets.Request<Texture2D>("Assets/Textures/NPCs/PrimordialWyrmBody_Glow").Value;

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

        var outline = Mod.Assets.Request<Texture2D>("Assets/Textures/NPCs/PrimordialWyrmBody_Outline").Value;

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
