using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public class GlobalItemPvPDamageProfiler : GlobalItem
    {
        public static Dictionary<int, Dictionary<string, int>> PlayerPvPDamageDealt = new Dictionary<int, Dictionary<string, int>>();

        public override void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (player == null || target == null)
                return;

            int playerId = player.whoAmI;
            string targetName = target.name;
            string key = targetName;

            if (!PlayerPvPDamageDealt.ContainsKey(playerId))
            {
                PlayerPvPDamageDealt[playerId] = new Dictionary<string, int>();
            }

            if (!PlayerPvPDamageDealt[playerId].ContainsKey(key))
            {
                PlayerPvPDamageDealt[playerId][key] = 0;
            }

            PlayerPvPDamageDealt[playerId][key] += hurtInfo.Damage;
        }
    }
}
