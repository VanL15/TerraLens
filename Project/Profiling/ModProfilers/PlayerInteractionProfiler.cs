using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public class PlayerInteractionProfiler : ModPlayer
    {
        public static Dictionary<string, int> ItemUseCounts = new Dictionary<string, int>();

        private int previousItemAnimation = 0;

        public override void PreUpdate()
        {
            // Check if itemAnimation just started
            if (previousItemAnimation == 0 && Player.itemAnimation > 0)
            {
                // Player started using an item
                Item item = Player.HeldItem;
                string modName = item.ModItem != null ? item.ModItem.Mod.Name : "Vanilla";
                string itemName = item.Name;
                string key = $"{modName}:{itemName}";

                if (!ItemUseCounts.ContainsKey(key))
                {
                    ItemUseCounts[key] = 0;
                }

                ItemUseCounts[key]++;
            }

            previousItemAnimation = Player.itemAnimation;
        }
    }
}
