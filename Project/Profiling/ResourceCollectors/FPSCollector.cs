using Terraria;

namespace TerraLens.Project.Profiling.ResourceCollectors
{
    public static class FPSCollector
    {
        public static double CurrentFPS { get; private set; } = 0.0;

        public static void UpdateFPS()
        {
            CurrentFPS = Main.frameRate;
        }
    }
}
