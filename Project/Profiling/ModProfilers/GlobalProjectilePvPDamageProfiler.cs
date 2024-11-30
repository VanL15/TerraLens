using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public class GlobalProjectilePvPDamageProfiler : GlobalProjectile
    {
        public static Dictionary<int, Dictionary<string, int>> PlayerPvPDamageDealt = new Dictionary<int, Dictionary<string, int>>();

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo hurtInfo)
        {
            Player attacker = Main.player[projectile.owner];
            if (attacker == null || target == null)
                return;

            int attackerId = attacker.whoAmI;
            string targetName = target.name;
            string key = targetName;

            if (!PlayerPvPDamageDealt.ContainsKey(attackerId))
            {
                PlayerPvPDamageDealt[attackerId] = new Dictionary<string, int>();
            }

            if (!PlayerPvPDamageDealt[attackerId].ContainsKey(key))
            {
                PlayerPvPDamageDealt[attackerId][key] = 0;
            }

            PlayerPvPDamageDealt[attackerId][key] += (int)hurtInfo.Damage;
        }
    }
}
