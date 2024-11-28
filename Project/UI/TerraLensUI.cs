using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using TerraLens.Project.Profiling.ResourceCollectors;
using TerraLens.Project.Config;
using Terraria;
using Humanizer;

namespace TerraLens.Project.UI
{
    public class TerraLensUI : UIState
    {
        private ExtendedUIPanel mainPanel;
        private UITabbedPanel tabbedPanel;
        private UIPanel titlePanel;
        private UIText titleText;
        private UIText closeButton;

        // References to dynamic text elements
        private UIText fpsText;
        private UIText cpuText;
        private UIText physicalMemoryText;
        private UIText managedMemoryText;

        // Draggable variables
        private bool isDragging = false;
        private Vector2 offset;

        // Timer variables for updating metrics
        private float updateTimer = 0f;
        private const float updateInterval = 1f; // 1 second

        public override void OnInitialize()
        {
            // Main panel setup
            mainPanel = new ExtendedUIPanel();
            mainPanel.SetPadding(10);
            mainPanel.Width.Set(600f, 0f);
            mainPanel.Height.Set(400f, 0f);
            mainPanel.Left.Set(TerraLensConfig.Instance.UIPanelPosX, 0f); // Initialize position from config
            mainPanel.Top.Set(TerraLensConfig.Instance.UIPanelPosY, 0f); // Initialize position from config
            mainPanel.HAlign = 0.5f;
            mainPanel.VAlign = 0.5f;
            mainPanel.BackgroundColor = new Color(43, 43, 43) * 0.8f; // Semi-transparent background
            mainPanel.BorderColor = Color.Black;
            Append(mainPanel);

            // Title panel setup
            titlePanel = new UIPanel();
            titlePanel.Width.Set(-10f, 1f);
            titlePanel.Height.Set(40f, 0f); 
            titlePanel.Top.Set(-20f, 0f); // Position partially above main panel
            titlePanel.HAlign = 0.5f;
            titlePanel.BackgroundColor = new Color(73, 94, 171);
            titlePanel.BorderColor = Color.Black;
            mainPanel.Append(titlePanel);

            // Title text
            titleText = new UIText("TerraLens Profiler", 0.7f, true);
            titleText.HAlign = 0.5f;
            titleText.VAlign = 0.5f;
            titlePanel.Append(titleText);

            // Assign correct mouse events
            titlePanel.OnLeftMouseDown += Title_OnLeftMouseDown;
            titlePanel.OnLeftMouseUp += Title_OnLeftMouseUp;

            // Link the titlePanel to mainPanel for extended ContainsPoint
            mainPanel.TitlePanel = titlePanel;

            // Close Button
            closeButton = new UIText("X");
            closeButton.Width.Set(20f, 0f);
            closeButton.Height.Set(20f, 0f);
            closeButton.HAlign = 1f;          // Align to the right
            closeButton.VAlign = 0.5f;        // Center vertically
            closeButton.OnLeftClick += CloseButtonClicked;
            titlePanel.Append(closeButton);



            // Tabbed panel setup
            tabbedPanel = new UITabbedPanel();
            tabbedPanel.Width.Set(0f, 1f);
            tabbedPanel.Height.Set(0f, 1f);
            tabbedPanel.Top.Set(40f, 0f); // Positioned below the title panel
            mainPanel.Append(tabbedPanel);

            // Adding tabs
            tabbedPanel.AddTab("System Metrics", CreateSystemMetricsPanel);
            tabbedPanel.AddTab("Mod Profiling", CreateModProfilingPanel);
        }

        private void Title_OnLeftMouseDown(UIMouseEvent evt, UIElement listeningElement)
        {
            // Get panel's current position
            Vector2 mainPanelPos = new Vector2(mainPanel.Left.Pixels, mainPanel.Top.Pixels);

            // Calculate offset between mouse and panel's top-left
            offset = Main.MouseScreen - mainPanelPos;
            isDragging = true;
        }

