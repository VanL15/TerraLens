using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public static class ModEntityProfiler
    {
        public class EntityInfo
        {
            public int CurrentCount { get; set; }
            public int TotalSpawned { get; set; }
        }

        public static Dictionary<string, EntityInfo> NPCCounts = new Dictionary<string, EntityInfo>();
        public static Dictionary<string, EntityInfo> ProjectileCounts = new Dictionary<string, EntityInfo>();

        // To track previously active entities
        private static bool[] previousNPCActive = new bool[Main.npc.Length];
        private static bool[] previousProjectileActive = new bool[Main.projectile.Length];

        public static void UpdateNPCs()
        {
            // Initialize previousNPCActive if first run
            if (previousNPCActive.Length != Main.npc.Length)
            {
                previousNPCActive = new bool[Main.npc.Length];
            }

            // Reset current counts
            foreach (var key in NPCCounts.Keys.ToList())
            {
                NPCCounts[key].CurrentCount = 0;
            }

            // Update counts based on active NPCs
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active)
                {
                    string modName = npc.ModNPC != null ? npc.ModNPC.Mod.Name : "Vanilla";
                    string npcName = npc.GivenOrTypeName;
                    string key = $"{modName}:{npcName}";

                    if (!NPCCounts.ContainsKey(key))
                    {
                        NPCCounts[key] = new EntityInfo();
                    }

                    NPCCounts[key].CurrentCount++;

                    // Detect spawn event
                    if (!previousNPCActive[i])
                    {
                        NPCCounts[key].TotalSpawned++;
                        previousNPCActive[i] = true;
                    }
                }
                else
                {
                    previousNPCActive[i] = false;
                }
            }
        }

        public static void UpdateProjectiles()
        {
            // Initialize previousProjectileActive if first run
            if (previousProjectileActive.Length != Main.projectile.Length)
            {
                previousProjectileActive = new bool[Main.projectile.Length];
            }

            // Reset current counts
            foreach (var key in ProjectileCounts.Keys.ToList())
            {
                ProjectileCounts[key].CurrentCount = 0;
            }

            // Update counts based on active projectiles
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active)
                {
                    string modName = proj.ModProjectile != null ? proj.ModProjectile.Mod.Name : "Vanilla";
                    string projName = proj.Name;
                    string key = $"{modName}:{projName}";

                    if (!ProjectileCounts.ContainsKey(key))
                    {
                        ProjectileCounts[key] = new EntityInfo();
                    }

                    ProjectileCounts[key].CurrentCount++;

                    // Detect spawn event
                    if (!previousProjectileActive[i])
                    {
                        ProjectileCounts[key].TotalSpawned++;
                        previousProjectileActive[i] = true;
                    }
                }
                else
                {
                    previousProjectileActive[i] = false;
                }
            }
        }
    }
}
