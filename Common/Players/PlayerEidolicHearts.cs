using AbyssalBlessings.Content.Items.Consumables;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AbyssalBlessings.Common.Players;

public sealed class PlayerEidolicHearts : ModPlayer
{
    /// <summary>
    ///     The tag Id for syncing eidolic hearts.
    /// </summary>
    private const string Tag = "EidolicHearts";

    /// <summary>
    ///     The amount of eidolic hearts consumed by the player.
    /// </summary>
    public int EidolicHeartsConsumed { get; set; }

    public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
        health = StatModifier.Default;
        health.Base = EidolicHeartsConsumed * EidolicHeart.HealthAddon;

        mana = StatModifier.Default;
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
        var packet = Mod.GetPacket();
        packet.Write(AbyssalBlessings.SyncEidolicHeart);
        packet.Write((byte)Player.whoAmI);
        packet.Write((byte)EidolicHeartsConsumed);
        packet.Send();
    }

    public override void CopyClientState(ModPlayer targetCopy) {
        if (targetCopy is not PlayerEidolicHearts modPlayer) {
            return;
        }

        modPlayer.EidolicHeartsConsumed = EidolicHeartsConsumed;
    }

    public override void SendClientChanges(ModPlayer clientPlayer) {
        if (clientPlayer is not PlayerEidolicHearts modPlayer || modPlayer.EidolicHeartsConsumed == EidolicHeartsConsumed) {
            return;
        }

        SyncPlayer(-1, Main.myPlayer, false);
    }

    public override void SaveData(TagCompound tag) {
        tag[Tag] = EidolicHeartsConsumed;
    }

    public override void LoadData(TagCompound tag) {
        EidolicHeartsConsumed = tag.GetInt(Tag);
    }
}