        private void Title_OnLeftMouseUp(UIMouseEvent evt, UIElement listeningElement)
        {
            isDragging = false;

            // Save the new position to config for persistence
            TerraLensConfig.Instance.UIPanelPosX = (int)mainPanel.Left.Pixels;
            TerraLensConfig.Instance.UIPanelPosY = (int)mainPanel.Top.Pixels;
            TerraLensConfig.Instance.Save();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isDragging)
            {
                // Get current mouse position in UI coordinates
                Vector2 currentMouse = Main.MouseScreen; // Already in UI coordinates

                // Calculate new panel position
                Vector2 newPos = currentMouse - offset;

                // Update the panel's position
                mainPanel.Left.Set(newPos.X, 0f);
                mainPanel.Top.Set(newPos.Y, 0f);
            }

            if (TerraLensConfig.Instance != null && TerraLensConfig.Instance.ShowOverlay)
            {
                // Increment the timer by elapsed game time in seconds
                updateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (updateTimer >= updateInterval)
                {
                    // Reset the timer
                    updateTimer = 0f;

                    // Update metrics
                    if (TerraLensConfig.Instance.CollectFPS)
                        FPSCollector.UpdateFPS();

                    if (TerraLensConfig.Instance.CollectCPU)
                        CPUCollector.UpdateCPUUsage();

                    if (TerraLensConfig.Instance.CollectMemory)
                        MemoryCollector.UpdateMemoryUsage();

                    // Update dynamic text elements
                    if (fpsText != null && TerraLensConfig.Instance.CollectFPS)
                        fpsText.SetText($"FPS: {FPSCollector.CurrentFPS:F2}");

                    if (cpuText != null && TerraLensConfig.Instance.CollectCPU)
                        cpuText.SetText($"CPU Usage: {CPUCollector.CurrentCPUUsage:F2}%");

                    if (physicalMemoryText != null && TerraLensConfig.Instance.CollectMemory)
                        physicalMemoryText.SetText($"Physical Memory: {MemoryCollector.PhysicalMemoryUsage} MB");

                    if (managedMemoryText != null && TerraLensConfig.Instance.CollectMemory)
                        managedMemoryText.SetText($"Managed Memory: {MemoryCollector.ManagedMemoryUsage} MB");
                }
            }
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            // Disable the overlay by updating the configuration
            TerraLensConfig.Instance.ShowOverlay = false;
        }

        private UIElement CreateSystemMetricsPanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f; // Start some padding from the top

            // Add a title for the tab
            var tabTitle = new UIText("System Metrics", 0.8f, true); // Title text with larger font
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 50f; // Space below the title

            // Display FPS
            if (TerraLensConfig.Instance != null && TerraLensConfig.Instance.CollectFPS)
            {
                fpsText = new UIText("FPS: 0", 0.4f, true); 
                fpsText.Top.Set(top, 0f);
                fpsText.Left.Set(10f, 0f);
                panel.Append(fpsText);
                top += 30f;
            }

            // Display CPU Usage
            if (TerraLensConfig.Instance != null && TerraLensConfig.Instance.CollectCPU)
            {
                cpuText = new UIText("CPU Usage: 0%",0.4f, true); 
                cpuText.Top.Set(top, 0f);
                cpuText.Left.Set(10f, 0f);
                panel.Append(cpuText);
                top += 30f;
            }

            // Display Memory Usage
            if (TerraLensConfig.Instance != null && TerraLensConfig.Instance.CollectMemory)
            {
                physicalMemoryText = new UIText("Physical Memory: 0 MB", 0.4f, true);
                physicalMemoryText.Top.Set(top, 0f);
                physicalMemoryText.Left.Set(10f, 0f);
                panel.Append(physicalMemoryText);
                top += 30f;
            
                managedMemoryText = new UIText("Managed Memory: 0 MB", 0.4f, true);
                managedMemoryText.Top.Set(top, 0f);
                managedMemoryText.Left.Set(10f, 0f);
                panel.Append(managedMemoryText);
                top += 30f;
            }

            return panel;
        }

        private UIElement CreateModProfilingPanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f; // Start some padding from the top

            // Add a title for the tab
            var tabTitle = new UIText("Mod Profiling", 0.8f, true); // Title text with larger font
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 50f; // Space below the title

            var placeholder = new UIText("Mod profiling data will be displayed here.", 0.4f, true);
            placeholder.Top.Set(top, 0f);
            placeholder.Left.Set(10f, 0f);
            panel.Append(placeholder);
            top += 30f;


            return panel;
        }
    }
}
