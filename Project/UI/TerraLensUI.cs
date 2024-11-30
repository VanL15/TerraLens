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
using Terraria.Localization;

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

        private UIList tilePlacementList;
        private UIList tileMiningList;
        private UIScrollbar tilePlacementScrollbar;

        private UIList playerDamageList;
        private UIScrollbar playerDamageScrollbar;

        private UIList npcDamageList;
        private UIScrollbar npcDamageScrollbar;

        private UIList pvpDamageList;
        private UIScrollbar pvpDamageScrollbar;

        private UIList biomeTimeList;
        private UIScrollbar biomeTimeScrollbar;

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
            tabbedPanel.AddTab("Tiles", CreateTilesPanel);
            tabbedPanel.AddTab("Player Damage", CreatePlayerDamagePanel);
            tabbedPanel.AddTab("NPC Damage", CreateNPCDamagePanel);
            tabbedPanel.AddTab("PvP Damage", CreatePvPDamagePanel);
            tabbedPanel.AddTab("Biome Time", CreateBiomeTimePanel);
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
                    if (npcList != null && TerraLensConfig.Instance.CollectNPCMetrics)
                    {
                        ModEntityProfiler.UpdateNPCs();
                        UpdateNPCList();
                    }

                    // Update Projectile List
                    if (projectileList != null && TerraLensConfig.Instance.CollectProjectileMetrics)
                    {
                        ModEntityProfiler.UpdateProjectiles();
                        UpdateProjectileList();
                    }

                    // Update Item Use List
                    if (itemUseList != null && TerraLensConfig.Instance.CollectItemUseMetrics)
                    {
                        UpdateItemUseList();
                    }

                    // Update Tile Interactions
                    if (tilePlacementList != null && tileMiningList != null && TerraLensConfig.Instance.CollectTileMetrics)
                    {
                        UpdateTilesPanel();
                    }

                    // Update Player Damage List
                    if (playerDamageList != null && TerraLensConfig.Instance.CollectPlayerDamageMetrics)
                    {
                        //PlayerDamageProfiler.UpdatePlayerDamages(); // Ensure this method updates the data
                        UpdatePlayerDamageList();
                    }

                    // Update NPC Damage List
                    if (npcDamageList != null && TerraLensConfig.Instance.CollectNPCDamageMetrics)
                    {
                        //NPCDamageProfiler.UpdateNPCDamages(); // Ensure this method updates the data
                        UpdateNPCDamageList();
                    }

                    // Update PvP Damage List
                    if (pvpDamageList != null && TerraLensConfig.Instance.CollectPvPDamageMetrics)
                    {
                        //PvPDamageProfiler.UpdatePvPDamages(); // Ensure this method updates the data
                        UpdatePvPDamageList();
                    }

                    // Update Biome Time List
                    if (biomeTimeList != null && TerraLensConfig.Instance.CollectBiomeTimeMetrics)
                    {
                        //BiomeTimeProfiler.UpdateBiomeTimes(); // Ensure this method updates the data
                        UpdateBiomeTimeList();
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

        private UIElement CreateTilesPanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("Tile Interactions", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Tile Placements Section
            var placementsTitle = new UIText("Tiles Placed", 0.6f, true);
            placementsTitle.Top.Set(top, 0f);
            placementsTitle.Left.Set(10f, 0f);
            panel.Append(placementsTitle);
            top += 25f;

            // Initialize Tile Placement List
            tilePlacementList = new UIList();
            tilePlacementList.Width.Set(-30f, 1f); // Leave space for scrollbar
            tilePlacementList.Height.Set(150f, 0f); // Adjust height as needed
            tilePlacementList.Top.Set(top, 0f);
            tilePlacementList.Left.Set(10f, 0f);
            panel.Append(tilePlacementList);

            // Scrollbar for Tile Placements
            tilePlacementScrollbar = new UIScrollbar();
            tilePlacementScrollbar.SetView(100f, 1000f);
            tilePlacementScrollbar.Height.Set(150f, 0f);
            tilePlacementScrollbar.Top.Set(top, 0f);
            tilePlacementScrollbar.Left.Set(-20f, 1f);
            tilePlacementList.SetScrollbar(tilePlacementScrollbar);
            panel.Append(tilePlacementScrollbar);
            top += 160f; // Space between placements and mining sections

            // Tile Mining Section
            var miningTitle = new UIText("Tiles Mined", 0.6f, true);
            miningTitle.Top.Set(top, 0f);
            miningTitle.Left.Set(10f, 0f);
            panel.Append(miningTitle);
            top += 25f;

            // Initialize Tile Mining List
            tileMiningList = new UIList();
            tileMiningList.Width.Set(-30f, 1f); // Leave space for scrollbar
            tileMiningList.Height.Set(150f, 0f); // Adjust height as needed
            tileMiningList.Top.Set(top, 0f);
            tileMiningList.Left.Set(10f, 0f);
            panel.Append(tileMiningList);

            // Scrollbar for Tile Mining
            var tileMiningScrollbar = new UIScrollbar();
            tileMiningScrollbar.SetView(100f, 1000f);
            tileMiningScrollbar.Height.Set(150f, 0f);
            tileMiningScrollbar.Top.Set(top, 0f);
            tileMiningScrollbar.Left.Set(-20f, 1f);
            tileMiningList.SetScrollbar(tileMiningScrollbar);
            panel.Append(tileMiningScrollbar);

            return panel;
        }

        private UIElement CreatePlayerDamagePanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("Player Damage", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Initialize Player Damage List
            playerDamageList = new UIList();
            playerDamageList.Width.Set(-30f, 1f); // Leave space for scrollbar
            playerDamageList.Height.Set(-top - 50f, 1f); // Fill remaining space
            playerDamageList.Top.Set(top, 0f);
            playerDamageList.Left.Set(10f, 0f);
            panel.Append(playerDamageList);

            // Scrollbar
            playerDamageScrollbar = new UIScrollbar();
            playerDamageScrollbar.SetView(100f, 1000f);
            playerDamageScrollbar.Height.Set(-top - 50f, 1f);
            playerDamageScrollbar.Top.Set(top, 0f);
            playerDamageScrollbar.Left.Set(-20f, 1f);
            playerDamageList.SetScrollbar(playerDamageScrollbar);
            panel.Append(playerDamageScrollbar);

            return panel;
        }

        private UIElement CreateNPCDamagePanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("NPC Damage", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Initialize NPC Damage List
            npcDamageList = new UIList();
            npcDamageList.Width.Set(-30f, 1f); // Leave space for scrollbar
            npcDamageList.Height.Set(-top - 50f, 1f); // Fill remaining space
            npcDamageList.Top.Set(top, 0f);
            npcDamageList.Left.Set(10f, 0f);
            panel.Append(npcDamageList);

            // Scrollbar
            npcDamageScrollbar = new UIScrollbar();
            npcDamageScrollbar.SetView(100f, 1000f);
            npcDamageScrollbar.Height.Set(-top - 50f, 1f);
            npcDamageScrollbar.Top.Set(top, 0f);
            npcDamageScrollbar.Left.Set(-20f, 1f);
            npcDamageList.SetScrollbar(npcDamageScrollbar);
            panel.Append(npcDamageScrollbar);

            return panel;
        }

        private UIElement CreatePvPDamagePanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("PvP Damage", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Initialize PvP Damage List
            pvpDamageList = new UIList();
            pvpDamageList.Width.Set(-30f, 1f); // Leave space for scrollbar
            pvpDamageList.Height.Set(-top - 50f, 1f); // Fill remaining space
            pvpDamageList.Top.Set(top, 0f);
            pvpDamageList.Left.Set(10f, 0f);
            panel.Append(pvpDamageList);

            // Scrollbar
            pvpDamageScrollbar = new UIScrollbar();
            pvpDamageScrollbar.SetView(100f, 1000f);
            pvpDamageScrollbar.Height.Set(-top - 50f, 1f);
            pvpDamageScrollbar.Top.Set(top, 0f);
            pvpDamageScrollbar.Left.Set(-20f, 1f);
            pvpDamageList.SetScrollbar(pvpDamageScrollbar);
            panel.Append(pvpDamageScrollbar);

            return panel;
        }

        private UIElement CreateBiomeTimePanel()
        {
            var panel = new UIElement();
            panel.Width.Set(0f, 1f);
            panel.Height.Set(0f, 1f);

            float top = 10f;

            // Add a title for the tab
            var tabTitle = new UIText("Biome Time", 0.8f, true);
            tabTitle.Top.Set(top, 0f);
            tabTitle.Left.Set(10f, 0f);
            panel.Append(tabTitle);
            top += 40f;

            // Initialize Biome Time List
            biomeTimeList = new UIList();
            biomeTimeList.Width.Set(-30f, 1f); // Leave space for scrollbar
            biomeTimeList.Height.Set(-top - 50f, 1f); // Fill remaining space
            biomeTimeList.Top.Set(top, 0f);
            biomeTimeList.Left.Set(10f, 0f);
            panel.Append(biomeTimeList);

            // Scrollbar
            biomeTimeScrollbar = new UIScrollbar();
            biomeTimeScrollbar.SetView(100f, 1000f);
            biomeTimeScrollbar.Height.Set(-top - 50f, 1f);
            biomeTimeScrollbar.Top.Set(top, 0f);
            biomeTimeScrollbar.Left.Set(-20f, 1f);
            biomeTimeList.SetScrollbar(biomeTimeScrollbar);
            panel.Append(biomeTimeScrollbar);

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

        private void UpdateTilesPanel()
        {
            UpdateTilePlacements();
            UpdateTileMinings();
        }

        private void UpdateTilePlacements()
        {
            tilePlacementList.Clear();

            // Add header row (optional)
            var header = new TileInteractionElement("Tile Name", "Placed Count");
            header.BackgroundColor = new Color(100, 100, 100, 100); // Optional: Different background for header
            tilePlacementList.Add(header);

            // Sort tiles by placed count in descending order
            var sortedTiles = TileInteractionProfiler.TilesPlaced
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in sortedTiles)
            {
                int tileType = entry.Key;
                int count = entry.Value;
                string tileName = GetTileName(tileType);

                var tileElement = new TileInteractionElement(tileName, count);
                tilePlacementList.Add(tileElement);
            }
        }

        private void UpdateTileMinings()
        {
            tileMiningList.Clear();

            // Add header row (optional)
            var header = new TileInteractionElement("Tile Name", "Mined Count");
            header.BackgroundColor = new Color(100, 100, 100, 100); // Optional: Different background for header
            tileMiningList.Add(header);

            // Sort tiles by mined count in descending order
            var sortedTiles = TileInteractionProfiler.TilesMined
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in sortedTiles)
            {
                int tileType = entry.Key;
                int count = entry.Value;
                string tileName = GetTileName(tileType);

                var tileElement = new TileInteractionElement(tileName, count);
                tileMiningList.Add(tileElement);
            }
        }

        private void UpdatePlayerDamageList()
        {
            playerDamageList.Clear();

            // Add header row (optional)
            var header = new TileInteractionElement("Player Name", "Damage Dealt");
            header.BackgroundColor = new Color(100, 100, 100, 100); // Optional: Different background for header
            playerDamageList.Add(header);

            // Sort players by damage dealt in descending order
            var sortedPlayers = PlayerDamageProfiler.DamageReceivedFromNPCs
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in sortedPlayers)
            {
                string playerName = entry.Key;
                int damageDealt = entry.Value;

                var damageElement = new TileInteractionElement(playerName, damageDealt);
                playerDamageList.Add(damageElement);
            }
        }

        private void UpdateNPCDamageList()
        {
            npcDamageList.Clear();

            // Add header row (optional)
            var header = new TileInteractionElement("NPC Name", "Damage Dealt");
            header.BackgroundColor = new Color(100, 100, 100, 100); // Optional: Different background for header
            npcDamageList.Add(header);

            // Sort NPCs by damage dealt in descending order
            var sortedNPCs = NPCDamageProfiler.DamageDealtToNPCs
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in sortedNPCs)
            {
                string npcName = entry.Key;
                int damageDealt = entry.Value;

                var damageElement = new TileInteractionElement(npcName, damageDealt);
                npcDamageList.Add(damageElement);
            }
        }

        private void UpdatePvPDamageList()
        {
            pvpDamageList.Clear();

            // Add header row (optional)
            var header = new TileInteractionElement("Entity Name", "PvP Damage");
            header.BackgroundColor = new Color(100, 100, 100, 100); // Optional: Different background for header
            pvpDamageList.Add(header);

            // Merge Projectile and Item PvP Damage
            var mergedPvPDamage = PvPDamageProfiler.MergePvPDamage();

            // Sort by PvP damage in descending order
            var sortedPvPDamage = mergedPvPDamage
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in sortedPvPDamage)
            {
                string entityName = entry.Key;
                int pvpDamage = entry.Value;

                var pvpElement = new TileInteractionElement(entityName, pvpDamage);
                pvpDamageList.Add(pvpElement);
            }
        }

        private void UpdateBiomeTimeList()
        {
            biomeTimeList.Clear();

            // Add header row (optional)
            var header = new TileInteractionElement("Biome Name", "Time Spent");
            header.BackgroundColor = new Color(100, 100, 100, 100); // Optional: Different background for header
            biomeTimeList.Add(header);

            // Sort biomes by time spent in descending order
            var sortedBiomes = BiomeTimeProfiler.BiomeTimeSpent
                .OrderByDescending(entry => entry.Value)
                .ThenBy(entry => entry.Key);

            foreach (var entry in sortedBiomes)
            {
                string biomeName = entry.Key;
                double timeSpent = entry.Value; // Assuming time is in seconds

                string formattedTime = FormatTime(timeSpent);

                var biomeElement = new TileInteractionElement(biomeName, formattedTime);
                biomeTimeList.Add(biomeElement);
            }
        }

        private string FormatTime(double totalSeconds)
        {
            int hours = (int)(totalSeconds / 3600);
            int minutes = (int)((totalSeconds % 3600) / 60);
            int seconds = (int)(totalSeconds % 60);

            return $"{hours}h {minutes}m {seconds}s";
        }

        private string GetTileName(int tileType)
        {
            var tile = TileLoader.GetTile(tileType);
            if (tile != null && !string.IsNullOrEmpty(tile.Name))
            {
                // Attempt to get the localized name
                string localizedName = Language.GetTextValue($"TileName.{tile.Name}");
                if (!string.IsNullOrEmpty(localizedName))
                    return localizedName;

                // Fallback to the tile's name property
                return tile.Name;
            }
            else
            {
                return "Unknown Tile";
            }
        }
    }
}
