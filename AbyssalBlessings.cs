using System.IO;
using AbyssalBlessings.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AbyssalBlessings;

public sealed class AbyssalBlessings : Mod
{
    /// <summary>
    ///     The packet Id for syncing player stats.
    /// </summary>
    public const byte SyncStats = 0;

    public override void HandlePacket(BinaryReader reader, int whoAmI) {
        var id = reader.ReadByte();

        switch (id) {
            case SyncStats:
                var index = reader.ReadByte();
                var amount = reader.ReadByte();

                var player = Main.player[index];

                if (!player.TryGetModPlayer(out PlayerEidolicHearts modPlayer)) {
                    return;
                }

                modPlayer.EidolicHeartsConsumed = amount;

                if (Main.netMode != NetmodeID.Server) {
                    return;
                }

                modPlayer.SyncPlayer(-1, whoAmI, false);

                break;
        }
    }
}
