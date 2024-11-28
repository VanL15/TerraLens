using System;
using System.Diagnostics;

namespace TerraLens.Project.Profiling.ResourceCollectors
{
    public static class MemoryCollector
    {
        public static long PhysicalMemoryUsage { get; private set; } = 0;
        public static long ManagedMemoryUsage { get; private set; } = 0;

        public static void UpdateMemoryUsage()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long physicalMemoryBytes = currentProcess.WorkingSet64;
            long managedMemoryBytes = GC.GetTotalMemory(false);

            PhysicalMemoryUsage = physicalMemoryBytes / (1024 * 1024);
            ManagedMemoryUsage = managedMemoryBytes / (1024 * 1024);
        }
    }
}
