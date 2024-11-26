using System;
using System.Diagnostics;

namespace TerraLens.Project.Profiling.ResourceCollectors
{
    public static class CPUCollector
    {
        private static TimeSpan lastTotalProcessorTime = TimeSpan.Zero;
        private static DateTime lastCheckTime = DateTime.UtcNow;
        public static double CurrentCPUUsage { get; private set; } = 0.0;

        public static void UpdateCPUUsage()
        {
            var currentProcess = Process.GetCurrentProcess();
            TimeSpan currentTotalProcessorTime = currentProcess.TotalProcessorTime;
            DateTime currentTime = DateTime.UtcNow;

            double cpuUsedMs = (currentTotalProcessorTime - lastTotalProcessorTime).TotalMilliseconds;
            double totalMsPassed = (currentTime - lastCheckTime).TotalMilliseconds;

            if (totalMsPassed > 0)
            {
                CurrentCPUUsage = (cpuUsedMs / (Environment.ProcessorCount * totalMsPassed)) * 100.0;
            }

            lastTotalProcessorTime = currentTotalProcessorTime;
            lastCheckTime = currentTime;
        }
    }
}
