using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerraLens.Project.Profiling
{
    public class BiomeTimeProfiler : ModPlayer
    {
        public static Dictionary<string, double> BiomeTimeSpent = new Dictionary<string, double>();

        private string previousBiome = "";

        public override void PostUpdate()
        {
            string currentBiome = GetCurrentBiome();

            if (!BiomeTimeSpent.ContainsKey(currentBiome))
            {
                BiomeTimeSpent[currentBiome] = 0;
            }

            BiomeTimeSpent[currentBiome] += 1.0 / 60.0; // Assuming this method is called 60 times per second
        }

        private string GetCurrentBiome()
        {
            if (Player.ZoneDungeon) return "Dungeon";
            if (Player.ZoneJungle) return "Jungle";
            if (Player.ZoneCorrupt) return "Corruption";
            if (Player.ZoneCrimson) return "Crimson";
            if (Player.ZoneHallow) return "Hallow";
            if (Player.ZoneSnow) return "Snow";
            if (Player.ZoneDesert) return "Desert";
            if (Player.ZoneGlowshroom) return "Glowing Mushroom";
            if (Player.ZoneHallow) return "Hallow";
            if (Player.ZoneMeteor) return "Meteorite";
            if (Player.ZoneBeach) return "Beach";
            if (Player.ZoneUnderworldHeight) return "Underworld";
            if (Player.ZoneSkyHeight) return "Sky";
            if (Player.ZoneOverworldHeight) return "Overworld";
            if (Player.ZoneRockLayerHeight) return "Cavern";
            if (Player.ZoneDirtLayerHeight) return "Underground";
            if (Player.ZoneOldOneArmy) return "Old One's Army";
            if (Player.ZoneRain) return "Rain";
            if (Player.ZoneSandstorm) return "Sandstorm";
            if (Player.ZoneTowerSolar) return "Solar Pillar";
            if (Player.ZoneTowerVortex) return "Vortex Pillar";
            if (Player.ZoneTowerNebula) return "Nebula Pillar";
            if (Player.ZoneTowerStardust) return "Stardust Pillar";
            if (Player.ZonePeaceCandle) return "Peace Candle";
            if (Player.ZoneWaterCandle) return "Water Candle";
            if (Player.ZoneSandstorm) return "Sandstorm";
            if (Player.ZoneTowerSolar) return "Solar Pillar";
            if (Player.ZoneTowerVortex) return "Vortex Pillar";
            if (Player.ZoneTowerNebula) return "Nebula Pillar";
            if (Player.ZoneTowerStardust) return "Stardust Pillar";
            if (Player.ZoneUndergroundDesert) return "Underground Desert";

            return "Other";
        }
    }
}
