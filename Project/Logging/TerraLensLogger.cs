using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TerraLens.Project.Config;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerraLens.Project.Logging
{
    public class TerraLensLogger : ModSystem
    {
        private static TerraLensLogger _instance;
        private StreamWriter _writer;
        private bool _isLoggingEnabled;
        private string _logFilePath;
        private object _lock = new object();

        public override void Load()
        {
            _instance = this;
            InitializeLogger();
        }

        public override void Unload()
        {
            CloseLogger();
            _instance = null;
        }

        private void InitializeLogger()
        {
            var config = TerraLensConfig.Instance;
            _isLoggingEnabled = config.EnableDataLogging;

            if (_isLoggingEnabled)
            {
                // Define the log file path, "ModData/terra_lens_logs.csv"
                string modFolder = Path.Combine(Main.SavePath, "Mods", "TerraLens");
                if (!Directory.Exists(modFolder))
                {
                    Directory.CreateDirectory(modFolder);
                }

                _logFilePath = Path.Combine(modFolder, $"terra_lens_logs_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

                try
                {
                    // Initialize the StreamWriter with append mode
                    _writer = new StreamWriter(_logFilePath, append: true, encoding: Encoding.UTF8);
                    // Write CSV headers
                    _writer.WriteLine("Timestamp,Category,EntityName,ActiveCount,TotalSpawned");
                    _writer.Flush();
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Main.NewText($"[TerraLens] Failed to initialize Logger: {ex.Message}", Color.Red);
                }
            }
        }

        private void CloseLogger()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer = null;
            }
        }

        public static void Log(string category, string entityName, int activeCount, int totalSpawned)
        {
            if (_instance == null || !_instance._isLoggingEnabled)
                return;

            string timestamp = DateTime.Now.ToString("o"); // ISO 8601 format

            string logEntry = $"{timestamp},{EscapeCSV(category)},{EscapeCSV(entityName)},{activeCount},{totalSpawned}";

            // Fire and forget the write operation
            Task.Run(() =>
            {
                lock (_instance._lock)
                {
                    try
                    {
                        _instance._writer.WriteLine(logEntry);
                        _instance._writer.Flush();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Main.NewText($"[TerraLens] Failed to write log entry: {ex.Message}", Color.Red);
                    }
                }
            });
        }

        private static string EscapeCSV(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
    }
}
