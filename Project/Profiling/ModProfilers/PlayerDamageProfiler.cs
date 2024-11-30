using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public class PlayerDamageProfiler : ModPlayer
    {
        public static Dictionary<string, int> DamageReceivedFromNPCs = new Dictionary<string, int>();

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            string modName = npc.ModNPC != null ? npc.ModNPC.Mod.Name : "Vanilla";
            string npcName = npc.GivenOrTypeName;
            string key = $"{modName}:{npcName}";

            if (!DamageReceivedFromNPCs.ContainsKey(key))
            {
                DamageReceivedFromNPCs[key] = 0;
            }

            DamageReceivedFromNPCs[key] += hurtInfo.Damage;
        }
    }
}
