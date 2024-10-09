using Terraria;
using Terraria.ModLoader;
using TerraLens.Project.DataCollection;

public class TerraLensSystem : ModSystem
{
    private double lastUpdate = 0;

    public override void PostUpdateEverything()
    {
        FPSCollector.UpdateFPS();
        CPUCollector.UpdateCPUUsage();

        // Check if 1 second has passed since the last update
        double currentTime = Main.GameUpdateCount / 60.0; // Convert frames to seconds (assuming 60 FPS)
        if (currentTime - lastUpdate >= 1)
        {
            lastUpdate = currentTime;
            Main.NewText($"FPS: {FPSCollector.GetFPS()}", 255, 255, 0); // Display FPS in chat with yellow text
            Main.NewText($"CPU Usage: {CPUCollector.GetCPUUsage()}%", 255, 0, 255); // Display CPU usage in chat with purple text
        }
    }
}
