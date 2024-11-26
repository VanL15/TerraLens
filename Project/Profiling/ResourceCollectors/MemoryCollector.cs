using System;
using System.Diagnostics;

namespace TerraLens.Project.Profiling.ResourceCollectors
{
    public static class MemoryCollector
    {
        public static double LatestPhysicalMemoryMB { get; private set; } = 0.0;
        public static double LatestManagedMemoryMB { get; private set; } = 0.0;

        public static void UpdateMemoryUsage()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                long physicalMemoryBytes = currentProcess.WorkingSet64;
                long managedMemoryBytes = GC.GetTotalMemory(false);

                LatestPhysicalMemoryMB = physicalMemoryBytes / (1024.0 * 1024.0);
                LatestManagedMemoryMB = managedMemoryBytes / (1024.0 * 1024.0);
            }
            catch (Exception ex)
            {
                LatestPhysicalMemoryMB = 0.0;
                LatestManagedMemoryMB = 0.0;
                Console.WriteLine($"MemoryCollector Error: {ex.Message}");
                //Mod.Logger.Error($"MemoryCollector Error: {ex.Message}");
            }
        }
    }
}
