using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerraLens.Project.Config
{
    public class TerraLensConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.ShowOverlay.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.ShowOverlay.Tooltip")]
        [DefaultValue(true)]
        public bool ShowOverlay { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectFPS.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectFPS.Tooltip")]
        [DefaultValue(true)]
        public bool CollectFPS { get; set; }  

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectCPU.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectCPU.Tooltip")]
        [DefaultValue(true)]
        public bool CollectCPU { get; set; } 

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectMemory.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectMemory.Tooltip")]
        [DefaultValue(true)]
        public bool CollectMemory { get; set; } 


        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModEntities.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModEntities.Tooltip")]
        [DefaultValue(true)]
        public bool CollectModEntities { get; set; } 

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModContent.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModContent.Tooltip")]
        [DefaultValue(true)]
        public bool CollectModContent { get; set; } 

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableLagSpikeDetection.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableLagSpikeDetection.Tooltip")]
        [DefaultValue(true)]
        public bool EnableLagSpikeDetection { get; set; } 

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LagSpikeThreshold.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LagSpikeThreshold.Tooltip")]
        [Range(16, 500)]
        [DefaultValue(50)]
        public int LagSpikeThreshold { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableDataLogging.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableDataLogging.Tooltip")]
        [DefaultValue(true)]
        public bool EnableDataLogging { get; set; } 

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LoggingInterval.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LoggingInterval.Tooltip")]
        [Range(1, 300)]
        [DefaultValue(60)]
        public int LoggingInterval { get; set; }
    }
}
