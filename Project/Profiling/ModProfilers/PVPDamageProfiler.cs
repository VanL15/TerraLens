using System.Collections.Generic;
using System.Linq;

namespace TerraLens.Project.Profiling.ModProfilers
{
    public static class PvPDamageProfiler
    {
        // Example dictionaries; adjust based on your implementation
        public static Dictionary<string, int> ProjectilePvPDamage = new Dictionary<string, int>();
        public static Dictionary<string, int> ItemPvPDamage = new Dictionary<string, int>();

        public static Dictionary<string, int> MergePvPDamage()
        {
            var merged = new Dictionary<string, int>();

            foreach (var entry in ProjectilePvPDamage)
            {
                if (merged.ContainsKey(entry.Key))
                    merged[entry.Key] += entry.Value;
                else
                    merged[entry.Key] = entry.Value;
            }

            foreach (var entry in ItemPvPDamage)
            {
                if (merged.ContainsKey(entry.Key))
                    merged[entry.Key] += entry.Value;
                else
                    merged[entry.Key] = entry.Value;
            }

            return merged;
        }

        /// <summary>
        /// Call this method to update or reset PvP damage data as needed.
        /// </summary>
        public static void UpdatePvPDamages()
        {
            // Implement logic to update PvP damage counts
            // This could involve resetting counters, aggregating data, etc.
        }
    }
}
