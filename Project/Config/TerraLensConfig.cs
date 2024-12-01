using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerraLens.Project.Config
{
    [Label("$Mods.TerraLens.Configs.TerraLensConfig.DisplayName")]
    public class TerraLensConfig : ModConfig
    {
        public static TerraLensConfig Instance;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            Instance = this;
        }

        // UI Settings
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.UISettings")]
        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.UIPanelPosX.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.UIPanelPosX.Tooltip")]
        [Range(0, 1920)]
        [DefaultValue(300)]
        public int UIPanelPosX { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.UIPanelPosY.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.UIPanelPosY.Tooltip")]
        [Range(0, 1080)]
        [DefaultValue(200)]
        public int UIPanelPosY { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.ShowOverlay.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.ShowOverlay.Tooltip")]
        [DefaultValue(true)]
        public bool ShowOverlay { get; set; }

        // Performance Metrics
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.PerformanceMetrics")]
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

        // Mod Metrics
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.ModMetrics")]
        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModEntities.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModEntities.Tooltip")]
        [DefaultValue(true)]
        public bool CollectModEntities { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModContent.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectModContent.Tooltip")]
        [DefaultValue(true)]
        public bool CollectModContent { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectPlayerDamageMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectPlayerDamageMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectPlayerDamageMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectNPCDamageMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectNPCDamageMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectNPCDamageMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectPvPDamageMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectPvPDamageMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectPvPDamageMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectBiomeTimeMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectBiomeTimeMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectBiomeTimeMetrics { get; set; }

        // Additional Metrics
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.AdditionalMetrics")]
        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectNPCMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectNPCMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectNPCMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectProjectileMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectProjectileMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectProjectileMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectItemUseMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectItemUseMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectItemUseMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectTileMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.CollectTileMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool CollectTileMetrics { get; set; }

        // Lag Spike Detection
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.LagSpikeDetection")]
        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableLagSpikeDetection.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableLagSpikeDetection.Tooltip")]
        [DefaultValue(true)]
        public bool EnableLagSpikeDetection { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LagSpikeThreshold.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LagSpikeThreshold.Tooltip")]
        [Range(16, 500)]
        [DefaultValue(50)]
        public int LagSpikeThreshold { get; set; }

        // Data Logging
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.DataLogging")]
        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableDataLogging.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.EnableDataLogging.Tooltip")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool EnableDataLogging { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LoggingInterval.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LoggingInterval.Tooltip")]
        [Range(1, 300)]
        [DefaultValue(60)]
        [ReloadRequired]
        public int LoggingInterval { get; set; }

        // Logging Categories
        [Header("$Mods.TerraLens.Configs.TerraLensConfig.Headers.LoggingCategories")]
        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogNPCMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogNPCMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogNPCMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogProjectileMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogProjectileMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogProjectileMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogItemUseMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogItemUseMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogItemUseMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogTileMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogTileMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogTileMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogPlayerDamageMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogPlayerDamageMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogPlayerDamageMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogNPCDamageMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogNPCDamageMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogNPCDamageMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogPvPDamageMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogPvPDamageMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogPvPDamageMetrics { get; set; }

        [LabelKey("$Mods.TerraLens.Configs.TerraLensConfig.LogBiomeTimeMetrics.Label")]
        [TooltipKey("$Mods.TerraLens.Configs.TerraLensConfig.LogBiomeTimeMetrics.Tooltip")]
        [DefaultValue(true)]
        public bool LogBiomeTimeMetrics { get; set; }
    }
}
