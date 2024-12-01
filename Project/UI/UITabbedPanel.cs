using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;

namespace TerraLens.Project.UI
{
    public class UITabbedPanel : UIElement
    {
        private UIList tabButtonList;
        private UIScrollbar tabButtonScrollbar;
        private UIElement tabContentContainer;
        private Dictionary<string, UIElement> tabs;
        private UIElement currentContent;

        public UITabbedPanel()
        {
            // Initialize the dictionary to hold tab contents
            tabs = new Dictionary<string, UIElement>();

            // Initialize the tab buttons list
            tabButtonList = new UIList();
            tabButtonList.Width.Set(0f, 1f); // Full width minus scrollbar
            tabButtonList.Height.Set(0f, .88f);
            tabButtonList.ListPadding = 5f; // Spacing between tab buttons
            tabButtonList.Left.Set(-594f, 1f);
            Append(tabButtonList);

            // Initialize the scrollbar for tab buttons
            tabButtonScrollbar = new UIScrollbar();
            tabButtonScrollbar.SetView(100f, 1000f);
            tabButtonScrollbar.Height.Set(0f, .88f);
            tabButtonScrollbar.Top.Set(0f, 0f);
            tabButtonScrollbar.Left.Set(-440f, 1f); // Positioned to the right of the tab buttons
            tabButtonList.SetScrollbar(tabButtonScrollbar);
            Append(tabButtonScrollbar);

            // Tab content container on the right
            tabContentContainer = new UIElement();
            tabContentContainer.Width.Set(-160f, 1f); // Adjusted based on tab buttons width
            tabContentContainer.Height.Set(0f, 1f);
            tabContentContainer.Top.Set(0f, 0f);
            tabContentContainer.Left.Set(160f, 0f); // Positioned to the right of tab buttons
            Append(tabContentContainer);
        }
        public void AddTab(string tabName, System.Func<UIElement> contentProvider)
        {
            // Create tab button
            var tabButton = new UITabButton(tabName);
            tabButton.Width.Set(140f, 0f); // Slightly less than container width to accommodate scrollbar
            tabButton.Height.Set(40f, 0f);
            tabButton.MarginLeft = 10f; // Padding from the left edge

            // Handle left click to switch tabs
            tabButton.OnLeftClick += (evt, element) => SwitchTab(tabName);
            tabButtonList.Add(tabButton);

             // Store tab content
            var content = contentProvider();
            tabs[tabName] = content;

            // Display first tab by default
            if (tabs.Count == 1)
            {
                SwitchTab(tabName);
            }
        }

        private void SwitchTab(string tabName)
        {
            // Hide the current active tab content
            if (currentContent != null)
            {
                tabContentContainer.RemoveChild(currentContent);
            }

            // Show the selected tab content
            currentContent = tabs[tabName];
            tabContentContainer.Append(currentContent);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
