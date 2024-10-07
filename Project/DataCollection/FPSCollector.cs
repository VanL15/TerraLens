using System;
using Terraria.ModLoader;

namespace TerraLens.Project.DataCollection
{
    internal class FPSCollector
    {
        private static DateTime lastFrameTime; // Time of the last frame update
        private static float currentFPS; // Stores the current FPS value

        // Constructor to initialize the first frame time
        static FPSCollector()
        {
            lastFrameTime = DateTime.Now;
        }

        // Method to be called every frame to update the FPS
        public static void UpdateFPS()
        {
            // Get the current time
            DateTime currentFrameTime = DateTime.Now;

            // Calculate the time difference between this frame and the last frame
            TimeSpan timeDifference = currentFrameTime - lastFrameTime;

            // Calculate FPS as 1 / seconds per frame
            currentFPS = (float)(1.0 / timeDifference.TotalSeconds);

            // Update lastFrameTime to the current frame time
            lastFrameTime = currentFrameTime;
        }

        // Method to retrieve the current FPS value
        public static float GetFPS()
        {
            return currentFPS;
        }
    }
}
