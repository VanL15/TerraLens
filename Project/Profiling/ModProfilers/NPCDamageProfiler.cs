using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public class NPCDamageProfiler : GlobalNPC
    {
        public static Dictionary<string, int> DamageDealtToNPCs = new Dictionary<string, int>();

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if (hit.Damage <= 0)
                return;

            string modName = npc.ModNPC != null ? npc.ModNPC.Mod.Name : "Vanilla";
            string npcName = npc.GivenOrTypeName;
            string key = $"{modName}:{npcName}";

            if (!DamageDealtToNPCs.ContainsKey(key))
            {
                DamageDealtToNPCs[key] = 0;
            }

            DamageDealtToNPCs[key] += (int)hit.Damage;
        }
    }
}
