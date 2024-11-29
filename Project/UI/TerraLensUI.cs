using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using TerraLens.Project.Profiling.ResourceCollectors;
using TerraLens.Project.Config;
using Terraria;
using TerraLens.Project.Profiling.ModProfilers;
using System.Collections.Generic;
using System.Linq;
using TerraLens.Project.Profiling;

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

        // Mod profiling elements
        private UIList npcList;
        private UIScrollbar npcScrollbar;
        //private FilterButton npcFilterButton;
        //private DropdownMenu npcDropdownMenu;
        //private bool isNpcDropdownOpen = false;

        private UIList projectileList;
        private UIScrollbar projectileScrollbar;

        private UIList itemUseList;
        private UIScrollbar itemUseScrollbar;

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
            tabbedPanel.AddTab("NPCs", CreateNPCsPanel);
            tabbedPanel.AddTab("Projectiles", CreateProjectilesPanel);
            tabbedPanel.AddTab("Items", CreateItemsPanel);
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

                    // Update NPC List
                    if (npcList != null && TerraLensConfig.Instance.CollectModEntities)
                    {
                        ModEntityProfiler.UpdateNPCs();
                        UpdateNPCList();
                    }

                    // Update Projectile List
                    if (projectileList != null && TerraLensConfig.Instance.CollectModEntities)
                    {
                        ModEntityProfiler.UpdateProjectiles();
                        UpdateProjectileList();
                    }

                    // Update Item Use List
                    if (itemUseList != null && TerraLensConfig.Instance.CollectModContent)
                    {
                        UpdateItemUseList();
                    }
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
                cpuText = new UIText("CPU Usage: 0%", 0.4f, true);
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

        private UIElement CreateNPCsPanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("NPCs", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);

            //// Create and position the Filter Button
            //npcFilterButton = new FilterButton(() =>
            //{
            //    isNpcDropdownOpen = !isNpcDropdownOpen;
            //    UpdateNPCList(); // Update the list to reflect changes
            //});
            //npcFilterButton.Top.Set(top, 0f); // Align with the title
            //npcFilterButton.HAlign = 1f;      // Align to the right edge
            //npcFilterButton.Left.Set(-90f, 0f); // Adjust based on Dropdown width + padding
            //panel.Append(npcFilterButton);

            //// Calculate the position for the dropdown menu
            //float dropdownX = npcFilterButton.GetOuterDimensions().X;
            //float dropdownY = npcFilterButton.GetOuterDimensions().Y + npcFilterButton.GetOuterDimensions().Height;

            //// Initialize Dropdown Menu
            //npcDropdownMenu = new DropdownMenu(GetAllMods(ModEntityProfiler.NPCCounts), new Vector2(dropdownX, dropdownY));
            //npcDropdownMenu.Width.Set(200f, 0f);
            //npcDropdownMenu.Height.Set(200f, 0f);
            //npcDropdownMenu.HAlign = 1f; // Align to the right
            //npcDropdownMenu.Left.Set(dropdownX, 0f); // Shift left by its width to align right edges
            //npcDropdownMenu.Top.Set(dropdownY, 0f); // Below the FilterButton

            // Initialize NPC List
            top += 40f;
            npcList = new UIList();
            npcList.Width.Set(-30f, 1f); // Leave space for scrollbar
            npcList.Height.Set(-top - 50f, 1f); // Fill remaining space
            npcList.Top.Set(top, 0f);
            npcList.Left.Set(10f, 0f);
            panel.Append(npcList);

            // Scrollbar
            npcScrollbar = new UIScrollbar();
            npcScrollbar.SetView(100f, 1000f);
            npcScrollbar.Height.Set(-top - 50f, 1f);
            npcScrollbar.Top.Set(top, 0f);
            npcScrollbar.Left.Set(-20f, 1f);
            npcList.SetScrollbar(npcScrollbar);
            panel.Append(npcScrollbar);

            return panel;
        }


        private UIElement CreateProjectilesPanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("Projectiles", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Initialize Projectile List
            projectileList = new UIList();
            projectileList.Width.Set(-30f, 1f); // Leave space for scrollbar
            projectileList.Height.Set(-top - 10f, 1f); // Fill remaining space
            projectileList.Top.Set(top, 0f);
            projectileList.Left.Set(10f, 0f);
            panel.Append(projectileList);

            // Scrollbar
            projectileScrollbar = new UIScrollbar();
            projectileScrollbar.SetView(100f, 1000f);
            projectileScrollbar.Height.Set(-top - 10f, 1f);
            projectileScrollbar.Top.Set(top, 0f);
            projectileScrollbar.Left.Set(-20f, 1f);
            projectileList.SetScrollbar(projectileScrollbar);
            panel.Append(projectileScrollbar);

            return panel;
        }

        private UIElement CreateItemsPanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("Items", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Initialize Item Use List
            itemUseList = new UIList();
            itemUseList.Width.Set(-30f, 1f); // Leave space for scrollbar
            itemUseList.Height.Set(-top - 10f, 1f); // Fill remaining space
            itemUseList.Top.Set(top, 0f);
            itemUseList.Left.Set(10f, 0f);
            panel.Append(itemUseList);

            // Scrollbar
            itemUseScrollbar = new UIScrollbar();
            itemUseScrollbar.SetView(100f, 1000f);
            itemUseScrollbar.Height.Set(-top - 10f, 1f);
            itemUseScrollbar.Top.Set(top, 0f);
            itemUseScrollbar.Left.Set(-20f, 1f);
            itemUseList.SetScrollbar(itemUseScrollbar);
            panel.Append(itemUseScrollbar);

            return panel;
        }

        //private List<string> GetAllMods(Dictionary<string, ModEntityProfiler.EntityInfo> entityCounts)
        //{
        //    return entityCounts.Keys.Select(k => k.Split(':')[0]).Distinct().OrderBy(m => m).ToList();
        //}

        private void UpdateNPCList()
        {
            npcList.Clear();

            // Get selected mods from the dropdown
            //List<string> selectedMods = isNpcDropdownOpen ? npcDropdownMenu.GetSelectedMods() : new List<string> { "All" };

            //// Apply filter
            //var filteredNPCs = ModEntityProfiler.NPCCounts
            //    .Where(entry =>
            //    {
            //        if (selectedMods.Contains("All") || selectedMods.Count == 0)
            //            return true;
            //        return selectedMods.Any(mod => entry.Key.StartsWith($"{mod}:"));
            //    })
            //    .OrderByDescending(entry => entry.Value.CurrentCount)
            //    .ThenBy(entry => entry.Key);

            //foreach (var entry in filteredNPCs)
            //{
            //    string npcInfo = $"{entry.Key} - Active: {entry.Value.CurrentCount}, Total Spawned: {entry.Value.TotalSpawned}";
            //    var text = new UIText(npcInfo, 0.4f, true);
            //    npcList.Add(text);
            //}

            //// Toggle Dropdown Menu
            //if (isNpcDropdownOpen)
            //{
            //    if (!mainPanel.Children.Contains(npcDropdownMenu))
            //    {
            //        mainPanel.Append(npcDropdownMenu);
            //    }
            //}
            //else
            //{
            //    if (mainPanel.Children.Contains(npcDropdownMenu))
            //    {
            //        mainPanel.RemoveChild(npcDropdownMenu);
            //    }
            //}

            // Display all NPCs without filtering (which is currently disabled)
            var allNPCs = ModEntityProfiler.NPCCounts
                .OrderByDescending(entry => entry.Value.CurrentCount)
                .ThenBy(entry => entry.Key);

            foreach (var entry in allNPCs)
            {
                string npcInfo = $"{entry.Key} - Active: {entry.Value.CurrentCount}, Total Spawned: {entry.Value.TotalSpawned}";
                var text = new UIText(npcInfo, 0.4f, true);
                npcList.Add(text);
            }
        }

        private void UpdateProjectileList()
        {
            projectileList.Clear();

            var allProjectiles = ModEntityProfiler.ProjectileCounts
                .OrderByDescending(entry => entry.Value.CurrentCount)
                .ThenBy(entry => entry.Key);

            foreach (var entry in allProjectiles)
            {
                string projectileInfo = $"{entry.Key} - Active: {entry.Value.CurrentCount}, Total Spawned: {entry.Value.TotalSpawned}";
                var text = new UIText(projectileInfo, 0.4f, true);
                projectileList.Add(text);
            }
        }

        private void UpdateItemUseList()
        {
            itemUseList.Clear();

            var allItemUses = PlayerInteractionProfiler.ItemUseCounts
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in allItemUses)
            {
                string itemInfo = $"{entry.Key} - Uses: {entry.Value}";
                var text = new UIText(itemInfo, 0.4f, true);
                itemUseList.Add(text);
            }
        }
    }
}
