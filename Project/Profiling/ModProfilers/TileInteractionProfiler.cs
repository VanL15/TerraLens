using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public class TileInteractionProfiler : GlobalTile
    {
        // Dictionary to track the number of times each tile type has been placed
        public static Dictionary<int, int> TilesPlaced = new Dictionary<int, int>();

        // Optional: Dictionary to track the number of times each tile type has been mined
        public static Dictionary<int, int> TilesMined = new Dictionary<int, int>();

        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            if (TilesPlaced.ContainsKey(type))
            {
                TilesPlaced[type]++;
            }
            else
            {
                TilesPlaced[type] = 1;
            }

            base.PlaceInWorld(i, j, type, item);
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail && !effectOnly)
            {
                if (TilesMined.ContainsKey(type))
                {
                    TilesMined[type]++;
                }
                else
                {
                    TilesMined[type] = 1;
                }
            }

            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
}
