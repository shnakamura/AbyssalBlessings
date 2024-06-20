using System.Collections.Generic;
using System.Linq;
using AbyssalBlessings.Content.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace AbyssalBlessings.Common.Players;

public sealed class PlayerSpawnInventory : ModPlayer
{
    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
        if (Player.name != "NeoXZenith" && Player.name != "Everest") {
            return Enumerable.Empty<Item>();
        }
        
        return new[] {
            new Item(ModContent.ItemType<MagicalIceStone>())
        };
    }
}
