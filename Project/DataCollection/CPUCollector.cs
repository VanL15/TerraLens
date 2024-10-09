using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria.ModLoader;
namespace TerraLens.Project.DataCollection
{
    internal class CPUCollector
    {
        private static DateTime lastUpdate;
        private static TimeSpan lastTotalProcessorTime;
        private static float latestCPUUsage;
        private static float averageCPUUsage;
        private static readonly int numCores = Environment.ProcessorCount;
        private static readonly Queue<float> cpuUsageQueue = new Queue<float>(); // queue to store most recent CPU usage values
        private static readonly int queueSize = 60; // number of values to keep in the queue (60 seconds)
        private static float runningSumCPUUsage = 0;

        static CPUCollector()
        {
            lastUpdate = DateTime.Now;
            lastTotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
        }
        public static void UpdateCPUUsage()
        {
            // Get the current time and total processor time
            DateTime currentTime = DateTime.Now;
            TimeSpan currentTotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;

            // Calculate time difference and CPU usage
            double elapsedMilliseconds = (currentTime - lastUpdate).TotalMilliseconds;
            double cpuUsageMilliseconds = (currentTotalProcessorTime - lastTotalProcessorTime).TotalMilliseconds;

            // Calculate CPU usage as a percentage of total CPU time divided by core count
            latestCPUUsage = (float)(cpuUsageMilliseconds / elapsedMilliseconds / numCores * 100);

            // Update last recorded time and processor time
            lastUpdate = currentTime;
            lastTotalProcessorTime = currentTotalProcessorTime;

            // Add the current CPU usage to the queue and update running sum
            cpuUsageQueue.Enqueue(latestCPUUsage);
            runningSumCPUUsage += latestCPUUsage;
            if (cpuUsageQueue.Count > queueSize)
            {
                runningSumCPUUsage -= cpuUsageQueue.Dequeue();
            }

            // Calculate the average CPU usage over the last queueSize seconds using running sum
            averageCPUUsage = runningSumCPUUsage / cpuUsageQueue.Count;
        }
        public static float GetCPUUsage()
        {
            return averageCPUUsage;
        }
    }
}