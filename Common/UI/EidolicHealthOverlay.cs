using System.Collections.Generic;
using AbyssalBlessings.Common.Players;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.UI;

public sealed class EidolicHealthOverlay : ModResourceOverlay
{
    private static readonly Dictionary<string, Asset<Texture2D>> VanillaAssetCache = new();

    public override void PostDrawResource(ResourceOverlayDrawContext context) {
        var asset = context.texture;

        var fancyPath = "Images/UI/PlayerResourceSets/FancyClassic/";
        var barPath = "Images/UI/PlayerResourceSets/HorizontalBars/";

        var player = Main.LocalPlayer;

        if (!player.TryGetModPlayer(out PlayerEidolicHearts modPlayer)) {
            return;
        }

        var hearts = modPlayer.EidolicHeartsConsumed;

        if (hearts <= 0 || context.resourceNumber >= hearts) {
            return;
        }

        if (asset == TextureAssets.Heart || asset == TextureAssets.Heart2 || CompareAssets(asset, fancyPath + "Heart_Fill") || CompareAssets(asset, fancyPath + "Heart_Fill_B")) {
            context.texture = Mod.Assets.Request<Texture2D>("Assets/Textures/UI/EidolicHeart_Heart");
            context.Draw();
        }
        else if (CompareAssets(asset, barPath + "HP_Fill") || CompareAssets(asset, barPath + "HP_Fill_Honey")) {
            context.texture = Mod.Assets.Request<Texture2D>("Assets/Textures/UI/EidolicHeart_Bar");
            context.Draw();
        }
    }

    private static bool CompareAssets(Asset<Texture2D> asset, string path) {
        if (!VanillaAssetCache.TryGetValue(path, out var value)) {
            asset = VanillaAssetCache[path] = Main.Assets.Request<Texture2D>(path);
        }

        return asset == value;
    }
}
