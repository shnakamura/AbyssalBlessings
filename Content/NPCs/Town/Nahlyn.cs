using System.Collections.Generic;
using AbyssalBlessings.Content.Items.Accessories;
using AbyssalBlessings.Content.Items.Weapons.Melee;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AbyssalBlessings.Content.NPCs.Town;

[AutoloadHead]
public class Nahlyn : ModNPC
{
    public override void SetStaticDefaults() {
        NPC.Happiness.SetNPCAffection<SEAHOE>(AffectionLevel.Love);
        NPC.Happiness.SetNPCAffection(NPCID.Pirate, AffectionLevel.Love);
        NPC.Happiness.SetNPCAffection<THIEF>(AffectionLevel.Like);
        NPC.Happiness.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like);
        NPC.Happiness.SetNPCAffection(NPCID.Stylist, AffectionLevel.Dislike);
        NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate);

        NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
        NPC.Happiness.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike);
        NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);

        var modifiers = new NPCID.Sets.NPCBestiaryDrawModifiers {
            Velocity = 1f,
            Direction = 1
        };

        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, modifiers);
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange(
            new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement($"Mods.{nameof(AbyssalBlessings)}.Bestiary.Nahlyn")
            }
        );
    }

    public override List<string> SetNPCNameList() {
        return new List<string> {
            "Nahlyn"
        };
    }

    public override void SetDefaults() {
        NPC.townNPC = true;
        NPC.friendly = true;

        NPC.lifeMax = 5000;
        NPC.defense = 10;

        NPC.width = 30;
        NPC.height = 50;

        NPC.knockBackResist = 0.8f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.aiStyle = NPCAIStyleID.Passive;
    }

    public override string GetChat() {
        var chat = new WeightedRandom<string>();

        return chat.Get();
    }

    public override void AddShops() {
        new NPCShop(Type)
            .AddWithCustomValue<ZephyrsHeart>(Item.buyPrice(1))
            .AddWithCustomValue<SirensPearl>(Item.buyPrice(gold: 10))
            .AddWithCustomValue<AnechoicCoating>(Item.buyPrice(silver: 60))
            .AddWithCustomValue<SulphurskinPotion>(Item.buyPrice(silver: 60))
            .AddWithCustomValue<BallOFugu>(Item.buyPrice(gold: 2))
            .AddWithCustomValue<Archerfish>(Item.buyPrice(gold: 2))
            .AddWithCustomValue<BlackAnurian>(Item.buyPrice(gold: 2))
            .AddWithCustomValue<HerringStaff>(Item.buyPrice(gold: 2))
            .AddWithCustomValue<Lionfish>(Item.buyPrice(gold: 2))
            .AddWithCustomValue<DeepDiver>(Item.buyPrice(1), CalamityConditions.DownedAquaticScourge)
            .AddWithCustomValue<SeasSearing>(Item.buyPrice(1), CalamityConditions.DownedAquaticScourge)
            .AddWithCustomValue<Lumenyl>(Item.buyPrice(silver: 20), CalamityConditions.DownedCalamitasClone)
            .AddWithCustomValue<DepthCells>(Item.buyPrice(silver: 20), CalamityConditions.DownedCalamitasClone)
            .AddWithCustomValue<TheCommunity>(Item.buyPrice(1), CalamityConditions.DownedLeviathan)
            .AddWithCustomValue<BrinyBaron>(Item.buyPrice(1), Condition.DownedDukeFishron)
            .AddWithCustomValue<TheOldReaper>(Item.buyPrice(1), CalamityConditions.DownedOldDuke)
            .AddWithCustomValue<HalibutCannon>(Item.buyPrice(gold: 50), CalamityConditions.DownedOldDuke)
            .AddWithCustomValue<AbyssalThrow>(Item.buyPrice(gold: 50), CalamityConditions.DownedPrimordialWyrm)
            .Register();
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        var position = NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY);

        var effects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        var trident = ModContent.Request<Texture2D>(Texture + "_Trident").Value;

        Main.EntitySpriteDraw(
            trident,
            position,
            null,
            NPC.GetAlpha(drawColor),
            NPC.rotation,
            trident.Size() / 2f,
            NPC.scale,
            effects
        );

        var texture = ModContent.Request<Texture2D>(Texture).Value;

        Main.EntitySpriteDraw(
            texture,
            position,
            null,
            NPC.GetAlpha(drawColor),
            NPC.rotation,
            texture.Size() / 2f,
            NPC.scale,
            effects
        );

        return false;
    }
}
