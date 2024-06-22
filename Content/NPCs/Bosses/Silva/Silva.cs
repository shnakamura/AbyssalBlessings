using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AbyssalBlessings.Content.NPCs.Bosses.Silva;

[AutoloadBossHead]
public sealed class Silva : ModNPC
{
    public override void SetDefaults() {
        NPC.noTileCollide = true;
        NPC.lavaImmune = true;
        NPC.noGravity = true;
        NPC.boss = true;

        NPC.lifeMax = 50000;
        NPC.defense = 50;
        
        NPC.width = 30;
        NPC.height = 50;

        NPC.knockBackResist = 0f;
        
        Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/Silva");
        SceneEffectPriority = SceneEffectPriority.BossHigh;
    }
}
